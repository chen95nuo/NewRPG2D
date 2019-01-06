#define ENABLE_ChangYou_LISTENER_LOG
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ExternalCall;
using KodGames.ExternalCall.Platform;


public class ChangYouPlatform : PlatformWithLoginView, IChangYouPlatformListener, IAnalyticOutput
{
	private int logoutRequestId = Request.InvalidID;
	private bool autoLogin = true;


	private bool CheckClientUpdate
	{
		get { return KodConfigPlugin.GetBoolValue("CheckClientUpdate"); }
	}

	// 初始化SDK
	public override void Initialize()
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] Initialize");
#endif

		base.Initialize();
		ChangYouPlugin.Initialize();

		//初始化, 设置等待标记
		waitingForInitialize = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CyInitializeResponse(string message)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] CyInitializeResponse : " + message);
#endif

		waitingForInitialize = false;
	}

	public override void Destroy()
	{
		ChangYouPlugin.Destroy();
	}

	//pp checkUpdate
	public override void CheckUpdate()
	{
		ChangYouPlugin.CheckUpdate();
		if (CheckClientUpdate)
			hasPassUpdateCheck = false;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CyCheckUpdataResponse(string message)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] CyCheckUpdataResponse : " + message);
#endif

		hasPassUpdateCheck = true;
	}

	//ky update client
	public override void UpdateClient()
	{
		ChangYouPlugin.UpdateClient();
	}

	// 显示登录框
	public override bool ShowPlatformLoginView(bool checkLastShowTime)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] ShowPlatformLoginView : " + checkLastShowTime);
#endif
		// 返回false表示不需要显示登录界面, 已经登录或者频繁点击
		if (base.ShowPlatformLoginView(checkLastShowTime) == false)
			return false;

		ChangYouPlugin.Login(autoLogin);
		return true;
	}

	// 登录结果回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CyLoginResponse(string message)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] CyLoginResponse : " + message);
#endif

		// 登陆成功, 标记下次可以自动登录(不重新获取token)
		autoLogin = true;

		// 解析参数
		string data, channelid, opcode, promoteid, displayName;
		ChangYouHelper.ParseLoginResponse(message, out data, out channelid, out opcode, out promoteid, out displayName);

		// 处理结果, 如果为false, 表示需要延迟处理消息
		if (this.OnPlatformLoginResult(true, "", message, CyLoginResponse) == false)
			return;

		// Success verify with server
		RequestMgr.Inst.Request(new LoginReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			promoteid,
			"",
			displayName,
			string.Format("{0}.{1}", BundlePlugin.GetMainBundleVersion(), KodConfigPlugin.GetRevision().ToString()),
			GameUtility.GetDeviceInfo(),
			KodGames.ClientHelper.AccountChannel.CYAGGREGATE_ACCOUNT,
			KodConfigPlugin.GetChannelId(),
			opcode,
			data,
			false,
			0));
	}

	public override void OnServerLoginFailed()
	{
		// 服务器登陆失败, 下次登陆重新获取token
		autoLogin = false;
		base.OnServerLoginFailed();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnRestartGame(string message)
	{
		bool pressOKNotCancel = false;
		KodGames.ExternalCall.AlertDialogHelper.ParseOnAlertDialogResponse(message, out pressOKNotCancel);
		if (pressOKNotCancel)
			DevicePlugin.ResetGame();
	}

	// 登出
	public override void Logout(int requestId)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] Logout");
#endif
		// 清空以前的本地同名数据
		InterimPositionData.ClearLocalData();
		BattleSceneAngleData.ClearLocalData();
		PackageFilterData.ClearLocalData();

		logoutRequestId = requestId;
		ChangYouPlugin.Logout();
	}

	// 登出回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CyLogoutResponse(string message)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] CyLogoutResponse : " + message);
