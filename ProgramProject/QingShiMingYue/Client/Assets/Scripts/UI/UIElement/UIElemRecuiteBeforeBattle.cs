using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIElemRecuiteBeforeBattle : MonoBehaviour
{
	public UIElemAssetIcon npcIcon;
	public UIBox traitBox;
	public SpriteText nameLabel;
	public UIElemAssetIcon skillLeft;
	public UIElemAssetIcon skillRight;
	public SpriteText levelLabel;
	public UIButton BgButton;
	public SpriteText npcDesc;

	public void SetData(KodGames.ClientClass.RecruiteNpc recruiteNpc)
	{
		BgButton.Data = recruiteNpc;

		// Set Npc Icon.
		npcIcon.SetData(recruiteNpc.AvatarId);

		// Set Trait Icon.
		UIElemTemplate.Inst.SetAvatarTraitIcon(traitBox, recruiteNpc.TraitType);

		// Set Npc name.
		nameLabel.Text = recruiteNpc.Name;
		npcDesc.Text = recruiteNpc.NpcDesc;

		// Set Npc Level.
		levelLabel.Text = GameUtility.FormatUIString("UIPnlCampaign_RecruiteLevel", recruiteNpc.Level);

		// Set Skill Icon.
		if (recruiteNpc.SkillIds.Count > 0)
		{
			skillLeft.Hide(false);
			skillLeft.SetData(recruiteNpc.SkillIds[0]);
			skillLeft.Text = ItemInfoUtility.GetAssetName(recruiteNpc.SkillIds[0]);
		}
		else
			skillLeft.Hide(true);

		if (recruiteNpc.SkillIds.Count > 1)
		{
			skillRight.Hide(false);
			skillRight.SetData(recruiteNpc.SkillIds[1]);
			skillRight.Text = ItemInfoUtility.GetAssetName(recruiteNpc.SkillIds[1]);
		}
		else
			skillRight.Hide(true);
	}
}
