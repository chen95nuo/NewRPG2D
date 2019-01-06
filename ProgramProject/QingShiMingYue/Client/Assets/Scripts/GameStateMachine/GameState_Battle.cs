using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

class GameState_Battle : GameStateBase
{
	private KodGames.ClientClass.CombatResultAndReward combatResultAndReward;
	public KodGames.ClientClass.CombatResultAndReward CombatResultAndReward { get { return combatResultAndReward; } }

	private BattleRecordPlayer recordPlayer = new BattleRecordPlayer();
	public BattleRecordPlayer BattleRecordPlayer { get { return recordPlayer; } }

	private UIPnlBattleResultBase.BattleResultData battleResultData;
	public UIPnlBattleResultBase.BattleResultData BattleResultData { get { return battleResultData; } }

	private int currentBattleIndex = 0;
	public int CurrentBattleIndex { get { return currentBattleIndex; } }

	public override bool IsGamingState { get { return true; } }

	public int BattleType
	{
		get
		{
			if (CombatResultAndReward.IsPlotBattle)
				return _CombatType.Tutorial;

			if (battleResultData == null)
			{
				Debug.LogError("[GameState_Battle][GetBattleType] battleResultData is null.");
				return _CombatType.Unknown;
			}

#if UNITY_EDITOR
			//BattleTest Support
			if (battleResultData is BattleTest.BattleTestBattleResultData)
			{
				return (battleResultData as BattleTest.BattleTestBattleResultData).BattleTestCombatType;
			}
#endif

			switch (battleResultData.BattleResultUIType)
			{
				case _UIType.UIPnlCampaignBattleResult:
					if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId((battleResultData as UIPnlCampaignBattleResult.CampaignBattleResultData).ZoneId))
						return _CombatType.ActivityCampaign;
					else
						return _CombatType.Campaign;

				case _UIType.UIPnlTowerBattleResult://正常攻打结算界面
				case _UIType.UIPnlTowerSweepBattle://扫荡结算界面
					return _CombatType.Tower;

				case _UIType.UIPnlCombatFriendBattleResult:
					return _CombatType.CombatFriend;

				case _UIType.UIPnlArenaBattleResult:
					return _CombatType.Arena;

				case _UIType.UIPnlWolfBattleResult:
					return _CombatType.WolfSmoke;

				case _UIType.UIPnlAdventureCombatResult:
					return _CombatType.Adventure;

				case _UIType.UIPnlFriendCampaginBattleResult:
					return _CombatType.FriendCampaign;

				case _UIType.UIPnlGuildPointMonsterBattleResult:
					return _CombatType.GuildMonster;

				case _UIType.UIPnlGuildPointBossBattleResult:
					return _CombatType.GuildBoss;

				default:
					Debug.LogError("[GameState_Battle][GetBattleType] Unknown BattleResultUIType. UIType=" + _UIType.GetDisplayNameByType(battleResultData.BattleResultUIType, ConfigDatabase.DefaultCfg));
					return _CombatType.Unknown;
			}
		}
	}

	void LogBattleAnaly()
	{
		BattleDataAnalyser analyser = new BattleDataAnalyser(this.combatResultAndReward);
		System.Text.StringBuilder sb = new System.Text.StringBuilder();

		for (int battleIdx = 0; battleIdx < analyser.BattleCount; battleIdx++)
		{
			sb.Append("战斗" + battleIdx + '\n');
			List<int> ids = analyser.GetAvatarIdxes(0, battleIdx);

			foreach (int avatarIdx in ids)
			{
				sb.Append("\navatar index=" + avatarIdx);
				int avatarResID = analyser.GetAvatarResourceIDByIndex(avatarIdx, battleIdx);
				sb.Append("   name=" + ItemInfoUtility.GetAssetName(avatarResID));
				sb.Append("  ResourceID=" + avatarResID);
				sb.Append("  LeftHP=" + analyser.GetAvatarLeftHP(avatarIdx, battleIdx));
				sb.Append("  LeftMaxHP=" + analyser.GetAvatarLeftMaxHP(avatarIdx, battleIdx));
			}
			sb.Append("\n");
		}
		Debug.Log(sb.ToString());
	}

	public override void Create(object userData)
	{
		base.Create(userData);

		List<object> parameters = userData as List<object>;
		this.combatResultAndReward = parameters[0] as KodGames.ClientClass.CombatResultAndReward;

//#if UNITY_EDITOR
//        BattleDataAnalyser analyser = new BattleDataAnalyser(this.combatResultAndReward);
//        analyser.LogFirstBattleAllAvatarAttributes();
//#endif

		//使用BattleTest时没有parameters[1]
		if (parameters.Count > 1)
		{
			battleResultData = parameters[1] as UIPnlBattleResultBase.BattleResultData;

			recordPlayer.CanSkip = battleResultData.CanSkipBattle();
			recordPlayer.SkipDelayTime = battleResultData.GetSkipBattleTime();
		}

	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_Battle.OnEscape");

		if (SysUIEnv.Instance.IsUIModuleShown(battleResultData.BattleResultUIType))
		{
			UIPnlBattleResultBase battleResult = SysUIEnv.Instance.GetUIModule(battleResultData.BattleResultUIType) as UIPnlBattleResultBase;

			if (battleResult != null)
				battleResult.DoOnClose();
		}
	}

