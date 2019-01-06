using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAvatarInfoSkill : MonoBehaviour
{
	public UIElemAssetIcon skillIcon;
	public UIBox skillType;

	public void SetData(KodGames.ClientClass.Skill activeSkill)
	{
		if (activeSkill == null)
			return;

		var skillCfg = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(activeSkill.ResourceId);

		// Set Skill Icon.
		if (skillCfg.type == CombatTurn._Type.CompositeSkill && activeSkill.LevelAttrib.Level > 0)
			skillIcon.SetData(activeSkill.ResourceId, 0, activeSkill.LevelAttrib.Level);
		else
			skillIcon.SetData(activeSkill.ResourceId);

		skillIcon.Data = activeSkill;

		// Set Skill Icon Trans.
		if (activeSkill.LevelAttrib.Level <= 0)
			UIUtility.CopyIconTrans(skillIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardGray);
		else
			UIUtility.CopyIconTrans(skillIcon.border, UIElemTemplate.Inst.iconBorderTemplate.iconCardNormal);

		// Set Skill Type.
		UIElemTemplate.Inst.SetSkillTypeIcon(skillType, skillCfg.type);
	}

	public void Hide(bool tf)
	{
		this.gameObject.SetActive(!tf);
	}
}

