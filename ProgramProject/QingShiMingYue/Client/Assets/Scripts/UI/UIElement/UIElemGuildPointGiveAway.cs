using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemGuildPointGiveAway : MonoBehaviour
{
	public SpriteText contextLabel;
	public SpriteText nameLable;
	public SpriteText levelLable;
	public SpriteText btnContextLable;
	public UIButton giveAwayBtn;

	public void SetData(GuildMemberInfo info)
	{
		var roleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(info.RoleId);
		nameLable.Text = GameUtility.FormatUIString("UIPnlGuildPointGiveAway_Player", roleCfg.Color, roleCfg.Name);
		contextLabel.Text = info.PlayerName;
		levelLable.Text = GameUtility.FormatUIString("UIPnlGuildPointGiveAway_Level", info.PlayerLevel);
		SetBtnStatus(info.ReceiveBoxCount > 0);
		giveAwayBtn.Data = info;
	}

	private void SetBtnStatus(bool isShow)
	{
		btnContextLable.Hide(isShow);
		giveAwayBtn.Hide(!isShow);
		giveAwayBtn.spriteText.Hide(!isShow);
	}
}
