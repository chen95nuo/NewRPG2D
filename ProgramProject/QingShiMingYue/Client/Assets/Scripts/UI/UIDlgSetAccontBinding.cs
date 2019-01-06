using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIDlgSetAccontBinding : UIModule
{
	//Account binding form
	public SpriteText playerAccountTitle;
	public UITextField playerAccountTFD;
	public UITextField playerPasswordTFD;
	public UITextField playerCheckPwdTFD;

	//Error message
	public SpriteText errorMsgLabel;

	public override bool Initialize ()
	{
		if (base.Initialize() == false)
			return false;

		playerAccountTitle.Text = Platform.Instance.AccountTitle;
		playerAccountTFD.placeHolder = Platform.Instance.RegisterAccountInputPlaceholder;
		playerPasswordTFD.placeHolder = Platform.Instance.PasswordInputPlaceholder;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		ResetControls();
		return true;
	}

	/// <summary>
	/// Refresh controls status, if success.
	/// </summary>
	private void ResetControls()
	{
		playerAccountTFD.Text = "";
		playerPasswordTFD.Text = "";
		playerCheckPwdTFD.Text = "";

		errorMsgLabel.Text = "";
	}

	public void SetErrorMessage(string errMsg)
	{
		//Set error message.
		errorMsgLabel.Text = errMsg;
	}

	private bool InputValidation()
	{
		if (InputEmpty(playerAccountTFD.Text))
		{
			errorMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgSetAccontBinding_ErrMsg_EmailEmpty"), GameDefines.txColorRed);
			return false;
		}

		if (InputEmpty(playerPasswordTFD.Text))
		{
			errorMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgSetAccontBinding_ErrMsg_PwdEmpty"), GameDefines.txColorRed);
			return false;
		}

		if (InputEmpty(playerCheckPwdTFD.Text))
		{
			errorMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgSetAccontBinding_ErrMsg_CheckPwdEmpty"), GameDefines.txColorRed);
			return false;
		}

		if (!playerPasswordTFD.Text.TrimEnd().Equals(playerCheckPwdTFD.Text.TrimEnd()))
		{
			errorMsgLabel.Text = string.Format(GameUtility.GetUIString("UIDlgSetAccontBinding_ErrMsg_PwdNotMatch"), GameDefines.txColorRed);
			return false;
		}

// 		if (playerPasswordTFD.Text.TrimEnd().Length > ConfigDatabase.DefaultCfg.GameConfig.pwdMax)
// 		{
// 			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIDlgRegister_label_PwdTooLong", ConfigDatabase.DefaultCfg.GameConfig.pwdMax));
// 			return false;
// 		}

		return true;
	}

	private void BindAccount(string email, string password)
	{
		Platform.Instance.Bind(email, password, false, password.Length);
	}

	private bool InputEmpty(string text)
	{
		return text == null || text.TrimEnd().Equals("");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnOkClick(UIButton btn)
	{
		if (!InputValidation())
			return;

		BindAccount(playerAccountTFD.Text.TrimEnd(), playerPasswordTFD.Text.TrimEnd());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCancelClick(UIButton btn)
	{
		HideSelf();
	}
}
