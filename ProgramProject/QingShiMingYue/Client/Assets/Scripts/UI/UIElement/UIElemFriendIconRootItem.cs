using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendIconRootItem : UIListItemContainerEx
{
	private UIElemFriendIconItem uiItem;

	private List<Reward> reward;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendIconItem>();

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

		for (int index = 0; index < Mathf.Min(uiItem.rewardIcons.Count, uiItem.rewardNames.Count); index++)
		{
			uiItem.rewardNames[index].Text = string.Empty;
			uiItem.rewardIcons[index].Hide(true);
		}

		if (rewards == null)
			return;

		for (int index = 0; index < Mathf.Min(rewards.Count, Mathf.Min(uiItem.rewardIcons.Count, uiItem.rewardNames.Count)); index++)
		{
			uiItem.rewardNames[index].Text = ItemInfoUtility.GetAssetName(rewards[index].id);
			uiItem.rewardIcons[index].Hide(false);
			uiItem.rewardIcons[index].SetData(rewards[index].id, rewards[index].count);
		}
	}
}
