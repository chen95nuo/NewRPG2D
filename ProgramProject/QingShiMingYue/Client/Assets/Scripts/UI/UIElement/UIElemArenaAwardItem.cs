using UnityEngine;
using System.Collections;
using ClientServerCommon;

public class UIElemArenaAwardItem : MonoBehaviour
{
	public UIElemAssetIcon awardIcon;
	public SpriteText awardName;
	public SpriteText awardCost;
	public SpriteText awardExtraDesc;
	public SpriteText awardPrice;
	public SpriteText awardRequimentText;
	public UIButton purchaseBtn;
	public UIButton goodsBtn;
	public SpriteText refreshTime;
	public SpriteText buyCount;

	private long cachedStatusTime = 0;
	private int cachedVipLevel = 0;
	private KodGames.ClientClass.Goods goods;
	public KodGames.ClientClass.Goods Goods { get { return goods; } }

	public void SetData(KodGames.ClientClass.Goods good)
	{
		goods = good;

		// Set data in button
		GoodConfig.Good goodCfg = ConfigDatabase.DefaultCfg.GoodConfig.GetGoodById(good.GoodsID);
		purchaseBtn.data = goodCfg;
		goodsBtn.data = good;

		awardIcon.SetData(goodCfg.id);

		if (goodCfg.rewards != null && goodCfg.rewards.Count > 0)
			awardName.Text = GameUtility.FormatUIString("UIElemArenaAwardItem_Goods_NameXCount_Str", ItemInfoUtility.GetAssetName(goodCfg.id), goodCfg.rewards[0].count);
		else
		{
			awardName.Text = ItemInfoUtility.GetAssetName(goodCfg.id);
			Debug.LogError("GoodsConfig dosen't contains any reward. goodsId=" + goodCfg.id);
		}
		awardExtraDesc.Text = ItemInfoUtility.GetAssetDesc(goodCfg.id);

		Cost cost = goodCfg.costs[0];
		if (cost.id == IDSeg._SpecialId.Badge)
		{
			if (ConsumeEnough(cost))
				awardPrice.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Badge"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, cost.count);
			else
				awardPrice.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Badge"), GameDefines.textColorBtnYellow, GameDefines.textColorRed, cost.count);
		}
		else
		{
			if (ConsumeEnough(cost))
				awardPrice.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Score"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, cost.count);
			else
				awardPrice.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Score"), GameDefines.textColorBtnYellow, GameDefines.textColorRed, cost.count);
		}

		if (goodCfg.costs.Count > 1)
		{
			if (cost.id == IDSeg._SpecialId.Badge)
			{
				if (ConsumeEnough(cost))
					awardCost.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Badge"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, cost.count);
				else
					awardCost.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Badge"), GameDefines.textColorBtnYellow, GameDefines.textColorRed, cost.count);
			}
			else
			{
				if (ConsumeEnough(cost))
					awardCost.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Score"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, cost.count);
				else
					awardCost.Text = string.Format(GameUtility.GetUIString("UIPnlArena_Label_Score"), GameDefines.textColorBtnYellow, GameDefines.textColorRed, cost.count);
			}
		}
		else
		{
			awardCost.Text = "";
		}

		SetRequimentText();
		UpdateCountData();
	}

	private bool ConsumeEnough(Cost cost)
	{
		switch (cost.id)
		{
			case IDSeg._SpecialId.Badge:
				return SysLocalDataBase.Inst.LocalPlayer.Badge >= cost.count;

			case IDSeg._SpecialId.ArenaHonorPoint:
				return SysLocalDataBase.Inst.LocalPlayer.ArenaData.HonorPoint >= cost.count;
		}
		return false;
	}

	private void UpdateCountData()
	{
			string text = "";
			// 更新数量限制
			if (goods.MaxBuyCount > 0)
			{
				if (goods.RemainBuyCount <= 0)
				{
					// 未到达限制,可购买
					text = GameUtility.FormatUIString(GetKeyByTimeDurationType(), GameDefines.textColorBtnYellow, GameDefines.textColorRed, goods.RemainBuyCount, GameDefines.textColorBtnYellow);
				}
				else
				{
					// 达到限制
					text = GameUtility.FormatUIString(GetKeyByTimeDurationType(), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, goods.RemainBuyCount, GameDefines.textColorBtnYellow);
				}
			}

			buyCount.Text = text;
	}

	public void UpdateData()
	{
		SetData(goods);
	}

	public void UpdateRefreshTime()
	{
		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		bool hasCloseCountdown = false;
		long statusTime = 0;
		if (goods.EndTime > 0 && goods.EndTime > nowTime)
		{
			// 有关闭时间设置
			hasCloseCountdown = true;
			statusTime = (goods.NextOpenTime - nowTime) / 1000 * 1000; // 求整到秒
		}

		if (cachedStatusTime != statusTime)
		{
			cachedStatusTime = statusTime;

			if (hasCloseCountdown)
			{
				if (statusTime > 0)
				{
					refreshTime.Text = GameUtility.Time2String(statusTime);
					purchaseBtn.Hide(true);

				}
				else
				{
					refreshTime.Text = "";
					purchaseBtn.Hide(false);
				}
			}
		}
	}

	private string GetKeyByTimeDurationType()
	{
		//判断商品的限购类型
		string keyValue = "";

		switch (goods.TimeDurationType)
		{
			case _TimeDurationType.Day:
				keyValue = "UIPnlArena_DayCountLimit_Label";
				break;
			case _TimeDurationType.Week:
				keyValue = "UIPnlArena_WeekCountLimit_Label";
				break;
			case _TimeDurationType.Month:
				keyValue = "UIPnlArena_MonthCountLimit_Label";
				break;
			case _TimeDurationType.Year:
				keyValue = "UIPnlArena_YearCountLimit_Label";
				break;
			default:
				keyValue = "UIPnlShop_CountLimit_Label";
				break;
		}
		return keyValue;
	}


	private void SetRequimentText()
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

		int vipLevel = SysLocalDataBase.Inst.LocalPlayer.VipLevel;
		if (cachedVipLevel != vipLevel)
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
		awardRequimentText.Text = text;

	}
}
