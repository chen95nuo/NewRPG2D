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
		// �Ƕ�������������, �������״̬
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
	// ����״̬����
	//-------------------------------------------------------------------------
	// �Ƕ�������������, �������״̬
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

	// �ȴ����練���ص�
	public void OnConnectionBusy(bool busy)
	{
		if (busy)
			SysUIEnv.Instance.ShowUIModule(typeof(UITipIndicator));
		else
			SysUIEnv.Instance.HideUIModule(typeof(UITipIndicator));
	}

	// ���������жϻص�,��ʱ����Ҫ���µ�¼
	private bool brokenDlgShown = false;
	public void OnConnectionBroken(string brokenMessage, bool isRelogin)
	{
		Debug.Log("OnConnectionBroken " + brokenMessage);

		// ��ֹ�ظ���ʾ���߿�
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && brokenDlgShown)
			return;
		brokenDlgShown = true;

		// ���UI����
		SysUIEnv.Instance.UnlockUIInput();
		while (UIManager.instance.blockInput)
			UIManager.instance.UnlockInput();

		// ֹͣ���ִ���, ���µ�¼��ʱ���ɾ��.
		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Broken();

		// ֹͣ�����,���µ�¼��ʱ���ɾ��, �������治�ܹ�ɾ��, ��������ģ��ķ���
		if (ActivityManager.Instance != null)
			ActivityManager.Instance.Pause = true;

		// ��ʾ�û����µ�¼
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		if (isRelogin)
		{
			okCallback.Callback = (userData) =>
			{
				// ǿ�����µ�¼(������GameState_Upgrading��״̬)
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

		// ǿ����ʾ����߲�
		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}

	// ���糬ʱ�ص�
	private bool timeOutDlgShown = false;
	public void OnConnectionTimeOut()
	{
		Debug.Log("OnConnectionTimeOut");

		// �����ǰ����UI����״̬, ���ѻָ�����, ֻ����������IS
		if (SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level <= ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.tutorialEndLevel)
		{
			// ��������, ��ֹ�ܵ���������Ϣ
			RequestMgr.Inst.Bussiness.DisconnectAS();
			RequestMgr.Inst.Bussiness.DisconnectIS();
			OnConnectionOutOfSync();
		}
		else
		{
			// ��ֹ�ظ���ʾ���߿�
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && timeOutDlgShown)
				return;
			timeOutDlgShown = true;

			// ��ʾ�û�����
			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
			okCallback.Callback = (userData) =>
			{
				// ����
				RequestMgr.Inst.Reconnect();
				timeOutDlgShown = false;
				return true;
			};

			// ��ʾ"���粻����, ������"
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

	// �յ���������Ϣʱ�����, ���ڹر������Ի���
	public void OnConnectionReceiveResponse()
	{
		if (timeOutDlgShown)
		{
			Debug.Log("OnConnectionReceiveResponse");

			// ��ǰ���ڵȴ���������, �ر������Ի���
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgMessage));
			timeOutDlgShown = false;

			// ���õȴ�״̬
			RequestMgr.Inst.ResetBusyState();
		}
	}

	// ������Ϣ��ͬ���ص�, ��ʱ����Ҫ����IS
	private bool desynchronyDlgShown = false;
	public void OnConnectionOutOfSync()
	{
		Debug.Log("OnConnectionOutOfSync");

		// ��ֹ�ظ���ʾ���߿�
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && desynchronyDlgShown)
			return;
		desynchronyDlgShown = true;

		// ���UI����
		SysUIEnv.Instance.UnlockUIInput();
		while (UIManager.instance.blockInput)
			UIManager.instance.UnlockInput();

		// ֹͣ���ִ���, ���µ�¼��ʱ���ɾ��.
		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Broken();

		// ֹͣ�����, ���µ�¼��ʱ���ɾ��, �������治�ܹ�ɾ��, ��������ģ��ķ���
		if (ActivityManager.Instance != null)
			ActivityManager.Instance.Pause = true;

		// ��ʾ�û���������IS
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			// ǿ����������IS
			SysGameStateMachine.Instance.EnterState<GameState_RetriveGameData>(null, true);
			desynchronyDlgShown = false;
			return true;
		};

		// ��ʾ"�����쳣, ������"
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

	// �����ļ���ͬ��֮��Ĵ���
	private bool configOutOfSyncDlgShown = false;
	public void OnConfigOutOfSync(string brokenMessage)
	{
		Debug.Log("OnConfigOutOfSync " + brokenMessage);

		// ��ֹ�ظ���ʾ���߿�
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgMessage)) && configOutOfSyncDlgShown)
			return;

		configOutOfSyncDlgShown = true;

		// ǿ�ƶ��߶���, ��ֹ�յ���������Ϣ
		RequestMgr.Inst.Bussiness.DisconnectAS();
		RequestMgr.Inst.Bussiness.DisconnectIS();

		// ���UI����
		SysUIEnv.Instance.UnlockUIInput();
		while (UIManager.instance.blockInput)
			UIManager.instance.UnlockInput();

		// ֹͣ���ִ���, ���µ�¼��ʱ���ɾ��.
		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Broken();

		// ֹͣ�����,���µ�¼��ʱ���ɾ��, �������治�ܹ�ɾ��, ��������ģ��ķ���
		if (ActivityManager.Instance != null)
			ActivityManager.Instance.Pause = true;

		// ��ʾ�û����µ�¼
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			// ǿ�����µ�¼(������GameState_Upgrading��״̬)
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

		// ǿ����ʾ����߲�
		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}
}
