using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildConstructTab : UIModule
{
	public List<UIButton> tabButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = _UIType.UIPnlGuildConstruct;

		return true;
	}

	public void ChangeTabButtons(int uiType)
	{
		foreach (UIButton btn in tabButtons)
			btn.controlIsEnabled = ((int)btn.data) != uiType;
	}


	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTabButton(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackButton(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildTab);
	}
}
