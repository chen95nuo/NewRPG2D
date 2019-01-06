using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlShop : UIModule
{
	// Tab buttons
	public List<UIButton> tabBtns;
	public UIButton rechargeButton;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabBtns[0].Data = _UIType.UIPnlShopProp;
		tabBtns[1].Data = _UIType.UIPnlShopGift;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;


		return true;
	}

	public void ChangeTabButtonState(int uiType)
	{
		foreach (UIButton btn in tabBtns)
			btn.controlIsEnabled = ((int)btn.Data) != uiType;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnShopTabClick(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule((int)btn.data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature="renaming")]
	private void OnClickRechargeButton(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlRecharge);
	}
}