//#define ENABLE_BUILD_LOG
#define ENABLE_DELAY_CONFIG
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.IO;
using ClientServerCommon;
using System.Text;
using System.Xml;
using KodGames;
using ICSharpCode.SharpZipLib.BZip2;

namespace KodGames.WorkFlow
{
	/// <summary>
	/// Config for building game
	/// </summary>
	class BuildConfig
	{
		public bool loaded = false;

		/// <summary>
		/// Setting for a language
		/// </summary>
		public class LanguageSetting
		{
			public struct GameConfig
			{
				public string name;
				public string assetName;
				public string filePath;
				public bool valid;
			}

			/// <summary>
			/// The ID of language
			/// </summary>
			public string languageID = "";

			/// <summary>
			/// File contains texts to be localized
			/// </summary>
			public string textFile = "";

			/// <summary>
			/// File needs to be localized
			/// </summary>
			public Dictionary<string, string> fileCopySetting = new Dictionary<string, string>();

			/// <summary>
			/// The scene user interface control setting. Setup control size for different language
			/// </summary>
			public Dictionary<string, Dictionary<string, Vector2>> sceneUIControlSetting = new Dictionary<string, Dictionary<string, Vector2>>();
			public List<GameConfig> gameConfigFiles = new List<GameConfig>();
			public string clientManifestFile = "";
		}

		public class LevelSetting
		{
			public string levelName = "";
			public string targetPath = "";
		}

		public class FileOperationSet
		{
			public enum _PhaseType
			{
				BeforeBuildPlayer,
				AfterBuildPlayer,
			}

			public string name = "";
			public _PhaseType phaseType;
			public List<Pair<string, Pair<string, string>>> operations = new List<Pair<string, Pair<string, string>>>();
		}

		/// <summary>
		/// UI level to be localized
		/// </summary>
		public LevelSetting uiEditorlevelSetting;
		//public List<LevelSetting> levelSettingList = new List<LevelSetting>();

		public string targetGameConfigFile = "";
		public string emptyConfigFile = "";

		/// <summary>
		/// Language list
		/// </summary>
		public Dictionary<string, LanguageSetting> languageSettingDict = new Dictionary<string, LanguageSetting>();

		/// <summary>
		/// The levels for Unity BuildPipline.
		/// </summary>
		public List<string> unityLevelList = new List<string>();

		/// <summary>
		/// The files to copy after Unity Build.
		/// </summary>
		public List<FileOperationSet> fileOperationSets = new List<FileOperationSet>();

		public List<string> buildGameAssetIgnorePathList = new List<string>();

		public List<string> buildGameAssetKeepNamePathList = new List<string>();

		public List<string> buildGameAssetDeleteUIForReSave = new List<string>();

		public static BuildConfig LoadBuildConfig(string buildConfigFile)
		{
			string fileName = PathUtility.UnifyPath(buildConfigFile);
			TextAsset xmlFile = ResourcesWrapper.LoadAssetAtPath<TextAsset>(fileName);
			if (xmlFile == null)
			{
				Debug.LogError("Build config not found : " + fileName);
				return null;
			}

			SecurityParser xmlParser = new SecurityParser();
			try
			{
				xmlParser.LoadXml(xmlFile.text);
			}
			catch (System.Exception ex)
			{
				Debug.LogError("Parse build config failed : " + ex.Message);
				return null;
			}

			BuildConfig buildConfig = new BuildConfig();
			buildConfig.Load(xmlParser.ToXml());

			return buildConfig;
		}

		/// <summary>
		/// Load setting from XML
		/// </summary>
		public bool Load(SecurityElement element)
		{
			if (element.Tag != "BuildConfig")
				return false;

			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					switch (subElem.Tag)
					{
						case "LanguageSet": LoadLanguageSet(subElem); break;
						case "UIEditorLevel": LoadUIEditorLevel(subElem); break;
						case "CopyFileSet": LoadCopyFileSet(subElem); break;
						case "UnityLevelSet": LoadUnityLevelSet(subElem); break;
						case "ProjectFileOperationSet": LoadProjectFileOperationSet(subElem); break;
						case "GameConfigBuildSet": LoadGameConfigFileBuildSetting(subElem); break;
						case "GameAssetBuildSet": LoadGameAssetFileBuildSetting(subElem); break;
					}
				}
			}

