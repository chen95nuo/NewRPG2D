using UnityEngine;
using System.Collections;

public class UIElemBreakThroughBtn : UIElemProgressItem
{
	private static int C_BREAK_CLIPCOUNT = 5;

	public override void Awake()
	{
		base.Awake();

		SetMax(C_BREAK_CLIPCOUNT);
	}

	public void SetBreakThroughIcon(int breakLevel)
	{
		Value = breakLevel;
	}

	public override AutoSpriteControlBase[] GetIconBorders()
	{
		return new AutoSpriteControlBase[] {UIElemTemplate.Inst.breakThroughTemplate.start0, 
											 UIElemTemplate.Inst.breakThroughTemplate.star1_5, 
											 UIElemTemplate.Inst.breakThroughTemplate.star6_10};
	}
}
