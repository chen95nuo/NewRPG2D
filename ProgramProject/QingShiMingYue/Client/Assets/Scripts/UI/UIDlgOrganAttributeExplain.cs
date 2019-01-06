using System;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public class UIDlgOrganAttributeExplain : UIModule
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
		explainLabel.Text = ConfigDatabase.DefaultCfg.BeastConfig.AttributeDesc;
		explainBg.SetSize(TipMaxHeight, explainLabel.GetLineHeight() * explainLabel.GetDisplayLineCount() + TipMinHeight);

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackBtn(UIButton btn)
	{
		HideSelf();
	}
}