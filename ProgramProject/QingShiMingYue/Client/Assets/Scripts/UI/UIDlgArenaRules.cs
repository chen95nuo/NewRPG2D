using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIDlgArenaRules : UIModule  
{
	public SpriteText rulesText;
	public SpriteText rulesTitle;

	public override bool OnShow (_UILayer layer, params object[] userDatas)
	{
		if(base.OnShow(layer, userDatas) == false)
			return false;
		rulesTitle.Text = ConfigDatabase.DefaultCfg.ArenaConfig.strategy.title;
		rulesText.Text = ConfigDatabase.DefaultCfg.ArenaConfig.strategy.desc;
		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseClick(UIButton btn)
	{
		HideSelf();
	}
}
