using UnityEngine;
using System.Collections;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

public class UIButtonMessage2 : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick,
	}

	public GameObject target;
	public string functionName;
	public int param;
	public Trigger trigger = Trigger.OnClick;
	public bool includeChildren = false;
	

	bool mStarted = false;
	bool mHighlighted = false;

	void Start () { mStarted = true; }

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnHover (bool isOver)
	{
		if (enabled)
		{
			if (((isOver && trigger == Trigger.OnMouseOver) ||
				(!isOver && trigger == Trigger.OnMouseOut))) Send();
			mHighlighted = isOver;
		}
	}

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (((isPressed && trigger == Trigger.OnPress) ||
				(!isPressed && trigger == Trigger.OnRelease))) Send();
		}
	}

	void OnClick () { if (enabled && trigger == Trigger.OnClick) Send(); }

	void OnDoubleClick () { if (enabled && trigger == Trigger.OnDoubleClick) Send(); }

	void Send ()
	{
		if (string.IsNullOrEmpty(functionName)) return;
		if (target == null) target = gameObject;

		if (includeChildren)
		{
			Transform[] transforms = target.GetComponentsInChildren<Transform>();

			for (int i = 0, imax = transforms.Length; i < imax; ++i)
			{
				Transform t = transforms[i];
//				t.gameObject.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
				t.gameObject.SendMessage(functionName, param, SendMessageOptions.DontRequireReceiver);
				
			}
		}
		else
		{
//			target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
//			target.SendMessage(functionName, param, SendMessageOptions.DontRequireReceiver);
			/**如果在中间,回调**/
			if(isInCenter())
			{
				target.SendMessage(functionName,param+"-"+gameObject.GetComponent<UILabel>().text, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
	/**litao**/
	bool isInCenter()
	{
		GameObject go=gameObject.transform.parent.gameObject.GetComponent<UICenterOnChild>().centeredObject;
		if(go==gameObject)
		{
			return true;
		}
		return false;
	}
	
}
