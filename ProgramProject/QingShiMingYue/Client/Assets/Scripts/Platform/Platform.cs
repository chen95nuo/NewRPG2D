using ClientServerCommon;
using UnityEngine;

public class _ProductPublisher
{
	public const int Local = 0;
	public const int KodGames = 1;
	public const int ChangYou = 2;
	public const int Cmge = 3;
	public const int Efun = 4;
}

public abstract class Platform : MonoBehaviour, KodGames.ExternalCall.IAlertDialogListener
{
	// Platform is a singleton.
	private static Platform instance;
	public static Platform Instance { get { return instance; } }

	public static void CreateOnGameObject(GameObject gameObject)
	{
		switch (KodConfig.GetPublisher())
		{
			case _ProductPublisher.KodGames: instance = gameObject.AddComponent<KodPlatform>(); break;
			case _ProductPublisher.ChangYou: instance = gameObject.AddComponent<ChangYouPlatform>(); break;
			case _ProductPublisher.Cmge: instance = gameObject.AddComponent<CmgePlatform>(); break;
			case _ProductPublisher.Efun: instance = gameObject.AddComponent<EfunPlatform>(); break;
			default: Debug.LogError("Invalid publisher type when create platform"); break;
		}

		if (instance != null)
			instance.Initialize();
	}

	// 平台类型
	public int PlatformType
	{
		get
		{
#if UNITY_ANDROID
			return _PlatformType.Android;
#elif UNITY_IPHONE
			if (KodGames.ExternalCall.KodConfigPlugin.IsAppStore())
				return _PlatformType.IPhone;
			else
				return _PlatformType.IPhoneBreak;
#endif
		}
	}

	// 渠道Id
	public int ChannelId
	{
		get { return KodGames.ExternalCall.KodConfigPlugin.GetChannelId(); }
	}

	// 平台相关的id, 有的平台会用到
	protected string platformId;
	public string PlatformId
	{
		get { return platformId; }
		set { platformId = value; }
	}

	// 服务器类型, 登录的时候由服务器发送下来
	private int serverType;
	public int ServerType
	{
		get { return serverType; }
		set { serverType = value; }
	}

	// 初始化
	public virtual void Initialize()
	{
		AddGameAnalytics();
	}

	// 销毁
	public virtual void Destroy()
	{
		// 默认使用Unity退出
		Application.Quit();
	}

	// 如果异步初始化, 在等待中标记为true
	protected bool waitingForInitialize = false;
	public bool WaitingForInitialize
	{
		get { return waitingForInitialize; }
	}

	// 检测客户端更新
	public virtual void CheckUpdate() { }

	// 强制更新客户端, 在游戏确认强制更新之后调用
	public virtual void UpdateClient() { }

	// 等待客户端更新检测结果
	protected bool hasPassUpdateCheck = true;
	public bool HasPassUpdateCheck
	{
		get { return hasPassUpdateCheck; }
	}

	// 注册
	public virtual void Regist(string account, string password, bool isPasswordEncrypted, int passwordLength) { }

	/*
	 * 登录,登出相关
	 */
	// 帐号名称,如"QQ帐号"
	public virtual string AccountTitle { get { return ""; } }

	// 登录帐号输入框中Placeholder文字,对于平台登录无效
	public virtual string LoginAccountInputPlaceholder { get { return ""; } }

	// 注册帐号输入框中Placeholder文字,对于平台登录无效
	public virtual string RegisterAccountInputPlaceholder { get { return ""; } }

	// 帐号相关密码输入框中的Placeholder, 对于平台登录无效
	public virtual string PasswordInputPlaceholder { get { return ""; } }

	// 是否支持平台登录
	public virtual bool IsPlatformLogin() { return false; }

