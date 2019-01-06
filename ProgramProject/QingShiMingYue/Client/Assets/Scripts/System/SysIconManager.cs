#define DELAY_LOAD_ICON_TEXTUER
using UnityEngine;
using System.Collections.Generic;
using KodGames;
using ClientServerCommon;

public interface IIconBase
{
	void OnDestroy();
	AutoSpriteControlBase GetControl();
}

// Avatar creator.
public class SysIconManger : SysModule
{
	private class MaterialData
	{
		public string textureName;
		public int referenceCount;
		public Material material;
	}
	
	private Dictionary<string, MaterialData> iconMaterialDict = new Dictionary<string, MaterialData>();

	public static SysIconManger Instance
	{
		get { return SysModuleManager.Instance.GetSysModule<SysIconManger>(); }
	}

	public override bool Initialize()
	{
#if !DELAY_LOAD_ICON_TEXTUER
		// Load all icon textures in config setting
		for (int i = 0; i < ConfigDatabase.DefaultCfg.itemDescConfig.GetIconTextureCount(); ++i)
			LoadIconTexture(ConfigDatabase.DefaultCfg.itemDescConfig.GetIconTexture(i));
#endif

		return true;
	}

	public override void Dispose()
	{
		// Common out, Have no idea to determine local asset, Now it will be ok to not free this texture,
		// it will only cause memory leak when quit application.
		foreach (var kvp in iconMaterialDict)
		{
			Resources.UnloadAsset(kvp.Value.material.mainTexture);
			//			Resources.UnloadAsset(kvp.Value.material);
		}

		iconMaterialDict.Clear();
	}

	public bool IsIconTextureLoaded(string textureName)
	{
		string lowerName = textureName.ToLower();
		return iconMaterialDict.ContainsKey(lowerName);
	}

	public void LoadIconTexture(string textureName)
	{
		string lowerName = textureName.ToLower();

		// Check if the texture is loaded
		if (iconMaterialDict.ContainsKey(lowerName))
			return;

		// Create the material
		Material material = new Material(Shader.Find("Kod/UI/Transparent Color"));
		material.mainTexture = ResourceManager.Instance.LoadAsset<Texture2D>(PathUtility.Combine(ConfigDatabase.DefaultCfg.AssetDescConfig.iconPath, lowerName), true);

		// Add to library
		MaterialData materialData = new MaterialData();
		materialData.textureName = lowerName;
		materialData.referenceCount = 0;
		materialData.material = material;

		iconMaterialDict.Add(lowerName, materialData);
	}

	public void AddIconTexture(string textureName, WWW wwwLoader)
	{
		string lowerName = textureName.ToLower();

		// Check if the texture is loaded
		if (iconMaterialDict.ContainsKey(lowerName))
			return;

		// Load the texture from WWW
		Texture2D texture = null;
		if (ImageFileLoader.IsPNGFile(wwwLoader.bytes) || ImageFileLoader.IsJPEGFile(wwwLoader.bytes))
		{
			texture = new Texture2D(2, 2);
			wwwLoader.LoadImageIntoTexture(texture);
		}
		else if (ImageFileLoader.IsBMPFile(wwwLoader.bytes))
		{
			texture = ImageFileLoader.LoadToTextureBMP(wwwLoader.bytes);
			if (texture == null)
				return;
		}
		else if (texture == null)
		{
			//			string filePath = Application.persistentDataPath + "/" + lowerName.GetHashCode();
			//			System.IO.File.WriteAllBytes(filePath, wwwLoader.bytes);

			Debug.LogError(string.Format("Unsupported icon file format, byte size:{1}, {0}", textureName, wwwLoader.bytes.Length));
			return;
		}

		// Create material
		Material material = new Material(Shader.Find("Extra/UITransparent"));
		material.mainTexture = texture;

		// Add to library
		MaterialData materialData = new MaterialData();
		materialData.textureName = lowerName;
		materialData.referenceCount = 0;
		materialData.material = material;

		iconMaterialDict.Add(lowerName, materialData);
	}

	public bool SetupControlIcon(IIconBase icon, string textureName)
	{
		return SetupControlIcon(icon.GetControl(), textureName, new Rect(0, 0, 1, 1));
	}

	public bool SetupControlIcon(IIconBase icon, AssetDescConfig.AssetDesc assetDesc)
	{
		return SetupControlIcon(icon.GetControl(), assetDesc.icon, ClientServerCommon.Converter.ToRect(assetDesc.uv));
	}

	public bool SetupControlIcon(IIconBase icon, string textureName, Rect uvRect)
	{
		return SetupControlIcon(icon.GetControl(), textureName, uvRect);
	}

	private bool SetupControlIcon(AutoSpriteControlBase control, string textureName, Rect uvRect)
	{
		if (control == null)
			return false;

		string lowerName = textureName.ToLower();
		if (iconMaterialDict.ContainsKey(lowerName) == false)
		{
#if DELAY_LOAD_ICON_TEXTUER
			LoadIconTexture(textureName);
			if (iconMaterialDict.ContainsKey(lowerName) == false)
				return false;
#else
			return false;
#endif
		}

		if (control == null)
			return false;

		// Save old icon for release
		Material oldMat = control.renderer.sharedMaterial;

		// Setup new icon
		UIUtility.SetIconInUVUnit(control, iconMaterialDict[lowerName].material, uvRect);

		// Add reference count
		iconMaterialDict[lowerName].referenceCount++;
		// Debug.Log(string.Format("SetupIcon:{0},Ref:{1}", iconMaterialDict[lowerName].textureName, iconMaterialDict[lowerName].referenceCount));

		// Release old icon
		ReleaseIcon(oldMat);

		return true;
	}

	public void ReleaseIcon(IIconBase icon)
	{
		if (icon == null || icon.GetControl() == null)
			return;

		var iconRenderer = icon.GetControl().renderer;
		if (iconRenderer != null)
		{
			ReleaseIcon(iconRenderer.sharedMaterial);
			iconRenderer.sharedMaterial = null;
		}
	}

	private void ReleaseIcon(Material mat)
	{
		foreach (var kvp in iconMaterialDict)
		{
			if (kvp.Value.material == mat)
			{
				kvp.Value.referenceCount--;
				//				Debug.Log(string.Format("ReleaseIcon:{0},Ref:{1}", kvp.Value.textureName, kvp.Value.referenceCount));

				break;
			}
		}
	}

	public void DestroyUnusedIcon()
	{
		List<string> deletingMaterials = new List<string>();
		foreach (var kvp in iconMaterialDict)
			if (kvp.Value.referenceCount == 0)
				deletingMaterials.Add(kvp.Key);

		foreach (var name in deletingMaterials)
		{
			MaterialData matData = iconMaterialDict[name];
			Resources.UnloadAsset(matData.material.mainTexture);
			//			Resources.UnloadAsset(matData.material);
			iconMaterialDict.Remove(name);
		}

#if UNITY_EDITOR
		if (deletingMaterials.Count != 0)
			Debug.Log(string.Format("Destroy unused icon : {0}", deletingMaterials.Count));
#endif
	}
}