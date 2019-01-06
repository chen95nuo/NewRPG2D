using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlTowerShop : UIModule
{
	public List<UIButton> tabButtons;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;
		
		tabButtons[0].Data = _UIType.UIPnlTowerNormalShop;		
		tabButtons[1].Data = _UIType.UIPnlTowerActivityShop;
		
		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;		

		RequestMgr.Inst.Request(new QueryGoodsReq(null));

		return true;
	}

	public void OpenShop(bool isOpen)
	{
		tabButtons[1].gameObject.SetActive(isOpen);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnTabBtnClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.Data);
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
