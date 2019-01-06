using UnityEngine;
using System.Collections;

public class UIEffectPowerUp : UIModule
{
	public enum LabelType
	{
		None,
		Success,
		Crit
	}

	public AutoSpriteControlBase effectOneBtn;
	public AutoSpriteControlBase effectTwoBtn;
	public UIElemAssetIcon itemOneIcon;
	public UIElemAssetIcon itemTwoIcon;
	public float effectDelayTime;

	public GameObject shakeAnimLight;

	public Animation successAnim;
	public Animation shinningAnim;

	public GameObject successObj;
	public GameObject critObj;

	public float hideAnimDuration;

	public delegate void EffectModuleHideCallback(params object[] userDatas);
	private EffectModuleHideCallback hideCallback;
	private object[] callbackDatas;
	private Animation shakeAnim;

	private LabelType labelType = LabelType.None;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		int assetId = (int)userDatas[0];
		labelType = (LabelType)userDatas[1];

		bool showOneEffectBtn = true;
		if (userDatas.Length > 2)
			showOneEffectBtn = (bool)userDatas[2];
		else
			showOneEffectBtn = false;

		effectOneBtn.gameObject.SetActive(showOneEffectBtn);
		effectTwoBtn.gameObject.SetActive(!showOneEffectBtn);

		shakeAnim = showOneEffectBtn ? effectOneBtn.animation : effectTwoBtn.animation;

		SetUICtrls(assetId);
		return true;
	}

	public void SetEffectHideCallback(EffectModuleHideCallback callback, params object[] userDatas)
	{
		hideCallback = callback;
		callbackDatas = userDatas;
	}

	public override void OnHide()
	{
		base.OnHide();

		// Hide And Stop Animation.
		StopCoroutine("BeginEffect");
		shakeAnim.Stop();
		shinningAnim.Stop();

		if (hideCallback != null)
			hideCallback(callbackDatas);

		// Set Callback Default Data.
		hideCallback = null;
		callbackDatas = null;
	}

	private void SetUICtrls(int assetId)
	{
		// Play Particle.
		shakeAnimLight.SetActive(true);
		shakeAnimLight.GetComponent<ParticleSystem>().Play();

		shakeAnim.Play();

		successAnim.gameObject.SetActive(false);
		shinningAnim.Play();

		if (effectOneBtn.gameObject.activeInHierarchy)
			itemOneIcon.SetData(assetId);
		else
			itemTwoIcon.SetData(assetId);

		effectOneBtn.controlIsEnabled = false;
		StartCoroutine("BeginEffect");
		Invoke("HideSelf", hideAnimDuration);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator BeginEffect()
	{
		yield return new WaitForSeconds(effectDelayTime);

		// Stop Particle.
		shakeAnimLight.SetActive(false);
		shakeAnimLight.GetComponent<ParticleSystem>().Stop();

		successAnim.gameObject.SetActive(true);

		effectOneBtn.controlIsEnabled = true;
		shakeAnim.Stop();
	}

	private void Update()
	{
		if (!this.IsShown || this.IsOverlayed)
			return;

		switch (labelType)
		{
			case LabelType.None:
				successObj.SetActive(false);
				critObj.SetActive(false);
				break;
			case LabelType.Success:
				successObj.SetActive(true);
				critObj.SetActive(false);
				break;
			case LabelType.Crit:
				successObj.SetActive(false);
				critObj.SetActive(true);
				break;
		}
	}
}
