using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class GameState_Dungeon : GameStateBase
{
	private UIPnlCampaignBattleResult.CampaignBattleResultData battleData;
	private int jumpId;		// ZoneId or DungeonId.

	public override bool IsGamingState { get { return true; } }

	public override void Create(object userData)
	{
		base.Create(userData);

		SetData(userData);
	}

	public override void Enter()
	{
		UIPnlLoading.ShowPanel(() =>
			{
				StartCoroutine("LoadingProgress");
			},
			null);
	}

#if UNITY_ANDROID

	public override void OnEscape()
	{
		Debug.Log("GameState_Dungeon.OnEscape");

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlCampaignSceneMid))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlCampaignSceneMid);
		else if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlChatTab))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlChatTab);
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
				this.battleData = userData as UIPnlCampaignBattleResult.CampaignBattleResultData;
			else
				this.jumpId = (int)userData;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadingProgress()
	{
		// Load dungeon scene
		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(ConfigDatabase.DefaultCfg.SceneConfig.campaignSceneId);
		if (SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName) == false)
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);

		OnEnterScene(null);
	}

	public void OnEnterScene(object userData)
	{
		SetData(userData);

		StopCoroutine("OnEnterCampaignScene");
		StartCoroutine("OnEnterCampaignScene");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator OnEnterCampaignScene()
	{
		// Show UI
		UIPnlCampaignBase.CampaignRecrod campaignRecord = null;
		CampaignConfig.Dungeon jumpDungeonCfg = null;

		if (battleData != null)
			campaignRecord = new UIPnlCampaignBase.CampaignRecrod(battleData.ZoneId, battleData.MapPosition, true);
		else if (jumpId != 0)
		{
			jumpDungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(jumpId);
			campaignRecord = new UIPnlCampaignBase.CampaignRecrod(jumpDungeonCfg == null ? jumpId : jumpDungeonCfg.ZoneId, jumpDungeonCfg == null ? IDSeg.InvalidId : jumpDungeonCfg.dungeonId);
		}
		else if (SysAssistant.Instance.CurrentTaskData != null)
		{
			switch (SysAssistant.Instance.CurrentTaskData.taskGuidCfg.TaskType)
			{
				case TaskConfig._TaskType.DungeonCanCombatAssistant:
					jumpId = SysAssistant.Instance.CurrentTaskData.datas[1];
					jumpDungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(SysAssistant.Instance.CurrentTaskData.datas[1]);
					campaignRecord = new UIPnlCampaignBase.CampaignRecrod(SysAssistant.Instance.CurrentTaskData.datas[0], SysAssistant.Instance.CurrentTaskData.datas[1]);
					break;
				case TaskConfig._TaskType.DungeonStarRewardAssistant:
					jumpId = SysAssistant.Instance.CurrentTaskData.datas[0];
					campaignRecord = new UIPnlCampaignBase.CampaignRecrod(SysAssistant.Instance.CurrentTaskData.datas[0], IDSeg.InvalidId, SysAssistant.Instance.CurrentTaskData.datas[1]);
					break;
			}
		}

		// Scroll to the last normal campaign , if last battle zone is normal campaign zone.
		int scrollIndex = jumpId != 0 ?
			CampaignData.GetZoneIndexInCfg(jumpDungeonCfg == null ? jumpId : jumpDungeonCfg.ZoneId) :
			CampaignData.GetLastNormalZoneIndex();

		var capaignData = CampaignData.GetCampaignData();
		CampaignSceneData.Instance.ScrollView(
							capaignData.shouldUnlockIndex < 0 ? scrollIndex : capaignData.shouldUnlockIndex,
							0,
							false,
							null);

		// Hide loading
		UIPnlLoading.HidePanel(false);

		// Hide UI.
		SysUIEnv.Instance.HideUIModule(typeof(UIPnlCampaignScene));
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignScene), campaignRecord);

		if (battleData != null && battleData.EnterUIType != _UIType.UnKonw)
		{
			SysUIEnv.Instance.ShowUIModule(battleData.EnterUIType);
		}

		while (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlLoading)))
			yield return null;

		SysUIEnv.Instance.GetUIModule<UIPnlCampaignScene>().FillData(campaignRecord);		
	}
}