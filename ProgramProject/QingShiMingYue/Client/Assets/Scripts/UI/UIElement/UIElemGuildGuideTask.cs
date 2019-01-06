using UnityEngine;
using System.Collections;

public class UIElemGuildGuideTask : MonoBehaviour 
{
	public SpriteText label;

	public void SetData()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildInvisibleTaskInfos.Count>0)
			label.Text = GameUtility.FormatUIString("UIPnlGuildGuide_TaskSuccess", GameDefines.textColorBtnYellow, GameDefines.cardColorChenSe);
		else
			label.Text = GameUtility.FormatUIString("UIPnlGuildGuide_TaskNoSuccess", GameDefines.textColorBtnYellow, GameDefines.cardColorChenSe);
	}
}
