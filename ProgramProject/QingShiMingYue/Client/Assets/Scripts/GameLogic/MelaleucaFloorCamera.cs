using System;
using System.Collections.Generic;
using UnityEngine;

public class MelaleucaFloorCamera : MonoBehaviour
{
	public TowerPlayerRole towerPlayerRole;

	public float visualAngleY = 0;
	public float visualAngleX = 0;

	public UIScroller scroller;

	public Vector3 fixedOffset = Vector3.zero;

	public float orbitDistance = 22;

	//附加角度,该值为0则为正视（正对着角色侧面）,跑动前后会插值到正视角或从正视角插值到侧视角
	public float additiveAngleH = 20;

	float angleH = 0;

	//插值旋转总时间
	float rotateToDefaultAngHDuration = 0;

	//差值旋转时间计时器
	float rotateToDefaultAngHTimer = 0;

	//Easing 曲线。EZ的插值函数不适合旋转相机的控制
	public AnimationCurve camStartEasingCrv, camEndEasingCrv;

	float StartInterpolate(float time, float duration, float start, float delta)
	{
		return camStartEasingCrv.Evaluate(time / duration) * delta + start;
	}

	float EndInterpolate(float time, float duration, float start, float delta)
	{
		return camEndEasingCrv.Evaluate(time / duration) * delta + start;
	}

	void Start()
	{
		SetRotateLimit(8, 0);

		rotateToDefaultAngHDuration = 1f;
	}

	void Update()
	{
		if (TowerSceneData.Instance == null)
			return;

		Transform calcTargetTrans;
		if (towerPlayerRole == null)
		{
			calcTargetTrans = TowerSceneData.Instance.initPathNode;
			calcTargetTrans.forward = TowerSceneData.Instance.RoleForward(calcTargetTrans);
		}
		else
			calcTargetTrans = towerPlayerRole.CachedTransform;

		Vector3 relaVec = TowerSceneData.Instance.CameraVector(calcTargetTrans, visualAngleX, visualAngleY).normalized * orbitDistance;
		Vector3 oriPosition = relaVec + calcTargetTrans.position;

		Vector3 offsetVector = KodGames.Math.RelativeOffset(calcTargetTrans, fixedOffset, false);

		if (towerPlayerRole != null && towerPlayerRole.IsMoving)//移动时，逐渐转向正视角
		{
			scroller.ScrollPosition = new Vector2(1, 0);

			if (rotateToDefaultAngHTimer > rotateToDefaultAngHDuration)
				rotateToDefaultAngHTimer = rotateToDefaultAngHDuration;

			if (rotateToDefaultAngHTimer > 0)
				rotateToDefaultAngHTimer -= Time.deltaTime;

			if (rotateToDefaultAngHTimer < 0)
				rotateToDefaultAngHTimer = 0;

			angleH = StartInterpolate(rotateToDefaultAngHTimer, rotateToDefaultAngHDuration, -scroller.Value.x, additiveAngleH);
		}
		else//停止移动时，转向侧视角
		{
			if (rotateToDefaultAngHTimer < 0)
				rotateToDefaultAngHTimer = 0;

			if (rotateToDefaultAngHTimer < rotateToDefaultAngHDuration)
				rotateToDefaultAngHTimer += Time.deltaTime;

			if (rotateToDefaultAngHTimer > rotateToDefaultAngHDuration)
				rotateToDefaultAngHTimer = rotateToDefaultAngHDuration;

			angleH = EndInterpolate(rotateToDefaultAngHTimer, rotateToDefaultAngHDuration, -scroller.Value.x, additiveAngleH);
		}

		oriPosition += offsetVector;

		//人物跑动时未处在屏幕中间，稍稍偏左，该问题暂未找到原因，暂时强制添加旋转角度使人物处于屏幕中间
		angleH += 10;

		//千层塔要以(0,1,0)为中心轴放置，方便计算
		relaVec = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angleH, Vector3.up), Vector3.one).MultiplyPoint3x4(relaVec.normalized);
		oriPosition = Matrix4x4.TRS(Vector3.zero, Quaternion.AngleAxis(angleH, Vector3.up), Vector3.one).MultiplyPoint3x4(oriPosition);

		oriPosition.y += +angleH * 0.045f;

		CachedTransform.forward = -relaVec;
		CachedTransform.position = oriPosition;
	}

	//设置相机向前可以转几层，向后可以转几层
	public void SetRotateLimit(int forwardFloorCount, int backwordFloorCount)
	{
		//1200,0.04;
		scroller.ScrollPosition = new Vector2(1, 0);
		scroller.MinValue = new Vector2(forwardFloorCount * -120, 0.1f);
		scroller.MaxValue = new Vector2(backwordFloorCount * 120, 0.2f);
	}

	//void OnGUI()
	//{
	//    GUILayout.Label("Angle:" + angleH);
	//    GUILayout.Label("ScrollerX:" + scroller.Value.x);
	//}
}
