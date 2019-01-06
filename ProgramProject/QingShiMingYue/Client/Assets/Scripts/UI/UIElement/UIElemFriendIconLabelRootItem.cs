using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendIconLabelRootItem : UIListItemContainerEx
{
	private UIElemFriendIconLabelItems uiItem;
	private List<Reward> reward;
	private bool shoutongBox;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendIconLabelItems>();

			SetData(reward, shoutongBox);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(List<Reward> rewards, bool shoutong)
	{
		this.reward = rewards;
		this.shoutongBox = shoutong;

		if (uiItem == null)
			return;

		//渲染前进行一次清空
		for (int index = 0; index < Mathf.Min(uiItem.rewardIcons.Count, uiItem.rewardNames.Count); index++)
		{
			uiItem.rewardNames[index].Text = string.Empty;
			uiItem.rewardIcons[index].Hide(true);
			uiItem.shoutongbox.Hide(true);
		}

		if (rewards == null)
			return;

		for (int index = 0; index < Mathf.Min(Mathf.Min(uiItem.rewardIcons.Count, uiItem.rewardNames.Count), rewards.Count); index++)
		{
			uiItem.rewardNames[index].Text = ItemInfoUtility.GetAssetName(rewards[index].id);
			uiItem.rewardIcons[index].Hide(false);
			uiItem.rewardIcons[index].SetData(rewards[index].id, rewards[index].count);

			uiItem.shoutongbox.Hide(!shoutong);
		}
	}
}
