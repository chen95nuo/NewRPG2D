//#define ENABLE_PLATFORM_LOGIN_LOG
using UnityEngine;
using ClientServerCommon;
using KodGames.ExternalCall;

/*
 * 所有用平台登录界面的平台, 从这里派生
 */
public abstract class PlatformWithLoginView : Platform
{
	protected delegate void UnityMessageDelegate(string message);

	// 使用平台界面登录
	public override bool IsPlatformLogin()
	{
		return true;
	}

	// 统一使用插件方式获取帐号标题
	public sealed override string AccountTitle
	{
		get { return KodConfigPlugin.GetStringValue("Platform_Account_Title"); }
	}

	// 如果平台登录界面的回调函数能够覆盖所有情况, 将LoginViewHasCloseCallback设置为True
	private bool HasLoginViewCloseCallback
	{
		get { return KodConfigPlugin.GetBoolValue("HasLoginViewCloseCallback"); }
	}

	// 是否登出操作会自动显示登录界面
	private bool LogoutWillShowLoginView
	{
		get { return KodConfigPlugin.GetBoolValue("LogoutWillShowLoginView"); }
	}

	// 在HasLoginViewCloseCallback为真的情况下, 准确标记登录框是否显示
	private bool loginViewIsShown = false;

	// 存储需要延迟处理的登录结果
	private UnityMessageDelegate delayLoginDel = null;
	private string delayLoginMessage = "";

	// 标记是否已经登录, 并不一定准确, 有可能登出操作没有通知到客户端. 
	private bool hasLogin = false;

	private float lastShowloginViewTime = float.MinValue;

	// 点击登录按钮登录
	public virtual bool ShowPlatformLoginView(bool checkLastShowTime)
	{
#if ENABLE_PLATFORM_LOGIN_LOG
		Debug.Log("[PlatformWithLoginView] ShowPlatformLoginView, HasLoginViewCloseCallback " + HasLoginViewCloseCallback);
#endif

		// 防止快速重复显示平台登录框
		if (checkLastShowTime && KodGames.TimeEx.realtimeSinceStartup - lastShowloginViewTime < 3f)
			return false;
		lastShowloginViewTime = KodGames.TimeEx.realtimeSinceStartup;

		// 如果有延迟登录信息, 直接调用登录结果
		if (string.IsNullOrEmpty(delayLoginMessage) == false)
		{
#if ENABLE_PLATFORM_LOGIN_LOG
			Debug.Log("[PlatformWithLoginView] ShowLoginUIModule delay login");
#endif

			// 回调会重新调用,平台的登录结果,从而会进入OnPlatformLoginResult
			var msg = delayLoginMessage;
			var del = delayLoginDel;
			delayLoginMessage = "";
			delayLoginDel = null;
			del(msg);

			return false;
		}

		// 1.如果平台登录框的关闭回调能覆盖所有情况, 使用loginViewIsShown准备标记登录框是否显示
		// 2.否则, loginViewIsShown有可能重复, 这时候如果关闭的时候没有重置标记, 会造成平台登录框不会自动弹出, 不过不会中断正常逻辑
		loginViewIsShown = true;

		// Record Platform PlatformLogin.
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.PlatformLogin);

