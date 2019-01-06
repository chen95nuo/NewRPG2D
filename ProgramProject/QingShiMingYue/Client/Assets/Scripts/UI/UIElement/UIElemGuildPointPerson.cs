using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildPointPerson : MonoBehaviour
{
	public SpriteText contextLabel;
	public UIBox itemBg;

	private float minBoxHeight = 64.7f;
	public void SetData(com.kodgames.corgi.protocol.GuildStageMsg msg)
	{
		// Receive date
		System.DateTime receiveTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(msg.time);
		System.DateTime nowTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;
		System.TimeSpan timeSpane = nowTime.Subtract(receiveTime);

		if (nowTime.Day != receiveTime.Day || timeSpane.Days > 0)
		{
			int dayBefore = timeSpane.Days <= 0 ? 1 : timeSpane.Days;
			UIUtility.UpdateUIText(contextLabel, string.Format("{0}{1} \n{2}{3}",GameDefines.textColorBtnYellow, string.Format(GameUtility.GetUIString("UIPnlEmail_Receive_TimeShortDesc"), dayBefore),GameDefines.colorWhite, msg.msg));
		}
		else
			UIUtility.UpdateUIText(contextLabel, string.Format("{0}{1} \n{2}{3}", GameDefines.textColorBtnYellow, string.Format(GameUtility.GetUIString("UIPnlEmail_Receive_TimeLongDesc"), receiveTime.Hour.ToString("D2"), receiveTime.Minute.ToString("D2")), GameDefines.colorWhite, msg.msg));

		if (contextLabel.GetDisplayLineCount() > 1)
			itemBg.SetSize(itemBg.width, minBoxHeight + contextLabel.GetLineHeight() * (contextLabel.GetDisplayLineCount() - 1) - contextLabel.lineSpacing);
		else itemBg.SetSize(itemBg.width, minBoxHeight);
	}
}
