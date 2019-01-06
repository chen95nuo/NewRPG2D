using UnityEngine;
using System.Collections;
using ClientServerCommon;
public class UIElemAlchemyBox : UIListItemContainer
{
	public UIElemAssetIcon rewardBox;
	public AutoSpriteControlBase isActivityBg;
	public AutoSpriteControlBase alreadyGet;
	public SpriteText timeTipsLabel;
	public SpriteText countLabel;

	private bool isShow = false;
	public bool IsShow { get { return isShow; } }

	private int rewardIndex = 0;

	/** 箭头显示逻辑，将hide掉得放到未显示列表里，
	 * 将刚出现的作为showindex,如果未显示列表中存在比showindex小的显示左箭头，大的显示右箭头*/
	public override void OnEnable()
	{
		if (Application.isPlaying)
		{
			isShow = true;
			if(SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnaceActivity)))
			{
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnaceActivity>().rewardIndexs.Remove(rewardIndex);
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnaceActivity>().ShowIndex = rewardIndex;			
			}
			else
			{
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnace>().rewardIndexs.Remove(rewardIndex);
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnace>().ShowIndex = rewardIndex;			
			}
		}
	
		base.OnEnable();
	}

	public override void OnDisabled()
	{
		if (Application.isPlaying)
		{
			isShow = false;
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnaceActivity)))
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnaceActivity>().rewardIndexs.Add(rewardIndex);		
			else
				SysUIEnv.Instance.GetUIModule<UIPnlDanFurnace>().rewardIndexs.Add(rewardIndex);			
		}
	
		base.OnDisabled();
	}

	public void SetData(com.kodgames.corgi.protocol.BoxReward boxReward, int index)
	{
		this.rewardIndex = index;
		
		if(!boxReward.hasPicked)
		{
			rewardBox.SetData(boxReward.iconId);
			alreadyGet.Hide(true);
		}
		else 
		{
			rewardBox.SetData(boxReward.openIconId);
			alreadyGet.Hide(false);
		}

		rewardBox.border.Data = boxReward;

		//是否可领取
		int todayCount = 0;
		if(SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanFurnaceActivity)))
			todayCount = SysUIEnv.Instance.GetUIModule<UIPnlDanFurnaceActivity>().TodayAlchemyCount;
		else
			todayCount = SysUIEnv.Instance.GetUIModule<UIPnlDanFurnace>().TodayAlchemyCount;

		countLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_PickNeedTimes"), boxReward.alchemyCount);

		if (boxReward.alchemyCount <= todayCount && !boxReward.hasPicked)
			timeTipsLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_CouldGet"), GameDefines.textColorGreen);
		else if (boxReward.hasPicked)
			timeTipsLabel.Text = "";
		else
			timeTipsLabel.Text = string.Format(GameUtility.GetUIString("UIPnlDanFurnace_leftCountGet"), GameDefines.textColorWhite, todayCount, boxReward.alchemyCount);				
	}
}