#endif

		this.OnLogout(true);

		RequestMgr.Inst.Response(new LogoutRes(logoutRequestId, true));
		logoutRequestId = Request.InvalidID;
	}

	// 重载设置渠道信息, 用于设置UserId
	public override KodGames.ClientClass.ChannelMessage ChannelMessage
	{
		get { return base.ChannelMessage; }
		set
		{
			base.ChannelMessage = value;
			ChangYouPlugin.SetUserId(value != null ? value.ChannelUniqueId.ToString() : "");
		}
	}

	// 进入游戏服务器
	public override void JoinGameArea(int areaId)
	{
#if ENABLE_ChangYou91_LISTENER_LOG
		Debug.Log ("[ChangYou91Platform] JoinGameArea : " + areaId);
#endif
		base.JoinGameArea(areaId);

		ChangYouPlugin.SetAccountId(SysLocalDataBase.Inst.LoginInfo.AccountId.ToString());
		ChangYouPlugin.SetServerId(areaId.ToString());
		ChangYouPlugin.SetRoleName(SysLocalDataBase.Inst.LocalPlayer.Name);
		ChangYouPlugin.SetRoleId(SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString());
		ChangYouPlugin.SetLevel(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
		ChangYouPlugin.SetLoginLog();

		// Record EnterGame. 
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.EnterGame);

		// Talking Game SetAccount.
		if (SysLocalDataBase.Inst != null && SysLocalDataBase.Inst.LoginInfo != null && SysLocalDataBase.Inst.LocalPlayer != null)
			GameAnalyticsUtility.SetTDGAAccount(SysLocalDataBase.Inst.LoginInfo, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
	}

	// 支付回调, 支付函数在基类实现
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CyPurchaseResponse(string message)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] CyPurchaseResponse : " + message);
#endif
	}

	// 显示/隐藏平台工具条
	public override void ShowToolBar(bool show)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] ShowToolBar : " + show);
#endif
		ChangYouPlugin.ShowToolBar(show);
	}

	// 显示用户中心
	public override void ShowUserCenter()
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] ShowUserCenter");
#endif
		ChangYouPlugin.ShowUserCenter();
	}

	// 显示客服界面
	public override void ShowCallCenter()
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] ShowCallCenter");
#endif
		ChangYouPlugin.ShowCallCenter(Platform.Instance.ChannelMessage.ChannelUniqueId, SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString(), SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID.ToString());
	}

	// 是否有论坛入口
	public override bool HasBBS { get { return KodConfigPlugin.GetBoolValue("HasBBS"); } }

	//平台获取UDID
	public override string GetUDID()
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] GetUDID");
#endif
		return ChangYouPlugin.GetUDID();
	}

	/*
	* Quit on platform
	*/
	public override void AndroidQuit()
	{
		if (KodConfigPlugin.GetBoolValue("IsPlatform_Quit"))
			ChangYouPlugin.Quit();
		else
			base.AndroidQuit();
	}

	public override void UploadGameData(bool isCreatRole)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] UploadGameData");
#endif
		int playerId = SysLocalDataBase.Inst.LocalPlayer.PlayerId;
		string playerName = SysLocalDataBase.Inst.LocalPlayer.Name;
		int playerLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
		int areaId = SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID;
		string areaName = SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaName;

		ChangYouPlugin.UploadGameData(playerId, playerName, playerLevel, areaId, areaName, isCreatRole);
	}

	public void RecordGameData(string value)
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] RecordGameData : " + value);
#endif
		if (value.Equals(GameRecordType.GetNameByType(GameRecordType.SetPlayerLevel)))
			ChangYouPlugin.SetLevel(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
		else
			ChangYouPlugin.SetEvent(value);
	}

	public override void AddGameAnalytics()
	{
#if ENABLE_ChangYou_LISTENER_LOG
		Debug.Log("[ChangYouPlatform] AddGameAnalytics");
#endif
		SysGameAnalytics.Instance.AddAnalytic(this);
	}
}