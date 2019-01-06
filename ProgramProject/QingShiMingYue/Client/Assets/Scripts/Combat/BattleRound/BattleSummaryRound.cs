using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

// 战斗结算round，纯客户端逻辑round，与服务器无关
// 控制战斗结算面板的现实和隐藏
public class BattleSummaryRound : BattleRound
{
	private const float delayTime = 1f;
	private float time = 0f;

	public BattleSummaryRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	public override bool Start()
	{
		battleRecordPlayer.ResetScaleTime();

		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			UIPnlBattleBar battleBar = uiEnv.GetUIModule<UIPnlBattleBar>();
			battleBar.HideAll();
			uiEnv.GetUIModule<UIPnlBattleRoleInfo>().SetCanShowRoleInfo(false);
			uiEnv.GetUIModule<UIPnlBattleRoleInfo>().Hide();
		}

		return base.Start();
	}

	public override void Update()
	{
		base.Update();

		if (RoundState == _RoundState.Running)
		{
			time += Time.deltaTime;
		}
	}

	public override void Finish()
	{

		if (KodGames.Camera.main != null)
		{
			CameraController_Battle cct = KodGames.Camera.main.GetComponent<CameraController_Battle>();
			if (cct != null)
			{
				//clear CurrentBattleRoles in CameraController_Touch
				cct.WatchData = null;
			}
		}

		SysFx sysFx = SysModuleManager.Instance.GetSysModule<SysFx>();
		if (sysFx != null)
		{
			sysFx.ResumeTimeScale();
		}

		GameState_Battle battleState = SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>();

		//秘境战斗记录玩家转动的视角，下次战斗/回放战斗时恢复视角
		var localSceneAngle = BattleSceneAngleData.Instance.GetBattleSceneAngleByBattleType(battleState.BattleType);
		if (localSceneAngle != null)
		{
			localSceneAngle.AngleH = BattleScene.GetBattleScene().BattleCameraCtrl.DeltaAngleH;
			BattleSceneAngleData.Instance.Put(battleState.BattleType, localSceneAngle);
		}
		UIPnlCampaignBattleResult campaignBattleResult = SysUIEnv.Instance.GetUIModule<UIPnlCampaignBattleResult>();
		if (
			//剧情战斗没有Delay
			(battleState.CombatResultAndReward != null && battleState.CombatResultAndReward.IsPlotBattle)
			|| (time > delayTime)
		)
		{
			//Set RoundState to Finished.
			base.Finish();

			var battleResultData = battleState.BattleResultData;

			//剧情战斗没有结算面板 返回进入战斗之前的界面
			if (battleState.CombatResultAndReward.IsPlotBattle)
			{
				switch (battleResultData.BattleResultUIType)
				{
					case _UIType.UIPnlTowerBattleResult:
					case _UIType.UIPnlTowerSweepBattle:
						SysGameStateMachine.Instance.EnterState<GameState_Tower>();
						break;
					case _UIType.UIPnlArenaBattleResult:
						SysGameStateMachine.Instance.EnterState<GameState_CentralCity>(new UserData_ShowUI(_UIType.UIPnlArena));
						break;
					case _UIType.UIPnlCampaignBattleResult:
						// Record InterrupteCampaing.
						SysLocalDataBase.Inst.LocalPlayer.CampaignData.InterruptCampaign = false;

						var campaignBattleResultData = battleResultData as UIPnlCampaignBattleResult.CampaignBattleResultData;

						if (ConfigDatabase.DefaultCfg.CampaignConfig.IsActivityZoneId(campaignBattleResultData.ZoneId))
							SysGameStateMachine.Instance.EnterState<GameState_ActivityDungeon>(battleResultData);
						else
						{
							//轮回梦境（剧情战斗）如果处在强制新手引导中，最后一关直接回主城
							if (campaignBattleResultData.DungeonId == 150995202 &&// "09000102"
								(SysLocalDataBase.Inst.LocalPlayer.UnDoneTutorials.Count > 0
									&& SysLocalDataBase.Inst.LocalPlayer.UnDoneTutorials.Contains(ConfigDatabase.DefaultCfg.GameConfig.initAvatarConfig.forceTutorialEndId) == true))
							{
								SysGameStateMachine.Instance.EnterState<GameState_CentralCity>();
							}
							else
								SysGameStateMachine.Instance.EnterState<GameState_Dungeon>(battleResultData);
						}
						break;

				}
				campaignBattleResult.ActiveButton();
			}
			else
			{
				SysUIEnv.Instance.ShowUIModule(battleState.BattleResultData.BattleResultUIType, battleState.BattleResultData);

				if (SysLocalDataBase.Inst.LocalPlayer.PlayerLevelUpData != null)
					campaignBattleResult.DisabledButton();

				//回放战斗时，不会重新加载战斗场景，CameraController中的targetDistance会缓存上一次战斗最后的距离，导致开始时相机目标距离不正确出现运动轨迹异常。
				//在战斗的最后，重置相机targetDistance为0.
				//CameraController_Battle有最小距离，当targetDistance==0时，minDistance起作用。
				BattleScene.GetBattleScene().BattleCameraCtrl.SetTraceTarget(this, 0, 0, EZAnimation.EASING_TYPE.Default);
			}
		}
		else
			campaignBattleResult.ActiveButton();
	}


}
