using UnityEngine;
using System.Collections;

public class UIElemDungeonRewardItem : MonoBehaviour
{
	public UIElemAssetIcon rewardIcon;
	public GameObject receivedMask;//首次通关已领时，图标会有遮罩，并有美术字“已领”

	public void SetData(int rewardId, int rewardCount, bool showFirstPassRewardReceivedMask)
	{
		rewardIcon.Data = rewardId;
		rewardIcon.SetData(rewardId, rewardCount);
		receivedMask.SetActive(showFirstPassRewardReceivedMask);
	}

}