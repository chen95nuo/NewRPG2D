#define ENABLE_UPGRADE_LOG
#define ENABLE_DELAY_CONFIG
//#define ENABLE_GETWAYSERVER_IN_EDITOR // pc端是否开启网关服务器
using UnityEngine;
using System;
using System.Collections;
using ClientServerCommon;
using KodGames.ExternalCall;
using LitJson;

public class GameState_Upgrading : GameStateBase
{
	private enum _State
	{
		Started,
		WaitingGateServer,
		ServerMainTain,
		StartManifest,
		WaitingManifest,
		NoticeUpgradeClient,
		CheckingGameConfig,
		UpgradingGameConfig,
		LoadingGameConfig,
		CheckingGameAssets,
		UpgradingGameAssets,
		LoadingGameAssets,
	}

	private class StepData
	{
		public _State state;
		public float progress;
		public string message;

		public StepData(_State state, float progress, string message)
		{
			this.state = state;
			this.progress = progress;
			this.message = message;
		}
	}

	private static readonly StepData[] stepData = 
	{ 
		new StepData(_State.Started, 0.01f, "UIPnlLoadingBackground_Init"),
		new StepData(_State.WaitingManifest, 0.02f, "UIPnlLoadingBackground_ConnectGateServer"),
		new StepData(_State.WaitingManifest, 0.04f, "UIPnlLoadingBackground_CheckVersion"),
		new StepData(_State.UpgradingGameConfig, 0.06f, "UIPnlLoadingBackground_DownloadConfig"),
		new StepData(_State.LoadingGameConfig, 0.10f, "UIPnlLoadingBackground_LoadingConfig"),
		new StepData(_State.UpgradingGameAssets, 0.11f, "UIPnlLoadingBackground_DownloadAsset"),
		new StepData(_State.LoadingGameAssets, 0.99f, "UIPnlLoadingBackground_LoadingResource"),
	};

	private _State state;
	private UIPnlLoginBackground uiLoginBackround;
	private string baseResourceUpdateURL;
	private ClientManifest.FileInfo gameConfigFileInfo = new ClientManifest.FileInfo();

	public override bool IsGamingState { get { return false; } }

