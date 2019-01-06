using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemWoldExpeditionBattles : MonoBehaviour
{
	public class LineUpAvatar : KodGames.ClientClass.Avatar
	{
		public int traitType;
		public string avatarName;
		public int avatarLevel;
	}

	public UIElemAssetIcon avatarIcon;//图标
	public UIBox recruiteIcon;//门客
	public SpriteText recruiteLabel;//门客
	public UIBox avatarType;//类型
	public SpriteText avatarName;//名字
	public SpriteText avatarLevel;//等级
	public UIElemAssetIconBreakThroughBtn breakThoughtItem;//突破等级

	private int position;
	public int Position
	{
		get { return this.position; }
	}

	public void Init(int battlePos)
	{
		position = battlePos;
		recruiteIcon.Hide(true);
		recruiteLabel.Hide(true);
		avatarLevel.Text = "";
		breakThoughtItem.SetBreakThroughIcon(0);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar, bool isRecruiteAvatar)
	{
		if (avatar == null)
		{
			avatarIcon.Data = null;
			avatarName.Text = "";
			avatarLevel.Text = "";

			if (avatarIcon.breakBorder != null)
				avatarIcon.breakBorder.SetBreakThroughIcon(0);
			if (avatarIcon.rightLable != null)
				avatarIcon.rightLable.Text = "";
			recruiteIcon.Hide(true);
			recruiteLabel.Hide(true);
			avatarType.Hide(true);
		}
		else
		{
			if (avatar is LineUpAvatar)
				avatarName.Text = (avatar as LineUpAvatar).avatarName;
			else
				avatarName.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);

			// Set Avatar Icon.
			avatarIcon.SetData(avatar);
			avatarIcon.Data = avatar;

			// Set Avatar Level Label.
			avatarLevel.Hide(false);
			avatarLevel.Text = GameUtility.FormatUIString("UILevelPrefix", avatar.LevelAttrib.Level);

			// Set Avatar Break Level.
			breakThoughtItem.Hide(false);
			breakThoughtItem.SetBreakThroughIcon(avatar.BreakthoughtLevel);

			int type = avatar is LineUpAvatar ? (avatar as LineUpAvatar).traitType : ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).traitType;
			UIElemTemplate.Inst.SetAvatarTraitIcon(avatarType, type);
			avatarType.Hide(false);

			//是否是门客
			recruiteIcon.Hide(!isRecruiteAvatar);
			recruiteLabel.Hide(!isRecruiteAvatar);
		}
	}

	public void ClearData()
	{
		avatarIcon.Data = null;
		//清理里面的数据
		breakThoughtItem.SetBreakThroughIcon(0);
		recruiteIcon.Hide(true);
		recruiteLabel.Hide(true);
		avatarType.Hide(true);
		avatarName.Text = "";
		avatarLevel.Text = "";
		avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
	}
}