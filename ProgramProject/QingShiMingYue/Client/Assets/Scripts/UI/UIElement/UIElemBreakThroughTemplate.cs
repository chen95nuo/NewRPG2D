using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIElemBreakThroughTemplate : MonoBehaviour 
{
	public AutoSpriteControlBase alpha;
	public AutoSpriteControlBase start0;
	public AutoSpriteControlBase star1_5;
	public AutoSpriteControlBase star6_10;

	public void SetBreakThrough(AutoSpriteControlBase control, int breakThrough)
	{
		if (breakThrough <= 5)
			UIUtility.CopyIcon(control, star1_5);
		else
			UIUtility.CopyIcon(control, star6_10);
	}
}
