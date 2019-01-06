using UnityEngine;
using System.Collections;
using System.Security;
using ClientServerCommon;
using System.IO;
using Mono.Xml;

public class FileLoaderFromTextAsset : IFileLoader
{
	private TextAsset textAsset;

	public FileLoaderFromTextAsset(TextAsset textAsset)
	{
		this.textAsset = textAsset;
	}

	private TextAsset LoadTextAsset(string filePath)
	{
		return textAsset;
	}

	public SecurityElement LoadAsXML(string filePath)
	{
		if (textAsset == null)
			return null;

		SecurityParser xmlParser = new SecurityParser();
		xmlParser.LoadXml(textAsset.text);

		return xmlParser.ToXml();
	}

	public Stream LoadAsSteam(string filePath)
	{
		if (textAsset == null)
			return null;

		return new MemoryStream(textAsset.bytes);
	}
}
