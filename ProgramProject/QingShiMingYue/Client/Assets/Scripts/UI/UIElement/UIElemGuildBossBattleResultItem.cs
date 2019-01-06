using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemGuildBossBattleResultItem : MonoBehaviour
{
	public SpriteText rankLabel;
	public SpriteText nameLabel;
	public SpriteText damageLabel;	

	public void SetData(com.kodgames.corgi.protocol.Rank rank)
	{
		rankLabel.Text = rank.rankValue.ToString();
		nameLabel.Text = rank.name;

		if ((int)rank.damage > 1000000)
			damageLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_LeftTipsLargeCount", (int)rank.damage / 10000, rank.doubleValue.ToString("P"));
		else
			damageLabel.Text = GameUtility.FormatUIString("UIPnlGuildPointMonsterBattleResult_LeftTips", (int)rank.damage, rank.doubleValue.ToString("P"));	
	}
}
