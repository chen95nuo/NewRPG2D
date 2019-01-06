#define ENABLE_Efun_LISTENER_LOG
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ExternalCall;
using KodGames.ExternalCall.Platform;

public class EfunPlatform : PlatformWithLoginView, IEfunPlatformListener, IAnalyticOutput
{
	private int logoutRequestId = Request.InvalidID;

	bool IsShowCircleUserCenter = false;

	//初始化sdk
	public override void Initialize()
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] Initialize");
#endif

		base.Initialize();
		EfunPlugin.Initialize();

		//初始化，设置等待标记
		waitingForInitialize = true;
	}

	//初始化sdk回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void EfunInitializeResponse(string message)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] EfunInitializeResponse" + message);
#endif

		waitingForInitialize = false;
	}

	//显示登入框
	public override bool ShowPlatformLoginView(bool checkLastShowTime)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] ShowPlatformLoginView" + checkLastShowTime);
#endif

		//返回false表示不需要显示登入框, 因为已经登入或存在频繁点击
		if (base.ShowPlatformLoginView(checkLastShowTime) == false)
		{
			return false;
		}

		EfunPlugin.Login();
		return true;
	}

	//登入成功回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void EfunLoginResponse(string message)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] EfunLoginResponse" + message);
#endif

		//解析参数
		string efuncode, efunmessage, efunsign, efunUserAccount, efunTimestamp, efunuserid;
		EfunHelper.ParseLoginResponse(message, out efuncode, out efunmessage, out efunsign, out efunUserAccount, out efunTimestamp, out efunuserid);

		//处理结果, 如果为false，表示需要延迟处理消息
		if (this.OnPlatformLoginResult(true, "", message, EfunLoginResponse) == false)
		{
			return;
		}

		// Success verify with server
		RequestMgr.Inst.Request(new LoginReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			efunUserAccount,
			efunTimestamp,
			efunUserAccount,
			string.Format("{0}.{1}", BundlePlugin.GetMainBundleVersion(), KodConfigPlugin.GetRevision().ToString()),
			GameUtility.GetDeviceInfo(),
			KodGames.ClientHelper.AccountChannel.EFUN_ACCOUNT,
			KodConfigPlugin.GetChannelId(),
			efunuserid,
			efunsign,
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
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] Logout");
#endif

		// 清空以前的本地同名数据
		InterimPositionData.ClearLocalData();
		BattleSceneAngleData.ClearLocalData();
		PackageFilterData.ClearLocalData();

		logoutRequestId = requestId;

		EfunPlugin.Logout();

		//由于Efun SDK没有登出功能，暂时用这个模拟登出
		this.OnLogout(true);

		RequestMgr.Inst.Response(new LogoutRes(logoutRequestId, true));
		logoutRequestId = Request.InvalidID;
	}

	//登出回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void EfunLogoutResponse(string message)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] EfunLogoutResponse" + message);
#endif

		this.OnLogout(true);

		RequestMgr.Inst.Response(new LogoutRes(logoutRequestId, true));
		logoutRequestId = Request.InvalidID;
	}

	//进入游戏服务器
	public override void JoinGameArea(int areaID)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] JoinGameArea");
#endif

		base.JoinGameArea(areaID);

		//enterGame 埋点
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.EnterGame);

		// Talking Game SetAccount.
		if (SysLocalDataBase.Inst != null && SysLocalDataBase.Inst.LoginInfo != null && SysLocalDataBase.Inst.LocalPlayer != null)
			GameAnalyticsUtility.SetTDGAAccount(SysLocalDataBase.Inst.LoginInfo, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
	}

	//支付成功回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void EfunPurchaseResponse(string messge)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] EfunPurchaseResponse" + messge);
#endif
	}

	// 显示/隐藏平台工具条
	public override void ShowToolBar(bool show)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] ShowToolBar");
#endif

		if (IsShowCircleUserCenter == false)
		{

			int serverCode = SysLocalDataBase.Inst.LoginInfo.LoginArea.AreaID;

			string remark = string.Format("{0}#{1}#{2}", SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString(),
										   serverCode.ToString(), Platform.Instance.ChannelId);

			EfunPlugin.ShowCircleUserCenter(serverCode.ToString(), SysLocalDataBase.Inst.LocalPlayer.Name, SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString(), SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level.ToString(), remark, SysLocalDataBase.Inst.LocalPlayer.PlayerId.ToString());

			IsShowCircleUserCenter = true;
		}
	}

	//是否有BBS
	public override bool HasBBS { get { return KodConfigPlugin.GetBoolValue("HasBBS"); } }

	//获取UDID
	public override string GetUDID()
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] GetUDID");
#endif

		return EfunPlugin.GetUDID();
	}

	public void RecordGameData(string value)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] RecordGameData : " + value);
#endif
		//		if (value.Equals(GameRecordType.GetNameByType(GameRecordType.SetPlayerLevel)))
		//			ChangYouPlugin.SetLevel(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
		//		else
		//			ChangYouPlugin.SetEvent(value);
	}

	//剪切板
	public override void CopyString(string str)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] CopyString : " + str);
#endif

		KodGames.ExternalCall.KodConfigPlugin.Pasteboard(str);
	}

	//facebook分享
	public override void FacebookPublishShare()
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] FacebookPublishShare");
#endif
		//让策划配置相应的facebook参数
		EfunPlugin.FacebookShare(
			GameUtility.GetUIString("FacebookShareName"),
			GameUtility.GetUIString("FacebookShareCaption"),
			GameUtility.GetUIString("FacebookShareDesc"),
			GameUtility.GetUIString("FacebookShareLink"),
			GameUtility.GetUIString("FacebookSharePictureUrl"));
	}

	//facebook分享回调
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void EfunFacebookShareCallBackResponse(string message)
	{
#if ENABLE_Efun_LISTENER_LOG
		Debug.Log("[EfunPlatform] EfunFacebookShareCallBackResponse" + message);
#endif

		//解析参数
		int FBcode;
		string FBmessage;
		EfunHelper.ParseFacebookShareCallBackResponse(message, out FBcode, out FBmessage);

		//code = 1000 表示success
		if (FBcode == 1000)
		{
			//需要确认服务在新加坡是否也这么写
			//if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgShare)))
			//    SysUIEnv.Instance.GetUIModule<UIDlgShare>().FacebookShareSuccess(0);
		}
	}
}