using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class UIElemDanActivityinfoIcon : MonoBehaviour
{
	public UIElemAssetIcon activityIcon;
	public UIBox selectBg;

	private int iconId;
	public int IconId
	{
		get { return iconId; }
	}

	public void SetData(int iconId)
	{
		this.iconId = iconId;
		activityIcon.SetData(iconId);
		activityIcon.Data = iconId;
	}

	public void SelectLight(bool isSelect)
	{
		selectBg.Hide(!isSelect);
	}
}