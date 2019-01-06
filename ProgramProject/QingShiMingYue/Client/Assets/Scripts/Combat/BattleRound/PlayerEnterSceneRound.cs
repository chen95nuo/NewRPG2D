using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 纯客户端逻辑round，处理玩家进入场景，主要功能是提供角色随机进场效果
public class PlayerEnterSceneRound : BattleRound
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

	private int teamIndex = 0;
	private bool userSceneCamera = false;
	private bool playEnterFx = false;
	private bool sponsorPlayEnterMove = false;

	private float roundTimer;
	private List<DelayAction> delayedActingRoles = new List<DelayAction>();

	private bool IsSponsorEnter
	{
		get { return teamIndex == BattleRecordPlayer.SponsorTeamIndex; }
	}

	public PlayerEnterSceneRound(BattleRecordPlayer battleRecordPlayer, BattleRound after, int teamIndex, bool userSceneCamera, bool playEnterFx, bool sponsorPlayEnterMove)
		: base(battleRecordPlayer)
	{
		this.AfterRound = after;
		this.teamIndex = teamIndex;
		this.userSceneCamera = userSceneCamera;
		this.playEnterFx = playEnterFx;
		this.sponsorPlayEnterMove = sponsorPlayEnterMove;
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			// 只处理制定队伍角色
			if (role.TeamIndex != this.teamIndex)
				continue;

			// 死亡的角色不显示
			if (role.IsDead())
			{
				role.Hide = true;
				continue;
			}

			ActingRoles.Add(role);
		}

		return this;
	}

	public override bool CanStartAfterRound()
	{
		if (RoundState == _RoundState.NotStarted)
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

		// 使用场景预设的入场摄像机
		if (userSceneCamera)
		{
			var cameraController = BattleRecordPlayer.BattleScene.mainCamera.GetComponent<CameraController_Battle>();
			if (cameraController != null)
				cameraController.ExternalControl = true;

			BattleRecordPlayer.BattleScene.SetTraceCameraPos();
		}

		BattleRecordPlayer.BattleScene.RandomPositionOffset();
		foreach (var role in ActingRoles)
		{
			// 如果挑战者需要进场移动效果, 根据初始化位置获取相应位置朝向信息
			Vector3 forward;
			if (IsSponsorEnter && sponsorPlayEnterMove)
				forward = BattleRecordPlayer.BattleScene.GetInitForward();
			else
				forward = BattleRecordPlayer.BattleScene.GetTeamForward(BattleRecordPlayer.BattleIndex, role.TeamIndex);

			// 获取初始化位置
			Vector3 position;
			if (IsSponsorEnter && sponsorPlayEnterMove)
				position = BattleRecordPlayer.BattleScene.GetInitPosition(role.GetBattlePositionRow(), role.GetBattlePositionColumn());
			else
				position = BattleRecordPlayer.BattleScene.GetStartPosition(battleRecordPlayer.BattleIndex, role.TeamIndex, role.GetBattlePositionRow(), role.GetBattlePositionColumn());

			role.Avatar.CachedTransform.position = position;
			role.Avatar.CachedTransform.forward = forward;

			// 进场特效需要延迟显示角色，使角色先后随机出现
			if (playEnterFx)
				delayedActingRoles.Add(new DelayAction(role, Random.Range(0, BattleRecordPlayer.BattleScene.maxEnterSceneDelayTime), false));
			else
			{
				// 角色初始化的时候应该是默认隐藏的, 直接显示角色
				role.Hide = false;

				// 这里先不显示血条, 当真正开始战斗的时候才显示
				role.BattleBar.Hide(true);

			}
		}

		// 如果是挑战者, 将摄像机trace到这个player
		if (IsSponsorEnter)
			SetCameraTrace(BattleRecordPlayer.BattleScene.cameraNormalDistance, BattleRecordPlayer.BattleScene.cameraInterpolateDuration);
		
		AvatarActOrderUtil.ShowAvatarActOrderBatch(ActingRoles);
		return true;
	}

	public override void Finish()
	{
		ClearCameraTrace();
		base.Finish();
	}

	public override void Update()
	{
		base.Update();

		if (RoundState == _RoundState.Running || RoundState == _RoundState.Finished)
		{
			roundTimer += Time.deltaTime;

			// 处理延迟显示
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

	public override void OnDrawGizmos()
	{
		Gizmos.DrawSphere(TracingPosition, 1f);
	}

	private void DoEnterSceneAction(BattleRole role)
	{
		// 使用EnterScene action显示角色, 这个动作会先播放动画同时产生显示角色的event
		AvatarAction actionCfg = role.GetActionByType(AvatarAction._Type.EnterScene);

		KodGames.ClientClass.ActionRecord actionRecord = new KodGames.ClientClass.ActionRecord();
		actionRecord.ActionId = actionCfg.id;
		actionRecord.SrcAvatarIndex = role.AvatarIndex;
		actionRecord.TargetAvatarIndices.Add(role.AvatarIndex);

		BattleRecordPlayer.PlayActionRecord(actionRecord);

		// 这里先不显示血条, 当真正开始战斗的时候才显示
		role.BattleBar.Hide(true);
	}
}
