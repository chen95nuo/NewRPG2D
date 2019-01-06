using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIModulePoolManager : MonoBehaviour
{
	private Dictionary<System.Type, List<UIModule>> moduleContainerMap = new Dictionary<System.Type, List<UIModule>>();
	private Dictionary<System.Type, List<UIModule>> moduleShowMap = new Dictionary<System.Type, List<UIModule>>();

	public List<UIModule> GetShownUIModules(System.Type type)
	{
		List<UIModule> mdls = null;
		if (moduleShowMap.TryGetValue(type, out mdls) == false)
			return new System.Collections.Generic.List<UIModule>();

		return mdls;
	}

	public List<UIModule> GetHidenUIModules(System.Type type)
	{
		List<UIModule> mdls = null;
		if (moduleContainerMap.TryGetValue(type, out mdls) == false)
			return new System.Collections.Generic.List<UIModule>();

		return mdls;
	}

	public List<UIModule> GetShowModules(System.Type type)
	{
		List<UIModule> showMdls = null;
		if (moduleShowMap.TryGetValue(type, out showMdls) == false)
			return new List<UIModule>();

		return showMdls;
	}

	public List<UIModule> GetAllShownModules()
	{
		List<UIModule> allMdls = new List<UIModule>();
		foreach (KeyValuePair<System.Type, List<UIModule>> type_MdlsPair in moduleContainerMap)
		{
			foreach (UIModule mdl in type_MdlsPair.Value)
			{
				allMdls.Add(mdl);
			}
		}

		return allMdls;
	}

	public UIModule AllocateItem(System.Type type)
	{
		UIModule uiModule = SysUIEnv.Instance.GetUIModule(type);

		List<UIModule> moduleContainer = null;
		if (moduleContainerMap.TryGetValue(type, out moduleContainer) == false)
		{
			moduleContainer = new List<UIModule>();
			moduleContainerMap.Add(type, moduleContainer);
		}

		UIModule module = null;
		if (moduleContainer.Count != 0)
		{
			module = moduleContainer[moduleContainer.Count - 1];
			moduleContainer.RemoveAt(moduleContainer.Count - 1);
			return module;
		}

		module = GameObject.Instantiate(uiModule) as UIModule;

		ObjectUtility.AttachToParentAndKeepWorldTrans(uiModule.transform.parent.gameObject, module.gameObject);
		return module;
	}

	public bool ShowModule(System.Type type, params object[] userDatas)
	{
		return ShowModuleWithLayer(type, _UILayer.Invalid, userDatas);
	}

	public bool ShowModuleWithLayer(System.Type type, _UILayer layer, params object[] userDatas)
	{
		UIModule module = AllocateItem(type);
		if (module == null)
			return false;

		module.Initialize();
		module.OnShow(layer, userDatas);

		List<UIModule> moduleShowContainer = null;
		if (moduleShowMap.TryGetValue(type, out moduleShowContainer) == false)
		{
			moduleShowContainer = new List<UIModule>();
			moduleShowMap.Add(type, moduleShowContainer);
		}

		moduleShowMap[type].Add(module);

		return true;
	}

	public void ReleaseModule(System.Type type, UIModule module, bool destroy)
	{
		List<UIModule> moduleContainer = null;
		if (moduleContainerMap.TryGetValue(type, out moduleContainer) == false)
			return;

		if (moduleContainer.Contains(module))
			return;

		moduleShowMap[type].Remove(module);

		if (destroy)
		{
			module.Dispose();
			GameObject.DestroyImmediate(module.gameObject);
		}
		else
		{
			moduleContainer.Add(module);
		}
	}

	public void ReleaseHidenModules()
	{
		foreach (KeyValuePair<System.Type, List<UIModule>> type_MdlsPair in moduleContainerMap)
		{
			for (int index = 0; index < type_MdlsPair.Value.Count; )
			{
				UIModule mdl = type_MdlsPair.Value[index];
				type_MdlsPair.Value.Remove(mdl);
				mdl.Dispose();
				GameObject.DestroyImmediate(mdl.gameObject);
			}
		}
	}

	public void ReleaseUnPersistentModules()
	{
		foreach (KeyValuePair<System.Type, List<UIModule>> type_MdlsPair in moduleContainerMap)
		{
			for (int index = 0; index < type_MdlsPair.Value.Count; index++)
			{
				UIModule mdl = type_MdlsPair.Value[index];
				if (mdl.persistent)
					continue;

				type_MdlsPair.Value.Remove(mdl);
				mdl.Dispose();
				GameObject.DestroyImmediate(mdl.gameObject);
				index--;
			}
		}
	}
}

