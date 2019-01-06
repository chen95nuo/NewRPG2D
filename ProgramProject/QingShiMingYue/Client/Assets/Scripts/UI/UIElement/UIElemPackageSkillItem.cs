using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemPackageSkillItem : UIElemPackageItemBase
{
	public UIElemAssetIcon skillIcon;
	public UIButton powerUpBtn;

	public UIButton explicitBtn;
	public SpriteText skillName;
	public SpriteText skillQuality;
	public SpriteText isEquiped;
	public List<SpriteText> attributeLabels;
	public SpriteText itemState;

	private KodGames.ClientClass.Skill skill;
	public KodGames.ClientClass.Skill Skill { get { return skill; } }

	public void SetData(KodGames.ClientClass.Skill skill)
	{
		this.skill = skill;
		container.Data = this;

		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
		if (skillCfg == null)
		{
			Debug.LogError("SkillCfg Not Found Id " + skill.ResourceId.ToString("X"));
			return;
		}

		// Set the Skill Icon and Data.
		skillIcon.SetData(skill);
		skillIcon.Data = skill;

		// Set the Skill Name.
		skillName.Text = ItemInfoUtility.GetAssetName(skill.ResourceId);

		// Set the Skill Quality.
		skillQuality.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(skill.ResourceId);

		// Set Attribute.
		for (int i = 0; i < attributeLabels.Count; i++)
			attributeLabels[i].Text = string.Empty;

		var modifiers = skillCfg.GetLevelModifers(skill.LevelAttrib.Level);
		if (modifiers != null)
		{
			var attributes = PlayerDataUtility.MergeAttributes(modifiers, true, true);

			for (int i = 0; i < attributes.Count && i < attributeLabels.Count; i++)
				attributeLabels[i].Text = GameUtility.FormatUIString(
						"UIDlgAttributeDetailTip_AttributeDetail",
						GameDefines.textColorBtnYellow,
						_AvatarAttributeType.GetDisplayNameByType(attributes[i].type, ConfigDatabase.DefaultCfg),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
		}

		// Set the Skill LevelUpButton's Data.
		powerUpBtn.Data = skill;

		//	Set the Skill explicitBtn's Data
		explicitBtn.Data = skill;

		//// Set the Skill State.
		if (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, skill))
		{
			itemState.Text = GameUtility.GetUIString("UIPackage_EquipmentOwner");
			explicitBtn.Hide(false);
		}
		else
		{
			itemState.Text = string.Empty;
			explicitBtn.Hide(true);
		}

	}
}
