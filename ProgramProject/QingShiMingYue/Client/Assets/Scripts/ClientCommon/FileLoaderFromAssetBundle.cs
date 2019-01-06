using UnityEngine;
using System.Collections;
using System.Security;
using ClientServerCommon;
using System.IO;
using Mono.Xml;

public class FileLoaderFromAssetBundle : IFileLoader
{
	private AssetBundle assetBundle;

	public FileLoaderFromAssetBundle(AssetBundle assetBundle)
	{
		this.assetBundle = assetBundle;
	}

	private TextAsset LoadTextAsset(string filePath)
	{
		if (assetBundle == null)
			return null;

		TextAsset textAsset = assetBundle.Load(filePath, typeof(TextAsset)) as TextAsset;
		if (textAsset == null)
		{
			Debug.LogError("Load asset failed : " + filePath);
			return null;
		}

		return textAsset;
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
