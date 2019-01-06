using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDungeonGuidNpc : MonoBehaviour
{
	[System.Serializable]
	public class NpcData
	{
		public UIElemAssetIcon npcIcon;
		public UIBox traitBox;
		public SpriteText nameLabel;

		public void SetData(int assetId, int level, int traitType, string avatarName)
		{
			// Set AvatarIcon.
			npcIcon.SetData(assetId, 0, level);

			// Set Name Label.
			nameLabel.Text = avatarName;

			// Set Trait Icon.
			traitBox.Hide(false);
			UIElemTemplate.Inst.SetAvatarTraitIcon(traitBox, traitType);
		}

		public void SetData(int assetId, int level)
		{
			SetData(assetId, level, ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(assetId).traitType, ItemInfoUtility.GetAssetName(assetId));
		}

		public void SetEmpty()
		{
			traitBox.Hide(true);
			nameLabel.Text = string.Empty;
			npcIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconLockBgBtn, null);
		}
	}

	public List<NpcData> npcDatas;
}