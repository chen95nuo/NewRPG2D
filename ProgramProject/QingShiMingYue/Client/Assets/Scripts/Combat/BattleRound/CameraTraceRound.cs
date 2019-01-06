using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 战斗前摄像机回正round，纯客户端逻辑round，与服务器无关
// 针对斜置摄像机，用来回正
public class CameraTraceRound : BattleRound
{
	private Vector3 startPos;
	private Vector3 endPos;
	private Quaternion startRotate;
	private Quaternion endRotate;

	private Camera camera;
	private float timer = 0f;
	private float time = 0f;

	public CameraTraceRound(BattleRecordPlayer battleRecordPlayer, BattleRound afterRound)
		: base(battleRecordPlayer)
	{
		this.AfterRound = afterRound;
	}

	//手动配置战斗使用
	public CameraTraceRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundRecord, battleRecordPlayer)
	{
	}

	public override bool Start()
	{
		if (base.Start() == false)
			return false;

		Transform startMarker = battleRecordPlayer.BattleScene.mainCamera.transform;
		Transform endMarker = battleRecordPlayer.BattleScene.mainMarker;
		startPos = startMarker.position;
		endPos = endMarker.position;
		startRotate = startMarker.rotation;
		endRotate = endMarker.rotation;

		camera = battleRecordPlayer.BattleScene.mainCamera;
		timer = 0f;
		time = battleRecordPlayer.BattleScene.traceTime;

		return true;
	}

	public override bool CanStartAfterRound()
	{
		return timer > time;
	}

	private void UpdateCamera()
	{
		if (timer > time)
			return;

		timer += Time.deltaTime;
		camera.transform.position = Vector3.Slerp(startPos, endPos, timer / time);
		camera.transform.rotation = Quaternion.Slerp(startRotate, endRotate, timer / time);

	}

	public override void Update()
	{
		base.Update();
		if (RoundState == _RoundState.Running)
		{
			UpdateCamera();
		}
	}

	public override void Finish()
	{
		base.Finish();
		if (battleRecordPlayer.BattleScene.mainCamera.GetComponent<CameraController_Battle>() != null)
		{
			battleRecordPlayer.BattleScene.mainCamera.GetComponent<CameraController_Battle>().ExternalControl = false;
		}
	}
}
