using UnityEngine;
using System;
using System.Collections;
using ClientServerCommon;

public class UIElemGuildChatItem : MonoBehaviour
{
	public SpriteText chatLabel;
	public SpriteText nameLabel;
	public SpriteText timeLabel;

	public void SetData(KodGames.ClientClass.GuildMsg guildMsg)
	{
		chatLabel.Text = guildMsg.Msg;

		//if (guildMsg.RoleId <= 0)
		//    guildMsg.RoleId = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleType(_RoleType.RoleType_Member).Id;

		var role = ConfigDatabase.DefaultCfg.GuildConfig.GetRoleByRoleId(guildMsg.RoleId);

		nameLabel.Text = GameUtility.FormatUIString("UIDlgGuildChat_Name", role.Color, role.Name,GameDefines.textColorOrange, guildMsg.PlayerName, guildMsg.PlayerLevel);

		DateTime date = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(guildMsg.Time);

		timeLabel.Text = GameUtility.FormatUIString("UIDlgGuildMessage_Time", date.Year, date.Month, date.Day) + GameUtility.FormatUIString("UITimeFormat_Hour", date.Hour, date.Minute, date.Second);
	}
}
