using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;
// 纯客户端逻辑round，处理战斗发起者跑向战斗对手。
public class SponsorGoToOpponentRound : BattleRound
{
	public SponsorGoToOpponentRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	List<int> sponsorIndexes;

	//跑动后停下来之后是否需要在做进场动作（向前跑一步）
	bool runToBattlePosition = false;

	public SponsorGoToOpponentRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		return this;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		//剧情战斗才有roundRecord
		if (roundRecord != null && roundRecord.configParameterDic != null && roundRecord.configParameterDic.Count > 0)
		{
			sponsorIndexes = new List<int>();

			//获取需要跑动的人物的AvatarIndex
			foreach (var pair in roundRecord.configParameterDic)
			{
				if (pair.Key.StartsWith("AvatarIndex"))
				{
					int avatarIndex = StrParser.ParseDecIntEx(pair.Value, -1);
					if (avatarIndex == -1)
					{
						Debug.LogError("[SponsorGoToOpponentRound] ConfigParameter ERROR. avatarIndex=" + pair.Value);
						continue;
					}
					if (!sponsorIndexes.Contains(avatarIndex))
						sponsorIndexes.Add(avatarIndex);
					else
						Debug.LogError("[SponsorGoToOpponentRound] ConfigParameter ERROR. avatarIndex Repeated:" + pair.Value);
				}
			}

			runToBattlePosition = StrParser.ParseBool(roundRecord.Parameter("RunToBattlePosition"), false);

			//如果只有一部分人向前跑，有人留在原地，由于战斗相机会试图让所有人都留在可见范围内，会不断拉远距离，因此要让战斗相机排除掉留在原地的角色
			List<int> exceptedSponsorIndexes = new List<int>();
			foreach (var role in battleRecordPlayer.BattleRoles)
			{
				if (role.TeamIndex == battleRecordPlayer.SponsorTeamIndex)
					exceptedSponsorIndexes.Add(role.AvatarIndex);
			}

			for (int i = 0; i < sponsorIndexes.Count; i++)
			{
				if (exceptedSponsorIndexes.Contains(sponsorIndexes[i]))
					exceptedSponsorIndexes.Remove(sponsorIndexes[i]);
			}

			//需要排除的角色
			if (battleRecordPlayer.BattleScene.BattleCameraCtrl.WatchData != null)
				battleRecordPlayer.BattleScene.BattleCameraCtrl.WatchData.exceptedRoleIdxes = exceptedSponsorIndexes;
			else
				battleRecordPlayer.BattleScene.BattleCameraCtrl.WatchData = new CameraController_Battle._WatchData()
				{
					CurrentRoles = battleRecordPlayer.BattleRoles,
					exceptedRoleIdxes = exceptedSponsorIndexes,
					isRoleVisible = (idx) =>
							{
								if (battleRecordPlayer.BattleRoles == null || battleRecordPlayer.BattleRoles.Count == 0 || idx < 0 || idx >= battleRecordPlayer.BattleRoles.Count)
									return false;
								return !battleRecordPlayer.BattleRoles[idx].Hide;
							}
				};
		}

		// Initialize actor list
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (role.TeamIndex != BattleRecordPlayer.SponsorTeamIndex)
				continue;

			//剧情战斗，指定角色跑动
			if (sponsorIndexes != null && sponsorIndexes.Count > 0)
			{
				if (!sponsorIndexes.Contains(role.AvatarIndex))
					continue;
			}

			ActingRoles.Add(role);
		}

		foreach (var role in ActingRoles)
		{
			// Run action
			AvatarAction actionCfg = role.GetActionByType(AvatarAction._Type.Run);
			Debug.Assert(actionCfg != null);

			KodGames.ClientClass.ActionRecord actionRecord = new KodGames.ClientClass.ActionRecord();
			actionRecord.SrcAvatarIndex = role.AvatarIndex;
			actionRecord.TargetAvatarIndices.Add(role.AvatarIndex);
			actionRecord.ActionId = actionCfg.id;

			BattleRecordPlayer.PlayActionRecord(actionRecord);

			// Get target position
			Vector3 targetPosition;
			if (!runToBattlePosition)
				targetPosition = BattleRecordPlayer.BattleScene.GetStartPosition(BattleRecordPlayer.BattleIndex, role.TeamIndex, role.GetBattlePositionRow(), role.GetBattlePositionColumn());
			else
				targetPosition = BattleRecordPlayer.BattleScene.GetBattlePosition(BattleRecordPlayer.BattleIndex, role.TeamIndex, role.GetBattlePositionRow(), role.GetBattlePositionColumn());

			role.MoveTo(targetPosition, ConfigDatabase.DefaultCfg.GameConfig.combatSetting.runSpeed);
		}

		// Trace camera
		SetCameraTrace(BattleRecordPlayer.BattleScene.cameraNormalDistance, BattleRecordPlayer.BattleScene.cameraInterpolateDuration);
		return true;
	}

	public override void Finish()
	{
		battleRecordPlayer.BattleScene.BattleCameraCtrl.WatchData.exceptedRoleIdxes = new List<int>();
		// Trace camera
		ClearCameraTrace();

		base.Finish();
	}

	public override void Update()
	{
		base.Update();
	}

	public override void OnDrawGizmos()
	{
		Gizmos.DrawSphere(TracingPosition, 1f);
	}
}
