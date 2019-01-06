using System;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public class UIDlgEastSeaExplain : UIModule
{	
	public SpriteText explainLabel;
	public UIBox explainBg;

	public float TipMaxHeight;
	public float TipMinHeight;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		//从配置文件中读取
		explainLabel.Text = userDatas[0].ToString();
		explainBg.SetSize(TipMaxHeight, explainLabel.GetLineHeight() * explainLabel.GetDisplayLineCount() + TipMinHeight);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		HideSelf();
	}
}