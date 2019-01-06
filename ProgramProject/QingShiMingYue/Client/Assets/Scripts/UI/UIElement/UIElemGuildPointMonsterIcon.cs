using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildPointMonsterIcon : MonoBehaviour
{
	public UIElemAssetIcon monsterIcon;
	public UIBox numberCost;
	public UIBox deadSign;
	public UIBox battleSign;

	public void SetNumberPlate(int iconId)
	{
		monsterIcon.Hide(false);
		numberCost.Hide(true);
		deadSign.Hide(true);
		battleSign.Hide(true);
		monsterIcon.SetData(iconId);	
	}

	public void SetNumberPlate(int iconId, int number)
	{
		monsterIcon.Hide(false);
		numberCost.Hide(false);
		deadSign.Hide(true);
		battleSign.Hide(true);
		monsterIcon.SetData(iconId);
		monsterIcon.border.SetColor(GameDefines.cardColorGray);
		numberCost.SetState(number - 1);
	}

	public void SetDeadBoss()
	{
		deadSign.Hide(false);
	}

	public void SetBossBattle()
	{
		battleSign.Hide(false);
	}

	public void SetNumberCost(int number)
	{
		monsterIcon.Hide(true);
		numberCost.Hide(false);
		deadSign.Hide(true);
		battleSign.Hide(true);
		numberCost.SetState(number - 1);
	}
}
