using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLevelRewardItem : MonoBehaviour
{
	public List<UIElemAssetIcon> rewardIcons;
	public List<SpriteText> IconNames;

	public void SetData(List<Reward> rewards)
	{
		for (int index = 0; index < rewardIcons.Count; index++)
		{
			rewardIcons[index].Hide(true);
			IconNames[index].Hide(true);
		}

		for (int index = 0; index < Math.Min(rewardIcons.Count, rewards.Count); index++)
		{
			rewardIcons[index].Hide(false);
			IconNames[index].Hide(false);
			rewardIcons[index].SetData(rewards[index].id, rewards[index].count);
			IconNames[index].Text = ItemInfoUtility.GetAssetName(rewards[index].id);
		}
	}
}
