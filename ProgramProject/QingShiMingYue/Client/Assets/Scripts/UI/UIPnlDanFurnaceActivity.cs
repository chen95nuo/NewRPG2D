using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlDanFurnaceActivity : UIPnlDanFurnace
{
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnActivityInfoClick(UIButton btn)
	{		
		RequestMgr.Inst.Request(new QueryDanActivityReq(DanConfig._ActivityType.Alchemy));
	}
}
