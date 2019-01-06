using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemDanExtraReward : MonoBehaviour
{
	public UIElemAssetIcon rewardIcon;	

	public void SetData(com.kodgames.corgi.protocol.ShowReward showReward)
	{
		rewardIcon.SetData(showReward.id, showReward.breakthought, showReward.level, showReward.count);
		rewardIcon.Data = showReward;		
	}

	public void EnableButton(bool enable)
	{
		rewardIcon.EnableButton(enable);
	}
}
