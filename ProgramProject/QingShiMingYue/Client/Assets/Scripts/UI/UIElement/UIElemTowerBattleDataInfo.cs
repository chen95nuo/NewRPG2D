using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerBattleDataInfo : MonoBehaviour
{
	public UIBox deadMark;	
	public SpriteText avatarName;
	public UIProgressBar hpBar;
	public UIElemAssetIcon avatarIcon;

	public void SetData(int avatarId, float leftHp, double MaxHp)
	{
		avatarIcon.SetData(avatarId);
		avatarName.Text = ItemInfoUtility.GetAssetName(avatarId);
		if (leftHp > 0)
		{
			deadMark.Hide(true);
			avatarIcon.border.SetColor(new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f));
			float hpPersent = leftHp / (float)MaxHp;
			hpBar.Value = hpPersent;
			hpBar.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleData_AvatarHpPersent_Label"), (int)(hpPersent * 100));
		}
		else
		{
			deadMark.Hide(false);
			avatarIcon.border.SetColor(new Color(128 / 255f, 128 / 255f, 128 / 255f, 255 / 255f));
			hpBar.Value = 0f;
			hpBar.Text = string.Empty;
		}
	}

	public void Hide(bool isHide)
	{
		deadMark.Hide(isHide);
		hpBar.Hide(isHide);
		avatarIcon.Hide(isHide);
		avatarName.Text = string.Empty;		
	}
}
