using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganSingleChip : MonoBehaviour
{
	public UIElemAssetIcon chipIcon;
	public UIBox lightBg;

	int chipId = 0;
	public int ChipId { get { return chipId; } }

	public void SetData(KodGames.ClientClass.Consumable chip)
	{
		chipId = chip.Id;
		chipIcon.SetData(chip.Id, chip.Amount);
		chipIcon.border.Data = chipId;
		lightBg.Hide(true);
	}

	//true light false hide
	public void SetLight(bool isLight)
	{		
		lightBg.Hide(!isLight);
	}

	public void Hide(bool isHide)
	{
		chipIcon.Hide(isHide);
	}
}

