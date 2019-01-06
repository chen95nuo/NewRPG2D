using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDanBoxReward : MonoBehaviour
{
	public UIElemAssetIcon[] itemIcons;

	public void SetData(List<com.kodgames.corgi.protocol.ShowReward> showRewards)
	{
		if(showRewards.Count > itemIcons.Length)
		{
			Debug.Log("UIElemDanBoxReward ItemIcons Count Error");
			return;
		}

		for (int i = 0; i < itemIcons.Length; i++)
		{
			itemIcons[i].Hide(true);
			if (i < showRewards.Count)
			{				
				itemIcons[i].Hide(false);
				itemIcons[i].SetData(showRewards[i].id, showRewards[i].breakthought, showRewards[i].level,showRewards[i].count);
				itemIcons[i].Data = showRewards[i];
			}				
		}
	}
}
