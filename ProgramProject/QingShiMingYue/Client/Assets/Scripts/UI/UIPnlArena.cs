using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlArena : UIModule
{
	//Tab btns.
	public UIButton challengeTabBtn;//挑战界面
	public UIButton rewardTabBtn;//领奖界面
	public UIButton rankTabBtn;//排名界面

	//Challenge Tab content.
	public GameObject challengeInfoRoot;
	public SpriteText rankLabel;//玩家排名
	public SpriteText remainningTimeLabel;//剩余挑战次数
	public SpriteText arenaLevelLabel;//比武场等级
	public SpriteText challengeTitleLabel;//挑战说明
	public UIScrollList challengeList;//可挑战玩家列表
	public GameObjectPool challengeObjectPool;//其他玩家
	public GameObjectPool myChallengeObjPool;//玩家自身

	//Reward Tab content.
	public SpriteText badgeNumLabel;//武魂数量
	public SpriteText scoreLabel;//积分数量
	public GameObject rewardInfoRoot;
	public UIScrollList rewardList;//奖励列表
	public GameObjectPool rewardObjectPool;

	//Rank Tab content.
	public UIScrollList rankList;//排名列表
	public GameObjectPool RankObjectPool;

	private float lastUpdateTime = 0;
	private long lasetResetTime;

	// Arena data
	private ArenaShowData arenaShowData = new ArenaShowData();
	public class ArenaShowData
	{
		public List<com.kodgames.corgi.protocol.PlayerRecord> challengeRecords = new List<com.kodgames.corgi.protocol.PlayerRecord>();
		public int myGradeId;
		public int mySpeed;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		TabToChallenge(true);
		return true;
	}

	private void Update()
	{
		if (Time.realtimeSinceStartup - lastUpdateTime < 1)
			return;

		var sysLocalDataBase = SysLocalDataBase.Inst;

		// Add PVP stamina item
		System.DateTime nextTime = KodGames.TimeEx.GetTimeAfterTime(
			KodGames.TimeEx.ToUTCDateTime(ConfigDatabase.DefaultCfg.ArenaConfig.restoreArenaChallengeTime),
			sysLocalDataBase.LoginInfo.ToServerDateTime(lasetResetTime),
			_TimeDurationType.Day);

		if (nextTime <= sysLocalDataBase.LoginInfo.NowDateTime)
		{
			lasetResetTime = KodGames.TimeEx.DateTimeToInt64(sysLocalDataBase.LoginInfo.NowDateTime);

			lasetResetTime = 0;
			if (sysLocalDataBase.LocalPlayer.ArenaData != null)
				sysLocalDataBase.LocalPlayer.ArenaData.ChallengePoint = Mathf.Min(0, sysLocalDataBase.LocalPlayer.ArenaData.ChallengePoint);
		}

		// Reset UI
		UpdateChallengeRemainningTimesLabel();

		//refresh lastTime
		UpdateCalulateCountDown();
	}

	private void ClearList()
	{
		// Clear challengeItem tab list.
		StopCoroutine("FillChallengeTabList");
		challengeList.ClearList(false);
		challengeList.ScrollListTo(0f);

		// Clear reward tab list.
		StopCoroutine("FillRewardTabList");
		rewardList.ClearList(false);
		rewardList.ScrollListTo(0f);

		//Clear rank tab list.
		StopCoroutine("FillRewardTabList");
		rankList.ClearList(false);
		rankList.ScrollListTo(0f);
	}

	private static int CompareTo(com.kodgames.corgi.protocol.PlayerRecord r1, com.kodgames.corgi.protocol.PlayerRecord r2)
	{
		if (r1 == null || r2 == null)
			return 0;
		return (r1.rank).CompareTo(r2.rank);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChallengeTabClick(UIButton btn)
	{
		TabToChallenge(false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardTabClick(UIButton btn)
	{
		TabToReward();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRankTabClick(UIButton btn)
	{
		TabToRank();
	}

	#region Challenge
	private void TabToChallenge(bool queryData)
	{
		ClearList();

		//Hide reward tab content.
		rewardInfoRoot.SetActive(false);
		rewardList.gameObject.SetActive(false);

		//Hide challengeItem tab content, the content will not be active before querying completed.
		challengeInfoRoot.SetActive(false);
		challengeList.gameObject.SetActive(false);

		//Hide RankItem.
		rankList.gameObject.SetActive(false);

		//Disable challengeItem tab button.
		challengeTabBtn.controlIsEnabled = false;

		//Enable reward tab button.
		rewardTabBtn.controlIsEnabled = true;

		//Enable Rank tab button.
		rankTabBtn.controlIsEnabled = true;

		if (queryData)
			RequestMgr.Inst.Request(new QueryArenaRankReq());
		else
			FillChallengeUI();
	}

	public void OnQueryArenaRankSuccess(ArenaShowData showData, long lastResetChallengeTime)
	{
		arenaShowData = showData;
		this.lasetResetTime = lastResetChallengeTime;
		challengeList.ClearList(false);
		FillChallengeUI();
	}

	public void OnAreanCombatSuccessCaseNotAvatar(string message)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), message);

		TabToChallenge(true);
	}

	public void OnArenaCombatSuccess(KodGames.ClientClass.CombatResultAndReward combatResultAndReward, int selfRank)
	{
		UIPnlArenaBattleResult.ArenaBattleResultData battleData = new UIPnlArenaBattleResult.ArenaBattleResultData(combatResultAndReward, selfRank);

		List<object> paramsters = new List<object>();
		paramsters.Add(combatResultAndReward);
		paramsters.Add(battleData);

		SysGameStateMachine.Instance.EnterState<GameState_Battle>(paramsters);
	}

	public void OnQuerySelfArenaHonorPointSuccess()
	{
		KodGames.ClientClass.ArenaData arenaData = SysLocalDataBase.Inst.LocalPlayer.ArenaData;

		if (challengeInfoRoot.activeSelf)
		{
			// Find self item in list and update UI
			for (int i = 0; i < challengeList.Count; ++i)
			{
				UIElemArenaMyRecordItem item = challengeList.GetItem(i).Data as UIElemArenaMyRecordItem;
				if (item != null)
				{
					item.SetSelfArenaHonorPoint(arenaData.HonorPoint);
					break;
				}
			}
		}
	}

	private void UpdateChallengeRemainningTimesLabel()
	{
		if (challengeInfoRoot.activeSelf == false)
			return;

		//今日剩余次数
		int curLeftTime = Mathf.Max(0, ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.DailyArenaCombatCount) - SysLocalDataBase.Inst.LocalPlayer.ArenaData.ChallengePoint);
		// My remaining challengeItem times.
		if (remainningTimeLabel.Data == null || (int)remainningTimeLabel.Data != curLeftTime)
		{
			remainningTimeLabel.Data = curLeftTime;
			remainningTimeLabel.Text = GameUtility.FormatUIString("UIPnlArena_ChallengeLabel_TimeNumber", (int)remainningTimeLabel.Data);
		}
	}

	private void UpdateCalulateCountDown()
	{
		for (int index = 0; index < rewardList.Count; index++)
		{
			UIElemArenaAwardItem item = rewardList.GetItem(index).Data as UIElemArenaAwardItem;
			item.UpdateRefreshTime();
		}
	}

	private void FillChallengeUI()
	{
		//显示玩家排名等信息 
		var arenaData = SysLocalDataBase.Inst.LocalPlayer.ArenaData;

		// Active challengeItem informations.
		challengeInfoRoot.SetActive(true);
		challengeList.gameObject.SetActive(true);
		arenaLevelLabel.Text = ConfigDatabase.DefaultCfg.ArenaConfig.GetDescByGradeID(arenaShowData.myGradeId);
		// My rank label.
		rankLabel.Text = arenaData.SelfRank.ToString();
		UpdateChallengeRemainningTimesLabel();

		// Create my rank record.
		com.kodgames.corgi.protocol.PlayerRecord myRecord = new com.kodgames.corgi.protocol.PlayerRecord();
		myRecord.playerId = SysLocalDataBase.Inst.LocalPlayer.PlayerId;
		myRecord.rank = arenaData.SelfRank;
		myRecord.playerName = SysLocalDataBase.Inst.LocalPlayer.Name;
		myRecord.playerLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
		myRecord.speed = arenaShowData.mySpeed;

		// Fill record data list.
		List<com.kodgames.corgi.protocol.PlayerRecord> records = new List<com.kodgames.corgi.protocol.PlayerRecord>();
		records.Add(myRecord);
		records.AddRange(arenaShowData.challengeRecords);

		// Order by rank level.
		records.Sort(CompareTo);

		// Fill challengeItem player list with player-type map.
		StartCoroutine("FillChallengeTabList", records);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillChallengeTabList(List<com.kodgames.corgi.protocol.PlayerRecord> records)
	{
		yield return null;

		//My player kvp list item.
		UIListItemContainer myPlayerItem = null;

		//Fill player kvp list control.
		foreach (var record in records)
		{
			if (record.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
			{
				UIListItemContainer item = myChallengeObjPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemArenaMyRecordItem myRecordItem = item.gameObject.GetComponent<UIElemArenaMyRecordItem>();
				item.data = myRecordItem;

				myRecordItem.SetData(record, SysLocalDataBase.Inst.LocalPlayer.ArenaData.HonorPoint, arenaShowData.myGradeId);
				myPlayerItem = item;
				challengeList.AddItem(item);
			}
			else
			{
				MainMenuItem menu = new MainMenuItem();
				menu.ScriptMethodToInvoke = this;

				UIListItemContainer item = challengeObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemArenaChallengeItem challengeItem = item.gameObject.GetComponent<UIElemArenaChallengeItem>();

				item.Data = challengeItem;
				menu.MethodToInvoke = "OnChallengeClick";
				menu.ControlText = GameUtility.GetUIString("UIElemArenaChallengeItem_Challenge");
				challengeItem.SetData(record, menu);

				challengeList.AddItem(item);
			}
		}

		if (myPlayerItem != null)
		{
			int myPlayerIndex = 0;
			foreach (var record in records)
			{
				myPlayerIndex++;
				if (record.playerId == SysLocalDataBase.Inst.LocalPlayer.PlayerId)
					break;
			}


			if (myPlayerIndex > 3)
				challengeList.ScrollToItem(challengeList.GetItem(myPlayerIndex - 1), 0f, EZAnimation.EASING_TYPE.Linear, -(myPlayerItem.TopLeftEdge.y - myPlayerItem.BottomRightEdge.y) * 0.5f);
			else
				challengeList.ScrollToItem(0, 0f);
		}
		else
			challengeList.ScrollListTo(0f);
	}

	//Challenge tab event handlers.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnViewLineupClick(UIButton btn)
	{
		com.kodgames.corgi.protocol.PlayerRecord record = btn.data as com.kodgames.corgi.protocol.PlayerRecord;

		RequestMgr.Inst.Request(new QueryArenaPlayerInfoReq(record.rank, arenaShowData.myGradeId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChallengeClick(UIButton btn)
	{
		com.kodgames.corgi.protocol.PlayerRecord record = btn.data as com.kodgames.corgi.protocol.PlayerRecord;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBeforeBattleLineUp), _CombatType.Arena, record.rank);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRefreshSelfArenaPointClick(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryArenaRankReq());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnGetRulesClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgArenaRules));
	}

	public void OnUpdateMyGradePointSuccess()
	{
		for (int index = 0; index < challengeList.Count; index++)
		{
			UIElemArenaMyRecordItem playerItem = challengeList.GetItem(index).Data as UIElemArenaMyRecordItem;
			if (playerItem != null)
			{
				playerItem.SetSelfArenaHonorPoint(SysLocalDataBase.Inst.LocalPlayer.ArenaData.HonorPoint);
				break;
			}
		}
	}

	#endregion

	#region Reward panel
	private void TabToReward()
	{
		//if (GameUtility.CheckGoodsList())
		//    InitRewardUI();
		//else
		RequestMgr.Inst.Request(new QueryGoodsReq(InitRewardUI));
	}

	public void InitRewardUI()
	{
		ClearList();

		//Hide reward tab content.
		challengeInfoRoot.SetActive(false);
		challengeList.gameObject.SetActive(false);

		//Hide rank tab content.
		rankList.gameObject.SetActive(false);

		//Show challengeItem tab content.
		rewardInfoRoot.SetActive(true);
		rewardList.gameObject.SetActive(true);

		//Enable challengeItem tab button.
		challengeTabBtn.controlIsEnabled = true;

		//Disable reward tab button.
		rewardTabBtn.controlIsEnabled = false;

		//Enable Rank tab button.
		rankTabBtn.controlIsEnabled = true;

		// Update UI
		UpdateRewardInfoCtrls();

		// Query arena goods list when no goods data at local.
		StartCoroutine("FillRewardTabList");
	}

	private void UpdateRewardInfoCtrls()
	{
		if (!rewardInfoRoot.activeInHierarchy)
			return;

		KodGames.ClientClass.ArenaData arenaData = SysLocalDataBase.Inst.LocalPlayer.ArenaData;

		if (!badgeNumLabel.Text.Equals(SysLocalDataBase.Inst.LocalPlayer.Badge.ToString()))
			badgeNumLabel.Text = SysLocalDataBase.Inst.LocalPlayer.Badge.ToString();

		if (!scoreLabel.Text.Equals(arenaData.HonorPoint.ToString()))
			scoreLabel.Text = arenaData.HonorPoint.ToString();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardTabList()
	{
		yield return null;

		List<GoodConfig.Good> shopGoods = new List<GoodConfig.Good>();

		foreach (var goods in SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods)
		{
			//if (goods.Status == _GoodsStatusType.Closed)
			//    continue;

			GoodConfig.Good goodCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goods.GoodsID);
			//if (goodCfg == null)
			//    continue;

			//// Only accept arena goods
			//if (goodCfg.goodsType != _Goods.ArenaGoods)
			//    continue;

			if (goodCfg != null && goods.Status != _GoodsStatusType.Closed && goodCfg.goodsType == _Goods.ArenaGoods)
				shopGoods.Add(goodCfg);
		}

		shopGoods.Sort((t1, t2) =>
		{
			int d1 = t1.goodsIndex;
			int d2 = t2.goodsIndex;
			return d2 - d1;
		});

		for (int index = 0; index < shopGoods.Count; index++)
		{
			for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods.Count; i++)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods[i].GoodsID == shopGoods[index].id)
				{
					UIListItemContainer item = rewardObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemArenaAwardItem awardItem = item.gameObject.GetComponent<UIElemArenaAwardItem>();
					item.Data = awardItem;
					awardItem.SetData(SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods[i]);
					rewardList.AddItem(item);
				}
			}
		}
	}

	// Reward tab event handlers.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnArenaGoodBuyClick(UIButton btn)
	{
		GoodConfig.Good goodCfg = btn.data as GoodConfig.Good;
		var goodsData = SysLocalDataBase.Inst.LocalPlayer.ShopData.GetGoodsById(goodCfg.id);
		if (goodsData != null)
			RequestMgr.Inst.Request(new BuyGoodsReq(goodCfg.id, 1, goodsData.StatusIndex, OnArenaGoodBuySuccess, null));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnArenaGoodInfoClick(UIButton btn)
	{
		KodGames.ClientClass.Goods good = btn.Data as KodGames.ClientClass.Goods;

		GoodConfig.Good goodsConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(good.GoodsID);

		if (goodsConfig.assetIconId != IDSeg.InvalidId)
			GameUtility.ShowAssetInfoUI(goodsConfig.assetIconId);
		else
		{
			if (goodsConfig.rewards.Count == 1)
			{
				// 如果只包含一个物品, 显示这个物品的信息
				if (GameUtility.ShowAssetInfoUI(goodsConfig.rewards[0].id) == false)
					// False 表示不支持该id物品的显示, 直接显示商品描述
					SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, good.GoodsID);
			}
			else
			{
				// 有多个物品, 显示商品信息
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgConsumableInfo, good.GoodsID);
			}
		}
	}

	private void OnArenaGoodBuySuccess(int goodsId, int amount, KodGames.ClientClass.Reward reward, List<KodGames.ClientClass.Cost> costs)
	{
		UpdateRewardInfoCtrls();

		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlArena_Msg_ExchangeSuccess_Success", SysLocalDataBase.GetRewardDesc(reward, true, false, false)));

		for (int i = 0; i < rewardList.Count; i++)
		{
			UIElemArenaAwardItem item = rewardList.GetItem(i).Data as UIElemArenaAwardItem;
			if (item != null)
				item.UpdateData();
		}
	}

	#endregion

	#region Rank panel
	private void TabToRank()
	{
		RequestMgr.Inst.Request(new QueryRankToFewReq());
	}

	public void QueryRankToFewResSuccess(List<com.kodgames.corgi.protocol.PlayerRecord> topFew)
	{
		InitRankUI(topFew);
	}

	public void InitRankUI(List<com.kodgames.corgi.protocol.PlayerRecord> topFew)
	{
		ClearList();

		//Hide reward tab content.
		challengeList.gameObject.SetActive(false);

		//Hide challengeItem tab content.
		rewardInfoRoot.SetActive(false);
		rewardList.gameObject.SetActive(false);

		//Show rank tab content.
		challengeInfoRoot.SetActive(true);
		rankList.gameObject.SetActive(true);

		//Enable challengeItem tab button.
		challengeTabBtn.controlIsEnabled = true;

		//Enable reward tab button.
		rewardTabBtn.controlIsEnabled = true;

		//Disable Rank tab button.
		rankTabBtn.controlIsEnabled = false;

		// Query arena rank list when no goods data at local.
		StartCoroutine("FillRankTabList", topFew);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRankTabList(List<com.kodgames.corgi.protocol.PlayerRecord> topFew)
	{
		yield return null;

		foreach (var record in topFew)
		{
			MainMenuItem menu = new MainMenuItem();
			menu.ScriptMethodToInvoke = this;
			UIListItemContainer item = RankObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemArenaChallengeItem challengeItem = item.gameObject.GetComponent<UIElemArenaChallengeItem>();

			item.Data = challengeItem;
			menu.MethodToInvoke = "OnViewLineupClick";
			menu.ControlText = GameUtility.GetUIString("UIElemArenaChallengeItem_ViewInfo");
			challengeItem.SetData(record, menu);
			rankList.AddItem(item);
		}
	}
	#endregion
}
