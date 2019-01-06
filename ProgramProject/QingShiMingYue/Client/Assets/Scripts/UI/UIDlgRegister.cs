using UnityEngine;
using System.Collections;
using System.Text;
using ClientServerCommon;

public class UIDlgRegister : UIModule
{
	public UITextField accountInputCtrl;
	public UITextField passwordInputCtrl;
	public UITextField checkPwdInputCtrl;

	public override bool Initialize ()
	{
		if (base.Initialize() == false)
			return false;

		accountInputCtrl.placeHolder = Platform.Instance.RegisterAccountInputPlaceholder;
		passwordInputCtrl.placeHolder = Platform.Instance.PasswordInputPlaceholder;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		accountInputCtrl.Text = "";
		passwordInputCtrl.Text = "";
		checkPwdInputCtrl.Text = "";

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();

#if !USE_LOGIN_UI_2
		//Back to the login dialog.
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgLogin));
#endif
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCreateAccountClick(UIButton btn)
	{
		//If the both input is not the same, show the error message and return.
		if (passwordInputCtrl.spriteText.Text != checkPwdInputCtrl.spriteText.Text)
		{
            SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIDlgRegister_Label_PwdNotMatchError"));
			return;
		}
// 		else if (passwordInputCtrl.Text.Trim().Length > ConfigDatabase.DefaultCfg.GameConfig.pwdMax)
// 		{
// 			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgRegister_label_PwdTooLong", ConfigDatabase.DefaultCfg.GameConfig.pwdMax));
// 			return;
// 		}

		// If both is the same, call the create account handler.
		string account = accountInputCtrl.Text.Trim();
		string password = passwordInputCtrl.Text.Trim();
		SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().CreateAccount(account, password, false, password.Length);
	}

	public void OnCreateAccountSuccess()
	{		
	}

	public void OnCreateAccountFailed()
	{
	}
}
