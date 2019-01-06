using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BattleScene : MonoBehaviour
{
	public float teamSpace;
	public float rowSpace;
	public float columnSpace;
	private static int columnCount = 3;
	private static int rowCount = 3;
	public float teamCenterOffset;
	public float maxColumnRandomOffset;
	public float maxRowRandomOffset;
	public float maxEnterSceneDelayTime;
	public BattleRecordPlayer battleRecordPlayer;
	public Transform initMarker;
	//是否强制使用第一场战斗的位置信息，用于多播怪物原地战斗
	public bool forceUseFirstCombatMarker;
	public Transform[] combatMarkers;
	public GUITexture maskGUITexture;
	public Camera maskCamera;
	private float[] columnRandomOffset;
	private float[] rowRandomOffset;
	public float cameraTraceOffset;
	public float cameraNormalDistance;
	public float cameraCombatDistance;
	public float cameraInterpolateDuration;
	public EZAnimation.EASING_TYPE cameraInterpolateEasingType;
	public EZAnimation.EASING_TYPE cameraTraceEasingType;
	public Camera mainCamera = null;
	private CameraController_Battle battleCameraCtrl = null;
	public CameraController_Battle BattleCameraCtrl
	{
		get
		{
			if (battleCameraCtrl == null)
				battleCameraCtrl = mainCamera.GetComponent<CameraController_Battle>();
			return battleCameraCtrl;
		}
	}
	public GameObject[] cameraAnimaObj;
	public Camera[] cameraAnima;
	public bool isOpponentEnter;
	public bool canSkipCameraAniamtion;
	private Vector3 tracePos;
	private Vector3 avatarHeight = new Vector3(0, 3f, 0);
	private Vector3 avatarBattleBarPosOffset = new Vector3(0, 0.3f, 0);
	private static BattleScene instance = null;
	public Transform mainMarker;
	public Transform traceMarker;
	public float traceTime = 0.5f;
	public string sponsorLiftBar;
	public bool playOpponentEnterFx = false; // 是否播放被挑战者入场特效

	public static BattleScene GetBattleScene()
	{
		if (instance == null)
		{
            GameObject battleDataObject = GameObject.Find("SceneData");
            if (battleDataObject == null)
                return null;
            instance = battleDataObject.GetComponent<BattleScene>();
            columnCount = ClientServerCommon.ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation;
            rowCount = ClientServerCommon.ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation;
		}
		return instance;
	}

	public void Awake()
	{
		if (maskGUITexture != null)
		{
			maskGUITexture.pixelInset = new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height);
		}

		BattleCameraCtrl.minDistance = cameraCombatDistance;
		BattleCameraCtrl.maxDistance = cameraNormalDistance;

		CreateTraceTargetPoint();
	}

	public void RandomPositionOffset()
	{
		maxColumnRandomOffset = Mathf.Abs(maxColumnRandomOffset);

		columnRandomOffset = new float[columnCount];
		for (int i = 0; i < columnRandomOffset.Length; ++i)
			columnRandomOffset[i] = Random.Range(-maxColumnRandomOffset, maxColumnRandomOffset);

		maxRowRandomOffset = Mathf.Abs(maxRowRandomOffset);

		rowRandomOffset = new float[rowCount];
		for (int i = 0; i < rowRandomOffset.Length; ++i)
			rowRandomOffset[i] = Random.Range(-maxRowRandomOffset, maxRowRandomOffset);
	}

	public Vector3 GetInitRight()
	{
		Vector3 forward = GetInitForward();
		Quaternion quaternion = Quaternion.AngleAxis(90, Vector3.up);
		Vector3 right = quaternion * forward;
		right.Normalize();
		return right;
	}

	public Vector3 GetInitForward()
	{
		//Vector3 initForward = (combatMarkers[0].position - initMarker.position);
		Vector3 initForward = (combatMarkers[1].position - combatMarkers[0].position);
		initForward.Normalize();
		return initForward;
	}

	public Vector3 GetTeamInitPosition()
	{
		if ((battleRecordPlayer != null && battleRecordPlayer.FirstBattle) || battleRecordPlayer == null)
			return combatMarkers[0].position;
		return initMarker.position;
	}

	public Vector3 GetInitRowPosition(int rowIndex)
	{
		Vector3 teamPosition = GetTeamInitPosition();
		Vector3 teamForward = GetInitForward();
		return teamPosition - teamForward * rowSpace * rowIndex;
	}

	public Vector3 GetInitPosition(int rowIndex, int columnIndex)
	{
		Vector3 rowForward = GetInitForward();
		Vector3 rowPosition = GetInitRowPosition(rowIndex);
		Vector3 battleRight = GetInitRight();
		Vector3 column = rowPosition + battleRight * (-(columnCount - 1) / 2f + columnIndex) * columnSpace + rowForward * columnRandomOffset[columnIndex]
			+ battleRight * rowRandomOffset[rowIndex];
		return column;
	}

	public Vector3 GetBattleRight(int battleIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Vector3 forward = GetTeamForward(battleIndex, 0);
		Quaternion quaternion = Quaternion.AngleAxis(90, Vector3.up);
		Vector3 right = quaternion * forward;
		right.Normalize();
		return right;
	}

	public Vector3 GetTeamForward(int battleIndex, int teamIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Transform teamMarker = combatMarkers[battleIndex + teamIndex];

		int opponentTeamIndex = teamIndex == 1 ? 0 : 1;
		Transform opponentTeamMarker = combatMarkers[battleIndex + opponentTeamIndex];

		Vector3 teamForward = (opponentTeamMarker.position - teamMarker.position);
		teamForward.Normalize();
		return teamForward;
	}

	public Vector3 GetTeamStartPosition(int battleIndex, int teamIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Transform teamMarker = combatMarkers[battleIndex + teamIndex];
		return teamMarker.position;
	}

	public Vector3 GetTeamStartRowPosition(int battleIndex, int teamIndex, int rowIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Vector3 teamPosition = GetTeamStartPosition(battleIndex, teamIndex);
		Vector3 teamForward = GetTeamForward(battleIndex, teamIndex);
		return teamPosition - teamForward * rowSpace * rowIndex;
	}

	public Vector3 GetStartPosition(int battleIndex, int teamIndex, int rowIndex, int columnIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Vector3 rowForward = GetInitForward();
		Vector3 rowPosition = GetTeamStartRowPosition(battleIndex, teamIndex, rowIndex);
		Vector3 battleRight = GetBattleRight(battleIndex);
		Vector3 column = rowPosition + battleRight * (-(columnCount - 1) / 2f + columnIndex) * columnSpace + rowForward * columnRandomOffset[columnIndex]
			+ battleRight * rowRandomOffset[rowIndex];
		return column;
	}

	public Vector3 GetTeamBattlePosition(int battleIndex, int teamIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Transform teamMarker = combatMarkers[battleIndex + teamIndex];

		int opponentTeamIndex = teamIndex == 1 ? 0 : 1;
		Transform opponentTeamMarker = combatMarkers[battleIndex + opponentTeamIndex];

		Vector3 teamForward = (opponentTeamMarker.position - teamMarker.position);
		teamForward.Normalize();

		Vector3 centerPos = (teamMarker.position + opponentTeamMarker.position) / 2;
		return centerPos - teamForward * teamSpace / 2;
	}

	public Vector3 GetTeamBattleRowPosition(int battleIndex, int teamIndex, int rowIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Vector3 teamPosition = GetTeamBattlePosition(battleIndex, teamIndex);
		Vector3 teamForward = GetTeamForward(battleIndex, teamIndex);
		return teamPosition - teamForward * rowSpace * rowIndex;
	}

	public Vector3 GetBattlePosition(int battleIndex, int teamIndex, int rowIndex, int columnIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Vector3 rowForward = GetInitForward();
		Vector3 rowPosition = GetTeamBattleRowPosition(battleIndex, teamIndex, rowIndex);
		Vector3 battleRight = GetBattleRight(battleIndex);
		Vector3 column = rowPosition + battleRight * (-(columnCount - 1) / 2f + columnIndex) * columnSpace + rowForward * columnRandomOffset[columnIndex]
			+ battleRight * rowRandomOffset[rowIndex];
		return column;
	}

	public Vector3 GetTeamBattleCenter(int battleIndex, int teamIndex)
	{
		battleIndex = forceUseFirstCombatMarker ? 0 : battleIndex;
		Vector3 rowForward = GetInitForward();
		Vector3 teamPosition = GetTeamBattlePosition(battleIndex, teamIndex);
		Vector3 column = teamPosition + rowForward * teamCenterOffset;
		return column;
	}

	public void EnableSceneMask(bool tf)
	{
		isEnableSceneMask = tf;

		if (maskGUITexture != null)
			maskGUITexture.enabled = tf;

		if (maskCamera != null)
			maskCamera.enabled = tf;
	}

	private bool isEnableSceneMask = false;
	public bool IsEnableSceneMask { get { return isEnableSceneMask; } }

	private void CreateTraceTargetPoint()
	{
		// Get team position
		Vector3 traceOffset = GetInitRowPosition(0) - GetInitRowPosition(1) + GetInitForward() * cameraTraceOffset;

		// Get trace point
		Plane plane = new Plane(GetInitForward(), traceOffset);
		Ray ray = new Ray(GetTeamInitPosition(), GetInitForward());

		float enter;
		if (plane.Raycast(ray, out enter) == false)
			tracePos = Vector3.zero;

		tracePos = ray.GetPoint(enter);
	}

	public bool IsNeedSponsorGoToOpponentRound()
	{
		return combatMarkers[0].position != initMarker.position;
	}

	public int GetCameraAnimationCount()
	{
		return cameraAnimaObj.Length;
	}

	public void SetAnimCameraEnable(Camera camera)
	{
		if (camera == null)
		{
			mainCamera.enabled = true;
			foreach (Camera c in cameraAnima)
			{
				c.enabled = false;
			}
		}
		else
		{
			mainCamera.enabled = false;
			foreach (Camera c in cameraAnima)
			{
				if (c == camera)
				{
					c.enabled = true;
				}
				else
				{
					c.enabled = false;
				}
			}
		}
	}

	public void SetTraceCameraPos()
	{
		mainCamera.transform.position = traceMarker.position;
		mainCamera.transform.rotation = traceMarker.rotation;
	}

	public bool NeedCameraTrace()
	{
		return mainMarker != null && traceMarker != null;
	}

	[ContextMenu("Place Camera")]
	public void PlaceCamera()
	{
		if (mainCamera != null)
		{
			mainCamera.transform.position = tracePos - mainCamera.transform.forward.normalized * cameraNormalDistance;
		}

		if (traceMarker != null)
		{
			traceMarker.position = tracePos - traceMarker.forward.normalized * cameraNormalDistance;
		}


		if (mainMarker != null)
		{
			maskCamera.transform.parent = traceMarker;
			mainMarker.localPosition = maskCamera.transform.localPosition;
			mainMarker.localRotation = maskCamera.transform.localRotation;
			maskCamera.transform.parent = mainCamera.transform;
		}
	}

	public Vector3 GetAvatarHeight()
	{
		return avatarHeight;
	}

	public Vector3 GetAvatarBattleBarOffset()
	{
		return avatarBattleBarPosOffset;
	}

	//public void OnDrawGizmos()
	//{
	//    int ROW = 8;
	//    int COLUMN = 4;

	//    Gizmos.DrawSphere(GetTeamInitPosition(), 0.1f);
	//    Gizmos.DrawLine(GetTeamInitPosition(), GetTeamInitPosition() + GetInitForward());

	//    for (int t = 0; t < 2; ++t)
	//    {
	//        for (int r = 0; r < ROW; ++r)
	//        {
	//            for (int c = 0; c < COLUMN; ++c)
	//            {
	//                Gizmos.DrawWireSphere(GetBattlePosition(0, t, r, c), 0.3f);
	//            }
	//        }
	//    }
	//}
}
