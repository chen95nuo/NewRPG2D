using System.Collections.Generic;
using UnityEngine;

public class SysGameStateMachine : SysModule, SysSceneManager.ISceneManagerListener
{
	public static SysGameStateMachine Instance
	{
		get { return SysModuleManager.Instance.GetSysModule<SysGameStateMachine>();}
	}
	
	private GameStateBase currentState;
	public GameStateBase CurrentState
	{
		get { return currentState; }
	}

	public override void OnUpdate()
	{
		if (currentState != null)
			currentState.OnUpdate();
	}

	public override void OnGUIUpdate()
	{
		if (currentState != null)
			currentState.OnGUIUpdate();
	}

	public void OnSceneWillChange(SysSceneManager manager, string currentScene, string newScene)
	{
		// Free memory
		// Dispose UI modules.
		SysUIEnv uiEvn = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		if (uiEvn != null)
			uiEvn.DisposeUIModules();

		// Memory leak!
//		// 清空缓存的字体纹理, 这个时候在场景中应该没有任何的spirtetext
//		FontManager.instance.FreeMemory();
//
//		// 如果正在显示Loading界面, 显示tips
//		SysUIEnv.Instance.AfterFreeText();

		GameMain.Inst.FreeMemory();
	}

	public void OnSceneChanged(SysSceneManager manager, string oldScene, string currentScene)
	{

	}

	public T EnterState<T>() where T : GameStateBase
	{
		return EnterState<T>(null);
	}

	public T EnterState<T>(object userData) where T : GameStateBase
	{
		return EnterState<T>(userData,false);
	}

	//force : (true)enter state as first enter (false) otherwise
	public T EnterState<T>(object userData, bool force) where T : GameStateBase
	{
		if (force == false)
		{
			if (currentState != null && typeof(T) == currentState.GetType())
				return CurrentState as T;
		}

//#if UNITY_EDITOR
		Debug.Log("EnterState " + typeof(T));
//#endif

		// Dispose systems.
		SysModuleManager.Instance.DisposeSysMdls(false);

		GameStateBase oldState = currentState;

		currentState = gameObject.AddComponent<T>();
		currentState.SysGameStateMachine = this;
		currentState.Create(userData);

		// Prepare
		if (oldState != null)
			oldState.PrepareExit();

		currentState.PrepareEnter();

		// Do change state
		if (oldState != null)
			oldState.Exit();

		currentState.Enter();

		// Done
		if (oldState != null)
		{
			oldState.DoneExit();
			oldState.Dispose();
		}

		currentState.DoneEnter();

		Object.Destroy(oldState);

		return CurrentState as T;
	}

	public T GetCurrentState<T>() where T : GameStateBase
	{
		return currentState as T;
	}

	public GameStateBase GetCurrentState()
	{
		return currentState;
	}
}