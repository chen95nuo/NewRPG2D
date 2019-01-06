using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Delay object base class.
public abstract class DelayObject
{
	// Delay callback.
	public delegate void DelayDelegate(object userData0, object userData1);

	public virtual string LogName
	{
		get { return this.GetType().ToString(); }
	}

	public DelayObject(DelayDelegate delayDelegate, object userData0, object userData1)
	{
		this.delayDelegate = delayDelegate;
		this.userData0 = userData0;
		this.userData1 = userData1;
	}

	public abstract bool IsOver();
	public virtual void Callback()
	{
		if (delayDelegate != null)
			delayDelegate(userData0, userData1);
	}

	private DelayDelegate delayDelegate; // Delay callback.
	private object userData0;
	private object userData1;
}

// Delay timer.
public class DelayTime : DelayObject
{
	public override string LogName
	{
		get { return this.GetType().ToString() + " delayTime " + delayTime.ToString(); }
	}

	private float timer;
	private float delayTime; // Delay timer.

	public DelayTime(float pDlyTime, DelayDelegate delayDelegate, object userData0, object userData1)
		: base(delayDelegate, userData0, userData1)
	{
		timer = 0;
		delayTime = pDlyTime;
	}

	public override bool IsOver()
	{
		timer += Time.deltaTime;
		return timer > delayTime;
	}
}

// Wait for game object destroy
public class DelayGameObject : DelayObject
{
	private GameObject delayObject;

	public DelayGameObject(GameObject obj, DelayDelegate delayDelegate, object userData0, object userData1)
		: base(delayDelegate, userData0, userData1)
	{
		delayObject = obj;
	}

	public override bool IsOver()
	{
		if (delayObject == null)
		{
			return true;
		}

		//if(delayObject.GetComponent<FXController>() != null && delayObject.GetComponent<FXController>().InPool)
		//{
		//    return true;
		//}

		return false;
	}
}

public class DelayFX : DelayObject
{
	private FXController fx;
	private bool finished = false;

	public DelayFX(FXController fx, DelayDelegate delayDelegate, object userData0, object userData1)
		: base(delayDelegate, userData0, userData1)
	{
		this.fx = fx;

		fx.AddFinishCallback(FxFinishCallback, null);
	}

	public override bool IsOver()
	{
		// Check if fx has been destroyed
		return fx == null || finished;
	}

	private void FxFinishCallback(object userData)
	{
		finished = true;
	}
}

public class DelayAnimCurveTranslateEnd : DelayObject
{
	private bool reachEnd = false;
	private KodGames.AnimCurve animCurve;

	public DelayAnimCurveTranslateEnd(KodGames.AnimCurve animCurve, DelayDelegate delayDelegate, object userData0, object userData1)
		: base(delayDelegate, userData0, userData1)
	{
		reachEnd = false;
		animCurve.SetTranslateEndDelegate(OnTranslateEnd, null);
	}

	public override bool IsOver()
	{
		return reachEnd;
	}

	private void OnTranslateEnd(object userData)
	{
		reachEnd = true;
	}
}

// Delay battle role.
public class DelayBattleRole : DelayObject
{
	private BattleRole delayRole;

	public DelayBattleRole(BattleRole role, DelayDelegate delayDelegate, object userData0, object userData1)
		: base(delayDelegate, userData0, userData0)
	{
		delayRole = role;
	}

	public override bool IsOver()
	{
		if (delayRole.ActionState == BattleRole._ActionState.Idle)
		{
			return true;
		}

		return false;
	}
}

// When SyngeneticDelayGameObject IsOver BattleRole will immediate enter idle state
public class SyngeneticDelayGameObject
{
	private DelayObject delayObject = null;

	public SyngeneticDelayGameObject(DelayObject delayObject)
	{
		this.delayObject = delayObject;
	}

	public bool IsOver()
	{
		return delayObject.IsOver();
	}
}

//剧情战斗用。剧情对话DelayObject
public class DialogueDelayObject : DelayObject
{
	//获取对话是否完成，由DialogueEvent传过来
	System.Func<bool> IsDialogClosed;

	public DialogueDelayObject(System.Func<bool> isDialogClosed, DelayDelegate delayDelegate, object userData0, object userData1)
		: base(delayDelegate, userData0, userData1)
	{
		this.IsDialogClosed = isDialogClosed;
	}

	//对话是否完毕
	public override bool IsOver()
	{
		return IsDialogClosed();
	}
}
