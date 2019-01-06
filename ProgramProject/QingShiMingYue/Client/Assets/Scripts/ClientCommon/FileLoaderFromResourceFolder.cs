using UnityEngine;
using System.Collections;
using System.Security;
using Mono.Xml;
using System.IO;
using ClientServerCommon;
using KodGames;

public class FileLoaderFromResourceFolder : IFileLoader
{
	public SecurityElement LoadAsXML(string filePath)
	{
		TextAsset textAsset = ResourcesWrapper.Load<TextAsset>(filePath);
		if (textAsset == null)
			return null;

		SecurityParser xmlParser = new SecurityParser();
		xmlParser.LoadXml(textAsset.text);
		
		return xmlParser.ToXml();
	}

	public Stream LoadAsSteam(string filePath)
	{
		TextAsset textAsset = ResourcesWrapper.Load<TextAsset>(filePath);
		if (textAsset == null)
			return null;

		return new MemoryStream(textAsset.bytes);
	}
}
