using UnityEngine;
using System.Collections;
using KodGames.ClientClass;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildApplyItem : MonoBehaviour
{
	public SpriteText indexLabel;
	public SpriteText nameLabel;
	public SpriteText levelLabel;
	public SpriteText leaderNameLabel;
	public SpriteText memberCount;
	public SpriteText declarationLabel;
	public SpriteText applyLabel;
	public UIButton infoBtn;
	public UIButton applyBtn;
	public UIBox saturated;
	public SpriteText applyed;
	public UIListItemContainer container;

	public void SetData(KodGames.ClientClass.GuildRecord guildRecord, int index)
	{
		//set button data
		applyBtn.Data = guildRecord.GuildId;
		infoBtn.Data = guildRecord.GuildId;
		infoBtn.IndexData = index;
		container.Data = this;

		// set guild info
		indexLabel.Text = index.ToString();
		nameLabel.Text = guildRecord.GuildName;
		levelLabel.Text = guildRecord.GuildLevel.ToString();
		leaderNameLabel.Text = guildRecord.LeaderPlayerName;
		memberCount.Text = GameUtility.FormatUIString("UIPnlGuildApplyList_MemberCout", guildRecord.GuildMemberNum, guildRecord.GuildMemberMax);

		if (string.IsNullOrEmpty(guildRecord.Declaration))
			declarationLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildDeclaration", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, GameUtility.GetUIString("UITextHolder_Declaration"));
		else
			declarationLabel.Text = GameUtility.FormatUIString("UIPnlGuildIntroInfo_GuildDeclaration", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, guildRecord.Declaration);


		//set button state
		if (guildRecord.UnderApply)
		{
			applyed.Hide(false);
			applyBtn.Hide(true);
			saturated.Hide(true);
			applyLabel.Hide(true);
			return;
		}

		if (guildRecord.GuildMemberNum >= guildRecord.GuildMemberMax)
		{
			applyed.Hide(true);
			applyBtn.Hide(true);
			applyLabel.Hide(true);
			saturated.Hide(false);
		}
		else
		{
			applyed.Hide(true);
			applyBtn.Hide(false);
			applyLabel.Hide(false);
			saturated.Hide(true);
		}

	}

}
