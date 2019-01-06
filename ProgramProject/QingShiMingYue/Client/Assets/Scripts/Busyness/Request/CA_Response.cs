using System;
using System.Collections.Generic;
using KodGames.ClientClass;
using KodGames.ClientHelper;

// 请求版本信息
class QueryManifestRes : Response
{
	private int appVersion;
	private string appDownloadURL;
	private string appUpdateDesc;
	private string baseResourceUpdateURL;
	private string gameConfigName;
	private int gameConfigSize;
	private int gameConfigUncompressedSize;
	private bool isForcedUpdate;

	public QueryManifestRes(int pRqstID, int appVersion, string appDownloadURL, string appUpdateDesc, string baseResourceUpdateURL, string gameConfigName, int gameConfigSize, int gameConfigUncompressedSize, bool isForcedUpdate)
		: base(pRqstID)
	{
		this.appVersion = appVersion;
		this.appDownloadURL = appDownloadURL;
		this.appUpdateDesc = appUpdateDesc;
		this.baseResourceUpdateURL = baseResourceUpdateURL;
		this.gameConfigName = gameConfigName;
		this.gameConfigSize = gameConfigSize;
		this.gameConfigUncompressedSize = gameConfigUncompressedSize;
		this.isForcedUpdate = isForcedUpdate;
	}

	public QueryManifestRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		GameState_Upgrading gameState = SysGameStateMachine.Instance.CurrentState as GameState_Upgrading;
		if (gameState != null)
			gameState.OnQueryManifestSuccess(appVersion, appDownloadURL, appUpdateDesc, baseResourceUpdateURL, gameConfigName, gameConfigSize, gameConfigUncompressedSize, isForcedUpdate);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		GameState_Upgrading gameState = SysGameStateMachine.Instance.CurrentState as GameState_Upgrading;
		if (gameState != null)
			gameState.OnQueryManifestFaild(errCode, errMsg);
	}
}

// 创建帐号
class CreateAccountRes : Response
{
	public CreateAccountRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public CreateAccountRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		//		KodGames.ExternalCall.TalkingDataAppCpaPlugin.Converted();
		//		KodGames.ExternalCall.MZMonitorPlugin.AdTrack();

		var srcReq = request as CreateAccountReq;

		SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().OnCreateAccountSuccess(
			srcReq.Account, srcReq.Password, srcReq.IsPasswordEncrypted, srcReq.PasswordLength);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().OnCreateAccountFalied(errCode, errMsg);
	}
}

// 登录&快速登录
class LoginRes : Response
{
	private int accountId;
	private List<Area> areas;
	private string token;
	private int lastAreaID;
	private bool quickLogin;
	private bool isFirstQuickLogin;
	private bool isShowActivityInterface;
	private KodGames.ClientClass.ChannelMessage channelMessage;

	public LoginRes(int pRqstID, int accountID, List<Area> areas, string token, int lastAreaID, bool quickLogin, bool isFirstQuickLogin, bool isShowActivityInterface, KodGames.ClientClass.ChannelMessage channelMessage)
		: base(pRqstID)
	{
		this.accountId = accountID;
		this.areas = areas;
		this.token = token;
		this.lastAreaID = lastAreaID;
		this.quickLogin = quickLogin;
		this.isFirstQuickLogin = isFirstQuickLogin;
		this.isShowActivityInterface = isShowActivityInterface;
		this.channelMessage = channelMessage;
	}

