﻿﻿using UnityEngine;
using System.Collections.Generic;

public class CentralCityCameraController : CameraController_Touch, ICameraTraceable
{
	public enum BuildingGazeTarget
	{
		//重置视角
		resetVisualAngle,
		//酒馆
		tavern,
		//门客馆
		retainer,
		//擂台
		arena,
		//家族
		guild,
	}

	//各个建筑物的注视视角
	public float gazeTavernHAngle, gazeTavernVAngle;
	public float gazeRetainerHAngle, gazeRetainerVAngle;
	public float gazeArenaHAngle, gazeArenaVAngle;
	public float gazeGuildHAngle, gazeGuildVAngle;

	public Transform mainCityTraceTarget;
	public Transform CentralCitySky;
	public Transform ShenLou;
	public float initDistance = 475.7f;

	/// <summary>
	/// -1:fxied relative to Camera
	/// </summary>
	public float SkySpeed;
	/// <summary>
	/// 1: fxied relative to Camera
	/// </summary>
	public float SkyVSpeed;
	/// <summary>
	/// 1: fxied relative to Camera
	/// </summary>
	public float ShenLouVSpeed;
	/// <summary>
	/// -1: fixed relative to Camera
	/// </summary>
	public float ShenLouSpeed;

	float SkyHAngle = 0;
	float ShenLouHAngle = 0;

	Vector3 ShenLouAxies, SkyAxies;

	public Vector3 TracingPosition
	{
		get
		{
			if (mainCityTraceTarget != null)
				return mainCityTraceTarget.position;
			else
				return Vector3.zero;
		}
	}

	protected override void Initialize()
	{
		if (mainCityTraceTarget != null)
			SetTraceTarget(this, initDistance, 0, EZAnimation.EASING_TYPE.Default);
		else
			Debug.LogError("missing camera trace target at centralcity");

		deltaAngleV = -6;

		if (CentralCitySky == null)
			Debug.LogError("CentralCitySky missed");
		if (ShenLou == null)
			Debug.LogError("蜃楼 未指定");
	}

	public override void Update()
	{
		base.Update();

		SkyHAngle += MovingDelta.x * SkySpeed;
		SkyHAngle %= 360;

		ShenLouHAngle += MovingDelta.x * ShenLouSpeed;
		ShenLouHAngle %= 360;

		if (CentralCitySky != null)
		{
			SkyAxies = Vector3.Cross(CachedTransform.position - CentralCitySky.position, Vector3.up).normalized;
			CentralCitySky.localRotation = Quaternion.AngleAxis(deltaAngleV * SkyVSpeed, SkyAxies) * Quaternion.AngleAxis(SkyHAngle, Vector3.up);
		}

		if (ShenLou != null)
		{
			ShenLouAxies = Vector3.Cross(CachedTransform.position - ShenLou.position, Vector3.up).normalized;
			ShenLou.localRotation = Quaternion.AngleAxis(deltaAngleV * ShenLouVSpeed, ShenLouAxies) * Quaternion.AngleAxis(ShenLouHAngle, Vector3.up);
		}
	}


	// 摄像机自动注视指定的建筑物
	public void GazeAt(BuildingGazeTarget targetBuilding, float speed, EZAnimation.EASING_TYPE hRotationEasingType, EZAnimation.EASING_TYPE vRotationEasingType)
	{
		switch (targetBuilding)
		{
			case BuildingGazeTarget.resetVisualAngle:
				RotateTo(speed, 0, 0, hRotationEasingType, vRotationEasingType, false);
				break;
			case BuildingGazeTarget.arena:
				RotateTo(speed, gazeArenaHAngle, gazeArenaVAngle, hRotationEasingType, vRotationEasingType, false);
				break;
			case BuildingGazeTarget.guild:
				RotateTo(speed, gazeGuildHAngle, gazeGuildVAngle, hRotationEasingType, vRotationEasingType, false);
				break;
			case BuildingGazeTarget.retainer:
				RotateTo(speed, gazeRetainerHAngle, gazeRetainerVAngle, hRotationEasingType, vRotationEasingType, false);
				break;
			case BuildingGazeTarget.tavern:
				RotateTo(speed, gazeTavernHAngle, gazeTavernVAngle, hRotationEasingType, vRotationEasingType, false);
				break;
		}
	}

	public void GazeAt(BuildingGazeTarget targetBuilding)
	{
		GazeAt(targetBuilding, 1, EZAnimation.EASING_TYPE.Default, EZAnimation.EASING_TYPE.Default);
	}

	//protected override void LateUpdate()
	//{
	//限制蜃楼的移动距离
	//float shenlouAngle = ShenLou.localEulerAngles.y;
	//float camAngle = CachedTransform.localEulerAngles.y;

	//while (shenlouAngle < 0) shenlouAngle += 360;
	//while (camAngle < 0) camAngle += 360;

	//float cha = shenlouAngle - camAngle;

	//if (cha < -180) cha += 360;
	//if (cha > 180) cha -= 360;

	//if ((cha < -34 && MovingDelta.x < 0) || (cha > 32 && MovingDelta.x > 0)) ShenLouSpeed = -1;
	//else
	//{
	//    if (ShenLouSpeed != -1 && ShenLouSpeed != shenLouOriSpeed) shenLouOriSpeed = ShenLouSpeed;
	//    else ShenLouSpeed = shenLouOriSpeed;
	//}
	//}

	//void OnGUI()
	//{
	//    if (GUILayout.Button("LookAt 擂台"))
	//        GazeAt(BuildingGazeTarget.arena);

	//    if (GUILayout.Button("LookAt 家族"))
	//        GazeAt(BuildingGazeTarget.guild);

	//    if (GUILayout.Button("LookAt 门客馆"))
	//        GazeAt(BuildingGazeTarget.retainer);

	//    if (GUILayout.Button("LookAt 酒肆"))
	//        GazeAt(BuildingGazeTarget.tavern);

	//    if (GUILayout.Button("重置视角"))
	//        GazeAt(BuildingGazeTarget.resetVisualAngle);
	//}
}