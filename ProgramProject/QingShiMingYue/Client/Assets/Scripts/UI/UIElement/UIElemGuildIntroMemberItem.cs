using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildIntroMemberItem : MonoBehaviour
{
	public SpriteText memberRoldLabel;
	public SpriteText memberNameLabel;
	public SpriteText memberLevelLabel;
	public SpriteText memberStatusLabel;
	public SpriteText memberContributionLabel;
	public SpriteText memberTotalContributionlabel;
	public SpriteText memberPowerLabel;
	public UIButton operationButton;

	private int playerId;
	public int PlayerId { get { return playerId; } }

	public void SetData(KodGames.ClientClass.GuildMemberInfo guildMemberInfo)
	{
		this.playerId = guildMemberInfo.PlayerId;

		//if (guildMemberInfo.RoleId <= 0)
		//    guildMemberInfo.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		var roleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(guildMemberInfo.RoleId);
		if (roleCfg == null)
		{
			Debug.LogWarning(string.Format("RoleId : {0} do not exist in GuildConfig", guildMemberInfo.RoleId.ToString("X")));
			return;
		}

		// Set Button Data.
		operationButton.Data = guildMemberInfo;

		// Set RoleLabel.
		memberRoldLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_MemberRole", roleCfg.Color, roleCfg.Name);

		// Set Member NameLabel.
		memberNameLabel.Text = guildMemberInfo.PlayerName;

		// Set Member levelLabel.
		memberLevelLabel.Text = GameUtility.FormatUIString("UIPnlGuildApplyInfo_Level", guildMemberInfo.PlayerLevel);

		// Set Contribution Label least days.
		memberContributionLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_ContributionLeast", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, guildMemberInfo.LatestContribution);

		// Set Contribution Label.
		memberTotalContributionlabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_Contribution", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, guildMemberInfo.TotalContribution);

		//Set Member Power
		memberPowerLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_Power", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, PlayerDataUtility.GetPowerString(guildMemberInfo.Power));

		// Set Status Label.
		if (guildMemberInfo.Online)
			memberStatusLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_OnLine", GameDefines.textColorGreen);
		else if (SysLocalDataBase.Inst.LoginInfo.NowTime - guildMemberInfo.OfflineTime > 1000 * 60 * 60 * 24 * 7)
			memberStatusLabel.Text = GameUtility.FormatUIString("UITimeLeftLabel_Week_GuildMember", GameDefines.textColorGray);
		else
			memberStatusLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroMember_OffLine", GameDefines.textColorGray, GameUtility.Time2SortString(SysLocalDataBase.Inst.LoginInfo.NowTime - guildMemberInfo.OfflineTime));
	}
}