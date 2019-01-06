using UnityEngine;
using System.Collections;

public class UIElemNotifAndBgLightItem : MonoBehaviour
{
	public UIBox notifItem;
	public UIButton btn;
	public UIBox bg;

	public void SetActive(bool isActive)
	{
		if (notifItem != null)
			notifItem.Hide(!isActive);
	}

	public void SetData(int id, bool isActive)
	{
		if (btn != null)
			btn.Data = id;
		if (bg != null)
			bg.Hide(!isActive);

	}
}
