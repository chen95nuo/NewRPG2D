using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlAdventureDelayReward : UIModule
{
	public SpriteText timeLabel;
	public SpriteText descLabel;

	public UIScrollList packageList;
	public GameObjectPool packagePool;

	public UIScrollList rewardList;
	public GameObjectPool rewardPool;

	public GameObject zeroRoot;
	public GameObject rewardRoot;

	public UIElemAssetIcon backBg;
	public UIButton pickRewardBtn;

	//无奖励时隐藏Root
	public GameObject packageRoot;
	public GameObject alphaRoot;
	public SpriteText noneDelayTips;

	private List<com.kodgames.corgi.protocol.DelayReward> delayRewards;
	private float delta = 0f;

	//默认选中的延时礼包
	private com.kodgames.corgi.protocol.DelayReward delayPackage = null;
	private MarvellousAdventureConfig.DelayRewardSet delayRewardSet = null;

	//更新用禁止重复刷新列表FLAG
	private bool isRefresh = false;

	//可领奖励数
	private int couldGetCount = 0;

	private void Update()
	{
		delta += Time.deltaTime;

		//延时奖励时间更新
		if (delta > 1)
		{
			delta = 0;

			if (delayPackage != null)
			{
				if (delayPackage.couldPickTime > SysLocalDataBase.Inst.LoginInfo.NowTime)
				{
					if (delayRewardSet != null)
						timeLabel.Text = DelayTypeTimeLabel();
					
					//时间到达后刷新奖励FLAG
					isRefresh = true;
				}

				else if (isRefresh)
				{
					//只需要刷新一次
					isRefresh = false;
					RefreshRewardList();
				}
			}

			//每个礼包的时间显示，当有新的礼包到达时间后需要重新进行排序
			int getCount = 0;

			for (int index = 0; index < packageList.Count; index++)
			{
				UIElemAdventurePackageItem item = packageList.GetItem(index).Data as UIElemAdventurePackageItem;				
				item.UpdateDelayPackageItemTime();
				
				if (item.IsTimeOut)
					getCount++;
			}

			if (couldGetCount != getCount)
			{
				couldGetCount = getCount;
				delayPackageSort();
			}
		}
	}

	private string DelayTypeTimeLabel()
	{
		string str = "";

		switch (delayRewardSet.DelayType)
		{
			case MarvellousAdventureConfig._MarvellousDelayRewardType.Practice:
				str = String.Format(GameUtility.GetUIString("UIPnlAdventureDelayReward_Practice_Label"), GameUtility.Time2String(delayPackage.couldPickTime - SysLocalDataBase.Inst.LoginInfo.NowTime));
				break;
			case MarvellousAdventureConfig._MarvellousDelayRewardType.Treasure:
				str = String.Format(GameUtility.GetUIString("UIPnlAdventureDelayReward_Treasure_Label"), GameUtility.Time2String(delayPackage.couldPickTime - SysLocalDataBase.Inst.LoginInfo.NowTime));
				break;
			case MarvellousAdventureConfig._MarvellousDelayRewardType.Study:
				str = String.Format(GameUtility.GetUIString("UIPnlAdventureDelayReward_Study_Label"), GameUtility.Time2String(delayPackage.couldPickTime - SysLocalDataBase.Inst.LoginInfo.NowTime));
				break;
		}

		return str;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;
		if(userDatas.Length > 0)
		{
			delayRewards = userDatas[0] as List<com.kodgames.corgi.protocol.DelayReward>;			
			InitData();
		}			
		else
			RequestMgr.Inst.Request(new MarvellousQueryDelayRewardReq());
		
		return false;
	}

	private void ClearData()
	{
		StopCoroutine("FillPackageList");
		packageList.ClearList(false);
		packageList.ScrollPosition = 0f;
		StopCoroutine("FillRewardList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;
	}

	public void QueryDelayRewardSucess(List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
	{
		this.delayRewards = delayRewards;
		InitData();
	}

	private void InitData()
	{
		zeroRoot.SetActive(false);
		rewardRoot.SetActive(false);
		backBg.Hide(true);
		ClearData();

		delayPackageSort();
		//如果有延时礼包，默认选中左边第一个
		if (delayRewards.Count > 0)
		{					
			hideNoneDealy(true);	
			delayPackage = delayRewards[0];
			RefreshRewardList();
		}
		else
		{
			backBg.SetData(ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.DefaultBg);
			backBg.Hide(false);
			hideNoneDealy(false);
		}
	}

	private void RefreshRewardList()
	{
		if (SysLocalDataBase.Inst.LoginInfo.NowTime > delayPackage.couldPickTime)
		{
			zeroRoot.SetActive(false);
			rewardRoot.SetActive(true);

			StopCoroutine("FillRewardList");
			rewardList.ClearList(false);
			rewardList.ScrollPosition = 0f;
			StartCoroutine("FillRewardList", delayPackage.eventId);
		}
		else
		{
			var reward = (MarvellousAdventureConfig.RewardEvent)ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetEventById(delayPackage.eventId);
			delayRewardSet = ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetDelayRewardSetById(reward.DelayBackBg);			
			backBg.SetData(reward.DelayBackBg);
			backBg.Hide(false);
			zeroRoot.SetActive(true);
			rewardRoot.SetActive(false);
			timeLabel.Text = DelayTypeTimeLabel();
			descLabel.Text = delayRewardSet.DelayDesc;
		}

		SetLight();
	}

	private void hideNoneDealy(bool hide)
	{
		alphaRoot.SetActive(hide);
		if (!hide)
			noneDelayTips.Text = GameUtility.GetUIString("UIPnlAdventureDelayReward_NoneReward_Tips");
		else
			noneDelayTips.Text = "";
	}

	private void delayPackageSort()
	{
		delayRewards.Sort((a1, a2) =>
		{
			//是否可领取排序
			int couldPick1 = a1.couldPickTime < SysLocalDataBase.Inst.LoginInfo.NowTime ? 1 : 0;
			int couldPick2 = a2.couldPickTime < SysLocalDataBase.Inst.LoginInfo.NowTime ? 1 : 0;

			if (couldPick1 != couldPick2)
				return couldPick2 - couldPick1;
			//获得时间排序
			return (int)(a1.gainRewardTime - a2.gainRewardTime);
		});

		StopCoroutine("FillPackageList");
		packageList.ClearList(false);
		packageList.ScrollPosition = 0f;
		StartCoroutine("FillPackageList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillPackageList()
	{
		yield return null;

		// Fill Data.
		for (int i = 0; i < delayRewards.Count; i++)
		{
			UIListItemContainer uiContainer = packagePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemAdventurePackageItem item = uiContainer.GetComponent<UIElemAdventurePackageItem>();
			uiContainer.data = item;
			item.SetData(delayRewards[i]);			
			packageList.AddItem(item.gameObject);
		}

		SetLight();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList(int eventId)
	{
		yield return null;

		var reward = (MarvellousAdventureConfig.RewardEvent)ConfigDatabase.DefaultCfg.MarvellousAdventureConfig.GetEventById(eventId);
		timeLabel.Text = GameUtility.GetUIString("UIPnlAdventureDelayReward_Reward_Tips");
		backBg.SetData(reward.DelayBackBg);
		backBg.Hide(false);
		// Fill Data.
		for (int i = 0; i < reward.DelayRewards.Count; i++)
		{
			var item = rewardPool.AllocateItem(false).GetComponent<UIElemAdventureDelayRewardProcessor>();
			item.SetData(reward.DelayRewards[i]);
			rewardList.AddItem(item);
		}
	}

	public void SetLight()
	{
		for (int i = 0; i < packageList.Count; i++)
		{
			UIElemAdventurePackageItem item = packageList.GetItem(i).Data as UIElemAdventurePackageItem;
			if (item.delayPackage == null || delayPackage == null)
				continue;

			if (item.delayPackage.gainRewardTime == delayPackage.gainRewardTime)
			{
				item.SetSelectedStat(true);
			}
			else
				item.SetSelectedStat(false);
		}
	}

	public void OnQueryPickDelayRewardSuccess(List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
	{
		this.delayRewards = delayRewards;
		couldGetCount--;
		InitData();
	}

	// Package Btn
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPackageBtnClick(UIButton btn)
	{
		UIElemAssetIcon itemIcon = btn.Data as UIElemAssetIcon;
		com.kodgames.corgi.protocol.DelayReward delayReward = itemIcon.Data as com.kodgames.corgi.protocol.DelayReward;
		delayPackage = delayReward;
		RefreshRewardList();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackBtnClick(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAdventureMain), delayRewards);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardBtnClick(UIButton btn)
	{
		UIElemAssetIcon itemIcon = btn.Data as UIElemAssetIcon;		
		GameUtility.ShowAssetInfoUI((int)itemIcon.Data, _UILayer.Top);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPickBtnClick(UIButton btn)
	{
		if (delayPackage != null)
			RequestMgr.Inst.Request(new MarvellousPickDelayRewardReq(delayPackage.eventId, delayPackage.gainRewardTime));
	}
}