	public override void Enter()
	{
		// Maybe blocked when loading. Hide loading UI
		UIPnlLoading.HidePanel(false);

		// 需要在一开始清除数据,不能够延迟
		// UnLock the UI.
		SysUIEnv.Instance.UnlockUIInput();

		// Destroy SysTutorial.
		SysModuleManager.Instance.DelSysModule<SysTutorial>();

		// Destroy ActivityManager
		SysModuleManager.Instance.DelSysModule<ActivityManager>();

		// Clear UI Event
		SysUIEnv.Instance.ClearShowEventsList();

		// Clear all last request.
		RequestMgr.Inst.DiscardAllRqsts();

		// Destroy additional splash
		bool guiTextureDestroyed = GameMain.Inst.CachedGuiTexture == null;
		if (KodConfigPlugin.HasAdditionalSplashScreen() == false)
		{
			SysUIEnv.Instance.ShowScreenMask(GameDefines.uiScreenMask);
			if (GameMain.Inst.CachedGuiTexture != null)
			{
				GameObject.DestroyImmediate(GameMain.Inst.CachedGuiTexture);
				guiTextureDestroyed = true;
			}
		}

		// 强制关闭loading界面, 在连续切换场景的时候有可能loading界面关闭动画被停止掉
		UIPnlLoading.HidePanel(true);

		// Show loading bar when no additional splash
		if (guiTextureDestroyed)
			ShowLoadingBar();

		StartCoroutine("LoadProcess");
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_Upgrading.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	public override void Exit()
	{
		uiLoginBackround.ShowLoadingBar(false);

		base.Exit();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadProcess()
	{
		// 加载scene的时候会卸载UI,UI中可能使用Player数据,需要再加载之前清空数据
		SysLocalDataBase.Inst.ClearAllData(true);

		// 清空原有助手
		SysAssistant.Instance.ClearData();

		/*
		 * Load scene
		 */
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.loginSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		// Destroy splash.
		if (GameMain.Inst.CachedGuiTexture != null)
		{
			SysUIEnv.Instance.ShowScreenMask(GameDefines.uiScreenMask);
			GameObject.Destroy(GameMain.Inst.CachedGuiTexture);

			ShowLoadingBar();
		}
		// Show login border after loading scene
		SetLoadingState(_State.Started);

		// Create platform
		if (Platform.Instance == null)
			GameMain.Inst.CreatePlatform();

		// Wait for plating initialization
		while (Platform.Instance.WaitingForInitialize)
			yield return null;

		// Record StartGame.
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.StartGame);


		// Check client update
		Platform.Instance.CheckUpdate();
		while (Platform.Instance.HasPassUpdateCheck == false)
			yield return null;

		// 从网关服务器请求登陆服务器信息
#if !UNITY_EDITOR || ENABLE_GETWAYSERVER_IN_EDITOR
		QueryGateServer();
		while (state == _State.WaitingGateServer || state == _State.ServerMainTain)
			yield return null;
#endif

		/*
		 * Retrieve game version (contain manifest)
		 */
		QueryManifest();
		while (state == _State.WaitingManifest)
			yield return null;

		/*
		 * Upgrade game client if needed
		 */
		while (state == _State.NoticeUpgradeClient)
			yield return null;

		/*
		 * Upgrade game config if need
		 */
		SysModuleManager.Instance.AddSysModule<ResourceDownloader>(false);
		bool needUpgradeGameConfig = ResourceDownloader.Instance.CheckLocalGameConfig(gameConfigFileInfo) == false;
        //needUpgradeGameConfig = false;
		if (needUpgradeGameConfig)
		{
#if ENABLE_UPGRADE_LOG
			Debug.Log(string.Format("[GameState_Upgrading] Upgrading Config, URL:{0} File:{1}, Size:{2}", baseResourceUpdateURL, gameConfigFileInfo.fileName, gameConfigFileInfo.fileSize));
#endif
			SetLoadingState(_State.UpgradingGameConfig);

			// Record Start Update Config.
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.StartUpdateConfig);

			ResourceDownloader.Instance.StartUpgradingGameConfig(baseResourceUpdateURL, gameConfigFileInfo, false);
			var stepData = GetLoadingState(_State.UpgradingGameConfig);
			float nextStepProgress = GetNextLoadingStateProgress(_State.UpgradingGameConfig);
			int totalDownloadSize = ResourceDownloader.Instance.GetTotalSize();
			string totalSizeString = GameUtility.ToMemorySizeString(totalDownloadSize);
			int previousPrecentageProgress = -1;
			while (ResourceDownloader.Instance.IsLoading())
			{
				// Update progress text
				float downloadProgress = ResourceDownloader.Instance.GetDownloadedSize() / (float)totalDownloadSize;
				float sliderProgress = Mathf.Lerp(stepData.progress, nextStepProgress, downloadProgress);
				int precentageProgress = GameUtility.FloatToPercentageInteger(downloadProgress);

				if (precentageProgress != previousPrecentageProgress)
				{
					previousPrecentageProgress = precentageProgress;
					uiLoginBackround.Step(sliderProgress, GameUtility.FormatUIString(stepData.message, totalSizeString, GameUtility.FloatToPercentage(downloadProgress, false)));
				}

				yield return null;
			}

			if (ResourceDownloader.Instance.GetFailedFileCount() != 0)
			{
#if ENABLE_UPGRADE_LOG
				Debug.Log("[GameState_Upgrading] Error occurs, count : " + ResourceDownloader.Instance.GetFailedFileCount());
#endif
				// Hide loading bar
				uiLoginBackround.ShowLoadingBar(false);

				// Error description dialog to re-query manifest
				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
				okCallback.Callback = (userData) =>
				{
					// Re-query manifest
					StartCoroutine("LoadProcess");
					return true;
				};

				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_UpdateError"), GameUtility.GetUIString("UIDlgMessage_UpdateConfigError"), false, null, okCallback);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);

				yield break;
			}

			if (ResourceDownloader.Instance.GetDamagedFileCount() != 0)
			{
#if ENABLE_UPGRADE_LOG
				Debug.Log("[GameState_Upgrading] file damaged occurs, count : " + ResourceDownloader.Instance.GetFailedFileCount());
#endif
				ResourceDownloader.Instance.DeleteGameConfig(gameConfigFileInfo);

				// Hide loading bar
				uiLoginBackround.ShowLoadingBar(false);	

				// Error description dialog to re-query manifest
				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
				okCallback.Callback = (userData) =>
				{
					// Re-query manifest
					StartCoroutine("LoadProcess");
					return true;
				};

				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_UpdateFileDamage"), GameUtility.GetUIString("UIDlgMessage_UpdateFileDamage"), false, null, okCallback);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);

				yield break;
			}

			// Record Finish Update Config.
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.FinishUpdateConfig);
		}

