using UnityEngine;
using UnityEditor;
using System;
using System.Security;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using System.IO;

namespace ClientServerCommon
{
	public class AssetDescConfigEditor : AssetDescConfig
	{
		private AvatarConfig avatarConfig;
		private EquipmentConfig equipmentConfig;
		private SkillConfig skillConfig;
		private DanConfig danConfig;
		private BeastConfig beastConfig;
		private Dictionary<string, Dictionary<int, Texture2D>> borderTexDict = new Dictionary<string, Dictionary<int, Texture2D>>();

		private Dictionary<string, string> pathName;

		public AssetDescConfigEditor(AvatarConfig avatarConfig, EquipmentConfig equipmentConfig, SkillConfig skillConfig, DanConfig danconfig, BeastConfig beastConfig)
		{
			this.avatarConfig = avatarConfig;
			this.equipmentConfig = equipmentConfig;
			this.skillConfig = skillConfig;
			this.danConfig = danconfig;
			this.beastConfig = beastConfig;
		}

		[MenuItem("Product/Tools/Combine Icon Textures")]
		public static void Combine()
		{
			// Init ConfigDatabase.
			ConfigDatabase.Initialize(null, false, false);

			IFileLoader cfgParser = new FileLoaderFromWorkspace();

			string filePath = "Assets/WorkAssets/Texts/GameConfig/";
			if (!Directory.Exists(filePath))
			{
				Debug.LogError(filePath + "is not exist!");
				return;
			}

			var avatarConfig = new AvatarConfig();
			avatarConfig.LoadFromXml(cfgParser.LoadAsXML(filePath + "AvatarConfig.xml"));
			avatarConfig.ConstructLogicData(null, 0);

			var equipmentConfig = new EquipmentConfig();
			equipmentConfig.LoadFromXml(cfgParser.LoadAsXML(filePath + "EquipmentConfig.xml"));
			equipmentConfig.ConstructLogicData(null, 0);

			var skillConfig = new SkillConfig();
			skillConfig.LoadFromXml(cfgParser.LoadAsXML(filePath + "SkillConfig.xml"));
			skillConfig.ConstructLogicData(null, 0);

			var danConfig = new DanConfig();
			danConfig.LoadFromXml(cfgParser.LoadAsXML(filePath + "DanConfig.xml"));
			danConfig.ConstructLogicData(null, 0);

			var beastConfig = new BeastConfig();
			beastConfig.LoadFromXml(cfgParser.LoadAsXML(filePath + "BeastConfig.xml"));
			beastConfig.ConstructLogicData(null, 0);

			string fileName = filePath + "AssetDescConfig.xml";
			AssetDescConfigEditor assetDescConfig = new AssetDescConfigEditor(avatarConfig, equipmentConfig, skillConfig, danConfig, beastConfig);
			assetDescConfig.LoadFromXml(cfgParser.LoadAsXML(fileName));
			assetDescConfig.ConstructLogicData(null, 0);

			assetDescConfig.LoadQualityBorderTexture();
			assetDescConfig.CombineToTextures(fileName);
		}

		private void LoadQualityBorderTexture()
		{
			borderTexDict.Clear();

			foreach (var cb in combineSettings)
			{
				if (cb.qualitySettings.Count <= 0)
					continue;

				borderTexDict.Add(cb.outputIcon, new Dictionary<int, Texture2D>());

				foreach (var qs in cb.qualitySettings)
				{
					var tex = LoadOriginalTexture(qs.border);
					if (tex == null)
						continue;

					borderTexDict[cb.outputIcon][qs.qualityLevel] = tex;
				}
			}
		}

		private void CombineToTextures(string fileName)
		{
			Dictionary<string, List<AssetDesc>> combinedItems = new Dictionary<string, List<AssetDesc>>();

			foreach (var itemCfg in this.GetAssetDescs())
			{
				// Reset uv
				itemCfg.uv = Converter.ToKodRect(new Rect(0, 0, 1, 1));

				string outputIcon = itemCfg.outputIcon.ToLower();
				if (outputIcon == "")
					continue;

				if (combinedItems.ContainsKey(outputIcon) == false)
					combinedItems.Add(outputIcon, new List<AssetDesc>());

				combinedItems[outputIcon].Add(itemCfg);
			}

			if (pathName == null)
				pathName = new Dictionary<string, string>();
			else
				pathName.Clear();

			foreach (var kvp in combinedItems)
			{
				if (CombineTextures(kvp.Key, kvp.Value) == false)
				{
					Debug.LogError("Combine texture failed " + kvp.Key + " " + kvp.Value);
					return;
				}
			}

			SaveToXml(fileName);
		}

