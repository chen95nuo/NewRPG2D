using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAdventureMain : UIModule
{
	public SpriteText energyLabel;
	public SpriteText getRewardCount;
	public SpriteText haveRewardCount;
	public UIBox delaySign;
	
	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;
	private int getCount = 0;
	private float delta = 0f;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		return true;
	}

	private void Update()
	{
		delta += Time.deltaTime;

		if (delta > 1)
		{
			delta = 0f;

			UpdateTimerTextView();

			//当有可领奖励时显示绿点
			if (getCount > 0)
				delaySign.Hide(false);
			else
				delaySign.Hide(true);

			//延时奖励刷新
			if (delayRewards != null && delayRewards.Count > 0 && getCount < delayRewards.Count)
			{
				if (SysLocalDataBase.Inst.LoginInfo.NowTime > delayRewards[getCount].couldPickTime)
				{				
					getCount++;
					getRewardCount.Text = string.Format(GameUtility.GetUIString("UIPnlAdventureMain_GetReward_Count"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, getCount);
				}
			}
		}
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;

		RequestMgr.Inst.Request(new MarvellousQueryDelayRewardReq());

		return false;
	}

	private void UpdateTimerTextView()
	{
		long addEnergyTime = SysLocalDataBase.Inst.LocalPlayer.Energy.GetNextGenerationLeftTime(SysLocalDataBase.Inst.LoginInfo.NowTime);
		if (addEnergyTime > 0)
		{
			energyLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAdventureMain_Energy_TimeLabel"), GameDefines.textColorBtnYellow,
				GameDefines.textColorWhite, GameUtility.Time2String(addEnergyTime));
		}
		else energyLabel.Text = string.Format(GameUtility.GetUIString("UIPnlAdventureMain_Energy_MaxLabel"), GameDefines.textColorBtnYellow);
	}

	public void OnQueryDelayRewardSuccess(List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
	{		
		UpdateTimerTextView();
		//按照可领时间排序,提高更新效率
		
		delayRewards.Sort((a1, a2) =>
		{
			int time = 0;
			if (a2.couldPickTime > a1.couldPickTime)
				time = -1;
			else if(a2.couldPickTime < a1.couldPickTime)
				time = 1;

			return time;
		});

		this.delayRewards = delayRewards;
		getCount = 0;
		//延时奖励数量
		haveRewardCount.Text = string.Format(GameUtility.GetUIString("UIPnlAdventureMain_DelayReward_Count"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, delayRewards.Count);

		for (int i = 0; i < delayRewards.Count; i++)
		{
			if (SysLocalDataBase.Inst.LoginInfo.NowTime > delayRewards[i].couldPickTime)
			{			
				getCount++;
			}				
		}
		//当有可领奖励时显示绿点
		if (getCount > 0)
			delaySign.Hide(false);
		else
			delaySign.Hide(true);

		//可领奖励数量
		getRewardCount.Text = string.Format(GameUtility.GetUIString("UIPnlAdventureMain_GetReward_Count"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, getCount);
	}

	//奇遇按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAdventureBtnClick(UIButton btn)
	{		
		RequestMgr.Inst.Request(new MarvellousQueryReq());
	}

	//延时奖励按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnDelayRewardClick(UIButton btn)
	{
		if (delayRewards.Count > 0)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAdventureDelayReward), delayRewards);
			HideSelf();
		}
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAdventureMain_Not_GetReward_Tips"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackBtnClick(UIButton btn)
	{
		HideSelf();
	}
}