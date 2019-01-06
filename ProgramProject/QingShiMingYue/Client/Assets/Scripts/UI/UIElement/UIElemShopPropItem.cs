using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using System;
using KodGames;

public class UIElemShopPropItem : MonoBehaviour
{
	public UIElemAssetIcon goodIcon;
	public UIElemAssetIcon goodNormalPriceIcon, goodSpecialPriceIcon;
	public AutoSpriteControlBase goodState;
	public UIButton goodBuy;
	public AutoSpriteControlBase goodPriceUnderLine;
	public SpriteText goodNameText;
	public SpriteText goodDescText;
	public SpriteText goodNormalPrice, goodNormalPriceText;
	public SpriteText goodSpecialPrice, goodSpecialPriceText;
	public SpriteText goodCountText;
	public SpriteText goodLevelCooldownText;
	public SpriteText goodDiscountTimeLabel;
	public SpriteText goodRequimentText;
	public SpriteText goodHaveCountText;
	public SpriteText goodStartTimeText;
	public SpriteText goodStartText;
	public UIElemAssistantBase assistantBase;

	private KodGames.ClientClass.Goods goods;
	public KodGames.ClientClass.Goods Goods { get { return goods; } }

	public UIListItemContainer uiContainer;

	private int cachedVipLevel = 0;
	private int cachedCount = 0;
	private long cachedStatusTime = 0;
	private long cachedCooldown = 0;
	private int cachedOwnCount = 0;

	private string limitStr = "";
	public string LimitStr { get { return limitStr; } }

	public void ClearData()
	{
		if (assistantBase != null)
			assistantBase.assistantData = IDSeg.InvalidId;
	}

	public void SetData(KodGames.ClientClass.Goods good)
	{
		// 助手数据
		if (assistantBase != null)
			assistantBase.assistantData = good.GoodsID;

		uiContainer.Data = this;
		this.goods = good;

		// Set icon
		int assetIcon = good.GoodsID;
		var goodsCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(good.GoodsID);
		if (goodsCfg != null && goodsCfg.assetIconId != IDSeg.InvalidId)
		{
			assetIcon = goodsCfg.assetIconId;
			goodIcon.SetData(goodsCfg.assetIconId);
		}
		else
			goodIcon.SetData(good.GoodsID);
		goodIcon.Data = good;
		// Set name 
		if (goodsCfg.rewards != null && goodsCfg.rewards.Count > 0)
			goodNameText.Text = GameUtility.FormatUIString("UIElemShopPropItem_Goods_NameXCount_Str", ItemInfoUtility.GetAssetName(goodsCfg.id), goodsCfg.rewards[0].count);
		else
		{
			goodNameText.Text = ItemInfoUtility.GetAssetName(goodsCfg.id);
			Debug.LogError("GoodsConfig doesn't contains any reward. goodsId=" + goodsCfg.id);
		}

		// Set desc
		if (IDSeg.ToAssetType(assetIcon) == IDSeg._AssetType.Item || IDSeg.ToAssetType(assetIcon) == IDSeg._AssetType.Special)
			goodDescText.Text = ItemInfoUtility.GetAssetDesc(assetIcon);
		else
			goodDescText.Text = ItemInfoUtility.GetAssetExtraDesc(assetIcon);

		// Set the good 's state icon.
		SetGoodStateIcon(good.Status);

		// Set the button 'buy' Data.
		goodBuy.Data = this;

		SetPrice();
		UpdateStatusTime(true);
		UpdateBuyAndCooldwonData(true);
		UpdateCountData(true);
		UpdateOwnCount(true);
		UpdateRequimentText(true);
	}

	public void UpdateData()
	{
		UpdateStatusTime(false);
		UpdateBuyAndCooldwonData(false);
		UpdateCountData(false);
		UpdateOwnCount(false);
	}

	public bool HasLimit()
	{
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		// 数量限制
		if (goods.MaxBuyCount > 0 && goods.RemainBuyCount <= 0)
			return true;

		// CD限制
		if (goods.NextOpenTime > 0 && nowTime < goods.NextOpenTime)
			return true;

		// VIP限制
		if (goods.VipLevel > 0 && SysLocalDataBase.Inst.LocalPlayer.VipLevel < goods.VipLevel)
			return true;

		// 状态时间限制
		if (goods.OpenTime > 0 && nowTime < goods.OpenTime)
			return true;
		if (goods.EndTime > 0 && nowTime > goods.EndTime)
			return true;

		// 等级限制
		if (goods.PlayerLevel > 0 && SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level < goods.PlayerLevel)
			return true;

		return false;
	}

