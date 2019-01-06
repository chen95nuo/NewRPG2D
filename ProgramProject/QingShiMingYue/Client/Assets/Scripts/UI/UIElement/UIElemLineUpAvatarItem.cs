using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLineUpAvatarItem : MonoBehaviour
{
	public class LineUpAvatar : KodGames.ClientClass.Avatar
	{
		public int traitType;
		public string avatarName;
	}

	public SpriteText avatarNameLabel;
	public SpriteText avatarLevelLabel;
	public SpriteText avatarShotNumLabel;
	public UIElemAssetIcon avatarIconBtn;
	public UIButton avatarBgBtn;
	public UIElemAssetIconBreakThroughBtn breakThoughtItem;
	public UIBox traitIcon;
	public UIBox recruiteIcon;

	private int position;
	public int Position { get { return this.position; } }

	private bool isDraged;
	public bool IsDraged
	{
		set
		{
			if (avatarIconBtn.IsHidden())
				return;

			isDraged = value;

			if (isDraged)
			{
				avatarNameLabel.Text = string.Empty;
				avatarLevelLabel.Text = string.Empty;
				avatarShotNumLabel.Text = string.Empty;
				breakThoughtItem.SetBreakThroughIcon(0);
				traitIcon.Hide(true);
				recruiteIcon.Hide(true);
			}
			else
			{
				// Set Name Label.
				if (AvatarData is LineUpAvatar)
					avatarNameLabel.Text = (AvatarData as LineUpAvatar).avatarName;
				else
					avatarNameLabel.Text = ItemInfoUtility.GetAssetName(AvatarData.ResourceId);

				avatarLevelLabel.Text = GameUtility.FormatUIString("UILevelPrefix", AvatarData.LevelAttrib.Level);
				avatarShotNumLabel.Text = avatarShotNum.ToString();
				breakThoughtItem.SetBreakThroughIcon(AvatarData.BreakthoughtLevel);
				traitIcon.Hide(false);
				recruiteIcon.Hide(!isRecruiteAvatar);
			}
		}
	}

	private bool isClose;
	public bool IsClose
	{
		set
		{
			isClose = value;

			if (isClose)
				UIUtility.CopyIcon(avatarBgBtn, UIElemTemplate.Inst.iconBorderTemplate.iconLockBgBtn);
			else
				UIUtility.CopyIcon(avatarBgBtn, UIElemTemplate.Inst.iconBorderTemplate.iconBgBtn);
		}
		get
		{
			return isClose;
		}
	}

	private bool hasAvatar;
	public bool HasAvatar
	{
		get
		{
			return !avatarIconBtn.IsHidden();
		}
	}

	public bool IsNpc
	{
		get { return AvatarData != null && string.IsNullOrEmpty(AvatarData.Guid); }
	}

	public KodGames.ClientClass.Avatar AvatarData { get { return avatarIconBtn.Data as KodGames.ClientClass.Avatar; } }

	private bool isRecruiteAvatar;
	public bool IsRecruiteAvatar { get { return isRecruiteAvatar; } }

	private int showLocationId;
	public int ShowLocationId { get { return showLocationId; } }

	private int avatarShotNum;
	public int AvatarShotNum
	{
		get
		{
			return avatarShotNum;
		}

		set
		{
			avatarShotNum = value;

			if (value <= 0)
				avatarShotNumLabel.Text = string.Empty;
			else
				avatarShotNumLabel.Text = value.ToString();
		}
	}

	public EZDragDropDelegate DragHandle
	{
		set
		{
			this.avatarBgBtn.SetDragDropDelegate(value);
			this.avatarIconBtn.border.SetDragDropDelegate(value);
		}
	}

	public void Init(int battlePos)
	{
		this.position = battlePos;

		// Set BackGroundIcon Disable.
		avatarBgBtn.controlIsEnabled = false;

		// Set Default UI Show.
		AvatarShotNum = PlayerDataUtility.GetIndexPosByBattlePos(battlePos);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar, bool isRecruiteAvatar)
	{
		SetData(avatar, isRecruiteAvatar, -1);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar, bool isRecruiteAvatar, int showLocationId)
	{
		this.isRecruiteAvatar = isRecruiteAvatar;
		this.showLocationId = showLocationId;

		if (avatar == null)
		{
			avatarIconBtn.Data = null;
			avatarNameLabel.Text = string.Empty;
			avatarLevelLabel.Text = string.Empty;
			avatarIconBtn.Hide(true);
			traitIcon.Hide(true);
			recruiteIcon.Hide(true);
			breakThoughtItem.SetBreakThroughIcon(0);
		}
		else
		{
			// Set Avatar Name Label.
			if (avatar is LineUpAvatar)
				avatarNameLabel.Text = (avatar as LineUpAvatar).avatarName;
			else
				avatarNameLabel.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);

			// Set Avatar Level.
			avatarLevelLabel.Text = GameUtility.FormatUIString("UILevelPrefix", avatar.LevelAttrib.Level);

			// Set Avatar Icon.
			avatarIconBtn.Data = avatar;
			avatarIconBtn.Hide(false);
			avatarIconBtn.SetData(avatar.ResourceId);
			(avatarIconBtn.border as UIButton).SetControlState(UIButton.CONTROL_STATE.OVER, true);
			(avatarIconBtn.border as UIButton).SetControlState(UIButton.CONTROL_STATE.NORMAL);

			// Set Avatar Break Level.
			breakThoughtItem.SetBreakThroughIcon(avatar.BreakthoughtLevel);

			// Set TraitIcon.
			int traitType = avatar is LineUpAvatar ? (avatar as LineUpAvatar).traitType : ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).traitType;
			UIElemTemplate.Inst.SetAvatarTraitIcon(traitIcon, traitType);
			traitIcon.Hide(false);

			// Set RecruteIcon.
			UIUtility.CopyIcon(recruiteIcon, IsNpc ? UIElemTemplate.Inst.iconBorderTemplate.iconAvatarNpc : UIElemTemplate.Inst.iconBorderTemplate.iconAvatarDiner);
			recruiteIcon.Hide(!isRecruiteAvatar);
		}
	}
}
