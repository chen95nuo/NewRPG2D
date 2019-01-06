using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ClientServerCommon;
using UnityEngine;
using KodGames.ClientClass;
using System.Runtime.InteropServices;
using KodGames.ExternalCall;

class GameState_Login : GameStateBase
{
	public override bool IsGamingState { get { return false; } }

	public override void Enter()
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlLoginBackground));
		SysUIEnv.Instance.GetUIModule<UIPnlLoginBackground>().PlayAnimation(true);

		if (Platform.Instance.IsPlatformLogin() == false && string.IsNullOrEmpty(SysPrefs.Instance.Account) == false && SysPrefs.Instance.AutoLogin)
		{
			// Auto login
			if (SysPrefs.Instance.QuickLogin)
				this.QuickLogin();
			else
				this.Login(SysPrefs.Instance.Account, SysPrefs.Instance.Password, SysPrefs.Instance.IsPasswordEncrypted, SysPrefs.Instance.PasswordLength);
		}
		else
		{
			Platform.Instance.ShowLoginUIModule(true);
		}

		// 显示广告
#if UNITY_IPHONE
		if (ConfigDatabase.DefaultCfg.GameConfig.isShowAd)
		{
			var viewer = KodGames.iAdViewer.CreateOnObject(GameMain.Inst.gameObject, ADBannerView.Type.Banner, ADBannerView.Layout.Bottom);
			if (viewer != null)
				viewer.Visable = true;
		}
#endif
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_Login.OnEscape");

		Platform.Instance.AndroidQuit();
	}

#endif

	public override void Exit()
	{
		// Hide dialog
		Platform.Instance.ShowLoginUIModule(false);
		SysUIEnv.Instance.HideUIModule(typeof(UIDlgRegister));

		base.Exit();
	}

	public void Login(string account, string password, bool isPasswordEncrypted, int passwordLength)
	{
		Platform.Instance.Login(account, password, isPasswordEncrypted, passwordLength);
	}

	public void QuickLogin()
	{
		Platform.Instance.QuickLogin();
	}

	public void OnLoginSuccess()
	{
		// Refresh login dialog status.
		if (SysUIEnv.Instance.IsUIModuleLoaded(typeof(UIDlgLogin)))
			SysUIEnv.Instance.GetUIModule<UIDlgLogin>().OnLoginSuccess();
		if (SysUIEnv.Instance.IsUIModuleLoaded(typeof(UIDlgLoginPlatform)))
			SysUIEnv.Instance.HideUIModule(typeof(UIDlgLoginPlatform));

		// Enter state select area.
		SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_SelectArea>();
	}

	public void OnLoginFailed(int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_AUTH_SERVER_UNAVAILABLE)
		{
			OnAuthServerUnavailable();
		}
		else
		{
			// Show error tips and notice UI
			if (!string.IsNullOrEmpty(errMsg))
			{
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errMsg);
			}

			if (Platform.Instance.IsPlatformLogin())
			{
				((PlatformWithLoginView)Platform.Instance).OnServerLoginFailed();
			}
			else
			{
				Platform.Instance.ShowLoginUIModule(true);

				// Show error message.
				if (SysUIEnv.Instance.IsUIModuleLoaded(typeof(UIDlgLogin)))
					SysUIEnv.Instance.GetUIModule<UIDlgLogin>().OnLoginFailed();
			}
		}
	}

	public void CreateAccount(string account, string password, bool isPasswordEncrypted, int passwordLength)
	{
		Platform.Instance.Regist(account, password, isPasswordEncrypted, passwordLength);
	}

	public void OnCreateAccountSuccess(string account, string password, bool isPasswordEncrypted, int passwordLength)
	{
		SysUIEnv.Instance.GetUIModule<UIDlgRegister>().OnCreateAccountSuccess();

		// Auto login, Hide login dialog
		this.Login(account, password, isPasswordEncrypted, passwordLength);
	}

	public void OnCreateAccountFalied(int errCode, string errMsg)
	{
		if (errCode == com.kodgames.corgi.protocol.Protocols.E_AUTH_SERVER_UNAVAILABLE)
		{
			OnAuthServerUnavailable();
		}
		else
		{
			// Show error tips and notice UI
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errMsg);

			// Show the error message.
			SysUIEnv.Instance.GetUIModule<UIDlgRegister>().OnCreateAccountFailed();
		}
	}

	private bool UpgradeCallback(object data)
	{
		string upgradeUrl = data as string;
		if (upgradeUrl != null && upgradeUrl != "")
			// Upgrade client
			Application.OpenURL(upgradeUrl);

		return false;
	}

	private void OnAuthServerUnavailable()
	{
		// Error description dialog to re-query manifest
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
			return true;
		};

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIDlgMessage_DisConnecTitle"), GameUtility.GetUIString("UIDlgMessage_DisConnecMessage"), false, null, okCallback);
		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}
}