using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendRewardsRootItem : UIListItemContainerEx
{
	private UIElemFriendRewardsItem uiItem;

	private List<Reward> reward;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendRewardsItem>();

			if (reward != null)
				SetData(reward);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(List<Reward> rewards)
	{
		this.reward = rewards;

		if (uiItem == null)
			return;

		for (int index = 0; index < uiItem.rewardIcons.Count; index++)
		{
			uiItem.rewardIcons[index].Hide(true);
		}

		if (rewards == null)
			return;

		for (int index = 0; index < Mathf.Min(rewards.Count, uiItem.rewardIcons.Count); index++)
		{
			uiItem.rewardIcons[index].Hide(false);
			uiItem.rewardIcons[index].SetData(rewards[index].id, rewards[index].count);
			uiItem.rewardIcons[index].Data = rewards[index];
		}
	}
}
