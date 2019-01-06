using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections.Generic;

public class Custom_CameraAnimRound : CombatRound
{
	CameraController_Battle battleCamera;
	bool process = false;
	//相机转动速度
	float cameraSpeed;
	//相机转动的目标角度，在场景中，以原普通战斗初始视角为基准，逆时针方向的角度为要配置的目标角度
	float hTargetAngle;

	/*
	 * 配置变量 相机转动速度 cameraSpeed 默认1
	 * 配置变量 hTargetAngle 相机要转到的角度。在场景中，以原普通战斗初始视角为基准，逆时针方向的角度为要配置的目标角度
	 */
	public Custom_CameraAnimRound(RoundRecord roundrecord, BattleRecordPlayer battleRecordPlayer)
		: base(roundrecord, battleRecordPlayer)
	{
		if (roundrecord.configParameterDic == null || roundrecord.configParameterDic.Count == 0)
		{
			Debug.LogError("[Custom_CameraAnimRound] Haven't set up ANY Camera Parameters");
			return;
		}

		battleCamera = BattleScene.GetBattleScene().BattleCameraCtrl;

		if (!string.IsNullOrEmpty(roundrecord.Parameter("ResetVisualAngle")))
		{
			bool reset = StrParser.ParseBool(roundrecord.Parameter("ResetVisualAngle"), false);
			//视角重置
			if (reset)
				hTargetAngle = ConfigDatabase.DefaultCfg.GameConfig.combatSetting.battleDefault.GetBattleDefaultItemByCombatType(SysGameStateMachine.Instance.GetCurrentState<GameState_Battle>().BattleType).sceneCamAngleHDefault;
		}
		else
		{
			if (!roundrecord.configParameterDic.ContainsKey("HTargetAngle"))
			{
				Debug.LogError("[Custom_CameraAnimRound] Haven't set up Camera Parameter:\"HTargetAngle\"");
				return;
			}

			hTargetAngle = StrParser.ParseFloat(roundrecord.Parameter("HTargetAngle"), battleCamera.DeltaAngleH);
		}

		cameraSpeed = StrParser.ParseFloat(roundrecord.Parameter("CameraSpeed"), 1);

		process = true;
	}

	public override bool Start()
	{
		if (!base.Start())
		{
			process = false;
			return false;
		}

		//相机转动
		battleCamera.RotateTo(cameraSpeed, hTargetAngle, 0);
		return true;
	}

	public override bool CanStartAfterRound()
	{
		if (!base.CanStartAfterRound())
			return false;

		if (!process)
			return true;

		return !battleCamera.IsAutoRotating;
	}
}
