using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarDinerRefresh : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool refreshItemPool;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		StartCoroutine("InitView", (int)userDatas[0]);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator InitView(int qualityType)
	{
		yield return null;

		var dinerBags = ConfigDatabase.DefaultCfg.DinerConfig.GetDinerBagsByQuality(qualityType);

		if (dinerBags == null)
			yield break;

		for (int i = 0; i < dinerBags.Count; i++)
		{
			if (dinerBags[i].RefreshType != DinerConfig._DinerRefreshType.Special)
				continue;

			UIElemAvatarDinerRefreshItem item = refreshItemPool.AllocateItem().GetComponent<UIElemAvatarDinerRefreshItem>();
			item.SetData(dinerBags[i]);

			scrollList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickForRefresh(UIButton btn)
	{
		DinerConfig.DinerBag dinerBag = btn.Data as DinerConfig.DinerBag;
		RequestMgr.Inst.Request(new RefreshDinerListReq(dinerBag.BagId));
	}

	public void OnRefreshListSuccess(int refreshType, int quallityType)
	{
		HideSelf();
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarDiner>().OnRefreshListSuccess(refreshType, quallityType);
	}
}