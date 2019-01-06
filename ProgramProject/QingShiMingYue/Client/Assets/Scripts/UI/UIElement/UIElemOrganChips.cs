using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemOrganChips : MonoBehaviour
{
	public UIElemOrganSingleChip[] chipIcons;

	public void SetData(List<KodGames.ClientClass.Consumable> chips)
	{
		for (int i = 0; i < chipIcons.Length; i++)
		{
			chipIcons[i].Hide(true);

			if (i < chips.Count)
			{
				chipIcons[i].Hide(false);
				chipIcons[i].SetData(chips[i]);
			}
				
		}
	}

	public void SetLight(int chipId)
	{
		for (int i = 0; i < chipIcons.Length; i++ )
		{
			if (chipIcons[i].ChipId == chipId)
				chipIcons[i].SetLight(true);
			else
				chipIcons[i].SetLight(false);
		}
	}
}

