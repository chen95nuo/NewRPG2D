using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlOrganTab : UIModule
{
	public List<UIButton> tabButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].data = _UIType.UIPnlOrgansBeastTab;
		tabButtons[1].data = _UIType.UIPnlOrganChipsTab;
		tabButtons[2].data = _UIType.UIPnlOrgansShopTab;
		
		return true;
	}
	
	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;

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

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClickBtn(UIButton btn)
	{
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().ShowMainScene();
	}
}

