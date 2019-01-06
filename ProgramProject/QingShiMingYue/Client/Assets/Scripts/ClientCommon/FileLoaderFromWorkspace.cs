using UnityEngine;
using System.Collections;
using System.Security;
using Mono.Xml;
using System.IO;
using ClientServerCommon;
using KodGames;

public class FileLoaderFromWorkspace : IFileLoader
{
	private TextAsset LoadTextAsset(string filePath)
	{
#if UNITY_EDITOR
		TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogWarning("Load asset failed : " + filePath);
			return null;
		}

		return textAsset;
#else
		return null;
#endif
	}

	public SecurityElement LoadAsXML(string filePath)
	{
		TextAsset textAsset = LoadTextAsset(filePath);
		if (textAsset == null)
			return null;

		SecurityParser xmlParser = new SecurityParser();
		xmlParser.LoadXml(textAsset.text);
		
		return xmlParser.ToXml();
	}

	public Stream LoadAsSteam(string filePath)
	{
		TextAsset textAsset = LoadTextAsset(filePath);
		if (textAsset == null)
			return null;

		return new MemoryStream(textAsset.bytes);
	}
}
