using UnityEngine;
using System.Collections;

public class UIElemGuildRankItem : MonoBehaviour
{
	public SpriteText nameLabel;
	public SpriteText constructLabel;
	public SpriteText levelLabel;
	public SpriteText rankLabel;
	public SpriteText memberLabel;
	public SpriteText leaderNameLabel;
	public UIButton viewBtn;
	public UIListItemContainer container;

	public void SetData(KodGames.ClientClass.GuildRankRecord guildRecord, int index)
	{
		nameLabel.Text = guildRecord.GuildName;

		levelLabel.Text = guildRecord.GuildLevel.ToString();

		guildRecord.Rank = index + 1;

		rankLabel.Text = guildRecord.Rank.ToString();

		constructLabel.Text = guildRecord.GuildConstruct.ToString();

		memberLabel.Text = GameUtility.FormatUIString("UIPnlGuildApplyList_MemberCout", guildRecord.CurrentGuildMemberCount, guildRecord.GuildMemberMax);

		viewBtn.Data = guildRecord.GuildId;

		container.Data = this;

		leaderNameLabel.Text = guildRecord.LeaderPlayerName;

	}
}
