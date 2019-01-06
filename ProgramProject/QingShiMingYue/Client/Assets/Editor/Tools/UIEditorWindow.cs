#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Security;
using Mono.Xml;
using System.IO;
using ClientServerCommon;
using System.Text;
using System.Xml;

namespace KodGames.WorkFlow
{
	public class UIEditorWindow : EditorWindow
	{
		private static string sUIPrefabPath = @"Assets/Resources/Objects/UI/Common";

		[MenuItem("Product/Show UI Editor &d")]
		public static void ShowWindow()
		{
			LoadSetting();
			GetWindow(typeof(UIEditorWindow), true, "UI Editor");
		}

		[MenuItem("Tools/UI/Select SpriteRoots with extra materials")]
		public static void SelectSpriteRootWithExtraMat()
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			SpriteRoot[] spriteRoots = uiContainer.GetComponentsInChildren<SpriteRoot>();
			if (spriteRoots == null || spriteRoots.Length == 0)
				return;

			List<GameObject> selectSpriteRoots = new List<GameObject>();
			foreach (SpriteRoot spriteRoot in spriteRoots)
			{
				if (spriteRoot.renderer.sharedMaterials.Length > 1)
					selectSpriteRoots.Add(spriteRoot.gameObject);
			}

			Selection.objects = selectSpriteRoots.ToArray();
		}

		[MenuItem("Tools/UI/Set ScrollList clip when moving")]
		public static void SetScrollListClipWhenMoving()
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			UIScrollList[] scrollLists = uiContainer.GetComponentsInChildren<UIScrollList>();
			if (scrollLists == null || scrollLists.Length == 0)
				return;

			List<GameObject> selectSpriteRoots = new List<GameObject>();
			foreach (var scrollList in scrollLists)
			{
				if (scrollList.clipWhenMoving == false)
				{
					scrollList.clipWhenMoving = true;
					selectSpriteRoots.Add(scrollList.gameObject);
				}
			}

