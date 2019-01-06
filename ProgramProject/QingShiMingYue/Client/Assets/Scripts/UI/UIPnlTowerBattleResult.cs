using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerBattleResult : UIPnlBattleResultBase
{
	public SpriteText badgeLabel;
	public SpriteText rewardTitle;
	public SpriteText rewardMessage;
	public GameObject successObj;
	public GameObject failedObj;
	public AutoSpriteControlBase battleResultBox;

	public UIBox battleType;

	public UIScrollList battleInfoList;
	public GameObjectPool battleInfoPool;

	public GameObjectPool firstBattleInfoPool;

	public GameObject actionButtonRoot;	

	public class TowerBattleResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;
		public KodGames.ClientClass.CombatResultAndReward BattleData { get { return battleData; } }

		private com.kodgames.corgi.protocol.MelaleucaFloorInfo melaleucaFloorInfo;
		public com.kodgames.corgi.protocol.MelaleucaFloorInfo MelaleucaFloorInfo { get { return melaleucaFloorInfo; } }

		private KodGames.ClientClass.CostAndRewardAndSync firstChallengeReward;
		public KodGames.ClientClass.CostAndRewardAndSync FirstChallengeReward { get { return firstChallengeReward; } }
		
		private List<KodGames.ClientClass.CostAndRewardAndSync> passRewards;
		public List<KodGames.ClientClass.CostAndRewardAndSync> PassRewards{ get { return passRewards; } }
		
		private List<KodGames.ClientClass.CostAndRewardAndSync> firstPassRewards;
		public List<KodGames.ClientClass.CostAndRewardAndSync> FirstPassRewards { get { return firstPassRewards; } }

		private List<int> firstPassLayers;
		public List<int> FirstPassLayers { get { return firstPassLayers; } }

		private int layers;
		public int Layers { get { return layers;} }

		private int point;
		public int Point { get { return point; } }

		public TowerBattleResultData(KodGames.ClientClass.CombatResultAndReward battleData, com.kodgames.corgi.protocol.MelaleucaFloorInfo melaleucaFloorInfo,
			KodGames.ClientClass.CostAndRewardAndSync firstChallengeReward, List<KodGames.ClientClass.CostAndRewardAndSync> passRewards,
			List<KodGames.ClientClass.CostAndRewardAndSync> firstPassRewards, List<int> firstPassLayers, int layers): base(_UIType.UIPnlTowerBattleResult)
		{
			this.battleData = battleData;
			this.melaleucaFloorInfo = melaleucaFloorInfo;
			this.firstChallengeReward = firstChallengeReward;
			this.passRewards = passRewards;
			this.firstPassRewards = firstPassRewards;
			this.firstPassLayers = firstPassLayers;
			this.layers = layers;	
			this.CombatType = _CombatType.Tower;
		}

		public override bool CanShowView()
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

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		public override string GetGoldRewardOrOtherStr()
		{
			return string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_Layer_Label"), this.melaleucaFloorInfo.currentLayer.ToString());
		}

		public override string GetExpRewardOrOtherStr()
		{
			int getPoint = this.melaleucaFloorInfo.currentPoint - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint;
			this.point = getPoint;
			//积分提前处理战斗后爬楼动画判定
			SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint = this.melaleucaFloorInfo.currentPoint;

			return getPoint.ToString();
		}

		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.Rewards[0];
		}

		public override bool CanShowFailGuid()
		{
			return true;
		}
	}

	public override void InitViews()
	{
		base.InitViews();

		var towerBattleResultData = battleResultData as TowerBattleResultData;
		bool isWin = towerBattleResultData.IsWinner();
		
		//战斗层数
		switch(towerBattleResultData.Layers)
		{
			case 2: battleType.SetState(0); break;
			case 4: battleType.SetState(1); break;
			case 8: battleType.SetState(2); break;
			default: battleType.SetState(0); break;
		}

		if (isWin)
		{
			//WinSet
			successObj.SetActive(true);
			failedObj.SetActive(false);
			battleResultBox.SetState(0);

			//通过过关奖励来显示首次通关的层
			int startLayer = towerBattleResultData.MelaleucaFloorInfo.currentLayer - towerBattleResultData.FirstPassRewards.Count + 1;
			string startLayerStr = GameDefines.textColorWhite + startLayer.ToString() + GameDefines.textColorBtnYellow;
			string endLayerStr = GameDefines.textColorWhite + towerBattleResultData.MelaleucaFloorInfo.currentLayer.ToString() + GameDefines.textColorBtnYellow;
			if (towerBattleResultData.FirstPassLayers.Count > 0)
				rewardTitle.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_HasFirstReward_Label"), GameDefines.textColorBtnYellow, startLayerStr, endLayerStr);
			else
				rewardTitle.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_NotFirstRewardTitle_Label"), GameDefines.textColorBtnYellow);

			FillData();
		}
		else
		{
			//FailedSet
			successObj.SetActive(false);
			failedObj.SetActive(true);
			battleResultBox.SetState(1);
			actionButtonRoot.transform.localPosition = new Vector3(0, -345f, -0.01f);

			//rewardTitle.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_FailedCount_Label"), GameDefines.textColorBtnYellow, ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.ChallengeCostPerTimes,
			//    ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel,VipConfig._VipLimitType.MelaleucaFloorChallengeCount)-SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.ChallengeFailsCount);

			rewardTitle.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_FailedCount_Label"), ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, VipConfig._VipLimitType.MelaleucaFloorChallengeCount) - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.ChallengeFailsCount - ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.ChallengeCostPerTimes);		
		}
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void FillData()
	{
		ClearData();
		StartCoroutine("FillList");
	}

	private void ClearData()
	{
		StopCoroutine("FillList");

		battleInfoList.ClearList(false);
		battleInfoList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;
		var towerBattleResultData = battleResultData as TowerBattleResultData;

		int startLayer = towerBattleResultData.MelaleucaFloorInfo.currentLayer - towerBattleResultData.PassRewards.Count + 1;

		//清空千机楼刷新数据防止请求两次
		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.NextResetTime = 0;

		//每日首次挑战
		if (towerBattleResultData.FirstChallengeReward.Reward != null)
		{
			rewardMessage.Text = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_FirstChallenge_Label"),
				towerBattleResultData.FirstChallengeReward.Reward.Consumable[0].Amount.ToString());
		}
		else rewardMessage.Text = string.Empty;

		for (int index = 0; index < towerBattleResultData.PassRewards.Count; index++)
		{
			if(index >= towerBattleResultData.PassRewards.Count - towerBattleResultData.FirstPassRewards.Count)
			{
				UIListItemContainer uiContainer = firstBattleInfoPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemLevelBattleFirstResult item = uiContainer.GetComponent<UIElemLevelBattleFirstResult>();
				uiContainer.data = item;
				item.SetData(startLayer + index);
				battleInfoList.AddItem(item.gameObject);
			}
			else
			{
				UIListItemContainer uiContainer = battleInfoPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemLevelBattleResult item = uiContainer.GetComponent<UIElemLevelBattleResult>();
				uiContainer.data = item;
				item.SetData(startLayer + index);
				battleInfoList.AddItem(item.gameObject);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
		SysGameStateMachine.Instance.EnterState<GameState_Tower>();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBattleInfoClick(UIButton btn)
	{
		var towerBattleResultData = battleResultData as TowerBattleResultData;
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgTowerBattleData), towerBattleResultData.BattleData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		var towerBattleResultData = battleResultData as TowerBattleResultData;
		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint -= towerBattleResultData.Point;
		GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
		battleState.ReplayBattle();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardBtn(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int id = (int)assetIcon.Data;
		GameUtility.ShowAssetInfoUI(id, _UILayer.Top);	
	}
}
