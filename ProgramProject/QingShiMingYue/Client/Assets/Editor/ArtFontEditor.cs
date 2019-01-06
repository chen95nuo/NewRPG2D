//#define ArtFontEditor_Log
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

[AddComponentMenu("Tools")]
public class ArtFontEditor : EditorWindow
{
	class ToolBar
	{
		string[] content;
		float height = 20f;
		int selected = 0;
		public int Selected
		{
			get
			{
				return this.selected;
			}
		}
		public ToolBar(string[] content, float height)
		{
			this.content = content;
			this.height = height;
		}

		public void OnEditorWindowGUI()
		{
			selected = GUILayout.Toolbar(selected, content, GUILayout.Height(height));
		}
	}

	[MenuItem("Tools/ArtfontTextureEditor %t")]
	static void ShowEditor()
	{
		inst = EditorWindow.GetWindow<ArtFontEditor>("ArtFontTex");
		inst.CheckPath(inst.TexSplitPath);
		inst.CheckPath(inst.TexToCombinePath);
		inst.CheckPath(inst.CombinedFileDestiFolderAbstract);
	}

	static ArtFontEditor inst = null;

	SpriteFont spriteFont;
	string texSplitPath = "WorkAssets/ArtFontEditor/";
	string TexSplitPath { get { return Application.dataPath + "/" + texSplitPath; } }
	//string TexSplitPath { get { return KodGames.PathUtility.Combine(Application.dataPath, texSplitPath); } }

	string texToCombinePath = "WorkAssets/ArtFontEditor/";
	string TexToCombinePath { get { return Application.dataPath + "/" + texToCombinePath; } }
	//string TexToCombinePath { get { return KodGames.PathUtility.Combine(Application.dataPath, texToCombinePath); } }

	string combinedFileDestiFolder = "WorkAssets/ArtFontEditor/Combined/";
	string CombinedFileDestiFolderAbstract { get { return Application.dataPath + "/" + combinedFileDestiFolder; } }
	//string CombinedFileDestiFolder { get { return KodGames.PathUtility.Combine(Application.dataPath, combinedFileDestiFolder); } }

	string combinedFileName = "combinedFontTexture";

	GUILayoutOption ctrlHeight = GUILayout.Height(20);
	GUILayoutOption btnHeight = GUILayout.Height(25);

	ToolBar topBar = new ToolBar(new string[] { "Split Texture", "Combine Textures", "帮助" }, 25);

	void OnGUI()
	{
		this.spriteFont = EditorGUILayout.ObjectField("SpriteFontPrefab", this.spriteFont, typeof(SpriteFont), true, ctrlHeight) as SpriteFont;
		topBar.OnEditorWindowGUI();
		EditorGUILayout.Space();

		if (topBar.Selected == 0)
			OnSplitTextureEditorGUI();
		else if (topBar.Selected == 1)
			OnCombineTextureEditorGUI();
		else if (topBar.Selected == 2)
			OnHelpGUI();
	}

	int selectedPage = 0;
	Vector2 textureScrollPosi = Vector2.zero;
	void OnSplitTextureEditorGUI()
	{
		this.texSplitPath = EditorGUILayout.TextField("保存路径", this.texSplitPath, ctrlHeight);
		EditorGUILayout.Space();

		Texture2D fontTexture = null;

		List<Texture2D> fontTextures = new List<Texture2D>();

		if (spriteFont != null)
		{
			for (int i = 0; i < spriteFont.GetMaterialCount(); i++)
				fontTextures.Add(spriteFont.GetMaterial(i).mainTexture as Texture2D);

			string[] pop = new string[fontTextures.Count];
			for (int i = 0; i < pop.Length; i++)
				pop[i] = "Page " + i;

			if (selectedPage >= pop.Length)
				selectedPage = pop.Length - 1;

			selectedPage = GUILayout.Toolbar(selectedPage, pop, ctrlHeight);
			fontTexture = spriteFont.GetMaterial(selectedPage).mainTexture as Texture2D;

			textureScrollPosi = EditorGUILayout.BeginScrollView(textureScrollPosi, false, false);
			{
				Rect area = GUILayoutUtility.GetRect(fontTexture.width, fontTexture.height);
				area.width = fontTexture.width;
				area.height = fontTexture.height;
				GUI.DrawTexture(area, fontTexture);
			}
			EditorGUILayout.EndScrollView();
		}

		if (fontTexture != null)
		{
			GUI.backgroundColor = Color.green;

			if (GUILayout.Button("全部拆分", btnHeight))
				SplitFontTexture(fontTextures);

			GUI.backgroundColor = Color.white;
		}
	}

