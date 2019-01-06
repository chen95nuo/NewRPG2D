using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController_Touch : CameraController
{
	[System.Serializable]
	public class CameraEasingData
	{
		public float timerH, timerV;
		public float duration_HRotation, duration_VRotation;

		public float deltaAngleHStart = 0, DeltaAngleH_Delta = 0;
		public float deltaAngleHReset = 0;

		public float deltaAngleVStart = 0, DeltaAngleV_Delta = 0;
		public float deltaAngleVReset = 0;

		public EZAnimation.Interpolator hRotationIntepolate, vRotationInterpolate;

		public bool isAutoRotating = false;
		public bool isEasingRotation = false;
		//自动旋转时是否可以被手动拖动打断
		public bool autoRotateCanBreakByDrag = false;
	}

	/// <summary>
	/// used by SyncDrag
	/// </summary>
	public enum DragState
	{
		None,
		Horizontal,
		Vertical,
	}
	/// <summary>
	/// for camera fx
	/// </summary>
	[System.NonSerialized]
	public Vector3 externalOffset = new Vector3(0, 0, 0);

	public Vector3 fixedOffset = new Vector3(0, 0, 0);

	public Vector3 translationOffset = new Vector3(0, 0, 0);

	/// <summary>
	/// When LockTouch ,Camera can't be controlled by fingers.
	/// </summary>
	public bool LockTouch = false;

	/// <summary>
	/// RotationEasing Velocity Curve.
	/// </summary>
	public AnimationCurve HCurve, VCurve;

	public bool IsAutoRotating
	{
		get
		{
			if (easingData != null)
				return easingData.isAutoRotating;

			return false;
		}
	}

	public bool IsEasingRotating
	{
		get
		{
			if (easingData != null)
				return easingData.isEasingRotation;

			return false;
		}
	}

	#region

	/// <summary>
	/// Sensitive.x:HorizontalRotation Sensitive;
	/// Sensitive.y:VerticalRotation Sensitive;
	/// Sensitive.z:ChangeDistace Sensitive;
	/// </summary>
	public Vector3 Sensitive = new Vector3(0.08f, 0.08f, 1f);

	/// <summary>
	/// adjust the duration when reseting VisualAngle
	/// </summary>
	public float resetVisualAngleDurationFactorH = 1, resetVisualAngleDurationFactorV = 1;

	/// <summary>
	/// if SyncDrag when dragging, Horizontal drag and Vertical drag will work at the sametime.
	/// else ONLY horizontal OR vertical dragging can work at the sametime.
	/// </summary>
	public bool SyncDrag = true;

	public bool DisableHRotation = false;
	public bool DisableVRotation = false;
	//protected bool DisableDistCtrl = false;
	public bool DisableDragEasing = false;

	/// <summary>
	/// min distance to tracetarget
	/// </summary>
	public float minDistance = 26;
	/// <summary>
	/// max distance to tracetarget
	/// </summary>
	public float maxDistance = 90;
	/// <summary>
	/// Horizontal Rotation min angle 
	/// </summary>
	public float HRotationMin = -360;
	/// <summary>
	/// Horizontal Rotation max angle
	/// </summary>
	public float HRotationMax = 360;
	/// <summary>
	/// can not be a RIGHT ANGLE (90,270,-90,-270...)
	/// Vertical Rotation max angle
	/// </summary>
	public float VRotationMin = -89;
	/// <summary>
	/// can not be a RIGHT ANGLE (90,270,-90,-270...)
	/// Vertical Rotation max angle
	/// </summary>
	public float VRotationMax = 89;

	/// <summary>
	/// Horizontal Angle Original Value
	/// </summary>
	public float angleHDefault = 90;
	/// <summary>
	/// Vertical Angle Original Value
	/// </summary>
	public float angleVDefault = 42;

	public float HEasingFactor = 1;

	public float VEasingFactor = 1;

	#endregion

	/// <summary>
	/// 不被视为UI的Layer，视为UI则不会拖动场景。
	/// </summary>
	protected int exceptedLayerMask = 0;

	protected CameraEasingData easingData;

	protected float DeltaDistance = 0;
	/// <summary>
	/// 滑动屏幕时，顺时针时减小，逆时针时增加
	/// </summary>
	protected float deltaAngleH = 0;
	public float DeltaAngleH
	{
		get
		{
			return deltaAngleH;
		}
	}

	/// <summary>
	/// 滑动屏幕时，顺时针时减小，逆时针时增加
	/// </summary>
	protected float deltaAngleV = 0;
	public float DeltaAngleV
	{
		get
		{
			return deltaAngleH;
		}
	}

	protected float realDistance;
	protected Vector3 SourcePosi = Vector3.zero;
	protected Vector3 RelaVector = Vector3.zero;

	protected Vector3 PrevStatus;
	protected Vector3 MovingDelta = Vector3.zero;

	protected DragState curDragState = DragState.None;
	protected POINTER_INFO prevDragData;
	protected bool uiOperating = false;

	protected bool inputDelegateAdded = false;
	public bool traceTargetSeted = false;

	protected float RealDeltaTime
	{
		get { return Time.timeScale == 0 ? 0 : Time.deltaTime / Time.timeScale; }
	}

	void Start()
	{
		if (HCurve == null || HCurve.keys.Length == 0)
		{
			HCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
			int idx = HCurve.AddKey(new Keyframe(0.65f, 0));
			if (idx != -1)
				HCurve.SmoothTangents(idx, 0);
		}
		if (VCurve == null || VCurve.keys.Length == 0)
		{
			VCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
			int idx = VCurve.AddKey(new Keyframe(0.618f, 0.1f));
			if (idx != -1)
				HCurve.SmoothTangents(idx, 0);
		}
		PrevStatus = new Vector3(deltaAngleH, deltaAngleV, 0);

		Initialize();
	}

	/// <summary>
	/// Specify any property here.
	/// </summary>
	protected virtual void Initialize() { }

	public void AddInputDelegate()
	{
		if (!inputDelegateAdded)
		{
			UIManager.instance.AddMouseTouchPtrListener(MouseTouchPtrListener);
			inputDelegateAdded = true;
		}
	}

	public void RemoveInputDelegate()
	{
		if (inputDelegateAdded)
		{
			UIManager.instance.RemoveMouseTouchPtrListener(MouseTouchPtrListener);
			inputDelegateAdded = false;
		}
	}

	//public void ForceSetTraceDataByTraceTarget()
	//{
	//    if (traceTarget != null)
	//        SourcePosi = traceTarget.TracingPosition;

	//    traceTargetSeted = true;
	//}

	public override void Update()
	{
		base.Update();

		if (traceTarget != null)
		{
			SourcePosi = traceTarget.TracingPosition;
			if (!traceTargetSeted)
				traceTargetSeted = true;
		}

		if (traceTargetSeted)
			OnTouchUpdate();
	}

	protected virtual void OnTouchUpdate()
	{
		if (easingData != null)
		{
			easingData.timerH += RealDeltaTime;
			easingData.timerV += RealDeltaTime;
			if (easingData.isAutoRotating)
			{
				if (!DisableHRotation)
				{
					if (easingData.timerH >= easingData.duration_HRotation)
						deltaAngleH = PrevStatus.x = easingData.deltaAngleHReset;
					else
						deltaAngleH = CalcEasingH();
				}

				if (!DisableVRotation)
				{
					if (easingData.timerV >= easingData.duration_VRotation)
						deltaAngleV = PrevStatus.y = easingData.deltaAngleVReset;
					else
						deltaAngleV = CalcEasingV();
				}
			}
			else if (easingData.isEasingRotation)
			{
				if (SyncDrag)
				{
					if (!DisableHRotation)
						deltaAngleH += CalcEasingH() - easingData.deltaAngleHStart;
					if (!DisableVRotation)
						deltaAngleV += CalcEasingV() - easingData.deltaAngleVStart;
				}
				else
				{
					if (curDragState == DragState.Horizontal && !DisableHRotation)
						deltaAngleH += CalcEasingH() - easingData.deltaAngleHStart;
					else if (curDragState == DragState.Vertical && !DisableVRotation)
						deltaAngleV += CalcEasingV() - easingData.deltaAngleVStart;
				}
			}

			if (easingData.timerH >= easingData.duration_HRotation && easingData.timerV >= easingData.duration_VRotation)
			{
				curDragState = DragState.None;
				easingData = null;
			}
		}

		MovingDelta.x = deltaAngleH - PrevStatus.x;
		MovingDelta.y = deltaAngleV - PrevStatus.y;

		ClampAngle(ref deltaAngleH, true);
		ClampAngle(ref deltaAngleV, false);
		//先Clamp再保存状态，防止计算Delta时出现巨大的Delta
		PrevStatus.x = deltaAngleH;
		PrevStatus.y = deltaAngleV;

		SetCamPosition();
	}

	protected void SetCamPosition()
	{
		realDistance = GetTargetDistance() + DeltaDistance;

		if (realDistance > maxDistance)
		{
			//修改DeltaDistance，限制在有意义的范围
			DeltaDistance -= realDistance - maxDistance;
			realDistance = maxDistance;
		}
		if (realDistance < minDistance)
		{
			//修改DeltaDistance，限制在有意义的范围
			DeltaDistance += realDistance - maxDistance;
			realDistance = minDistance;
		}

		float temp = realDistance * Mathf.Cos((angleVDefault + deltaAngleV) * Mathf.Deg2Rad);
		RelaVector.y = realDistance * Mathf.Sin((angleVDefault + deltaAngleV) * Mathf.Deg2Rad);
		RelaVector.x = temp * Mathf.Cos((angleHDefault + deltaAngleH) * Mathf.Deg2Rad);
		RelaVector.z = temp * Mathf.Sin((angleHDefault + deltaAngleH) * Mathf.Deg2Rad);

		//RelaVector = new Vector3(0, 0, -realDistance);
		//RelaVector = Quaternion.Euler(angleVDefault + deltaAngleV, -(angleHDefault + deltaAngleH), 0f) * RelaVector;

		CachedTransform.position = SourcePosi + RelaVector;
		CachedTransform.LookAt(SourcePosi);
		//Apply Translation
		CachedTransform.position += KodGames.Math.RelativeOffset(CachedTransform, translationOffset, false);

		//TODO 仿千层楼的externalOffset
		CachedTransform.position += externalOffset + fixedOffset;
	}

	/// <summary>
	/// 自动旋转摄像机到指定的角度
	/// </summary>
	/// <param name="speed">旋转速度</param>
	/// <param name="hTargetAngle">水平目标角度（相对于默认角度）</param>
	/// <param name="vTargetAngle">垂直目标角度（相对于默认角度）</param>
	/// <param name="HRotationEasingType">水平旋转动画类型</param>
	/// <param name="VRotationEasingType">垂坠旋转动画类型</param>
	public void RotateTo(float speed, float hTargetAngle, float vTargetAngle, EZAnimation.EASING_TYPE HRotationEasingType, EZAnimation.EASING_TYPE VRotationEasingType, bool autoRotateCanBreakByDrag)
	{
		if ((easingData != null && easingData.isAutoRotating) || speed == 0)
			return;

		easingData = new CameraEasingData()
		{
			isAutoRotating = true,
			isEasingRotation = false,
			timerV = 0,
			timerH = 0,
			deltaAngleHReset = hTargetAngle,
			deltaAngleVReset = vTargetAngle,
			deltaAngleHStart = deltaAngleH,
			deltaAngleVStart = deltaAngleV,
			hRotationIntepolate = EZAnimation.GetInterpolator(HRotationEasingType),
			vRotationInterpolate = EZAnimation.GetInterpolator(VRotationEasingType),
			autoRotateCanBreakByDrag = autoRotateCanBreakByDrag
		};

		if (!Mathf.Approximately(deltaAngleH % 360, hTargetAngle % 360))
		{
			easingData.DeltaAngleH_Delta = Mathf.DeltaAngle(deltaAngleH, hTargetAngle);
			//set duration by speed
			easingData.duration_HRotation = Mathf.Abs(easingData.DeltaAngleH_Delta) / 180 * resetVisualAngleDurationFactorH / speed;
		}

		if (!Mathf.Approximately(deltaAngleV % 360, vTargetAngle % 360))
		{
			easingData.DeltaAngleV_Delta = Mathf.DeltaAngle(deltaAngleV, vTargetAngle);
			//set duration by speed
			easingData.duration_VRotation = Mathf.Abs(easingData.DeltaAngleV_Delta) / 180 * resetVisualAngleDurationFactorV / speed;
		}
	}

	public void RotateTo(float speed, float hTargetAngle, float vTargetAngle)
	{
		RotateTo(speed, hTargetAngle, vTargetAngle, EZAnimation.EASING_TYPE.Default, EZAnimation.EASING_TYPE.Default, false);
	}

	public void ResetAngles()
	{
		deltaAngleH = 0;
		deltaAngleV = 0;
	}

	//[-PI,PI] and limit
	protected void ClampAngle(ref float angle, bool IsHorizontalAngle)
	{
		angle %= 360;
		if (IsHorizontalAngle)
		{
			if (angle > HRotationMax)
				angle = HRotationMax;
			if (angle < HRotationMin)
				angle = HRotationMin;
		}
		else
		{
			if (angle + angleVDefault > VRotationMax)
				angle = VRotationMax - angleVDefault;
			if (angle + angleVDefault < VRotationMin)
				angle = VRotationMin - angleVDefault;
		}
	}

	protected virtual void MouseTouchPtrListener(POINTER_INFO data)
	{
		if (data.evt == POINTER_INFO.INPUT_EVENT.MOVE || data.evt == POINTER_INFO.INPUT_EVENT.NO_CHANGE)
			return;

		if (!uiOperating &&
		(
			(data.targetObj != null
			&& data.evt == POINTER_INFO.INPUT_EVENT.PRESS
			&& (((1 << data.targetObj.gameObject.layer) & KodGames.Camera.main.cullingMask) == 0)
			&& (((1 << data.targetObj.gameObject.layer) & exceptedLayerMask) == 0))
		))
			uiOperating = true;

		if (uiOperating &&
				(data.evt == POINTER_INFO.INPUT_EVENT.RELEASE
				|| data.evt == POINTER_INFO.INPUT_EVENT.TAP
				|| data.evt == POINTER_INFO.INPUT_EVENT.RELEASE_OFF))
		{
			//POINTER_INFO.INPUT_EVENT.RELEASE will be triggered twice
			prevDragData = data;
			uiOperating = false;
		}

		if (data.targetObj != null || uiOperating)
			return;

		if (LockTouch)
			return;

		ProcessDragLogic(data);
	}

	protected virtual void ProcessDragLogic(POINTER_INFO data)
	{
		//UIManager's inputDelta is NOT STEADY on TouchDevices ......
		Vector3 DevicePosiDelta;

		if (data.evt == POINTER_INFO.INPUT_EVENT.DRAG)
		{
			DevicePosiDelta = data.devicePos - prevDragData.devicePos;
			if (easingData != null && (easingData.isEasingRotation || easingData.autoRotateCanBreakByDrag))
				easingData = null;

			if (easingData == null || !easingData.isAutoRotating)
			{
				if (SyncDrag)
				{
					if (!DisableHRotation)
						deltaAngleH -= DevicePosiDelta.x * Sensitive.x;
					if (!DisableVRotation)
						deltaAngleV -= DevicePosiDelta.y * Sensitive.y;
				}
				else
				{
					if (DisableHRotation && !DisableVRotation)
						deltaAngleV -= DevicePosiDelta.y * Sensitive.y;
					else if (DisableVRotation && !DisableHRotation)
						deltaAngleH -= DevicePosiDelta.x * Sensitive.x;
					else if (!DisableHRotation && !DisableHRotation)
					{
						if (curDragState == DragState.None)
						{
							if (Mathf.Abs(DevicePosiDelta.x) - Mathf.Abs(DevicePosiDelta.y) >= 0)
								curDragState = DragState.Horizontal;
							else
								curDragState = DragState.Vertical;
						}
						if (curDragState == DragState.Horizontal)
							deltaAngleH -= DevicePosiDelta.x * Sensitive.x;
						else if (curDragState == DragState.Vertical)
							deltaAngleV -= DevicePosiDelta.y * Sensitive.y;
					}
				}
			}
			prevDragData = data;
		}

		if (data.evt == POINTER_INFO.INPUT_EVENT.PRESS)
		{
			if (easingData != null && (!easingData.isAutoRotating || easingData.autoRotateCanBreakByDrag))
			{
				easingData = null;
				curDragState = DragState.None;
			}
			prevDragData = data;
		}

		if (!DisableDragEasing && (data.evt == POINTER_INFO.INPUT_EVENT.RELEASE || data.evt == POINTER_INFO.INPUT_EVENT.RELEASE_OFF))
		{
			if (easingData != null && (easingData.isAutoRotating && easingData.autoRotateCanBreakByDrag == false))
				return;

			float xTargetDelta = 0, yTargetDelta = 0;
			float hduration = 0, vduration = 0;
			DevicePosiDelta = data.devicePos - prevDragData.devicePos;

			xTargetDelta = -DevicePosiDelta.x * 0.05f * HEasingFactor;
			hduration = Mathf.Abs(xTargetDelta / 10);
			yTargetDelta = -DevicePosiDelta.y * 0.05f * VEasingFactor;
			vduration = Mathf.Abs(yTargetDelta / 10);

			hduration = Mathf.Clamp(hduration, 0.5f, 2f);
			vduration = Mathf.Clamp(vduration, 0.5f, 2f);

			easingData = new CameraEasingData()
			{
				isAutoRotating = false,
				isEasingRotation = true,
				timerH = 0,
				timerV = 0,
				deltaAngleHStart = deltaAngleH,
				DeltaAngleH_Delta = xTargetDelta,
				deltaAngleVStart = deltaAngleV,
				DeltaAngleV_Delta = yTargetDelta,
				duration_HRotation = hduration,
				duration_VRotation = vduration,
				vRotationInterpolate = VInterpolate,
				hRotationIntepolate = HInterpolate
			};
		}
	}

	float VInterpolate(float time, float start, float delta, float duration)
	{
		return VCurve.Evaluate(time / duration) * delta + start;
	}

	float HInterpolate(float time, float start, float delta, float duration)
	{
		return HCurve.Evaluate(time / duration) * delta + start;
	}

	protected float CalcEasingH()
	{
		return easingData.hRotationIntepolate(easingData.timerH, easingData.deltaAngleHStart, easingData.DeltaAngleH_Delta, easingData.duration_HRotation);
	}

	protected float CalcEasingV()
	{
		return easingData.vRotationInterpolate(easingData.timerV, easingData.deltaAngleVStart, easingData.DeltaAngleV_Delta, easingData.duration_VRotation);
	}

}