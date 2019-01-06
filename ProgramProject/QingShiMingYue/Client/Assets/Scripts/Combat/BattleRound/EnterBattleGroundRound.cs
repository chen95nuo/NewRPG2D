using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

//服务器下发的round，用来控制avatar跑到预订的战斗地点
public class EnterBattleGroundRound : CombatRound
{
	private float roundTime = 0;

	public EnterBattleGroundRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{

	}

	public override bool CanStartAfterRound()
	{
		if (RoundState == _RoundState.NotStarted)
			return false;

		foreach (var turnRecord in RoundRecord.TurnRecords)
		{
			BattleRole avatar = BattleRecordPlayer.BattleRoles[turnRecord.AvatarIndex];

			if (avatar.IsMoving)
				return false;
		}

		return roundTime >= 0.5f;
	}

	public override void Update()
	{
		if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
		{
			roundTime += Time.deltaTime;
		}

		base.Update();
	}

	public override void Finish()
	{
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			role.Foothold = role.Position;
		}

		GameState_Battle curGameState = SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>();

		if (curGameState.CombatResultAndReward.IsPlotBattle || battleRecordPlayer.IsSkip)
		{
			base.Finish();
			return;
		}

		//角色进场后，副本历练恢复到上次的视角，不打断战斗---------------------------------------------------------------------------------------------------------------
		//除剧情战斗外目前一定会有此Round
		if (battleRecordPlayer.FirstBattle)
		{
			var recordedSceneAngle = BattleSceneAngleData.Instance.GetBattleSceneAngleByBattleType(curGameState.BattleType);
			BattleScene bs = BattleScene.GetBattleScene();

			float camAngleH = 0;

			//如果该种战斗模式支持记录战斗视角
			if (recordedSceneAngle != null)
				camAngleH = recordedSceneAngle.AngleH;
			//战斗开始后转到默认视角
			else camAngleH = ConfigDatabase.DefaultCfg.GameConfig.combatSetting.battleDefault.GetBattleDefaultItemByCombatType(curGameState.BattleType).sceneCamAngleHDefault;

			//如果玩家没有手动拖动屏幕
			if (!bs.BattleCameraCtrl.IsAutoRotating && !bs.BattleCameraCtrl.IsEasingRotating)
				BattleScene.GetBattleScene().BattleCameraCtrl.RotateTo(0.7f, camAngleH, 0, EZAnimation.EASING_TYPE.Default, EZAnimation.EASING_TYPE.Default, true);
		}

		base.Finish();
	}
}
