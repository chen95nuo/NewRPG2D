using KodGames.ClientClass;
using UnityEngine;
using System.Collections.Generic;

//手动配置战斗使用
public class Custom_PrepareEnterSceneRound : BattleRound
{
	public Custom_PrepareEnterSceneRound(RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(battleRecordPlayer)
	{
		this.roundRecord = roundRecord;
	}

	public override BattleRound Initialize()
	{
		base.Initialize();

		//获取我方角色，SetCameraTrace后GetTraceTraget用
		foreach (var role in BattleRecordPlayer.BattleRoles)
		{
			if (role.TeamIndex != 0)
				continue;

			ActingRoles.Add(role);
		}

		//剧情战斗不允许旋转相机
		battleRecordPlayer.BattleScene.BattleCameraCtrl.LockTouch = true;

		return this;
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		var cameraController = BattleRecordPlayer.BattleScene.mainCamera.GetComponent<CameraController_Battle>();
		if (cameraController != null)
			cameraController.ExternalControl = true;

		BattleRecordPlayer.BattleScene.SetTraceCameraPos();

		BattleRecordPlayer.BattleScene.RandomPositionOffset();

		//将摄像机trace到我方（设置时间为0，直接设置位置，因为这个时候相机会从0开始向目标距离Easing，这是错误的，会导致相机大幅度运动）
		SetCameraTrace(BattleRecordPlayer.BattleScene.cameraNormalDistance, 0);

		//AvatarActOrderUtil.ShowAvatarActOrderBatch(ActingRoles);
		return true;
	}

	public override void Finish()
	{
		ClearCameraTrace();
		base.Finish();
	}

}