using System;
using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemCentralCityTempItem : MonoBehaviour
{
	public enum CountDownType
	{
		Undefined = 1,
		Second = 2,
		Minute = 3,
		//Hour = 4, // hour currently not used
	}

	public enum ElemShowFactor
	{
		Undefined = 0,
		AlwaysShow = 1,
		AlwaysHide = 2,
		ShowAfterCountDown = 3,
		HideAfterCountDown = 4,
	}

	public UIButton operateButton;
	public SpriteText countDownText;
	public Vector3 particalPos = new Vector3(0f, 0f, 0f);
	public Vector3 particalScale = new Vector3(1f, 1f, 1f);

	private GameObject particalGo;
	private long countDownTime;
	private CountDownType cdType = CountDownType.Undefined;
	private ElemShowFactor displayFactor = ElemShowFactor.Undefined;

	private bool isHidden = false;
	public bool IsHidden
	{
		get { return isHidden; }
	}

	public virtual void Init() { }
	public virtual void Update() { SetParticle(); }
	public virtual bool ShowPartical() { return false; }

	protected virtual string ParticalName()
	{
		return GameDefines.assistantParticle;
	}

	private void SetParticle()
	{
		if (ShowPartical())
		{
			if (particalGo == null)
			{
				particalGo = ResourceManager.Instance.InstantiateAsset<GameObject>(KodGames.PathUtility.Combine(GameDefines.uiEffectPath, ParticalName()));
				ObjectUtility.AttachToParentAndKeepLocalTrans(operateButton.gameObject, particalGo);

				particalGo.transform.localPosition = particalPos;
				particalGo.transform.localScale = particalScale;
			}
			else
			{
				var animation = operateButton.GetComponentInChildren<Animation>();
				if (animation != null)
					animation.Play();
			}
		}
		else
		{
			if (particalGo != null)
				GameObject.Destroy(particalGo);
		}
	}

	protected bool HideNow(long nowTime)
	{
		switch (displayFactor)
		{
			case ElemShowFactor.AlwaysHide:
				return true;

			case ElemShowFactor.AlwaysShow:
				return false;

			case ElemShowFactor.ShowAfterCountDown:
				return (nowTime <= countDownTime);

			case ElemShowFactor.HideAfterCountDown:
				return (nowTime >= countDownTime);
		}

		return false;
	}

	protected void SetData(MonoBehaviour script, string methodName, int gotoUI)
	{
		SetData(script, methodName, gotoUI, 0, CountDownType.Undefined, ElemShowFactor.AlwaysShow);
	}

	protected void SetData(long endTime)
	{
		SetData(null, "", 0, endTime, CountDownType.Second, ElemShowFactor.AlwaysShow);
	}

	protected void SetData(long endTime, CountDownType cdType)
	{
		SetData(null, "", 0, endTime, cdType, ElemShowFactor.AlwaysShow);
	}

	protected void SetData(MonoBehaviour script, string methodName, int gotoUI, long endTime)
	{
		SetData(script, methodName, gotoUI, endTime, CountDownType.Second, ElemShowFactor.AlwaysShow);
	}

	protected void SetData(MonoBehaviour script, string methodName, int gotoUI, long endTime, CountDownType cdType, ElemShowFactor dpFactor)
	{
		operateButton.scriptWithMethodToInvoke = script;
		operateButton.methodToInvoke = methodName;
		operateButton.Data = gotoUI;

		this.countDownTime = endTime;
		this.cdType = cdType;
		this.displayFactor = dpFactor;

		RefreshDisplayStat(SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	protected void ResetDisplayFactor(ElemShowFactor dpFactor)
	{
		if (displayFactor == dpFactor)
			return;

		displayFactor = dpFactor;
		RefreshDisplayStat(SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	protected void ResetEndTime(long endTime)
	{
		if (this.countDownTime == endTime)
			return;

		this.countDownTime = endTime;
		RefreshDisplayStat(SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	protected void UpdateOnTime(long nowTime)
	{
		RefreshDisplayStat(nowTime);

		if (countDownText == null || isHidden)
			return;

		long leftTime = (countDownTime > nowTime ? countDownTime - nowTime : 0) / 1000;

		if (cdType == CountDownType.Minute)
			countDownText.Text = string.Format("{0:D2}:{1:D2}", leftTime / 3600, (leftTime % 3600) / 60);
		else
			countDownText.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", leftTime / 3600, (leftTime % 3600) / 60, leftTime % 60);
	}

	protected void RefreshDisplayStat(long nowTime)
	{
		bool hideNow = HideNow(nowTime);
		if (isHidden != hideNow)
			isHidden = hideNow;

		this.gameObject.SetActive(!isHidden);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnTempButtonClick(UIButton btn)
	{
		if (btn.Data == null)
			return;

		switch ((int)btn.Data)
		{
			case _UIType.UIPnlStartServerReward:
				if (ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.StartServerReward) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("FUNC_NOT_OPEN",
										ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.StartServerReward)));
				else
					GameUtility.JumpUIPanel((int)btn.Data);
				break;

			default:
				GameUtility.JumpUIPanel((int)btn.Data);
				break;
		}
	}
}
