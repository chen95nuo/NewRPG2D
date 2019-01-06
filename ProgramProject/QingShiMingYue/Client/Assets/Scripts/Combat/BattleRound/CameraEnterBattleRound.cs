using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 战斗前摄像机拉近round，纯客户端逻辑round，与服务器无关
// 正式战斗前推进摄像机
public class CameraEnterBattleRound : BattleRound
{
	private float roundTime = 0;

	public CameraEnterBattleRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	public CameraEnterBattleRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		// Initialize actor list
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (role.TeamIndex != BattleRecordPlayer.SponsorTeamIndex)
				continue;

			ActingRoles.Add(role);
		}

		return this;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		// Camera focus to battle distance
		SetCameraTrace(BattleRecordPlayer.BattleScene.cameraCombatDistance, BattleRecordPlayer.BattleScene.cameraInterpolateDuration);

		return true;
	}

	public override bool CanStartAfterRound()
	{
		if (RoundState == _RoundState.NotStarted)
			return false;

		return roundTime >= BattleRecordPlayer.BattleScene.cameraInterpolateDuration;
	}

	public override void Finish()
	{
		ClearCameraTrace();

		//第一场战斗后玩家才能拖动屏幕. 在GameState_Battle中设置为True;剧情战斗锁屏
		if (!SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().CombatResultAndReward.IsPlotBattle)
			BattleScene.GetBattleScene().BattleCameraCtrl.LockTouch = false;
		
		base.Finish();
	}

	public override void Update()
	{
		base.Update();

		if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
		{
			roundTime += Time.deltaTime;
		}
	}
}