	public LoginRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		// Record CheckToken Result. 
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.GameLoginSuccess);

		// 保存渠道信息
		Platform.Instance.ChannelMessage = channelMessage;

		// Set login info
		var loginInfo = SysLocalDataBase.Inst.LoginInfo;
		loginInfo.QuickLogin = quickLogin;

		if (quickLogin)
		{
			var srcReq = request as QuickLoginReq;
			loginInfo.Account = string.IsNullOrEmpty(srcReq.BindedAccount) ? accountId.ToString() : srcReq.BindedAccount;
			loginInfo.Password = "";
		}
		else
		{
			var srcReq = request as LoginReq;
			// 保存显示帐号名
			loginInfo.Account = srcReq.DisplayAccount;
			loginInfo.Password = srcReq.Password;
		}

		// 如果服务器能够获取用户名, 使用服务器的用户名
		if (channelMessage != null && !string.IsNullOrEmpty(channelMessage.ChannelUserName))
		{
			loginInfo.Account = channelMessage.ChannelUserName;
		}

		loginInfo.AccountId = accountId;
		loginInfo.ServerAreas = areas;
		loginInfo.LoginToken = token;
		loginInfo.LastAreaId = lastAreaID;
		//loginInfo.LastAreaId = (lastAreaID == -1 && areas.Count != 0 ? areas[0].AreaID : lastAreaID);

		// Save login informations.
		SysPrefs pref = SysModuleManager.Instance.GetSysModule<SysPrefs>();
		pref.AutoLogin = true;
		pref.QuickLogin = quickLogin;
		pref.Account = loginInfo.Account;
		if (quickLogin)
			pref.SavePassword("", false, 0);
		else
		{
			var srcReq = request as LoginReq;
			pref.SavePassword(loginInfo.Password, srcReq.IsPasswordEncrypted, srcReq.PasswordLength);
		}
		SysPrefs.Instance.HasAccountLogined = !quickLogin;
		pref.Save();

		if (isFirstQuickLogin)
		{
			//			KodGames.ExternalCall.TalkingDataAppCpaPlugin.Converted();
			//			KodGames.ExternalCall.MZMonitorPlugin.AdTrack();

			// 第一次登陆 ，清空以前的本地同名数据
			InterimPositionData.ClearLocalData();
			BattleSceneAngleData.ClearLocalData();
			PackageFilterData.ClearLocalData();
		}

		SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().OnLoginSuccess();

		if (isShowActivityInterface)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgActivationCode), request, accountId, token);

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().OnLoginFailed(errCode, errMsg);
	}
}

// 登出
class LogoutRes : Response
{
	private bool success;

	public LogoutRes(int pRqstID, bool success)
		: base(pRqstID)
	{
		this.success = success;
	}

	public LogoutRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (success)
		{
			var logoutReq = request as LogoutReq;
			if (logoutReq != null && logoutReq.LogoutSuccessDel != null)
				logoutReq.LogoutSuccessDel();

			// 成功登出
			RequestMgr.Inst.Bussiness.Logout(0);
			SysPrefs.Instance.AutoLogin = false;
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();

			// 清空本地阵位,本地战斗视角
			InterimPositionData.ClearLocalData();
			BattleSceneAngleData.ClearLocalData();
			PackageFilterData.ClearLocalData();
		}

		return true;
	}
}

// 激活码
class AuthActivityCodeRes : Response
{
	public AuthActivityCodeRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public AuthActivityCodeRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgActivationCode)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgActivationCode));

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		string message = string.Empty;
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_AUTH_ACTIVITY_CODE_FAILED_ACTIVITY_CODE_ALREADY_USED)
			message = GameUtility.GetUIString("UIDlgActivationCode_AlreadyUsed");

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_AUTH_ACTIVITY_CODE_FAILED_ACTIVITY_CODE_ERROR)
			message = GameUtility.GetUIString("UIDlgActivationCode_WrongCode");

		if (errCode == com.kodgames.corgi.protocol.Protocols.E_AUTH_SERVER_UNAVAILABLE)
			GameMain.Inst.OnConnectionBroken(message, true);
		else
			SysUIEnv.Instance.GetUIModule<UIDlgActivationCode>().OnCodeFailed(message);
	}
}

// 绑定帐号
class BindAccountRes : Response
{
	public BindAccountRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public BindAccountRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		BindAccountReq req = request as BindAccountReq;

		SysPrefs pref = SysModuleManager.Instance.GetSysModule<SysPrefs>();
		pref.AutoLogin = false;
		pref.QuickLogin = false;
		pref.Account = req.Account;
		pref.SavePassword(req.Password, req.PasswordEncrypted, req.PasswordLength);
		pref.HasAccountLogined = true;
		pref.Save();

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIDlgSetAccontBinding)))
			SysUIEnv.Instance.HideUIModule<UIDlgSetAccontBinding>();

		SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();

		return true;
	}

	protected override void PrcErr(Request request, int errCode, string errMsg)
	{
		SysUIEnv.Instance.GetUIModule<UIDlgSetAccontBinding>().SetErrorMessage(errMsg);
	}
}

// 重置密码
class ResetPasswordRes : Response
{
	public ResetPasswordRes(int pRqstID)
		: base(pRqstID)
	{
	}

	public ResetPasswordRes(int pRqstID, int errCode, string errMsg)
		: base(pRqstID, errCode, errMsg)
	{
	}

	public override bool Execute(Request request)
	{
		if (!base.Execute(request))
			return false;

		ResetPasswordReq req = request as ResetPasswordReq;

		// Save new password
		SysPrefs.Instance.SavePassword(req.NewPassword, req.PasswordEncrypted, req.PasswordLength);
		SysPrefs.Instance.Save();

		// Notice UI
		SysUIEnv.Instance.GetUIModule<UIDlgModifyPwd>().OnResetPasswordSuccess();

		return true;
	}
}