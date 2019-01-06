using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGamePreamble : UIModule
{
	public UIElemGamePreamble[] preamblePage;
	public AutoSpriteControlBase skipPageControl;
	public float delayShowSkipTime = 1.0f;

	public EZAnimation.EASING_TYPE alphaEasing;
	public Color alphaColor;
	public float alphaDuration;

	private int currentPageIndex;
	private bool skipPageControlShown = false;

	public override bool Initialize()
	{
		if (base.Initialize() == false)
			return false;

		// Initialize SpriteText's Text.
		foreach (var label in this.GetComponentsInChildren<SpriteText>(true))
			if (!string.IsNullOrEmpty(label.localizingKey))
				label.Text = GameUtility.GetUIString(label.localizingKey);

		for (int i = 0; i < preamblePage.Length; ++i)
			preamblePage[i].borderButton.Data = i;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		EnableMaskV(true);
		ShowPage(0);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		AudioManager.Instance.StopMusic();
	}

	private void EnableMaskV(bool enable)
	{
		object screenMask = GameObject.FindObjectOfType(typeof(UIScreenMask));

		if (screenMask != null)
			(screenMask as UIScreenMask).EnableBorderV = enable;
	}

	private void ShowPage(int page)
	{
		currentPageIndex = page;

		// Show page
		preamblePage[currentPageIndex].gameObject.SetActive(true);
		preamblePage[currentPageIndex].borderButton.controlIsEnabled = preamblePage[currentPageIndex].enableBorderAtStart;

		// Hide other pages
		for (int i = 0; i < preamblePage.Length; ++i)
			if (i != currentPageIndex)
				preamblePage[i].gameObject.SetActive(false);

		HideContinueText();
	}

	private void Update()
	{
		var currentPage = preamblePage[currentPageIndex];
		if (currentPage.IsPlaying() == false && IsContinueTextShown() == false)
			ShowContinueText(0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickPage(UIButton btn)
	{
		var currentPage = preamblePage[currentPageIndex];

		if (currentPage != null && currentPage.IsPlaying())
		{
			if (currentPage.IsPausing())
				currentPage.SetPause(false); // Resume
			else if (currentPage.SkipToStep()) // Skip and return whether paused
				ShowContinueText(delayShowSkipTime);
		}
		else
		{
			currentPageIndex++;

			if (currentPageIndex < preamblePage.Length)
				ShowPage(currentPageIndex);

			if (currentPageIndex == preamblePage.Length - 1)
			{
				new FadeTextAlpha().Start(preamblePage[currentPageIndex].borderButton.spriteText,
								  EZAnimation.ANIM_MODE.FromTo,
								  alphaColor,
								  EZAnimation.GetInterpolator(alphaEasing),
								  alphaDuration,
								  0f,
								  null,
								  (anim) =>
								  {
									  EnableMaskV(false);
									  RequestMgr.Inst.Request(new NoviceCombatReq());
								  });
			}

		}
	}

	private bool IsContinueTextShown()
	{
		return skipPageControlShown;
	}

	private void ShowContinueText(float delayShowTime)
	{
		skipPageControlShown = true;
		if (delayShowTime > 0)
		{
			preamblePage[currentPageIndex].borderButton.controlIsEnabled = false;
			Invoke("DoShowContinueText", delayShowTime);
		}
		else
		{
			DoShowContinueText();
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void DoShowContinueText()
	{
		preamblePage[currentPageIndex].borderButton.controlIsEnabled = true;
		skipPageControl.Hide(false);
	}

	private void HideContinueText()
	{
		skipPageControl.Hide(true);
		skipPageControlShown = false;
	}
}