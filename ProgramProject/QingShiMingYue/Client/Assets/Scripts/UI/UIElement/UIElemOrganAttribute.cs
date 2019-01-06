using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganAttribute : MonoBehaviour
{
	public SpriteText attribute1;
	public SpriteText attribute2;

	public void SetData(AttributeCalculator.Attribute att1, double equipValue,float value)
	{
		string attMsg1 = ItemInfoUtility.GetAttributeNameValueString(att1.type, att1.value - equipValue, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

		if (equipValue > 0)
			attMsg1 = string.Format(GameUtility.GetUIString("UIPnlOrganGrowPage_AttributeText"), attMsg1, GameDefines.txColorGreen, KodGames.Math.RoundToInt(equipValue));

		attribute1.Text = attMsg1;

		attribute2.Text = ItemInfoUtility.GetAttributeNameValueString(att1.type, att1.value * value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite) +
			GameUtility.FormatUIString("UIPnlOrganGrowPage_Persent", (int)(value * 100));		
	}

	public void SetData(AttributeCalculator.Attribute att1, AttributeCalculator.Attribute att2)
	{
		attribute1.Text = ItemInfoUtility.GetAttributeNameValueString(att1.type, att1.value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

		if (att2.value != 0)
			attribute2.Text = ItemInfoUtility.GetAttributeNameValueString(att2.type, att2.value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);
		else attribute2.Text = "";
	}

	public void SetDataWithAdd(AttributeCalculator.Attribute att1, AttributeCalculator.Attribute att2)
	{
		attribute1.Text = ItemInfoUtility.GetAttributeNameValueStringWithAdd(att1.type, att1.value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);

		if (att2.value != 0)
			attribute2.Text = ItemInfoUtility.GetAttributeNameValueStringWithAdd(att2.type, att2.value, GameDefines.textColorBtnYellow, GameDefines.textColorWhite);
		else attribute2.Text = "";
	}
}

