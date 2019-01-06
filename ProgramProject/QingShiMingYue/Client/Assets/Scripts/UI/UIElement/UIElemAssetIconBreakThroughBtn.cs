using System;
using System.Collections.Generic;

public class UIElemAssetIconBreakThroughBtn : UIElemProgressItem
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
		return new AutoSpriteControlBase[] {UIElemTemplate.Inst.breakThroughTemplate.alpha, 
											 UIElemTemplate.Inst.breakThroughTemplate.star1_5, 
											 UIElemTemplate.Inst.breakThroughTemplate.star6_10};
	}
}
