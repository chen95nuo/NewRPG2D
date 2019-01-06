using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildShopTab : UIModule
{
	public List<UIButton> tabButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = _UIType.UIPnlGuildPublicShop;
		tabButtons[1].Data = _UIType.UIPnlGuildPrivateShop;
		tabButtons[2].Data = _UIType.UIPnlGuildShopActivity;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		// 设置上次显示商店绿点的时间
		SysLocalDataBase.Inst.LocalPlayer.GuildGameData.PublicShopLastNotifyTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		return true;
	}

	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != uiType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		int uiType = (int)btn.Data;
		if (uiType == _UIType.UIPnlGuildShopActivity)
		{
			var activityData = ActivityManager.Instance.GetActivity<ActivityGuildShop>();
			if (activityData == null || activityData.ActivityInfo == null || (activityData.ActivityInfo.CloseTime > 0 && activityData.ActivityInfo.CloseTime < SysLocalDataBase.Inst.LoginInfo.NowTime) || (activityData.ActivityInfo.OpenTime > 0 && activityData.ActivityInfo.OpenTime > SysLocalDataBase.Inst.LoginInfo.NowTime))
			{
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildShopActivity_CloseInfo"));
				return;
			}
		}

		SysUIEnv.Instance.ShowUIModule(uiType);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackButton(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
	}
}
