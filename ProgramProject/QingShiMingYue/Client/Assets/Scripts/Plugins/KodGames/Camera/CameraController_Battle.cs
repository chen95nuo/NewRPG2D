using UnityEngine;
using System.Collections.Generic;

public class CameraController_Battle : CameraController_Touch
{
	public class _WatchData
	{
		public List<BattleRole> CurrentRoles = new List<BattleRole>();

		//即使能看到也忽略的角色的avatarIndex.
		public List<int> exceptedRoleIdxes = new List<int>();
		/// <summary>
		/// Whether a role is VISIBLE in Scene,not "activeInHierarchy"
		/// </summary>
		public System.Func<int, bool> isRoleVisible;
	}

	_WatchData _watchData;
	public _WatchData WatchData
	{
		get
		{
			return _watchData;
		}
		set
		{
			_watchData = value;
		}
	}
	public AnimationCurve verticalAngleCurve;

	//在这里调整当保持角色在屏幕之内时，角色距离屏幕边界的远近程度
	private float outScreenCheckThreshold = 2f;

	public bool ExternalControl = false;

	protected override void Initialize()
	{
		base.Initialize();
		DisableDragEasing = false;
		DisableVRotation = true;
		exceptedLayerMask |= 1 << GameDefines.UISceneRaycastLayer;
		Debug.Assert(verticalAngleCurve != null, "verticalAngleCurve is null");
		if (verticalAngleCurve != null)
			Debug.Assert(verticalAngleCurve.keys.Length > 1, "verticalAngleCurve key count isn't enough");
	}

	void Awake()
	{
		//各个分辨率下视口适配
		if (GetComponent<CameraFixViewPoint>() == null)
			gameObject.AddComponent<CameraFixViewPoint>();

		var maskCamObj = GameObject.Find("MaskCamera");
		if (maskCamObj != null && maskCamObj.gameObject.GetComponent<CameraFixViewPoint>() == null)
			maskCamObj.gameObject.AddComponent<CameraFixViewPoint>();
	}

	public override void Update()
	{
		//Not call base.Update() when ExternalControl because at base.Update() :" taretDistanceEasingData.timer += Time.deltaTime;"
		if (ExternalControl)
			return;

		base.Update();
	}

	public void ResetDeltaAngles()
	{
		deltaAngleH = 0;
		deltaAngleV = 0;
	}

	protected override void OnTouchUpdate()
	{
		CalcDeltaDistance();

		if (easingData != null)
		{
			easingData.timerH += RealDeltaTime;
			easingData.timerV += RealDeltaTime;
			if (easingData.isAutoRotating)
			{
				if (easingData.timerH >= easingData.duration_HRotation)
					deltaAngleH = easingData.deltaAngleHReset;
				else
					deltaAngleH = CalcEasingH();
			}
			else if (easingData.isEasingRotation)
			{
				if (SyncDrag)
				{
					if (!DisableHRotation)
						deltaAngleH += CalcEasingH() - easingData.deltaAngleHStart;
				}
				else
				{
					if (curDragState == DragState.Horizontal && !DisableHRotation)
						deltaAngleH += CalcEasingH() - easingData.deltaAngleHStart;
				}
			}

			if (easingData.timerH >= easingData.duration_HRotation)
			{
				curDragState = DragState.None;
				easingData = null;
			}
		}

		//must between [0,360]
		deltaAngleH %= 360;
		if (deltaAngleH < 0)
			deltaAngleH += 360;

		deltaAngleV = verticalAngleCurve.Evaluate(deltaAngleH);

		ClampAngle(ref deltaAngleV, false);

		SetCamPosition();
	}

