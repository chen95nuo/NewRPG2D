using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDlgAttributeComparison : UIModule
{
	public UIElemAttributeComparison currentAttribute;
	public UIElemAttributeComparison comparisonAttribute;

	private KodGames.ClientClass.Equipment currentEquip;
	private KodGames.ClientClass.Equipment comparisionEquip;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		currentEquip = userDatas[0] as KodGames.ClientClass.Equipment;
		comparisionEquip = userDatas[1] as KodGames.ClientClass.Equipment;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		currentEquip = null;
		comparisionEquip = null;
	}

	private void InitView()
	{
		currentAttribute.SetData(currentEquip);
		comparisonAttribute.SetData(comparisionEquip);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}