			return true;
		}

		private void LoadLanguageSet(SecurityElement element)
		{
			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					if (subElem.Tag != "Language")
						continue;

					LanguageSetting setting = new LanguageSetting();
					setting.languageID = StrParser.ParseStr(subElem.Attribute("Name"), setting.languageID);
					setting.textFile = StrParser.ParseStr(subElem.Attribute("TextFile"), setting.textFile);

					if (setting.languageID == "")
						continue;

					if (languageSettingDict.ContainsKey(setting.languageID))
					{
						Debug.LogError("Duplicated language ID : " + setting.languageID);
						continue;
					}

					languageSettingDict.Add(setting.languageID, setting);
				}
			}
		}

		private void LoadUIEditorLevel(SecurityElement element)
		{
			LevelSetting setting = new LevelSetting();
			setting.levelName = StrParser.ParseStr(element.Attribute("Name"), setting.levelName);
			setting.targetPath = StrParser.ParseStr(element.Attribute("TargetPath"), setting.targetPath);

			if (setting.levelName == "")
				return;

			uiEditorlevelSetting = setting;

			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					switch (subElem.Tag)
					{
						case "UIControlSetting":
							string controlName = StrParser.ParseStr(subElem.Attribute("Name"), "");
							LoadSceneUISetting(subElem, setting.levelName, controlName);
							break;
						case "UIDeleteForReSave":
							LoadDeleteUIForReSave(subElem);
							break;
					}
				}
			}
		}

		public static readonly char[] splitter = { ',' };

		public static Vector2 ParseVec2(string str)
		{
			if (str == null)
				return Vector2.zero;

			string[] vecs = str.Split(splitter);
			if (vecs.Length < 2)
				return Vector2.zero;

			return new Vector2(float.Parse(vecs[0]), float.Parse(vecs[1]));
		}

		private void LoadDeleteUIForReSave(SecurityElement element)
		{
			var assetUIName = StrParser.ParseStr(element.Attribute("Name"), "");
			if (buildGameAssetDeleteUIForReSave.Contains(assetUIName) == false)
				buildGameAssetDeleteUIForReSave.Add(assetUIName);
		}

		private void LoadSceneUISetting(SecurityElement element, string scene, string controlName)
		{
			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					if (subElem.Tag != "Setting")
						continue;

					string language = StrParser.ParseStr(subElem.Attribute("Language"), "");
					Vector2 spriteSize = ParseVec2(subElem.Attribute("SpriteSize"));
					if (languageSettingDict.ContainsKey(language) == false)
						continue;

					LanguageSetting setting = languageSettingDict[language];
					if (setting.sceneUIControlSetting.ContainsKey(scene) == false)
						setting.sceneUIControlSetting.Add(scene, new Dictionary<string, Vector2>());

					setting.sceneUIControlSetting[scene].Add(controlName, spriteSize);
				}
			}
		}

		private void LoadCopyFileSet(SecurityElement element)
		{
			string targetFileName = StrParser.ParseStr(element.Attribute("TargetFile"), "");

			if (element.Children != null)
			{
				foreach (SecurityElement subElem in element.Children)
				{
					if (subElem.Tag != "CopyFile")
						continue;

					string languageID = StrParser.ParseStr(subElem.Attribute("Language"), "");
					string fileName = StrParser.ParseStr(subElem.Attribute("File"), "");

					if (languageID == "" || fileName == "")
						continue;

					if (languageSettingDict.ContainsKey(languageID) == false)
					{
						Debug.LogError("Invalid language ID in copy file set : " + languageID);
						continue;
					}

					if (languageSettingDict[languageID].fileCopySetting.ContainsKey(targetFileName))
					{
						Debug.LogError("Duplicated copy file setting : " + languageID + " " + targetFileName);
						continue;
					}

					languageSettingDict[languageID].fileCopySetting.Add(targetFileName, fileName);
				}
			}
		}

		private void LoadUnityLevelSet(SecurityElement element)
		{
			if (element.Children != null)
				foreach (SecurityElement subElem in element.Children)
					if (subElem.Tag == "Level")
						unityLevelList.Add(StrParser.ParseStr(subElem.Attribute("Name"), ""));
		}

		private void LoadProjectFileOperationSet(SecurityElement element)
		{
			if (element.Children != null)
			{
				foreach (SecurityElement platformElem in element.Children)
				{
					if (platformElem.Tag != "OperationSet")
						continue;

					var operationSet = new FileOperationSet();
					operationSet.name = StrParser.ParseStr(platformElem.Attribute("Name").ToLower(), "");
					operationSet.phaseType = (FileOperationSet._PhaseType)System.Enum.Parse(typeof(FileOperationSet._PhaseType), platformElem.Attribute("Phase"));

					if (platformElem.Children != null)
					{
						foreach (SecurityElement subElem in platformElem.Children)
						{
							var operation = default(Pair<string, Pair<string, string>>);
							operation.first = StrParser.ParseStr(subElem.Tag, "");
							operation.second.first = StrParser.ParseStr(subElem.Attribute("From"), "");
							operation.second.second = StrParser.ParseStr(subElem.Attribute("To"), "");
							operationSet.operations.Add(operation);
						}
					}

					this.fileOperationSets.Add(operationSet);
				}
			}
		}

		private void LoadGameConfigFileBuildSetting(SecurityElement element)
		{
			targetGameConfigFile = StrParser.ParseStr(element.Attribute("TargetFile"), "");
			emptyConfigFile = StrParser.ParseStr(element.Attribute("EmptyConfigFile"), "");

			if (element.Children != null)
			{
				foreach (SecurityElement langElem in element.Children)
				{
					if (langElem.Tag != "Language")
						continue;

					string languageID = StrParser.ParseStr(langElem.Attribute("Name"), "");

					LanguageSetting languageSetting = languageSettingDict[languageID];
					if (languageSetting == null)
					{
						Debug.LogError("Invalid language " + languageID);
						continue;
					}

					if (langElem.Children != null)
					{
						foreach (SecurityElement subElem in langElem.Children)
						{
							LanguageSetting.GameConfig gameConfig;
							gameConfig.name = StrParser.ParseStr(subElem.Attribute("Name"), "");
							gameConfig.assetName = StrParser.ParseStr(subElem.Attribute("AssetName"), "");
							gameConfig.filePath = StrParser.ParseStr(subElem.Attribute("File"), "");
							gameConfig.valid = StrParser.ParseBool(subElem.Attribute("Valid"), true);

							languageSetting.gameConfigFiles.Add(gameConfig);
						}
					}
				}
			}
		}

		private void LoadGameAssetFileBuildSetting(SecurityElement element)
		{
			if (element.Children != null)
			{
				foreach (SecurityElement langElem in element.Children)
				{
					switch (langElem.Tag)
					{
						case "IgnorePath":
							string path = PathUtility.UnifyPath(StrParser.ParseStr(langElem.Attribute("Path"), ""));
							if (path != "")
								buildGameAssetIgnorePathList.Add(path);

							break;

						case "KeepAssetNamePaht":
							string keepPath = PathUtility.UnifyPath(StrParser.ParseStr(langElem.Attribute("Path"), ""));
							if (string.IsNullOrEmpty(keepPath) == false && buildGameAssetKeepNamePathList.Contains(keepPath) == false)
								buildGameAssetKeepNamePathList.Add(keepPath);
							break;

						case "ClientManifest":
							string languageID = StrParser.ParseStr(langElem.Attribute("Language"), "");
							LanguageSetting languageSetting = languageSettingDict[languageID];
							if (languageSetting != null)
								languageSetting.clientManifestFile = StrParser.ParseStr(langElem.Attribute("File"), "");
							else
								Debug.LogError("Invalid language " + languageID);

							break;
					}
				}
			}
		}
	}

	public class BuildTools : EditorWindow
	{
		private static string sIntermediateFolder = "Assets/Intermediate";
		private static string sBuildConfigFile = "Assets/WorkAssets/Texts/Editor/BuildConfig.xml";
		private BuildConfig buildConfig = new BuildConfig();
		private string[] languageIDList = new string[0];
		private int selectedLanguageIndex = 0;

		public static BuildTools buildTools;
		[MenuItem("Product/Build Product &v")]
		public static void ShowPanel()
		{
			buildTools = GetWindow(typeof(BuildTools), true, "Building Tool") as BuildTools;
			buildTools.Initialize();
		}

		[MenuItem("Product/Build/quick &b")]
		public static void BuildProject()
		{

			if (buildTools != null)
			{
				buildTools.BuildButton();
			}
			else
			{
				Debug.LogError("Build编辑器未实例化....");
			}

		}
		[MenuItem("Product/Build/quick &c")]
		public static void CloseBuild()
		{

			if (buildTools != null)
			{
				buildTools.Close();
			}
			else
			{
				Debug.LogError("Build编辑器未实例化....");
			}

		}
		private void Initialize()
		{
			// Load config
			buildConfig = BuildConfig.LoadBuildConfig(sBuildConfigFile);
			if (buildConfig == null)
				return;


			//BuildConfig.
			// Init language list
			languageIDList = new string[buildConfig.languageSettingDict.Count];

			int index = 0;
			foreach (var kvp in buildConfig.languageSettingDict)
			{
				languageIDList[index] = kvp.Value.languageID;
				++index;
			}

			selectedLanguageIndex = languageIDList.Length != 0 ? 0 : -1;

			// Load saved setting
			LoadSetting();
		}

		private void LoadSetting()
		{
			string languageID = PlayerPrefs.GetString("BuildProduct.LanguageID", "");
			for (int i = 0; i < languageIDList.Length; ++i)
			{
				if (System.String.Compare(languageIDList[i], languageID, true) == 0)
				{
					selectedLanguageIndex = i;
					break;
				}
			}
		}

		private void SaveSetting()
		{
			PlayerPrefs.SetString("BuildProduct.LanguageID", selectedLanguageIndex >= 0 ? languageIDList[selectedLanguageIndex] : "");
			PlayerPrefs.SetString("BuildProduct.LanguageFile", selectedLanguageIndex >= 0 ? buildConfig.languageSettingDict[languageIDList[selectedLanguageIndex]].textFile : "");
			PlayerPrefs.Save();
		}

		/// <summary>
		/// Copy language text file to target folder specified in build config.
		/// </summary>
		private void CopyLocalizedFilesAndBuildConfig()
		{
			if (selectedLanguageIndex < 0 || selectedLanguageIndex >= languageIDList.Length)
				return;

			CopyLocalizedFiles(buildConfig, languageIDList[selectedLanguageIndex]);
			BuildGameAsset(buildConfig, languageIDList[selectedLanguageIndex], false, false, false, 0, 0);
		}

		private static void ChangeSpriteSetting(BuildConfig buildConfig, string lanuageId)
		{
			//EditorApplication.OpenScene(buildConfig.uiEditorlevelSetting.levelName);

			BuildConfig.LanguageSetting languageSetting = buildConfig.languageSettingDict[lanuageId];
			// Get UI root
			GameObject root = GameObject.Find("UIContainer");
			if (root == null)
			{
				Debug.LogError("Not found UIContainer in this scene.");
				return;
			}

			//////////////////////////////////////////////////////////////////////////
			// Change sprite setting
			if (languageSetting.sceneUIControlSetting.ContainsKey(EditorApplication.currentScene))
			{
				Debug.Log("Localizing control setting...");

				Dictionary<string, Vector2> sceneUIControlSetting = languageSetting.sceneUIControlSetting[EditorApplication.currentScene];

				foreach (var kvp in sceneUIControlSetting)
				{
					SpriteRoot control = ObjectUtility.GetComponentInChild<SpriteRoot>(root, kvp.Key);
					if (control == null)
						Debug.LogError("Can not find control : " + kvp.Key);

					Debug.Log(control.gameObject.name);
					control.SetSize(kvp.Value.x, kvp.Value.y);
				}
			}
		}

		private void Build()
		{
			if (selectedLanguageIndex < 0 || selectedLanguageIndex >= languageIDList.Length)
				return;

			string languageId = languageIDList[selectedLanguageIndex];

			// Copy language file to target folder
			CopyLocalizedFiles(buildConfig, languageId);

			// Build UI
			BuildGameUI(buildConfig, EditorUserBuildSettings.activeBuildTarget, languageId);

			// Build game asset including game config
			BuildGameAsset(buildConfig, languageId, false, false, false, 0, 0);
		}
		internal void BuildButton()
		{
			// Save current scene
			string currentScene = EditorApplication.currentScene;
			Build();
			SaveSetting();
			System.GC.Collect();

			// Reopen previous scene
			EditorApplication.OpenScene(currentScene);
		}

		private void OnGUI()
		{
			string errorMsg = "";
			if (Application.isPlaying)
			{
				errorMsg = "Can not use this tool when playing";
			}
			else
			{
				EditorGUILayout.BeginHorizontal();
				selectedLanguageIndex = EditorGUILayout.Popup("Language", selectedLanguageIndex, languageIDList);
				if (GUILayout.Button("Set Language") && errorMsg != null)
				{
					SaveSetting();
				}
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Error Message", errorMsg);

			EditorGUILayout.Space();
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Localize File") && errorMsg != null)
			{
				CopyLocalizedFilesAndBuildConfig();
				SaveSetting();
			}

			//if (GUILayout.Button("Change Sprite Size") && errorMsg != null)
			//{
			//    ChangeSpriteSetting(buildConfig, languageIDList[selectedLanguageIndex]);
			//    SaveSetting();
			//}

			if (GUILayout.Button("Build Game Config") && errorMsg != null)
			{
				CopyLocalizedFilesAndBuildConfig();
				SaveSetting();
			}

			if (GUILayout.Button("Build") && errorMsg != null)
			{
				// Save current scene
				/*string currentScene = EditorApplication.currentScene;
				Build();
				SaveSetting();
				System.GC.Collect();

				// Reopen previous scene
				EditorApplication.OpenScene(currentScene);
                 */
				BuildButton();
			}

			EditorGUILayout.EndHorizontal();
		}



		private static void CopyLocalizedFiles(BuildConfig buildConfig, string lanuageId)
		{
			Debug.Log("------ Localizing Files ------");

			BuildConfig.LanguageSetting languageSetting = buildConfig.languageSettingDict[lanuageId];

			foreach (var kvp in languageSetting.fileCopySetting)
			{
				PathUtility.CopyFile(kvp.Value, kvp.Key);
			}

			AssetDatabase.Refresh();
		}

		private static bool SerializeConfigToFile(string configName, string filePath, string outputPath)
		{
			IFileLoader fileLoader = new FileLoaderFromWorkspace();
			int fileFormat = Configuration._FileFormat.Xml;

			ConfigDatabase.Initialize(null, false, false);

			// Load config
			Configuration configuration = null;
			switch (configName)
			{
				case "ActionConfig": configuration = ConfigDatabase.LoadConfig<ActionConfig>(null, fileLoader, fileFormat, filePath); break;
				case "AnimationConfig": configuration = ConfigDatabase.LoadConfig<AnimationConfig>(null, fileLoader, fileFormat, filePath); break;
				case "AppleGoodConfig": configuration = ConfigDatabase.LoadConfig<AppleGoodConfig>(null, fileLoader, fileFormat, filePath); break;
				case "ArenaConfig": configuration = ConfigDatabase.LoadConfig<ArenaConfig>(null, fileLoader, fileFormat, filePath); break;
				case "AssetDescConfig": configuration = ConfigDatabase.LoadConfig<AssetDescConfig>(null, fileLoader, fileFormat, filePath); break;
				case "AvatarAssetConfig": configuration = ConfigDatabase.LoadConfig<AvatarAssetConfig>(null, fileLoader, fileFormat, filePath); break;
				case "AvatarConfig": configuration = ConfigDatabase.LoadConfig<AvatarConfig>(null, fileLoader, fileFormat, filePath); break;
				case "CampaignConfig": configuration = ConfigDatabase.LoadConfig<CampaignConfig>(null, fileLoader, fileFormat, filePath); break;
				case "ClientConfig": configuration = ConfigDatabase.LoadConfig<ClientConfig>(null, fileLoader, fileFormat, filePath); break;
				case "ClientManifest": configuration = ConfigDatabase.LoadConfig<ClientManifest>(null, fileLoader, fileFormat, filePath); break;
				case "DailySignInConfig": configuration = ConfigDatabase.LoadConfig<DailySignInConfig>(null, fileLoader, fileFormat, filePath); break;
				case "DialogueConfig": configuration = ConfigDatabase.LoadConfig<DialogueConfig>(null, fileLoader, fileFormat, filePath); break;
				case "EquipmentConfig": configuration = ConfigDatabase.LoadConfig<EquipmentConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GameConfig": configuration = ConfigDatabase.LoadConfig<GameConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GoodConfig": configuration = ConfigDatabase.LoadConfig<GoodConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GuideConfig": configuration = ConfigDatabase.LoadConfig<GuideConfig>(null, fileLoader, fileFormat, filePath); break;
				case "ItemConfig": configuration = ConfigDatabase.LoadConfig<ItemConfig>(null, fileLoader, fileFormat, filePath); break;
				case "LevelConfig": configuration = ConfigDatabase.LoadConfig<LevelConfig>(null, fileLoader, fileFormat, filePath); break;
				case "LevelRewardConfig": configuration = ConfigDatabase.LoadConfig<LevelRewardConfig>(null, fileLoader, fileFormat, filePath); break;
				case "LocalNotificationConfig": configuration = ConfigDatabase.LoadConfig<LocalNotificationConfig>(null, fileLoader, fileFormat, filePath); break;
				case "PveConfig": configuration = ConfigDatabase.LoadConfig<PveConfig>(null, fileLoader, fileFormat, filePath); break;
				case "QuestConfig": configuration = ConfigDatabase.LoadConfig<QuestConfig>(null, fileLoader, fileFormat, filePath); break;
				case "SceneConfig": configuration = ConfigDatabase.LoadConfig<SceneConfig>(null, fileLoader, fileFormat, filePath); break;
				case "SkillConfig": configuration = ConfigDatabase.LoadConfig<SkillConfig>(null, fileLoader, fileFormat, filePath); break;
				case "StringsConfig": configuration = ConfigDatabase.LoadConfig<StringsConfig>(null, fileLoader, fileFormat, filePath); break;
				case "TavernConfig": configuration = ConfigDatabase.LoadConfig<TavernConfig>(null, fileLoader, fileFormat, filePath); break;
				case "TutorialConfig": configuration = ConfigDatabase.LoadConfig<TutorialConfig>(null, fileLoader, fileFormat, filePath); break;
				case "VipConfig": configuration = ConfigDatabase.LoadConfig<VipConfig>(null, fileLoader, fileFormat, filePath); break;
				case "MeridianConfig": configuration = ConfigDatabase.LoadConfig<MeridianConfig>(null, fileLoader, fileFormat, filePath); break;
				case "StartServerRewardConfig": configuration = ConfigDatabase.LoadConfig<StartServerRewardConfig>(null, fileLoader, fileFormat, filePath); break;
				case "MysteryShopConfig": configuration = ConfigDatabase.LoadConfig<MysteryShopConfig>(null, fileLoader, fileFormat, filePath); break;
				case "InitPlayerConfig": configuration = ConfigDatabase.LoadConfig<InitPlayerConfig>(null, fileLoader, fileFormat, filePath); break;
				//case "NpcConfig": configuration = ConfigDatabase.LoadConfig<NpcConfig>(null, fileLoader, fileFormat, filePath); break;
				case "PositionConfig": configuration = ConfigDatabase.LoadConfig<PositionConfig>(null, fileLoader, fileFormat, filePath); break;
				case "DomineerConfig": configuration = ConfigDatabase.LoadConfig<DomineerConfig>(null, fileLoader, fileFormat, filePath); break;
				case "SuiteConfig": configuration = ConfigDatabase.LoadConfig<SuiteConfig>(null, fileLoader, fileFormat, filePath); break;
				case "PartnerConfig": configuration = ConfigDatabase.LoadConfig<PartnerConfig>(null, fileLoader, fileFormat, filePath); break;
				case "DinerConfig": configuration = ConfigDatabase.LoadConfig<DinerConfig>(null, fileLoader, fileFormat, filePath); break;
				case "IllustrationConfig": configuration = ConfigDatabase.LoadConfig<IllustrationConfig>(null, fileLoader, fileFormat, filePath); break;
				case "TaskConfig": configuration = ConfigDatabase.LoadConfig<TaskConfig>(null, fileLoader, fileFormat, filePath); break;
				case "MelaleucaFloorConfig": configuration = ConfigDatabase.LoadConfig<MelaleucaFloorConfig>(null, fileLoader, fileFormat, filePath); break;
				case "TreasureBowlConfig": configuration = ConfigDatabase.LoadConfig<TreasureBowlConfig>(null, fileLoader, fileFormat, filePath); break;
				case "WolfSmokeConfig": configuration = ConfigDatabase.LoadConfig<WolfSmokeConfig>(null, fileLoader, fileFormat, filePath); break;
				case "SpecialGoodsConfig": configuration = ConfigDatabase.LoadConfig<SpecialGoodsConfig>(null, fileLoader, fileFormat, filePath); break;
				case "QinInfoConfig": configuration = ConfigDatabase.LoadConfig<QinInfoConfig>(null, fileLoader, fileFormat, filePath); break;
				case "MonthCardConfig": configuration = ConfigDatabase.LoadConfig<MonthCardConfig>(null, fileLoader, fileFormat, filePath); break;
				case "OperationConfig": configuration = ConfigDatabase.LoadConfig<OperationConfig>(null, fileLoader, fileFormat, filePath); break;
				case "MarvellousAdventureConfig": configuration = ConfigDatabase.LoadConfig<MarvellousAdventureConfig>(null, fileLoader, fileFormat, filePath); break;
				case "IllusionConfig": configuration = ConfigDatabase.LoadConfig<IllusionConfig>(null, fileLoader, fileFormat, filePath); break;
				case "FriendCampaignConfig": configuration = ConfigDatabase.LoadConfig<FriendCampaignConfig>(null, fileLoader, fileFormat, filePath); break;
				case "SevenElevenGiftConfig": configuration = ConfigDatabase.LoadConfig<SevenElevenGiftConfig>(null, fileLoader, fileFormat, filePath); break;
				case "DanConfig": configuration = ConfigDatabase.LoadConfig<DanConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GuildConfig": configuration = ConfigDatabase.LoadConfig<GuildConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GuildPublicShopConfig": configuration = ConfigDatabase.LoadConfig<GuildPublicShopConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GuildPrivateShopConfig": configuration = ConfigDatabase.LoadConfig<GuildPrivateShopConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GuildExchangeShopConfig": configuration = ConfigDatabase.LoadConfig<GuildExchangeShopConfig>(null, fileLoader, fileFormat, filePath); break;
				case "GuildStageConfig": configuration = ConfigDatabase.LoadConfig<GuildStageConfig>(null, fileLoader, fileFormat, filePath); break;
				case "PowerConfig": configuration = ConfigDatabase.LoadConfig<PowerConfig>(null, fileLoader, fileFormat, filePath); break;
				case "ChangeNameConfig": configuration = ConfigDatabase.LoadConfig<ChangeNameConfig>(null, fileLoader, fileFormat, filePath); break;
				case "BeastConfig": configuration = ConfigDatabase.LoadConfig<BeastConfig>(null, fileLoader, fileFormat, filePath); break;
			}

			if (configuration == null)
			{
				Debug.LogError("Invalid config name : " + configName);
				return false;
			}

			PathUtility.CreateDirectory(Path.GetDirectoryName(outputPath));

			using (FileStream fileSteam = new FileStream(outputPath, FileMode.Create))
			{
				ConfigDatabase.Serialize(configuration, fileSteam);
			}

			return true;
		}

		private static ClientManifestEditor BuildClientManifest(string outputPath, string clientManifestFile, List<string> ignorePaths, bool streamAllAssets, int baseAppRevision, bool compress, List<Object> additionalFiles, List<string> additionalFilePathInResource, List<string> additionalFilePathInWorkspace, List<string> keepNamePaths)
		{
			Debug.Log("------ Building ClientManifest ------");
			return ClientManifestEditor.BuildClientManifest(outputPath, clientManifestFile, ignorePaths, streamAllAssets, baseAppRevision, compress, additionalFiles, additionalFilePathInResource, additionalFilePathInWorkspace, keepNamePaths);
		}

		private static void BuildGameAsset(BuildConfig buildConfig, string lanuageId, bool binaryMode, bool compress, bool streamAllAssets, int baseAppRevision, int revision)
		{
			int fileSize, uncompressedFileSize;
			ClientManifestEditor clientManifest;
			BuildGameAsset(buildConfig, lanuageId, binaryMode, compress, streamAllAssets, baseAppRevision, revision, out fileSize, out uncompressedFileSize, out clientManifest);
		}

		private static void BuildConfigFile(BuildConfig buildConfig, BuildConfig.LanguageSetting.GameConfig gameConfig, bool binaryMode, List<Object> outConfigFiles, List<string> outFilePathInResource, List<string> outFilePathInWorkspace)
		{
			string textAssetName = gameConfig.valid ? gameConfig.filePath : buildConfig.emptyConfigFile;

			if (binaryMode)
			{
				string tempFileName = Path.Combine(sIntermediateFolder, gameConfig.name + ".bytes");
				if (SerializeConfigToFile(gameConfig.name, textAssetName, tempFileName) == false)
					throw new System.Exception("Serialize config file failed : " + gameConfig.name);

				AssetDatabase.ImportAsset(tempFileName);
				textAssetName = tempFileName;
			}

			var textAsset = AssetDatabase.LoadAssetAtPath(textAssetName, typeof(TextAsset)) as TextAsset;
			if (textAsset == null)
				throw new System.Exception("Load config file failed : " + textAssetName);

			outConfigFiles.Add(textAsset);
			outFilePathInResource.Add(gameConfig.assetName);
			outFilePathInWorkspace.Add(textAssetName);
		}

		private static string BuildGameAsset(BuildConfig buildConfig, string lanuageId, bool binaryMode, bool compress, bool streamAllAssets, int baseAppRevision, int revision, out int fileSize, out int uncompressedFileSize, out ClientManifestEditor clientManifest)
		{
			Debug.Log("------ Building Game Config ------");

			// Initialize out parameters
			fileSize = 0;
			uncompressedFileSize = 0;

			// Clear asset bundle folder
			PathUtility.RemoveFile(GameDefines.assetBundleFolder);
			string assetBundleFolder = GameDefines.assetBundleFolder;
			if (PathUtility.CreateDirectory(assetBundleFolder) == false)
				throw new System.Exception("Create target path failed : " + assetBundleFolder);

			var languageSetting = buildConfig.languageSettingDict[lanuageId];
			var configFiles = new List<Object>();
			var filePathInResource = new List<string>();
			var filePathInWorkspace = new List<string>();

#if ENABLE_DELAY_CONFIG
			// Build config file except client manifest
			foreach (var gameConfig in languageSetting.gameConfigFiles)
			{
				if (gameConfig.name.Equals("ClientManifest", System.StringComparison.CurrentCultureIgnoreCase))
					continue;

				BuildConfigFile(buildConfig, gameConfig, binaryMode, configFiles, filePathInResource, filePathInWorkspace);
			}
#endif

			// Build asset bundle
			clientManifest = BuildClientManifest(GameDefines.assetBundleFolder, languageSetting.clientManifestFile, buildConfig.buildGameAssetIgnorePathList, streamAllAssets, baseAppRevision, compress, configFiles, filePathInResource, filePathInWorkspace, buildConfig.buildGameAssetKeepNamePathList);

			// Build client manifest
			configFiles.Clear();
			filePathInResource.Clear();
			filePathInWorkspace.Clear();

			foreach (var gameConfig in languageSetting.gameConfigFiles)
			{
#if ENABLE_DELAY_CONFIG
				if (gameConfig.name.Equals("ClientManifest", System.StringComparison.CurrentCultureIgnoreCase) == false)
					continue;
#endif

				BuildConfigFile(buildConfig, gameConfig, binaryMode, configFiles, filePathInResource, filePathInWorkspace);
			}

			// Build config asset bundle
			string targetFile = Path.Combine(assetBundleFolder, buildConfig.targetGameConfigFile);

#if UNITY_4_0 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_4_8 || UNITY_4_9
			BuildPipeline.BuildAssetBundleExplicitAssetNames(configFiles.ToArray(),
				filePathInResource.ToArray(),
				targetFile,
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle,
				EditorUserBuildSettings.activeBuildTarget);
#else
			BuildPipeline.BuildAssetBundleExplicitAssetNames(configFile.ToArray(),
				filePathInResource.ToArray(),
				targetFile,
				BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets | BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle,
				EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
#endif

			// Replace file name with file hash name
			byte[] bytes = System.IO.File.ReadAllBytes(targetFile);
			uncompressedFileSize = bytes.Length;

			string fileName = string.Format("{0}.kod", KodGames.Cryptography.Md5Hash(bytes));
			string renamedFileName = PathUtility.Combine(assetBundleFolder, fileName);
			using (FileStream renamedFile = new FileStream(renamedFileName, FileMode.Create))
				if (compress)
					BZip2.Compress(new MemoryStream(bytes), renamedFile, false, 9);
				else
					renamedFile.Write(bytes, 0, bytes.Length);
			PathUtility.RemoveFile(targetFile);

			// Get file size
			System.IO.FileInfo _fileInfo = new System.IO.FileInfo(renamedFileName);
			fileSize = (int)_fileInfo.Length;

			// Save file name and size to PlayerPrefs for editor testing
			PlayerPrefs.SetString("BuildProduct.GameConfigName", fileName);
			PlayerPrefs.SetInt("BuildProduct.GameConfigSize", fileSize);
			PlayerPrefs.SetInt("BuildProduct.GameConfigUncompressedSize", uncompressedFileSize);
			PlayerPrefs.Save();

			System.GC.Collect();

			return renamedFileName;
		}

		private static bool BuildGameUI(BuildConfig buildConfig, BuildTarget buildTarget, string languageId)
		{
			Debug.Log("------ Building Game UI ------");

			BuildConfig.LanguageSetting languageSetting = buildConfig.languageSettingDict[languageId];

			// Load language config
			StringsConfig cfg = ConfigDatabase.LoadConfig<StringsConfig>(null, new FileLoaderFromWorkspace(), Configuration._FileFormat.Xml, languageSetting.textFile);
			if (cfg == null)
				return true;

			// Load editor level
			if (buildConfig.uiEditorlevelSetting != null)
			{
				if (EditorApplication.OpenScene(buildConfig.uiEditorlevelSetting.levelName))
				{
					UIEditorWindow.CheckBrokenUIPrefab();
					UIEditorWindow.DoClearUIModules();
					UIEditorWindow.DoLoadUIModules(buildConfig.uiEditorlevelSetting.targetPath);

					ChangeSpriteSetting(buildConfig, languageId);

					UIEditorWindow.DoBuildUIModules(cfg);
					UIEditorWindow.DoDelteUIModules(buildConfig.uiEditorlevelSetting.targetPath, buildConfig.buildGameAssetDeleteUIForReSave);
					UIEditorWindow.DoSaveUIModules(buildConfig.uiEditorlevelSetting.targetPath);
					UIEditorWindow.CheckBrokenUIPrefab();
				}
				else
				{
					Debug.LogError("OpenScene failed : " + buildConfig.uiEditorlevelSetting.levelName);
					return false;
				}
			}

			return false;
		}

		private static void ProcessFileOperation(BuildConfig buildConfig, List<KeyValuePair<string, string>> envValues, string setName, BuildConfig.FileOperationSet._PhaseType phaseType)
		{
			foreach (var set in buildConfig.fileOperationSets)
			{
				if (set.name.Equals(setName, System.StringComparison.CurrentCultureIgnoreCase) == false)
					continue;

				if (set.phaseType != phaseType)
					continue;

				foreach (var operation in set.operations)
				{
					if (string.Equals(operation.first, "CopyFile", System.StringComparison.CurrentCultureIgnoreCase))
						PathUtility.CopyFile(ProcessEnvString(operation.second.first, envValues), ProcessEnvString(operation.second.second, envValues));
					else if (string.Compare(operation.first, "RemoveFile", System.StringComparison.OrdinalIgnoreCase) == 0)
						PathUtility.RemoveFile(ProcessEnvString(operation.second.first, envValues));
				}
			}
		}

		private static void ParseFromCommandLine(out BuildTarget _buildTarget, out string _outputPath, out string _bundleId, out string _lanuageId,
			out bool _streamAllAssets, out int _baseAppRevision, out int _svnRevision, out string _manifestFile, out string _scriptingDefine,
			out bool _buildGameAsset, out bool _buildGameUI, out string _fileOperationSet, out bool _developmentBuild,
			out List<KeyValuePair<string, string>> envValues, out Dictionary<string, string> modiferBuildConfigs)
		{
			_outputPath = "";

			_buildTarget = BuildTarget.XBOX360;
			_bundleId = "";
			_lanuageId = "";
			_streamAllAssets = false;
			_baseAppRevision = 0;
			_svnRevision = 0;
			_manifestFile = "";
			_scriptingDefine = "";
			_buildGameAsset = true;
			_buildGameUI = true;
			_fileOperationSet = "";
			_developmentBuild = false;

			modiferBuildConfigs = new Dictionary<string, string>();
			//_buildTarget = BuildTarget.Android;
			//_bundleId = "com.KodGames.WuLin";
			//_lanuageId = "Zh_cn";
			//_streamAllAssets = false;
			//_baseAppRevision = 0;
			//_svnRevision = 0;
			//_buildGameAsset = true;
			//_buildGameUI = false;
			//_fileOperationSet = "XXWan";

			Debug.Log(System.Environment.CommandLine);
			string[] args = System.Environment.GetCommandLineArgs();
			for (int i = 0; i < args.Length; ++i)
			{
				Debug.Log(args[i]);
				if (i < args.Length - 1)
				{
					if (string.Compare(args[i], "-BuildTarget", true) == 0)
					{
						string param = args[++i];
						if (string.Compare(param, BuildTarget.iPhone.ToString(), true) == 0)
							_buildTarget = BuildTarget.iPhone;
						if (string.Compare(param, BuildTarget.Android.ToString(), true) == 0)
							_buildTarget = BuildTarget.Android;
					}
					else if (string.Compare(args[i], "-BundleID", true) == 0)
						_bundleId = args[++i];
					else if (string.Compare(args[i], "-Language", true) == 0)
						_lanuageId = args[++i];
					else if (string.Compare(args[i], "-OutputPath", true) == 0)
						_outputPath = args[++i];
					else if (string.Compare(args[i], "-BaseAppRevision", true) == 0)
						_baseAppRevision = StrParser.ParseDecInt(args[++i], 0);
					else if (string.Compare(args[i], "-SvnRevision", true) == 0)
						_svnRevision = StrParser.ParseDecInt(args[++i], 0);
					else if (string.Compare(args[i], "-StreamAllAssets", true) == 0)
						_streamAllAssets = StrParser.ParseBool(args[++i], false);
					else if (string.Compare(args[i], "-ManifestFile", true) == 0)
						_manifestFile = args[++i];
					else if (string.Compare(args[i], "-ScriptingDefine", true) == 0)
						_scriptingDefine = args[++i];
					else if (string.Compare(args[i], "-BuildGameAsset", true) == 0)
						_buildGameAsset = StrParser.ParseBool(args[++i], _buildGameAsset);
					else if (string.Compare(args[i], "-BuildGameUI", true) == 0)
						_buildGameUI = StrParser.ParseBool(args[++i], _buildGameUI);
					else if (string.Compare(args[i], "-FileOperationSet", true) == 0)
						_fileOperationSet = args[++i];
					else if (string.Compare(args[i], "-DevelopmentBuild", true) == 0)
						_developmentBuild = StrParser.ParseBool(args[++i], _developmentBuild);
					else if (string.Compare(args[i], "-RemoveLevel", true) == 0)
						modiferBuildConfigs.Add(args[i], args[++i]);
					else if (string.Compare(args[i], "-AddIgnorePath", true) == 0)
						modiferBuildConfigs.Add(args[i], PathUtility.UnifyPath(args[++i]));
					else if (string.Compare(args[i], "-RemoveIgnorePath", true) == 0)
						modiferBuildConfigs.Add(args[i], PathUtility.UnifyPath(args[++i]));
				}
			}

			// Create env parameters
			envValues = new List<KeyValuePair<string, string>>();
			envValues.Add(new KeyValuePair<string, string>("%OUTPUT_PATH%", _outputPath));
			envValues.Add(new KeyValuePair<string, string>("%BUILD_TARGET%", _buildTarget.ToString()));
			envValues.Add(new KeyValuePair<string, string>("%BUNDLE_ID%", _bundleId));
			envValues.Add(new KeyValuePair<string, string>("%LANGUAGE_ID%", _lanuageId));
			envValues.Add(new KeyValuePair<string, string>("%PRODUCT_NAME%", PlayerSettings.productName));

			Debug.Log("Build Parameters : " +
				"\n_buildTarget " + _buildTarget +
				"\n_bundleId " + _bundleId +
				"\n_lanuageId " + _lanuageId +
				"\n_svnRevision " + _svnRevision +
				"\n_baseAppRevision " + _baseAppRevision +
				"\n_streamAllAssets " + _streamAllAssets +
				"\n_manifestFile " + _manifestFile +
				"\n_outputPath " + _outputPath +
				"\n_scriptingDefine " + _scriptingDefine +
				"\n_buildGameAsset " + _buildGameAsset +
				"\n_buildGameUI " + _buildGameUI +
				"\n_fileOperationSet " + _fileOperationSet +
				"\n_developmentBuild " + _developmentBuild);
		}

		private static string ProcessEnvString(string str, List<KeyValuePair<string, string>> envValues)
		{
			bool processed = true;
			while (processed)
			{
				processed = false;
				foreach (var kvp in envValues)
				{
					string processedStr = str.Replace(kvp.Key, kvp.Value);
					processed |= processedStr != str;
					str = processedStr;
				}
			}

			return str;
		}

		public static void BuildAsset()
		{
			// Parse command line
			BuildTarget _buildTarget;
			string _outputPath, _bundleId, _lanuageId;
			bool _streamAllAssets;
			int _baseAppRevision, _svnRevision;
			bool _buildGameAsset, _buildGameUI;
			string _manifestFile, _scriptingDefine, _fileOperationSet;
			bool _developmentBuild;
			List<KeyValuePair<string, string>> envValues;
			Dictionary<string, string> modiferBuildConfig;

			ParseFromCommandLine(out _buildTarget, out _outputPath, out _bundleId, out _lanuageId, out _streamAllAssets, out _baseAppRevision, out _svnRevision, out _manifestFile, out _scriptingDefine,
								 out _buildGameAsset, out _buildGameUI, out _fileOperationSet, out _developmentBuild, out envValues, out modiferBuildConfig);

			// Process asset
			BuildConfig buildConfig = BuildConfig.LoadBuildConfig(sBuildConfigFile);
			if (buildConfig == null)
				EditorApplication.Exit(1);

			// Verify command line
			if (_buildTarget == BuildTarget.XBOX360)
			{
				Debug.LogError("Invalid build target : " + _buildTarget);
				EditorApplication.Exit(1);
			}
			if (buildConfig.languageSettingDict.ContainsKey(_lanuageId) == false)
			{
				Debug.LogError("Invalid language ID : " + _lanuageId);
				EditorApplication.Exit(1);
			}

			if (_baseAppRevision == 0)
				Debug.Log("Build base asset");

			BuildTargetGroup targetGroup = BuildTargetGroup.Unknown;
			if (_buildTarget == BuildTarget.iPhone)
				targetGroup = BuildTargetGroup.iPhone;
			else if (_buildTarget == BuildTarget.Android)
				targetGroup = BuildTargetGroup.Android;

			string oldScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, _scriptingDefine);

			try
			{
				// Copy language file to target folder
				CopyLocalizedFiles(buildConfig, _lanuageId);

				// Build client config
				int fileSize, uncompressedFileSize;
				ClientManifestEditor clientManifest;
				string gameConfigFileName = BuildGameAsset(buildConfig, _lanuageId, true, true, _streamAllAssets, _baseAppRevision, _svnRevision, out fileSize, out uncompressedFileSize, out clientManifest);
				if (gameConfigFileName != "")
					// Save file name and size to build result for server usage
					ServerManifestEditor.ModifyConfig(_manifestFile, gameConfigFileName, fileSize, uncompressedFileSize);

				// 不用Build客户端, 所以不需要删除文件
			}
			catch (System.Exception e)
			{
				Debug.LogError(string.Format("Exception occurs when build player : message:{0}, Stack:{0}", e.Message, e.StackTrace));
				PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, oldScriptingDefineSymbols);
				EditorApplication.Exit(1);
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, oldScriptingDefineSymbols);
		}

		//[MenuItem("Product/BuildPlayer")]
		public static void BuildPlayer()
		{
			// Parse command line
			BuildTarget _buildTarget;
			string _outputPath, _bundleId, _lanuageId;
			bool _streamAllAssets;
			int _baseAppRevision, _svnRevision;
			bool _buildGameAsset, _buildGameUI;
			string _manifestFile, _scriptingDefine, _fileOperationSet;
			bool _developmentBuild;
			List<KeyValuePair<string, string>> envValues;
			Dictionary<string, string> modiferBuildConfig;
			ParseFromCommandLine(out _buildTarget, out _outputPath, out _bundleId, out _lanuageId, out _streamAllAssets, out _baseAppRevision, out _svnRevision, out _manifestFile, out _scriptingDefine,
								 out _buildGameAsset, out _buildGameUI, out _fileOperationSet, out _developmentBuild, out envValues, out modiferBuildConfig);

			// Process asset
			BuildConfig buildConfig = BuildConfig.LoadBuildConfig(sBuildConfigFile);
			if (buildConfig == null)
				EditorApplication.Exit(1);

			// Process BuildConfig Set From python.
			foreach (var config in modiferBuildConfig)
			{
				switch (config.Key)
				{
					case "-RemoveLevel":
						var values = config.Value.Split('#');
						foreach (var removeLevel in values)
						{
							if (string.IsNullOrEmpty(removeLevel))
								continue;

							if (buildConfig.unityLevelList.Contains(removeLevel))
								buildConfig.unityLevelList.Remove(removeLevel);
						}
						break;

					case "-AddIgnorePath":
						var addPaths = config.Value.Split('#');
						foreach (var addPath in addPaths)
						{
							if (string.IsNullOrEmpty(addPath))
								continue;

							if (buildConfig.buildGameAssetIgnorePathList.Contains(addPath) == false)
								buildConfig.buildGameAssetIgnorePathList.Add(addPath);
						}

						break;

					case "-RemoveIgnorePath":
						var removePaths = config.Value.Split('#');
						foreach (var removePath in removePaths)
						{
							if (string.IsNullOrEmpty(removePath))
								continue;

							if (buildConfig.buildGameAssetIgnorePathList.Contains(removePath))
								buildConfig.buildGameAssetIgnorePathList.Remove(removePath);
						}

						break;
				}
			}

			// Verify command line
			if (_buildTarget == BuildTarget.XBOX360)
			{
				Debug.LogError("Invalid build target : " + _buildTarget);
				EditorApplication.Exit(1);
			}
			if (buildConfig.languageSettingDict.ContainsKey(_lanuageId) == false)
			{
				Debug.LogError("Invalid language ID : " + _lanuageId);
				EditorApplication.Exit(1);
			}

			BuildTargetGroup targetGroup = BuildTargetGroup.Unknown;
			if (_buildTarget == BuildTarget.iPhone)
				targetGroup = BuildTargetGroup.iPhone;
			else if (_buildTarget == BuildTarget.Android)
				targetGroup = BuildTargetGroup.Android;

			string oldScriptingDefineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
			PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, _scriptingDefine);

			try
			{
				// Copy language file to target folder
				CopyLocalizedFiles(buildConfig, _lanuageId);

				// Build UI
				if (_buildGameUI)
					BuildGameUI(buildConfig, _buildTarget, _lanuageId);

				// Build asset bundle
				if (_buildGameAsset)
				{
					// Build client config
					int fileSize, uncompressedFileSize;
					ClientManifestEditor clientManifest;
					string gameConfigFileName = BuildGameAsset(buildConfig, _lanuageId, true, true, _streamAllAssets, _baseAppRevision, _svnRevision, out fileSize, out uncompressedFileSize, out clientManifest);
					if (gameConfigFileName != "")
						// Save file name and size to build result for server usage
						ServerManifestEditor.ModifyConfig(_manifestFile, gameConfigFileName, fileSize, uncompressedFileSize);

					if (_streamAllAssets)
					{
						// 将所有打包的资源文件删除, 不在客户端中出现
						foreach (var resourcePath in new string[] { PathUtility.UnifyPath("Assets/Resources"), PathUtility.UnifyPath("Assets/StreamingAssets") })
						{
							foreach (var filePath in clientManifest.fileNamesInWorkspace)
							{
								bool ignore = false;
								foreach (var ignorePath in buildConfig.buildGameAssetIgnorePathList)
								{
									// Ignore files in buildGameAssetIgnorePathList and Resources folder
									var path = PathUtility.UnifyPath(filePath);
									if (path.Contains(resourcePath) && path.Contains(ignorePath) == false)
										continue;

									ignore = true;
									break;
								}

								// Remove the file
								if (ignore)
									continue;

								Debug.Log(filePath);
								File.Delete(filePath);
							}
						}


						AssetDatabase.Refresh();
					}
				}

				// Build unity

				// Delete old project
				string outputPath = ProcessEnvString(_outputPath, envValues);
				PathUtility.CreateDirectory(outputPath);
				outputPath = Path.Combine(outputPath, PlayerSettings.productName);
				if (Directory.Exists(outputPath))
					Directory.Delete(outputPath, true);

				BuildOptions buildOption = _developmentBuild ? BuildOptions.Development | BuildOptions.ConnectWithProfiler : BuildOptions.None;
#if UNITY_ANDROID
				buildOption |= BuildOptions.AcceptExternalModificationsToPlayer;
#endif

				// Process project
				ProcessFileOperation(buildConfig, envValues, _fileOperationSet, KodGames.WorkFlow.BuildConfig.FileOperationSet._PhaseType.BeforeBuildPlayer);
				AssetDatabase.Refresh();

				// Get output path
#if UNITY_ANDROID
				string result = BuildPipeline.BuildPlayer(buildConfig.unityLevelList.ToArray(), ProcessEnvString(_outputPath, envValues), _buildTarget, buildOption);
#else
				string result = BuildPipeline.BuildPlayer(buildConfig.unityLevelList.ToArray(), Path.Combine(ProcessEnvString(_outputPath, envValues), PlayerSettings.productName), _buildTarget, buildOption);
#endif
				if (result != null && result != "")
				{
					Debug.Log("Build Unity Player failed. " + result);
					EditorApplication.Exit(1);
				}

				// Process project
				ProcessFileOperation(buildConfig, envValues, _fileOperationSet, KodGames.WorkFlow.BuildConfig.FileOperationSet._PhaseType.AfterBuildPlayer);
			}
			catch (System.Exception e)
			{
				Debug.LogError(string.Format("Exception occurs when build player : message:{0}, Stack:{1}", e.Message, e.StackTrace));
				PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, oldScriptingDefineSymbols);
				EditorApplication.Exit(1);
			}

			PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, oldScriptingDefineSymbols);
		}
	}
}
