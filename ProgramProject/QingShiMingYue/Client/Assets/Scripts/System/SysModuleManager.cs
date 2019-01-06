using UnityEngine;
using System;
using System.Collections.Generic;

public class SysModuleManager
{
	private static SysModuleManager instance = null;
	public static SysModuleManager Instance
	{
		get
		{
			if (instance == null)
				instance = new SysModuleManager();
			
			return instance;
		}
	}

	// System unit to store the manager info.
	public class SysMdlUnit
	{
		public SysModule mdl; // SysModule instance.
		public bool prs; // Persistent flag to specify this instance can't be destroyed during changing state. 
	}

	private GameObject rootGameObject;
	public List<SysMdlUnit> modules = new List<SysMdlUnit>();
	private Dictionary<System.Type, SysMdlUnit> type_moduleMap = new Dictionary<System.Type, SysMdlUnit>(); // System modules.

	protected SysModuleManager()
	{

	}

	public void Initialize(GameObject rootGameObject)
	{
		this.rootGameObject = rootGameObject;
	}

	public void OnUpdate()
	{
		// Update all system modules.
		for (int i = 0; i < modules.Count; ++i)
		{
			var module = modules[i];
			if (module.mdl != null)
				module.mdl.OnUpdate();
		}
	}

	public void UpdateGUI()
	{
		// Update all system modules.
		for (int i = 0; i < modules.Count; ++i)
		{
			var module = modules[i];
			if (module.mdl != null)
				module.mdl.OnGUIUpdate();
		}
	}

	public T AddSysModule<T>(bool persistent) where T : SysModule
	{
		Type t = typeof(T);

		if (type_moduleMap.ContainsKey(t))
			return type_moduleMap[t].mdl as T;

		SysMdlUnit newSysMldUnit = new SysMdlUnit();
		modules.Add(newSysMldUnit);
		type_moduleMap.Add(t, newSysMldUnit);

		newSysMldUnit.prs = persistent;
		newSysMldUnit.mdl = rootGameObject.AddComponent<T>();
		newSysMldUnit.mdl.Initialize();
		
		return newSysMldUnit.mdl as T;
	}

	public bool DelSysModule<T>() where T : SysModule
	{
		Type t = typeof(T);

		if (!type_moduleMap.ContainsKey(t))
		{
			List<Type> delList = new List<Type>();

			// Search for derived class
			foreach (var kvp in type_moduleMap)
			{
				if (kvp.Key.IsSubclassOf(t))
					delList.Add(kvp.Key);
			}

			foreach (var type in delList)
			{
				var module = type_moduleMap[type];
				module.mdl.Dispose();
				GameObject.Destroy(module.mdl);
				modules.Remove(module);
				type_moduleMap.Remove(t);
			}

			return delList.Count != 0;
		}
		else
		{
			var module = type_moduleMap[t];
			module.mdl.Dispose();
			GameObject.Destroy(module.mdl);
			modules.Remove(module);
			type_moduleMap.Remove(t);

			return true;
		}
	}

	public T GetSysModule<T>() where T : SysModule
	{
		Type t = typeof(T);

		if (!type_moduleMap.ContainsKey(t))
		{
			// Search for derived class
			for (int i = 0; i < modules.Count; ++i)
			{
				var module = modules[i];
				if (module.mdl.GetType().IsSubclassOf(t))
					return (T)(module.mdl);
			}

			return null;
		}

		return (T)(type_moduleMap[t].mdl);
	}

	public void DisposeSysMdls(bool includePersistent)
	{
		if (includePersistent)
		{
			// Dispose system modules.
			foreach (var kvp in type_moduleMap)
			{
				kvp.Value.mdl.Dispose();
				GameObject.Destroy(kvp.Value.mdl);
			}

			modules.Clear();
			type_moduleMap.Clear();
		}
		else
		{
			// Disposed candidates.
			List<System.Type> dsps = new List<System.Type>();

			// Find disposed module.
			foreach (var kvp in type_moduleMap)
			{
				if (!kvp.Value.prs)
					dsps.Add(kvp.Key);
			}

			// Dispose system modules.
			foreach (var key in dsps)
			{
				var module = type_moduleMap[key];
				module.mdl.Dispose();
				GameObject.Destroy(module.mdl);
				modules.Remove(module);
				type_moduleMap.Remove(key);
			}
		}
	}
}
