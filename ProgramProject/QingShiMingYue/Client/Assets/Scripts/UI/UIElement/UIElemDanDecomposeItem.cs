using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDanDecomposeItem : MonoBehaviour
{
	public UIElemAssetIcon danIcons;
	public SpriteText danAttrLabel;
	public UIButton danBg;
	public UIBox selectBtn;
	public AutoSpriteControlBase danItem;

	public float boxWidth;
	public float boxHeight;
	
	private bool isSelect;
	public bool IsSelect
	{
		get { return isSelect; }
	}

	private KodGames.ClientClass.Dan dan;
	public KodGames.ClientClass.Dan Dan
	{
		get { return dan; }
	}

	private int defaultLine = 5;

	public void SetData(KodGames.ClientClass.Dan dan)
	{
		danIcons.SetData(dan.ResourceId, dan.BreakthoughtLevel, dan.LevelAttrib.Level);
		danIcons.Data = dan;
		danBg.Data = this;
		selectBtn.Hide(true);
		isSelect = false;
		this.dan = dan;

		string attr = "";
		foreach (var attributeGroup in dan.DanAttributeGroups)
		{
			//描述
			attr = attr + attributeGroup.AttributeDesc;
			//数值
			attr = attr + string.Format(GameUtility.GetUIString("UIPnlDanInfo_UpAttr_Label"), System.Math.Round(attributeGroup.DanAttributes[0].PropertyModifierSets[dan.LevelAttrib.Level - 1].Modifiers[0].AttributeValue * 100, 3)) + "\n";
		}

		danAttrLabel.Text = attr;

		if (danAttrLabel.GetDisplayLineCount() > defaultLine)
			danItem.SetSize(boxWidth, danAttrLabel.GetLineHeight() * (danAttrLabel.GetDisplayLineCount() - defaultLine) + boxHeight);
		else danItem.SetSize(boxWidth, 100f);
	}

	public bool SetLight()
	{
		isSelect = !isSelect;
		selectBtn.Hide(!isSelect);

		return isSelect;
	}

	public void SetSelect(bool isAll)
	{
		isSelect = isAll;
		selectBtn.Hide(!isSelect);
	}
}
