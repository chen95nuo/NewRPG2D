using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemActivityFixReward : MonoBehaviour
{
	public UIElemAssetIcon furnaceIcon;
	public UIElemAssetIcon[] itemIcons;

	public void SetData(List<com.kodgames.corgi.protocol.ShowReward> baseRewards, List<com.kodgames.corgi.protocol.ShowReward> extraRewards)
	{
		furnaceIcon.SetData(baseRewards[0].id, baseRewards[0].breakthought, baseRewards[0].level);
		furnaceIcon.Data = baseRewards[0];
		
		//目前需求固定获得只支持配置两个
		if (extraRewards.Count > itemIcons.Length)
			Debug.Log("Rewards Count Error");
		for (int i = 0; i < itemIcons.Length; i++)
		{
			itemIcons[i].Hide(true);
			if (i < extraRewards.Count)
			{
				itemIcons[i].Hide(false);
				itemIcons[i].SetData(extraRewards[i].id, extraRewards[i].breakthought, extraRewards[i].level, extraRewards[i].count);
				itemIcons[i].Data = extraRewards[i];
			}
		}
	}
}
