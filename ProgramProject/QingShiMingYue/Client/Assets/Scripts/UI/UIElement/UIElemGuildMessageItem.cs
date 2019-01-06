using UnityEngine;
using System;
using System.Collections;

public class UIElemGuildMessageItem : MonoBehaviour
{
	public SpriteText timeLabel;
	public SpriteText messageLabel;

	public void SetData(KodGames.ClientClass.GuildNews guildNew)
	{
		messageLabel.Text = guildNew.Content;

		DateTime date = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(guildNew.Time);

		timeLabel.Text = GameUtility.FormatUIString("UIDlgGuildMessage_Time", date.Year, date.Month, date.Day) + GameUtility.FormatUIString("UITimeFormat_Hour", date.Hour, date.Minute, date.Second);
	}
}
