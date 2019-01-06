using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemTowerRewardListItem : MonoBehaviour
{
	public List<UIElemAssetIcon> rewardIcons;

	public void SetData(List<Reward> rewards)
	{
		for (int index = 0; index < rewardIcons.Count; index++)
			rewardIcons[index].Hide(true);

		for (int i = 0; i < rewards.Count && i < rewardIcons.Count; i++)
		{
			rewardIcons[i].Hide(false);
			rewardIcons[i].Data = rewards[i].id;
			rewardIcons[i].SetData(rewards[i].id,rewards[i].count);	
		}
	}

	public void SetData(List<MelaleucaFloorConfig.RewardShow> rewardShows)
	{
		for (int index = 0; index < rewardIcons.Count; index++)
			rewardIcons[index].Hide(true);

		for (int i = 0; i < rewardShows.Count && i < rewardIcons.Count; i++)
		{
			rewardIcons[i].Hide(false);
			rewardIcons[i].SetData(rewardShows[i].RewardId);
			rewardIcons[i].rightLable.Text = rewardShows[i].Desc;
			rewardIcons[i].rightLable.gameObject.SetActive(true);
		}
	}

	public void SetData(KodGames.ClientClass.Reward reward)
	{
		for (int index = 0; index < rewardIcons.Count; index++)
			rewardIcons[index].Hide(true);

		int avatarCount = reward.Avatar.Count;
		int equipCount = reward.Equip.Count;
		int skillCount = reward.Skill.Count;

		if (avatarCount + equipCount + skillCount <= rewardIcons.Count && avatarCount + equipCount + skillCount > 0)
		{
			int i = 0;
			for (; i < avatarCount; i++)
			{
				rewardIcons[i].Hide(false);
				rewardIcons[i].SetData(reward.Avatar[i]);
			}

			for (; i < avatarCount + equipCount; i++)
			{
				rewardIcons[i].Hide(false);
				rewardIcons[i].SetData(reward.Equip[i - avatarCount]);
			}

			for (; i < avatarCount + equipCount + skillCount; i++)
			{
				rewardIcons[i].Hide(false);
				rewardIcons[i].SetData(reward.Skill[i - avatarCount - equipCount]);
			}
		}
	}
}
