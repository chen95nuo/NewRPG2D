using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class SysUIEnv : SysModule, SysSceneManager.ISceneManagerListener
{
	public static SysUIEnv Instance { get { return SysModuleManager.Instance.GetSysModule<SysUIEnv>(); } }

	// UIModule unit to store the manager info.
	private class UIModuleData
	{
		public UIModule module; // UIModule instance.
		public Type uiClass;
		public string resourceName;
		public int uiType;
		//public bool singleton;
		public System.Type[] linkedTypes;
		public System.Type[] ignoreMutexTypes;
		public bool hideOtherModules;

		public void Create()
		{
			if (module != null)
				return;

			GameObject go = ResourceManager.Instance.InstantiateAsset<GameObject>(GameDefines.uiModulePath + "/" + resourceName);
			if (go == null)
				return;

			module = go.GetComponent(uiClass) as UIModule;
			Debug.Assert(module != null, "Load UI Module failed : " + uiClass.Name + " " + module.name);
			module.Initialize();
		}

		public void Destroy()
		{
			if (module != null)
			{
				module.Dispose();
				GameObject.Destroy(module.gameObject);
				module = null;
			}
		}
	}

	public delegate bool ShowUIFilterDelegate(System.Type uiTypeClass);

	private ShowUIFilterDelegate showUIFilterDel;
	public ShowUIFilterDelegate ShowUIFilterDel
	{
		get { return showUIFilterDel; }
		set { showUIFilterDel = value; }
	}

	private UIScrollList lockedScroll = null;
	private UIScroller lockedScroller = null;
	private GameObject lockedUIObject = null;
	private List<GameObject> lockedUIObjects = new List<GameObject>();
	private List<int> lockObjectLayers = new List<int>();
	private GameObject uiCamObj; // Current UI camera.
	private GameObject screenMask = null;
	private List<UIModule> uitipFlows = new List<UIModule>();
	// UI camera.
	private Camera uiCam;
	public Camera UICam
	{
		get { return uiCam; }
	}

	// Panel manager.
	private UIPanelMgrEx pnlMgr = new UIPanelMgrEx();
	public UIPanelMgrEx UIPnlMgr
	{
		get { return pnlMgr; }
	}

	// UI root object.
	private Transform uiRtObj; // UI root object.
	public Transform UIRoot
	{
		get { return uiRtObj; }
	}

	private GameObject uiMgr; // UI manager.
	private GameObject uiFntMgr; // UI Font manager.

	// UI FX root object.	
	private Transform uiFxRtObj; // UI FX root object.
	public Transform UIFxRoot
	{
		get { return uiFxRtObj; }
	}

	private UIModulePoolManager modulePool;
	public UIModulePoolManager ModulePool
	{
		get
		{
			if (modulePool == null)
				modulePool = gameObject.AddComponent<UIModulePoolManager>();

			return modulePool;
		}
	}

	private bool isUILocked = false;
	public bool IsUILocked
	{
		get { return isUILocked; }
	}

	private Dictionary<System.Type, UIModuleData> uiModuleDataDict = new Dictionary<System.Type, UIModuleData>(); // UI modules.
	private Dictionary<int, System.Type> id_typeDict = new Dictionary<int, Type>();

	private List<UIModuleData> uiModuleTempList = new List<UIModuleData>();
	private List<UIModuleData> uiModulePriorityList = new List<UIModuleData>();

	private const float dynamicSpaceZ = 0.1f; // Space z between UI controls.
	private const float dynamicMinLocalZ = 50;
	private const float dynamicMaxLocalZ = 100;

	private float assembleTipTimer = 1f;
	private float intervalTimer = 2.1f;
	private float timer = 0f;
	private bool firstShowTips = true;
	private List<string> tipmsgs = new List<string>();

	private float dynamicLocalZ; // Current local z.
	/// <summary>
	/// Dynamic UI controls local z, such as hurt text, etc. Relative to the UI-root. 
	/// </summary>
	public float DynamicLocalZ
	{
		get
		{
			dynamicLocalZ -= dynamicSpaceZ;

			if (dynamicLocalZ < dynamicMinLocalZ)
				dynamicLocalZ = dynamicMaxLocalZ;

			return dynamicLocalZ + UIPnlMgr.BottomMostPanelZ;
		}
	}

	private float uiModuleScaleRate = 1.0f;
	public float UIModuleScaleRate
	{
		get { return uiModuleScaleRate; }
	}
	public List<UIModule> currentShowedModuls = new List<UIModule>();

	#region UIModleShowEvent
	public class UIModleShowEvent
	{
		private System.Type uiType;
		private object[] data;

		private bool hasShown = false;
		public bool HasShown { get { return hasShown; } }

		public UIModleShowEvent(int uiType, params object[] data)
		{
			this.data = data;
			this.uiType = SysUIEnv.Instance.GetClassByType(uiType);
		}

		public UIModleShowEvent(System.Type uiType, params object[] data)
		{
			this.uiType = uiType;
			this.data = data;
		}

		public virtual void Execute()
		{
			if (hasShown)
				return;

			SysUIEnv.Instance.ShowUIModule(uiType, data);

			hasShown = true;

			return;
		}

		public virtual bool Finished()
		{
			if (hasShown == false)
				return false;

			if (SysUIEnv.Instance.IsUIModuleLoaded(uiType) == false)
				return true;

			UIModule ui = SysUIEnv.Instance.GetUIModule(uiType);
			return ui.IsShown == false;
		}
	}

	public class UIModuleDelayShowEvent : UIModleShowEvent
	{
		private bool delay;
		public bool Delay
		{
			get { return delay; }
			set { delay = value; }
		}

		public UIModuleDelayShowEvent(System.Type uiType, params object[] data)
			: base(uiType, data)
		{
			this.delay = true;
		}

		public override void Execute()
		{
			if (delay)
				return;

			base.Execute();
		}
	}

	private bool pauseShowEvent = false;
	public bool PauseShowEvent
	{
		get { return pauseShowEvent; }
		set { pauseShowEvent = value; }
	}

	private LinkedList<UIModleShowEvent> showEventsList = new LinkedList<UIModleShowEvent>();

	public void AddShowEvent(UIModleShowEvent showEvent)
	{
		showEventsList.AddLast(showEvent);
	}

	public void ClearShowEventsList()
	{
		showEventsList.Clear();
	}

	private void UpdateShowEvent()
	{
		if (pauseShowEvent)
			return;

		if (showEventsList.Count == 0)
			return;

		// �п����ڶϿ�����֮��,�������¼�
		//if (RequestMgr.Inst.IsConnected == false)
		//    return;

		UIModleShowEvent showEvent = showEventsList.First.Value;
		if (showEvent.HasShown == false)
			showEvent.Execute();

		if (showEvent.Finished())
			showEventsList.RemoveFirst();
	}
	#endregion

	public override bool Initialize()
	{
		Debug.Log(string.Format("DPI:{0} Width:{1} Height:{2}", Screen.dpi, Screen.width, Screen.height));

		// Create UI camera.
		uiCamObj = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiPath, GameDefines.uiCam));
		uiCamObj.name = GameDefines.uiCam;
		uiCam = (Camera)uiCamObj.GetComponent("Camera");
		//uiCam.cullingMask = (1 << GameDefines.UIRaycastLayer) | (1 << GameDefines.lockedUILayer) | (1 << GameDefines.ignoreRayTestLayer);

		float standardScreenProportion = GameDefines.uiDefaultScreenSize.x / GameDefines.uiDefaultScreenSize.y;
		float currentScreenProportion = uiCam.pixelWidth / uiCam.pixelHeight;
		//		float standardOrthographicSize = uiCam.orthographicSize;
		if (currentScreenProportion < standardScreenProportion)
			uiCam.orthographicSize = standardScreenProportion * uiCam.pixelHeight / uiCam.pixelWidth * GameDefines.uiDefaultScreenSize.y / 2;

		// Find UI root object.
		uiRtObj = ObjectUtility.FindChildObject(uiCamObj, GameDefines.uiCnt).transform;

		// Create UI manager.
		uiMgr = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiPath, GameDefines.uiMgr));
		uiMgr.name = GameDefines.uiMgr;

		// Reset camera to UI manager.
		UIManager.instance.UIScreenScale = uiCam.pixelHeight / (uiCam.orthographicSize * 2);
		UIManager.instance.rayCamera = uiCam;
		UIManager.instance.rayMask = (1 << GameDefines.UIRaycastLayer) | (1 << GameDefines.UIInvisableLayer);
		UIManager.instance.RemoveCamera(0);
		UIManager.instance.AddCamera(uiCam, (1 << GameDefines.UIRaycastLayer) | (1 << GameDefines.UIInvisableLayer), Mathf.Infinity, 0);
		UIManager.instance.AddCamera(Camera.main, (1 << GameDefines.UISceneRaycastLayer), Mathf.Infinity, 1);

		// Create font manager.
		uiFntMgr = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiPath, GameDefines.uiFntMgr));
		uiFntMgr.name = GameDefines.uiFntMgr;

		// Create FX root object.
		uiFxRtObj = new GameObject("FxRoot").transform;
		ObjectUtility.AttachToParentAndResetLocalTrans(uiRtObj, uiFxRtObj);

		// Don'type destroy UI managed objects when change scene.
		GameObject.DontDestroyOnLoad(uiCamObj);
		GameObject.DontDestroyOnLoad(uiMgr);
		GameObject.DontDestroyOnLoad(uiFntMgr);

		// Initialize panel manager.
		pnlMgr.Initialize();

		// Set dynamic UI default value for local z.
		dynamicLocalZ = dynamicMaxLocalZ;

		modulePool = gameObject.AddComponent<UIModulePoolManager>();
		return true;
	}

	public override void Dispose()
	{
		// Destroy UI managed objects.
		GameObject.Destroy(uiCamObj);
		GameObject.Destroy(uiMgr);
		GameObject.Destroy(uiFntMgr);

		// Dispose UI modules.
		foreach (var kvp in uiModuleDataDict)
			kvp.Value.Destroy();

		uiModuleDataDict.Clear();
		id_typeDict.Clear();

		// Dispose UI environment.
		pnlMgr.Dispose();
	}

	public override void OnUpdate()
	{
		UpdateShowEvent();
		UpdatePerformEnter();
	}






	#region UITips StartCoroutine

	public void AddTip(string message)
	{
		tipmsgs.Add(message);
	}

	public void AddTip(string message, float timer)
	{
		AddTip(message);
		assembleTipTimer = timer;
	}
	public void AddTip(string message, float timer, float intervalTimer)
	{
		this.intervalTimer = intervalTimer;
		AddTip(message, timer);
	}


	public void ClearAllTip()
	{
		if (uitipFlows == null || uitipFlows.Count == 0)
			return;
		tipmsgs.Clear();
		foreach (UIModule module in uitipFlows)
		{
			UIPnlTipFlow tip = module as UIPnlTipFlow;
			tip.StopTipsIEnumerator();
			tip.OnHide();
		}
		uitipFlows.Clear();
	}

	public void UpdatePerformEnter()
	{
		if (tipmsgs != null && tipmsgs.Count > 0)
		{
			timer += Time.deltaTime;
			if (timer >= intervalTimer || firstShowTips)
			{
				if (firstShowTips) firstShowTips = !firstShowTips;
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), tipmsgs[0], assembleTipTimer);
				uitipFlows.AddRange(SysUIEnv.Instance.ModulePool.GetShownUIModules(typeof(UIPnlTipFlow)));
				DelTip(tipmsgs[0]);
				timer = 0f;
			}
		}
		else
		{
			firstShowTips = true;
		}
	}



	private void DelTip(string message)
	{
		tipmsgs.Remove(message);
	}

	#endregion

	public bool IsUIObject(GameObject gameObject)
	{
		int layer = gameObject.layer;
		return layer == GameDefines.UIRaycastLayer ||
			layer == GameDefines.UIIgnoreRaycastLayer ||
			layer == GameDefines.UIInvisableLayer ||
			layer == GameDefines.lockedUILayer;
	}

	#region UIModule operation
	private UIModuleData GetUIModuleData(System.Type type)
	{
		UIModuleData uiModuleData;
		if (uiModuleDataDict.TryGetValue(type, out uiModuleData) == false)
			return null;

		return uiModuleData;
	}

	private UIModuleData GetUIModuleData(int uiType)
	{
		return GetUIModuleData(GetClassByType(uiType));
	}

	public bool RegisterUIModule(System.Type type, string resName, int uiType, Type[] linkedTypes, bool hideOtherModules, Type[] igoreMutexTypes)
	{
		if (uiModuleDataDict.ContainsKey(type))
			return false;

		Debug.Assert(type.IsSubclassOf(typeof(UIModule)), "type is " + type.ToString());

		UIModuleData newUIModule = new UIModuleData();
		newUIModule.uiClass = type;
		newUIModule.resourceName = resName;
		newUIModule.uiType = uiType;
		newUIModule.linkedTypes = linkedTypes;
		newUIModule.ignoreMutexTypes = igoreMutexTypes;
		newUIModule.hideOtherModules = hideOtherModules;
		uiModuleDataDict.Add(type, newUIModule);
		if (uiType != ClientServerCommon._UIType.UnKonw)
			id_typeDict.Add(uiType, type);

		return true;
	}

	public System.Type GetClassByType(int uiType)
	{
		System.Type type;
		if (id_typeDict.TryGetValue(uiType, out type) == false)
			return null;

		return type;
	}

	public int GetUITypeByClass(Type type)
	{
		UIModuleData uiModuleData;
		if (uiModuleDataDict.TryGetValue(type, out uiModuleData) == false)
			return _UIType.UnKonw;

		return uiModuleData.uiType;
	}

	// Add UI module to be managed.
	public bool LoadUIModule(System.Type type)
	{
		if (uiModuleDataDict.ContainsKey(type) == false)
			return false;

		if (uiModuleDataDict[type].module == null)
			uiModuleDataDict[type].Create();

		return true;
	}

	// Delete UI module.
	public bool UnloadUIModule(System.Type type)
	{
		if (!uiModuleDataDict.ContainsKey(type))
			return false;

		uiModuleDataDict[type].Destroy();

		return true;
	}

	public bool IsUIModuleLoaded(System.Type type)
	{
		if (!uiModuleDataDict.ContainsKey(type))
			return false;

		return uiModuleDataDict[type].module != null;
	}

	public bool IsUIModuleLoaded(int uiType)
	{
		return IsUIModuleLoaded(GetClassByType(uiType));
	}

	public bool IsUIModuleShown(System.Type type)
	{
		if (IsUIModuleLoaded(type) == false)
			return false;

		return GetUIModule(type).IsShown;
	}

	public bool IsUIModuleShown(int uiType)
	{
		return IsUIModuleShown(GetClassByType(uiType));
	}

	// Dispose UI modules.
	public void DisposeUIModules()
	{
		// Disposed candidates.
		var dsps = new List<Type>();

		// Find disposed module.
		foreach (var kvp in uiModuleDataDict)
			if (kvp.Value.module != null && !kvp.Value.module.persistent)
				dsps.Add(kvp.Key);

		// Dispose UI modules.
		foreach (var key in dsps)
			if (uiModuleDataDict.ContainsKey(key))
				uiModuleDataDict[key].Destroy();

		// Delete FX child objects.
		ObjectUtility.DestroyChildObjects(uiFxRtObj.gameObject);

		ClearAllTip();
		if (ModulePool != null)
			ModulePool.ReleaseHidenModules();
	}

	// Get UI module.
	public T GetUIModule<T>() where T : UIModule
	{
		return GetUIModule(typeof(T), true) as T;
	}

	public T GetUIModule<T>(bool createWhenNotLoaded) where T : UIModule
	{
		return GetUIModule(typeof(T), createWhenNotLoaded) as T;
	}

	public UIModule GetUIModule(System.Type type)
	{
		return GetUIModule(type, true);
	}

	public UIModule GetUIModule(System.Type type, bool createWhenNotLoaded)
	{
		if (!uiModuleDataDict.ContainsKey(type))
			return null;

		if (uiModuleDataDict[type].module == null && createWhenNotLoaded)
			uiModuleDataDict[type].Create();

		return uiModuleDataDict[type].module;
	}

	public UIModule GetUIModule(int uiType)
	{
		return GetUIModule(uiType, true);
	}

	public UIModule GetUIModule(int uiType, bool createWhenNotLoaded)
	{
		return GetUIModule(GetClassByType(uiType), createWhenNotLoaded);
	}

	public bool ShowUIModule(System.Type type, params object[] userDatas)
	{
		return ShowUIModuleWithLayer(type, _UILayer.Invalid, userDatas);
	}

	public bool ShowUIModuleWithLayer(System.Type type, _UILayer layer, params object[] userDatas)
	{
		if (showUIFilterDel != null && showUIFilterDel(type) == false)
			return false;

		UIModuleData uiModuleData = GetUIModuleData(type);
		if (uiModuleData == null)
			return false;

		// Show linked UI, showing UI may be in linked UIs for shown order.
		if (uiModuleData.linkedTypes != null)
		{
			List<Type> showingUIs = new List<Type>();
			GetLinkedUIs(type, showingUIs);

			foreach (var linkedType in showingUIs)
			{
				UIModule module = GetUIModule(linkedType);
				if (module == null || module.IsShown)
					continue;

				module.OnShow(layer, module.GetType() == type ? userDatas : null);
			}

			// Sort UI _layer
			var sortingUIs = new List<UIModule>();
			foreach (_UILayer _layer in Enum.GetValues(typeof(_UILayer)))
			{
				sortingUIs.Clear();
				foreach (var linkedType in showingUIs)
				{
					UIModule module = GetUIModule(linkedType);
					if (module.ShowLayer == _layer)
					{
						sortingUIs.Add(module);
					}
				}

				// Bubble sort
				for (int i = sortingUIs.Count; i > 1; --i)
				{
					for (int j = 0; j < i - 1; ++j)
					{
						UIModule ui1 = sortingUIs[j];
						UIModule ui2 = sortingUIs[j + 1];

						if (ui1.PnlEx.OffsetZ < ui2.PnlEx.OffsetZ)
						{
							float offsetZ = ui1.PnlEx.OffsetZ;
							ui1.PnlEx.ResetOffsetZ(ui2.PnlEx.OffsetZ);
							ui2.PnlEx.ResetOffsetZ(offsetZ);
						}
					}
				}
			}
		}
		else
		{
			// Show UI, it may be shown in linked UIs
			UIModule showingUI = GetUIModule(type);
			if (showingUI != null && showingUI.IsShown == false)
				showingUI.OnShow(layer, showingUI.GetType() == type ? userDatas : null);
		}

		ProcessOverlay();

		return true;
	}

	public void ShowUIModule<T>(params object[] userDatas) where T : UIModule
	{
		ShowUIModule(typeof(T), userDatas);
	}

	public bool ShowUIModule(int uiType, params object[] userDatas)
	{
		var moduleData = GetUIModuleData(uiType);
		if (moduleData != null)
			return ShowUIModule(moduleData.uiClass, userDatas);

		return false;
	}

	public void HideUIModule(System.Type type)
	{
		UIModuleData uiModuleData = GetUIModuleData(type);
		if (uiModuleData == null)
			return;

		if (uiModuleData.linkedTypes != null)
		{
			// Get all UI linked with hiding UI
			List<Type> uiModuleLinkedData = new List<Type>();
			GetLinkedUIs(type, uiModuleLinkedData);

			// Get UI linked with shown UIs except hiding one
			List<Type> linkedUIByOthers = new List<Type>();
			foreach (var kvp in uiModuleDataDict)
				if (kvp.Value.module != null && kvp.Value.module.IsShown && kvp.Key != type && uiModuleLinkedData.Contains(kvp.Key) == false)
					GetLinkedUIs(kvp.Key, linkedUIByOthers);

			// Hide linked UI
			foreach (var linkedType in uiModuleLinkedData)
			{
				UIModule module = GetUIModule(linkedType);
				if (module == null || module.IsShown == false)
					continue;

				// Skip UI linked by other
				if (linkedUIByOthers.Contains(linkedType))
					continue;

				module.OnHide();
			}
		}
		else
		{
			// Hide UI, it may be hide in linked UIs
			UIModule hidingUI = GetUIModule(type);
			if (hidingUI != null && hidingUI.IsShown)
				hidingUI.OnHide();
		}

		ProcessOverlay();
	}

	public void HideUIModule<T>() where T : UIModule
	{
		HideUIModule(typeof(T));
	}

	public void HideUIModule(int uiType)
	{
		var moduleData = GetUIModuleData(uiType);
		if (moduleData != null)
			HideUIModule(moduleData.uiClass);
	}

	public void HideUIModules()
	{
		foreach (var kvp in uiModuleDataDict)
			if (kvp.Value.module != null && kvp.Value.module.IsShown)
				kvp.Value.module.OnHide();
	}

	public void HideUIModules(Type exceptUIModule)
	{
		foreach (var kvp in uiModuleDataDict)
			if (exceptUIModule != kvp.Key && kvp.Value.module != null && kvp.Value.module.IsShown)
				kvp.Value.module.OnHide();
	}

	public void UnloadHidenUI()
	{
		uiModuleTempList.Clear();

		// Update all UI modules.
		foreach (var moduleUnit in uiModulePriorityList)
		{
			if (moduleUnit.module == null)
				continue;

			// Skip shown UI
			if (moduleUnit.module.IsShown)
				continue;

			// Skip persistent UI
			if (moduleUnit.module.persistent)
				continue;

			uiModuleTempList.Add(moduleUnit);
		}

		foreach (var moduleUnit in uiModuleTempList)
		{
			UnloadUIModule(moduleUnit.module.GetType());
			uiModulePriorityList.Remove(moduleUnit);
		}

		if (ModulePool != null)
			ModulePool.ReleaseHidenModules();
	}

	public void OnUIModuleShown(UIModule module)
	{
		System.Type type = module.GetType();
		if (uiModuleDataDict.ContainsKey(type) == false)
			return;

		// Hide Mutex UIs
		HideMutexModules(type);

		// Add to priority list for auto releasing
		UIModuleData mdlUnit = uiModuleDataDict[type];
		if (uiModulePriorityList.Contains(mdlUnit) == false)
			uiModulePriorityList.Add(mdlUnit);
	}

	public void OnUIModuleHidden(UIModule module)
	{
	}

	public void AfterFreeText()
	{
		foreach (var uiModule in uiModuleDataDict)
			if (uiModule.Value.module != null)
				uiModule.Value.module.AfterFreeText();
	}

	private void GetLinkedUIs(System.Type type, List<Type> linedUIs)
	{
		GetLinkedUIs(type, linedUIs, new List<Type>());
	}

	private void GetLinkedUIs(System.Type type, List<Type> linedUIs, List<Type> checkedUIs)
	{
		if (checkedUIs.Contains(type) == false)
		{
			checkedUIs.Add(type);

			// Grab UIModuleData
			UIModuleData uiModuleData = GetUIModuleData(type);
			if (uiModuleData == null)
				return;

			// Add lined UI
			if (uiModuleData.linkedTypes != null)
				foreach (var linkedType in uiModuleData.linkedTypes)
					GetLinkedUIs(linkedType, linedUIs, checkedUIs);
		}
		// 忽略之前的排序，重新定义顺序，如果Remove失败表示没有在队列中
		else if (linedUIs.Remove(type))
		{
			linedUIs.Add(type);
		}

		// Add self, and skip repeated UI
		if (linedUIs.Contains(type) == false)
			linedUIs.Add(type);
	}

	private void HideMutexModules(Type type)
	{
		// Grab UIModuleData
		UIModuleData uiModuleData = GetUIModuleData(type);
		if (uiModuleData == null)
			return;

		//		// 若果该界面时Panel，将所有不是TopMost的对话框Hide。
		//		if (!uiModuleData.module.model && uiModuleData.module.ShowLayer != _UILayer.TopMost)
		//			foreach (var kvp in uiModuleDataDict)
		//				if (kvp.Value.module != null && kvp.Value.module.model && kvp.Value.module.IsShown)
		//					kvp.Value.module.OnHide();

		// If need not hide others, return
		if (uiModuleData.hideOtherModules == false)
			return;

		// Get ignore UI
		List<Type> ignoreMutexTypes = new List<Type>();
		GetLinkedUIs(type, ignoreMutexTypes);

		if (uiModuleData.ignoreMutexTypes != null)
			foreach (var uiType in uiModuleData.ignoreMutexTypes)
				GetLinkedUIs(uiType, ignoreMutexTypes);

		// Hide other UI except ignore UIs
		foreach (var kvp in uiModuleDataDict)
			if (kvp.Value.module != null && kvp.Value.module.IsShown && (ignoreMutexTypes == null || ignoreMutexTypes.Contains(kvp.Key) == false) && kvp.Value.module.ignoreMutex == false)
				kvp.Value.module.OnHide();
	}

	private void ProcessOverlay()
	{
		// Get first overlay module
		UIModule firstOverlayModule = null;
		foreach (var kvp in uiModuleDataDict)
			if (kvp.Value.module != null && kvp.Value.module.IsShown && kvp.Value.module.overlay)
				if (firstOverlayModule == null || kvp.Value.module.PnlEx.OffsetZ < firstOverlayModule.PnlEx.OffsetZ)
					firstOverlayModule = kvp.Value.module;


		List<UIModule> shownMdls = modulePool.GetAllShownModules();
		foreach (var kvp in shownMdls)
		{
			if (kvp.overlay)
			{
				if (firstOverlayModule == null || kvp.PnlEx.OffsetZ < firstOverlayModule.PnlEx.OffsetZ)
					firstOverlayModule = kvp;
			}
		}

		// Set overlay
		if (firstOverlayModule != null)
			if (firstOverlayModule.IsOverlayed)
				firstOverlayModule.RemoveOverlay();

		foreach (var kvp in uiModuleDataDict)
			if (kvp.Value.module != null && kvp.Value.module.IsShown && kvp.Value.module != firstOverlayModule)
				if (firstOverlayModule != null && kvp.Value.module.PnlEx.OffsetZ > firstOverlayModule.PnlEx.OffsetZ && kvp.Value.module.canNotOverlay == false)
					kvp.Value.module.Overlay();
				else if (kvp.Value.module.IsOverlayed)
					kvp.Value.module.RemoveOverlay();

		foreach (var kvp in shownMdls)
			if (kvp != firstOverlayModule)
				if (firstOverlayModule != null && kvp.PnlEx.OffsetZ > firstOverlayModule.PnlEx.OffsetZ)
					kvp.Overlay();
				else if (kvp.IsOverlayed)
					kvp.RemoveOverlay();
	}
	#endregion

	#region UI Input

	public GameObject GetListIndexByData(string attachCompentName, int buttonData)
	{
		GameObject scrollObj = GameObject.FindGameObjectWithTag(attachCompentName);

		UIScrollList scrollList = scrollObj.GetComponent<UIScrollList>();
		if (scrollObj == null)
		{
			Debug.Log("Could Not Found ScrollList with tag " + attachCompentName);
		}
		else
		{

			for (int i = 0; i < scrollList.Count; i++)
			{
				UIListItemContainer container = scrollList.GetItem(i) as UIListItemContainer;
				if (container == null || container.Data == null)
					continue;

				UIElemEquipSelectItem item = container.Data as UIElemEquipSelectItem;
				if (item != null)
				{
					int id = (int)item.selectBtn.indexData;

					if (id == buttonData)
					{
						return item.selectBtn.gameObject;

					}
				}
				else
				{
					UIElemSkillItem skillItem = container.Data as UIElemSkillItem;
					int id = (int)skillItem.selectBtn.indexData;

					if (id == buttonData)
					{
						return skillItem.selectBtn.gameObject;
					}
				}

			}

		}
		return null;
	}

	public void LockUIInput()
	{
		LockUIInput("", "", 0, 0, false, false);
	}

	public void LockUIInput(string controlTag, string IconTag, int tagIndex, int buttonData, bool isSkillOrEquip, bool isLisItemObj)
	{
		// Reset
		UnlockUIInput();

		isUILocked = true;

		List<string> controlTags = new List<string>();
		controlTags.Add(controlTag);

		if (SysGameStateMachine.Instance.GetCurrentState() is GameState_CentralCity)
			KodGames.Camera.main.GetComponent<CentralCityCameraController>().LockTouch = true;

		if (IconTag != null && !IconTag.Equals(""))
			controlTags.Add(IconTag);

		for (int i = 0; i < controlTags.Count; i++)
		{
			if (!string.IsNullOrEmpty(controlTags[i]))
			{
				if (isLisItemObj && !controlTags[i].Equals("UITipHelp_CloseBtn"))
					lockedScroll = FindControlInListByTag(controlTags[i], tagIndex, -1, ref lockedUIObject);
				else if (buttonData > 0 && !controlTags[i].Equals("UITipHelp_CloseBtn"))
				{
					if (isSkillOrEquip)
						lockedUIObject = GetListIndexByData(controlTags[i], buttonData);
					else
						lockedScroll = FindControlInListByTag(controlTags[i], -1, buttonData, ref lockedUIObject);
				}
				else
				{
					lockedUIObject = GameObject.FindWithTag(controlTags[i]);
					var ascb = lockedUIObject.GetComponent<AutoSpriteControlBase>();

					if (ascb != null && ascb.Container is UIListItemContainer)
						lockedScroll = (ascb.Container as UIListItemContainer).GetScrollList();
				}

				lockObjectLayers.Add(lockedUIObject == null ? 0 : lockedUIObject.layer);

				if (lockedScroll != null)
				{
					//Debug.Log("locked scroll " + lockedScroll.name);
					lockedScroll.touchScroll = false;
				}

				UIListButton3D sceneBtn = lockedUIObject.GetComponent<UIListButton3D>();
				if (sceneBtn != null)
				{
					lockedScroller = sceneBtn.List;
					lockedScroller.TouchScroll = false;
				}

				UIScroller scroll = lockedUIObject.GetComponent<UIScroller>();
				if (scroll != null)
				{
					lockedScroller = scroll;
					lockedScroller.TouchScroll = false;
				}

				switch (lockedUIObject.layer)
				{
					case GameDefines.UIRaycastLayer: lockedUIObject.layer = GameDefines.lockedUILayer; break;
					case GameDefines.UISceneRaycastLayer: lockedUIObject.layer = GameDefines.lockedSceneUILayer; break;
					case GameDefines.UIInvisableLayer: lockedUIObject.layer = GameDefines.lockedInvisableUILayer; break;
					default: Debug.LogError(string.Format("Locking controll ({0}) should be in UI layer.", controlTags[i])); break;
				}
				lockedUIObjects.Add(lockedUIObject);
			}

			// Lock UI manager
			UIManager.instance.rayMask = (1 << GameDefines.lockedUILayer) | (1 << GameDefines.lockedInvisableUILayer);
			UIManager.instance.SetCameraLayerMask(0, (1 << GameDefines.lockedUILayer) | (1 << GameDefines.lockedInvisableUILayer), Mathf.Infinity);
			UIManager.instance.SetCameraLayerMask(1, (1 << GameDefines.lockedSceneUILayer), Mathf.Infinity);
		}
	}

	public UIScrollList FindControlInListByTag(string controlTag, int tagIndex, int buttonData, ref GameObject controlObj)
	{
		if (string.IsNullOrEmpty(controlTag))
			return null;

		GameObject[] gos = GameObject.FindGameObjectsWithTag(controlTag);
		if (gos == null || gos.Length <= 0)
			return null;

		var tempControlObj = gos[0];
		var ascb = tempControlObj.GetComponent<AutoSpriteControlBase>();
		if (ascb == null)
			return null;


		var container = ascb.Container as UIListItemContainer;
		if (container == null)
			return null;


		var tempScroll = container.GetScrollList();
		if (tempScroll == null)
			return null;
		foreach (var go in gos)
		{
			if (buttonData > 0)
			{
				ascb = go.GetComponent<AutoSpriteControlBase>();
				if (ascb == null)
					continue;

				if (ascb.indexData == buttonData)
				{
					controlObj = go;
					return tempScroll;
				}
			}
			else
			{
				ascb = go.GetComponent<AutoSpriteControlBase>();
				if (ascb == null)
					continue;

				container = ascb.Container as UIListItemContainer;
				if (container == null)
					continue;

				if (container.Index == tagIndex)
				{
					controlObj = go;
					break;
				}
			}
		}

		return tempScroll;
	}


	public void UnlockUIInput()
	{
		isUILocked = false;

		if (SysGameStateMachine.Instance.GetCurrentState() is GameState_CentralCity)
		{
			if (KodGames.Camera.main != null && KodGames.Camera.main.GetComponent<CentralCityCameraController>() != null)
				KodGames.Camera.main.GetComponent<CentralCityCameraController>().LockTouch = false;
		}

		// Reset
		for (int i = 0; i < lockedUIObjects.Count; i++)
		{
			if (lockedUIObjects[i] != null)
			{
				lockedUIObjects[i].layer = lockObjectLayers[i];
				lockedUIObjects[i] = null;
			}
		}

		lockedUIObject = null;

		// Rest scroll.
		if (lockedScroll != null)
		{
			lockedScroll.touchScroll = true;
			lockedScroll = null;
		}

		// Rest scroller.
		if (lockedScroller != null)
		{
			lockedScroller.TouchScroll = true;
			lockedScroller = null;
		}

		// Clear Layers.
		lockedUIObjects.Clear();
		lockObjectLayers.Clear();

		// Unlock UI manager
		UIManager.instance.rayMask = (1 << GameDefines.UIRaycastLayer) | (1 << GameDefines.UIInvisableLayer);
		UIManager.instance.SetCameraLayerMask(0, (1 << GameDefines.UIRaycastLayer) | (1 << GameDefines.UIInvisableLayer), Mathf.Infinity);
		UIManager.instance.SetCameraLayerMask(1, (1 << GameDefines.UISceneRaycastLayer), Mathf.Infinity);
	}
	#endregion

	#region SysSceneManager.ISceneManagerListener
	public void OnSceneWillChange(SysSceneManager manager, string currentScene, string newScene)
	{

	}

	public void OnSceneChanged(SysSceneManager manager, string oldScene, string currentScene)
	{
		// Replace camera for scene picking
		UIManager.instance.ReplaceCamera(1, Camera.main);
	}
	#endregion

	public void ShowScreenMask(string path)
	{
		if (screenMask != null)
			return;

		screenMask = ResourceManager.Instance.InstantiateAsset<GameObject>(path);
		ObjectUtility.AttachToParentAndResetLocalTrans(UIRoot, screenMask.transform);
		Vector3 pos = screenMask.transform.localPosition;
		pos.z = pnlMgr.TopMostPanelZ;
		screenMask.transform.localPosition = pos;
	}


}
