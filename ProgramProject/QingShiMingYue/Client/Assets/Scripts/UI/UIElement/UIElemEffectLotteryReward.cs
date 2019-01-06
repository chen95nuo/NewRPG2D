using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemEffectLotteryReward : MonoBehaviour
{
	public UIElemAssetIcon rewardIcon;
	public SpriteText rewardName;
	public UIListItemContainer container;

	public void SetData(Reward reward)
	{
		rewardIcon.SetData(reward.id, reward.count);
		rewardIcon.Data = reward;
		rewardName.Text = ItemInfoUtility.GetAssetName(reward.id);
	}

	public void EnableButton(bool enable)
	{
		rewardIcon.EnableButton(enable);
	}
}