	void OnCombineTextureEditorGUI()
	{
		this.texToCombinePath = EditorGUILayout.TextField("单个字体文件文件夹", this.texToCombinePath, ctrlHeight);
		this.combinedFileDestiFolder = EditorGUILayout.TextField("合并后的文件保存路径", this.combinedFileDestiFolder, ctrlHeight);

		EditorGUILayout.Space();

		this.combinedFileName = EditorGUILayout.TextField("合并后的文件名", this.combinedFileName, ctrlHeight);
		if (string.IsNullOrEmpty(this.combinedFileName) || !IsValidFileName(this.combinedFileName))
		{
			EditorGUILayout.HelpBox("文件名无效！", MessageType.Error);
			return;
		}

		EditorGUILayout.Space();

		if (spriteFont != null)
		{
			GUI.backgroundColor = Color.green;
			if (GUILayout.Button("合并Texture", btnHeight))
				CombineFontTextures();

			GUI.backgroundColor = Color.white;
		}
	}

	Vector2 helpScrollPosi = Vector2.zero;
	void OnHelpGUI()
	{
		helpScrollPosi = EditorGUILayout.BeginScrollView(helpScrollPosi, false, false);
		EditorGUILayout.Space();
		GUILayout.Label("本工具用于将艺术字图片文件按字符拆分成多个单独的字体图片，为png格式。");
		GUILayout.Label("若要拆分文件，请单击\"Split Texture\",若要合并文件，请单击\"Combine Textures\"");
		GUILayout.Label("注意不能修改程序生成的单个字体文件的文件名。命名规则为： 字符_字体描述文件中的字符id_贴图序号.png");
		GUILayout.Label("对单个字体文件修改完成后，使用合并文件功能，自动将指定文件夹内的单个png文件合成一张完整的字体文件");
		EditorGUILayout.Space();

		GUILayout.Label("拆分文件");
		GUILayout.Label("首先，为编辑器顶部的\"SpritFontPrefab\"拖入字体。在某一场景中拖入UIFontManager，建议使用UICommon场景，\n例如，在UICommon中将UIFontManager的子节点ArtFont拖入，编辑器便能够获取改字体的所有信息。就可以进行字体拆分了,合并文件同理。");

		EditorGUILayout.Space();
		GUILayout.Label("文件路径设置，建议使用默认路径");
		GUILayout.Label("可以手动设置各项路径，需要注意记住你所设置的路径\n更改了拆分后的文件所在的路径，那么合并这个字体时也要正确的设置需要合并的文件所在的文件夹路径");

		EditorGUILayout.Space();
		GUILayout.Label("注意");
		//EditorGUILayout.HelpBox("每次用工具重新生成艺术字并导入Unity后，必须重新进入UICommon，否则字体描述信息不能更新，拆图会出错", MessageType.Warning);
		EditorGUILayout.HelpBox("工具尽量保证保持图片的导入格式，但请在合并后重新确认或设置字体图片的导入格式", MessageType.Warning);
		EditorGUILayout.HelpBox("由于Unity的限制，工具合并后的图片为RGBA32格式，不能为RGBA4444(16bit)格式（不支持像素操作）", MessageType.Warning);

		EditorGUILayout.EndScrollView();
	}

	#region 逻辑处理

	void SplitFontTexture(List<Texture2D> fontTextures)
	{
		CheckPath(TexSplitPath);
		CheckPath(TexToCombinePath);
		CheckPath(CombinedFileDestiFolderAbstract);

		DeleteAllBuiltTextures(this.TexSplitPath, SearchOption.TopDirectoryOnly);

		//Reload. In case that font defination is regenerated by tools.
		//And if the scene is not reloaded, the spriteFont will not be updated.
		this.spriteFont.Awake();

		List<Texture2D> runTimeFontTextures = new List<Texture2D>();
		List<Texture2D> nonReadable = new List<Texture2D>();

		for (int i = 0; i < fontTextures.Count; i++)
		{
			Texture2D fontTexture = fontTextures[i];
			// Re-import, Make texture readable
			string texturePath = AssetDatabase.GetAssetPath(fontTexture);
			TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(texturePath);

			if (!importer.isReadable)
			{
				nonReadable.Add(fontTexture);
				// Reimport it with isReadable
				importer.isReadable = true;
				AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceSynchronousImport);
			}

			Texture2D runtimeTexture = new Texture2D(fontTexture.width, fontTexture.height, TextureFormat.RGBA32, false);
			runtimeTexture.SetPixels32(fontTexture.GetPixels32());
			runtimeTexture.Apply();
			runTimeFontTextures.Add(runtimeTexture);
		}

