using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAdventureBuyRewardItem : MonoBehaviour
{
	public List<UIElemAssetIcon> itemIcons;

	public void SetData(List<Reward> rewards, bool showCountInName)
	{
		int index = 0;
		for (; index < rewards.Count; index++)
		{
			itemIcons[index].Hide(false);
			itemIcons[index].GetComponent<UIButton>().Data = rewards[index].id;
			itemIcons[index].SetData(rewards[index].id, rewards[index].count, showCountInName);
		}

		for (; index < itemIcons.Count; index++)
		{
			itemIcons[index].Hide(true);
			itemIcons[index].Text = "";
		}
	}
}
