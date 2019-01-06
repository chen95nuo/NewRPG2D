using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIPnlSetting : UIModule
{
	//Binding email button.
	public UIButton bindingBtn;
	public SpriteText bindingTipLabel;

	//Modify password and logout button.
	public UIButton modifyPwdBtn;
	public UIButton logoutBtn;

	public SpriteText bindEmailTitleLabel;
	public SpriteText bindEmailLabel;

	//Music and sound setting item.
	public UIElemSelectItem gameMusicBtn;
	public UIElemSelectItem soundEffectBtn;

	// OfficialInfo texts
	public SpriteText officialInfo;

	public SpriteText musicTxt;
	public SpriteText soundTxt;

	//Invite code input form.
	public GameObject inviteBg;
	public UITextField inviteCodeInput;


	public UIButton announceBtn;
	public Transform ann_point_three;
	public Transform ann_point_two;

	public UIButton sendFeedBackBtn;
	public Transform sen_point_three;
	public Transform sen_point_two;

	public UIButton enterBtn;
	public Transform ent_point_three;

	public SpriteText enterBBSText;	//显示论坛还是显示用户中心

	private const int BUTTON_TWO_SIZE_WIDTH = 115;
	private const int BUTTON_THR_SIZE_WIDTH = 100;
	private const int BUTTON_SIZE_HEIGHT = 35;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		bindEmailTitleLabel.Text = Platform.Instance.AccountTitle;

		if (!ConfigDatabase.DefaultCfg.GameConfig.isInviteCode)
		{
			//Set Button.
			announceBtn.Hide(false);
			announceBtn.SetSize(BUTTON_THR_SIZE_WIDTH, BUTTON_SIZE_HEIGHT);
			announceBtn.transform.localPosition = ann_point_three.localPosition;

			sendFeedBackBtn.Hide(false);
			sendFeedBackBtn.SetSize(BUTTON_THR_SIZE_WIDTH, BUTTON_SIZE_HEIGHT);
			sendFeedBackBtn.transform.localPosition = sen_point_three.localPosition;

			enterBtn.Hide(false);
			enterBtn.SetSize(BUTTON_THR_SIZE_WIDTH, BUTTON_SIZE_HEIGHT);
			enterBtn.transform.localPosition = ent_point_three.localPosition;

			// 显示论坛的默认逻辑为,如果有论坛,显示论坛,否则显示用户中心
			if (Platform.Instance.HasBBS)
				enterBBSText.Text = Platform.Instance.BBSTitle;
			else
				enterBBSText.Text = Platform.Instance.UserCenterTitle;
		}
		else
		{
			announceBtn.Hide(false);
			announceBtn.SetSize(BUTTON_TWO_SIZE_WIDTH, BUTTON_SIZE_HEIGHT);
			announceBtn.transform.localPosition = ann_point_two.localPosition;

			sendFeedBackBtn.Hide(false);
			sendFeedBackBtn.SetSize(BUTTON_TWO_SIZE_WIDTH, BUTTON_SIZE_HEIGHT);
			sendFeedBackBtn.transform.localPosition = sen_point_two.localPosition;

			enterBtn.Hide(true);
			enterBBSText.Text = string.Empty;
		}

		gameMusicBtn.AddStateChangeDel(OnGameMusicClick);
		soundEffectBtn.AddStateChangeDel(OnSoundEffectClick);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlSetting);

		inviteCodeInput.Text = string.Empty;

		var function = SysLocalDataBase.Inst.LocalPlayer.Function;
		inviteBg.SetActive(function != null ? function.ShowExchange : false);

		//Reset controls status.
		ResetControls();
		return true;
	}

	private void ResetControls()
	{
		// Set officialInfo texts
		officialInfo.Text = ConfigDatabase.DefaultCfg.GameConfig.GetOfficialInfos(Platform.Instance.PlatformType, Platform.Instance.ChannelId);

		if (Platform.Instance.IsGuest)
		{
			//Show binding button.
			bindingBtn.Hide(false);
			bindingTipLabel.Hide(false);

			//Hide logout and modify pwd buttons.
			modifyPwdBtn.Hide(true);
			logoutBtn.Hide(true);
			bindEmailTitleLabel.Hide(true);
			bindEmailLabel.Hide(true);
		}
		else
		{
			//Hide binding button.
			bindingBtn.Hide(true);
			bindingTipLabel.Hide(true);

			//Show logout and modify pwd buttons.
			modifyPwdBtn.Hide(false);
			logoutBtn.Hide(false);
			bindEmailTitleLabel.Hide(false);
			bindEmailLabel.Hide(false);

			bindEmailLabel.Text = SysLocalDataBase.Inst.LoginInfo.Account;
		}

		// set music/SE text color and button stat
		if (SysPrefs.Instance.MusicOn)
		{
			gameMusicBtn.SetState(true);
		}
		else
		{
			gameMusicBtn.SetState(false);
		}

		if (SysPrefs.Instance.SoundOn)
		{
			soundEffectBtn.SetState(true);
		}
		else
		{
			soundEffectBtn.SetState(false);
		}

		if (Platform.Instance.IsPlatformLogin())
		{
			modifyPwdBtn.Hide(true);
		}
	}

	private void OnGameMusicClick(bool state, object data)
	{
		if (state)
		{
			SetMusicOn(true);
		}
		else
		{
			SetMusicOn(false);
		}
	}

	private void OnSoundEffectClick(bool state, object data)
	{
		if (state)
		{
			SetSoundOn(true);
		}
		else
		{
			SetSoundOn(false);
		}
	}

	// Music ON/OFF settings 
	private void SetMusicOn(bool open)
	{
		SysPrefs preFs = SysPrefs.Instance;

		if (open)
		{
			// Open the music 
			preFs.MusicOn = true;
		}
		else
		{
			//Shut down the music
			preFs.MusicOn = false;
		}
		//Save Settings
		preFs.Save();
	}

	//Sound ON/OFF settings
	private void SetSoundOn(bool open)
	{
		SysPrefs preFs = SysPrefs.Instance;
		if (open)
		{
			//Open the sound effect
			preFs.SoundOn = true;
		}
		else
		{
			//Shut down the sound effect
			preFs.SoundOn = false;
		}
		//Save Settings
		preFs.Save();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBindingClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgSetAccontBinding));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnModifyPwdClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgModifyPwd));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnLogoutClick(UIButton btn)
	{
		HideSelf();
		RequestMgr.Inst.Request(new LogoutReq(null));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnEnterForumClick(UIButton btn)
	{
		if (Platform.Instance.HasBBS)
			Platform.Instance.ShowBBS();
		else
			Platform.Instance.ShowUserCenter();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnConvertClick(UIButton btn)
	{
		if (string.IsNullOrEmpty(inviteCodeInput.Text))
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlSetting_Input_Convert"));
		else
			RequestMgr.Inst.Request(new ExchangeCodeReq(inviteCodeInput.Text.TrimEnd()));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSetPlayerName(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgSetName));
	}

	public void OnSettingExchangeRewardSuccess(KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, string strConver, string strGetway)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgConver), costAndRewardAndSync, strConver, strGetway);
		inviteCodeInput.Text = "";
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSendFeedbackClick(UIButton btn)
	{
		Platform.Instance.ShowCallCenter();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickShowAnnounce(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAnnouncement);
	}
}