		private bool CombineTextures(string combinedTextureName, List<AssetDesc> assetDescList)
		{
			if (combinedTextureName == "")
				return false;

			CombineSetting combineSetting = this.GetCombineSetting(combinedTextureName);
			if (combineSetting == null)
			{
				Debug.LogError("Missing combine setting for output texture : " + combinedTextureName);
				return false;
			}

			// Combine Asset
			List<KeyValuePair<string, List<AssetDesc>>> combinedAssetDescs = new List<KeyValuePair<string, List<AssetDesc>>>();
			{
				Dictionary<string, List<AssetDesc>> assetDescsDict = new Dictionary<string, List<AssetDesc>>();
				foreach (var desc in assetDescList)
				{
					string inputIcon = desc.originalIcon;

					// Skip empty original icon
					if (inputIcon == "")
						continue;

					if (assetDescsDict.ContainsKey(inputIcon) == false)
						assetDescsDict.Add(inputIcon, new List<AssetDesc>());

					assetDescsDict[inputIcon].Add(desc);
				}

				foreach (var kvp in assetDescsDict)
				{
					combinedAssetDescs.Add(kvp);
				}
			}

			// No icon to be generated
			if (combinedAssetDescs.Count == 0)
				return true;
			if (combineSetting.combine)
			{
				// Calculate icon count per-texture
				int iconCountInOneTexture = 0;
				{
					// Get the Item AssetIDs by combinedTextureName
					Texture2D iconTexture = null;
					foreach (var cfg in assetDescList)
					{
						iconTexture = LoadOriginalTexture(cfg.originalIcon);
						if (iconTexture != null)
							break;
					}

					if (iconTexture == null)
						return false;

					// Max Small icon count in a big Texture
					iconCountInOneTexture = (combineSetting.maxCombinedTextureSize / iconTexture.width) * (combineSetting.maxCombinedTextureSize / iconTexture.height);
					if (iconCountInOneTexture == 0)
						return false;
				}

				var packingDescList = new List<int>();
				var packingTexList = new List<Texture2D>();

				for (int assetIndex = 0, texIndex = 0; assetIndex < combinedAssetDescs.Count; ++texIndex)
				{
					packingDescList.Clear();
					packingTexList.Clear();

					while (packingTexList.Count < iconCountInOneTexture && assetIndex < combinedAssetDescs.Count)
					{
						Texture2D tex = LoadProcessTexture(combinedAssetDescs[assetIndex].Key, combinedAssetDescs[assetIndex].Value[0].id);
						if (tex == null)
							return false;

						packingDescList.Add(assetIndex);
						packingTexList.Add(tex);

						++assetIndex;
					}

					// Bug : sometimes texture in the list is destroyed by system.
					foreach (var t in packingTexList)
					{
						if (t == null)
						{
							Debug.LogError("Internal error, please regenerate");
							return false;
						}
					}

					if (packingTexList.Count == 0)
						continue;

					var outputTextureName = string.Format("{0}_{1:D2}", combinedTextureName, texIndex);
					var combinedTexture = new Texture2D(combineSetting.maxCombinedTextureSize, combineSetting.maxCombinedTextureSize);
					var iconUVs = combinedTexture.PackTextures(packingTexList.ToArray(), 0, combineSetting.maxCombinedTextureSize, false);

					// Save to config
					Debug.Assert(iconUVs.Length == packingTexList.Count);
					for (int iconIndex = 0; iconIndex < iconUVs.Length; ++iconIndex)
					{
						foreach (var item in combinedAssetDescs[packingDescList[iconIndex]].Value)
						{
							item.icon = outputTextureName;
							item.uv = Converter.ToKodRect(iconUVs[iconIndex]);
						}
					}

					// Save combined texture
					string texturePathName = KodGames.PathUtility.Combine(this.combineOutputPath, outputTextureName) + ".png";
					byte[] bytes = combinedTexture.EncodeToPNG();
					KodGames.PathUtility.CreateDirectory(this.combineOutputPath);
					File.WriteAllBytes(texturePathName, bytes);

					// Make the texture Readable Flag true
					AssetDatabase.ImportAsset(texturePathName);
					TextureImporter textureImporter = AssetImporter.GetAtPath(texturePathName) as TextureImporter;
					if (textureImporter != null)
					{
						textureImporter.textureType = TextureImporterType.GUI;
						textureImporter.textureFormat = GetTextureFormat(combineSetting.textureFormat);
						textureImporter.isReadable = false;
						AssetDatabase.ImportAsset(texturePathName);
					}
				}
			}
			else
			{
				char[] spliter = { '/' };
				for (int assetIndex = 0; assetIndex < combinedAssetDescs.Count; ++assetIndex)
				{
					string[] names = combinedAssetDescs[assetIndex].Key.Split(spliter);
					if (names == null || names.Length <= 0)
						Debug.LogError("split error : " + combinedAssetDescs[assetIndex].Key);

					string outputTextureName = names[names.Length - 1];

					foreach (var kv in pathName)
						if (kv.Value == outputTextureName && !kv.Key.Equals(combinedAssetDescs[assetIndex].Key, StringComparison.OrdinalIgnoreCase))
						{
							Debug.LogError("already exists texture: " + outputTextureName);
							return false;
						}

					if (pathName.ContainsKey(combinedAssetDescs[assetIndex].Key) == false)
						pathName.Add(combinedAssetDescs[assetIndex].Key, outputTextureName);

					Texture2D packingTex = LoadProcessTexture(combinedAssetDescs[assetIndex].Key, combinedAssetDescs[assetIndex].Value[0].id);
					if (packingTex == null)
						return false;

					// Save to config
					foreach (var item in combinedAssetDescs[assetIndex].Value)
					{
						item.icon = outputTextureName;
						item.uv = Converter.ToKodRect(new Rect(0, 0, 1, 1));
					}

					// Save combined texture
					string texturePathName = KodGames.PathUtility.Combine(this.combineOutputPath, outputTextureName) + ".png";
					byte[] bytes = packingTex.EncodeToPNG();
					KodGames.PathUtility.CreateDirectory(this.combineOutputPath);
					File.WriteAllBytes(texturePathName, bytes);

					// Make the texture Readable Flag true
					AssetDatabase.ImportAsset(texturePathName);
					TextureImporter textureImporter = AssetImporter.GetAtPath(texturePathName) as TextureImporter;
					if (textureImporter != null)
					{
						textureImporter.textureType = TextureImporterType.GUI;
						textureImporter.textureFormat = GetTextureFormat(combineSetting.textureFormat);
						textureImporter.isReadable = false;
						AssetDatabase.ImportAsset(texturePathName);
					}
				}
			}

			return true;
		}

