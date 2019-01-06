using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIDlgAdventureMessage : UIModule
{
	//Message to show.
	public SpriteText msgContentLabel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if(!base.OnShow(layer, userDatas)) return false;
		msgContentLabel.Text = userDatas[0].ToString();
		return true;
	}
	
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOkBtn(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule<UIPnlRecharge>();
		OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickQuitBtn(UIButton btn)
	{
		OnHide();
	}
}