using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UITipDragHelp : UIModule
{
	public GameObject arrowObject;
	public EZAnimation.EASING_TYPE animationEasingType;

	private GameObject attachObj;
	private GameObject fromObj;
	private Vector3 arrowAttachPosition = Vector3.zero;
	private Vector3 arrowFromPosition = Vector3.zero;
	private const float animationDuration = 2.0f;
	private const float positionChangeValue = 20.0f;

	private ClientServerCommon.TutorialConfig.Action action;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ArrowPositon_x") &&
		SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.ContainerValue("ArrowPositon_y"))
		{
			float clientValue_x = (float)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ArrowPositon_x");
			float clientValue_y = (float)SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.GetValue("ArrowPositon_y");

			arrowObject.transform.position = new Vector3(clientValue_x, clientValue_y, arrowObject.transform.position.z);
		}

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("ArrowPositon_x", arrowObject.transform.position.x);
		SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("ArrowPositon_y", arrowObject.transform.position.y);

		StopVoices();

		attachObj = null;
		fromObj = null;
	}

	private void Update ()
	{
		if (!fromObj || !attachObj)
			return;

		if (arrowAttachPosition == Vector3.zero)
			return;

		if (arrowObject.transform.position==new Vector3(arrowAttachPosition.x,arrowAttachPosition.y,arrowObject.transform.position.z))
		{
			arrowObject.transform.position = new Vector3(arrowFromPosition.x, arrowFromPosition.y, arrowFromPosition.z);
			AnimatePosition.Do(arrowObject, EZAnimation.ANIM_MODE.FromTo,
				arrowObject.transform.position,
				arrowAttachPosition,
				EZAnimation.GetInterpolator(animationEasingType),
				animationDuration,
				0f, null, null);
		}
	}

	public void ShowHelp(ClientServerCommon.TutorialConfig.Action action)
	{
		if (ShowSelf() == false)
			return;

		this.action = action;

		if (action.strValue.Equals("") || action.attachComponentName.Equals(""))
			Debug.LogError("Action Tag is null");

		fromObj = GameObject.FindWithTag(this.action.strValue);
		attachObj = GameObject.FindWithTag(this.action.attachComponentName);

		arrowAttachPosition = new Vector3(attachObj.transform.position.x, attachObj.transform.position.y - positionChangeValue, attachObj.transform.localPosition.z);
		arrowObject.transform.position = new Vector3(fromObj.transform.position.x, fromObj.transform.position.y - positionChangeValue, attachObj.transform.localPosition.z);
		arrowFromPosition = new Vector3(fromObj.transform.position.x, fromObj.transform.position.y - positionChangeValue, attachObj.transform.localPosition.z);

		AnimatePosition.Do(arrowObject, EZAnimation.ANIM_MODE.FromTo,
							arrowObject.transform.position,
							arrowAttachPosition,
							EZAnimation.GetInterpolator(animationEasingType),
							animationDuration,
							0f, null, null);

		PlayerVoices();
	}

	private void PlayerVoices()
	{
		if (!string.IsNullOrEmpty(action.voice))
			AudioManager.Instance.PlayStreamSound(action.voice, 0.1f);
	}

	private void StopVoices()
	{
		AudioManager.Instance.StopSound(action.voice);
	}


	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClose(UIButton btn)
	{
		if (action.tapClose)
		{
			HideSelf();

			if (action.tapCloseUIType != ClientServerCommon._UIType.UnKonw)
				SysUIEnv.Instance.ShowUIModule(action.tapCloseUIType);
		}
	}
}
