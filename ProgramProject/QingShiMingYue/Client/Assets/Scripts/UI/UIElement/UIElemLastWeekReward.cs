using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemLastWeekReward : MonoBehaviour
{
	public SpriteText RankLabel;
	public GameObjectPool rewardPool;
	public AutoSpriteControlBase bgBox;
	private List<UIElemTowerRewardListItem> rewardListItems = new List<UIElemTowerRewardListItem>();

	public void SetData(MelaleucaFloorConfig.WeekRewardShow weekRewardShow)
	{
		RankLabel.Text  = weekRewardShow.From;

		FillData(weekRewardShow.RewardShow);
	}

	public void ReleaseRewardItem()
	{	
		for (int index = 0; index < rewardListItems.Count; index++)
			rewardPool.ReleaseItem(rewardListItems[index].gameObject);
		
		rewardListItems.Clear();
	}

	public void FillData(List<MelaleucaFloorConfig.RewardShow > rewardShows)
	{
		float LineHeight = -40f;

		UIElemTowerRewardListItem item = rewardPool.AllocateItem().GetComponent<UIElemTowerRewardListItem>();
		item.SetData(rewardShows);
		rewardListItems.Add(item);

		item.transform.parent = bgBox.transform;
		item.transform.localPosition = new Vector3(0, LineHeight, 0);
	}
}