		//[charCode,charIndex]
		Dictionary<int, int> charDic = this.spriteFont.CharMap;
		//[charIndex]=FontChar
		List<SpriteChar> charList = new List<SpriteChar>(this.spriteFont.Chars);
		Dictionary<int, List<SpriteChar>> pageGroupedChars = new Dictionary<int, List<SpriteChar>>();
		foreach (var spriteChar in charList)
		{
			if (spriteChar == null)
				continue;

			if (!pageGroupedChars.ContainsKey(spriteChar.page))
				pageGroupedChars.Add(spriteChar.page, new List<SpriteChar>());

			pageGroupedChars[spriteChar.page].Add(spriteChar);
		}

		try
		{
			foreach (var page in pageGroupedChars)
			{
				Texture2D thisPageTex = runTimeFontTextures[page.Key];
				List<SpriteChar> thisPageChars = page.Value;

				foreach (var spriteChar in thisPageChars)
				{
					string fileName = Convert.ToChar(spriteChar.id).ToString();

					if (!IsValidFileName(fileName))
						fileName = "无效文件名";

					//字符+字符Uniicode+所在贴图序号
					//逻辑修改：字体生成工具不是每次都把同一个字符放到相同的page里，命名里不能包含页码.
					fileName += "_" + spriteChar.id + ".png";

					int texWidth = thisPageTex.width;
					int texHeight = thisPageTex.height;

					int x = Mathf.RoundToInt(spriteChar.UVs.x * texWidth);
					int y = Mathf.RoundToInt(spriteChar.UVs.y * texHeight);
					int width = Mathf.RoundToInt((spriteChar.UVs.xMax - spriteChar.UVs.x) * texWidth);
					int height = Mathf.RoundToInt((spriteChar.UVs.yMax - spriteChar.UVs.y) * texHeight);

					Texture2D singleCharPng = new Texture2D(width, height, TextureFormat.RGBA32, false);
					singleCharPng.SetPixels(thisPageTex.GetPixels(x, y, width, height));
					singleCharPng.Apply();

					// needs to be ARGB32, RGBA32, BGRA32, RGB24, Alpha8 or DXT
					byte[] singleCharFile = singleCharPng.EncodeToPNG();

					DestroyImmediate(singleCharPng);

					File.WriteAllBytes(TexSplitPath + fileName, singleCharFile);
				}
			}
		}
		finally
		{
			foreach (var temp in runTimeFontTextures)
				GameObject.DestroyImmediate(temp);

			runTimeFontTextures.Clear();

			//Revert readable settings.
			foreach (var temp in nonReadable)
			{
				string texturePath = AssetDatabase.GetAssetPath(temp);
				TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(texturePath);
				importer.isReadable = false;
				AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceSynchronousImport);
			}

			nonReadable.Clear();

