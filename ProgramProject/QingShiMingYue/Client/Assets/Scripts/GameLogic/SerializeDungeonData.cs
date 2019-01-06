using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class MyVector3
{
	[System.Reflection.Obfuscation(Exclude = true)]
	public float x;
	[System.Reflection.Obfuscation(Exclude = true)]
	public float y;
	[System.Reflection.Obfuscation(Exclude = true)]
	public float z;

	public MyVector3()
	{
		this.x = 0f;
		this.y = 0f;
		this.z = -0.001f;
	}

	public MyVector3(Vector3 v)
	{
		this.x = v.x;
		this.y = v.y;
		this.z = v.z;
	}

	public MyVector3(MyVector3 v)
	{
		this.x = v.x;
		this.y = v.y;
		this.z = v.z;
	}

	public Vector3 ConvertToVector3()
	{
		return new Vector3(x, y, z);
	}

	public override string ToString()
	{
		return "x:" + x + " y:" + y + " z:" + z;
	}
}

[Serializable]
public class SerializeDungeonData
{
	[System.Reflection.Obfuscation(Exclude = true)]
	public string dungeonkey;
	[System.Reflection.Obfuscation(Exclude = true)]
	public List<MyVector3> locations;
	[System.Reflection.Obfuscation(Exclude = true)]
	public List<MyVector3> shopLocations;

	public SerializeDungeonData() { }

	public SerializeDungeonData(string dungeonkey)
	{
		this.dungeonkey = dungeonkey;
		this.locations = new List<MyVector3>();
		this.shopLocations = new List<MyVector3>();
	}
}

public class SerializeDungoenTools
{
#if UNITY_EDITOR
	private static string filePath = "Assets/Resources/Texts/LocalData/dungeonLocation.bytes";

	public static void InitSerializeFile(List<string> dungeonLocationNames)
	{
		if (!File.Exists(filePath))
		{
			Debug.Log("file not exist.");

			List<SerializeDungeonData> dungeonDatas = new List<SerializeDungeonData>();

			for (int index = 0; index < dungeonLocationNames.Count; index++)
			{
				SerializeDungeonData serializeData = new SerializeDungeonData(dungeonLocationNames[index]);
				dungeonDatas.Add(serializeData);
			}

			SerializeDataToFile(dungeonDatas);
		}
	}

	public static void SerializeDataToFile(List<SerializeDungeonData> dungeonLocationDatas)
	{
		if (File.Exists(filePath))
			File.Delete(filePath);

		Stream fileStream = null;
		BinaryFormatter binFormat = null;

		try
		{
			fileStream = new FileStream(KodGames.PathUtility.UnifyPath(filePath), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
			binFormat = new BinaryFormatter();
			binFormat.Serialize(fileStream, dungeonLocationDatas);
		}
		catch (System.Exception e)
		{
			Debug.Log(e.Message);
		}
		finally
		{
			if (fileStream != null)
			{
				fileStream.Close();
				fileStream = null;
			}

			if (binFormat != null)
				binFormat = null;
		}
	}
#endif

	public static List<SerializeDungeonData> DeserializeFromFile()
	{
		TextAsset file = null;
		if (Application.isPlaying)
			file = ResourceManager.Instance.LoadAsset<TextAsset>("Texts/LocalData/dungeonLocation");
		else
			file = Resources.Load("Texts/LocalData/dungeonLocation") as TextAsset;

		if (file == null)
		{
			Debug.Log("Load DungeonLocation.bytes error.");
			return null;
		}

		var fileStream = new MemoryStream(file.bytes);
		var serializeData = (List<SerializeDungeonData>)new BinaryFormatter().Deserialize(fileStream);
		if (serializeData == null)
			Debug.LogError("Deserialize DungeonLocation.bytes error.");

		if (fileStream != null)
		{
			fileStream.Close();
			fileStream = null;
		}

		if (file != null)
			file = null;

		return serializeData;
	}

	public static SerializeDungeonData GetSerializeDataFromFile(string dungeonLocationName)
	{
		List<SerializeDungeonData> serializeDatas = DeserializeFromFile();

		if (serializeDatas == null)
			return null;

		foreach (SerializeDungeonData data in serializeDatas)
		{
			if (data.dungeonkey.Equals(dungeonLocationName))
				return data;
		}

		return null;
	}
}