using UnityEngine;
using System.Collections.Generic;

//一些按钮需要在Disable样式下可以点击，一般之后要弹tips提示“xx不能”字样
public class UIElemDisableStyleClickableBtnTemplate : MonoBehaviour
{
	public AutoSpriteControlBase btn;
	public AutoSpriteControlBase normalButton;

	public AutoSpriteControlBase bigButton1;
	public AutoSpriteControlBase bigButton1Normal;

	public void SetIcon(AutoSpriteControlBase baseIcon, bool disableStyle)
	{
		if (!disableStyle)
			UIUtility.CopyIcon(baseIcon, normalButton);
		else
			UIUtility.CopyIcon(baseIcon, btn);
	}

	public void SetBigButon1(AutoSpriteControlBase baseIcon, bool disableStyle)
	{
		if (disableStyle)
			UIUtility.CopyIcon(baseIcon, bigButton1);
		else
			UIUtility.CopyIcon(baseIcon, bigButton1Normal);
	}
}
