using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDungeonStarReward : MonoBehaviour
{
	public List<UIElemAssetIcon> rewardIcons;

	public void SetData(List<Reward> rewards)
	{
		for (int index = 0; index < rewardIcons.Count; index++)
			rewardIcons[index].Hide(true);

		for (int index = 0; index < Math.Min(rewardIcons.Count, rewards.Count); index++)
		{
			rewardIcons[index].Hide(false);
			rewardIcons[index].SetData(rewards[index].id, rewards[index].count);
			rewardIcons[index].border.Text = ItemInfoUtility.GetAssetName(rewards[index].id);
		}
	}
}