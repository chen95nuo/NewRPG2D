//#define MONTHCARD_LOG
using UnityEngine;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildPlayerListItem : MonoBehaviour
{
	public SpriteText playerNameLable;
	public SpriteText currentStatusLable;
	public SpriteText roleLable;
	public SpriteText challengeCountLable;

	public void SetData(GuildMemberInfo info)
	{

		playerNameLable.Text = GetStrAddColor(info.PlayerName, info.Online ? GameDefines.guildPlayerListOnline1 : GameDefines.guildPlayerListOnline2);
		var guildRoleCfg = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(info.RoleId);
		roleLable.Text = GetStrAddColor(guildRoleCfg.Name, info.Online ? GameDefines.guildPlayerListOnline1 : GameDefines.guildPlayerListOnline2);

		currentStatusLable.Text = info.Online ? GameUtility.FormatUIString("UIElemGuildPlayerListItem_Online", GameDefines.guildPlayerListOnline1) : GameUtility.FormatUIString("UIElemGuildPlayerListItem_NoOnline", GameDefines.guildPlayerListOnline2);
		challengeCountLable.Text = GetStrAddColor(info.FreeChallengeCount.ToString(), info.Online ? GameDefines.guildPlayerListOnline1 : GameDefines.guildPlayerListOnline2);
	}

	private string GetStrAddColor(string str,Color color)
	{
		return GameUtility.FormatUIString("UIElemGuildPlayerListItem_Color", color, str);
	}
	
}
