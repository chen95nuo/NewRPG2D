using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemStartServerReawrdItem : MonoBehaviour
{
	public List<UIElemAssetIcon> rewardIcons;	//三个可领取物
	public List<SpriteText> iconNames;	//领取物品名称
	public AutoSpriteControlBase rewardPickedBase;	//已经领取
	public UIButton pickBase;	//领取
	public SpriteText loginDayLabel;	//显示本奖励对应的登陆天数

	public UIListItemContainer container;

	private int loginDay;	//记录本奖励对应的登陆天数

	private int pickID;
	public int PickID
	{
		get { return pickID; }
	}

	public void SetData(StartServerRewardConfig.StartServerReward startServerReward)
	{
		// Set Assistant Data.
		pickBase.GetComponent<UIElemAssistantBase>().assistantData = startServerReward.id;

		// Set PickID.
		pickID = startServerReward.id;

		// Set Container Data.
		container.Data = this;

		// Set PickRewardButton 's Data.
		pickBase.Data = pickID;

		loginDay = startServerReward.day;

		// Set LoginDay Label.
		loginDayLabel.Text = GameUtility.FormatUIString("UIDlgStartServerReward_Item_LoginDay", loginDay);

		// Set RewardIcons an names
		for (int i = 0; i < Math.Min(rewardIcons.Count, iconNames.Count); i++)
		{
			rewardIcons[i].Hide(true);
			iconNames[i].Hide(true);
		}


		for (int index = 0; index < System.Math.Min(rewardIcons.Count, startServerReward.rewards.Count); index++)
		{
			if (startServerReward.rewards[index] != null)
			{
				// Set Icon.
				rewardIcons[index].Hide(false);
				rewardIcons[index].SetData(startServerReward.rewards[index].id, startServerReward.rewards[index].count);
				rewardIcons[index].Data = startServerReward.rewards[index].id;

				// Set Name.
				iconNames[index].Hide(false);
				iconNames[index].Text = ItemInfoUtility.GetAssetName(startServerReward.rewards[index].id);
			}
		}


		// Set Controller Base.
		RefreshControllView();
	}

	/// <summary>
	///  控制按钮的显示状态
	/// </summary>
	public void RefreshControllView()
	{
		if (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.DayCount >= loginDay)
			pickBase.controlIsEnabled = true;
		else
			pickBase.controlIsEnabled = false;

		//获取领取状态,根据状态设置领取按钮的显示
		bool rewardPicked = SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo != null && (SysLocalDataBase.Inst.LocalPlayer.StartServerRewardInfo.UnPickIds.Contains(pickID) == false);
		pickBase.Hide(rewardPicked);
		rewardPickedBase.Hide(!rewardPicked);
	}
}