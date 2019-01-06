using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendStartOne : MonoBehaviour
{
	public UIElemAssetIcon playerIcon;//图标
	public SpriteText playerName;
	public SpriteText playerLevel;
	public SpriteText playerPower;

	public void SetData(int resourceId, string name, int level, int playerId, int powerValue)
	{
		if (resourceId == 0)
			playerIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
		else
			playerIcon.SetData(resourceId);

		playerIcon.Data = playerId;

		playerName.Text = name;
		playerLevel.Text = level <= 0 ? string.Empty : GameUtility.FormatUIString("UILevelPrefix", level);

		if (powerValue > 0)
			playerPower.Text = GameUtility.FormatUIString("UIPnlAvatar_Power", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, PlayerDataUtility.GetPowerString(powerValue));
		else
			playerPower.Text = string.Empty;
	}
}
