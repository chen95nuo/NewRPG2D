using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemWolfPointIcon : MonoBehaviour
{
	public List<UIElemAssetIcon> rewardIcons;

	public void SetData(List<Reward> rewards)
	{
		for (int index = 0; index < rewardIcons.Count; index++)
		{
			rewardIcons[index].Hide(true);
		}

		for (int index = 0; index < rewardIcons.Count && index < rewards.Count; index++)
		{
			rewardIcons[index].Hide(false);
			rewardIcons[index].SetData(rewards[index].id, rewards[index].count);
			rewardIcons[index].Data = rewards[index];
		}
	}

	public int GetIconCount()
	{
		return rewardIcons.Count;
	}
}