using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlShopProp : UIPnlShopPageBase
{
	private int displayPlayerLevel = 0;
	private int displayVipLevel = 0;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		displayPlayerLevel = SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level;
		displayVipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;

		SysUIEnv.Instance.GetUIModule<UIPnlShop>().ChangeTabButtonState(_UIType.UIPnlShopProp);

		//修改主城下方显示
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlShopProp);

		//if (GameUtility.CheckGoodsList())
		//    InitView();
		//else
		RequestMgr.Inst.Request(new QueryGoodsReq(InitView));

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
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

		List<GoodConfig.Good> shopGoods = new List<GoodConfig.Good>();

		for (int index = 0; index < SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods.Count; index++)
		{
			var goods = SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods[index];

			if (goods.Status == _GoodsStatusType.Closed)
				continue;

			GoodConfig.Good goodConfig = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goods.GoodsID);

			if (goodConfig != null
				&& goodConfig.goodsType == _Goods.NormalGoods
				&& goodConfig.GetGoodItemType() != ItemConfig._Type.Package
				&& goods.ShowVipLevel <= displayVipLevel
				&& goods.ShowPlayerLevel <= displayPlayerLevel)
			{
				shopGoods.Add(goodConfig);
			}
		}

		shopGoods.Sort((t1, t2) =>
		{
			int d1 = t1.goodsIndex;
			int d2 = t2.goodsIndex;
			return d2 - d1;
		});

		for (int index = 0; index < shopGoods.Count; index++)
		{
			for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods.Count; i++)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods[i].GoodsID == shopGoods[index].id)
				{
					UIElemShopPropItem item = goodsItemPool.AllocateItem().GetComponent<UIElemShopPropItem>();

					item.SetData(SysLocalDataBase.Inst.LocalPlayer.ShopData.Goods[i]);
					scrollList.AddItem(item.uiContainer);
				}
			}
		}
	}
}
