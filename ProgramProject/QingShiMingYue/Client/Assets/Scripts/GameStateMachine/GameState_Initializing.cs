using System;
using System.Collections.Generic;
using ClientServerCommon;

class GameState_Initializing : GameStateBase
{
	public override bool IsGamingState { get { return false; } }

	public override void Create(object userData)
	{
		LoadInitialConfig();
	}

	public override void Dispose()
	{

	}

	public override void Enter()
	{
		// Notice plugin showing game.
#if UNITY_WEBPLAYER
		Application.ExternalCall(GameDefines.wpfShowGame, true);
#endif
		// Add GameAnalytics.
		SysModuleManager.Instance.AddSysModule<SysGameAnalytics>(true);

		// Add Prefers system module.
		SysModuleManager.Instance.AddSysModule<SysPrefs>(true);

		// Add Resource manager
		SysModuleManager.Instance.AddSysModule<ResourceManager>(true);
		SysModuleManager.Instance.AddSysModule<ResourceCache>(true);

		// Add scene manager.
		var sysSceneManager = SysModuleManager.Instance.AddSysModule<SysSceneManager>(true);
		sysSceneManager.AddSceneManagerListener(SysGameStateMachine.Instance);

		// Add UI environment.
		SysModuleManager.Instance.AddSysModule<SysUIEnv>(true);

		// Initialize UI modules
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		foreach (var ui in GameDefines.GetAllUIModuleDatas())
			uiEnv.RegisterUIModule(ui.type, ui.prefabName, ui.moduleType, ui.linkedTypes, ui.hideOtherModules, ui.ignoreMutexTypes);
		uiEnv.ShowUIFilterDel = GameUtility.ShowUIFilter;

		SysModuleManager.Instance.AddSysModule<SysIconManger>(true);

		// Add Input system, need add after SysUIEvn.
		SysModuleManager.Instance.AddSysModule<SysInput>(true);

		// Add Fx system.
		SysModuleManager.Instance.AddSysModule<SysFx>(true);

		// Add AudioManager.
		SysModuleManager.Instance.AddSysModule<AudioManager>(true);
		sysSceneManager.AddSceneManagerListener(AudioManager.Instance);

		// Add LocalDatabase system.
		SysModuleManager.Instance.AddSysModule<SysLocalDataBase>(true);

		// Notification system
		SysModuleManager.Instance.AddSysModule<SysNotification>(true);

		// Add Assistant
		SysModuleManager.Instance.AddSysModule<SysAssistant>(true);

		// Initialize request manager.
		RequestMgr.Inst.Initialze(10f,
			GameMain.Inst.OnConnectionBroken,
			GameMain.Inst.OnConnectionBusy,
			GameMain.Inst.OnConnectionTimeOut,
			GameMain.Inst.OnConnectionReceiveResponse,
			GameMain.Inst.OnConnectionOutOfSync,
			GameMain.Inst.OnConfigOutOfSync);

		SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_Initializing.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	private void LoadInitialConfig()
	{
		// Loading config
#if UNITY_EDITOR
		ConfigDatabase.Initialize(new MathParserFactory(), false, false);
#elif UNITY_IPHONE
		ConfigDatabase.Initialize(new MathParserFactory(), true, true);
#else
		ConfigDatabase.Initialize(new MathParserFactory(), true, true);
#endif

		ConfigDatabase.AddLogger(new ClientServerCommon.UnityLogger());

		ConfigSetting cfgSetting = GameDefines.SetupConfigSetting(new ConfigSetting(Configuration._FileFormat.Xml));
		IFileLoader fileLoader = new FileLoaderFromResourceFolder();
		ConfigDatabase.DefaultCfg.LoadConfig<ClientConfig>(fileLoader, cfgSetting);

		// Following config will be reload from AssetBundle after upgrade game config
		ConfigDatabase.DefaultCfg.LoadConfig<StringsConfig>(fileLoader, cfgSetting);
		ConfigDatabase.DefaultCfg.LoadConfig<SceneConfig>(fileLoader, cfgSetting);
	}
}
