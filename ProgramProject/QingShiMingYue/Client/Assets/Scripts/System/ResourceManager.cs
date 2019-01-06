//#define ENABLE_RESOURCE_MANAGER_LOG
//#define ENABLE_RESOURCE_MANAGER_METRICS
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using ClientServerCommon;

/// <summary>
/// 资源管理器
/// 跟进资源描述文件, 从本地(包中)或者临时目录加载资源.
/// </summary>
public class ResourceManager : SysModule
{
	/// <summary>
	/// 单件
	/// </summary>
	public static ResourceManager Instance { get { return SysModuleManager.Instance.GetSysModule<ResourceManager>(); } }

	/*
	 * Path utility
	 */
	public string GetLocalFileDirectory()
	{
#if UNITY_EDITOR
		return Path.Combine(Path.GetDirectoryName(Application.dataPath), GameDefines.assetBundleFolder);
#elif UNITY_IPHONE || UNITY_ANDROID
		return Path.Combine(Application.temporaryCachePath, GameDefines.assetBundleFolder);
#else
		return GameDefines.assetBundleFolder;
#endif
	}

	public string GetLocalFilePath(string file)
	{
		return System.IO.Path.Combine(GetLocalFileDirectory(), file);
	}

	public string GetLocalFileUrl(string file)
	{
		return "file://" + GetLocalFilePath(file);
	}

	/*
	 * Upgrading Resource 
	 */
	public bool CheckIfNeedUpdate()
	{
		return false;
	}

	/*
	 * Loading Resource 
	 */
	public Object LoadAsset(string assetName, bool noCache)
	{
#if ENABLE_RESOURCE_MANAGER_METRICS
		totalLoadCount++;
#endif

		// Convert to lowercase name
		assetName = KodGames.PathUtility.UnifyPath(assetName);

		// Check if Need load from cache
		if (ConfigDatabase.DefaultCfg.ClientManifest != null)
		{
			ClientManifest.FileInfo fileInfo = ConfigDatabase.DefaultCfg.ClientManifest.GetFileByName(assetName);
			if (fileInfo != null)
			{
#if ENABLE_RESOURCE_MANAGER_METRICS
				assetBundleLoadCount++;
#endif

				// Try to load from resource cache
				ResourceCache resCache = ResourceCache.Instance;
				if (resCache != null && resCache.Contains(assetName))
				{
					return resCache.GetAsset(assetName);
				}

				// Load from asset bundle and add to resource cache				
				string abPath = GetLocalFilePath(fileInfo.fileName);
#if ENABLE_RESOURCE_MANAGER_LOG
				Debug.Log("[ResourceManager] Load asset from AB : " + abPath);
#endif

				AssetBundle ab = AssetBundle.CreateFromFile(abPath);
				if (ab == null)
				{
					Debug.LogError("Load AB failed : " + abPath);
					return null;
				}

				// Load object from AB
				Object obj = ab.mainAsset;
				ab.Unload(false);
				ab = null;

				// Add to resource cache
				if (resCache != null && noCache == false)
				{
					resCache.AddAsset(assetName, obj);
					return resCache.GetAsset(assetName);
				}
				else
				{
					return obj;
				}
			}
		}

#if ENABLE_RESOURCE_MANAGER_METRICS
		resourceLoadCount++;
#endif

		// Load from resources folder
		return ResourcesWrapper.Load(assetName);
	}

	public Object LoadAsset(string assetName)
	{
		return LoadAsset(assetName, false);
	}

	public T LoadAsset<T>(string assetName, bool noCache) where T : Object
	{
		return LoadAsset(assetName, noCache) as T;
	}

	public T LoadAsset<T>(string assetName) where T : Object
	{
		return LoadAsset<T>(assetName, false);
	}

	public T InstantiateAsset<T>(string assetName) where T : Object
	{
		T t = LoadAsset<T>(assetName);
		if (t == null)
			return null;

		return Object.Instantiate(t) as T;
	}

	//public void ReleaseAsset<T>(T asset) where T : Object
	//{
	//    if (resourceCache != null)
	//        resourceCache.ReleaseAssetRef(asset);
	//}

	public WWW LoadStreamingTexture(string textureName)
	{
		// Convert to lowercase name
		string assetName = KodGames.PathUtility.UnifyPath(textureName);

		// Check if Need load from cache
		if (ConfigDatabase.DefaultCfg.ClientManifest != null)
		{
			ClientManifest.FileInfo fileInfo = ConfigDatabase.DefaultCfg.ClientManifest.GetFileByName(assetName);
			if (fileInfo != null)
			{
				string streamingFilePath = GetLocalFileUrl(fileInfo.fileName);
#if ENABLE_RESOURCE_MANAGER_LOG
				Debug.Log("[ResourceManager] Load asset from streaming asset : " + streamingFilePath);
#endif

				return new WWW(streamingFilePath);
			}
		}

		// print the path to the streaming assets folder
		var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, textureName);
		if (filePath.Contains("://"))
			return new WWW(filePath);
		else
			return new WWW("file://" + filePath);
	}

	public WWW LoadStreamingAudio(string audioName)
	{
		audioName = KodGames.PathUtility.UnifyPath(audioName + GameDefines.audioFormat, false);

		if (ConfigDatabase.DefaultCfg.ClientManifest != null)
		{
			ClientManifest.FileInfo fileInfo = ConfigDatabase.DefaultCfg.ClientManifest.GetFileByName(audioName.ToLower());
			if (fileInfo != null)
			{
				string streamingFilePath = GetLocalFileUrl(fileInfo.fileName);
#if ENABLE_RESOURCE_MANAGER_LOG
			Debug.Log("[ResourceManager] Load asset from streaming asset : " + streamingFilePath);
#endif
				return new WWW(streamingFilePath);
			}
		}

		var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, audioName);

		if (filePath.Contains("://"))
			return new WWW(filePath);
		else
			return new WWW("file://" + filePath);
	}

#if ENABLE_RESOURCE_MANAGER_METRICS
	private int totalLoadCount = 0;
	private int resourceLoadCount = 0;
	private int assetBundleLoadCount = 0;

	public override void OnGUIUpdate()
	{
		base.OnGUIUpdate(); 
		
		GUILayout.BeginVertical();
		GUILayout.Label("------ ResourceManager ------");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Total Load Count : ");
		GUILayout.Label(totalLoadCount.ToString());
		GUILayout.EndHorizontal(); 
		GUILayout.BeginHorizontal();
		GUILayout.Label("Resource Load Count : ");
		GUILayout.Label(resourceLoadCount.ToString());
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("AssetBundle Load Count : ");
		GUILayout.Label(assetBundleLoadCount.ToString());
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
	}
#endif
}
