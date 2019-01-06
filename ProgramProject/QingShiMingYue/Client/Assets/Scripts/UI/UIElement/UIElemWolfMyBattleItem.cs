using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemWolfMyBattleItem : MonoBehaviour
{
	public class LineUpAvatar : KodGames.ClientClass.Avatar
	{
		public int traitType;
		public string avatarName;
	}

	public SpriteText avatarNameLabel;//角色名字
	public SpriteText avatarLevelLabel;//角色等级
	public UIElemAssetIcon avatarIconBtn;//角色图标
	public UIButton avatarBgBtn;//详细控制
	public SpriteText avatarShotNumLabel;//出手顺序
	public UIBox avatarShotNumBox;//出手顺序的框
	public UIElemAssetIconBreakThroughBtn breakThoughtItem;//突破等级
	public UIBox traitIcon;//类型
	public UIBox recruiteIcon;//门客显示
	public UIProgressBar hpProgress;//显示血量
	public UIBox dieBox;//死亡关闭
	public UIBox alphaBg; // 死亡阴影

	private int position;
	public int Position
	{
		get { return this.position; }
	}

	public KodGames.ClientClass.Avatar AvatarData
	{
		get { return avatarIconBtn.Data as KodGames.ClientClass.Avatar; }
	}

	private bool isRecruiteAvatar;
	public bool IsRecruiteAvatar
	{
		get { return isRecruiteAvatar; }
	}

	public EZDragDropDelegate DragHandle
	{
		set
		{
			this.avatarBgBtn.SetDragDropDelegate(value);
			this.avatarIconBtn.border.SetDragDropDelegate(value);
		}
	}

	//出手顺序
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
			{
				avatarShotNumLabel.Text = string.Empty;
				avatarShotNumBox.Hide(true);
			}
			else
			{
				avatarShotNumBox.Hide(false);
				avatarShotNumLabel.Text = ItemInfoUtility.GetLevelCN(value);
			}
		}
	}

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
				breakThoughtItem.SetBreakThroughIcon(0);
				avatarShotNumLabel.Text = string.Empty;
				avatarShotNumBox.Hide(true);
				traitIcon.Hide(true);
				recruiteIcon.Hide(true);
				hpProgress.Value = 0f;
				hpProgress.Text = string.Empty;
				dieBox.Hide(true);
			}
			else
			{
				// Set Name Label.
				if (AvatarData is LineUpAvatar)
					avatarNameLabel.Text = (AvatarData as LineUpAvatar).avatarName;
				else
					avatarNameLabel.Text = ItemInfoUtility.GetAssetName(AvatarData.ResourceId);

				avatarShotNumLabel.Text = ItemInfoUtility.GetLevelCN(avatarShotNum);
				avatarShotNumBox.Hide(avatarShotNum <= 0);
				avatarLevelLabel.Text = GameUtility.FormatUIString("UILevelPrefix", AvatarData.LevelAttrib.Level);
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

	//获得这个位置上是否显示该位置上的角色
	private bool hasAvatar;
	public bool HasAvatar
	{
		get
		{
			return !avatarIconBtn.IsHidden();
		}
	}

	public void init(int battlePos)
	{
		position = battlePos;

		recruiteIcon.Hide(true);
		avatarIconBtn.Hide(true);
		traitIcon.Hide(true);
		breakThoughtItem.SetBreakThroughIcon(0);

		dieBox.Hide(true);
		alphaBg.Hide(true);

		avatarNameLabel.Text = string.Empty;
		avatarLevelLabel.Text = string.Empty;

		hpProgress.Value = 0;
		hpProgress.Text = string.Empty;

		AvatarShotNum = PlayerDataUtility.GetIndexPosByBattlePos(battlePos);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar, bool isRecruiteAvatar)
	{
		this.isRecruiteAvatar = isRecruiteAvatar;

		if (avatar == null)
		{
			avatarIconBtn.Data = null;
			avatarNameLabel.Text = string.Empty;
			avatarLevelLabel.Text = string.Empty;
			avatarIconBtn.Hide(true);
			traitIcon.Hide(true);
			recruiteIcon.Hide(true);
			breakThoughtItem.SetBreakThroughIcon(0);
			dieBox.Hide(true);
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
			recruiteIcon.Hide(!isRecruiteAvatar);
		}
	}

	public void SetData(KodGames.ClientClass.Location location, float value)
	{
		if (location == null)
		{
			alphaBg.Hide(true);
			dieBox.Hide(true);
			hpProgress.Value = 0;
			hpProgress.Text = string.Empty;
		}
		else
		{
			hpProgress.Value = value;
			hpProgress.Text = GameUtility.FormatUIString("UIPnlWolfMyBattle_HPpro", ((value <= 0.01 && value > 0) ? 1 : (int)(value * 100)));
			dieBox.Hide(value > 0);

			if (value > 0)
				alphaBg.Hide(true);
			else
				alphaBg.Hide(false);
		}
	}
}
