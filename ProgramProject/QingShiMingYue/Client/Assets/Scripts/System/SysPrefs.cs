using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

/// <summary>
/// Prefers system to manage game saved datas. 
/// </summary>
public class SysPrefs : SysModule
{
	public static SysPrefs Instance
	{
		get { return SysModuleManager.Instance.GetSysModule<SysPrefs>(); }
	}

	// Volume changed callback.
	public delegate void VolumeChangedDelegate(float vlm);

	// Save flag.
	private class SAVE_FLAG
	{
		public const int LoginInfo = 1 << 0; // Login info.
		public const int Volume = 1 << 1; // Volume.
		public const int Video = 1 << 2; // Video.
		public const int AudioSwitch = 1 << 3; // Audio switch.
		public const int UseRemoteIcon = 1 << 4; // Use remote icon
		public const int GameConfigVersion = 1 << 5; // Game config version
		public const int CampaignCombatFinish = 1 << 6;
		public const int QuickEquipHint = 1 << 7;
	}

	// Save flag.
	private int saveFlag;
	
	private VolumeChangedDelegate soundVolumeChangedDelegate; // Sound volume changed callback.
	private VolumeChangedDelegate musicVolumeChangedDelegate; // Music volume changed callback.

	private bool autoLogin = true;
	public bool AutoLogin
	{
		get { return this.autoLogin; }
		set { this.autoLogin = value; }
	}

	private bool quickLogin;
	public bool QuickLogin
	{
		get { return quickLogin; }
		set
		{
			if (quickLogin != value)
			{
				quickLogin = value;
				saveFlag |= SAVE_FLAG.LoginInfo;
			}
		}
	}

	// Account.
	private string account;
	public string Account
	{
		get { return account; }
		set
		{
			if (account != value)
			{
				account = value;
				saveFlag |= SAVE_FLAG.LoginInfo;
			}
		}
	}

	// Encrypt Password.
	private string password;
	public string Password
	{
		get { return password; }
	}

	private bool isPasswordEncrypted;
	public bool IsPasswordEncrypted
	{
		get { return isPasswordEncrypted; }
	}

	private int passwordLength = 0;
	public int PasswordLength
	{
		get { return passwordLength; }
	}

	private bool hasAccountLogined = false;
	public bool HasAccountLogined
	{
		get { return hasAccountLogined; }
		set
		{
			if (hasAccountLogined != value)
			{
				hasAccountLogined = value;
				saveFlag |= SAVE_FLAG.LoginInfo;
			}
		}
	}
	
	public void SavePassword(string password, bool isPasswordEncrypted, int passwordLength)
	{
		this.password = password;
		this.isPasswordEncrypted = isPasswordEncrypted;
		this.passwordLength = passwordLength;
		saveFlag |= SAVE_FLAG.LoginInfo;
	}

	// Sound on.
	private bool soundOn;
	public bool SoundOn
	{
		get { return soundOn; }
		set
		{
			if (soundOn != value)
			{
				soundOn = value;
				saveFlag |= SAVE_FLAG.AudioSwitch;
			}

			if (soundVolumeChangedDelegate != null)
				soundVolumeChangedDelegate(soundOn ? SoundVolume : 0);
		}
	}

	// Music on.
	private bool musicOn; // Music on.
	public bool MusicOn
	{
		get { return musicOn; }
		set
		{
			if (musicOn != value)
			{
				musicOn = value;
				saveFlag |= SAVE_FLAG.AudioSwitch;
			}

			if (musicVolumeChangedDelegate != null)
				musicVolumeChangedDelegate(musicOn ? MusicVolume : 0);
		}
	}

	// Sound volume
	private float soundVolume;
	public float SoundVolume
	{
		get { return soundVolume; }

		set
		{
			if (soundVolume != value)
			{
				soundVolume = value;
				saveFlag |= SAVE_FLAG.Volume;
			}

			if (SoundOn && soundVolumeChangedDelegate != null)
				soundVolumeChangedDelegate(soundVolume);
		}
	}

	// Music volume
	private float musicVolume; // Music volume.
	public float MusicVolume
	{
		get { return musicVolume; }
		set
		{
			if (musicVolume != value)
			{
				musicVolume = value;
				saveFlag |= SAVE_FLAG.Volume;
			}

			if (MusicOn && musicVolumeChangedDelegate != null)
				musicVolumeChangedDelegate(musicVolume);
		}
	}

	// Vidow quality.
	private int graphicsQuality;
	public int GraphicsQuality
	{
		get { return graphicsQuality; }
		set
		{
			if (graphicsQuality != value)
			{
				graphicsQuality = value;
				saveFlag |= SAVE_FLAG.Video;

				if (graphicsQuality != QualitySettings.GetQualityLevel())
					QualitySettings.SetQualityLevel(graphicsQuality);
			}
		}
	}

	private bool downloadIconViaCarrierDataNetwork;
	public bool DownloadIconViaCarrierDataNetwork
	{
		get { return downloadIconViaCarrierDataNetwork; }
		set
		{
			if (downloadIconViaCarrierDataNetwork != value)
			{
				downloadIconViaCarrierDataNetwork = value;
				saveFlag |= SAVE_FLAG.UseRemoteIcon;
			}
		}
	}

	private int gameConfigVersion = 0;
	public int GameConfigVersion
	{
		get { return gameConfigVersion; }
		set
		{
			if (gameConfigVersion != value)
			{
				gameConfigVersion = value;
				saveFlag |= SAVE_FLAG.GameConfigVersion;
			}
		}
	}

