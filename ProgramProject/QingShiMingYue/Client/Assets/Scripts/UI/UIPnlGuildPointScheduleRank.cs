using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlGuildPointScheduleRank : UIPnlPackageBase
{
	public UIScrollList skillList;
	public GameObjectPool skillObjectPool;
	public SpriteText emptyTip;
	public SpriteText selectedText;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		return true;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkillIcon(UIButton btn)
	{

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkillLevelUp(UIButton btn)
	{

	}
}
