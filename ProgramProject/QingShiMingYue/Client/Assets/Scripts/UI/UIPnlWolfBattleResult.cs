using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlWolfBattleResult : UIPnlBattleResultBase
{
	public SpriteText starDescLabel;
	public SpriteText firstAppraiseThreeLabel;
	public AutoSpriteControlBase winRewardBg;
	public UIScrollList rewardList;
	public GameObjectPool rewardPool;

	public SpriteText faildDescLabel;

	//结算面板
	//胜利
	public SpriteText rewardLabel1;
	public SpriteText rewardCount1;
	public SpriteText rewardLabel2;
	public SpriteText rewardCount2;
	public UIElemAssetIcon rewardIcon1;
	public UIElemAssetIcon rewardIcon2;
	//失败
	public SpriteText faildLabel1;
	public SpriteText faildLabel2;

	public UIButton resetBtn;

	//首次通关奖励，界面排版不同-----------------------------------------------------------------------
	//一般奖励列表，首通奖励列表
	public GameObject firstPass_RewardInfoPnl, firstPass_FirstPassRewardInfoPnl;
	public UIScrollList firstPass_RewardList, firstPass_FirstPassRewardList;
	public GameObjectPool firstPass_rewardPool, firstPass_FirstPassRewardPool;

	public Transform btnRootFirstPassMarker, goldLabelFirstPassMarker, expLabelFirstPassMarker, goldNormalPassMarker, expNormalPassMarker;
	public Transform btnRoot, goldLabelRoot, expLabelRoot;
	public GameObject expLabelObj;

	public class WolfBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;

		private List<KodGames.ClientClass.WolfEggs> wolfEggs;
		public List<KodGames.ClientClass.WolfEggs> WolfEggs { get { return wolfEggs; } }

		private KodGames.ClientClass.CostAndRewardAndSync eggsCostAndRewardAndSync;
		public KodGames.ClientClass.CostAndRewardAndSync EggsCostAndRewardAndSync { get { return eggsCostAndRewardAndSync; } }

		private com.kodgames.corgi.protocol.Avatar showAvatar;
		public com.kodgames.corgi.protocol.Avatar ShowAvatar { get { return showAvatar; } }

		private int alreadyFailedTimes;
		private int alreadyResetTimes;
		public int AlreadyResetTimes { get { return alreadyResetTimes; } }

		private int additionId;
		public int AdditionId { get { return additionId; } }

		private int stageId;
		public int StageId { get { return stageId; } }

		public WolfBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData, List<KodGames.ClientClass.WolfEggs> wolfEggs, KodGames.ClientClass.CostAndRewardAndSync eggsCostAndRewardAndSync, com.kodgames.corgi.protocol.Avatar showAvatar, int alreadyFailedTimes, int alreadyResetTimes, int additionId, int stageId)
			: base(_UIType.UIPnlWolfBattleResult)
		{
			this.battleData = battleData;
			this.wolfEggs = wolfEggs;
			this.eggsCostAndRewardAndSync = eggsCostAndRewardAndSync;
			this.showAvatar = showAvatar;
			this.alreadyFailedTimes = alreadyFailedTimes;
			this.alreadyResetTimes = alreadyResetTimes;
			this.CombatType = _CombatType.WolfSmoke;
			this.additionId = additionId;
			this.stageId = stageId;
		}

		public override bool CanShowView()
		{
			return true;
		}

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		public override bool IsWinner()
		{
			if (battleData != null)
				return battleData.BattleRecords[battleData.BattleRecords.Count - 1].TeamRecords[0].IsWinner;

			return false;
		}

		public override int GetAppraiseNumber()
		{
			return 0;
		}

		public override string GetGoldRewardOrOtherStr()
		{
			int goldRewardOrOtherCount = 0;

			string message = string.Empty;

			bool isWin = false;
			if (this.battleData != null)
				isWin = battleData.BattleRecords[battleData.BattleRecords.Count - 1].TeamRecords[0].IsWinner;
			if (isWin)
			{
				foreach (KodGames.ClientClass.Consumable consumable in this.battleData.DungeonReward.Consumable)
				{
					if (ClientServerCommon.IDSeg._SpecialId.GameMoney == consumable.Id)
						goldRewardOrOtherCount = consumable.Amount;
				}
				message = goldRewardOrOtherCount.ToString();
			}
			else
			{
				message = string.Format(GameUtility.GetUIString("UIPnlBattleResult_WolfSmoke_FaildCount"), ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddCanFaildCount) - this.alreadyFailedTimes);
			}

			return message;
		}

		public override string GetExpRewardOrOtherStr()
		{
			int expRewardOrOtherCount = 0;

			string message = string.Empty;

			bool isWin = false;
			if (this.battleData != null)
				isWin = battleData.BattleRecords[battleData.BattleRecords.Count - 1].TeamRecords[0].IsWinner;
			if (isWin)
			{
				foreach (KodGames.ClientClass.Consumable consumable in this.battleData.DungeonReward.Consumable)
				{
					if (ClientServerCommon.IDSeg._SpecialId.Medals == consumable.Id)
						expRewardOrOtherCount = consumable.Amount;
				}
				message = expRewardOrOtherCount.ToString();
			}
			else
			{
				message = string.Format(GameUtility.GetUIString("UIPnlBattleResult_WolfSmoke_Reset"), ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount) - this.alreadyResetTimes, ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount));
			}

			return message;
		}

		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.DungeonReward;
		}

		public override KodGames.ClientClass.Reward GetFirstPassReward()
		{
			return battleData.FirstpassReward;
		}

		public override bool HasFirstPassReward()
		{
			if (GetFirstPassReward() == null)
				return false;

			var dic = SysLocalDataBase.ConvertIdCountList(GetFirstPassReward());

			if (dic == null || dic.Count == 0)
				return false;

			return true;
		}

		public override bool CanShowFailGuid()
		{
			return true;
		}
	}

	public override void InitViews()
	{
		if (battleResultData.HasFirstPassReward() && battleResultData.IsWinner())
		{
			//有首次通关奖励
			btnRoot.localPosition = btnRootFirstPassMarker.localPosition;
			expLabelRoot.localPosition = expLabelFirstPassMarker.localPosition;
			goldLabelRoot.localPosition = goldLabelFirstPassMarker.localPosition;
		}
		else if (battleResultData.IsWinner())
		{
			expLabelRoot.localPosition = expNormalPassMarker.localPosition;
			goldLabelRoot.localPosition = goldNormalPassMarker.localPosition;
		}

		base.InitViews();

		// Record InterrupteCampaing.
		SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = true;

		bool isWin = battleResultData.IsWinner();
		resetBtn.gameObject.SetActive(false);

		winRewardBg.gameObject.SetActive(isWin);
		if (isWin)
		{
			//无军功
			expLabelObj.SetActive(false);

			//战斗胜利
			rewardLabel1.Text = GameUtility.GetUIString("UIPnlBattleResult_WolfSmoke_Gamemoney_Label");
			rewardLabel2.Text = GameUtility.GetUIString("UIPnlBattleResult_WolfSmoke_Exploit_Label");

			rewardIcon1.SetData(IDSeg._SpecialId.GameMoney);
			rewardIcon2.SetData(IDSeg._SpecialId.Medals);

			rewardCount1.Text = battleResultData.GetGoldRewardOrOtherStr();
			rewardCount2.Text = battleResultData.GetExpRewardOrOtherStr();

			faildLabel1.Text = string.Empty;
			faildLabel2.Text = string.Empty;
			faildDescLabel.Text = string.Empty;
			ClearData();
			StartCoroutine("FillRewardList");
		}
		else
		{
			expLabelObj.SetActive(true);

			//
			var wolfBattleInfo = battleResultData as WolfBattleResultData;
			int resetCount = ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.WolfSmokeAddResetCount) - wolfBattleInfo.AlreadyResetTimes;
			if (resetCount > 0)
				resetBtn.gameObject.SetActive(true);

			resetBtn.Data = wolfBattleInfo.AlreadyResetTimes;
			rewardLabel1.Text = string.Empty;
			rewardLabel2.Text = string.Empty;
			rewardIcon1.Hide(true);
			rewardIcon2.Hide(true);
			rewardCount1.Text = string.Empty;
			rewardCount2.Text = string.Empty;
			faildLabel1.Text = battleResultData.GetGoldRewardOrOtherStr();
			faildLabel2.Text = battleResultData.GetExpRewardOrOtherStr();

			faildDescLabel.Text = GameUtility.GetUIString("UIPnlBattleResult_WolfSmoke_FaildLabel");
			topLvlObject.SetActive(false);
		}
		rewardInfoPanel.SetActive(false);
		firstPass_RewardInfoPnl.SetActive(false);
		firstPass_FirstPassRewardInfoPnl.SetActive(false);
	}

	private void ClearData()
	{
		StopCoroutine("FillRewardList");
		rewardList.ClearList(false);
		rewardList.ScrollPosition = 0f;

		firstPass_RewardList.ClearList(false);
		firstPass_RewardList.ScrollPosition = 0f;

		firstPass_FirstPassRewardList.ClearList(false);
		firstPass_FirstPassRewardList.ScrollPosition = 0f;
	}

	public WolfBattleResultData GetwolfBattleInfo()
	{
		var wolfBattleResultData = battleResultData as WolfBattleResultData;
		return wolfBattleResultData;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRewardList()
	{
		yield return null;

		if (!battleResultData.HasFirstPassReward())
		{
			foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetBattleReward()))
			{
				UIElemBattleResultDungeonItem item = rewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
				item.SetData(kvp.first, kvp.second);

				rewardList.AddItem(item.gameObject);
			}
			rewardList.ScrollToItem(0, 0);
		}
		else//普通奖励，首通奖励两个列表
		{
			foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetBattleReward()))
			{
				UIElemBattleResultDungeonItem item = firstPass_rewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
				item.SetData(kvp.first, kvp.second);

				firstPass_RewardList.AddItem(item.gameObject);
			}
			firstPass_RewardList.ScrollToItem(0, 0);

			foreach (var kvp in SysLocalDataBase.ConvertIdCountList(battleResultData.GetFirstPassReward()))
			{
				UIElemBattleResultDungeonItem item = firstPass_FirstPassRewardPool.AllocateItem().GetComponent<UIElemBattleResultDungeonItem>();
				item.SetData(kvp.first, kvp.second);

				firstPass_FirstPassRewardList.AddItem(item.gameObject);
			}

			firstPass_FirstPassRewardList.ScrollToItem(0, 0);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		RequestMgr.Inst.Request(new QueryWolfSmoke());
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}

	//点击图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClinkRewardItem(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI(assetIcon.AssetId);
	}

	//点击重置按钮
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRestart(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgWolfStart), (int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	protected override IEnumerator ShowRewardPanel()
	{
		Animation ani = resultInfoPanel.GetComponent<Animation>();
		if (ani != null)
			ani.Play();

		yield return new WaitForSeconds(0.5f);

		if (!battleResultData.HasFirstPassReward())
		{
			if (rewardInfoPanel != null && battleResultData.IsWinner())
				rewardInfoPanel.SetActive(true);
			else if (loseInfoPanel != null)
				loseInfoPanel.SetActive(true);
		}
		else//有首次通关奖励，显示首次通关奖励列表
		{
			if (firstPass_RewardInfoPnl != null && battleResultData.IsWinner())
			{
				firstPass_RewardInfoPnl.SetActive(true);

				if (firstPass_FirstPassRewardInfoPnl != null && battleResultData.IsWinner())
					firstPass_FirstPassRewardInfoPnl.SetActive(true);
			}
			else if (loseInfoPanel != null)
					loseInfoPanel.SetActive(true);
		}
	}
}