		return true;
	}

	// 显示独立的游戏登录按钮, 点击后显示平台界面
	public override void ShowLoginUIModule(bool show)
	{
#if ENABLE_PLATFORM_LOGIN_LOG
		Debug.Log("[PlatformWithLoginView] ShowLoginUIModule " + show);
#endif

		if (show)
		{
			// 如果能够准确记录登录框的显示状态, 直接显示平台登录框, 不显示登录按钮了
			if (HasLoginViewCloseCallback)
			{
#if ENABLE_PLATFORM_LOGIN_LOG
				Debug.Log("[PlatformWithLoginView] ShowLoginUIModule HasLoginViewCloseCallback " + HasLoginViewCloseCallback);
#endif
				if (loginViewIsShown == false)
					this.ShowPlatformLoginView(false);
			}
			else
			{
#if ENABLE_PLATFORM_LOGIN_LOG
				Debug.Log("[PlatformWithLoginView] ShowLoginUIModule HasLoginViewCloseCallback " + HasLoginViewCloseCallback);
#endif
				// 显示登录按钮
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgLoginPlatform));

				// 如果平台登录框已经显示, 不再自动显示登录框
				if (loginViewIsShown == false)
				{
#if ENABLE_PLATFORM_LOGIN_LOG
					Debug.Log("[PlatformWithLoginView] ShowLoginUIModule loginViewIsShown " + loginViewIsShown);
#endif
					SysUIEnv.Instance.GetUIModule<UIDlgLoginPlatform>().ShowLoginView();
				}
			}
		}
		else
		{
#if ENABLE_PLATFORM_LOGIN_LOG
			Debug.Log("[PlatformWithLoginView] ShowLoginUIModule hide UIDlgLoginPlatform");
#endif

			SysUIEnv.Instance.HideUIModule(typeof(UIDlgLoginPlatform));
		}
	}

	// 当收到登录结果的时候调用, 返回false, 表示当前需要延迟处理登录结果
	protected bool OnPlatformLoginResult(bool success, string errMsg, string platformLoginInfo, UnityMessageDelegate delayLoginDel)
	{
#if ENABLE_PLATFORM_LOGIN_LOG
		Debug.Log(string.Format("[PlatformWithLoginView] OnPlatformLoginResult {0},{1},{2},{3}", success, errMsg, platformLoginInfo, delayLoginDel));
#endif

		// 不管能不能准确标记关状态, 全都重置loginViewIsShown
		loginViewIsShown = false;

		if (success)
		{
			if (hasLogin)
			{
#if ENABLE_PLATFORM_LOGIN_LOG
				Debug.Log("[PlatformWithLoginView] OnPlatformLoginResult has login");
#endif
				// 唯一可能是平台内登出的时候没有回调通知. 这时候需要执行登出操作, 然后延迟登录
				OnLogout(true);
				RequestMgr.Inst.Response(new LogoutRes(0, true));
				this.delayLoginMessage = platformLoginInfo;
				this.delayLoginDel = delayLoginDel;

				return false;
			}

			if (SysGameStateMachine.Instance.GetCurrentState<GameState_Login>() == null)
			{
#if ENABLE_PLATFORM_LOGIN_LOG
				Debug.Log("[PlatformWithLoginView] OnPlatformLoginResult not in GameState_Login");
#endif
				// 有可能是从平台切换帐号的回调, 如果当前不再登录状态, 记录登录结果, 当再次进入到GameState_Login, 继续处理
				this.delayLoginMessage = platformLoginInfo;
				this.delayLoginDel = delayLoginDel;

				return false;
			}

			// 只有在GameState_Login的时候才标记登录状态
			hasLogin = true;

			// 埋点
			SysGameAnalytics.Instance.RecordGameData(GameRecordType.PlatformLoginSuccess);

		}
		else
		{
			// Failed show error message
			if (string.IsNullOrEmpty(errMsg) == false)
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errMsg);

			if (HasLoginViewCloseCallback)
			{
				// 重新打开平台登录框
				this.ShowPlatformLoginView(false);
			}
		}

		return true;
	}

	public virtual void OnServerLoginFailed()
	{
		RequestMgr.Inst.Request(new LogoutReq(null));
	}

	// 收到登出结果的时候调用
	public virtual void OnLogout(bool success)
	{
#if ENABLE_PLATFORM_LOGIN_LOG
		Debug.Log("[PlatformWithLoginView] OnLogout " + success);
#endif

		if (success)
		{
			// 标记登出状态, 不确定是否在切换帐号的时候一定能够有logout通知, 如果没有,需要在登录结果的时候先logout, 在延迟登录
			hasLogin = false;

			// 登出成功之后, 如果登出会自动显示登录框, 使用loginViewIsShown准确标记登录框显示状态,
			// 否则,再次进入登录状态的时候会主动调用ShowLoginUIModule, 这时候主动显示框			
			if (LogoutWillShowLoginView)
			{
#if ENABLE_PLATFORM_LOGIN_LOG
				Debug.Log("[PlatformWithLoginView] OnLogout loginViewIsShown is true");
#endif
				loginViewIsShown = true;
			}
		}
	}

	// 支付不需要强制等待服务器返回
	private float lastPayViewTime = 0;
	public override void Pay(int goodId)
	{
		Pay(goodId, "0");
	}

	public override void Pay(int goodId, string additionalData)
	{
		if (CheckPayment())
		{
			// 由于支付没有等待, 防止快速重复点击支付登录框
			if (KodGames.TimeEx.realtimeSinceStartup - lastPayViewTime < 1f)
				return;

			lastPayViewTime = KodGames.TimeEx.realtimeSinceStartup;

			var appGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodId, GameUtility.GetDeviceInfo().DeviceType);
			if (appGoodCfg == null)
				appGoodCfg = ConfigDatabase.DefaultCfg.AppleGoodConfig.GetSubGoodInfoById(goodId, _DeviceType.Unknown);

			// 所有平台支付都不等待支付结果
			GameMain.Inst.GetIAPListener().PayProduct(appGoodCfg.productId, goodId, 1, Request.GetAnNewId(), additionalData);
		}
	}
}