using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIPnlShopMysteryRefresh : UIModule
{
	public UIScrollList scrollList;
	public GameObjectPool refreshItemPool;
	public UIButton TabBtn;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		StartCoroutine("FillList");
		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		StopCoroutine("FillList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0f;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		var mysteryShopConfig = ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.Normal);
		for (int i = 0; i < mysteryShopConfig.refreshSet.Count; i++)
		{
			if (mysteryShopConfig.refreshSet[i].cost == null || mysteryShopConfig.refreshSet[i].cost.count <= 0)
			{
				continue;
			}

			var item = refreshItemPool.AllocateItem().GetComponent<UIElemShopMysteryRefresh>();
			item.SetData(mysteryShopConfig.refreshSet[i]);
			scrollList.AddItem(item.gameObject);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRefreshBtnClick(UIButton btn)
	{
		//MysteryShopConfig.Refresh refresh = btn.Data as MysteryShopConfig.Refresh;
		//if (SysLocalDataBase.Inst.LocalPlayer.VipLevel >= refresh.vipLevel)
		//{
		//    if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlShopMystery)))
		//        SysUIEnv.Instance.GetUIModule<UIPnlShopMystery>().SetSelectReset(refresh.refreshId);

		//    this.HideSelf();
		//}
		//else
		//    SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlShopMysteryRefresh_VIPNot"));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackBtnClick(UIButton btn)
	{
		HideSelf();
	}
}