		//Get Texture by Item.AssetID
		private Texture2D LoadOriginalTexture(string originalIcon)
		{
			if (string.IsNullOrEmpty(originalIcon))
				return null;

			string iconPath = KodGames.PathUtility.FindFileWithExtension(Path.Combine(this.combineInputPath, originalIcon));
			if (iconPath == null)
				return null;

			// Make the texture Readable Flag true
			TextureImporter textureImporter = AssetImporter.GetAtPath(iconPath) as TextureImporter;
			if (textureImporter == null)
			{
				Debug.LogError("Miss icon : " + iconPath);
				return null;
			}

			textureImporter.textureType = TextureImporterType.GUI;
			textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
			textureImporter.isReadable = true;

			AssetDatabase.ImportAsset(iconPath);

			return AssetDatabase.LoadAssetAtPath(iconPath, typeof(Texture2D)) as Texture2D;
		}

		private Texture2D LoadProcessTexture(string originalIcon, int assetId)
		{
			var tex = LoadOriginalTexture(originalIcon);
			if (tex == null)
				return null;

			// Process quality style icon
			int qualityLevel = -1;
			int assetType = IDSeg.ToAssetType(assetId);
			string assetName = IDSeg._AssetType.GetNameByType(assetType).ToLower();
			switch (assetType)
			{
				case IDSeg._AssetType.Avatar:
					{
						var cfg = avatarConfig.GetAvatarById(assetId);
						if (cfg != null)
							qualityLevel = cfg.qualityLevel;
						break;
					}

				case IDSeg._AssetType.Equipment:
					{
						var cfg = equipmentConfig.GetEquipmentById(assetId);
						if (cfg != null)
							qualityLevel = cfg.qualityLevel;
						break;
					}

				case IDSeg._AssetType.CombatTurn:
					{
						var cfg = skillConfig.GetSkillById(assetId);
						if (cfg != null)
							qualityLevel = cfg.qualityLevel;
						break;
					}

				case IDSeg._AssetType.Dan:
					{
						var cfg = danConfig.GetDanIconByIconId(assetId);
						if (cfg != null)
							qualityLevel = cfg.Breakthought;
						break;
					}

				case IDSeg._AssetType.Beast:
					{
						var cfg = beastConfig.GetBeastIconByIconId(assetId);
						if (cfg != null)
							qualityLevel = cfg.Breakthought;
						break;
					}

				case IDSeg._AssetType.Illusion:
					qualityLevel = 1;
					break;
			}

			// Load mask texture
			if (borderTexDict.ContainsKey(assetName) &&
				borderTexDict[assetName].ContainsKey(qualityLevel))
			{
				var borderTex = borderTexDict[assetName][qualityLevel];
				Texture2D newTex = new Texture2D(borderTex.width, borderTex.height, TextureFormat.ARGB32, false);
				int paddingX = (borderTex.width - tex.width) / 2;
				int paddingY = (borderTex.height - tex.height) / 2;
				for (int y = 0; y < borderTex.height; ++y)
					for (int x = 0; x < borderTex.width; ++x)
					{
						if ((x <= paddingX || (x >= borderTex.width - paddingX)) &&
						   (y <= paddingY || (y >= borderTex.height - paddingY)))
							newTex.SetPixel(x, y, borderTex.GetPixel(x, y));
						else
							newTex.SetPixel(x, y, BlendColor(tex.GetPixel(x - paddingX, y - paddingY), borderTex.GetPixel(x, y)));
					}

				newTex.Apply();

				return newTex;
			}

			return tex;
		}