			Selection.objects = selectSpriteRoots.ToArray();
		}

		void OnGUI()
		{
			sUIPrefabPath = EditorGUILayout.TextField("UI Prefab Path", sUIPrefabPath);

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Load UI Prefabs"))
			{
				ReloadAllUIModules();
				SaveSetting();
			}

			if (GUILayout.Button("Save UI Prefabs"))
			{
				SaveAllUIModules();
				SaveSetting();
			}

			if (GUILayout.Button("Build&Save"))
			{
				BuildAndSaveAllUIModules();
				SaveSetting();
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();

			if (GUILayout.Button("Show All UI"))
				ShowSelectUI(false);

			if (GUILayout.Button("Show Selected UI"))
				ShowSelectUI(true);

			EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Reset Offset Z"))
				UITools.SetUIOffestZ();

			EditorGUILayout.Separator();

			if (GUILayout.Button("Set Game View Size"))
				AShimSetGameSizeWindow.Init();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Localize"))
				LocalizeUI();

			if (GUILayout.Button("Apply Template"))
				UITemplateFormatter.Fromat();
			EditorGUILayout.EndHorizontal();

			// Selection
			EditorGUILayout.Separator();
			EditorGUILayout.LabelField("Selection :");

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("UI with texture"))
				SelectUIWithTexture();

			if (GUILayout.Button("UI with empty texture"))
				SelectUIWithEmptyTexture();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("UI with material"))
				SelectUIWithMaterial();

			if (GUILayout.Button("UI with mat texture"))
				SelectUIWithMaterialTexture();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("UI with font"))
				SelectUIWithFont();

			if (GUILayout.Button("UI with template"))
				SelectUIWithTemplate();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Packaged Texture"))
				SelectPackagedTextures();
			if (GUILayout.Button("Packaged Texture in Material"))
				SelectPackagedTexturesInMaterial();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("EZScreenPlacement"))
				SelectEZScreenPlacement();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.EndHorizontal();
		}

		public static void LoadSetting()
		{
			sUIPrefabPath = PlayerPrefs.GetString("UIEditorWindow.sUIPrefabPath", sUIPrefabPath);
		}

		private static void SaveSetting()
		{
			PlayerPrefs.SetString("UIEditorWindow.sUIPrefabPath", sUIPrefabPath);
		}

		private static GameObject GetUIContainer()
		{
			return GameObject.Find("UIContainer");
		}

		private static GameObject GetUICamera()
		{
			return GameObject.Find("UICamera");
		}

		public static void ReloadAllUIModules()
		{
			if (EditorUtility.DisplayDialog("Reload All UI Modules", "Load UI module will lose changing in current scene.\nContinue?", "Reload", "Cancel") == false)
				return;

			DoReloadUIModules();
		}

		public static void SaveAllUIModules()
		{
			if (EditorUtility.DisplayDialog("Save All UI Modules", "Save UI module will replace prefab in prefab path.\nContinue?", "Save", "Cancel") == false)
				return;

			DoSaveUIModules(sUIPrefabPath);
		}

		public static void BuildAndSaveAllUIModules()
		{
			if (EditorUtility.DisplayDialog("Build and Save", "Save UI module will replace prefab in prefab path.\nContinue?", "Save", "Cancel") == false)
				return;

			DoBuildAndSaveUIModules(sUIPrefabPath, LoadLauguageFile());
		}

		public static void LocalizeUI()
		{
			DoLocalizeUI(LoadLauguageFile());
		}

		private static StringsConfig LoadLauguageFile()
		{
			var fileName = PlayerPrefs.GetString("BuildProduct.LanguageFile", "");
			return ConfigDatabase.LoadConfig<StringsConfig>(null, new FileLoaderFromWorkspace(), Configuration._FileFormat.Xml, fileName);
		}

		private static void DoReloadUIModules()
		{
			DoClearUIModules();
			DoLoadUIModules(sUIPrefabPath);
		}

		public static void DoDelteUIModules(string prefabPath, List<string> deleteUIModules)
		{
			DirectoryInfo dir = new DirectoryInfo(prefabPath);

			// If the source directory does not exist, throw an exception.
			if (!dir.Exists)
				return;

			// Get the file contents of the directory to copy.
			foreach (FileInfo file in dir.GetFiles())
			{
				if (deleteUIModules.Contains(file.Name) == false)
					continue;

				// Copy the file.
				GameObject prefab = AssetDatabase.LoadAssetAtPath(Path.Combine(prefabPath, file.Name), typeof(GameObject)) as GameObject;
				if (prefab == null)
					continue;

				if (PrefabUtility.GetPrefabType(prefab) == PrefabType.None)
					continue;

				AssetDatabase.DeleteAsset(Path.Combine(prefabPath, file.Name));
				AssetDatabase.Refresh();
			}
		}

		public static void DoClearUIModules()
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			List<GameObject> destoryingObjs = new List<GameObject>();
			foreach (var child in uiContainer.GetComponentsInChildren<Transform>(true))
			{
				if (child.parent == uiContainer.transform)
					destoryingObjs.Add(child.gameObject);
			}

			foreach (var obj in destoryingObjs)
				Object.DestroyImmediate(obj.gameObject);
		}

		public static void DoLoadUIModules(string prefabPath)
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			DirectoryInfo dir = new DirectoryInfo(prefabPath);

			// If the source directory does not exist, throw an exception.
			if (!dir.Exists)
				return;

			// Get the file contents of the directory to copy.
			foreach (FileInfo file in dir.GetFiles())
			{
				// Copy the file.
				GameObject prefab = AssetDatabase.LoadAssetAtPath(Path.Combine(prefabPath, file.Name), typeof(GameObject)) as GameObject;
				if (prefab == null)
					continue;

				if (PrefabUtility.GetPrefabType(prefab) == PrefabType.None)
					continue;

				GameObject go = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
				go.SetActive(true);
				ObjectUtility.AttachToParentAndResetLocalTrans(uiContainer, go);
			}
		}

		public static void DoSaveUIModules(string prefabPath)
		{
			if (PathUtility.CreateDirectory(prefabPath) == false)
			{
				Debug.LogError("Create target path failed : " + prefabPath);
				return;
			}

			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			foreach (var child in uiContainer.GetComponentsInChildren<Transform>(true))
			{
				if (child == null || child.parent != uiContainer.transform)
					continue;

				bool activeSelf = child.gameObject.activeSelf;
				child.gameObject.SetActive(true);

				// Reset transform
				Vector3 pos = child.localPosition;
				Quaternion rotation = child.localRotation;
				Vector3 scale = child.localScale;

				child.localPosition = Vector3.zero;
				child.localRotation = Quaternion.identity;
				child.localScale = Vector3.one;

				string prefabFilePath = PathUtility.Combine(false, prefabPath, child.gameObject.name + ".prefab");
				GameObject prefab = AssetDatabase.LoadAssetAtPath(prefabFilePath, typeof(GameObject)) as GameObject;
				if (prefab != null)
				{
					if (PrefabUtility.GetPrefabType(prefab) != PrefabType.None)
					{
						PrefabUtility.ReplacePrefab(child.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
					}
					else
					{
						Debug.LogError(string.Format("{0} is not a prefab.", prefabFilePath));
					}
				}
				else
				{
					PrefabUtility.CreatePrefab(prefabFilePath, child.gameObject, ReplacePrefabOptions.ConnectToPrefab);
				}

				// Reset transform
				child.localPosition = pos;
				child.localRotation = rotation;
				child.localScale = scale;

				// Reset active state
				child.gameObject.SetActive(activeSelf);
			}

			AssetDatabase.SaveAssets();
			//EditorApplication.SaveScene();
		}

		public static void DoBuildAndSaveUIModules(string prefabPath, StringsConfig stringCfg)
		{
			DoBuildUIModules(stringCfg);
			DoSaveUIModules(prefabPath);
		}

		public static void DoBuildUIModules(StringsConfig stringCfg)
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			// Set active state
			Dictionary<GameObject, bool> prefabStateDict = new Dictionary<GameObject, bool>();
			foreach (var child in uiContainer.GetComponentsInChildren<Transform>(true))
			{
				if (child.parent != uiContainer.transform)
					continue;

				prefabStateDict.Add(child.gameObject, child.gameObject.activeSelf);
				child.gameObject.SetActive(true);
			}

			// Localize UI
			DoLocalizeUI(stringCfg);

			// Generate atlases
			BuildAtlases ba = BuildAtlases.BuildSpriteAtlases();
			ba.OnWizardCreate();
			ba.Close();

			// Recover active state
			foreach (var kvp in prefabStateDict)
				kvp.Key.SetActive(kvp.Value);
		}

		private static void DoLocalizeUI(StringsConfig stringCfg)
		{
			// Get UI root
			GameObject root = GameObject.Find("UIContainer");
			if (root == null)
			{
				Debug.LogError("Not found UIContainer in this scene.");
				return;
			}

			//////////////////////////////////////////////////////////////////////////
			// Localize all UI with text
			{
				bool hasBldNoVal = false;
				StringBuilder bldNoVal = new StringBuilder("No values controls==========>\n\n");

				bool hasBldNoKey = false;
				StringBuilder bldNoKey = new StringBuilder("No key controls==========>\n\n");

				foreach (var st in root.GetComponentsInChildren<SpriteText>())
				{
					if (st.localizingKey != "")
					{
						if (stringCfg.HasString("UI", st.localizingKey))
						{
							st.Text = stringCfg.GetString("UI", st.localizingKey);
						}
						else
						{
							hasBldNoVal = true;
							bldNoVal.AppendFormat("{0} = ?? path = {1}\n", st.localizingKey, ObjectUtility.GetTransRootPath(st.transform));
						}
					}
					else if (st.Text != "")
					{
						hasBldNoKey = true;
						bldNoKey.AppendFormat(" ?? = {0} path = {1}\n", st.Text, ObjectUtility.GetTransRootPath(st.transform));
					}
				}

				foreach (var tf in root.GetComponentsInChildren<UITextField>())
				{
					if (tf.localizingKey != "")
					{
						if (stringCfg.HasString("UI", tf.localizingKey))
						{
							tf.placeHolder = stringCfg.GetString("UI", tf.localizingKey);
						}
						else
						{
							hasBldNoVal = true;
							bldNoVal.AppendFormat("{0} = ?? path = {1}\n", tf.localizingKey, ObjectUtility.GetTransRootPath(tf.transform));
						}
					}
					else if (tf.Text != "")
					{
						hasBldNoKey = true;
						bldNoKey.AppendFormat(" ?? = {0} path = {1}\n", tf.Text, ObjectUtility.GetTransRootPath(tf.transform));
					}
				}

				if (hasBldNoVal)
					Debug.LogWarning(bldNoVal.ToString());

				if (hasBldNoKey)
					Debug.LogWarning(bldNoKey.ToString());
			}
		}

		private void ShowSelectUI()
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			foreach (var child in uiContainer.GetComponentsInChildren<Transform>(true))
			{
				if (child.parent != uiContainer.transform)
					continue;

				child.gameObject.SetActive(ArrayUtility.Contains(Selection.gameObjects, child.gameObject));
			}
		}
		private void ShowSelectUI(bool onlySelected)
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			foreach (var child in uiContainer.GetComponentsInChildren<Transform>(true))
			{
				if (child.parent != uiContainer.transform)
					continue;

				child.gameObject.SetActive(onlySelected == false || ArrayUtility.Contains(Selection.gameObjects, child.gameObject));
			}
		}

		private static bool CreateDirectory(string targetPath)
		{
			if (Directory.Exists(targetPath) == false)
			{
				Directory.CreateDirectory(targetPath);
				return Directory.Exists(targetPath);
			}
			else
			{
				return true;
			}
		}

		private static void CopyFile(string sourceFile, string targetFile)
		{
#if ENABLE_BUILD_LOG
			Debug.Log(sourceFile + " " + targetFile);
#endif
			// Convert to full path
			sourceFile = Path.GetFullPath(sourceFile);
			targetFile = Path.GetFullPath(targetFile);

			if (Directory.Exists(sourceFile))
			{
				// Copy directory
				DirectoryCopy(sourceFile, targetFile, true);
			}
			else
			{
				// Copy file
				// Create target path if it doesn't exist
				string targetPath = Path.GetDirectoryName(targetFile);
				if (CreateDirectory(targetPath) == false)
				{
					Debug.LogError("Create target path failed : " + targetPath);
					return;
				}

				// Notice can not use AssetDatabase.DeleteAsset, AssetDatabase.CopyAsset
				// 1. DeleteAsset will cause GUID changed which is used in ezgui
				// 2. CopyAsset will cause a internal asset of unity.
				File.Delete(targetFile);
				File.Copy(sourceFile, targetFile);
			}
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the source directory does not exist, throw an exception.
			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			// If the destination directory does not exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}


			// Get the file contents of the directory to copy.
			FileInfo[] files = dir.GetFiles();

			foreach (FileInfo file in files)
			{
				// Create the path to the new copy of the file.
				string temppath = Path.Combine(destDirName, file.Name);

				// Copy the file.
				file.CopyTo(temppath, true);
			}

			// If copySubDirs is true, copy the subdirectories.
			if (copySubDirs)
			{

				foreach (DirectoryInfo subdir in dirs)
				{
					// Skip svn folder
					if (string.Compare(subdir.Name, ".svn", true) == 0)
						continue;

					// Create the subdirectory.
					string temppath = Path.Combine(destDirName, subdir.Name);

					// Copy the subdirectories.
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}

		private static void RemoveFile(string filePath)
		{
			if (Directory.Exists(filePath))
				Directory.Delete(filePath, true);
			else if (File.Exists(filePath))
				File.Delete(filePath);
		}

		//////////////////////////////////////////////////////////////////////////
		// Utilities
		public static void SelectUIWithTexture()
		{
			Texture selectedTex = Selection.activeObject as Texture;
			if (selectedTex == null)
			{
				EditorUtility.DisplayDialog("", "No texture selected", "OK");
				return;
			}

			string texGUID = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(selectedTex));

			GameObject root = GameObject.Find("UICamera");
			if (root == null)
			{
				EditorUtility.DisplayDialog("", "No UICamera in scene", "OK");
				return;
			}

			List<Object> controlObjects = new List<Object>();
			foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>())
				foreach (var state in sprite.States)
					foreach (var guid in state.frameGUIDs)
						if (texGUID == guid)
							if (controlObjects.Contains(sprite.gameObject) == false)
								controlObjects.Add(sprite.gameObject);

			if (controlObjects.Count != 0)
				Selection.objects = controlObjects.ToArray();
			else
				EditorUtility.DisplayDialog("SelectUIWithTexture", "Not found", "OK");
		}

		public static void SelectUIWithEmptyTexture()
		{
			GameObject root = GameObject.Find("UICamera");
			if (root == null)
			{
				EditorUtility.DisplayDialog("SelectUIWithEmptyTexture", "No UICamera in scene", "OK");
				return;
			}

			List<Object> controlObjects = new List<Object>();
			foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>())
				foreach (var state in sprite.States)
					foreach (var guid in state.frameGUIDs)
						if (guid == null || guid == "" || AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)) == null)
							if (controlObjects.Contains(sprite.gameObject) == false)
								controlObjects.Add(sprite.gameObject);

			if (controlObjects.Count != 0)
				Selection.objects = controlObjects.ToArray();
			else
				EditorUtility.DisplayDialog("SelectUIWithEmptyTexture", "Not found", "OK");
		}

		public static void SelectUIWithMaterial()
		{
			Material selectMat = Selection.activeObject as Material;
			if (selectMat == null)
			{
				EditorUtility.DisplayDialog("SelectUIWithMaterial", "No material selected", "OK");
				return;
			}

			GameObject root = GameObject.Find("UICamera");
			if (root == null)
			{
				EditorUtility.DisplayDialog("SelectUIWithMaterial", "No UICamera in scene", "OK");
				return;
			}

			List<Object> controlObjects = new List<Object>();
			foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>(true))
				if (sprite.gameObject.renderer != null)
					if (sprite.gameObject.renderer.sharedMaterial == selectMat)
						if (controlObjects.Contains(sprite.gameObject) == false)
							controlObjects.Add(sprite.gameObject);

			if (controlObjects.Count != 0)
				Selection.objects = controlObjects.ToArray();
			else
				UnityEditor.EditorUtility.DisplayDialog("SelectUIWithMaterial", "Not found", "Ok");
		}

		public static void SelectUIWithMaterialTexture()
		{
			Texture selectTex = Selection.activeObject as Texture;
			if (selectTex == null)
			{
				EditorUtility.DisplayDialog("SelectUIWithMaterialTexture", "No texture selected", "OK");
				return;
			}

			GameObject root = GameObject.Find("UICamera");
			if (root == null)
			{
				EditorUtility.DisplayDialog("SelectUIWithMaterialTexture", "No UICamera in scene", "OK");
				return;
			}

			List<Object> controlObjects = new List<Object>();
			foreach (var sprite in root.GetComponentsInChildren<Renderer>(true))
				if (sprite.gameObject.renderer != null)
					if (sprite.gameObject.renderer.sharedMaterial != null && sprite.gameObject.renderer.sharedMaterial.mainTexture == selectTex)
						if (controlObjects.Contains(sprite.gameObject) == false)
							controlObjects.Add(sprite.gameObject);


			if (controlObjects.Count != 0)
				Selection.objects = controlObjects.ToArray();
			else
				UnityEditor.EditorUtility.DisplayDialog("SelectUIWithMaterialTexture", "Not found", "Ok");
		}

		public static void SelectUIWithFont()
		{
			GameObject go = Selection.activeGameObject;
			if (go == null)
			{
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Font", "No selection", "Ok");
				return;
			}

			var font = go.GetComponent<SpriteFontBase>();
			if (font == null)
			{
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Font", "No selected font", "Ok");
				return;
			}

			GameObject cameraGO = GameObject.Find("UICamera");
			if (cameraGO == null)
			{
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Font", "No UICamera Game Object in the scene", "Ok");
				return;
			}

			List<GameObject> selections = new List<GameObject>();
			foreach (SpriteText st in cameraGO.GetComponentsInChildren<SpriteText>(true))
				if (st.fontName.Equals(font.gameObject.name))
					selections.Add(st.gameObject);

			if (selections.Count != 0)
				Selection.objects = selections.ToArray();
			else
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Font", "Not Found", "Ok");
		}

		public static void SelectUIWithTemplate()
		{
			GameObject go = Selection.activeGameObject;
			if (go == null)
			{
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Template", "No selection", "Ok");
				return;
			}

			UITemplate uiTemplate = go.GetComponent<UITemplate>();
			if (uiTemplate == null)
			{
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Template", "No selected template", "Ok");
				return;
			}

			GameObject cameraGO = GameObject.Find("UICamera");
			if (cameraGO == null)
			{
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Template", "No UICamera Game Object in the scene", "Ok");
				return;
			}

			List<GameObject> selections = new List<GameObject>();
			foreach (SpriteRoot sp in cameraGO.GetComponentsInChildren<SpriteRoot>(true))
				if (sp.templateName.Equals(uiTemplate.gameObject.name))
					selections.Add(sp.gameObject);

			foreach (SpriteText st in cameraGO.GetComponentsInChildren<SpriteText>(true))
				if (st.templateName.Equals(uiTemplate.gameObject.name))
					selections.Add(st.gameObject);

			if (selections.Count != 0)
				Selection.objects = selections.ToArray();
			else
				UnityEditor.EditorUtility.DisplayDialog("Select UI With Template", "Not Found", "Ok");
		}

		public static void SelectPackagedTextures()
		{
			GameObject root = GameObject.Find("UICamera");
			if (root == null)
			{
				EditorUtility.DisplayDialog("", "No UICamera in scene", "OK");
				return;
			}

			List<string> GUIDs = new List<string>();
			foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>(true))
				if (sprite.renderer != null && sprite.renderer.sharedMaterial != null)
					foreach (var state in sprite.States)
						for (int i = 0; i < state.frameGUIDs.Length; ++i)
							if (state.frameGUIDs[i] != "" && GUIDs.Contains(state.frameGUIDs[i]) == false)
								GUIDs.Add(state.frameGUIDs[i]);

			List<Object> objs = new List<Object>();
			List<string> names = new List<string>();
			foreach (var guid in GUIDs)
			{
				names.Add(AssetDatabase.GUIDToAssetPath(guid));
				objs.Add(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)));
			}

			Selection.objects = objs.ToArray();

			var sb = new System.Text.StringBuilder();
			names.Sort();
			foreach (var n in names)
				sb.AppendLine(n);
			Debug.Log(sb);
		}

		public static void SelectPackagedTexturesInMaterial()
		{
			GameObject root = GameObject.Find("UICamera");
			if (root == null)
			{
				EditorUtility.DisplayDialog("", "No UICamera in scene", "OK");
				return;
			}

			Material selectedMat = Selection.activeObject as Material;
			if (selectedMat == null)
			{
				EditorUtility.DisplayDialog("", "No selected material", "OK");
				return;
			}

			List<string> GUIDs = new List<string>();
			foreach (var sprite in root.GetComponentsInChildren<AutoSpriteBase>(true))
				if (sprite.renderer != null && sprite.renderer.sharedMaterial != null && sprite.renderer.sharedMaterial == selectedMat)
					foreach (var state in sprite.States)
						for (int i = 0; i < state.frameGUIDs.Length; ++i)
							if (state.frameGUIDs[i] != "" && GUIDs.Contains(state.frameGUIDs[i]) == false)
								GUIDs.Add(state.frameGUIDs[i]);

			List<Object> objs = new List<Object>();
			List<string> names = new List<string>();
			foreach (var guid in GUIDs)
			{
				names.Add(AssetDatabase.GUIDToAssetPath(guid));
				objs.Add(AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Texture)));
			}

			Selection.objects = objs.ToArray();

			var sb = new System.Text.StringBuilder();
			names.Sort();
			foreach (var n in names)
				sb.AppendLine(n);
			Debug.Log(sb);
		}

		public static void SelectEZScreenPlacement()
		{
			GameObject root = Selection.activeGameObject;
			if (root == null)
			{
				EditorUtility.DisplayDialog("SelectEZScreenPlacement", "No Selected Root", "OK");
				return;
			}

			List<Object> controlObjects = new List<Object>();
			foreach (var sp in root.GetComponentsInChildren<EZScreenPlacement>(true))
				if (controlObjects.Contains(sp.gameObject) == false)
					controlObjects.Add(sp.gameObject);

			if (controlObjects.Count != 0)
				Selection.objects = controlObjects.ToArray();
			else
				UnityEditor.EditorUtility.DisplayDialog("SelectEZScreenPlacement", "Not found", "Ok");
		}

		[MenuItem("Tools/UI/Select ScrollList with PositionItems Immediately")]
		public static void SelectScrollListPositionItemsImmediately()
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			UIScrollList[] scrolllists = uiContainer.GetComponentsInChildren<UIScrollList>();
			if (scrolllists == null)
				return;

			List<GameObject> selectedObjs = new List<GameObject>();
			foreach (var scrolllist in scrolllists)
			{
				if (scrolllist.positionItemsImmediately)
					selectedObjs.Add(scrolllist.gameObject);
			}

			Selection.objects = selectedObjs.ToArray();
		}

		[MenuItem("Tools/UI/Set EZ")]
		public static void SetEZ()
		{
			GameObject uiContainer = GetUIContainer();
			if (uiContainer == null)
			{
				Debug.LogError("uiContainer is null");
				return;
			}

			EZScreenPlacement[] ezs = uiContainer.GetComponentsInChildren<EZScreenPlacement>();
			if (ezs == null)
				return;

			List<GameObject> selectedObjs = new List<GameObject>();
			foreach (var ez in ezs)
			{
				if (ez.relativeTo.verticalScale == EZScreenPlacement.VERTICAL_SCALE.SCREEN_SPRITE_SCALE)
				{
					ez.relativeTo.verticalScale = EZScreenPlacement.VERTICAL_SCALE.SCREEN_SPRITE_DOCK;
					selectedObjs.Add(ez.gameObject);
				}
			}

			Selection.objects = selectedObjs.ToArray();
		}

		[MenuItem("Tools/UI/CheckBrokenUIPrefab")]
		public static void CheckBrokenUIPrefab()
		{
			CheckBrokenUIPrefab(sUIPrefabPath);
		}

		private static void CheckBrokenUIPrefab(string prefabPath)
		{
			DirectoryInfo dir = new DirectoryInfo(prefabPath);
			if (dir.Exists == false)
				throw new System.Exception("Invlaid UI Path");

			bool found = false;

			// Get the file contents of the directory to copy.
			foreach (FileInfo file in dir.GetFiles())
			{
				if (Path.GetExtension(file.Name).Equals(".prefab", System.StringComparison.CurrentCultureIgnoreCase) == false)
					continue;

				// Copy the file.
				var filePath = Path.Combine(prefabPath, file.Name);
				GameObject prefab = AssetDatabase.LoadAssetAtPath(filePath, typeof(GameObject)) as GameObject;
				if (prefab != null)
					continue;

				AssetDatabase.DeleteAsset(filePath);
				Debug.LogError("Delete Broken prefab" + file.Name);
				found = true;
			}

			if (found)
				throw new System.Exception("Broken prefab found");
		}
	}
}
#endif