using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemFriendCampaginWeekRewardRootItem : UIListItemContainerEx
{
	private UIElemFriendCampaginWeekRewardItem uiItem;

	private com.kodgames.corgi.protocol.FCRewardInfo rewardInfo;

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemFriendCampaginWeekRewardItem>();

			SetData(rewardInfo);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;

		base.OnDisabled();
	}

	public void SetData(com.kodgames.corgi.protocol.FCRewardInfo rewardInfo)
	{
		this.rewardInfo = rewardInfo;

		if (uiItem == null)
			return;

		HideRewards();

		if (rewardInfo.upperLimit != rewardInfo.lowerLimit)
			uiItem.titleLabel.Text = GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_RewardTitle_1", rewardInfo.lowerLimit, rewardInfo.upperLimit);
		else
			uiItem.titleLabel.Text = GameUtility.FormatUIString("UIPnlFriendCampaginWeekReward_RewardTitle_2", rewardInfo.lowerLimit);

		for (int index = 0; index < Mathf.Min(Mathf.Min(uiItem.rewardCounts.Count, uiItem.rewards.Count), this.rewardInfo.fcRewards.Count); index++)
		{
			uiItem.rewardCounts[index].Text = this.rewardInfo.fcRewards[index].count.ToString();
			uiItem.rewards[index].Hide(false);

			//Build Reward.
			Reward reward = new Reward();
			reward.id = this.rewardInfo.fcRewards[index].id;
			reward.level = this.rewardInfo.fcRewards[index].level;
			reward.breakthoughtLevel = this.rewardInfo.fcRewards[index].breakThroughLevel;

			if (this.rewardInfo.fcRewards[index].level == 0)
				uiItem.rewards[index].SetData(this.rewardInfo.fcRewards[index].id);
			else
				uiItem.rewards[index].SetData(this.rewardInfo.fcRewards[index].id, this.rewardInfo.fcRewards[index].breakThroughLevel, this.rewardInfo.fcRewards[index].level);

			uiItem.rewards[index].Data = reward;
		}
	}

	private void HideRewards()
	{
		for (int index = 0; index < Mathf.Min(uiItem.rewardCounts.Count, uiItem.rewards.Count); index++)
		{
			uiItem.rewardCounts[index].Text = string.Empty;
			uiItem.rewards[index].SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
			uiItem.rewards[index].Hide(true);
		}
	}
}
