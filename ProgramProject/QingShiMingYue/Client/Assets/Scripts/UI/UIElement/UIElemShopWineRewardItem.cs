using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemShopWineRewardItem : MonoBehaviour
{
	public List<UIElemAssetIcon> rewards;
	public List<SpriteText> rewardNames;

	public void SetData(List<int> rewardIds)
	{
		//Clean Icon
		for (int index = 0; index < Mathf.Min(rewards.Count, rewardNames.Count); index++)
		{
			rewardNames[index].Text = string.Empty;			
			rewards[index].border.Hide(true);			
		}

		//Fill List
		for (int index = 0; index < Mathf.Min(Mathf.Min(rewards.Count, rewardNames.Count), rewardIds.Count); index++)
		{
			rewardNames[index].Text = ItemInfoUtility.GetAssetName(rewardIds[index]);
			if (rewardIds[index] != IDSeg.InvalidId)
			{
				rewards[index].border.Hide(false);
				rewards[index].SetData(rewardIds[index]);
			}					
		}
	}
}
