using System;
using System.Collections.Generic;
using UnityEngine;

public class UIElemChooseCardDan : UIElemChooseCardBasic
{
	private Vector2 originalSize;
	private int maxLines = 5;

	public void Awake()
	{
		originalSize = new Vector2(itemBg.width, itemBg.height);
	}

	public void SetData(KodGames.ClientClass.Dan dan)
	{
		SetData(dan, false);
	}

	public void SetData(KodGames.ClientClass.Dan dan, bool selected)
	{
		SetBaseData(dan.ResourceId, dan.Guid, selected);

		itemBg.Data = this;
		itemIcon.SetData(dan.ResourceId, dan.BreakthoughtLevel, dan.LevelAttrib.Level);

		string attr = string.Empty;
		foreach (var attributeGroup in dan.DanAttributeGroups)
		{
			//描述
			attr = attr + attributeGroup.AttributeDesc;
			//数值
			attr = attr + GameUtility.FormatUIString("UIPnlDanInfo_UpAttr_Label", System.Math.Round(attributeGroup.DanAttributes[0].PropertyModifierSets[dan.LevelAttrib.Level - 1].Modifiers[0].AttributeValue * 100, 3)) + "\n";
		}

		itemQualityLabel.Text = attr;

		if (itemQualityLabel.GetDisplayLineCount() > maxLines)
			itemBg.SetSize(originalSize.x, originalSize.y + (itemQualityLabel.GetDisplayLineCount() - maxLines) * (itemQualityLabel.GetLineHeight() + itemQualityLabel.lineSpacing) - itemQualityLabel.lineSpacing);
		else
			itemBg.SetSize(originalSize.x, originalSize.y);
	}
}
