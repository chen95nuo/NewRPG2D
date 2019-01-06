using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointTab : UIModule
{
	public List<UIButton> tabButtons;
	public UIBox capacityBg;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].data = _UIType.UIPnlGuildPointBuildRank;
		tabButtons[1].data = _UIType.UIPnlGuildPointScheduleRank;
		tabButtons[2].data = _UIType.UIPnlGuildPointRacing;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;

		//SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlPackageItemTab);

		return false;
	}


	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != uiType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}
}

