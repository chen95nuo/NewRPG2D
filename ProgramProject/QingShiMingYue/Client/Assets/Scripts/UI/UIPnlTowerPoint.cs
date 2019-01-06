using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerPoint : UIModule
{

	public List<UIButton> tabButtons;
	public UIButton closeButton;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].data = _UIType.UIPnlTowerThisWeekRank;
		tabButtons[1].data = _UIType.UIPnlTowerLastWeekRank;
		tabButtons[2].data = _UIType.UIPnlTowerWeekReward;
		
		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;		

		return true;
	}
	
	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnTabBtnClick(UIButton btn)
	{		
			SysUIEnv.Instance.ShowUIModule((int)btn.data);			
	}

	public void SetSelectedBtn(int lnkUI)
	{
		for (int index = 0; index < tabButtons.Count; index++ )
		{
			bool enableBtn = (lnkUI != (int)tabButtons[index].Data);

			if (tabButtons[index].controlIsEnabled != enableBtn)
				tabButtons[index].controlIsEnabled = enableBtn;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
		if (!SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerScene))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTowerScene);
	}
}
