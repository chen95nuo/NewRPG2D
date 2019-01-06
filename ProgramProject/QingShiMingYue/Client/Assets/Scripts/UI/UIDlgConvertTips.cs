using UnityEngine;
using ClientServerCommon;
using System.Collections;
using System.Collections.Generic;
using KodGames.ExternalCall;

public class UIDlgConvertTips : UIModule
{
	public SpriteText tips;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (false == base.OnShow(layer, userDatas))
			return false;

		tips.Text = (string)(userDatas[0]);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}
}