		/* 
		 * Load game config
		 */
		bool firstLoadConfig = ConfigDatabase.DefaultCfg.ClientManifest == null;
		if (firstLoadConfig || needUpgradeGameConfig)
		{
			SetLoadingState(_State.LoadingGameConfig);
			LoadGameConfigFromAB(gameConfigFileInfo.fileName);
		}

		/*
		 * Upgrade assets if need
		 */
		SetLoadingState(_State.CheckingGameAssets);
		bool needUpgradeGameAssets = ResourceDownloader.Instance.CheckLocalAssets() == false;
		if (needUpgradeGameAssets)
		{
			// Record Start Update Asset.
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.StartUpdateAsset);

			// Upgrading game asset
			ResourceDownloader.Instance.StartUpgradingAssets(baseResourceUpdateURL, false);

			var stepData = GetLoadingState(_State.UpgradingGameAssets);
			float nextStepProgress = GetNextLoadingStateProgress(_State.UpgradingGameAssets);
			int totalDownloadSize = ResourceDownloader.Instance.GetTotalSize();
			string totalSizeString = GameUtility.ToMemorySizeString(totalDownloadSize);
			int previousPrecentageProgress = 0;
			while (ResourceDownloader.Instance.IsLoading())
			{
				// Update progress text
				float downloadProgress = ResourceDownloader.Instance.GetDownloadedSize() / (float)totalDownloadSize;
				float sliderProgress = Mathf.Lerp(stepData.progress, nextStepProgress, downloadProgress);
				int precentageProgress = GameUtility.FloatToPercentageInteger(downloadProgress);

				if (precentageProgress != previousPrecentageProgress)
				{
					previousPrecentageProgress = precentageProgress;
					uiLoginBackround.Step(sliderProgress, GameUtility.FormatUIString(stepData.message, totalSizeString, GameUtility.FloatToPercentage(downloadProgress, false)));
				}

				yield return null;
			}

			if (ResourceDownloader.Instance.GetFailedFileCount() != 0)
			{
#if ENABLE_UPGRADE_LOG
				Debug.Log("[GameState_Upgrading] Error occurs, count : " + ResourceDownloader.Instance.GetFailedFileCount());
#endif
				// Hide loading bar
				uiLoginBackround.ShowLoadingBar(false);

				// Error description dialog to re-query manifest
				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
				okCallback.Callback = (userData) =>
				{


					// Re-query manifest
					StartCoroutine("LoadProcess");
					return true;
				};

				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_UpdateError"), GameUtility.GetUIString("UIDlgMessage_UpdateAssetError"), false, null, okCallback);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);

				yield break;
			}

