#define ENABLE_KUNLUN_LISTENER_LOG
using UnityEngine;
using ClientServerCommon;
using KodGames.ExternalCall;

/// <summary>
/// KunLun listener.
/// </summary>
public class KodPlatform : Platform
{
	private string EncryptPassword(string password, bool isPasswordEncrypted)
	{
		if (isPasswordEncrypted)
			return password;
		else
			return KodGames.Cryptography.Md5Hash(password);
	}

	//-------------------------------------------------------------------------
	// IPlatform.
	//-------------------------------------------------------------------------
	public override void Regist(string account, string password, bool isPasswordEncrypted, int passwordLength)
	{
		RequestMgr.Inst.Request(new CreateAccountReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			account,
			EncryptPassword(password, isPasswordEncrypted),
			KodConfigPlugin.GetChannelId(),
			"",
			GameUtility.GetDeviceInfo(),
			"",
			true,
			passwordLength));
	}

	public override void Login(string account, string password, bool isPasswordEncrypted, int passwordLength)
	{
		RequestMgr.Inst.Request(new LoginReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			account,
			EncryptPassword(password, isPasswordEncrypted),
			account,
			string.Format("{0}.{1}", BundlePlugin.GetMainBundleVersion(), KodConfigPlugin.GetRevision().ToString()),
			GameUtility.GetDeviceInfo(),
			KodGames.ClientHelper.AccountChannel.LOCAL,
			KodConfigPlugin.GetChannelId(),
			"",
			"",
			true,
			passwordLength));
	}

	public override void QuickLogin()
	{
		RequestMgr.Inst.Request(new QuickLoginReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			KodConfigPlugin.GetRevision(),
			GameUtility.GetDeviceInfo(),
			KodConfigPlugin.GetChannelId(),
			"",
			""));
	}

	public override void Bind(string account, string password, bool isPasswordEncrypted, int passwordLength)
	{
		RequestMgr.Inst.Request(new BindAccountReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			account,
			EncryptPassword(password, isPasswordEncrypted),
			"",
			GameUtility.GetDeviceInfo(),
			"",
			true,
			password.Length));
	}

	public override void ChangePassword(string account, string password, bool isPasswordEncrypted, string newPassword, bool isNewPasswordEncrypted, int passwordLength)
	{
		RequestMgr.Inst.Request(new ResetPasswordReq(
			KodConfig.GetAuthServerIP(),
			KodConfig.GetAuthServerPort(),
			account,
			EncryptPassword(password, isPasswordEncrypted),
			EncryptPassword(newPassword, isNewPasswordEncrypted),
			true,
			newPassword.Length));
	}

	public override void JoinGameArea(int areaID)
	{
	}

	public override bool IsGuest
	{
		get { return SysPrefs.Instance.QuickLogin; }
	}

	public override string LoginAccountInputPlaceholder
	{
		get { return GameUtility.GetUIString("UIDlgLogin_Account_PlaceHolder"); }
	}

	public override string RegisterAccountInputPlaceholder
	{
		get { return GameUtility.GetUIString("UIDlgRegister_Account_PlaceHolder"); }
	}

	public override string AccountTitle
	{
		get { return GameUtility.GetUIString("UI_Account_Title"); }
	}

	public override string PasswordInputPlaceholder
	{
		get { return GameUtility.GetUIString("UIDlgLogin_Pwd_PlaceHolder"); }
	}
}