	private void UpdateRequimentText(bool force)
	{
		// 更新购买道具条件 VIP等级 庄主等级
		string text = "";
		if (goods.PlayerLevel > 0)
		{
			//判断物品要求玩家等级
			if (goods.PlayerLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			{
				text = GameUtility.FormatUIString("UIPnlShop_MinPlayerLevel_Label", GameDefines.txColorYellow2, GameDefines.txColorRed, goods.PlayerLevel);
			}
			else
			{
				text = GameUtility.FormatUIString("UIPnlShop_MinPlayerLevel_Label", GameDefines.txColorYellow2, GameDefines.txColorWhite, goods.PlayerLevel);
			}
		}

		if (goods.MelaleucaLimits.Count > 0 || goods.WolfSmokeLimits.Count > 0)
			text = GameUtility.GetUIString("UIPnlTowerShop_Activity_Limit_Label");

		for (int i = 0; i < goods.WolfSmokeLimits.Count; i++)
		{
			if (i != 0)
				text = text + GameUtility.GetUIString("UIPnlTowerShop_Mark_Label");

			switch (goods.WolfSmokeLimits[i].Type)
			{
				//烽火狼烟判断通关数
				case _WolfSmokeLimitType.HistoryPassStage:
					if (goods.WolfSmokeLimits[i].IntValue > SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().HistoryPassStage)
						text += GameUtility.FormatUIString("UIPnlShop_WolfActivityShop_HistoryPass", GameDefines.txColorRed, goods.WolfSmokeLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_WolfActivityShop_HistoryPass", GameDefines.txColorWhite, goods.WolfSmokeLimits[i].IntValue);
					break;

				case _WolfSmokeLimitType.PassStage:
					if (goods.WolfSmokeLimits[i].IntValue > SysUIEnv.Instance.GetUIModule<UIPnlWolfInfo>().PassStageIndex - 1)
						text += GameUtility.FormatUIString("UIPnlShop_WolfActivityShop_PassStage", GameDefines.txColorRed, goods.WolfSmokeLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_WolfActivityShop_PassStage", GameDefines.txColorWhite, goods.WolfSmokeLimits[i].IntValue);
					break;

				default: break;
			}
		}

		for (int i = 0; i < goods.MelaleucaLimits.Count; i++)
		{
			if (i != 0)
				text = text + GameUtility.GetUIString("UIPnlTowerShop_Mark_Label");

			switch (goods.MelaleucaLimits[i].Type)
			{
				//千机楼判断当日积分
				case _MelaleucaLimitType.DayPoint:
					if (goods.MelaleucaLimits[i].IntValue > SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint)
						text += GameUtility.FormatUIString("UIPnlShop_DayPoint_Label", GameDefines.txColorRed, goods.MelaleucaLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_DayPoint_Label", GameDefines.txColorWhite, goods.MelaleucaLimits[i].IntValue);
					break;

				case _MelaleucaLimitType.DayLayer:
					if (goods.MelaleucaLimits[i].IntValue > SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentLayer)
						text += GameUtility.FormatUIString("UIPnlShop_DayLayer_Label", GameDefines.txColorRed, goods.MelaleucaLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_DayLayer_Label", GameDefines.txColorWhite, goods.MelaleucaLimits[i].IntValue);
					break;

				case _MelaleucaLimitType.WeekPoint:
					if (goods.MelaleucaLimits[i].IntValue > SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.ThisWeekPoint && goods.MelaleucaLimits[i].IntValue > SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.CurrentPoint)
						text += GameUtility.FormatUIString("UIPnlShop_WeekPoint_Label", GameDefines.txColorRed, goods.MelaleucaLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_WeekPoint_Label", GameDefines.txColorWhite, goods.MelaleucaLimits[i].IntValue);
					break;

				case _MelaleucaLimitType.LastWeekPoint:
					if (goods.MelaleucaLimits[i].IntValue > SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekPoint)
						text += GameUtility.FormatUIString("UIPnlShop_LastWeekPoint_Label", GameDefines.txColorRed, goods.MelaleucaLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_LastWeekPoint_Label", GameDefines.txColorWhite, goods.MelaleucaLimits[i].IntValue);
					break;

				case _MelaleucaLimitType.LastWeekrank:
					if (goods.MelaleucaLimits[i].IntValue < SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekRank || SysLocalDataBase.Inst.LocalPlayer.MelaleucaFloorData.LastWeekRank == 0)
						text += GameUtility.FormatUIString("UIPnlShop_LastWeekrank_Label", GameDefines.txColorRed, goods.MelaleucaLimits[i].IntValue);
					else
						text += GameUtility.FormatUIString("UIPnlShop_LastWeekrank_Label", GameDefines.txColorWhite, goods.MelaleucaLimits[i].IntValue);
					break;

				default: break;
			}
		}

		int vipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;
		if (force || cachedVipLevel != vipLevel)
		{
			//判断物品要求VIP等级
			if (goods.VipLevel > 0)
			{
				if (vipLevel < goods.VipLevel)
				{
					// 未到vip
					if (!text.Equals(""))
						text = GameUtility.AppendString(
							text,
							GameUtility.FormatUIString("UIPnlShop_MinVip_Label", GameDefines.txColorRed, goods.VipLevel),
							false);
					else
						text = GameUtility.FormatUIString("UIPnlShop_OnlyVip_Label", GameDefines.txColorYellow2, GameDefines.txColorRed, goods.VipLevel);
				}
				else
				{
					// 达到VIP
					if (!text.Equals(""))
						text = GameUtility.AppendString(
							text,
							GameUtility.FormatUIString("UIPnlShop_MinVip_Label", GameDefines.txColorWhite, goods.VipLevel),
							false);
					else
						text = GameUtility.FormatUIString("UIPnlShop_OnlyVip_Label", GameDefines.txColorYellow2, GameDefines.txColorWhite, goods.VipLevel);
				}
			}
		}

		UIUtility.UpdateUIText(goodRequimentText, text);
	}

	private void UpdateOwnCount(bool force)
	{
		// 更新持有数量
		List<Reward> reward = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goods.GoodsID).rewards;

		int itemId = 0;

		if (reward != null && reward.Count > 0)
			itemId = reward[0].id;

		int count = ItemInfoUtility.GetGameItemCount(itemId);

		if (force || cachedOwnCount != count)
		{
			cachedOwnCount = count;
			goodHaveCountText.Text = string.Format(GameUtility.GetUIString("ItemOwnNum"), GameDefines.txColorYellow2, GameDefines.txColorWhite, count);
		}
	}

	private void UpdateCountData(bool force)
	{
		// 更新数量限制
		int vipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;
		if (force || cachedCount != goods.RemainBuyCount || cachedVipLevel != vipLevel)
		{
			string text = "";
			// 更新数量限制
			cachedCount = goods.RemainBuyCount;
			if (goods.MaxBuyCount > 0)
			{
				if (goods.RemainBuyCount > 0)
				{
					// 未到达限制,可购买
					if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfShop)) || SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerShop)))
						text = GameUtility.FormatUIString(GetKeyByTimeDurationType(), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, goods.RemainBuyCount);
					else
						text = GameUtility.FormatUIString(GetKeyByTimeDurationType(), GameDefines.txColorYellow2, GameDefines.txColorWhite, goods.RemainBuyCount);
				}
				else
				{
					// 达到限制
					if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfShop)) || SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlTowerShop)))
						text = GameUtility.FormatUIString(GetKeyByTimeDurationType(), GameDefines.textColorBtnYellow, GameDefines.txColorRed, goods.RemainBuyCount);
					else
						text = GameUtility.FormatUIString(GetKeyByTimeDurationType(), GameDefines.txColorYellow2, GameDefines.txColorRed, goods.RemainBuyCount);
				}
			}

			UIUtility.UpdateUIText(goodCountText, text);
		}
	}

	private string GetKeyByTimeDurationType()
	{
		//判断商品的限购类型
		string keyValue = "";

		switch (goods.TimeDurationType)
		{
			case _TimeDurationType.Day:
				keyValue = "UIPnlShop_DayCountLimit_Label";
				break;
			case _TimeDurationType.Week:
				keyValue = "UIPnlShop_WeekCountLimit_Label";
				break;
			case _TimeDurationType.Month:
				keyValue = "UIPnlShop_MonthCountLimit_Label";
				break;
			case _TimeDurationType.Year:
				keyValue = "UIPnlShop_YearCountLimit_Label";
				break;
			default:
				keyValue = "UIPnlShop_CountLimit_Label";
				break;
		}
		return keyValue;
	}

	private void UpdateStatusTime(bool force)
	{
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		bool hasCloseCountdown = false;
		long statusTime = 0;
		if (goods.EndTime > 0)
		{
			// 有关闭时间设置
			hasCloseCountdown = true;
			statusTime = (goods.EndTime - nowTime) / 1000 * 1000; // 求整到秒
		}

		if (force || cachedStatusTime != statusTime)
		{
			cachedStatusTime = statusTime;

			string text = force ? "" : null;
			if (hasCloseCountdown)
			{
				text = "";
				if (statusTime > 0)
				{
					if (goods.Status == _GoodsStatusType.Discount)
					{
						if (statusTime < 86400000)
						{
							if (text != null && !text.Equals(GameUtility.FormatUIString("UIPnlShop_DisCountTime", GameDefines.textColorGreen, GameUtility.Time2String(statusTime))))
								text = GameUtility.FormatUIString("UIPnlShop_DisCountTime", GameDefines.textColorGreen, GameUtility.Time2String(statusTime));
						}
						else if (text != null)
						{

							int day = (int)statusTime / 86400000;
							text = GameUtility.FormatUIString("UIPnlShop_DisCountTimeDay", GameDefines.textColorGreen, day);
						}
					}
					else
					{
						if (text != null && !text.Equals(GameUtility.FormatUIString("UIPnlShop_BuyTime", GameDefines.textColorGreen, GameUtility.Time2String(statusTime))))
							text = GameUtility.FormatUIString("UIPnlShop_BuyTime", GameDefines.textColorGreen, GameUtility.Time2String(statusTime));
					}
				}
				else
				{
					// 设置已结束
					text = GameUtility.FormatUIString(
						"UIPnlShop_StatusTimeOver_Lable",
						GameDefines.textColorRed,
						_GoodsStatusType.GetDisplayNameByType(_GoodsStatusType.Closed, ConfigDatabase.DefaultCfg));
				}
			}

			UIUtility.UpdateUIText(goodDiscountTimeLabel, text);
		}
	}

	private void UpdateBuyAndCooldwonData(bool force)
	{
		// 检测倒计时, 需要在UpdateCountAndVipData之前执行
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		long cooldownTime = 0;
		if (goods.NextOpenTime > 0 && nowTime < goods.NextOpenTime)
			cooldownTime = (goods.NextOpenTime - nowTime) / 1000 * 1000; // 秒
		else
			goodLevelCooldownText.Text = "";

		string text = "";
		//更新距离开售时间
		if (goods.OpenTime > 0 && goods.OpenTime > SysLocalDataBase.Inst.LoginInfo.NowTime)
		{
			long leftTime = goods.OpenTime - SysLocalDataBase.Inst.LoginInfo.NowTime;
			if (!text.Equals(GameUtility.FormatUIString("UIPnlShop_CountLimit_Cooldown_Label", GameUtility.Time2String(leftTime))))
				text = GameUtility.FormatUIString("UIPnlShop_CountLimit_Cooldown_Label", GameUtility.Time2String(leftTime));
			goodStartText.Hide(false);
		}
		else
		{
			goodStartText.Hide(true);
		}

		UIUtility.UpdateUIText(goodStartTimeText, text);

		// 更新购买冷却时间
		cachedCooldown = cooldownTime;
		if (cooldownTime > 0)
			// 有倒计时,同时未达到
			goodLevelCooldownText.Text = GameUtility.FormatUIString("UIPnlShop_CountLimit_Cooldown_Label", GameUtility.Time2String(cachedCooldown));


		if (string.IsNullOrEmpty(text) == false || !goodLevelCooldownText.Text.Equals("") || (goods.EndTime > 0 && goods.EndTime <= SysLocalDataBase.Inst.LoginInfo.NowTime))
		{
			// 有限制, 隐藏按钮
			goodBuy.Hide(true);
		}
		else
		{
			// 无限制,显示按钮
			goodBuy.Hide(false);
		}
	}

	private void SetPrice()
	{
		// Normal price.
		var goodsCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(goods.GoodsID);
		int goodPrice = goodsCfg.costs[0].count;
		int costId = goodsCfg.costs[0].id;
		goodNormalPriceIcon.SetData(costId);
		goodNormalPriceText.Text = goodPrice.ToString();

		// Discount price
		bool hasDiscount = goods.Discount != 0;
		goodSpecialPrice.Hide(!hasDiscount);
		goodSpecialPriceIcon.Hide(!hasDiscount);
		goodSpecialPriceText.Hide(!hasDiscount);
		goodPriceUnderLine.Hide(!hasDiscount);

		// If has discount , show the original price and under line
		if (hasDiscount)
		{
			goodPriceUnderLine.width = goodNormalPriceText.GetWidth(goodNormalPriceText.Text);
			// Set the sell price.
			goodSpecialPriceIcon.SetData(costId);
			goodSpecialPriceText.Text = (goods.Discount != 0 ? goods.Discount : goodPrice).ToString();
		}
		else
		{
			goodDiscountTimeLabel.Text = "";
		}
	}

	private void SetGoodStateIcon(int goodStatus)
	{
		UIElemTemplate uiElemTemplate = SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIElemTemplate>();
		UIElemlShopIconTemplate shopIconTemplate = uiElemTemplate.shopIconTemplate;
		switch (goodStatus)
		{
			case _GoodsStatusType.Normal:
				goodState.Hide(true);
				break;

			case _GoodsStatusType.New:
				goodState.Hide(false);
				UIUtility.CopyIcon(goodState, shopIconTemplate.iconGoodNew);
				break;

			case _GoodsStatusType.Discount:
				goodState.Hide(false);
				UIUtility.CopyIcon(goodState, shopIconTemplate.iconGoodDiscourt);
				break;

			case _GoodsStatusType.Hot:
				goodState.Hide(false);
				UIUtility.CopyIcon(goodState, shopIconTemplate.iconGoodHot);
				break;

		}

		uiElemTemplate.gameObject.SetActive(false);
	}
}