			if (ResourceDownloader.Instance.GetDamagedFileCount() != 0)
			{
#if ENABLE_UPGRADE_LOG
				Debug.Log("[GameState_Upgrading] file damaged occurs, count : " + ResourceDownloader.Instance.GetFailedFileCount());
#endif
				ResourceDownloader.Instance.DeleteLocalAssets();

				// Hide loading bar
				uiLoginBackround.ShowLoadingBar(false);

				// Error description dialog to re-query manifest
				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
				okCallback.Callback = (userData) =>
				{
					// Re-query manifest
					StartCoroutine("LoadProcess");
					return true;
				};

				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_UpdateFileDamage"), GameUtility.GetUIString("UIDlgMessage_UpdateFileDamage"), false, null, okCallback);
				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);

				yield break;
			}

			// Record Finish Update Asset.
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.FinishUpdateAsset);
		}

		SetLoadingState(_State.LoadingGameAssets);
#if ENABLE_DELAY_CONFIG
		// Delay UnLoad.
		if (firstLoadConfig || needUpgradeGameConfig)
			ConfigDatabase.DefaultCfg.DelayUnloadConfig();
#endif

		// ReLoad Background Card.
		if (needUpgradeGameAssets || needUpgradeGameAssets)
			SysUIEnv.Instance.GetUIModule<UIPnlLoginBackground>().SetBackGroundIcon();

		/*
		 * Go to login state
		 */
		SysGameStateMachine.Instance.EnterState<GameState_Login>();
	}

	private void ShowLoadingBar()
	{
		var uiEnv = SysUIEnv.Instance;
		uiEnv.ShowUIModule(typeof(UIPnlLoginBackground));
		uiLoginBackround = uiEnv.GetUIModule<UIPnlLoginBackground>();
		uiLoginBackround.ShowLoadingBar(false);
	}

	private void SetLoadingState(_State state)
	{
		this.state = state;

		var data = GetLoadingState(state);
		if (data != null)
		{
			uiLoginBackround.ShowLoadingBar(true);
			uiLoginBackround.Step(data.progress, GameUtility.GetUIString(data.message));
		}
	}

	private StepData GetLoadingState(_State state)
	{
		foreach (var data in stepData)
			if (data.state == state)
				return data;

		return null;
	}

	private float GetNextLoadingStateProgress(_State state)
	{
		foreach (var data in stepData)
			if (data.state > state)
				return data.progress;

		return 1f;
	}

	private bool LoadGameConfigFromAB(string fileName)
	{
#if ENABLE_UPGRADE_LOG
		Debug.Log("[GameState_Upgrading] LoadGameConfigFromAB : " + fileName);
#endif
		string filePath = ResourceManager.Instance.GetLocalFilePath(fileName);
		AssetBundle ab = AssetBundle.CreateFromFile(filePath);
		if (ab == null)
		{
			Debug.LogError("Load Game Config failed : " + filePath);
			return false;
		}

		IFileLoader fileLoader = new FileLoaderFromAssetBundle(ab);
#if UNITY_EDITOR
		ConfigSetting cfgSetting = GameDefines.SetupConfigSetting(new ConfigSetting(Configuration._FileFormat.Xml));
#else
		ConfigSetting cfgSetting = GameDefines.SetupConfigSetting(new ConfigSetting(Configuration._FileFormat.ProtoBufBinary));
#endif

#if !ENABLE_DELAY_CONFIG
		// Unload old config
		ConfigDatabase.DefaultCfg.UnloadConfig(typeof(ClientConfig), typeof(StringsConfig), typeof(SceneConfig));
		System.GC.Collect();

		ConfigDatabase.DefaultCfg.LoadGameConfig(fileLoader, cfgSetting);
#else
		// Delay unload config
		ConfigDatabase.DelayLoadFileDel = ConfigDelayLoader.DelayLoadConfig;

		// Reload ClientManifest
		ConfigDatabase.DefaultCfg.LoadConfig<ClientManifest>(fileLoader, cfgSetting);
		Debug.Log(ConfigDatabase.DefaultCfg.ClientManifest);
#endif
		// Unload AB
		ab.Unload(true);
		ab = null;
		System.GC.Collect();

		return true;
	}

	private void QueryGateServer()
	{
		SetLoadingState(_State.WaitingGateServer);
		StopCoroutine("DoQueryGateServer");
		StartCoroutine("DoQueryGateServer");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator DoQueryGateServer()
	{
		// GetWay Server.
		JsonData jsonData = new JsonData();
		jsonData["version"] = BundlePlugin.GetMainBundleVersion();
		jsonData["platform"] = DevicePlugin.GetPlatformName();
		jsonData["revision"] = KodConfigPlugin.GetRevision();
		jsonData["channelId"] = KodConfigPlugin.GetChannelId();
		jsonData["subChannel"] = KodConfigPlugin.GetSubChannelId();

		var url = GameUtility.FormatUIString("UIPnlLoadingBackground_GetWayServerUrl",
									 KodConfig.GetServerIP(),
									 KodConfig.GetServerPort(),
									 WWW.EscapeURL(jsonData.ToJson()));

#if ENABLE_UPGRADE_LOG
		Debug.Log(string.Format("GetServer IP Infos : {0}", url));
#endif

		var www = new WWW(url);

		RequestMgr.Inst.RetainBusy();
		yield return www;
		RequestMgr.Inst.ReleaseBusy();

		if (string.IsNullOrEmpty(www.error))
			OnQueryGetWayServerSuccess(www.text);
		else
		{
#if ENABLE_UPGRADE_LOG
			Debug.Log(string.Format("GetServer Error : {0}", www.error));
#endif
			OnQueryGetWayServerError();
		}
		www = null;
	}

	private void OnQueryGetWayServerSuccess(string message)
	{
		try
		{
			// 解析结果
			var jsonData = JsonMapper.ToObject(message);
			int state = Int32.Parse(jsonData["state"].ToString());

			// Server is MainTain else.
			if (state == 0)
			{
				var errMessage = jsonData["retMsg"].ToString();

				// 服务器处于维护状态, 显示提示（不能点击）
				UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
				showData.SetData(errMessage, false, false);
				SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);

				// 进入维护状态，在进入前台的时候重新请求
				SetLoadingState(_State.ServerMainTain);
			}
			else if (state == 1)
			{
				var authServerIP = jsonData["ip"].ToString();
				int authServerPort = Int32.Parse(jsonData["port"].ToString());

				// 服务器正常， 设置登陆服务器地址
				ConfigDatabase.DefaultCfg.ClientConfig.authServerIP = authServerIP;
				ConfigDatabase.DefaultCfg.ClientConfig.authServerPort = authServerPort;

				// 进入下一步状态
				SetLoadingState(_State.StartManifest);
			}
			else // 服务器处理失败
				OnQueryGateServerFail();
		}
		catch (System.Exception exception)
		{
			Debug.Log("GetServer Parse Exception : " + exception.StackTrace);
			OnQueryGetWayServerError();
		}
	}

	// 服务器处理失败
	private void OnQueryGateServerFail()
	{
		// Hide the loadingBar.
		uiLoginBackround.ShowLoadingBar(false);

		// Show error message to reconnect.
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.CallbackData = null;
		okCallback.Callback = delegate(object data)
		{
			QueryGateServer();
			return true;
		};

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"), GameUtility.GetUIString("UIDlgMessage_OnServerOperateFail"), false, null, okCallback);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	// www 发送或者连接失败
	private void OnQueryGetWayServerError()
	{
		// 出错之后重新请求网关服务器

		// Hide the loadingBar.
		uiLoginBackround.ShowLoadingBar(false);

		// Show error message to reconnect.
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.CallbackData = null;
		okCallback.Callback = delegate(object data)
		{
			QueryGateServer();
			return true;
		};

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"), GameUtility.GetUIString("UIDlgMessage_OnConnectionTimeOut"), false, null, okCallback);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	private void QueryManifest()
	{
		// Check GameUpdate.
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.CheckUpdate);

		RequestMgr.Inst.Request(new QueryManifestReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			0,
			KodConfigPlugin.GetRevision(),
			KodConfigPlugin.GetChannelId(),
			KodConfigPlugin.GetSubChannelId(),
			GameUtility.GetDeviceInfo()));

		SetLoadingState(_State.WaitingManifest);
	}

	public void OnQueryManifestSuccess(int appVersion, string appDownloadURL, string appUpdateDesc, string baseResourceUpdateURL, string gameConfigName, int gameConfigSize, int gameConfigUncompressedSize, bool isForcedUpdate)
	{
		// Check GameUpdate Result.
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.CheckUpdateResult);

