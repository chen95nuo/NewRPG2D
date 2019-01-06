using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIPnlTipFlow : UIModule
{
	public SpriteText message;
	public UIButton messageBg;
	public float textFadeDuration;
	public float textFadeDelay;

	private const float SINGLE_LINE_BG_HEIGHT = 32f;
	private const float SINGLE_LINE_HEIGHT = 14.2f;
	private const float BG_EXCEED = SINGLE_LINE_BG_HEIGHT - SINGLE_LINE_HEIGHT;
	private Animation tipAnimation;
	private float animationTimer = 0.6f;

	private FadeTextAlpha fadeTextAlpha;



	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;
		tipAnimation = messageBg.GetComponent<Animation>();

		message.Text = (string)userDatas[0];

		tipAnimation.Play("UI_TipsFlow_Show");

		messageBg.SetSize(messageBg.width, BG_EXCEED + SINGLE_LINE_HEIGHT * message.GetDisplayLineCount());



		if (userDatas.Length > 1)
		{
			textFadeDelay = (float)userDatas[1] + animationTimer;
			textFadeDuration = (float)userDatas[1] + animationTimer;
		}
		else
		{
			textFadeDelay = animationTimer;
			textFadeDuration = animationTimer;
		}

		fadeTextAlpha = FadeTextAlpha.Do(message, EZAnimation.ANIM_MODE.FromTo, new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f),
			EZAnimation.GetInterpolator(EZAnimation.EASING_TYPE.Linear), textFadeDuration, textFadeDelay, null, null);
		StartCoroutine("PlayAnimationDisappear", textFadeDelay);
		Invoke("OnHide", (textFadeDelay + animationTimer));

		return true;
	}

	public void StopTipsIEnumerator()
	{
		StopAnimation();
		if (fadeTextAlpha != null)
			fadeTextAlpha.Stop();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator PlayAnimationDisappear(float timer)
	{

		yield return new WaitForSeconds(timer);
		tipAnimation.Stop("UI_TipsFlow_Show");
		tipAnimation.Play("UI_TipsFlow_Disappear");

	}

	private void StopAnimation()
	{
		if (tipAnimation != null)
		{
			tipAnimation.Stop("UI_TipsFlow_Show");
			tipAnimation.Stop("UI_TipsFlow_Disappear");
		}
		

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public override void OnHide()
	{
		ResetUIColorAndStopAnimation();
		fadeTextAlpha.Clear();
		EZAnimator.instance.StopAnimation(fadeTextAlpha, true);
		StopAnimation();
		base.OnHide();
		SysUIEnv.Instance.ModulePool.ReleaseModule(typeof(UIPnlTipFlow), this, false);
	}

	private void ResetUIColorAndStopAnimation()
	{
		StopTipsIEnumerator();
		if (messageBg != null)
		{
			foreach (Material material in messageBg.GetComponent<MeshRenderer>().materials)
				material.color = new Color(1f, 1f, 1f, 1f);

			foreach (Material material in message.GetComponent<MeshRenderer>().materials)
				material.color = new Color(1f, 1f, 1f, 1f);
			messageBg.Color = new Color(1f, 1f, 1f, 1f);
			message.Color = new Color(240f / 255f, 208f / 255f, 142f / 255f, 1f);
		}
	}
}