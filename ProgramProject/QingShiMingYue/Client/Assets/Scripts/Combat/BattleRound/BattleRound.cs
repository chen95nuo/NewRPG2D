using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

// 战斗播放round基类 实现通用功能
// 1，是否可以开始下一个round
// 2，保存round的状态
// 3，是否可以启动当前round
// 4，当前round是否可以结束
public abstract class BattleRound : ICameraTraceable
{
	public enum _RoundState
	{
		NotStarted,
		Running,
		Pause,
		Finished,
	}

	protected KodGames.ClientClass.RoundRecord roundRecord;
	public KodGames.ClientClass.RoundRecord RoundRecord
	{
		get { return roundRecord; }
		set { roundRecord = value; }
	}

	private static bool enableLog = false;
	public static bool EnableLog
	{
		get { return enableLog; }
		set { enableLog = value; }
	}

	private _RoundState roundState = _RoundState.NotStarted;
	public _RoundState RoundState
	{
		get { return roundState; }
		set { roundState = value; }
	}

	// This round should not be start before previous round is started
	private BattleRound prevRound = null;
	public BattleRound PrevRound
	{
		get { return prevRound; }
		set { prevRound = value; }
	}

	// This round should be start when sequenceRound CanStartAfterRound
	private BattleRound sequenceRound = null;
	public BattleRound SequenceRound
	{
		get { return sequenceRound; }
		set { sequenceRound = value; }
	}

	// This round should be start when after round finish all action
	private BattleRound afterRound = null;
	public BattleRound AfterRound
	{
		get { return afterRound; }
		set { afterRound = value; }
	}

	protected BattleRecordPlayer battleRecordPlayer;
	public BattleRecordPlayer BattleRecordPlayer
	{
		get { return battleRecordPlayer; }
	}

	protected List<BattleRole> actingRoles = new List<BattleRole>();
	public List<BattleRole> ActingRoles
	{
		get { return this.actingRoles; }
	}

	public Vector3 TracingPosition
	{
		get
		{
			if (ActingRoles.Count == 0)
				return Vector3.zero;

			// 获取前排角色
			BattleRole role = null;
			foreach (var battleRole in ActingRoles)
			{
				//剧情战中，Hide的Role可能还没有设置物理位置
				if (battleRole.Hide)
					continue;

				if (role == null)
					role = battleRole;
				else if (role.GetBattlePositionRow() < battleRole.GetBattlePositionRow())
					role = battleRole;
			}

			//剧情战斗时可能此时所有角色都没有出现
			//TODO 所有人入场前会返回zero...摄像机视角突然改变
			if (role == null)
				return Vector3.zero;

			// Get team position
			Vector3 traceOffset = BattleRecordPlayer.BattleScene.GetInitRowPosition(0) - BattleRecordPlayer.BattleScene.GetInitRowPosition(role.GetBattlePositionRow()) +
				BattleRecordPlayer.BattleScene.GetInitForward() * BattleRecordPlayer.BattleScene.cameraTraceOffset;

			// Get trace point
			Plane plane;

			if (role.IsMoving)
				plane = new Plane(BattleRecordPlayer.BattleScene.GetInitForward(), role.CachedTransform.position + traceOffset);
			else
				plane = new Plane(BattleRecordPlayer.BattleScene.GetInitForward(),
				BattleRecordPlayer.BattleScene.GetStartPosition(BattleRecordPlayer.BattleIndex, role.TeamIndex, role.GetBattlePositionRow(), role.GetBattlePositionColumn()) + traceOffset);

			Ray ray = new Ray(BattleRecordPlayer.BattleScene.GetTeamInitPosition(), BattleRecordPlayer.BattleScene.GetInitForward());

			float enter;
			if (plane.Raycast(ray, out enter) == false)
				return Vector3.zero;

			return ray.GetPoint(enter);
		}
	}

	public virtual string LogName
	{
		get
		{
			if (roundRecord != null)
				return ClientServerCommon._CombatRoundType.GetNameByType(roundRecord.RoundType);

			return this.GetType().ToString();
		}
	}

	public BattleRound(BattleRecordPlayer battleRecordPlayer)
	{
		this.battleRecordPlayer = battleRecordPlayer;
	}

	public BattleRound(KodGames.ClientClass.RoundRecord roundRecord, BattleRecordPlayer battleRecordPlayer)
	{
		this.battleRecordPlayer = battleRecordPlayer;
		this.roundRecord = roundRecord;
	}

	public virtual BattleRound Initialize()
	{
		return this;
	}

	public virtual bool CanStartAfterRound()
	{
		if (RoundState == _RoundState.NotStarted)
			return false;

		foreach (var role in actingRoles)
		{
			if (role.ActionState != BattleRole._ActionState.Idle)
				return false;
		}

		return true;
	}

	public virtual bool CanStart()
	{
		// All action in previous round should be finished
		if (sequenceRound != null && sequenceRound.RoundState != _RoundState.Finished)
			return false;

		// This round should not be start before previous round is started
		if (prevRound != null && prevRound.RoundState == _RoundState.NotStarted)
			return false;

		// This round should be start when after round CanStartAfterRound
		if (afterRound != null && afterRound.CanStartAfterRound() == false)
			return false;

		foreach (var role in actingRoles)
			if (role.ActionState != BattleRole._ActionState.Idle)
				return false;

		return true;
	}

	public virtual bool Start()
	{
		if (RoundState != _RoundState.NotStarted)
			return false;

		if (EnableLog)
			Debug.Log(LogName + " Start");

		RoundState = _RoundState.Running;
		return true;
	}

	public virtual void Pause()
	{
		if (EnableLog)
			Debug.Log(LogName + " Pause");
	}

	public virtual void Resume()
	{
		Debug.Log(LogName + " Resume");
	}

	public virtual void Finish()
	{
		if (EnableLog)
			Debug.Log(LogName + " Finish");

		RoundState = _RoundState.Finished;
	}

	protected virtual bool CheckFinishState()
	{
		Debug.Assert(RoundState != _RoundState.NotStarted || RoundState != _RoundState.Finished);
		return CanStartAfterRound();
	}

	public virtual void Update()
	{
		if (RoundState == _RoundState.NotStarted || RoundState == _RoundState.Finished)
			return;

		// Check finish
		if (CheckFinishState())
		{
			Finish();
		}
	}

	public virtual void OnDrawGizmos()
	{
	}

	public void SetCameraTrace(float distance, float duration)
	{
		if (Camera.main != null)
		{
			CameraController cameraController = Camera.main.GetComponent<CameraController>();
			if (cameraController != null)
			{
				cameraController.SetTraceTarget(this, distance, duration, BattleRecordPlayer.BattleScene.cameraInterpolateEasingType);
			}
		}
	}

	public void ClearCameraTrace()
	{
		if (Camera.main != null)
		{
			CameraController cameraController = Camera.main.GetComponent<CameraController>();
			if (cameraController != null)
				cameraController.SetTraceTarget(null, 0, 0, BattleRecordPlayer.BattleScene.cameraInterpolateEasingType);
		}
	}
}
