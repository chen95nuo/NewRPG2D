using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointRankTab : UIModule
{
	public List<UIButton> tabButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].data = _UIType.UIPnlGuildPointBossRank;
		tabButtons[1].data = _UIType.UIPnlGuildPointExplorationRank;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return true;
		HideTabUI();
		int uitype = _UIType.UIPnlGuildPointBossRank;
		if (userDatas != null && userDatas.Length > 0)
			uitype = (int)userDatas[0];
			
		ChangeTabButtons(uitype);
		SysUIEnv.Instance.ShowUIModule(uitype);
		return false;
	}

	public override void OnHide()
	{
		HideTabUI();
		base.OnHide();
	}

	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != uiType;
	}

	private void HideTabUI()
	{
		foreach (UIButton btn in tabButtons)
			SysUIEnv.Instance.HideUIModule((int)btn.Data);
	}	

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		HideTabUI();
		ChangeTabButtons((int)btn.data);
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	
}

