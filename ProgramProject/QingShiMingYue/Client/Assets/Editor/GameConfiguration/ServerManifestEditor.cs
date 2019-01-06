﻿using System;
using System.Collections.Generic;
using System.Xml;
using ClientServerCommon;
using System.IO;
using System.Text;

public class ServerManifestEditor
{
	public static void ModifyConfig(string fileName, string gameConfigFilePath, int fileSize, int uncompressedFileSize)
	{
		// Create XML
		XmlDocument cfgDoc = new XmlDocument();
		cfgDoc.AppendChild(cfgDoc.CreateXmlDeclaration("1.0", "utf-8", null));

		// Load to memory
		byte[] fileData = File.ReadAllBytes(fileName);

		// Remove BOM flag
		cfgDoc.LoadXml(StrParser.GetTextWithoutBOM(fileData));

		// Modify XML
		XmlNode rootNode = cfgDoc.SelectSingleNode("Config");

		XmlElement rootElement = rootNode as XmlElement;
		if (rootElement != null)
		{
			if (gameConfigFilePath != null)
			{
				rootElement.SetAttribute("GameConfigName", Path.GetFileName(gameConfigFilePath));
				rootElement.SetAttribute("GameConfigSize", fileSize.ToString());
				rootElement.SetAttribute("GameConfigUncompressedSize", uncompressedFileSize.ToString());
			}
		}

		// Save
		XmlWriterSettings settings = new XmlWriterSettings();
		settings.Indent = true;

		// Encode the temp XML with UTF-8 without BOM flag
		settings.Encoding = new UTF8Encoding(false);
		settings.NewLineChars = Environment.NewLine;

		XmlWriter writer = XmlWriter.Create(fileName, settings);

		// Save the temp XML
		cfgDoc.Save(writer);
		writer.Close();
	}
}
