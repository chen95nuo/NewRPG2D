using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using KodGames.ExternalCall;

//-----------------------------------------------------------------------------
// Game state.
//-----------------------------------------------------------------------------
public class GameState
{
	public const int INV = 0;
	public const int INIT = 1;	// Initialize.
	public const int FRST_DL = 2;	// First download.
	public const int CHECK_VERSION = 3; // Check and upgrade game config
	public const int CNCT_SRV = 4;	// Connect server.
	public const int SELECT_AREA = 5; // Select game area
	public const int QRY_DT = 6;	// Query player data.
	public const int CRT_AVT = 7;	// Create avatar.
	public const int MN_MENU = 8;	// Main menu.
	public const int BATTLE = 9;	// Battle.
}

public class GameMain : MonoBehaviour
{
	// GameMain is a singleton.
	private static GameMain sInst;
	public static GameMain Inst { get { return sInst; } }

	public int maxFrameRate = 30;

	// Use this for initialization
	private void Awake()
	{
        //PlayerPrefs.DeleteAll();
		// Initialize crittercism
#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
//		CrittercismPlugin.SetupExceptionHandler(true);
#endif

#if UNITY_ANDROID
		Debug.Log("CpuAbi:" + KodGames.ExternalCall.DevicePlugin.GetCpuAbi() + " CpuAbi2:" + KodGames.ExternalCall.DevicePlugin.GetCpuAbi2());
#endif
		// If game main is restart, destroy it, GameMain must be a singleton.
		if (sInst != null)
		{
			GameObject.Destroy(gameObject);
			return;
		}

		sInst = this;
		DontDestroyOnLoad(gameObject);

		// Splash.
		if (CachedGuiTexture != null)
		{
			float height = Screen.width * CachedGuiTexture.texture.height / CachedGuiTexture.texture.width;
			float y = (Screen.height - height) / 2;
			CachedGuiTexture.pixelInset = new Rect(0, y, Screen.width, height);
		}
	}

	// Use this for initialization
	private void Start()
	{
		// Set random seed.
		UnityEngine.Random.seed = (int)DateTime.Now.Ticks;

		// Initialize timer
		TimeEx.Initialize();

		// Set max frame rate
		Time.timeScale = 1;
		Application.targetFrameRate = maxFrameRate;

		SysModuleManager.Instance.Initialize(this.gameObject);

		DevicePlugin.SetIdleTimerDisabled(true);
		DevicePlugin.CreateDeskShortCut();

		StartCoroutine("LoadProcess");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadProcess()
	{
		if (KodConfigPlugin.HasAdditionalSplashScreen())
		{
			string filePath = "file://" + KodConfigPlugin.GetAdditionalSplashPath();
			if (string.IsNullOrEmpty(filePath) == false)
			{
				// Load splash texture
				var www = new WWW(filePath);
				yield return www;

				// Replace splash
				var splashTex = www.textureNonReadable;
				CachedGuiTexture.texture = splashTex;
				www = null;
			}
		}

		SysGameStateMachine stateMachine = SysModuleManager.Instance.AddSysModule<SysGameStateMachine>(true);
		stateMachine.EnterState<GameState_Initializing>();
	}

	// Update is called once per frame
	private void Update()
	{
		// Process escape key.
#if UNITY_ANDROID
		if (Input.GetKeyUp("escape"))
		{
			Platform.Instance.AndroidQuit();
		}
#endif

		// Update game logic.
		GameUpdate();
	}

	private void DoOnEscape()
	{
		// 
		if (SysUIEnv.Instance.IsUILocked
			|| UIManager.instance.blockInput)
			return;
		//
		if (SysGameStateMachine.Instance == null
			|| SysUIEnv.Instance == null
			|| ConfigDatabase.DefaultCfg.GameConfig.androidEscapeKey == null)
		{
			Platform.Instance.AndroidQuit();
			return;
		}

		//
		if (ConfigDatabase.DefaultCfg.GameConfig.androidEscapeKey.open == false)
			return;

		//
		for (int index = 0; index < ConfigDatabase.DefaultCfg.GameConfig.androidEscapeKey.ignoreUI.Count; index++)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(ConfigDatabase.DefaultCfg.GameConfig.androidEscapeKey.ignoreUI[index]))
				return;
		}

