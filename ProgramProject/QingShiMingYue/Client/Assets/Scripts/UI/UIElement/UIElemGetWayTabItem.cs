using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemGetWayTabItem : MonoBehaviour
{
	public UIElemAssetIcon assembleIcon;
	public AutoSpriteControlBase selectedLight;

	public void SetData(int resourceId)
	{
		assembleIcon.SetData(resourceId);
	}

	public void SetSelectedStat(bool selected)
	{
		selectedLight.Hide(!selected);
	}
}