	// 显示登录界面, 支持自定义的登录界面
	public virtual void ShowLoginUIModule(bool show)
	{
		if (show)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgLogin));
		else
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgLogin));
	}

	// 直接登录
	public virtual void Login(string account, string password, bool isPasswordEncrypted, int passwordLength) { }

	// 快速登录
	public virtual void QuickLogin() { }


	// 是否为游客登录
	public virtual bool IsGuest
	{
		get { return default(bool); }
	}

	// 登录时从服务器获取的平台信息
	private KodGames.ClientClass.ChannelMessage channelMessage;
	public virtual KodGames.ClientClass.ChannelMessage ChannelMessage
	{
		get { return channelMessage; }
		set { channelMessage = value; }
	}

	// 登出
	public virtual void Logout(int requestId)
	{
		RequestMgr.Inst.Response(new LogoutRes(requestId, true));
	}

	// 绑定帐号
	public virtual void Bind(string account, string password, bool isPasswordEncrypted, int passwordLength) { }

	// 修改秘密啊
	public virtual void ChangePassword(string account, string password, bool isPasswordEncrypted, string newPassword, bool isNewPasswordEncrypted, int passwordLength) { }

	// 进入游戏服务器时调用
	public virtual void JoinGameArea(int areaID)
	{
		if (SysLocalDataBase.Inst != null && SysLocalDataBase.Inst.LoginInfo != null && SysLocalDataBase.Inst.LocalPlayer != null)
			GameAnalyticsUtility.SetTDGAAccount(SysLocalDataBase.Inst.LoginInfo, SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level);
	}

	/*
	 * 支付相关
	 */
	// 检测是否开启支付
	protected bool CheckPayment()
	{
		if (ConfigDatabase.DefaultCfg.GameConfig.disableRecharge)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UITIP_DisableRecharge"));
			return false;
		}

		return true;
	}

	// 支付
	public virtual void Pay(int goodId)
	{
		Pay(goodId, "0");
	}

	// 支付
	public virtual void Pay(int goodId, string additionalData)
	{
		if (CheckPayment())
		{
			var appGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodId, GameUtility.GetDeviceInfo().DeviceType);
			if (appGoodCfg == null)
				appGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodId, _DeviceType.Unknown);

			// 对于需要等待支付返回的情况, 使用Request控制等待, 在IAPPaymentRequest中会调用IAPListener的PayProduct (默认等待支付结果, 多数情况下适用于苹果支付流程)
			RequestMgr.Inst.Request(new IAPPaymentRequest(appGoodCfg.productId, goodId, 1, additionalData));
		}
	}

	/*
	 * 游戏内平台相关界面显示
	 */

	// 显示/隐藏平台工具条
	public virtual void ShowToolBar(bool show) { }

	// 平台自定义的用户中心名称,如"91社区"
	public string UserCenterTitle { get { return GameUtility.GetUIString("UIPnlSetting_Btn_EnterUserCenter"); } }

	// 显示用户中心按钮
	public virtual void ShowUserCenter() { }

	// 是否有论坛入口
	public virtual bool HasBBS { get { return false; } }

	// 平台自定义的论坛名称
	public string BBSTitle { get { return GameUtility.GetUIString("UIPnlSetting_Btn_EnterBBS"); } }

	// 进入论坛, 默认通过外部链接打开论坛
	public virtual void ShowBBS()
	{
		Application.OpenURL(ConfigDatabase.DefaultCfg.GameConfig.GetOffficialForumURLs(PlatformType, ChannelId));
	}

	// 进入客服中心, 默认显示游戏自定义的反馈界面
	public virtual void ShowCallCenter()
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgFeedBack));
	}

	/*
	 * Facebook相关
	 */
	public virtual void LoginFacebook() { }
	public virtual void FacebookPublishShare() { }
	public virtual void FacebookSendRequest() { }
	public virtual void UpdateResetTime() { }
	// 是否显示facebook分享按钮
	public virtual UIElemCentralCityTempItem.ElemShowFactor GetFacebookShowFactor()
	{
		return UIElemCentralCityTempItem.ElemShowFactor.AlwaysHide;
	}

	/*
	 * Quit on platform
	 */
	public virtual void AndroidQuit()
	{
		KodGames.ExternalCall.AlertDialog.Show(
			"OnAlertDialogResponse",
			GameUtility.GetUIString("UIDlgMessage_Title_Tips"),
			GameUtility.GetUIString("UIDlgMessage_Msg_GameQuit"),
			GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK"),
			GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public virtual void OnAlertDialogResponse(string message)
	{
		bool pressOKNotCancel = false;
		KodGames.ExternalCall.AlertDialogHelper.ParseOnAlertDialogResponse(message, out pressOKNotCancel);
		if (pressOKNotCancel)
			this.Destroy();
	}

	/*
	 * Busy control
	 */
	private int isBusy = 0;

	protected bool IsBusy()
	{
		return isBusy > 0;
	}

	protected void RetainBusy()
	{
		isBusy++;
		RequestMgr.Inst.RetainBusy();
	}

	protected void ReleaseBusy()
	{
		isBusy = System.Math.Max(isBusy - 1, 0);
		RequestMgr.Inst.ReleaseBusy();
	}
	// 获取UDID（有可能平台提供接口）
	public virtual string GetUDID() { return KodGames.ExternalCall.DevicePlugin.GetUDID(); }

	// 游戏统计
	public virtual void AddGameAnalytics() { }

	// 统计游戏数据
	public virtual void UploadGameData(bool isCreatRole) { }

	//拷贝字符串
	public virtual void CopyString(string str) { }
}
