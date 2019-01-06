using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDanPackage : MonoBehaviour
{
	public UIElemAssetIcon danIcons;
	public SpriteText danAttrLabel;
	public UIButton danBg;
	public UIButton danInfo;
	public AutoSpriteControlBase danItem;

	public UIButton lockBtn;
	public UIButton unLockBtn;
	public UIBox isNew;

	public float boxWidth;
	public float boxHeight;

	private int defaultLine = 5;

	private KodGames.ClientClass.Dan dan;
	public KodGames.ClientClass.Dan Dan
	{
		get { return dan; }
	}

	private int index;
	public int Index
	{
		get { return index; }
	}

	private bool isLocked;
	public bool IsLocked
	{
		get { return isLocked; }
	}

	public void SetData(KodGames.ClientClass.Dan dan, long viewTime, int index)
	{

		this.dan = dan;
		this.index = index;

		danIcons.SetData(dan.ResourceId, dan.BreakthoughtLevel, dan.LevelAttrib.Level);
		danIcons.Data = dan;

		lockBtn.Data = this;
		unLockBtn.Data = this;
		danInfo.Data = this;

		bool newGet = dan.CreateTime > viewTime;

		//已锁状态不存在新标签
		if (!dan.Locked)
		{
			UIElemTemplate elemTemplate = SysUIEnv.Instance.GetUIModule<UIElemTemplate>();
			elemTemplate.towerRankTemplate.SetTowerRankBg(danBg, newGet);
			isNew.Hide(!newGet);
		}
		else
		{
			UIElemTemplate elemTemplate = SysUIEnv.Instance.GetUIModule<UIElemTemplate>();
			elemTemplate.towerRankTemplate.SetTowerRankBg(danBg, false);
			isNew.Hide(true);
		}

		this.isLocked = dan.Locked;

		ChangeLock(dan.Locked);

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

		danInfo.Hide(!PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, dan.Guid, dan.ResourceId));
	}

	public void ChangeLock(bool locked)
	{
		this.isLocked = locked;
		dan.Locked = locked;

		lockBtn.Hide(!locked);
		unLockBtn.Hide(locked);
	}
}
