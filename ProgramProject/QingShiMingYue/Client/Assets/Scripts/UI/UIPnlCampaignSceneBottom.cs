using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIPnlCampaignSceneBottom : UIModule
{
	public SpriteText costLabel;
	public SpriteText dungeonExpLabel;
	public SpriteText dungeonGoldLabel;
	public SpriteText dungeonEnterTimesLabel;
	public SpriteText dungeonEnterLevelLabel;
	public UIScrollList specialList;
	public GameObjectPool specialRewardPool;
	public GameObjectPool possibilityTextPool;//美术字："几率获得"
	public GameObjectPool firstPassRewardReceivedPool;//美术字"首通奖励"

	public UIButton dungeonGuidBtn;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		RefreshViews((int)userDatas[0]);
		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		specialList.ClearList(false);
		specialList.ScrollPosition = 0f;
	}

	public void RefreshViews(int dungeonId)
	{
		CampaignConfig.Dungeon dungeonConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);

		// Set Dungeon Guid Data.
		dungeonGuidBtn.Hide(!dungeonConfig.hasGuide);
		dungeonGuidBtn.Data = dungeonConfig;

		// Set dungeon enter cost label.
		if (dungeonConfig.enterCosts.Count <= 0)
			costLabel.Text = string.Empty;
		else
			costLabel.Text = GameUtility.FormatUIString(
								"UIDungeon_Map_EnterCost",
								GameDefines.textColorBtnYellow.ToString(),
								ItemInfoUtility.GetAssetName(dungeonConfig.enterCosts[0].id),
								GameDefines.txColorWhite.ToString(),
								dungeonConfig.enterCosts[0].count);

		// Set dungeon enter advice level.
		dungeonEnterLevelLabel.Text = GameUtility.FormatUIString(
										"UIDungeon_Map_EnterAdviceLevel",
										GameDefines.textColorBtnYellow.ToString(),
										GameDefines.txColorWhite.ToString(),
										dungeonConfig.levelLimit);

		// Set dungeon enter times and best Record.
		int todayEnterTimes = 0;
		KodGames.ClientClass.Dungeon tempDungeon = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonConfig.ZoneId, dungeonConfig.dungeonId);
		if (tempDungeon != null)
			todayEnterTimes = tempDungeon.TodayCompleteTimes;

		dungeonEnterTimesLabel.Text = GameUtility.FormatUIString(
										"UIDungeon_Map_EnterTimes",
										GameDefines.textColorBtnYellow.ToString(),
										GameDefines.txColorWhite.ToString(),
										todayEnterTimes,
										dungeonConfig.enterCount);


		// Set dungeon reward desc: Exp and Gold and so on.
		int gameMoneyRewardCount = 0;
		int expRewardCount = 0;

		if (dungeonConfig.fixedRewards.Count <= 0)
			Debug.LogError("CampaignConfig DungeonId " + dungeonConfig.dungeonId + " has not set FixedReward.");
		else
		{
			foreach (var reward in dungeonConfig.fixedRewards)
			{
				switch (reward.id)
				{
					case IDSeg._SpecialId.GameMoney:
						gameMoneyRewardCount = reward.count;
						break;
					case IDSeg._SpecialId.Experience:
						expRewardCount = reward.count;
						break;
				}
			}
		}

		dungeonGoldLabel.Text = GameUtility.FormatUIString(
									"UIDungeon_Map_GameMoney",
									GameDefines.textColorBtnYellow.ToString(),
									GameDefines.txColorWhite.ToString(),
									gameMoneyRewardCount.ToString());

		dungeonExpLabel.Text = GameUtility.FormatUIString(
									"UIDungeon_Map_Exp",
									GameDefines.textColorBtnYellow.ToString(),
									GameDefines.txColorWhite.ToString(),
									expRewardCount.ToString());

		ClearData();
		StartCoroutine("FillList", dungeonConfig);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(CampaignConfig.Dungeon dungeonCfg)
	{
		yield return null;

		var tempDungeon = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);



		//有星星，该关卡已通过，则肯定领取了首通奖励，两段逻辑的区别仅是填充顺序的不同。。。
		if (tempDungeon != null && tempDungeon.BestRecord > 0)
		{
			if (dungeonCfg.displayRewards != null && dungeonCfg.displayRewards.Count > 0)
			{
				//填充“概率获得”
				specialList.AddItem(possibilityTextPool.AllocateItem());

				//填充概率获得物品
				foreach (var reward in dungeonCfg.displayRewards)
				{
					UIElemDungeonRewardItem item = specialRewardPool.AllocateItem().GetComponent<UIElemDungeonRewardItem>();
					item.SetData(reward.id, reward.count, false);

					specialList.AddItem(item.gameObject);
				}
			}

			if (dungeonCfg.firstPassRewards != null && dungeonCfg.firstPassRewards.Count > 0)
			{
				//填充“首通奖励”
				specialList.AddItem(firstPassRewardReceivedPool.AllocateItem());

				//填充首通奖励物品("已领")
				foreach (var firstPassReward in dungeonCfg.firstPassRewards)
				{
					UIElemDungeonRewardItem item = specialRewardPool.AllocateItem().GetComponent<UIElemDungeonRewardItem>();
					item.SetData(firstPassReward.id, firstPassReward.count, true);

					specialList.AddItem(item.gameObject);
				}
			}
		}
		else
		{
			if (dungeonCfg.firstPassRewards != null && dungeonCfg.firstPassRewards.Count > 0)
			{
				//填充“首通奖励”
				specialList.AddItem(firstPassRewardReceivedPool.AllocateItem());

				//填充首通奖励物品
				foreach (var firstPassReward in dungeonCfg.firstPassRewards)
				{
					UIElemDungeonRewardItem item = specialRewardPool.AllocateItem().GetComponent<UIElemDungeonRewardItem>();
					item.SetData(firstPassReward.id, firstPassReward.count, false);

					specialList.AddItem(item.gameObject);
				}
			}

			if (dungeonCfg.displayRewards != null && dungeonCfg.displayRewards.Count > 0)
			{
				//填充“概率获得”
				specialList.AddItem(possibilityTextPool.AllocateItem());

				//填充概率获得物品
				foreach (var reward in dungeonCfg.displayRewards)
				{
					UIElemDungeonRewardItem item = specialRewardPool.AllocateItem().GetComponent<UIElemDungeonRewardItem>();
					item.SetData(reward.id, reward.count, false);

					specialList.AddItem(item.gameObject);
				}
			}
		}
	}


	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardIcon(UIButton btn)
	{
		//从Button组件获取物品ID
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI((int)assetIcon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDungeonGuid(UIButton btn)
	{
		var dungeonCfg = btn.Data as CampaignConfig.Dungeon;

		if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonGuidNpcsByDungeonId(dungeonCfg.dungeonId) == null)
			RequestMgr.Inst.Request(new QueryDungeonGuideReq(dungeonCfg.dungeonId));
		else
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAvatarLineUpGuide), dungeonCfg.dungeonId);
	}
}
