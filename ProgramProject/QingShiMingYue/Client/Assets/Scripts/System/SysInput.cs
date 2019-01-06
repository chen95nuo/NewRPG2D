using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Input system to manage game input. 
/// </summary>
public class SysInput : SysModule
{
	// Delegate definition for receiving input info
	public delegate void InputDelegate(POINTER_INFO ptr);

	public override bool Initialize()
	{
		UIManager.instance.AddMouseTouchPtrListener(PointerInfoDelegate);

		return true;
	}

	public void AddInputDelegate(InputDelegate cb)
	{
		inpCb += cb;
	}

	public void RemoveInputDelegate(InputDelegate cb)
	{
		inpCb -= cb;
	}

	public void LockInput()
	{
		UIManager.instance.LockInput();
	}

	public void UnlockInput()
	{
		UIManager.instance.UnlockInput();
	}

	// Delegate definition for receiving pointer info
	protected void PointerInfoDelegate(POINTER_INFO ptr)
	{
		if (inpCb != null)
			inpCb(ptr);
	}

	protected InputDelegate inpCb;
}
