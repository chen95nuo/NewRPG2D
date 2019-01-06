using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class CampaignData
{
	public enum ZoneReward
	{
		UnKonw = 0,
		NormalOneReward = 1,
		NormalTwoReward = 2,
		NormalThreeReward = 3,
		HardOneReward = 4,
		HardTwoReward = 5,
		HardThreeReward = 6,
		NightmareOneReward = 7,
		NightmareTwoReward = 8,
		NightmareThreeReward = 9,
	}

	public List<bool> zoneLockStates = new List<bool>(); // 普通副本的锁定状态：有锁还是没有锁
	public int shouldUnlockIndex = -1;               // 普通副本中需要进行解锁的副本索引

	// 普通副本状态： 是否有解锁副本，各个副本的状态
	public static CampaignData GetCampaignData()
	{
		CampaignData campaignData = new CampaignData();

		List<CampaignConfig.Zone> normalZones = ConfigDatabase.DefaultCfg.CampaignConfig.zones;

		for (int index = 0; index < normalZones.Count; index++)
		{
			bool currentZoneLockState = true;

			if (0 == index && normalZones[index].GetDungeonDifficultyByDifficulty(_DungeonDifficulity.Common).levelLimit <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
				currentZoneLockState = false;
			else
			{
				KodGames.ClientClass.Zone preZone = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(normalZones[index - 1].zoneId);
				KodGames.ClientClass.Zone zone = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(normalZones[index].zoneId);

				if (preZone != null && zone != null)
				{
					currentZoneLockState = preZone.Status < _ZoneStatus.ZoneComplete || (preZone.Status == _ZoneStatus.ZoneComplete && zone.Status == _ZoneStatus.UnlockAnimation);

					// 如果当前副本为 '未播放解锁动画'状态，并且上一个副本的状态为'已完成' ,那么当前副本为需要解锁的副本。
					if (preZone.Status == _ZoneStatus.ZoneComplete && zone.Status == _ZoneStatus.UnlockAnimation && normalZones[index].GetDungeonDifficultyByDifficulty(_DungeonDifficulity.Common).levelLimit <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
						campaignData.shouldUnlockIndex = index;
				}
			}

			campaignData.zoneLockStates.Add(currentZoneLockState);
		}

		return campaignData;
	}

	// 普通副本的评价
	public static ZoneReward GetZoneRewardState(int zoneId)
	{
		CampaignConfig.Zone zoneConfig = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId);
		KodGames.ClientClass.Zone zoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneId);

		// 没有副本记录
		if (zoneRecord == null || zoneRecord.DungeonDifficulties == null)
			return ZoneReward.UnKonw;

		for (int i = 0; i < zoneRecord.DungeonDifficulties.Count; i++)
		{
			var diffRecord = zoneRecord.DungeonDifficulties[i];
			var diffConfig = zoneConfig.GetDungeonDifficultyByDifficulty(diffRecord.DifficultyType);
			int starCount = 0;
			for (int j = 0; j < diffRecord.Dungeons.Count; j++)
				starCount += diffRecord.Dungeons[j].BestRecord;

			for (int j = 0; j < diffConfig.starRewardConditions.Count; j++)
			{
				if (starCount >= diffConfig.starRewardConditions[j].requireStarCount && !diffRecord.BoxPickedIndexs.Contains(j))
					return ZoneReward.UnKonw + i * (ZoneReward.NormalThreeReward - ZoneReward.UnKonw) + j + 1;
			}
		}

		return ZoneReward.UnKonw;
	}

	public static bool IsDiffcultComplement(int zoneID, int diffType)
	{
		KodGames.ClientClass.Zone normalZoneRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(zoneID);
		KodGames.ClientClass.DungeonDifficulty diffcult = null;

		foreach (var tempDiffcult in normalZoneRecord.DungeonDifficulties)
		{
			if (tempDiffcult.Dungeons == null || tempDiffcult.Dungeons.Count <= 0)
				continue;

			if (diffType == tempDiffcult.DifficultyType)
			{
				diffcult = tempDiffcult;
				break;
			}
		}

		List<int> dungeonStars = new List<int>();
		if (diffcult != null && diffcult.Dungeons.Count > 0)
		{
			foreach (var tempDungeon in diffcult.Dungeons)
				dungeonStars.Add(tempDungeon.BestRecord);

			dungeonStars.Sort();
		}

		return IsDiffcultComplement(ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneID), diffType, dungeonStars);
	}

	private static bool IsDiffcultComplement(CampaignConfig.Zone zoneConfig, int diffcultType, List<int> diffcultRecords)
	{
		CampaignConfig.DungeonDifficulty diffcult = zoneConfig.GetDungeonDifficultyByDifficulty(diffcultType);
		if (diffcult == null || diffcultRecords.Count <= 0)
			return false;

		return diffcult.dungeons.Count == diffcultRecords.Count && diffcultRecords[0] >= 1;
	}

	public static int GetLastNormalZoneIndex()
	{
		for (int index = 0; index < ConfigDatabase.DefaultCfg.CampaignConfig.zones.Count; index++)
			if (ConfigDatabase.DefaultCfg.CampaignConfig.zones[index].zoneId == SysLocalDataBase.Inst.LocalPlayer.CampaignData.LastNormalBattleZoneId)
				return index;

		var campaignData = GetCampaignData();
		return campaignData.shouldUnlockIndex > 0 ? campaignData.shouldUnlockIndex - 1 : ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.defaultNormalCampaignIndex;
	}

	// 设置本地记录难度切换状态
	public static void SetCampaignDiffTabState(int zoneID, int zoneDiff)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.LocalRecordDiffTab.ContainsKey(zoneID))
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.LocalRecordDiffTab[zoneID] = zoneDiff;
		else
			SysLocalDataBase.Inst.LocalPlayer.CampaignData.LocalRecordDiffTab.Add(zoneID, zoneDiff);
	}

	// 获取本地记录难度切换状态
	public static int GetCampaignDiffTabState(int zoneID)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.LocalRecordDiffTab.ContainsKey(zoneID))
			return SysLocalDataBase.Inst.LocalPlayer.CampaignData.LocalRecordDiffTab[zoneID];
		else
		{
			ZoneReward zoneStar = GetZoneRewardState(zoneID);

			switch (zoneStar)
			{
				case ZoneReward.UnKonw:
				case ZoneReward.NormalOneReward:
				case ZoneReward.NormalTwoReward:
				case ZoneReward.NormalThreeReward:
					return _DungeonDifficulity.Common;

				case ZoneReward.HardOneReward:
				case ZoneReward.HardTwoReward:
				case ZoneReward.HardThreeReward:
					return _DungeonDifficulity.Hard;

				default:
					return _DungeonDifficulity.Nightmare;
			}
		}
	}

	// 设置战斗中副本的对话状态
	public static void SetDungeonDialogueState(int zoneID, int dungeonID, int stageIndex, int stageType)
	{
		KodGames.ClientClass.Dungeon dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(zoneID, dungeonID);
		int record = 1;

		switch (stageType)
		{
			case _StateDialogueType.BeforeBattle:
				record = record << (3 * _StateDialogueType.AfterStageLost);
				break;
			default:
				record = record << (stageIndex * _StateDialogueType.AfterStageLost + stageType - _StateDialogueType.BeforeStage);
				break;
		}

		dungeonRecord.DungeonDialogState = dungeonRecord.DungeonDialogState | record;

		RequestMgr.Inst.Request(new SetDungeonDialogStateReq(zoneID, dungeonID, dungeonRecord.DungeonDialogState));
	}

	// 获取战斗中副本的对话状态
	public static CampaignConfig.StageDialogue GetDungeonDialogue(int zoneID, int dungeonID, int stageIndex, int stageType)
	{
		KodGames.ClientClass.Dungeon dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(zoneID, dungeonID);

		bool canShowDialogue = false;
		int record = dungeonRecord.DungeonDialogState;

		if (record == 0)
			canShowDialogue = true;
		else
		{
			switch (stageType)
			{
				case _StateDialogueType.BeforeBattle:
					record = record >> (3 * _StateDialogueType.AfterStageLost);
					canShowDialogue = (record & 1) != 1;
					break;
				case _StateDialogueType.AfterStageLost:

					bool isStageWinNotPlay = ((record >> (stageIndex * _StateDialogueType.AfterStageLost + _StateDialogueType.AfterStageWin - _StateDialogueType.BeforeStage)) & 1) != 1;
					record = record >> (stageIndex * _StateDialogueType.AfterStageLost + stageType - _StateDialogueType.BeforeStage);
					canShowDialogue = (record & 1) != 1 && isStageWinNotPlay;
					break;
				default:
					record = record >> (stageIndex * _StateDialogueType.AfterStageLost + stageType - _StateDialogueType.BeforeStage);
					canShowDialogue = (record & 1) != 1;
					break;
			}
		}

		if (canShowDialogue)
		{
			return ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonID).GetStageDialogue(stageIndex, stageType);
		}

		return null;
	}

	// 获取配置文件中章节的索引，普通副本与秘境的分别索引
	public static int GetZoneIndexInCfg(int zoneId)
	{
		if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId))
		{
			for (int index = 0; index < ConfigDatabase.DefaultCfg.CampaignConfig.secretZones.Count; index++)
			{
				if (ConfigDatabase.DefaultCfg.CampaignConfig.secretZones[index].zoneId == zoneId)
					return index;
			}
		}
		else
		{
			for (int index = 0; index < ConfigDatabase.DefaultCfg.CampaignConfig.zones.Count; index++)
			{
				if (ConfigDatabase.DefaultCfg.CampaignConfig.zones[index].zoneId == zoneId)
					return index;
			}
		}

		return -1;
	}

	// 判定秘境是否开启，用于判定是否加锁。此时与普通副本不同的ui显示，此时不判定等级条件，如果开启了但是等级不到依然不加锁。
	public static bool IsZoneTimeOpen(int zoneId)
	{
		// 历练不进行时间控制
		if (!ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId))
			return true;

		var zoneCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId);
		if (zoneCfg == null)
		{
			Debug.LogError("Zone Not Found " + zoneCfg.zoneId.ToString("X"));
			return false;
		}

		if (ActivityManager.Instance == null)
			return false;

		var zoneActivity = ActivityManager.Instance.GetActivitySecret(zoneCfg.activityId);
		if (zoneActivity == null)
		{
			Debug.LogError("Activity Not Found " + zoneCfg.activityId.ToString("X"));
			return false;
		}

		return zoneActivity.IsActive;
	}

	public static bool IsZonePreLimitOpen(int zoneId)
	{
		// 秘境没有前置章节的控制
		if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId))
			return true;

		for (int index = 0; index < ConfigDatabase.DefaultCfg.CampaignConfig.zones.Count; index++)
		{
			if (zoneId == ConfigDatabase.DefaultCfg.CampaignConfig.zones[index].zoneId)
			{
				if (index == 0)
					return true;

				var preZoneRecrod = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchZone(ConfigDatabase.DefaultCfg.CampaignConfig.zones[index - 1].zoneId);

				return preZoneRecrod != null && preZoneRecrod.Status == _ZoneStatus.ZoneComplete;
			}
		}

		return false;
	}

	public static bool IsZoneLevelLimitOpen(int zoneId)
	{
		var zoneCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId);

		return SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= zoneCfg.dungeonDifficulties[0].levelLimit;
	}

	public static bool IsDungeonPreDiffLimitOpen(int dungeonId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		if (dungeonCfg.DungeonDifficulty == _DungeonDifficulity.Common)
			return true;
		else
			return IsDiffcultComplement(dungeonCfg.ZoneId, dungeonCfg.DungeonDifficulty - 1);
	}

	public static bool IsDungeonDiffLevelLimitOpen(int dungeonId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		var diffCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(dungeonCfg.ZoneId).GetDungeonDifficultyByDifficulty(dungeonCfg.DungeonDifficulty);

		return SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= diffCfg.levelLimit;
	}

	public static bool IsDungeonPreLimitOpen(int dungeonId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		var dungeonCfgs = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(dungeonCfg.ZoneId).GetDungeonDifficultyByDifficulty(dungeonCfg.DungeonDifficulty).dungeons;

		for (int index = 0; index < dungeonCfgs.Count; index++)
		{
			if (dungeonCfgs[index].dungeonId == dungeonCfg.dungeonId)
			{
				if (index == 0)
					return true;

				var dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfgs[index - 1].ZoneId, dungeonCfgs[index - 1].dungeonId);

				return dungeonRecord != null && dungeonRecord.BestRecord > 0;
			}
		}

		return false;
	}

	public static string CheckZoneEnterErrorMsg(int zoneId)
	{
		var zoneCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(zoneId);
		bool isActivityZone = ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(zoneId);
		string errorMsg = string.Empty;

		// 检查功能开启
		int openFunction = isActivityZone ? _OpenFunctionType.Secret : _OpenFunctionType.Dungeon;
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(openFunction) ||
			ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(openFunction) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			errorMsg = GameUtility.FormatUIString(
				isActivityZone ? "UIMainScene_ActivityCampaignClose" : "UIMainScene_CampaignClose",
				ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(openFunction));

		// 检查开启时间
		if (string.IsNullOrEmpty(errorMsg) && !IsZoneTimeOpen(zoneCfg.zoneId))
			errorMsg = GameUtility.FormatUIString(
				isActivityZone ? "UIPnlCampaign_JumpError_Time_ActivityZone" : "UIPnlCampaign_JumpError_Time_Zone",
				ItemInfoUtility.GetAssetName(zoneId),
				zoneCfg.openDesc);

		// 检查章节开启等级
		if (string.IsNullOrEmpty(errorMsg) && !IsZoneLevelLimitOpen(zoneCfg.zoneId))
			errorMsg = GameUtility.FormatUIString(
				isActivityZone ? "UIPnlCampaign_JumpError_ActivityZoneLevelLimit" : "UIPnlCampaign_JumpError_ZoneLevelLimit",
				ItemInfoUtility.GetAssetName(zoneId),
				Math.Max(1, zoneCfg.dungeonDifficulties[0].levelLimit));

		// 检查章节前置关卡
		if (string.IsNullOrEmpty(errorMsg) && !IsZonePreLimitOpen(zoneCfg.zoneId))
		{
			int zoneIndex = GetZoneIndexInCfg(zoneCfg.zoneId);
			int preZoneId = isActivityZone ? ConfigDatabase.DefaultCfg.CampaignConfig.secretZones[zoneIndex - 1].zoneId : ConfigDatabase.DefaultCfg.CampaignConfig.zones[zoneIndex - 1].zoneId;
			errorMsg = GameUtility.FormatUIString(
				isActivityZone ? "UIPnlCampaign_JumpError_ActivityZone" : "UIPnlCampaign_JumpError_Zone",
				ItemInfoUtility.GetAssetName(preZoneId));
		}

		return errorMsg;
	}

	public static string CheckDungeonEnterErrorMsg(int dungeonId, bool checkPreLimit)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		bool isActivityZone = ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(dungeonCfg.ZoneId);
		var errorMsg = CheckZoneEnterErrorMsg(dungeonCfg.ZoneId);

		if (string.IsNullOrEmpty(errorMsg))
		{
			// 检查章节的前置难度等级
			if (!IsDungeonPreDiffLimitOpen(dungeonId))
				errorMsg = GameUtility.FormatUIString(
					isActivityZone ? "UIPnlCampaign_JumpError_ActivityZoneDiff" : "UIPnlCampaign_JumpError_ZoneDiff",
					ItemInfoUtility.GetAssetName(dungeonCfg.ZoneId),
					_DungeonDifficulity.GetDisplayNameByType(dungeonCfg.DungeonDifficulty - 1, ConfigDatabase.DefaultCfg));

			// 检查关卡所在难度的等级
			if (!IsDungeonDiffLevelLimitOpen(dungeonId))
				errorMsg = GameUtility.FormatUIString(
					isActivityZone ? "UIPnlCampaign_JumpError_ActivityZoneDiffLevelLimit" : "UIPnlCampaign_JumpError_ZoneDiffLevelLimit",
					ItemInfoUtility.GetAssetName(dungeonCfg.ZoneId),
					_DungeonDifficulity.GetDisplayNameByType(dungeonCfg.DungeonDifficulty, ConfigDatabase.DefaultCfg),
					ConfigDatabase.DefaultCfg.CampaignConfig.GetZoneById(dungeonCfg.ZoneId).GetDungeonDifficultyByDifficulty(dungeonCfg.DungeonDifficulty).levelLimit);

			// 检查关卡前置关卡
			if (checkPreLimit && !IsDungeonPreLimitOpen(dungeonId))
				errorMsg = GameUtility.FormatUIString(
					"UIPnlCampaign_JumpError_Dungeon",
					ItemInfoUtility.GetAssetName(dungeonCfg.dungeonId));
		}

		return errorMsg;
	}

	public static void OpenCampaignView(int dungeonId)
	{
		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId);
		if (dungeonCfg == null)
		{
			Debug.Log("DungeonId is Invalid : " + dungeonId.ToString("X"));
			return;
		}

		OpenCampaignView(ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(dungeonCfg.ZoneId) ? _UIType.UI_ActivityDungeon : _UIType.UI_Dungeon, dungeonId);
	}

	// 进入副本（秘境或者历练）参数用于跳转
	public static void OpenCampaignView(int uiType, params object[] datas)
	{
		if (SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones == null || SysLocalDataBase.Inst.LocalPlayer.CampaignData.Zones.Count <= 0)
		{
			RequestMgr.Inst.Request(new QueryDungeonListReq(() =>
				{
					OpenCampaignView(uiType, datas);
				}));

			return;
		}

		string errorMsg = string.Empty;
		if (datas != null && datas.Length > 0)
			errorMsg = CheckDungeonEnterErrorMsg((int)datas[0], true);

		if (string.IsNullOrEmpty(errorMsg))
		{
			switch (uiType)
			{
				case _UIType.UI_Dungeon:
					EnterNormalDungeon(datas);
					break;

				case _UIType.UI_ActivityDungeon:
					EnterActivityDungeon(datas);
					break;
			}
		}
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), errorMsg);
	}

	public static void EnterNormalDungeon(params object[] datas)
	{
		if (SysGameStateMachine.Instance.CurrentState is GameState_Dungeon)
			(SysGameStateMachine.Instance.CurrentState as GameState_Dungeon).OnEnterScene(datas != null && datas.Length > 0 ? datas[0] : null);
		else
			SysGameStateMachine.Instance.EnterState<GameState_Dungeon>(datas != null && datas.Length > 0 ? datas[0] : null);
	}

	public static void EnterActivityDungeon(params object[] datas)
	{
		if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.Secret) ||
			ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Secret) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIMainScene_ActivityCampaignClose", ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Secret)));
			return;
		}

		if (SysGameStateMachine.Instance.CurrentState is GameState_ActivityDungeon)
			(SysGameStateMachine.Instance.CurrentState as GameState_ActivityDungeon).OnEnterScene(datas != null && datas.Length > 0 ? datas[0] : null);
		else
			SysGameStateMachine.Instance.EnterState<GameState_ActivityDungeon>(datas != null && datas.Length > 0 ? datas[0] : null);
	}
}
