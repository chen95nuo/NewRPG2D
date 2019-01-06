using System;
using System.Collections.Generic;

public class UIElemGuildIntroReviewItem : MonoBehaviour
{
	public SpriteText reviewMemberNameLabel;
	public SpriteText reviewMemberLvLabel;
	public SpriteText reviewMemberRankLabel;
	public UIButton viewAvatarButton;
	public UIButton refuseButton;
	public UIButton confirmbutton;

	private KodGames.ClientClass.GuildApplyInfo applyInfo;
	public KodGames.ClientClass.GuildApplyInfo ApplyInfo { get { return applyInfo; } }

	public void SetData(KodGames.ClientClass.GuildApplyInfo applyInfo)
	{
		this.applyInfo = applyInfo;

		// Set Name Label.
		reviewMemberNameLabel.Text = applyInfo.PlayerName;

		// Set Level Label.
		reviewMemberLvLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroReview_LV", applyInfo.PlayerLevel);

		// Set Rank Label.
		reviewMemberRankLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_Power", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, PlayerDataUtility.GetPowerString(applyInfo.Power));

		// Set Button Data.
		viewAvatarButton.Data = applyInfo.PlayerId;
		refuseButton.Data = applyInfo.PlayerId;
		confirmbutton.Data = applyInfo.PlayerId;
	}
}
