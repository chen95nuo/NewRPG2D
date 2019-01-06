using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

public class UIElemShopMysteryItem : MonoBehaviour
{
	public UIElemAssetIcon goodIcon;
	public SpriteText goodNameLabel;
	public UIElemAssetIcon goodSellPriceIcon;
	public SpriteText goodSellPriceLabel;

	public UIBox discountBox;
	public UIBox alreadyBoughtBox;
	public UIButton buyGoodButton;

	public SpriteText goodCountLabel;

	private int goodsId;
	public int GoodsId
	{
		get
		{
			return goodsId;
		}
	}
	private int goodsIndex;
	public int GoodsIndex
	{
		get
		{
			return goodsIndex;
		}
	}

	public void SetData(MysteryGoodInfo goodsInfo)
	{
		this.goodsId = goodsInfo.GoodId;
		this.goodsIndex = goodsInfo.GoodIndex;

		ClientServerCommon.Goods good = new ClientServerCommon.Goods();
		
		if(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShopMystery))
			good = ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.Normal).GetGoodsById(goodsId);
		
		if(SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlTowerNormalShop))
			good = ConfigDatabase.DefaultCfg.MysteryShopConfig.GetShopByType(MysteryShopConfig._ShopType.MelaleucaFloor).GetGoodsById(goodsId);
		
		goodIcon.SetData(good.reward.id);
		goodIcon.border.Data = good.reward.id;
		goodCountLabel.Text = GameUtility.FormatUIString("UIPnlShopMystery_GoodCount", GameDefines.textColorBtnYellow, GameDefines.colorWhite,good.reward.count);
		goodNameLabel.Text = ItemInfoUtility.GetAssetName(good.reward.id);
		goodSellPriceIcon.SetData(good.cost.id);

		if (good.discountCost == null || good.discountCost.count <= 0)
		{
			discountBox.Hide(true);
			goodSellPriceLabel.Text = good.cost.count.ToString();
		}
		else
		{
			discountBox.Hide(false);
			goodSellPriceLabel.Text = good.discountCost.count.ToString();
		}

		ResetGoodsBuyStates(goodsInfo.BuyOrNot);
		buyGoodButton.Data = new MysteryGoodInfo()
		{
			GoodId = this.goodsId,
			GoodIndex = this.goodsIndex
		};
	}

	public void SetData(KodGames.ClientClass.WolfSmokeGoodsInfo goodsInfo)
	{
		this.goodsId = goodsInfo.GoodsId;
		this.goodsIndex = goodsInfo.GoodsIndex;

		ClientServerCommon.Goods good = new ClientServerCommon.Goods();

		good = ConfigDatabase.DefaultCfg.WolfSmokeConfig.Shop.GetGoodsById(goodsId);

		goodIcon.SetData(good.reward.id);
		goodIcon.border.Data = good.reward.id;
		goodCountLabel.Text = GameUtility.FormatUIString("UIPnlShopMystery_GoodCount", GameDefines.textColorBtnYellow, GameDefines.colorWhite, good.reward.count);
		goodNameLabel.Text = ItemInfoUtility.GetAssetName(good.reward.id);
		goodSellPriceIcon.SetData(good.cost.id);

		if (good.discountCost == null || good.discountCost.count <= 0)
		{
			discountBox.Hide(true);
			goodSellPriceLabel.Text = good.cost.count.ToString();
		}
		else
		{
			discountBox.Hide(false);
			goodSellPriceLabel.Text = good.discountCost.count.ToString();
		}

		ResetGoodsBuyStates(goodsInfo.AlreadyBuy);
		buyGoodButton.Data = new WolfSmokeGoodsInfo()
		{
			GoodsId = this.goodsId,
			GoodsIndex = this.goodsIndex
		};
	}


	public void ResetGoodsBuyStates(bool isBought)
	{
		alreadyBoughtBox.Hide(!isBought);
		buyGoodButton.Hide(isBought);
	}
}
