using System;
using System.Collections.Generic;
using UnityEngine;
using KodGames.ClientClass;

// 纯客户端逻辑round，处理等待进入下一场战斗
public class WaitForNextBattleRound : BattleRound
{
	private float roundTime = 0;
	private bool dontWait = false;

	public WaitForNextBattleRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	public WaitForNextBattleRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound, bool dontWait)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
		this.dontWait = dontWait;
	}


	public override bool CanStartAfterRound()
	{
		if (dontWait || roundTime >= 1f)
		{
			return base.CanStartAfterRound();
		}
		else
		{
			return false;
		}
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		// Show next battle UI
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			UIPnlBattleBar battleBar = uiEnv.GetUIModule<UIPnlBattleBar>();
			battleBar.HideAll();

			//if (!battleRecordPlayer.BattleScene.melaleucaFloorBattle)
			//    battleBar.HideContinueButton(false);
		}

		return true;
	}

	public override void Finish()
	{
		// Show battle bar
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null)
		{
			UIPnlBattleBar battleBar = uiEnv.GetUIModule<UIPnlBattleBar>();
			battleBar.HideAll();
			uiEnv.GetUIModule<UIPnlBattleRoleInfo>().SetCanShowRoleInfo(false);
			uiEnv.GetUIModule<UIPnlBattleRoleInfo>().Hide();
		}

		base.Finish();
		battleRecordPlayer.IsFinished = true;
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
