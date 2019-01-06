using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDanSelectItem : MonoBehaviour
{
	public UIListItemContainer container;

	public UIBox itemBg;
	public UIElemAssetIcon danIcon;
	public SpriteText danAttrLabel;
	public SpriteText ownerLabel;
	public UIBox recommendedLable;

	public UIButton xiangBtn;
	public UIButton selectBtn;

	private int defaultLine = 5;
	private float minBoxHeight = 100f;

	private KodGames.ClientClass.Dan dan;
	public KodGames.ClientClass.Dan Dan { get { return dan; } }

	public void SetData(KodGames.ClientClass.Dan dan, int positionId, bool isRecommended)
	{
		this.container.Data = this;
		this.dan = dan;
		danIcon.Data = dan;
		xiangBtn.Data = this;
		selectBtn.Data = this;

		danIcon.SetData(dan.ResourceId, dan.BreakthoughtLevel, dan.LevelAttrib.Level);

		ownerLabel.Hide(!PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, positionId, dan.Guid, dan.ResourceId));

		xiangBtn.Hide(!PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, dan.Guid, dan.ResourceId));

		string attr = "";
		foreach (var attributeGroup in dan.DanAttributeGroups)
		{
			//ÃèÊö
			attr = attr + attributeGroup.AttributeDesc;
			//ÊýÖµ
			attr = attr + string.Format(GameUtility.GetUIString("UIPnlDanInfo_UpAttr_Label"), System.Math.Round(attributeGroup.DanAttributes[0].PropertyModifierSets[dan.LevelAttrib.Level - 1].Modifiers[0].AttributeValue * 100, 3)) + "\n";
		}

		danAttrLabel.Text = attr;

		if (danAttrLabel.GetDisplayLineCount() > defaultLine)
			itemBg.SetSize(itemBg.width, minBoxHeight + danAttrLabel.GetLineHeight() * (danAttrLabel.GetDisplayLineCount() - defaultLine) - danAttrLabel.lineSpacing);
		else itemBg.SetSize(itemBg.width, minBoxHeight);

		recommendedLable.Hide(isRecommended);

	}
}