#if UNITY_EDITOR
		// Use config setting set in build process
		Debug.Log("Using TEST_PRODUCT_UPGRADE");
		gameConfigName = PlayerPrefs.GetString("BuildProduct.GameConfigName", "");
		gameConfigSize = PlayerPrefs.GetInt("BuildProduct.GameConfigSize");
		gameConfigUncompressedSize = PlayerPrefs.GetInt("BuildProduct.GameConfigUncompressedSize");
#endif
		// Check if need upgrade game config
		this.baseResourceUpdateURL = baseResourceUpdateURL;

		gameConfigFileInfo = new ClientManifest.FileInfo();
		gameConfigFileInfo.assetName = "GameConfig";
		gameConfigFileInfo.fileName = gameConfigName;
		gameConfigFileInfo.fileSize = gameConfigSize;
		gameConfigFileInfo.uncompressedFileSize = gameConfigUncompressedSize;
		// Check if need upgrade client
		if (KodConfigPlugin.GetRevision() < appVersion)
		{
			// Record Start Update App.
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.StartUpdateApp);

#if ENABLE_UPGRADE_LOG
			Debug.Log(string.Format("[GameState_Upgrading] Upgrading client, local:{0}, new:{1}, URL:{2}", KodConfigPlugin.GetRevision(), appVersion, appDownloadURL));