		//
#if UNITY_ANDROID
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgMessage) && SysUIEnv.Instance.GetUIModule<UIDlgMessage>().IgnoreEscape)
			return;
		SysGameStateMachine.Instance.CurrentState.OnEscape();
#endif
	}

#if UNITY_EDITOR
	private void OnGUI()
	{
		// Update game ui.
		GameGUIUpdate();
	}
#endif

	private void OnApplicationQuit()
	{
		GameRelease();
	}

	private void OnApplicationPause(bool pause)
	{
		//		Debug.Log("OnApplicationPause " + pause);
		// 非短线重连机制下, 检查网络状态
		if (RequestMgr.Inst.DoesSupprotReconnect == false && pause == false
			&& brokenDlgShown == false && timeOutDlgShown == false && desynchronyDlgShown == false && configOutOfSyncDlgShown == false)
			CheckNetworkConnection();
	}

	private void OnApplicationFocus(bool focus)
	{
		//Debug.Log("OnApplicationFocus " + focus);
	}

	// On level was loaded call back.***HAS BUGS***, this method is not call on correct order.
	private void OnLevelWasLoaded(int level)
	{
	}

	#region ExternalCall
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPauseGameRequest(string message)
	{
		//		Debug.Log("OnPauseGameRequest");
		Time.timeScale = 0.01f;
		Application.targetFrameRate = 1;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnResumeGameRequest(string message)
	{
		//		Debug.Log("OnResumeGameRequest");
		Time.timeScale = 1;
		Application.targetFrameRate = maxFrameRate;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReceiveMemoryWarning(string message)
	{
		Debug.Log("OnReceiveMemoryWarning");
		FreeMemory();
	}

	public void FreeMemory()
	{
		// Free UI memory
		if (SysUIEnv.Instance != null)
			SysUIEnv.Instance.UnloadHidenUI();

		// Free Fx pool
		if (SysFx.Instance != null)
			SysFx.Instance.ReleaseAllFxPool();

		if (AudioManager.Instance != null)
			AudioManager.Instance.FreeMemory();

		// Free game object pool
		GameObjectPoolManager.Singleton.FreeMemory();

		// Free icon memory
		if (SysIconManger.Instance != null)
			SysIconManger.Instance.DestroyUnusedIcon();

		// Free cache
		if (ResourceCache.Instance != null)
			ResourceCache.Instance.FreeCache();

		// Free unused assert
		Resources.UnloadUnusedAssets();

		// Collect GC
		System.GC.Collect();
	}
	#endregion

	#region Plugin linsteners
	/*
	 * IAP Listener
	 */
	public void CreateIAPListener()
	{
		if (GetIAPListener() != null)
			return;

		IAPListener.CreateOnGameObject(gameObject);
	}

	public void RemoveIAPListener()
	{
		IAPListener iapListener = GetIAPListener();
		if (iapListener == null)
			return;

		Destroy(iapListener);
	}

	public IAPListener GetIAPListener()
	{
		return GetComponent<IAPListener>();
	}

	/*
	 * Platform
	 */
	public void CreatePlatform()
	{
		if (GetPlatform() != null)
			return;

		Platform.CreateOnGameObject(gameObject);
	}

	public Platform GetPlatform()
	{
		return GetComponent<Platform>();
	}

	#endregion

	//-------------------------------------------------------------------------
	// Game functions.
	//-------------------------------------------------------------------------
	private void GameRelease()
	{
		// Force process event
		RequestMgr.Inst.FlushAllRequest();

		// Dispose request manager.
		RequestMgr.Inst.Dispose();

		// Dispose all system modules.
		if (SysModuleManager.Instance != null)
			SysModuleManager.Instance.DisposeSysMdls(true);
	}

	private void GameUpdate()
	{
		// Update Request manager.
		RequestMgr.Inst.OnUpdate();

		SysModuleManager.Instance.OnUpdate();
	}

	private void GameGUIUpdate()
	{
		SysModuleManager.Instance.UpdateGUI();
	}

	//-------------------------------------------------------------------------
	// 网络状态控制
	//-------------------------------------------------------------------------
	// 非断线重连机制下, 检查网络状态
	private void CheckNetworkConnection()
	{
		if (SysModuleManager.Instance != null)
		{
			var sysGameStateMachine = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>();
			if (sysGameStateMachine != null)
			{
				var currentState = sysGameStateMachine.GetCurrentState();
				if (currentState != null && currentState.IsGamingState)
				{
					StartCoroutine("DoCheckNetworkConnection");
				}
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator DoCheckNetworkConnection()
	{
		yield return null;

		int status = RequestMgr.Inst.NetStatus;
		if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_NEEDQUERYINITINFO)
			SysGameStateMachine.Instance.EnterState<GameState_RetriveGameData>(null, true);
		else if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_DISCONNECTED)
			SysGameStateMachine.Instance.EnterState<GameState_RetriveGameData>(null, true);
		else if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_CLOSED)
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>(null, true);
	}

	// 等待网络反馈回调
	public void OnConnectionBusy(bool busy)
	{
		if (busy)
			SysUIEnv.Instance.ShowUIModule(typeof(UITipIndicator));
		else
			SysUIEnv.Instance.HideUIModule(typeof(UITipIndicator));
	}

	// 网络连接中断回调,这时候需要重新登录
	private bool brokenDlgShown = false;
	public void OnConnectionBroken(string brokenMessage, bool isRelogin)
	{
		Debug.Log("OnConnectionBroken " + brokenMessage);

		// 防止重复显示断线框
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && brokenDlgShown)
			return;
		brokenDlgShown = true;

		// 解除UI锁定
		SysUIEnv.Instance.UnlockUIInput();
		while (UIManager.instance.blockInput)
			UIManager.instance.UnlockInput();

		// 停止新手处理, 重新登录的时候会删除.
		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Broken();

		// 停止活动处理,重新登录的时候会删除, 在这里面不能够删除, 会有其他模块的访问
		if (ActivityManager.Instance != null)
			ActivityManager.Instance.Pause = true;

		// 提示用户重新登录
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		if (isRelogin)
		{
			okCallback.Callback = (userData) =>
			{
				// 强制重新登录(包括在GameState_Upgrading的状态)
				SysGameStateMachine.Instance.EnterState<GameState_Upgrading>(null, true);
				brokenDlgShown = false;
				return true;
			};
			//if (brokenMessage == null || brokenMessage.Equals(""))
			//	brokenMessage = GameUtility.GetUIString("UIDlgMessage_Message_ReConnect");
		}
		else
		{
			okCallback.Callback = (userData) =>
			{
				SysGameStateMachine.Instance.EnterState<GameState_RetriveGameData>(null, true);
				brokenDlgShown = false;
				return true;
			};
			//if (brokenMessage == null || brokenMessage.Equals(""))
			//	brokenMessage = GameUtility.GetUIString("UIDlgMessage_DisConnecMessage");
		}

		if (brokenMessage == null || brokenMessage.Equals(""))
			brokenMessage = GameUtility.GetUIString("UIDlgMessage_DisConnecMessage");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"),
			brokenMessage,
			false,
			null,
			okCallback);

		// 强制显示到最高层
		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}

	// 网络超时回调
	private bool timeOutDlgShown = false;
	public void OnConnectionTimeOut()
	{
		Debug.Log("OnConnectionTimeOut");

		// 如果当前处于UI锁定状态, 很难恢复锁定, 只能重新连接IS
		if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level <= ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.tutorialEndLevel)
		{
			// 主动断线, 防止受到后续的消息
			RequestMgr.Inst.Bussiness.DisconnectAS();
			RequestMgr.Inst.Bussiness.DisconnectIS();
			OnConnectionOutOfSync();
		}
		else
		{
			// 防止重复显示断线框
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && timeOutDlgShown)
				return;
			timeOutDlgShown = true;

			// 提示用户重试
			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
			okCallback.Callback = (userData) =>
			{
				// 重试
				RequestMgr.Inst.Reconnect();
				timeOutDlgShown = false;
				return true;
			};

			// 提示"网络不给力, 请重试"
			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(
				GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"),
				GameUtility.GetUIString("UIDlgMessage_OnConnectionTimeOut"),
				false,
				null,
				okCallback);

			SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
		}
	}

	// 收到服务器消息时候调用, 用于关闭重连对话框
	public void OnConnectionReceiveResponse()
	{
		if (timeOutDlgShown)
		{
			Debug.Log("OnConnectionReceiveResponse");

			// 当前正在等待重连操作, 关闭重连对话框
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgMessage));
			timeOutDlgShown = false;

			// 重置等待状态
			RequestMgr.Inst.ResetBusyState();
		}
	}

	// 网络消息不同步回调, 这时候需要重连IS
	private bool desynchronyDlgShown = false;
	public void OnConnectionOutOfSync()
	{
		Debug.Log("OnConnectionOutOfSync");

		// 防止重复显示断线框
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && desynchronyDlgShown)
			return;
		desynchronyDlgShown = true;

		// 解除UI锁定
		SysUIEnv.Instance.UnlockUIInput();
		while (UIManager.instance.blockInput)
			UIManager.instance.UnlockInput();

		// 停止新手处理, 重新登录的时候会删除.
		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Broken();

		// 停止活动处理, 重新登录的时候会删除, 在这里面不能够删除, 会有其他模块的访问
		if (ActivityManager.Instance != null)
			ActivityManager.Instance.Pause = true;

		// 提示用户重新连接IS
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			// 强制重新连接IS
			SysGameStateMachine.Instance.EnterState<GameState_RetriveGameData>(null, true);
			desynchronyDlgShown = false;
			return true;
		};

		// 提示"网络异常, 请重试"
		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"),
			GameUtility.GetUIString("UIDlgMessage_OnConnectionOutOfSync"),
			false,
			null,
			okCallback);

		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}

	// 配置文件不同步之后的处理
	private bool configOutOfSyncDlgShown = false;
	public void OnConfigOutOfSync(string brokenMessage)
	{
		Debug.Log("OnConfigOutOfSync " + brokenMessage);

		// 防止重复显示断线框
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && configOutOfSyncDlgShown)
			return;

		configOutOfSyncDlgShown = true;

		// 强制断线断线, 防止收到后续的消息
		RequestMgr.Inst.Bussiness.DisconnectAS();
		RequestMgr.Inst.Bussiness.DisconnectIS();

		// 解除UI锁定
		SysUIEnv.Instance.UnlockUIInput();
		while (UIManager.instance.blockInput)
			UIManager.instance.UnlockInput();

		// 停止新手处理, 重新登录的时候会删除.
		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Broken();

		// 停止活动处理,重新登录的时候会删除, 在这里面不能够删除, 会有其他模块的访问
		if (ActivityManager.Instance != null)
			ActivityManager.Instance.Pause = true;

		// 提示用户重新登录
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			// 强制重新登录(包括在GameState_Upgrading的状态)
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>(null, true);
			configOutOfSyncDlgShown = false;
			return true;
		};

		if (brokenMessage == null || brokenMessage.Equals(""))
			brokenMessage = GameUtility.GetUIString("UIDlgMessage_DisConnecMessage");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"),
			brokenMessage,
			false,
			null,
			okCallback);

		// 强制显示到最高层
		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}
}
