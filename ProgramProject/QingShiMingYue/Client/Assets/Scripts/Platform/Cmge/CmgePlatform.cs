#define ENABLE_Cmge_LISTENER_LOG
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ExternalCall;
using KodGames.ExternalCall.Platform;

public class CmgePlatform : PlatformWithLoginView, ICmgePlatformListener, IAnalyticOutput
{
	private int logoutRequestId = Request.InvalidID;

	// 初始化SDK
	public override void Initialize()
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] Initialize");
#endif

		base.Initialize();
		CmgePlugin.Initialize();

		//初始化, 设置等待标记
		waitingForInitialize = true;
	}

	// 初始化sdk回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CmgeInitializeResponse(string message)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] CmgeInitializeResponse: " + message);
#endif

		waitingForInitialize = false;
	}

	// 显示登录框
	public override bool ShowPlatformLoginView(bool checkLastShowTime)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] ShowPlatformLoginView: " + checkLastShowTime);
#endif

		// 返回false表示不需要显示登录界面, 已经登录或者频繁点击
		if (base.ShowPlatformLoginView(checkLastShowTime) == false)
		{
			return false;
		}

		CmgePlugin.Login();
		return true;
	}

	// 登入成功回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CmgeLoginResponse(string message)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] CmgeLoginResponse: " + message);
#endif

		//解析参数
		string cmgeSign, channelid, cmgeUserId, cmgeIsVIP, cmgeTimestamp, cmgeUserAccount;
		CmgeHelper.ParseLoginResponse(message, out cmgeSign, out channelid, out cmgeUserId, out cmgeIsVIP, out cmgeUserAccount, out cmgeTimestamp);

		// 处理结果, 如果为false, 表示需要延迟处理消息
		if (this.OnPlatformLoginResult(true, "", message, CmgeLoginResponse) == false)
		{
			return;
		}

		// Success verify with server
		RequestMgr.Inst.Request(new LoginReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			cmgeUserAccount,
			cmgeTimestamp,
			cmgeUserAccount,
			string.Format("{0}.{1}", BundlePlugin.GetMainBundleVersion(), KodConfigPlugin.GetRevision().ToString()),
			GameUtility.GetDeviceInfo(),
			KodGames.ClientHelper.AccountChannel.CMGE_ACCOUNT,
			KodConfigPlugin.GetChannelId(),
			cmgeUserId,
			cmgeSign,
			false,
			0));
	}

	public override void OnServerLoginFailed()
	{
		base.OnServerLoginFailed();
	}

	//登出
	public override void Logout(int requestId)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] Logout");
#endif

		// 清空以前的本地同名数据
		InterimPositionData.ClearLocalData();
		BattleSceneAngleData.ClearLocalData();
		PackageFilterData.ClearLocalData();

		logoutRequestId = requestId;

		CmgePlugin.Logout();

		//由于中手游SDK没有登出功能，暂时用这个模拟登出
		this.OnLogout(true);

		RequestMgr.Inst.Response(new LogoutRes(logoutRequestId, true));
		logoutRequestId = Request.InvalidID;
	}

	//登出回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CmgeLogoutResponse(string message)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] CmgeLogoutResponse: " + message);
#endif

		this.OnLogout(true);

		RequestMgr.Inst.Response(new LogoutRes(logoutRequestId, true));
		logoutRequestId = Request.InvalidID;
	}

	//进入游戏服务器
	public override void JoinGameArea(int areaID)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] JoinGameArea");
#endif

		base.JoinGameArea(areaID);

		CmgePlugin.JoinGameArea(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);

		//enterGame 埋点
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.EnterGame);

		// Talking Game SetAccount.
		if (SysLocalDataBase.Inst != null && SysLocalDataBase.Inst.LoginInfo != null && SysLocalDataBase.Inst.LocalPlayer != null)
			GameAnalyticsUtility.SetTDGAAccount(SysLocalDataBase.Inst.LoginInfo, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
	}

	//支付成功回调,支付函数在基类实现
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CmgePurchaseResponse(string messge)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] CmgePurchaseResponse:" + messge);
#endif
	}

	// 显示/隐藏平台工具条
	public override void ShowToolBar(bool show)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] ShowToolBar:" + show);
#endif

		CmgePlugin.ShowToolBar(show);
	}

	//是否有BBS
	public override bool HasBBS { get { return KodConfigPlugin.GetBoolValue("HasBBS"); } }

	//获取UDID
	public override string GetUDID()
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] GetUDID");
#endif
		return CmgePlugin.GetUDID();
	}

    /*
	* Quit on platform
	*/
    public override void AndroidQuit()
    {
        if (KodConfigPlugin.GetBoolValue("IsPlatform_Quit"))
            CmgePlugin.Quit("OnAlertDialogResponse",
            GameUtility.GetUIString("UIDlgMessage_Title_Tips"),
            GameUtility.GetUIString("UIDlgMessage_Msg_GameQuit"),
            GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK"),
            GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel"));
        else
            base.AndroidQuit();
    }

	public void RecordGameData(string value)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] RecordGameData : " + value);
#endif
		//		if (value.Equals(GameRecordType.GetNameByType(GameRecordType.SetPlayerLevel)))
		//			ChangYouPlugin.SetLevel(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
		//		else
		//			ChangYouPlugin.SetEvent(value);
	}

	public override void AddGameAnalytics()
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] AddGameAnalytics");
#endif
		SysGameAnalytics.Instance.AddAnalytic(this);
	}

	//剪切板
	public override void CopyString(string str)
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] CopyString : " + str);
#endif

		KodGames.ExternalCall.KodConfigPlugin.Pasteboard(str);
	}

	//facebook分享
	public override void FacebookPublishShare()
	{
#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] FacebookPublishShare");
#endif
		//让策划配置相应的facebook参数
		CmgePlugin.FacebookShare(
			GameUtility.GetUIString("FacebookShareName"),
			GameUtility.GetUIString("FacebookShareCaption"),
			GameUtility.GetUIString("FacebookShareDesc"),
			GameUtility.GetUIString("FacebookShareLink"),
			GameUtility.GetUIString("FacebookSharePictureUrl"));
	}

	//facebook分享回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void CmgeFacebookShareCallBackResponse(string message)
	{

#if ENABLE_Cmge_LISTENER_LOG
		Debug.Log("[CmgePlatform] CmgeFacebookShareCallBackResponse : " + message);
#endif

		//解析参数
		int code;
		string msg, result;
		CmgeHelper.ParseFacebookShareCallBackResponse(message, out code, out msg, out result);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgShare)))
			SysUIEnv.Instance.GetUIModule<UIDlgShare>().FacebookShareSuccess(code);

		//		#if ENABLE_KUNLUNTW_LISTENER_LOG
		//		Debug.Log("[KunLunTWPlatform] FacebookFeed" + message);
		//		#endif
		//		int code = 0;
		//		string msg = "";
		//		string result = "";
		//		KunLunHelper.ParseFacebookGetfriendResponse(message, ref code, ref msg, ref result);
		//		
		//		if (code == 0)
		//			RequestMgr.Inst.Request(new FacebookRewardReq(0));
	}
}