using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemDanCultureXiantItem : MonoBehaviour
{
	public SpriteText levelLable;
	public SpriteText arriLable;
	public UIBox bg;

	public void SetData(int level,string context,bool isShow)
	{
		if (isShow)
			bg.SetToggleState(1);
		else
			bg.SetToggleState(0);
		levelLable.Text = GameUtility.FormatUIString("UIPnlDanCulture_LevelText", level);
		arriLable.Text = context;
	}

}
