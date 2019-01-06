using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemChooseCardSkill : UIElemChooseCardBasic
{
	public SpriteText[] attributeLabels;

	public void SetData(KodGames.ClientClass.Skill skill)
	{
		SetData(skill, false);
	}

	public void SetData(KodGames.ClientClass.Skill skill, bool selected)
	{
		SetBaseData(skill.ResourceId, skill.Guid, selected);

		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);

		// Set Attribute.
		for (int i = 0; i < attributeLabels.Length; i++)
			attributeLabels[i].Text = string.Empty;

		var modifiers = skillCfg.GetLevelModifers(skill.LevelAttrib.Level);
		if (modifiers != null)
		{
			var attributes = PlayerDataUtility.MergeAttributes(modifiers, true, true);

			for (int i = 0; i < attributes.Count && i < attributeLabels.Length; i++)
				attributeLabels[i].Text = GameUtility.FormatUIString(
						"UIElemChooseCardSkill_AttributeDetail",
						GameDefines.textColorBtnYellow,
						_AvatarAttributeType.GetDisplayNameByType(attributes[i].type, ConfigDatabase.DefaultCfg),
						GameDefines.textColorWhite,
						ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
		}

		itemBg.Data = this;
		itemIcon.SetData(skill);
	}
}
