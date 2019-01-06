using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerSweepBattle : UIPnlBattleResultBase
{
	public SpriteText badgeLabel;
	public SpriteText rewardTitle;
	public SpriteText rewardMessage;

	//public SpriteText battleInfoLabel;

	public UIBox battleType;

	public UIScrollList battleInfoList;
	public GameObjectPool battleInfoPool;
	public GameObjectPool layerPool;
	public GameObjectPool rewardPool;
	public GameObjectPool battleInfoLabelPool;

	public class TowerSweepResultData : BattleResultData
	{
		private KodGames.ClientClass.CombatResultAndReward battleData;
		public KodGames.ClientClass.CombatResultAndReward BattleData { get { return battleData; } }

		private com.kodgames.corgi.protocol.MelaleucaFloorInfo melaleucaFloorInfo;
		public com.kodgames.corgi.protocol.MelaleucaFloorInfo MelaleucaFloorInfo { get { return melaleucaFloorInfo; } }

		private KodGames.ClientClass.CostAndRewardAndSync firstChallengeReward;
		public KodGames.ClientClass.CostAndRewardAndSync FirstChallengeReward { get { return firstChallengeReward; } }

		private List<KodGames.ClientClass.CostAndRewardAndSync> passRewards;
		public List<KodGames.ClientClass.CostAndRewardAndSync> PassRewards { get { return passRewards; } }

		private List<KodGames.ClientClass.CostAndRewardAndSync> firstPassRewards;
		public List<KodGames.ClientClass.CostAndRewardAndSync> FirstPassRewards { get { return firstPassRewards; } }

		private List<int> firstPassLayers;
		public List<int> FirstPassLayers { get { return firstPassLayers; } }

		private int layers;
		public int Layers { get { return layers; } }

		private int combatCount;
		public int CombatCount { get { return combatCount; } }

		public TowerSweepResultData(KodGames.ClientClass.CombatResultAndReward battleData, com.kodgames.corgi.protocol.MelaleucaFloorInfo melaleucaFloorInfo,
			KodGames.ClientClass.CostAndRewardAndSync firstChallengeReward, List<KodGames.ClientClass.CostAndRewardAndSync> passRewards,
			List<KodGames.ClientClass.CostAndRewardAndSync> firstPassRewards, List<int> firstPassLayers, int layers, int combatCount)
			: base(_UIType.UIPnlTowerSweepBattle)
		{
			this.battleData = battleData;
			this.melaleucaFloorInfo = melaleucaFloorInfo;
			this.firstChallengeReward = firstChallengeReward;
			this.passRewards = passRewards;
			this.firstPassRewards = firstPassRewards;
			this.firstPassLayers = firstPassLayers;
			this.layers = layers;
			this.combatCount = combatCount;
			this.CombatType = _CombatType.Tower;
		}

		public override bool CanShowView()
		{
			return true;
		}

		public override bool IsWinner()
		{
			return true;
		}

		public override int GetAppraiseNumber()
		{
			return 0;
		}

		public override bool CanShowLvlInformation()
		{
			return true;
		}

		//扫荡到达层数
		public override string GetGoldRewardOrOtherStr()
		{
			string getLayer = "";
			if (this.MelaleucaFloorInfo != null)
				getLayer = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_Layer_Label"), this.melaleucaFloorInfo.currentLayer.ToString());
			else
				getLayer = string.Format(GameUtility.GetUIString("UIPnlTowerBattleResult_Layer_Label"), SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer.ToString());

			return getLayer;
		}

		//扫荡获得积分
		public override string GetExpRewardOrOtherStr()
		{
			int getPoint = 0;
			if (this.MelaleucaFloorInfo != null)
				getPoint = this.melaleucaFloorInfo.currentPoint - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint;

			//积分提前处理战斗后爬楼动画判定
			SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint += getPoint;
			return getPoint.ToString();
		}

		public override KodGames.ClientClass.Reward GetBattleReward()
		{
			return battleData.Rewards[0];
		}

		public override bool CanShowFailGuid()
		{
			return false;
		}
	}

	public override void InitViews()
	{
		base.InitViews();

		FillData();
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
		var sweepResultData = battleResultData as TowerSweepResultData;

		//清空千机楼刷新数据防止请求两次
		SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.NextResetTime = 0;

		//战斗层数
		switch (sweepResultData.Layers)
		{
			case 2: battleType.SetState(0); break;
			case 4: battleType.SetState(1); break;
			default: battleType.SetState(0); break;
		}

		//获取扫荡次数
		int sweepBattleCount = sweepResultData.CombatCount;
		//获取扫荡类型
		int sweepBattleType = sweepResultData.Layers;
		//获取起始层数
		int startLayer = SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer;

		bool isWin = true;

		for (int i = 0; i < sweepBattleCount && isWin; i++)
		{
			if (sweepResultData.BattleData != null)
			{
				if (sweepResultData.PassRewards == null)
					isWin = false;
				else
					if (i >= sweepResultData.PassRewards.Count / sweepBattleType)
						isWin = false;
			}
			int layerStart = startLayer + i * sweepBattleType;

			UIListItemContainer uiContainer = battleInfoPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemTowerLayerResultItem item = uiContainer.GetComponent<UIElemTowerLayerResultItem>();
			uiContainer.data = item;
			item.SetData(layerStart + 1, layerStart + sweepBattleType, isWin);
			battleInfoList.AddItem(item.gameObject);
			
			//胜利层数开始标记j
			int j = 0;
			for (; j < sweepBattleType && isWin; j++)
			{
				int battleLayer = startLayer + sweepBattleType * i + j + 1;
				UIListItemContainer uiContainerLayer = layerPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemTowerLayer itemLayer = uiContainerLayer.GetComponent<UIElemTowerLayer>();
				uiContainerLayer.data = itemLayer;
				itemLayer.SetData(battleLayer, true);
				battleInfoList.AddItem(itemLayer.gameObject);

				if (battleLayer > SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.MaxPassLayer && isWin)
				{
					MelaleucaFloorConfig.Floor floorCfg =new MelaleucaFloorConfig.Floor();

					int maxLayer = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.Floors.Count;

					if (battleLayer > maxLayer)
						floorCfg = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(maxLayer);
					else
						floorCfg = ConfigDatabase.DefaultCfg.MelaleucaFloorConfig.GetFloorByLayer(battleLayer);
		
					UIListItemContainer uiContainerReward = rewardPool.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemTowerRewardListItem itemReward = uiContainerReward.GetComponent<UIElemTowerRewardListItem>();
					uiContainerReward.data = itemReward;

					itemReward.SetData(floorCfg.FirstPassReward);
					battleInfoList.AddItem(itemReward.gameObject);
				}
			}

			if (!isWin)
			{
				BattleDataAnalyser analyser = new BattleDataAnalyser(sweepResultData.BattleData);
				
				//失败层数开始标记调用
				for (; j < analyser.BattleCount; j++)
				{
					int battleLayer = startLayer + sweepBattleType * i + j + 1;
					UIListItemContainer uiContainerLayer = layerPool.AllocateItem().GetComponent<UIListItemContainer>();
					UIElemTowerLayer itemLayer = uiContainerLayer.GetComponent<UIElemTowerLayer>();
					uiContainerLayer.data = itemLayer;
					itemLayer.SetData(battleLayer, false);
					battleInfoList.AddItem(itemLayer.gameObject);
				}

				bool isAvatarLife = false;

				//定位到失败的战斗
				int battleIdx = analyser.BattleCount - 1;

				List<int> ids = analyser.GetAvatarIdxes(0, battleIdx);

				foreach (int avatarIdx in ids)
				{
					if (analyser.GetAvatarLeftHP(avatarIdx, battleIdx) > 0f)
						isAvatarLife = true;
				}

				string failedMessage = string.Empty;

				if (isAvatarLife)
					failedMessage = GameUtility.GetUIString("UIPnlTowerBattleResult_FailedResult2_Label");
				else
					failedMessage = GameUtility.GetUIString("UIPnlTowerBattleResult_FailedResult1_Label");

				UIListItemContainer uiContainerLabel = battleInfoLabelPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemTowerTextLabel itemLabel = uiContainerLabel.GetComponent<UIElemTowerTextLabel>();
				uiContainerLabel.data = itemLabel;
				itemLabel.SetData(failedMessage);
				battleInfoList.AddItem(itemLabel.gameObject);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		//千机楼扫荡标记
		SysUIEnv.Instance.GetUIModule<UIPnlTowerPlayerInfo>().IsSweepBattle = true;

		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTowerScene));
		else
			SysGameStateMachine.Instance.EnterState<GameState_Tower>();
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnReplayClick(UIButton btn)
	{
		HideSelf();

		var sweepResultData = battleResultData as TowerSweepResultData;

		//如果在千机楼场景则加载战斗场景，如果在战斗场景则重播
		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower)
		{
			List<object> paramsters = new List<object>();
			paramsters.Add(sweepResultData.BattleData);
			paramsters.Add(sweepResultData);
			SysGameStateMachine.Instance.EnterState<GameState_Battle>(paramsters);
		}
		else
		{
			GameState_Battle battleState = SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().CurrentState as GameState_Battle;
			battleState.ReplayBattle();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardBtn(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int id = (int)assetIcon.Data;
		GameUtility.ShowAssetInfoUI(id, _UILayer.Top);
	}
}
