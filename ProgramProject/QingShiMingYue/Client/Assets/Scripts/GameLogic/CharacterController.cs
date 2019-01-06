using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class CharacterController : MonoBehaviour
{
	// Debug flag.
	protected class _LogType
	{
		public const int Move = 1 << 1;
		public const int Action = 1 << 2;
		public const int STYLE = 1 << 3;
	}

	//-------------------------------------------------------------------------
	// Log interface.
	//-------------------------------------------------------------------------

	protected static int logFlag = 0; // Log flag.

	// Move log flag.
	public static bool LogMove
	{
		get { return (logFlag & _LogType.Move) != 0; }
		set { logFlag = value ? (logFlag | _LogType.Move) : (logFlag & ~_LogType.Move); }
	}

	// Action log flag.
	public static bool LogAction
	{
		get { return (logFlag & _LogType.Action) != 0; }
		set { logFlag = value ? (logFlag | _LogType.Action) : (logFlag & ~_LogType.Action); }
	}

	// Action log flag.
	public static bool LogStyle
	{
		get { return (logFlag & _LogType.STYLE) != 0; }
		set { logFlag = value ? (logFlag | _LogType.STYLE) : (logFlag & ~_LogType.STYLE); }
	}

	protected void LogMsg(string msg)
	{
		Debug.Log(string.Format("Character({0}) : {1}", avatar.AvatarId.ToString("X8"), msg));
	}

	//-------------------------------------------------------------------------
	// Data.
	//-------------------------------------------------------------------------
	protected Avatar avatar; // Avatar.
	public Avatar Avatar
	{
		get { return avatar; }
	}

    public Vector3 Position
    {
		get { return CachedTransform.position; }
		set { CachedTransform.position = value; }
    }

	private bool hide;
	public virtual bool Hide
	{
		get { return hide; }
		set
		{
			hide = value;

			foreach (var renderer in this.GetComponentsInChildren<Renderer>())
			{
				renderer.enabled = !hide;
			}
		}
	}

	protected float moveSpeed; // Moving speed.
	protected Vector3 moveDirection; // Moving direction.	
	protected Vector3 moveTargetPosition; // Destination position.

	protected bool forwardChanged; // Forward changed.
	public bool ForwardChanged
	{
		get { return forwardChanged; }
	}

	public Vector3 Forward
	{
		get { return CachedTransform.forward; }
		set
		{
			if (value != Vector3.zero)
			{
				CachedTransform.forward = value;
				CachedTransform.forward.Normalize();

				// Mark forward changed flag.
				forwardChanged = true;
			}
		}
	}

	public Vector3 Right
	{
		get { return CachedTransform.right; }
		set
		{
			if (value != Vector3.zero)
			{
				CachedTransform.right = value;
				CachedTransform.right.Normalize();
			}
		}
	}

	public Vector3 Up
	{
		get { return CachedTransform.up; }
		set
		{
			if (value != Vector3.zero)
			{
				CachedTransform.up = value;
				CachedTransform.up.Normalize();
			}
		}
	}

	// Moving flag.
	protected bool isMoving;
	public bool IsMoving
	{
		get { return isMoving; }
	}

	// Current action id.
	protected int curActID = IDSeg.InvalidId;
	public int CurActID
	{
		get { return curActID; }
	}

	protected int avatarAssetId = IDSeg.InvalidId;
	public int AvatarAssetId
	{
		get { return avatarAssetId; }
		set {avatarAssetId = value;}
	}

	public bool IsLoopAction
	{
		get { return Avatar.IsLoopAnim(); }
	}

	//-------------------------------------------------------------------------
	// Basic interface.
	//-------------------------------------------------------------------------
	public virtual void Awake()
	{
		avatar = GetComponent<Avatar>();
	}

	public virtual void Update()
	{
		// Update movement.
		if (isMoving)
			UpdateMovement();
	}

	public virtual void PreLoadAnimation(int actionId)
	{
		AvatarAction action = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actionId);
		if (action == null)
			return;

		Avatar.PreLoadAnimation(action.GetAnimationName(avatarAssetId));
	}

	public virtual bool PlayAction(int actionId)
	{
		if (Avatar != null && Avatar.AvatarAnimation != null && Avatar.AvatarAnimation.PlayingAnimation != null && Avatar.AvatarAnimation.PlayingAnimation.animationName.Contains("Die"))
		{
			return false;
		}

		// Stop last action.
		StopAction();

		// Play the animation of this action.
		AvatarAction action = ConfigDatabase.DefaultCfg.ActionConfig.GetActionById(actionId);
		if (action == null)
			return false;

		if (action.GetAnimationName(avatarAssetId) == "")
		{
			Debug.LogError(string.Format("Animation name is empty in action({0})", action.id.ToString("X8")));
			return false;
		}

		if (!Avatar.PlayAnim(action.GetAnimationName(avatarAssetId)))
			return false;

		// Set animation finish callback.
		Avatar.SetAnimationFinishDeletage(OnAnimationFinished, null, null);

		// Save the current action ID.
		curActID = actionId;

		// Log action.
		if (LogAction)
			LogMsg(String.Format("PlayAction actionId:{0:X} actType:{1:X} animation:{2}", actionId, AvatarAction._Type.GetNameByType(action.actionType), action.GetAnimationName(avatarAssetId)));

		return true;
	}

	public virtual void StopAction()
	{
		if (LogAction)
			LogMsg(String.Format("StopAction"));

		// Stop animation.
		Avatar.StopAnim();

		// If is moving, stopped it.
		if (isMoving)
			StopMove();

		// Rest current action id.
		curActID = IDSeg.InvalidId;
	}

    public void MoveToWithoutAdjustAnimation(Vector3 targetPosition, float speed)
    {
        if (targetPosition == this.Position)
        {
            Debug.LogWarning("Move target is the same as current position");
            return;
        }

        if (LogMove)
            LogMsg(String.Format("MoveTo target:{0} speed:{1}", targetPosition, speed));

        moveSpeed = speed;
        moveTargetPosition = targetPosition;
        isMoving = true;
        moveDirection = moveTargetPosition - this.Position;
        moveDirection.Normalize();
    }

	public void MoveTo(Vector3 targetPosition, float speed)
	{
        MoveToWithoutAdjustAnimation(targetPosition,speed);

		// Adjust move animation duration.
        Avatar.SetAnimDurationByMoveTime(GetMoveDuration());
	}

	public void StopMove()
	{
		if (LogMove)
			LogMsg(String.Format("StopMove"));

		moveSpeed = 0;
		isMoving = false;
	}

	// Get move duration time.
	public float GetMoveDuration()
	{
		if (moveSpeed <= 0)
			return 0;

		return Vector3.Distance(this.Position, moveTargetPosition) / moveSpeed;
	}

	// Update movement.
	protected virtual void UpdateMovement()
	{
		if (moveSpeed == 0)
			return;

		// Map the animation.
		if (!Avatar.IsAnimMoveBegin())
			return;

		if (LogMove)
			LogMsg(String.Format("Move speed:{0:f} moveTargetPosition:{1}", moveSpeed, moveTargetPosition.ToString()));

		Vector3 oldPos = this.Position;
		MoveForward(moveSpeed * Time.deltaTime);
		Vector3 newPos = this.Position;

		Vector3 oldDir = moveTargetPosition - oldPos;
		oldDir.Normalize();

		Vector3 newDir = moveTargetPosition - newPos;
		newDir.Normalize();

		//UnityEngine.Debug.DrawLine( CachedTransform.position, CachedTrans.position + moveDirection * 10, Color.red );
		//UnityEngine.Debug.DrawLine( CachedTransform.position, CachedTrans.position + oldDir * 10, Color.blue );
		//UnityEngine.Debug.DrawLine( CachedTransform.position, CachedTrans.position + newDir * 10, Color.green );

		//Debug.Log( "ideal dot=" + Vector3.Dot( moveDirection, oldDir ) );
		//Debug.Log( "actual dot=" + Vector3.Dot( moveDirection, newDir ) );

		if (Vector3.Dot(moveDirection, moveTargetPosition - newPos) < 0)
		{
			if (LogMove)
				LogMsg("Move finished");

			moveSpeed = 0;
			CachedTransform.position = moveTargetPosition;
			isMoving = false;
		}
	}

	protected void MoveForward(float dis)
	{
		CachedTransform.Translate(moveDirection * dis, Space.World);
	}

	protected virtual void OnAnimationFinished(object userData0, object userData1)
	{
		if (LogAction)
			LogMsg(String.Format("AnimFinish actionId:{0:X}", curActID));

		curActID = IDSeg.InvalidId; // Current action id.
	}
}