using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIElemLineUpAvatar : MonoBehaviour
{
	public UIElemAssetIcon avatarIcon;
	public AutoSpriteControlBase avatarBtn;
	public AutoSpriteControlBase selectedLight;
	public UIBox notifyIcon;
	public SpriteText nameLabel;
	private int index;
	public int Index
	{
		get { return index; }
		set { index = value; }
	}

	public void ClearData()
	{
		nameLabel.Text = string.Empty;
	}

	public void SetData(KodGames.ClientClass.Location avatarLocation, MonoBehaviour script, string invokeMethod)
	{
		SetData(script, invokeMethod);
		avatarIcon.SetData(avatarLocation.ResourceId);
		avatarIcon.Data = avatarLocation;
		avatarBtn.IndexData = avatarLocation.ResourceId;
		nameLabel.Text = ItemInfoUtility.GetAssetName(avatarLocation.ResourceId);

		this.index = PlayerDataUtility.GetIndexPosByBattlePos(avatarLocation.ShowLocationId);
	}

	public void SetData(int index, bool open, bool isEmploy, MonoBehaviour script, string invokeMethod)
	{
		SetData(script, invokeMethod);

		if (isEmploy)
			avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconRecruitBtn, string.Empty);
		else if (open)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlViewAvatar)))
				avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconBgBtn, string.Empty);
			else
				avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
		}
		else
			avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconLockBgBtn, string.Empty);

		avatarIcon.Data = index;
		avatarIcon.border.Text = string.Empty;

		this.index = index;
	}

	public void SetData(MonoBehaviour script, string invokeMethod)
	{
		selectedLight.Hide(true);
		SetNotify(false);
		avatarIcon.Data = this;
		avatarIcon.EnableButton(true);
		avatarIcon.SetTriggerMethod(script, invokeMethod);
		avatarIcon.border.Text = string.Empty;
	}

	public void SetSelectedStat(bool selected)
	{
		if (avatarIcon.Data == null)
			selectedLight.Hide(true);
		else
			selectedLight.Hide(!selected);
	}

	public void SetNotify(bool show)
	{
		if (notifyIcon != null)
			notifyIcon.Hide(!show);
	}
}