			AssetDatabase.Refresh();
		}
	}

	void CombineFontTextures()
	{
		CheckPath(TexSplitPath);
		CheckPath(TexToCombinePath);
		CheckPath(CombinedFileDestiFolderAbstract);

		//Reload. In case that font defination is regenerated by tools.
		//And if the scene is not reloaded, the spriteFont will not be updated.
		this.spriteFont.Awake();

		//[charCode,charIndex]
		var charDic = this.spriteFont.CharMap;
		//[charIndex]=FontChar
		var charArray = this.spriteFont.Chars;

		Dictionary<int, List<KeyValuePair<int, Texture2D>>> pageGroupedCharTexes = new Dictionary<int, List<KeyValuePair<int, Texture2D>>>();
		List<string> combinedTexPaths = new List<string>();

		string[] allFiles = Directory.GetFiles(TexToCombinePath, "*.png", SearchOption.TopDirectoryOnly);
		foreach (string filename in allFiles)
		{
			Texture2D texture = new Texture2D(0, 0);
			texture.LoadImage(File.ReadAllBytes(filename));
			int code = GetCharCodeFromFileName(filename);

			if (!charDic.ContainsKey(code))
			{
				EditorUtility.DisplayDialog("注意", "发现文件" + filename + "\n但是字体配置里面并没有文字 \"" + Convert.ToChar(code) + "\"", "OK");
				continue;
			}

			SpriteChar spriteChar = charArray[charDic[code]];
			if (!pageGroupedCharTexes.ContainsKey(spriteChar.page))
				pageGroupedCharTexes.Add(spriteChar.page, new List<KeyValuePair<int, Texture2D>>());

			pageGroupedCharTexes[spriteChar.page].Add(new KeyValuePair<int, Texture2D>(charDic[code], texture));
		}

		int texWidth = this.spriteFont.TexWidth;
		int texHeight = this.spriteFont.TexHeight;

		foreach (var page in pageGroupedCharTexes)
		{
			Texture2D newCombined = new Texture2D(texWidth, texHeight, TextureFormat.RGBA32, false);

			Color[] alpha = new Color[texWidth * texHeight];
			for (int i = 0; i < alpha.Length; i++)
				alpha[i] = new Color(1, 1, 1, 0);

			newCombined.SetPixels(alpha);

			var sprites = page.Value;
			foreach (KeyValuePair<int, Texture2D> pair in sprites)
			{
				SpriteChar spriteChar = charArray[pair.Key];
				Texture2D charTex = pair.Value;

				int x = Mathf.RoundToInt(spriteChar.UVs.x * texWidth);
				int y = Mathf.RoundToInt(spriteChar.UVs.y * texHeight);
				int width = Mathf.RoundToInt((spriteChar.UVs.xMax - spriteChar.UVs.x) * texWidth);
				int height = Mathf.RoundToInt((spriteChar.UVs.yMax - spriteChar.UVs.y) * texHeight);

#if ArtFontEditor_Log
			Debug.Log(string.Format("[Combine] id={0}  x={1} y={2} width={3} height={4}", pair.Key, x, y, width, height));
#endif
				newCombined.SetPixels(x, y, width, height, charTex.GetPixels());
			}

			string fileFullPath = this.CombinedFileDestiFolderAbstract + this.combinedFileName + " " + page.Key + ".png";

			if (File.Exists(fileFullPath))
			{
				if (!EditorUtility.DisplayDialog("注意", "文件:" + fileFullPath + "已经存在，是否覆盖？", "覆盖", "取消"))
					continue;
			}

			newCombined.Apply();
			File.WriteAllBytes(fileFullPath, newCombined.EncodeToPNG());
			DestroyImmediate(newCombined);

			combinedTexPaths.Add("Assets/" + this.combinedFileDestiFolder + this.combinedFileName + " " + page.Key + ".png");
		}

		//确保新生成的图片能够被Unity识别（如果原来不存在的话）
		AssetDatabase.Refresh();
		TextureImporter originalImporter = (TextureImporter)TextureImporter.GetAtPath(AssetDatabase.GetAssetPath(spriteFont.GetMaterial(0).mainTexture as Texture2D));

		foreach (string assetPath in combinedTexPaths)
		{
			Debug.Log("combined " + assetPath);

			TextureImporter importer = (TextureImporter)TextureImporter.GetAtPath(assetPath);
			importer.textureType = originalImporter.textureType;
			importer.textureFormat = originalImporter.textureFormat;
			importer.isReadable = originalImporter.isReadable;
			importer.npotScale = originalImporter.npotScale;
			AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
		}
		AssetDatabase.Refresh();

		EditorUtility.DisplayDialog("提示", "合并完成！", "OK");
	}

	int GetCharCodeFromFileName(string fileName)
	{
		try
		{
			//路径中可能含有'_'，取分割后的最后一个字符串
			//a_65.png
			string[] splited1 = fileName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
			return int.Parse(splited1[splited1.Length - 1].Split(new char[] { '.' })[0]);
		}
		catch (System.Exception ex)
		{
			Debug.LogError("解析文件名错误 fileName=" + fileName);
			throw ex;
		}
	}

	/// <summary>
	/// 检查文件名是否合法
	/// </summary>
	/// <param name="fileName">文件名,不包含路径</param>
	private bool IsValidFileName(string fileName)
	{
		bool isValid = true;
		//Unity不支持空格开头
		string errChar = " \\/:*?\"<>|";  //

		if (string.IsNullOrEmpty(fileName))
		{
			isValid = false;
		}
		else
		{
			for (int i = 0; i < errChar.Length; i++)
			{
				if (fileName.Contains(errChar[i].ToString()))
				{
					isValid = false;
					break;
				}
			}
		}
		return isValid;
	}

	void CheckPath(string path)
	{
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
			AssetDatabase.Refresh();
		}
	}

	//删除之前构建的所有文件
	void DeleteAllBuiltTextures(string path, SearchOption deleteOpinion)
	{
		CheckPath(path);

		string[] files = Directory.GetFiles(path, "*.png", deleteOpinion);

		foreach (var file in files)
			File.Delete(file);

		AssetDatabase.Refresh();
	}

	#endregion
}
