using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 对手角色进入场景, 可以设定是直接显示还是, 延迟播放特效之后出现.
public class OpponentEnterSceneRound : BattleRound
{
	private class DelayAction
	{
		public BattleRole battleRole;
		public float delayTime;
		public bool processed;

		public DelayAction(BattleRole battleRole, float delayTime, bool processed)
		{
			this.battleRole = battleRole;
			this.delayTime = delayTime;
			this.processed = processed;
		}
	}

	private float roundTimer;
	private List<DelayAction> delayedActingRoles = new List<DelayAction>();

	public OpponentEnterSceneRound(BattleRecordPlayer battleRecordPlayer)
		: base(battleRecordPlayer)
	{
	}

	public OpponentEnterSceneRound(BattleRecordPlayer battleRecordPlayer, BattleRound after)
		: base(battleRecordPlayer)
	{
		this.AfterRound = after;
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		// Initialize actor list
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (role.TeamIndex == BattleRecordPlayer.SponsorTeamIndex)
				continue;

			ActingRoles.Add(role);
		}

		return this;
	}

	public override bool CanStartAfterRound()
	{
		if (!base.CanStartAfterRound())
			return false;

		foreach (var role in delayedActingRoles)
			if (role.processed == false || role.battleRole.ActionState != BattleRole._ActionState.Idle)
				return false;

		return true;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		BattleRecordPlayer.BattleScene.RandomPositionOffset();

		//只在第一场战斗旋转相机。
		if (BattleRecordPlayer.BattleScene.NeedCameraTrace() && BattleRecordPlayer.FirstBattle)
		{
			if (BattleRecordPlayer.BattleScene.mainCamera.GetComponent<CameraController_Battle>() != null)
				BattleRecordPlayer.BattleScene.mainCamera.GetComponent<CameraController_Battle>().ExternalControl = true;

			BattleRecordPlayer.BattleScene.SetTraceCameraPos();
		}

		foreach (var role in ActingRoles)
		{
			// Initialize position
			Vector3 forward = BattleRecordPlayer.BattleScene.GetTeamForward(BattleRecordPlayer.BattleIndex, role.TeamIndex);
			Vector3 position = BattleRecordPlayer.BattleScene.GetStartPosition(BattleRecordPlayer.BattleIndex, role.TeamIndex, role.GetBattlePositionRow(), role.GetBattlePositionColumn());
			role.Avatar.CachedTransform.position = position;
			role.Avatar.CachedTransform.forward = forward;
			if (battleRecordPlayer.BattleScene.isOpponentEnter)
			{
				// Add delay action
				delayedActingRoles.Add(new DelayAction(role, Random.Range(0, BattleRecordPlayer.BattleScene.maxEnterSceneDelayTime), false));
			}
			else
			{
				// Show player
				role.Hide = false;

				// Hide battle bar
				role.BattleBar.Hide(true);
			}
		}
		AvatarActOrderUtil.ShowAvatarActOrderBatch(ActingRoles);
		return true;
	}

	public override void Update()
	{
		base.Update();

		if (!battleRecordPlayer.BattleScene.isOpponentEnter)
		{
			return;
		}

		if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
		{
			roundTimer += Time.deltaTime;

			foreach (var role in delayedActingRoles)
			{
				if (role.processed == false && roundTimer >= role.delayTime)
				{
					DoEnterSceneAction(role.battleRole);
					role.processed = true;
				}
			}
		}
	}

	private void DoEnterSceneAction(BattleRole role)
	{
		// Play enter scene action
		AvatarAction actionCfg = role.GetActionByType(AvatarAction._Type.EnterScene);

		KodGames.ClientClass.ActionRecord actionRecord = new KodGames.ClientClass.ActionRecord();
		actionRecord.ActionId = actionCfg.id;
		actionRecord.SrcAvatarIndex = role.AvatarIndex;
		actionRecord.TargetAvatarIndices.Add(role.AvatarIndex);

		BattleRecordPlayer.PlayActionRecord(actionRecord);

		// Hide battle bar
		role.BattleBar.Hide(true);
	}
}
