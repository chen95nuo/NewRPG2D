using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemShopGiftItem : MonoBehaviour
{
	public List<UIElemAssetIcon> itemIcons;

	public void SetData(List<Reward> rewards, bool showCountInName)
	{
		int index = 0;
		for (; index < rewards.Count; index++)
		{
			itemIcons[index].Hide(false);
			itemIcons[index].SetData(rewards[index].id, rewards[index].breakthoughtLevel, rewards[index].level, rewards[index].count, showCountInName);
			itemIcons[index].Data = rewards[index];
		}

		for (; index < itemIcons.Count; index++)
		{
			itemIcons[index].Hide(true);
			itemIcons[index].Text = "";
			itemIcons[index].Data = null;
		}
	}
}
