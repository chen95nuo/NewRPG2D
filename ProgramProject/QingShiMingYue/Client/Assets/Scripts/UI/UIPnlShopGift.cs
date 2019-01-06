using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlShopGift : UIPnlShopPageBase
{
	private int displayPlayerLevel = 0;
	private int displayVipLevel = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		displayPlayerLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
		displayVipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;

		SysUIEnv.Instance.GetUIModule<UIPnlShop>().ChangeTabButtonState(_UIType.UIPnlShopGift);

		////if (GameUtility.CheckGoodsList())
		////    InitView();
		////else
		RequestMgr.Inst.Request(new QueryGoodsReq(InitView));

		return true;
	}

	public override void OnHide()
	{
		ClearData();

		base.OnHide();
	}

	public void InitView()
	{
		ClearData();
		StartCoroutine("FillData");
	}

	protected override void Update()
	{
		base.Update();

		if (displayPlayerLevel != SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level
			|| displayVipLevel != SysLocalDataBase.Inst.LocalPlayer.VipLevel)
		{
			displayPlayerLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
			displayVipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;

			ClearData();
			StartCoroutine("FillData");
		}
	}

	// Fill data for the scrollList
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		foreach (var goods in SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods)
		{
			if (goods.Status == _GoodsStatusType.Closed)
				continue;

			GoodConfig.Good goodConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goods.GoodsID);

			if (goodConfig != null
				&& goodConfig.goodsType == _Goods.NormalGoods
				&& goodConfig.GetGoodItemType() == ItemConfig._Type.Package
				&& goods.ShowVipLevel <= displayVipLevel
				&& goods.ShowPlayerLevel <= displayPlayerLevel)
			{
				// Add to list
				UIElemShopPropItem item = goodsItemPool.AllocateItem().GetComponent<UIElemShopPropItem>();

				item.SetData(goods);
				scrollList.AddItem(item.gameObject);
			}
		}
	}
}