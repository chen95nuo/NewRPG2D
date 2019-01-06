using UnityEngine;
using System.Collections;
using ClientServerCommon;
using KodGames;

// Login background.
public class UIPnlLoading : UIModule
{
	public EZAnimation.EASING_TYPE enterAnimation;
	public float enterAnimationDuring = 0f;

	public EZAnimation.EASING_TYPE exitAnimation;
	public float exitAnimationDuring = 0f;

	public Color beginColor;
	public Color destColor;
	public UIButton bgBtn;
	public UIBox[] otherCtrls;
	public UIBox tipsBox;

	public UIElemAssetIcon backGround;

	public delegate void AnimCompletionDelegate();
	private AnimCompletionDelegate showAnimCompletionDel;
	private AnimCompletionDelegate hideAnimCompletionDel;
	private bool suppressAnim = false;

	public static void ShowPanel(AnimCompletionDelegate showAnimCompletionDel, AnimCompletionDelegate hideAnimCompletionDel)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlLoading)))
		{
			var uiPnlLoading = SysUIEnv.Instance.GetUIModule<UIPnlLoading>();

			// Call previous hide delegate
			if (uiPnlLoading.hideAnimCompletionDel != null)
				uiPnlLoading.hideAnimCompletionDel();

			// Call current show delegate
			if (showAnimCompletionDel != null)
			{
				showAnimCompletionDel();
				uiPnlLoading.showAnimCompletionDel = null; // Set showAnimCompletionDel is null, due to the delegate has been called.
			}

			uiPnlLoading.hideAnimCompletionDel = hideAnimCompletionDel;
		}
		else
		{
			var uiPnlLoading = SysUIEnv.Instance.GetUIModule<UIPnlLoading>();
			if (uiPnlLoading != null)
			{
				uiPnlLoading.showAnimCompletionDel = showAnimCompletionDel;
				uiPnlLoading.hideAnimCompletionDel = hideAnimCompletionDel;
			}

			SysUIEnv.Instance.ShowUIModule<UIPnlLoading>();
		}
	}

	public static void HidePanel(bool suppressAnim)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlLoading)) == false)
			return;

		SysUIEnv.Instance.GetUIModule<UIPnlLoading>().suppressAnim = suppressAnim;
		SysUIEnv.Instance.HideUIModule<UIPnlLoading>();
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (SysUIEnv.Instance != null)
			SysUIEnv.Instance.PauseShowEvent = true;

		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Pause = true;

		if (SysAssistant.Instance != null)
			SysAssistant.Instance.Pause = true;

		int loadingId = Random.Range(0, ConfigDatabase.DefaultCfg.GameConfig.loadingSettings.Count);
		backGround.SetData(ConfigDatabase.DefaultCfg.GameConfig.loadingSettings[loadingId].id);

		ShowTips();

		FadeSpriteAlpha.Do(bgBtn, EZAnimation.ANIM_MODE.FromTo, beginColor, destColor, EZAnimation.GetInterpolator(enterAnimation), enterAnimationDuring, 0f, null, EnterCompletionDelegate);
		foreach (var ctrl in otherCtrls)
			FadeSpriteAlpha.Do(ctrl, EZAnimation.ANIM_MODE.FromTo, beginColor, destColor, EZAnimation.GetInterpolator(enterAnimation), enterAnimationDuring, 0f, null, null);
		FadeTextAlpha.Do(tipsBox.spriteText, EZAnimation.ANIM_MODE.FromTo, beginColor, destColor, EZAnimation.GetInterpolator(enterAnimation), enterAnimationDuring, 0f, null, null);

		return true;
	}

	public override void OnHide()
	{
		if (suppressAnim)
		{
			// Force to call ExitCompletionDelegate
			ExitCompletionDelegate(null);
		}
		else
		{
			FadeSpriteAlpha.Do(bgBtn, EZAnimation.ANIM_MODE.FromTo, destColor, beginColor, EZAnimation.GetInterpolator(exitAnimation), exitAnimationDuring, 0, null, ExitCompletionDelegate);
			foreach (var ctrl in otherCtrls)
				FadeSpriteAlpha.Do(ctrl, EZAnimation.ANIM_MODE.FromTo, destColor, beginColor, EZAnimation.GetInterpolator(exitAnimation), exitAnimationDuring, 0f, null, null);
			FadeTextAlpha.Do(tipsBox.spriteText, EZAnimation.ANIM_MODE.FromTo, destColor, beginColor, EZAnimation.GetInterpolator(exitAnimation), exitAnimationDuring, 0f, null, null);
		}
	}

	private void ShowTips()
	{
		var tipList = ConfigDatabase.DefaultCfg.StringsConfig.GetAllStringsInBlock(GameDefines.strBlkLoadingTips);
		tipsBox.Hide(tipList.Count == 0);
		if (tipsBox.IsHidden() == false)
		{
			Random.seed = (int)System.DateTime.Now.Ticks;
			tipsBox.Text = tipList[Random.Range(0, tipList.Count)];
		}
	}	

	private void ExitCompletionDelegate(EZAnimation anim)
	{
		if (hideAnimCompletionDel != null)
		{
			hideAnimCompletionDel();
			hideAnimCompletionDel = null;
		}

		if (SysUIEnv.Instance != null)
			SysUIEnv.Instance.PauseShowEvent = false;

		if (SysTutorial.Instance != null)
			SysTutorial.Instance.Pause = false;

		if (SysAssistant.Instance != null)
			SysAssistant.Instance.Pause = false;

		base.OnHide();
	}

	private void EnterCompletionDelegate(EZAnimation anim)
	{
		if (showAnimCompletionDel != null)
		{
			showAnimCompletionDel();
			showAnimCompletionDel = null;
		}
	}
}