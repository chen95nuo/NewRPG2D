using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

class GameState_RetriveGameData : GameStateBase
{
	public override bool IsGamingState { get { return true; } }

	public override void Enter()
	{
		// Remove old IAPListener ,and will create new one later
		GameMain.Inst.RemoveIAPListener();
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

		// Destroy Splash.
		if (GameMain.Inst.guiTexture != null)
			UnityEngine.GameObject.Destroy(GameMain.Inst.guiTexture);

		// Show loading panel
		UIPnlLoading.ShowPanel(() =>
			{
				StartCoroutine("LoadingProgress");
			},
			null);

		// 清空以前的本地同名数据
		InterimPositionData.ClearLocalData();
		BattleSceneAngleData.ClearLocalData();
		PackageFilterData.ClearLocalData();
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_RetriveGameData.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	public override void Exit()
	{
		base.Exit();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public IEnumerator LoadingProgress()
	{
		// Unload background
		SysUIEnv.Instance.UnloadUIModule(typeof(UIPnlLoginBackground));

		// 加载scene的时候会卸载UI,UI中可能使用Player数据,需要再加载之前清空数据
		SysLocalDataBase.Inst.ClearAllData(false);

		/*
		 * Load scene
		 */
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.loginSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		int status = RequestMgr.Inst.NetStatus;
		if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_DISCONNECTED)
			ReconnectIS();
		else if (status == com.kodgames.corgi.protocol.Protocols.E_GAME_NETSTATUS_CLOSED)
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>(null, true);
		else
			QueryInitInfo();
	}

	private void ReconnectIS()
	{
		var area = SysLocalDataBase.Inst.LoginInfo.SearchArea(SysLocalDataBase.Inst.LoginInfo.LastAreaId);
		if (area != null)
			RequestMgr.Inst.Request(new ConnectISReq(
				area.InterfaceServerIP,
				area.Port,
				KodGames.ClientHelper.NetType.NETTYPE_TCP,
				area.AreaID,
				area.NewAreaId,				
				OnReconnectISSuccess,
				OnReconnectISFailed));
	}

	private void OnReconnectISSuccess()
	{
		QueryInitInfo();
	}

	private void OnReconnectISFailed(int errCode, string errMsg)
	{
		// Reload directly
		SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
	}

	// Query player info.
	private void QueryInitInfo()
	{
		RequestMgr.Inst.Request(new QueryInitInfoReq());
		//		RequestMgr.Inst.Request(new QueryGoodsReq(null)); // 断线重连逻辑测试, 先不发送
		//		RequestMgr.Inst.Request(new QueryPositionListReq());
		//		RequestMgr.Inst.Request(new QueryDungeonListReq()); // 断线重连逻辑测试, 先不发送
	}

	public void OnQueryInitInfoSuccess(KodGames.ClientClass.Player player)
	{
		// Initialize IAP system
		GameMain.Inst.CreateIAPListener();

		// Initialize notification system
		SysNotification.Instance.RegisterRemoteNotification();
		SysNotification.Instance.RescheduleAllLoginNotification();

		// In KunLun SDK InitServer will process unfinished payment, call it after IAP listener created.
		Platform.Instance.JoinGameArea(SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID);

		// Add SysTutorial.
		SysModuleManager.Instance.AddSysModule<SysTutorial>(true);
		SysTutorial.Instance.SetQuestList(player.UnDoneTutorials);

		// 如果 新手引导- > 选择角色 未引导，不进入CentralCity，新手引导会引导进入角色选择.
		if (SysTutorial.Instance.CombatEndStep() == false)
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>();
		else
			SysTutorial.Instance.UpdateTutorial();
	}

	public void OnQueryInitInfoFaild(string errorMsg)
	{
		// Show message dlg and reconnect
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
			return true;
		};

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"), GameUtility.GetUIString("UIDlgMessage_DisConnecMessage"), false, null, okCallback);
		SysUIEnv.Instance.HideUIModule<UIDlgMessage>();
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgMessage), _UILayer.TopMost, showData);
	}
}