	public override bool Initialize()
	{
		// Resume to preferable setting.
		quickLogin = PlayerPrefs.GetInt(GameDefines.gdQuickLogin, 1) == 1;
		account = PlayerPrefs.GetString(GameDefines.gdAcc);
		passwordLength = PlayerPrefs.GetInt(GameDefines.gdPwdLength);
		password = PlayerPrefs.GetString(GameDefines.gdPwd);
		isPasswordEncrypted = PlayerPrefs.GetInt(GameDefines.gdisPasswordEncrypted) == 1;
		hasAccountLogined = PlayerPrefs.GetInt(GameDefines.gdHasAccountLogined) == 1;

		SoundOn = PlayerPrefs.GetInt(GameDefines.gdSndSwt, 1) == 1;
		MusicOn = PlayerPrefs.GetInt(GameDefines.gdMscSwt, 1) == 1;

		SoundVolume = PlayerPrefs.GetFloat(GameDefines.gdSndVlm, 1.0f);
		MusicVolume = PlayerPrefs.GetFloat(GameDefines.gdMscVlm, 0.5f);

#if !UNITY_EDITOR
		GraphicsQuality = PlayerPrefs.GetInt(GameDefines.gdVdoQlt, QualitySettings.GetQualityLevel());
#endif

		downloadIconViaCarrierDataNetwork = PlayerPrefs.GetInt(GameDefines.gdRemoteIcon, 1) == 1;
		gameConfigVersion = PlayerPrefs.GetInt(GameDefines.gdGameConfigVersion, 0);

		// Clear save flag.
		saveFlag = 0;

		return true;
	}

//	private void OnGUI()
//	{
//		int qualityLevel = QualitySettings.GetQualityLevel();
//		if (GUILayout.Button(QualitySettings.names[qualityLevel]))
//		{
//			qualityLevel ++;
//			if (qualityLevel >= QualitySettings.names.Length)
//				qualityLevel = 0;
//
//			QualitySettings.SetQualityLevel(qualityLevel);
//		}
//	}

	public override void Dispose()
	{
		Save();
	}

	public void Save()
	{
		PlayerPrefs.SetInt(GameDefines.gdHasSvDt, 1);

		if ((saveFlag & SAVE_FLAG.LoginInfo) != 0)
		{
			PlayerPrefs.SetInt(GameDefines.gdQuickLogin, QuickLogin ? 1 : 0);
			PlayerPrefs.SetString(GameDefines.gdAcc, Account);
			PlayerPrefs.SetInt(GameDefines.gdPwdLength, PasswordLength);
			PlayerPrefs.SetString(GameDefines.gdPwd, Password);
			PlayerPrefs.SetInt(GameDefines.gdisPasswordEncrypted, isPasswordEncrypted ? 1 : 0);
			PlayerPrefs.SetInt(GameDefines.gdHasAccountLogined, hasAccountLogined ? 1 : 0);
		}

		if ((saveFlag & SAVE_FLAG.Volume) != 0)
		{
			PlayerPrefs.SetFloat(GameDefines.gdSndVlm, SoundVolume);
			PlayerPrefs.SetFloat(GameDefines.gdMscVlm, MusicVolume);
		}

		if ((saveFlag & SAVE_FLAG.AudioSwitch) != 0)
		{
			PlayerPrefs.SetInt(GameDefines.gdSndSwt, SoundOn ? 1 : 0);
			PlayerPrefs.SetInt(GameDefines.gdMscSwt, MusicOn ? 1 : 0);
		}

		if ((saveFlag & SAVE_FLAG.Video) != 0)
		{
			PlayerPrefs.SetInt(GameDefines.gdVdoQlt, GraphicsQuality);
		}

		if ((saveFlag & SAVE_FLAG.UseRemoteIcon) != 0)
			PlayerPrefs.SetInt(GameDefines.gdRemoteIcon, DownloadIconViaCarrierDataNetwork ? 1 : 0);

		if ((saveFlag & SAVE_FLAG.GameConfigVersion) != 0)
			PlayerPrefs.SetInt(GameDefines.gdGameConfigVersion, GameConfigVersion);

		// Clear save flag.
		saveFlag = 0;
	}

	public void SetSndVlmCb(VolumeChangedDelegate cb)
	{
		soundVolumeChangedDelegate = cb;
	}

	public void SetMscVlmCb(VolumeChangedDelegate cb)
	{
		musicVolumeChangedDelegate = cb;
	}

	public void ResetAccountInfo()
	{
		Account = "";
		SavePassword("", false, 0);
	}

#if UNITY_EDITOR
	[UnityEditor.MenuItem("Tools/Clear Login info")]
	public static void ClearLoginInfo()
	{
		PlayerPrefs.DeleteKey(GameDefines.gdQuickLogin);
		PlayerPrefs.DeleteKey(GameDefines.gdAcc);
		PlayerPrefs.DeleteKey(GameDefines.gdPwdLength);
		PlayerPrefs.DeleteKey(GameDefines.gdPwd);
		PlayerPrefs.DeleteKey(GameDefines.gdisPasswordEncrypted);
		PlayerPrefs.DeleteKey(GameDefines.gdHasAccountLogined);
		PlayerPrefs.DeleteKey(GameDefines.gdQuickEquipHint);
	}
#endif
}
