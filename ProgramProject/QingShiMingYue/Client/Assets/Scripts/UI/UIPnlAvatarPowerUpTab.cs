using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarPowerUpTab : UIPnlItemInfoBase
{
	//Tab radio buttons.
	public UIButton levelUpTabBtn;
	public UIButton breakThTabBtn;
	public UIButton domineerTabBtn;
	public UIButton meridianTabBtn;
	public UIBox levelNotifyActivity;
	public UIBox breakNotifyActivity;

	private KodGames.ClientClass.Avatar avatarData;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		levelUpTabBtn.Data = _UIType.UIPnlAvatarLevelUp;
		breakThTabBtn.Data = _UIType.UIPnlAvatarBreakThrough;
		domineerTabBtn.Data = _UIType.UIPnlAvatarDomineerTab;
		meridianTabBtn.Data = _UIType.UIPnlAvatarMeridianTab;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().SetIsOverlaySetPower();

		return true;
	}

	public void SetNotifyState(int type)
	{
		levelNotifyActivity.Hide(type == _UIType.UIPnlAvatarLevelUp || !ItemInfoUtility.IsLevelNotifyActivity_Avatar(avatarData));
		breakNotifyActivity.Hide(type == _UIType.UIPnlAvatarBreakThrough || !ItemInfoUtility.IsBreakNotifyActivity_Avatar(avatarData));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabSelected(UIButton btn)
	{
		//检测当前操作的avatar等级是否满足

		if ((int)btn.Data == _UIType.UIPnlAvatarMeridianTab)
		{
			int minLevel = GetTheMinLevel();

			if (avatarData.LevelAttrib.Level < minLevel)
			{
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTipFlow, GameUtility.FormatUIString("UIPnlAvatarMeridian_AvatarLevelNotEnought", minLevel));
				return;
			}
		}

		SysUIEnv.Instance.ShowUIModule((int)btn.Data, avatarData);
	}

	public void UpdateTabStatus(int uiType, KodGames.ClientClass.Avatar avatar)
	{
		foreach (var button in new UIButton[] { levelUpTabBtn, breakThTabBtn, domineerTabBtn, meridianTabBtn })
			button.controlIsEnabled = (int)button.Data != uiType;

		avatarData = avatar;

		SetNotifyState(uiType);

		meridianTabBtn.Hide(SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Meridian));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabClose(UIButton btn)
	{
		// Hide sub panel
		foreach (int tabType in new int[] { _UIType.UIPnlAvatarLevelUp, _UIType.UIPnlAvatarBreakThrough, _UIType.UIPnlAvatarDomineerTab, _UIType.UIPnlAvatarMeridianTab })
			if (SysUIEnv.Instance.IsUIModuleShown(tabType))
				SysUIEnv.Instance.HideUIModule(tabType);

		// Relative UI
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlPackageAvatarTab))
			SysUIEnv.Instance.GetUIModule<UIPnlPackageAvatarTab>().RefreshView(avatarData);

		//if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatar))
		//    SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnDinerAvatarFiredByTimes();
	}

	private int GetTheMinLevel()
	{
		int minLevel = 0;

		MeridianConfig.MeridianConfigSetting meridianConfigSet = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianConfigSettingByQualityLevel(ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId).qualityLevel);

		if (meridianConfigSet != null)
		{
			minLevel = meridianConfigSet.GetMeridiansByType(MeridianConfig._MeridianType.Twelve)[0].level;

			foreach (var meridian in meridianConfigSet.GetMeridiansByType(MeridianConfig._MeridianType.Twelve))
			{
				if (minLevel > meridian.level)
					minLevel = meridian.level;
			}

			foreach (var meridian in meridianConfigSet.GetMeridiansByType(MeridianConfig._MeridianType.Eight))
			{
				if (minLevel > meridian.level)
					minLevel = meridian.level;
			}
		}

		return minLevel;
	}
}
