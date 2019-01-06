using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class GameState_ActivityDungeon : GameStateBase
{
	private UIPnlCampaignBattleResult.CampaignBattleResultData battleData;
	private int jumpId;	 // ZoneId or DungeonId.

	public override bool IsGamingState { get { return true; } }

	public override void Create(object userData)
	{
		base.Create(userData);

		SetData(userData);
	}

	public override void Enter()
	{
		base.Enter();

		UIPnlLoading.ShowPanel(() =>
			{
				StartCoroutine("LoadingProgress");
			},
			null);
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_ActivityDungeon.OnEscape");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlCampaignSceneMid);
		else
			SysGameStateMachine.Instance.EnterState<GameState_CentralCity>();
	}

#endif

	private void SetData(object userData)
	{
		if (userData != null)
		{
			this.battleData = null;
			this.jumpId = IDSeg.InvalidId;

			if (userData is UIPnlCampaignBattleResult.CampaignBattleResultData)
				battleData = userData as UIPnlCampaignBattleResult.CampaignBattleResultData;
			else
				jumpId = (int)userData;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public IEnumerator LoadingProgress()
	{
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.activityCampaignSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		OnEnterScene(null);
	}

	public void OnEnterScene(object userData)
	{
		SetData(userData);

		// Show UI.
		UIPnlCampaignBase.CampaignRecrod campaignRecord = null;
		CampaignConfig.Dungeon jumpDungeonCfg = null;

		if (battleData != null)
			// zoneId, scrollPostion.
			campaignRecord = new UIPnlCampaignBase.CampaignRecrod(battleData.ZoneId, battleData.MapPosition, true);
		else if (jumpId != 0)
		{
			jumpDungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(jumpId);
			campaignRecord = new UIPnlCampaignBase.CampaignRecrod(jumpDungeonCfg == null ? jumpId : jumpDungeonCfg.ZoneId, jumpDungeonCfg == null ? IDSeg.InvalidId : jumpDungeonCfg.dungeonId);
		}
		else if (SysAssistant.Instance.CurrentTaskData != null && SysAssistant.Instance.CurrentTaskData.taskGuidCfg.TaskType == TaskConfig._TaskType.SecretDungeonAssistant)
		{
			jumpId = SysAssistant.Instance.CurrentTaskData.datas[0];
			campaignRecord = new UIPnlCampaignBase.CampaignRecrod(SysAssistant.Instance.CurrentTaskData.datas[0], IDSeg.InvalidId, SysAssistant.Instance.CurrentTaskData.datas[1], 0f);
		}

		// Scroll to Jump Id, or scroll	to first.
		int scrollIndex = jumpId != 0 ?
			CampaignData.GetZoneIndexInCfg(jumpDungeonCfg == null ? jumpId : jumpDungeonCfg.ZoneId) :
			0;

		// Hide Loading.
		UIPnlLoading.HidePanel(false);

		// Hide UI.
		SysUIEnv.Instance.HideUIModule(typeof(UIPnlCampaignActivityScene));

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignActivityScene), campaignRecord, scrollIndex);

		if (battleData != null && battleData.EnterUIType != _UIType.UnKonw)
		{
			SysUIEnv.Instance.ShowUIModule(battleData.EnterUIType);
		}
	}
}