#endif

	public override void Enter()
	{
		// 清空界面event事件，防止升级界面直接弹出
		SysUIEnv.Instance.ClearShowEventsList();

		UIPnlLoading.ShowPanel(() =>
			{
				currentBattleIndex = 0;
				StopCoroutine("LoadingStep");
				StartCoroutine("LoadingStep");
			},
			null);

		// Talking Game Battle.
		if (battleResultData != null && battleResultData.BattleResultUIType != _UIType.UIPnlTowerSweepBattle)
			GameAnalyticsUtility.OnEventBattle(battleResultData.CombatType, battleResultData, combatResultAndReward);
	}

	public override void Exit()
	{
		recordPlayer.ResetScaleTime();
	}

	public override void OnUpdate()
	{
		base.OnUpdate();

		if (recordPlayer.IsFinished && !recordPlayer.IsSkip)
		{
			PlayNextBattle();
		}
		else
		{
			recordPlayer.Update();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadingStep()
	{
		// Load scene
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(combatResultAndReward.BattleRecords[0].SceneId);

		if (SysSceneManager.Instance != null)
			if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
				yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		//Show BattleScene.
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlBattleScene));

		// Set battle record
		PlayNextBattle();

		UIPnlLoading.HidePanel(false);
	}

	private void PlayBattle(int battleIndex)
	{
		// Create record player
		recordPlayer.Initialize();

		if (battleResultData != null && battleResultData.BattleResultUIType == _UIType.UIPnlTutorialBattleResult)
		{
			recordPlayer.IsTutorial = true;
			recordPlayer.ScaleTime(ConfigDatabase.DefaultCfg.TutorialConfig.tutorialCombat.speed);
		}
		else
		{
			if (SysLocalDataBase.Inst != null)
				recordPlayer.ScaleTime(SysLocalDataBase.Inst.GetBattleSpeed());
		}
		//一场战斗数据  battleIndex 表示场次战斗Index
		recordPlayer.InitializeBattle(combatResultAndReward.BattleRecords[battleIndex], 0, battleIndex, battleIndex == combatResultAndReward.BattleRecords.Count - 1);

		if (BattleType == _CombatType.Tutorial)
			recordPlayer.BuildPlayerForCustomBattle();
		else
			recordPlayer.BuildPlayer();

		recordPlayer.LoadResource();

		//记录视角相关。
		//在BattleStartRound结束前不能拖动场景。不能依赖ExternalControl，在CameraEnterBattleRound中设为false。
		//注意类似千机楼这种在原地刷怪的战斗只有第一波有CameraEnterBattleRound。
		if (!CombatResultAndReward.IsPlotBattle && BattleType != _CombatType.Tower)
			BattleScene.GetBattleScene().BattleCameraCtrl.LockTouch = true;

		recordPlayer.Start();
	}

	public void ReplayBattle()
	{
		if (recordPlayer == null)
		{
			return;
		}
		recordPlayer.DestroyAllBattleRole();

		recordPlayer = new BattleRecordPlayer();
		recordPlayer.Initialize();
		recordPlayer.CanSkip = true;
		recordPlayer.SkipDelayTime = 0;
		currentBattleIndex = 0;
		PlayNextBattle();

		string currentMusic = ClientServerCommon.ConfigDatabase.DefaultCfg.SceneConfig.GetBgMusicBySceneName(Application.loadedLevelName);
		if (!AudioManager.Instance.IsMusicPlaying(currentMusic))
		{
			AudioManager.Instance.StopMusic(0);
			AudioManager.Instance.PlayMusic(currentMusic, true, 0, 0);
		}

		BattleScene bs = BattleScene.GetBattleScene();
		//bugfix:千机楼，第二次重复播放时不能拖动场景
		bs.BattleCameraCtrl.AddInputDelegate();
		//回放战斗时重置视角
		bs.BattleCameraCtrl.ResetDeltaAngles();
	}

	public void PlayNextBattle()
	{
		PlayBattle(currentBattleIndex++);
	}

	public void SkipBattle()
	{
		recordPlayer.IsSkip = true;
	}

#if UNITY_EDITOR

	[ContextMenu("SaveBattleRcdBinaryData")]
	public void SaveBattleRecordToFile()
	{
		string fileDestFullName = Application.dataPath + @"\BattleRecord.bytes";
		System.IO.FileStream stream = new System.IO.FileStream(fileDestFullName, System.IO.FileMode.Create);
		System.IO.MemoryStream memStream = new System.IO.MemoryStream();
		try
		{
			ProtoBuf.Serializer.NonGeneric.Serialize(memStream, CombatResultAndReward.ToProtobufWithOnlyBattleData());
			stream.Write(memStream.GetBuffer(), 0, (int)memStream.Length);
			Debug.Log("BattleDataSaved at " + fileDestFullName);
		}
		finally
		{
			stream.Close();
			memStream.Close();
		}
	}

#endif
}
