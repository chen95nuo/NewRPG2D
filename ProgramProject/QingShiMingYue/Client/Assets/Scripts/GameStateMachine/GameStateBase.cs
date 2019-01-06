using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public abstract class GameStateBase : MonoBehaviour
{
	public abstract bool IsGamingState { get; }

	public virtual void Create(object userData) { }
	public virtual void Dispose() { }

	public virtual void PrepareEnter() { }
	public virtual void Enter() { }
	public virtual void DoneEnter() { }

	public virtual void PrepareExit() { }
	public virtual void Exit() { }
	public virtual void DoneExit() { }

	public virtual void OnUpdate() { }
	public virtual void OnGUIUpdate() { } // To show engine GUI

#if UNITY_ANDROID
	public abstract void OnEscape();
#endif

	private SysGameStateMachine sysGameStateMachine;
	public SysGameStateMachine SysGameStateMachine
	{
		get { return sysGameStateMachine; }
		set { sysGameStateMachine = value; }
	}
}
