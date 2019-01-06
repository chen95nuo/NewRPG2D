using UnityEngine;
using System.Collections;

public class UIElemDanIntroduce : MonoBehaviour
{
	public UIBox selectedLight;
	public UIElemAssetIcon assetIcon;

	private int _lnkUI = -1;
	public int LnkUI
	{
		get
		{
			return _lnkUI;
		}
		set
		{
			_lnkUI = value;
			assetIcon.Data = _lnkUI;
		}
	}

	public void SetData(int iconId)
	{
		assetIcon.SetData(iconId);
	}

	public void SetSelectedStat(bool selected)
	{
		selectedLight.Hide(!selected);
	}
}
