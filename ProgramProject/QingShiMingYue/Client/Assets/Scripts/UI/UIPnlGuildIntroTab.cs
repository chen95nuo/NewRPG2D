using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildIntroTab : UIModule
{
	public List<UIButton> tabButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = _UIType.UIPnlGuildIntroInfo;
		tabButtons[1].Data = _UIType.UIPnlGuildIntroMember;
		tabButtons[2].Data = _UIType.UIPnlGuildIntroReview;

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
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackButton(UIButton btn)
	{
		if (ItemInfoUtility.IsInMainScene())
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
		else
		{
			foreach (var tabBtn in tabButtons)
				if (SysUIEnv.Instance.GetUIModule((int)tabBtn.Data) != null && SysUIEnv.Instance.GetUIModule((int)tabBtn.Data).IsShown)
					SysUIEnv.Instance.HideUIModule((int)tabBtn.Data);
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildPointMain);
			HideSelf();
		}
	}
}