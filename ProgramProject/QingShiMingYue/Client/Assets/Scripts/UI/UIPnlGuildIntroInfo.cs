using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIPnlGuildIntroInfo : UIModule
{
	public SpriteText guildNameLabel;
	public SpriteText guildLeaderNameLabel;
	public SpriteText guildLvLabel;
	public SpriteText guildRankLvLabel;
	public SpriteText guildMemberCountLabel;
	public SpriteText guildContributionLabel;
	public SpriteText guildRequireContributionLabel;
	public UIButton transferGuildButton;
	public UIButton leaveGuildButton;
	public GameObject autoAcceptRoot;
	public UIBox autoAcceptBox;
	public UIButton modifyDeclarationButton;
	public SpriteText guildDeclarationLabel;

	public UIButton setGuildNameBtn;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		setGuildNameBtn.Hide(true);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildIntroTab>().ChangeTabButtons(_UIType.UIPnlGuildIntroInfo);

		RequestMgr.Inst.Request(new GuildQueryReq(() =>
		{
			InitView();
			return true;
		}));

		return true;
	}

	private void InitView()
	{
		var preTxColor = GameDefines.textColorBtnYellow;
		var nexTxColor = GameDefines.textColorWhite;
		var guildMiniInfo = SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo;
		var guildLevelCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetGuildLevelByLevel(guildMiniInfo.GuildLevel);

		// Set Guild NameLabel.
		guildNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildName", preTxColor, nexTxColor, guildMiniInfo.GuildName);

		// Set Guild LeaderLabel.
		guildLeaderNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildLeader", preTxColor, nexTxColor, guildMiniInfo.LeaderName);

		// Set Guild LevelLabel.
		guildLvLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildLv", preTxColor, nexTxColor, guildMiniInfo.GuildLevel);

		// Set Guild RankLevelLabel.
		guildRankLvLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildRankLv", preTxColor, nexTxColor, guildMiniInfo.GuildRank);

		// Set Guild MumberCountLabel.
		guildMemberCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildMemberCount", preTxColor, nexTxColor, guildMiniInfo.MemberCount, guildLevelCfg.MemberMax);

		// Set Guild ConstructionLabel.
		guildContributionLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildContribution", preTxColor, nexTxColor, guildMiniInfo.GuildConstruct);

		// Set Guild Require ConstructionLabel.
		if (guildMiniInfo.GuildLevel < ConfigDatabase.DefaultCfg.GuildConfig.MaxGuildLevel)
			guildRequireContributionLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildRequireContribution1", preTxColor, nexTxColor, guildLevelCfg.NextLevelNeedConstruct - guildMiniInfo.GuildConstruct);
		else
			guildRequireContributionLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildRequireContribution2", preTxColor, nexTxColor);

		// Set Button State. QuitButton TransferButton AutoEnterButton.
		var roleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(guildMiniInfo.RoleId);
		transferGuildButton.Hide(roleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeLeader) == false);
		leaveGuildButton.Hide(roleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeLeader));
		autoAcceptRoot.SetActive(roleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeLeader));

		if (autoAcceptRoot.activeInHierarchy)
			autoAcceptBox.Hide(!guildMiniInfo.GuildAllowAutoEnter);

		// Set Declaration Label.
		guildDeclarationLabel.Text = ItemInfoUtility.GetGuildDeclaration();
		modifyDeclarationButton.Hide(roleCfg.Rights.Contains(GuildConfig._RoleRightType.ChangeDeclaration) == false);

		// Set Guild Name Button.
		setGuildNameBtn.Hide(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildLeaderPlayerId != SysLocalDataBase.Inst.LocalPlayer.PlayerId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuitGuild(UIButton btn)
	{
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK_Space");
		okCallback.Callback = (userData) =>
		{
			RequestMgr.Inst.Request(new GuildQuitReq());

			return true;
		};

		MainMenuItem cancelCallback = new MainMenuItem();
		cancelCallback.ControlText = GameUtility.GetUIString("UIDlgFriendMsg_Ctrl_Cancel");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(GameUtility.GetUIString("UIPnlGuildIntroInfo_QuitGuildTitle"), GameUtility.GetUIString("UIPnlGuildIntroInfo_QuitGuildMessage"), cancelCallback, okCallback);

		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTransferGuild(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildAssignment), new UIDlgGuildAssignment.OnAssignmentSuccess(InitView));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSetGuidNames(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgSetGuidName));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAutoEnterButton(UIButton btn)
	{
		var allAutoEnter = autoAcceptBox.IsHidden() == false;

		RequestMgr.Inst.Request(new GuildSetAutoEnterReq(!allAutoEnter, () =>
			{
				autoAcceptBox.Hide(!autoAcceptBox.IsHidden());
				return true;
			}));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickLevelIntro(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuideDetail, ConfigDatabase.DefaultCfg.GuildConfig.GetMainTypeByGuideType(GuildConfig._GuideType.LevelGuide));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDeclarModify(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgGuildNotifyModify), true, new UIDlgGuildNotifyModify.OnModifySuccess(() =>
			{
				guildDeclarationLabel.Text = ItemInfoUtility.GetGuildDeclaration();
			}));
	}

	public void SetInitView()
	{
		InitView();
	}
}