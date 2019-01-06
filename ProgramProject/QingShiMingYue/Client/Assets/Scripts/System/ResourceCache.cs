﻿//#define ENABLE_RESOURCE_CACHE_LOG
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 资源缓存器
/// </summary>
public class ResourceCache : SysModule
{
	public static ResourceCache Instance { get { return SysModuleManager.Instance.GetSysModule<ResourceCache>(); } }

	private class CachedAsset
	{
		public string assetName;
		public Object obj;
		public int refCount;
	}

	private Dictionary<string, CachedAsset> cachedAssets = new Dictionary<string, CachedAsset>();

	public bool Contains(string assetName)
	{
		return cachedAssets.ContainsKey(assetName);
	}

	public void AddAsset(string assetName, Object obj) 
	{
#if ENABLE_RESOURCE_CACHE_LOG
		Debug.Log("[ResourceCache] Add asset to cache : " + assetName);
#endif
		Debug.Assert(Contains(assetName) == false);

		CachedAsset cachedAsset = new CachedAsset();
		cachedAsset.assetName = assetName;
		cachedAsset.obj = obj;
		cachedAsset.refCount = 0;

		cachedAssets.Add(assetName, cachedAsset);
	}

	public Object GetAsset(string assetName) 
	{
#if ENABLE_RESOURCE_CACHE_LOG
		Debug.Log("[ResourceCache] Load asset from cache : " + assetName);
#endif
		CachedAsset cachedAsset = null;
		if (cachedAssets.TryGetValue(assetName, out cachedAsset) == false)
			return null;

		return cachedAsset.obj;
	}

	public void FreeCache()
	{
#if ENABLE_RESOURCE_CACHE_LOG
		Debug.Log("[ResourceCache] Free Cache");
#endif

		foreach (var kvp in cachedAssets)
#if UNITY_EDITOR
			Object.DestroyImmediate(kvp.Value.obj, true);
#else
			Object.Destroy(kvp.Value.obj);
#endif

		cachedAssets.Clear();
	}
}