	/// <summary>
	/// Auto calculate the distance between camera and Tracetarget to keep all roles in screen
	/// Change Sensitive.z to adjust easing speed when camera is adjusting distance.
	/// </summary>
	void CalcDeltaDistance()
	{
		if (WatchData == null || WatchData.CurrentRoles == null || WatchData.isRoleVisible == null)
			return;

		float outScreenDelta = -65535;
		Vector3 temp;
		Camera mainCam = KodGames.Camera.main;
		BattleRole role;

		//分辨率适配
		float defaultRatio = GameDefines.uiDefaultScreenSize.x / GameDefines.uiDefaultScreenSize.y;
		float curRatio = Screen.width / (float)Screen.height;

		float xDelta = 0, yDelta = 0;
		if (curRatio > defaultRatio)//过宽
		{
			float expectedWidth = Screen.height * defaultRatio;
			xDelta = ((Screen.width - expectedWidth) / (float)Screen.width) / 2f;
		}
		else if (curRatio < defaultRatio)//过高
		{
			float expectedHeight = Screen.width / defaultRatio;
			yDelta = ((Screen.height - expectedHeight) / (float)Screen.height) / 2f;
		}

		float thresholdXRight = 1 - xDelta, thresholdXLeft = xDelta;
		float thresholdYTop = 1 - yDelta, thresholdYDown = yDelta;

		for (int i = 0; i < WatchData.CurrentRoles.Count; i++)
		{
			role = WatchData.CurrentRoles[i];

			if (role == null || !WatchData.isRoleVisible(i))
				continue;

			if (WatchData.exceptedRoleIdxes.Contains(role.AvatarIndex))
				continue;

			//bottomLeft (0,0) topRight(1,1)
			temp = mainCam.WorldToViewportPoint(role.CachedTransform.position + (role.CachedTransform.position - SourcePosi).normalized * outScreenCheckThreshold);

			if (temp.x > 0.5f)
				outScreenDelta = Mathf.Max(outScreenDelta, temp.x - thresholdXRight);
			else
				outScreenDelta = Mathf.Max(outScreenDelta, thresholdXLeft - temp.x);
			if (temp.y > 0.5f)
				outScreenDelta = Mathf.Max(outScreenDelta, temp.y - thresholdYTop);
			else
				outScreenDelta = Mathf.Max(outScreenDelta, thresholdYDown - temp.y);
		}

		//One circumstance,no role(All die),and outScreenDelta will keep -65535.
		if (outScreenDelta != -65535)
			//使用二次函数，当相机越接近目标点时，接近的速度越小，尽量减少越过目标点的幅度。
			DeltaDistance += outScreenDelta * Mathf.Abs(outScreenDelta * 40) * RealDeltaTime * 400;
		//DeltaDistance += outScreenDelta * Sensitive.z * RealDeltaTime * 400;

		if (DeltaDistance < 0)
			DeltaDistance = 0;
	}

	[ContextMenu("Log VerticalAngleCrvKey")]
	void LogVerticalAngleCrvKeys()
	{
		Debug.Log("VerticalAngleCurveKey++++++++++++++++++++++++++++++++++++++");
		for (int i = 0; i < verticalAngleCurve.keys.Length; i++)
			Debug.Log(string.Format("[Time]:{0}  ,  [Value]:{1}", verticalAngleCurve.keys[i].time, verticalAngleCurve.keys[i].value));
		Debug.Log("LogFinish++++++++++++++++++++++++++++++++++++++++++++++");
	}

	[ContextMenu("BuidDefaultVerticalCurve")]
	void BuidDefaultVerticalCurve()
	{
		verticalAngleCurve = AnimationCurve.Linear(0, 0, 360, 0);
	}

	void OnGUI()
	{
		//GUI.skin.label.fontSize = 24;
		//GUI.color = Color.cyan;
		//GUILayout.Label(DeltaDistance.ToString(), GUILayout.Width(300));
		//GUILayout.Label("offset" + offset.ToString(), GUILayout.Width(300));
		//GUILayout.Label("outScreenDelta" + outScreenDelta.ToString(), GUILayout.Width(400));

		//GUILayout.Label("Horizontal " + DeltaAngleH);
		//GUILayout.Label("InputDelAdded " + inputDelegateAdded);
		//GUILayout.Label("ExternalControl " + ExternalControl);
		//GUILayout.Label("LockTouch " + LockTouch);
		//GUILayout.Label("GetTargetDistance:" + GetTargetDistance());
		//GUILayout.Label("TracePosition:" + SourcePosi);
		//GUILayout.Label("realDistance" + realDistance);
		//GUILayout.Label("Physics Distance" + Vector3.Distance(SourcePosi, CachedTransform.position));
	}
}
