using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGiftItemRoot : UIListItemContainerEx
{
	//专门用来管理预制品的
	private UIElemGiftItem uiItem;
	private List<Reward> rewards;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemGiftItem>();

			SetData(rewards);
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
		this.rewards = rewards;

		if (uiItem == null || rewards == null)
			return;

		int index = 0;
		for (; index < Mathf.Min(Mathf.Min(uiItem.rewards.Count, uiItem.rewardCounts.Count), this.rewards.Count); index++)
		{
			uiItem.rewardCounts[index].Text = GameUtility.FormatUIString("UIPnlActivityInvite_Item_Reward_Count", this.rewards[index].count);
			uiItem.rewards[index].Hide(false);

			if (this.rewards[index].level == 0)
				uiItem.rewards[index].SetData(this.rewards[index].id);
			else
				uiItem.rewards[index].SetData(this.rewards[index].id, this.rewards[index].breakthoughtLevel, this.rewards[index].level);

			uiItem.rewards[index].Data = this.rewards[index];
		}

		for (; index < Mathf.Min(uiItem.rewards.Count, uiItem.rewardCounts.Count); index++)
		{
			uiItem.rewardCounts[index].Text = string.Empty;
			uiItem.rewards[index].SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
			uiItem.rewards[index].Hide(true);
		}
	}
}
