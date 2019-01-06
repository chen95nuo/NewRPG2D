using UnityEngine;
using System;
using System.Collections.Generic;
using ClientServerCommon;

public static class GameAnalyticsUtility
{
	public static void SetTDGAAccount(LoginInfo info)
	{
		SetTDGAAccount(info, -1);
	}

	public static void SetTDGAAccount(LoginInfo info, int level)
	{
		int accountType = 0;
		if (!info.QuickLogin)
		{
			accountType = 1;
		}

		SetTDGAAccount(info, accountType, level);
	}

	public static void SetTDGAAccount(LoginInfo info, int accountType, int level)
	{
		if (info == null)
		{
			return;
		}

		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.SetAccount(info.AccountId.ToString(), accountType, info.Account, level, info.LoginArea.AreaName);
	}

	public static void OnEventBattle(int battleType, UIPnlBattleResultBase.BattleResultData battleResultData, KodGames.ClientClass.CombatResultAndReward battleData)
	{
		switch (battleType)
		{
			case _CombatType.Campaign:
			case _CombatType.ActivityCampaign:
				var campaignData = battleResultData as UIPnlCampaignBattleResult.CampaignBattleResultData;
				var combats = new List<KodGames.ClientClass.CombatResultAndReward>();
				combats.Add(battleData);
				OnEventBattleCampaign(campaignData.DungeonId, combats, false);
				break;

			case _CombatType.Tower:

				if (battleResultData is UIPnlTowerBattleResult.TowerBattleResultData)
				{
					var towerBattle = battleResultData as UIPnlTowerBattleResult.TowerBattleResultData;
					OnEventBattleTower(towerBattle.IsWinner(), towerBattle.MelaleucaFloorInfo.currentLayer, towerBattle.MelaleucaFloorInfo.currentPoint - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint);
				}
				else if (battleResultData is UIPnlTowerSweepBattle.TowerSweepResultData)
				{
					var sweepBattle = battleResultData as UIPnlTowerSweepBattle.TowerSweepResultData;
					OnEventBattleTower(sweepBattle.IsWinner(), sweepBattle.MelaleucaFloorInfo.currentLayer, sweepBattle.MelaleucaFloorInfo.currentPoint - SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint);
				}

				break;
			case _CombatType.WolfSmoke:
				OnEventBattleWolf(battleResultData as UIPnlWolfBattleResult.WolfBattleResultData);
				break;
		}
	}

	public static void OnEventBattleCampaign(int dungeonId, List<KodGames.ClientClass.CombatResultAndReward> combats, bool continueCombat)
	{
		if (dungeonId == IDSeg.InvalidId || combats == null || combats.Count <= 0)
			return;

		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		bool isWin = continueCombat ? true : combats[0].BattleRecords[combats[0].BattleRecords.Count - 1].TeamRecords[0].IsWinner;
		string dungeonInfo = string.Format((isWin ? "{0}_Win_{1} ({2}_{3})" : "{0}_Lose_{1}  ({2}_{3})"),
										   dungeonId.ToString("X"),
										   combats.Count,
										   ItemInfoUtility.GetAssetName(dungeonCfg.ZoneId),
										   ItemInfoUtility.GetAssetName(dungeonCfg.dungeonId));

		var rewards = new List<ClientServerCommon.Reward>();
		foreach (var combat in combats)
		{
			rewards.AddRange(SysLocalDataBase.CCRewardToCSCReward(combat.FirstpassReward, false));
			rewards.AddRange(SysLocalDataBase.CCRewardToCSCReward(combat.DungeonReward, false));
			rewards.AddRange(SysLocalDataBase.CCRewardToCSCReward(combat.DungeonReward_ExpSilver, false));
		}

		string rewardInfo = SysLocalDataBase.GetRewardDesc(rewards, false, true, false);

		Dictionary<string, object> map = new Dictionary<string, object>();
		map.Add("PlayerLevel", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level.ToString());
		map.Add("DungeonId", dungeonInfo);
		map.Add("Reward", rewardInfo);

		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnEvent(ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(dungeonCfg.ZoneId) ? "CampaignActivityBattle" : "CampaignBattle", map);
	}

	public static void OnEventBattleWolf(UIPnlWolfBattleResult.WolfBattleResultData battleResultData)
	{
		var stageCfg = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetStageById(battleResultData.StageId);
		var additionCfg = ConfigDatabase.DefaultCfg.WolfSmokeConfig.GetAdditionById(battleResultData.AdditionId);
		if (additionCfg == null || stageCfg == null)
			return;

		Dictionary<string, object> map = new Dictionary<string, object>();
		map.Add("PlayerLevel", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level.ToString());
		map.Add("Addition", PositionConfig._EmBattleType.GetDisplayNameByType(additionCfg.EmBattleAttribute.type, ConfigDatabase.DefaultCfg));
		map.Add("BattleResult", battleResultData.IsWinner() ? "Win" : "Lose");
		map.Add("StageId", string.Format("{0}_{1}", battleResultData.StageId.ToString("X"), stageCfg.StageName));

		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnEvent("WolfBattle", map);
	}

	public static void OnEventBattleTower(bool isWin, int currentLayer, int getPointCount)
	{
		Dictionary<string, object> map = new Dictionary<string, object>();
		map.Add("PlayerLevel", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level.ToString());
		map.Add("CurrentLayer", currentLayer.ToString());
		map.Add("BattleResult", isWin ? "Win" : "Lose");
		map.Add("GetPoint", getPointCount.ToString());

		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnEvent("TowerBattle", map);
	}

	public static void OnEventErrorMessage(string tag, string message)
	{
		if (string.IsNullOrEmpty(tag) || string.IsNullOrEmpty(message))
			return;

		Dictionary<string, object> map = new Dictionary<string, object>();
		map.Add("Tag", tag);
		map.Add("PlayerLevel", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level.ToString());
		map.Add("PlayerExp", SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Experience.ToString());
		map.Add("ErrorMsg", message);

		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnEvent("ErrorMessage", map);
	}

	public static void OnReward(double virtualCurrencyAmount, string reason)
	{
		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnReward(virtualCurrencyAmount, reason);
	}

	public static void OnPurchase(string item, int number, double price)
	{
		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnPurchase(item, number, price);
	}

	public static void OnMission(List<KodGames.ClientClass.Quest> quests)
	{
		if (quests == null || quests.Count <= 0)
			return;

		foreach (KodGames.ClientClass.Quest q in quests)
		{
			string missionId = ItemInfoUtility.GetAssetName(q.QuestId);
			if (q.Phase == ClientServerCommon.QuestConfig._PhaseType.Active)
			{
				KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnBegin(missionId);
			}
			else if (q.Phase >= ClientServerCommon.QuestConfig._PhaseType.FinishedAndGotReward)
			{
				KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnCompleted(missionId);
			}
		}
	}

	public static void OnTutorialBegin(SysTutorial.TutorialData tutorialData)
	{
		if (tutorialData.stepIndex == 0)
		{
			string tutorialName = "Tutorial_" + tutorialData.tutorialConfig.id.ToString("X");
			KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnBegin(tutorialName);
		}
	}

	public static void OnTutorialCompleted(int tutorialId)
	{
		string tutorialName = "Tutorial_" + tutorialId.ToString("X");
		KodGames.ExternalCall.TalkingDataGameAnalyticsPlugin.OnCompleted(tutorialName);
	}
}