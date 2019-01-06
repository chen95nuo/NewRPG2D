using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;


public class UIElemInviteItemRoot : UIListItemContainerEx
{
	private UIElemInviteItem uiItem;
	public UIElemInviteItem UiItem
	{
		set { uiItem = null; }
	}

	private KodGames.ClientClass.InviteReward rewardItemData;
	public KodGames.ClientClass.InviteReward RewardItemData
	{
		set { rewardItemData = value; }
		get { return rewardItemData; }
	}

	public override void OnEnable()
	{
		base.OnEnable();

		if (Application.isPlaying)
		{
			if (SubItem != null)
				uiItem = SubItem.GetComponent<UIElemInviteItem>();
			SetData(this.rewardItemData);
		}
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
			uiItem = null;
		base.OnDisabled();
	}

	public void SetData(KodGames.ClientClass.InviteReward rewardItem)
	{
		RewardItemData = rewardItem;

		if (uiItem == null || rewardItem == null)
			return;

		//Set Text.
		uiItem.leftCenter.Text = GameUtility.FormatUIString("UIPnlActivityInvite_Item_Count",
															GameDefines.textColorBtnYellow.ToString(),
															GameDefines.textColorBlue.ToString(),
															RewardItemData.RequireCount);
		uiItem.leftDown.Text = GameUtility.FormatUIString("UIPnlActivityInvite_Item_Level",
															GameDefines.textColorBtnYellow.ToString(),
															GameDefines.textColorOrange.ToString(),
															RewardItemData.RequireLevel);

		//Set Reward.
		SetRewardItem();

		//Set Btn Stage.
		SetButtonState(RewardItemData.PickState);
	}

	private void SetRewardItem()
	{
		List<ClientServerCommon.Reward> rewards = SysLocalDataBase.CCRewardToCSCReward(RewardItemData.Reward, true);

		int index = 0;
		for (; index < Mathf.Min(Math.Min(uiItem.rewardCounts.Count, uiItem.rewards.Count), rewards.Count); index++)
		{
			uiItem.rewardCounts[index].Text = GameUtility.FormatUIString("UIPnlActivityInvite_Item_Reward_Count", rewards[index].count);
			uiItem.rewards[index].Hide(false);

			if (rewards[index].level == 0)
				uiItem.rewards[index].SetData(rewards[index].id);
			else
				uiItem.rewards[index].SetData(rewards[index].id, rewards[index].breakthoughtLevel, rewards[index].level);

			uiItem.rewards[index].Data = rewards[index];
		}

		for (; index < Mathf.Min(uiItem.rewards.Count, uiItem.rewardCounts.Count); index++)
		{
			uiItem.rewardCounts[index].Text = string.Empty;

			uiItem.rewards[index].SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, string.Empty);
			uiItem.rewards[index].Hide(true);
		}
	}

	//设置成为公共方法并且带参数是为了方便外部调用修改Item的状态
	public void SetButtonState(int stage)
	{
		if (RewardItemData.PickState != stage)
			RewardItemData.PickState = stage;

		//Set Button Data.
		uiItem.getRewardBtn.Data = RewardItemData;

		switch (stage)
		{
			//不可领取
			case _InviteCodeRewardPickState.UnPickable:
				uiItem.getRewardBox.Hide(true);
				uiItem.getRewardBtn.Hide(false);
				uiItem.getRewardNotify.Hide(true);
				UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetIcon(uiItem.getRewardBtn, true);
				break;
			//可领取
			case _InviteCodeRewardPickState.Pickable:
				uiItem.getRewardBox.Hide(true);
				uiItem.getRewardBtn.Hide(false);
				uiItem.getRewardNotify.Hide(false);
				UIElemTemplate.Inst.disableStyleClickableBtnTemplate.SetIcon(uiItem.getRewardBtn, false);
				break;
			//夷陵区
			case _InviteCodeRewardPickState.HasPicked:
				uiItem.getRewardBtn.Hide(true);
				uiItem.getRewardBox.Hide(false);
				uiItem.getRewardNotify.Hide(true);

				break;
		}
	}
}