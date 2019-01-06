using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemSkillItem : MonoBehaviour
{
	public UIListItemContainer container;

	public UIElemAssetIcon iconBtn;
	public UIBox assembleIcon;
	public SpriteText skillNameLabel;
	public SpriteText skillEquipLabel;
	public SpriteText skillQualityLabel;
	public List<SpriteText> skillAttributeLabels;

	public UIButton detailInfoBtn;
	public UIButton selectBtn;

	private KodGames.ClientClass.Skill skill;
	public KodGames.ClientClass.Skill Skill { get { return skill; } }

	public void SetData(KodGames.ClientClass.Skill skill, int positionId)
	{
		this.container.Data = this;
		this.skill = skill;
		this.iconBtn.Data = this;
		this.detailInfoBtn.Data = this;
		this.selectBtn.Data = this;
		this.selectBtn.IndexData = skill.ResourceId;

		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
		if (skillCfg == null)
		{
			Debug.LogError("SkillCfg Not Found Id " + skill.ResourceId.ToString("X"));
			return;
		}

		// Set Skill Icon.
		iconBtn.SetData(skill);

		// Set Assemble Icon.
		assembleIcon.Hide(!skill.IsAssembleActive);

		// Set Skill Name.
		skillNameLabel.Text = ItemInfoUtility.GetAssetName(skill.ResourceId);

		//Set skill quality.
		skillQualityLabel.Text = GameDefines.textColorBtnYellow.ToString() + GameUtility.GetUIString("UIDlgAvatarFilter_QualityTitle") + ": " + ItemInfoUtility.GetAssetQualityDesc(skill.ResourceId, true, true);

		// Set equip owner.
		// Only LineUp In Current Position, show lineUp message label.
		skillEquipLabel.Text = PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, positionId, skill.Guid, skill.ResourceId) ? GameUtility.GetUIString("UIPnl_LineUp1") : string.Empty;
		// Whatever is lined up in position , show Line Up button.
		bool isLineUp = PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, skill.Guid, skill.ResourceId);
		detailInfoBtn.Hide(!isLineUp);

		// Set Attribute.
		for (int i = 0; i < skillAttributeLabels.Count; i++)
			skillAttributeLabels[i].Text = string.Empty;

		var modifiers = skillCfg.GetLevelModifers(skill.LevelAttrib.Level);
		if (modifiers != null)
		{
			var attributes = PlayerDataUtility.MergeAttributes(modifiers, true, true);

			for (int i = 0; i < attributes.Count && i < skillAttributeLabels.Count; i++)
				skillAttributeLabels[i].Text = GameUtility.FormatUIString(
						"UIElemSkillItem_AttributeDetail",
						GameDefines.textColorBtnYellow,
						_AvatarAttributeType.GetDisplayNameByType(attributes[i].type, ConfigDatabase.DefaultCfg),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
		}
	}
}

