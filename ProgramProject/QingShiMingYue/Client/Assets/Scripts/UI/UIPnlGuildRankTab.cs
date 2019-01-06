using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;


public class UIPnlGuildRankTab : UIModule
{
	public UIButton[] tabBtns;
	public UIBox fadeBtn;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabBtns[0].data = _UIType.UIPnlGuildRankList;
		tabBtns[1].data = _UIType.UIPnlGuildScheduleRankList;
		tabBtns[2].data = _UIType.UIPnlGuildRacingRankList;
		return true;
	}

	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabBtns)
		{
			btn.controlIsEnabled = ((int)btn.data) != uiType;
		}
		fadeBtn.Hide(uiType != _UIType.UIPnlGuildRankList);
	}
	
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
		HideSelf();
	}



}
