using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlDanDecomposeActivity : UIPnlDanDecompose
{
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnActivityInfoClick(UIButton btn)
	{		
		RequestMgr.Inst.Request(new QueryDanActivityReq(DanConfig._ActivityType.Decompose));
	}
}
