using System;
using System.Collections.Generic;
using UnityEngine;

public class UITipAssistant : UIModule
{
	public UIButton effectButton;
	private FXController fxControll;

	private Transform origianlEffParent;
	public bool isTutorialObj;
	public bool pressControl;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		origianlEffParent = effectButton.CachedTransform.parent;
		isTutorialObj = false;
		pressControl = false;
		return true;
	}

	public override void OnHide()
	{
		if (fxControll != null)
			GameObject.Destroy(fxControll.gameObject);

		effectButton.CachedTransform.parent = origianlEffParent;
		effectButton.Data = null;
		base.OnHide();
	}

	public void ShowAssistant(AutoSpriteControlBase attachObj)
	{
		if (ShowSelf() == false)
			return;

		// Set Particle
		fxControll = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, GameDefines.uiFx_Assistant)).GetComponent<FXController>();
		fxControll.gameObject.SetActive(false);

		AutoFollowObject autoFollow = fxControll.GetComponent<AutoFollowObject>();
		if (autoFollow == null)
			autoFollow = fxControll.gameObject.AddComponent<AutoFollowObject>();

		autoFollow.SetTarget(attachObj.gameObject, Vector3.zero);
		fxControll.gameObject.SetActive(true);

		// Set Effect Button.
		effectButton.Data = attachObj;
		effectButton.SetSize(attachObj.width, attachObj.height);
		effectButton.anchor = attachObj.anchor;
		InvokeRepeating("KeepEffectButtonPosition", 0, 0.1f);
	}

	public void IsTutorialObjTag(string tag)
	{
		if (String.IsNullOrEmpty((effectButton.Data as AutoSpriteControlBase).tag))
			return;

		if ((effectButton.Data as AutoSpriteControlBase).tag.Equals(tag))
			isTutorialObj = true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void KeepEffectButtonPosition()
	{
		if (fxControll == null)
			return;

		effectButton.CachedTransform.parent = null;
		effectButton.CachedTransform.position = fxControll.transform.position;
		effectButton.CachedTransform.localPosition = new Vector3(effectButton.CachedTransform.localPosition.x, effectButton.CachedTransform.localPosition.y, -0.001f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEffectButton(UIButton btn)
	{
		var targetButton = btn.Data as UIButton;
		UIObjectEx.InvokeMethod(targetButton.scriptWithMethodToInvoke, targetButton.methodToInvoke, targetButton);

		if (isTutorialObj)
			SysLocalDataBase.Inst.LocalPlayer.ClientDynamicValue.AddDynamicValue("PressControl", true);

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}
