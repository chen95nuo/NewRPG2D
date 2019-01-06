using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemNpcLineUp : MonoBehaviour
{

	public UIBox traitIcon;
	public UIElemAssetIcon npcIcon;

	public void SetData(com.kodgames.corgi.protocol.NpcInfo npcInfo)
	{
		npcIcon.SetData(npcInfo.avatarId);
		npcIcon.rightLable.Text = string.Format(GameUtility.GetUIString("UIPnlTowerNpcLineUp_NpcLevel_Label"), npcInfo.level);
		npcIcon.rightLable.gameObject.SetActive(true);
		npcIcon.assetNameLabel.Text = npcInfo.npcName;

		//Set TraitIcon
		traitIcon.Hide(false);
		UIElemTemplate.Inst.SetAvatarTraitIcon(traitIcon, npcInfo.traitType);
	}

	public void SetEmpty()
	{
		traitIcon.Hide(true);
		npcIcon.assetNameLabel.Text = string.Empty;
		npcIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconLockBgBtn, null);
	}
}
