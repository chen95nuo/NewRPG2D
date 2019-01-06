using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemRewardGroupItem : MonoBehaviour 
{
	public List<UIElemRewardItem> rewardItems;
	
	public void SetData(List<int> rewardIds, List<int> rewardCounts)
	{
		foreach(UIElemRewardItem rewardItem in rewardItems)
		{
			int index = rewardItems.IndexOf(rewardItem);
			if(index < rewardIds.Count)
			{
				rewardItem.SetData(rewardIds[index], rewardCounts[index]);
			}
			else
			{
				rewardItem.SetData(0, 0);
			}
		}
	}
}
