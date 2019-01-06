using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemWolfNumberPlate : MonoBehaviour
{
	public SpriteText numberPlateLabel;
	public SpriteText numberPowerLabel;

	public void SetNumberPlate(string name, int powerValue)
	{
		numberPlateLabel.Text = name;

		if (powerValue > 0)
			numberPowerLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_Power", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, PlayerDataUtility.GetPowerString(powerValue));
		else
			numberPowerLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_NoPower", GameDefines.textColorBtnYellow, GameDefines.textColorWhite);
	}

	public void SetNumberPlate(string name)
	{
		numberPlateLabel.Text = name;

		if (numberPowerLabel != null)
			numberPowerLabel.Text = string.Empty;
	}
}
