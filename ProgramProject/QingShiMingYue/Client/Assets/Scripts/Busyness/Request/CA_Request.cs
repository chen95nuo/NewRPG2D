using System;
using System.Collections.Generic;
using KodGames.ClientClass;
using KodGames.ClientHelper;

// 请求版本信息
class QueryManifestReq : Request
{
	private string authServerHostName;
	private int port;
	private int resourceVersion;
	private int version;
	private int channelID;
	private int subChannelID;
	private DeviceInfo deviceInfo;

	public override bool CanResend { get { return false; } }

	public QueryManifestReq(string authServerHostName, int port, int resourceVersion, int version, int channelID, int subChannelID, DeviceInfo deviceInfo)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.resourceVersion = resourceVersion;
		this.version = version;
		this.channelID = channelID;
		this.subChannelID = subChannelID;
		this.deviceInfo = deviceInfo;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QueryManifest(authServerHostName, port, resourceVersion, version, channelID, deviceInfo, ID, subChannelID);
	}
}

// 创建帐号
class CreateAccountReq : Request
{
	private string authServerHostName;
	private int port;
	private string account;
	private string password;
	private int channelId;
	private string mobile;
	private string klsso;
	private DeviceInfo deviceInfo;

	public string Account { get { return this.account; } }
	public string Password { get { return this.password; } }

	private bool isPasswordEncrypted;
	public bool IsPasswordEncrypted { get { return isPasswordEncrypted; } }

	private int passwordLength = 0;
	public int PasswordLength { get { return passwordLength; } }

	public override bool CanResend { get { return false; } }

	public CreateAccountReq(string authServerHostName, int port, string account, string password, int channelId,
							string mobile, DeviceInfo deviceInfo, string klsso,
							bool isPasswordEncrypted, int passwordLength)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.account = account;
		this.password = password;
		this.channelId = channelId;
		this.mobile = mobile;
		this.klsso = klsso;
		this.deviceInfo = deviceInfo;
		this.isPasswordEncrypted = isPasswordEncrypted;
		this.passwordLength = passwordLength;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.CreateAccount(authServerHostName, port, account, password, channelId, mobile, deviceInfo, klsso, ID);
	}
}

// 登录
class LoginReq : Request
{
	private string authServerHostName;
	public string AuthServerHostName { get { return authServerHostName; } }

	private int port;
	public int Port { get { return port; } }

	private string account;
	public string Account { get { return account; } }

	private string password;
	public string Password { get { return password; } }

	private string displayAccount;
	public string DisplayAccount { get { return displayAccount; } }

	private string version;

	private DeviceInfo deviceInfo;

	private AccountChannel channelType;

	private int channelID;
	public int ChannelID { get { return channelID; } }
	private string channelUniqueId;

	private string channelToken;
	public string ChannelToken { get { return channelToken; } }

	private bool isPasswordEncrypted;
	public bool IsPasswordEncrypted { get { return this.isPasswordEncrypted; } }

	private int passwordLength = 0;
	public int PasswordLength { get { return passwordLength; } }

	public override bool CanResend { get { return false; } }

	public LoginReq(string authServerHostName, int port, string account, string password, string displayAccount, string version, DeviceInfo deviceInfo,
					AccountChannel channelType, int channelID, string channelUniqueId, string channelToken,
					bool isPasswordEncrypted, int passwordLength)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.account = account;
		this.password = password;
		this.displayAccount = displayAccount;
		this.version = version;
		this.channelType = channelType;
		this.channelID = channelID;
		this.channelUniqueId = channelUniqueId;
		this.channelToken = channelToken;
		this.deviceInfo = GameUtility.GetDeviceInfo();
		this.isPasswordEncrypted = isPasswordEncrypted;
		this.passwordLength = passwordLength;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		// Record GameLogin.
		SysGameAnalytics.Instance.RecordGameData(GameRecordType.GameLogin);

		return bsn.Login(authServerHostName, port, account, password, version, channelID, deviceInfo, channelUniqueId, channelToken, channelType, ID);
	}
}

