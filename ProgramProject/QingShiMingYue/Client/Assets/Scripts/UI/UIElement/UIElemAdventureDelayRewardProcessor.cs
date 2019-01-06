using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemAdventureDelayRewardProcessor : UIListItemContainerEx
{
	private UIElemAdventureDelayReward uiItem;
	private Reward reward;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemAdventureDelayReward>();
			SetData(this.reward);
		}
	}

	public override void OnDisabled()
	{		
		if (Application.isPlaying)
			uiItem = null;
		base.OnDisabled();
	}

	public void SetData(Reward reward)
	{
		this.reward = reward;

		if (uiItem == null || reward == null)
			return;

		uiItem.rewardItem.SetData(reward.id,reward.count);
		uiItem.rewardItem.Data = reward.id;
	}
}