		private TextureImporterFormat GetTextureFormat(string formatStr)
		{
			TextureImporterFormat textureFormat = TextureImporterFormat.AutomaticTruecolor;

			try
			{
				textureFormat = (TextureImporterFormat)Enum.Parse(typeof(TextureImporterFormat), formatStr);
			}
			catch (System.Exception e)
			{
				textureFormat = TextureImporterFormat.AutomaticTruecolor;
				return textureFormat;
			}

			return textureFormat;
		}

		private Color BlendColor(Color src, Color dst)
		{
			//return dst;
			return new Color(src.r * src.a + dst.r * (1 - src.a), src.g * src.a + dst.g * (1 - src.a), src.b * src.a + dst.b * (1 - src.a), src.a * src.a + dst.a * (1 - src.a));
		}

		private void SaveToXml(string fileName)
		{
			// Create XML
			XmlDocument cfgDoc = new XmlDocument();
			cfgDoc.AppendChild(cfgDoc.CreateXmlDeclaration("1.0", "utf-8", null));

			// Load to memory
			TextAsset textAsset = UnityEditor.AssetDatabase.LoadAssetAtPath(fileName, typeof(TextAsset)) as TextAsset;

			// Remove BOM flag
			cfgDoc.LoadXml(StrParser.GetTextWithoutBOM(textAsset.bytes));

			// Modify XML
			XmlNodeList nodeList = cfgDoc.GetElementsByTagName("AssetDesc");
			foreach (XmlNode itemNode in nodeList)
			{
				XmlElement itemElement = itemNode as XmlElement;
				if (itemElement == null)
					continue;

				int id = StrParser.ParseHexInt(itemElement.GetAttribute("Id"), IDSeg.InvalidId);
				AssetDesc assetDescCfg = GetAssetDescById(id);
				if (assetDescCfg != null)
				{
					itemElement.SetAttribute("Icon", assetDescCfg.icon);
					itemElement.SetAttribute("UV", string.Format("{0},{1},{2},{3}", assetDescCfg.uv.x, assetDescCfg.uv.y, assetDescCfg.uv.xMax, assetDescCfg.uv.yMax));
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

			// Refresh Asset Database
			AssetDatabase.Refresh();
			Debug.Log("Combine Icon success");
		}
	}
}
