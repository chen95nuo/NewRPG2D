using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgModifyPwd : UIModule
{
	public SpriteText emailTitleLabel;
	public SpriteText emailLabel;
	public UITextField oldPwdLabel;
	public UITextField newPwdLabel;
	public UITextField checkNewPwdLabel;

	//Error massage label.
	public SpriteText errMsgLabel;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		emailTitleLabel.Text = Platform.Instance.AccountTitle;
		oldPwdLabel.placeHolder = Platform.Instance.PasswordInputPlaceholder;
		newPwdLabel.placeHolder = Platform.Instance.PasswordInputPlaceholder;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		ResetControls();
		return true;
	}

	private void ResetControls()
	{
		emailLabel.Text = SysLocalDataBase.Inst.LoginInfo.Account;

		oldPwdLabel.Text = "";
		newPwdLabel.Text = "";
		checkNewPwdLabel.Text = "";

		errMsgLabel.Text = "";
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOkClick(UIButton btn)
	{
		if (!InputValidation())
			return;

		var password = newPwdLabel.Text.TrimEnd();
		Platform.Instance.ChangePassword(SysLocalDataBase.Inst.LoginInfo.Account, oldPwdLabel.Text.TrimEnd(), false, password, false, password.Length);
	}

	public void OnResetPasswordSuccess()
	{
		HideSelf();

		// Back login
		RequestMgr.Inst.Request(new LogoutReq(null));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCancelClick(UIButton btn)
	{
		HideSelf();
	}

	private bool InputValidation()
	{

		if (InputEmpty(newPwdLabel.Text))
		{
			errMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgModifyPwd_ErrMsg_NewPwdEmpty"), GameDefines.txColorRed);
			return false;
		}

		if (InputEmpty(checkNewPwdLabel.Text))
		{
			errMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgModifyPwd_ErrMsg_CheckPwdEmpty"), GameDefines.txColorRed);
			return false;
		}

		if (!newPwdLabel.Text.TrimEnd().Equals(checkNewPwdLabel.Text.TrimEnd()))
		{
			errMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgModifyPwd_ErrMsg_PwdNotMatch"));
			return false;
		}

		// 		if (newPwdLabel.Text.TrimEnd().Length > ConfigDatabase.DefaultCfg.GameConfig.pwdMax)
		// 		{
		// 			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgRegister_label_PwdTooLong", ConfigDatabase.DefaultCfg.GameConfig.pwdMax));
		// 			return false;
		// 		}

		return true;
	}

	private bool InputEmpty(string text)
	{
		if (text == null || text.TrimEnd().Equals(""))
			return true;
		else
			return false;
	}
}