#endif

			// Hide the loadingBar.
			uiLoginBackround.ShowLoadingBar(false);

			// Set NoticeUpgradeClient state
			SetLoadingState(_State.NoticeUpgradeClient);

			// Notice platform update client (if this function is available in platform)
			Platform.Instance.UpdateClient();

#if !UNITY_EDITOR
#if UNITY_IPHONE
			MainMenuItem okCallback = new MainMenuItem();
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Update");
			okCallback.CallbackData = appDownloadURL;
			okCallback.Callback = (userData) =>
			{
				// Open upgrade URL
				string clientAppDownloadURL = userData as string;
				if (clientAppDownloadURL != null && clientAppDownloadURL != "")
					Application.OpenURL(clientAppDownloadURL);

				// Return false to prevent closing dialog
				return false;
			};

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			showData.SetData(GameUtility.GetUIString("UIDlgMessage_Title_Update"), appUpdateDesc, false, null, okCallback);
			SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
#elif UNITY_ANDROID
			SelfUpdaterPlugin.Update(appDownloadURL, appUpdateDesc, isForcedUpdate);
#endif
#endif
			return;
		}
		else
			SelfUpdaterPlugin.DeleteUpdateFile();

		OnCheckingGameConfig();
	}

	public void OnQueryManifestFaild(int errCode, string errMsg)
	{
		// Hide the loadingBar.
		uiLoginBackround.ShowLoadingBar(false);

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.CallbackData = null;
		okCallback.Callback = delegate(object data)
		{
			QueryManifest();
			return true;
		};

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"), errMsg, false, null, okCallback);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	public void OnCheckingGameConfig()
	{
#if ENABLE_UPGRADE_LOG
		Debug.Log(string.Format("[GameState_Upgrading] gameConfigFile : {0}, size : {1} ", gameConfigFileInfo.fileName, gameConfigFileInfo.fileSize));
#endif

		SetLoadingState(_State.CheckingGameConfig);
	}

	public void OnApplicationPause(bool pause)
	{
		if (pause == false && state == _State.ServerMainTain)
		{
			// Application enter foreground when ServerMainTain, re-query manifest
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlTip));

			// 重新请求网关服务器
			QueryGateServer();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void UpdateClientCancelReq(string parmas)
	{
		OnCheckingGameConfig();
	}
}