using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemGuildShopItem : MonoBehaviour
{
	public UIElemAssetIcon itemIcon;
	public SpriteText itemNameLabel;
	public SpriteText costNameLabel;
	public SpriteText itemCountLimitLabel;
	public SpriteText itemBuyLimitLabel;
	public UIButton buyButton;

	private int goodId;
	public int GoodId { get { return goodId; } }

	private long nextRefreshTime;
	public long NextRefreshTime { get { return nextRefreshTime; } }

	public void SetData(KodGames.ClientClass.GuildPublicGoods guildPublicGood)
	{
		// Set GoodId.
		this.goodId = guildPublicGood.GooodsId;

		var goodsCfg = ConfigDatabase.DefaultCfg.GuildPublicShopConfig.GetGoodsById(guildPublicGood.GooodsId);

		// Set AssetIcon.
		itemIcon.SetData(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId);

		// Set Name Label.
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId);

		// Set CostIcon.
		costNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildPublicShop_Cost", ItemInfoUtility.GetAssetName(goodsCfg.Costs[0].id), goodsCfg.Costs[0].count);

		// Set ItemCountLimit Label.
		int guildBuyCount = goodsCfg.GetLimitCountByGuildLevel(SysLocalDataBase.Inst.LocalPlayer.GuildGameData.GuildMiniInfo.GuildLevel);

		itemCountLimitLabel.Text = GameUtility.FormatUIString("UIPnlGuildPublicShop_ItemCountLimit", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, guildBuyCount - guildPublicGood.BuyCount);

		// Set Data.
		itemIcon.Data = guildPublicGood;
		buyButton.Data = guildPublicGood;
	}

	public void SetData(KodGames.ClientClass.GuildPrivateGoods guildPrivateGood)
	{
		// Set GoodId.
		this.goodId = guildPrivateGood.GooodsId;

		// Set NextRefreshTime.
		this.nextRefreshTime = guildPrivateGood.NextRefreshTime;

		var goodsCfg = ConfigDatabase.DefaultCfg.GuildPrivateShopConfig.GetGoodsById(guildPrivateGood.GooodsId);

		// Set AssetIcon.
		itemIcon.SetData(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId);

		// Set Name Label.
		itemNameLabel.Text = ItemInfoUtility.GetAssetName(goodsCfg.GoodsIconId != IDSeg.InvalidId ? goodsCfg.GoodsIconId : goodsCfg.GoodsId);

		// Set CostIcon.

		Cost cost = new Cost();

		int cout = goodsCfg.Costinfos.Count - 1;

		if (guildPrivateGood.BuyCount >= cout)
			cost = goodsCfg.Costinfos[cout].Costs[0];
		else
			cost = goodsCfg.Costinfos[guildPrivateGood.BuyCount].Costs[0];

		costNameLabel.Text = GameUtility.FormatUIString("UIPnlGuildPublicShop_Cost", ItemInfoUtility.GetAssetName(cost.id), cost.count);

		// Set ItemCountLimit Label.

		itemBuyLimitLabel.Text = guildPrivateGood.LimitDesc;

		itemCountLimitLabel.Text = GameUtility.FormatUIString(
			"UIPnlGuildPrivateShop_ItemCountLimit",
			GameDefines.textColorBtnYellow,
			GetLabelByTimeType(goodsCfg.ResetType),
			GameDefines.textColorWhite,
			goodsCfg.BuyLimitCountPerCircle - guildPrivateGood.BuyCount);

		// Set Data.
		itemIcon.Data = guildPrivateGood;
		buyButton.Data = guildPrivateGood;
	}

	private string GetLabelByTimeType(int timeType)
	{
		switch (timeType)
		{
			case _TimeDurationType.Day:
				return GameUtility.GetUIString("UIPnlGuildPublicShop_TimeDay");

			case _TimeDurationType.Week:
				return GameUtility.GetUIString("UIPnlGuildPublicShop_TimeWeek");

			case _TimeDurationType.Month:
				return GameUtility.GetUIString("UIPnlGuildPublicShop_TimeMonth");

			case _TimeDurationType.Year:
				return GameUtility.GetUIString("UIPnlGuildPublicShop_TimeYear");

			default:
				return "";
		}
	}
}
