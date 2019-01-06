using UnityEngine;
using System.Collections;
using System.Text;
using ClientServerCommon;

public class UIDlgLogin : UIModule
{
	//Buttons
	public UITextField accountInputCtrl;
	public UITextField passwordInputCtrl;
	public UIButton qickStartBtn;
	public UIButton quickStartBtnLeftArrow, quickStartBtnRightArrow;
	public UIButton registerBtn;

	private bool savedPasswordChanged;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		accountInputCtrl.placeHolder = Platform.Instance.LoginAccountInputPlaceholder;
		passwordInputCtrl.placeHolder = Platform.Instance.PasswordInputPlaceholder;

		accountInputCtrl.SetValueChangedDelegate((obj) =>
		{
			ClearPasseword();
		});


		passwordInputCtrl.SetValueChangedDelegate((obj) =>
		{
			savedPasswordChanged = true;
		});

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		qickStartBtn.Hide(SysPrefs.Instance.HasAccountLogined);
		quickStartBtnLeftArrow.Hide(SysPrefs.Instance.HasAccountLogined);
		quickStartBtnRightArrow.Hide(SysPrefs.Instance.HasAccountLogined);
		if (SysPrefs.Instance.QuickLogin)
		{
			// Guest didn't show account and password
			accountInputCtrl.Text = "";
			passwordInputCtrl.Text = "";
			savedPasswordChanged = true;
		}
		else
		{
			var sb = new System.Text.StringBuilder();
			int pwLength = SysPrefs.Instance.PasswordLength;
			for (int i = 0; i < pwLength; ++i)
				sb.Append("*");

			accountInputCtrl.Text = SysPrefs.Instance.Account;
			passwordInputCtrl.Text = sb.ToString();
			savedPasswordChanged = false;
		}

		return true;
	}

	private void ClearPasseword()
	{
		passwordInputCtrl.Text = "";
		savedPasswordChanged = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnQuickRegister(UIButton btn)
	{
		//Call the quick login handler.
		SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().QuickLogin();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnRegister(UIButton btn)
	{
#if !USE_LOGIN_UI_2
		HideSelf();
#endif
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgRegister));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnLogin(UIButton btn)
	{
		if (savedPasswordChanged)
			SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().Login(
				accountInputCtrl.Text, passwordInputCtrl.Text, false, passwordInputCtrl.Text.Length);
		else
			SysGameStateMachine.Instance.GetCurrentState<GameState_Login>().Login(
				SysPrefs.Instance.Account, SysPrefs.Instance.Password, SysPrefs.Instance.IsPasswordEncrypted, SysPrefs.Instance.PasswordLength);
	}

	public void OnLoginSuccess()
	{
		HideSelf();
	}

	public void OnLoginFailed()
	{
		ClearPasseword();
	}
}
