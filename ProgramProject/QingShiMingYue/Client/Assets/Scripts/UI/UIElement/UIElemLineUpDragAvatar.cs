using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLineUpDragAvatar : MonoBehaviour
{
	public SpriteText avatarNameLabel;
	public UIElemAssetIcon avatarIconBtn;
	public UIButton avatarBgBtn;
	public UIBox traitIcon;

	private int position;
	public int Position { get { return this.position; } }

	private int battlePosType;
	public int BattlePosType { get { return battlePosType; } }

	private bool isDraged;
	public bool IsDraged // 拖拽处理 Drag set
	{
		// 拖拽中隐藏姓名版、类型
		set
		{
			isDraged = value;
			if (!avatarIconBtn.IsHidden())
			{
				if (avatarNameLabel != null) 
				{
					if (isDraged)  
						avatarNameLabel.Text = "";  
					else if (GetAvatarData() != null)
							avatarNameLabel.Text = ItemInfoUtility.GetAssetName(GetAvatarData().ResourceId);
						  else
							avatarNameLabel.Text = "";
				}
				HideTypeIcon(isDraged);
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

	public EZDragDropDelegate DragHandle
	{
		set
		{
			this.avatarBgBtn.SetDragDropDelegate(value);
			this.avatarIconBtn.border.SetDragDropDelegate(value);
		}
	}

	public void Init(int battlePos, int battlePosType)
	{
		// 初始化布阵
		this.position = battlePos;                         
		this.battlePosType = battlePosType;
		// Set BackGroundIcon Disable.	
		avatarBgBtn.controlIsEnabled = false;

		// Set Default UI Show.
		IsDraged = false;

		HideTypeIcon(true);
	}

	public void SetData(KodGames.ClientClass.Avatar avatar)
	{
		// Hide As Default.
		avatarIconBtn.Data = null;
		avatarIconBtn.Hide(true);
		avatarNameLabel.Text = "";
		HideTypeIcon(true);

		if (avatar != null)
		{
			avatarIconBtn.Data = avatar;
			avatarIconBtn.Hide(false);
			avatarIconBtn.SetData(avatar.ResourceId);
			avatarNameLabel.Text = ItemInfoUtility.GetAssetName(avatar.ResourceId);
			int traitType = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).traitType;
			traitIcon.SetState(traitType);

			HideTypeIcon(false);
		}
	}

	public void HideTypeIcon(bool hideOrNot)
	{
		if (traitIcon != null)
			traitIcon.Hide(hideOrNot);
	}

	public KodGames.ClientClass.Avatar GetAvatarData()
	{
		return avatarIconBtn.Data as KodGames.ClientClass.Avatar;
	}



	
}
