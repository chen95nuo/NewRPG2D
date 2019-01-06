using System;
using System.Collections.Generic;
using KodGames.ClientClass;
using ClientServerCommon;
using UnityEngine;

// 服务器逻辑round基类 实现通用功能
// 1，是否可以开始下一个round
// 2，当前round是否可以结束
// 3，顺序播放动画
public abstract class CombatRound : BattleRound
{
	public override string LogName
	{
		get
		{
			return string.Format("{0} teamIndex({1}) rowIndex({2}) turnCount({3})",
				this.GetType(), RoundRecord.TeamIndex, RoundRecord.RowIndex, RoundRecord.TurnRecords.Count);
		}
	}

	public CombatRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(battleRecordPlayer)
	{
		this.roundRecord = roundRecord;
        //ActionRecord actionRecord = roundRecord.TurnRecords[0].ActionRecords[0];
        //Debug.Log(actionRecord.GetHashCode()+"***********************" + actionRecord.EventRecords.Count);
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEnv != null && roundRecord.RoundIndex != 0)
		{
			uiEnv.GetUIModule<UIPnlBattleBar>().UpdateRoundIndex(roundRecord.RoundIndex, battleRecordPlayer.BattleRecord.MaxRecordCount);
		}

		foreach (var turnRecord in roundRecord.TurnRecords)
		{
			if (turnRecord.AvatarIndex < 0)
			{
				foreach (var actionRecord in turnRecord.ActionRecords)
				{
					BattleRecordPlayer.PlayActionRecord(actionRecord);
				}
			}
			else
			{
				turnRecord.CurrentActionIndex = 0;
				if (turnRecord.CurrentActionIndex < turnRecord.ActionRecords.Count)
				{
                    ActionRecord actionRecord = turnRecord.ActionRecords[turnRecord.CurrentActionIndex++];
                    //Debug.Log("&&&&&&"+actionRecord.GetHashCode());
                    BattleRecordPlayer.PlayActionRecord(actionRecord);
				}
			}
		}

		return true;
	}

	public override void Update()
	{
		base.Update();

		if (RoundState != _RoundState.Running)
			return;

		foreach (var turnRecord in roundRecord.TurnRecords)
		{
			if (turnRecord.AvatarIndex >= 0)
			{
				BattleRole avatar = BattleRecordPlayer.BattleRoles[turnRecord.AvatarIndex];

				if (turnRecord.CurrentActionIndex >= turnRecord.ActionRecords.Count)
					continue;

				if (avatar.ActionState != BattleRole._ActionState.Idle)
					continue;

				ActionRecord actionRecord = turnRecord.ActionRecords[turnRecord.CurrentActionIndex];
				BattleRole actionSrcAvatar = BattleRecordPlayer.BattleRoles[actionRecord.SrcAvatarIndex];

				if (actionSrcAvatar.ActionState != BattleRole._ActionState.Idle)
					continue;

				if (turnRecord.CurrentActionIndex < turnRecord.ActionRecords.Count)
				{
					BattleRecordPlayer.PlayActionRecord(turnRecord.ActionRecords[turnRecord.CurrentActionIndex++]);
				}
			}
		}
	}

	public override bool CanStartAfterRound()
	{
		if (base.CanStartAfterRound() == false)
		{
			return false;
		}

		foreach (var turnRecord in roundRecord.TurnRecords)
		{
			if (turnRecord.AvatarIndex < 0)
				continue;

			if (turnRecord.CurrentActionIndex < turnRecord.ActionRecords.Count && !BattleRecordPlayer.IsSkip)
				return false;
		}

		return true;
	}

	protected override bool CheckFinishState()
	{
		if (base.CheckFinishState() == false)
			return false;

		foreach (var turnRecord in roundRecord.TurnRecords)
		{
			if (turnRecord.AvatarIndex < 0)
				continue;

			BattleRole avatar = BattleRecordPlayer.BattleRoles[turnRecord.AvatarIndex];

			if (turnRecord.CurrentActionIndex < turnRecord.ActionRecords.Count)
				return false;

			if (avatar.ActionState != BattleRole._ActionState.Idle)
				return false;
		}

		return true;
	}
}
