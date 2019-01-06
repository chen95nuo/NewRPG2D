using System;
using System.Collections.Generic;
using UnityEngine;
using ClientServerCommon;

public class UIElemAvatarDinerRefreshItem : MonoBehaviour
{
	public UIElemAssetIcon qualityIcon;
	public GameObject costObj;
	public UIElemAssetIcon costIcon;
	public UIButton refreshBtn;

	public void SetData(DinerConfig.DinerBag dinerBag)
	{
		refreshBtn.Data = dinerBag;

		// Set Country Icon.
		qualityIcon.SetData(dinerBag.IconId);

		// Set Cost.
		if (dinerBag.Costs.Count <= 0)
			costObj.SetActive(false);
		else
		{
			costObj.SetActive(true);
			costIcon.SetData(dinerBag.Costs[0].id);
			costIcon.border.Text = dinerBag.Costs[0].count.ToString();
		}
	}
}