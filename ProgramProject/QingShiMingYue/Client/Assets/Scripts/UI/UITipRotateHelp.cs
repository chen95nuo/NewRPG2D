using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UITipRotateHelp : UIModule
{
	protected bool isDrag;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		AddInputDelegate();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		isDrag = false;
		RemoveInputDelegate();
	}

	protected void Update()
	{
		if (isDrag)
			HideSelf();
	}

	protected void AddInputDelegate()
	{
		UIManager.instance.AddMouseTouchPtrListener(MouseTouchPtrListener);
	}

	protected void RemoveInputDelegate()
	{
		UIManager.instance.RemoveMouseTouchPtrListener(MouseTouchPtrListener);
	}

	protected virtual void MouseTouchPtrListener(POINTER_INFO data)
	{
		if (data.evt == POINTER_INFO.INPUT_EVENT.MOVE || data.evt == POINTER_INFO.INPUT_EVENT.NO_CHANGE)
			return;

		if (data.evt == POINTER_INFO.INPUT_EVENT.DRAG)
			isDrag = true;
	}

}
