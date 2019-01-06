using UnityEngine;
using System.Collections;

public class UIElemListItemBgTemplate : MonoBehaviour 
{
	public UIButton darkBgBtn;
	public UIButton fadeBgBtn;
	
	public void SetListItemBg(AutoSpriteControlBase border, bool fade)
	{
		UIUtility.CopyIcon(border, fade ? fadeBgBtn : darkBgBtn);
	}
}