// 快速登录
class QuickLoginReq : Request
{
	private string authServerHostName;
	public string AuthServerHostName { get { return authServerHostName; } }

	private int port;
	public int Port { get { return port; } }

	private int version;
	private DeviceInfo deviceInfo;

	private int channelID;

	private string channelToken;
	public string ChannelToken { get { return channelToken; } }

	private string bindedAccount;
	public string BindedAccount { get { return bindedAccount; } }

	public override bool CanResend { get { return false; } }

	public QuickLoginReq(string authServerHostName, int port, int version, DeviceInfo deviceInfo, int channelID, string channelToken, string bindedAccount)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.version = version;
		this.deviceInfo = deviceInfo;

		this.channelID = channelID;
		this.channelToken = channelToken;

		this.bindedAccount = bindedAccount;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.QuickLogin(authServerHostName, port, version, channelID, deviceInfo, channelToken, bindedAccount, ID);
	}
}

// 登出, 这条协议不发送网络及请求
class LogoutReq : Request
{
	public delegate void LogoutSuccessDelegate();

	private LogoutSuccessDelegate logoutSuccessDel;
	public LogoutSuccessDelegate LogoutSuccessDel
	{
		get { return logoutSuccessDel; }
	}

	public LogoutReq(LogoutSuccessDelegate logoutSuccessDel)
	{
		this.logoutSuccessDel = logoutSuccessDel;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		Platform.Instance.Logout(this.ID);
		return true;
	}
}

// 验证激活码
class AuthActivityCodeReq : Request
{
	private string authServerHostName;
	private int port;
	private int accountId;
	private string activityCode;

	public override bool CanResend { get { return false; } }

	public AuthActivityCodeReq(string authServerHostName, int port, int accountId, string activityCode)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.accountId = accountId;
		this.activityCode = activityCode;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.AuthActivityCode(authServerHostName, port, accountId, activityCode, ID);
	}
}

// 绑定帐号
class BindAccountReq : Request
{
	private string authServerHostName;
	private int port;
	private string account;
	private string password;
	private string mobile;
	private string klsso;
	private KodGames.ClientClass.DeviceInfo deviceInfo;

	public string Account { get { return this.account; } }
	public string Password { get { return this.password; } }

	private bool passwordEncrypted;
	public bool PasswordEncrypted { get { return passwordEncrypted; } }

	private int passwordLength = 0;
	public int PasswordLength { get { return passwordLength; } }

	public override bool CanResend { get { return false; } }

	public BindAccountReq(string authServerHostName, int port, string account, string password,
						  string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso,
						  bool passwordEncrypted, int passwordLength)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.account = account;
		this.password = password;
		this.mobile = mobile;
		this.deviceInfo = deviceInfo;
		this.klsso = klsso;
		this.passwordEncrypted = passwordEncrypted;
		this.passwordLength = passwordLength;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.BindAccount(authServerHostName, port, account, password, mobile, deviceInfo, klsso, ID);
	}
}

// 修改帐号
class ResetPasswordReq : Request
{
	private string authServerHostName;
	private int port;
	private string email;
	private string oldPassword;

	private string newPassword;
	public string NewPassword { get { return newPassword; } }

	private bool passwordEncrypted;
	public bool PasswordEncrypted { get { return passwordEncrypted; } }

	private int passwordLength = 0;
	public int PasswordLength { get { return passwordLength; } }

	public override bool CanResend { get { return false; } }

	public ResetPasswordReq(string authServerHostName, int port, string email, string oldPassword, string newPassword, bool passwordEncrypted, int passwordLength)
	{
		this.authServerHostName = authServerHostName;
		this.port = port;
		this.email = email;
		this.oldPassword = oldPassword;
		this.newPassword = newPassword;
		this.passwordEncrypted = passwordEncrypted;
		this.passwordLength = passwordLength;
	}

	public override bool Execute(IBusiness bsn)
	{
		if (!base.Execute(bsn))
			return false;

		return bsn.ResetPassword(authServerHostName, port, email, oldPassword, newPassword, ID);
	}
}