using System;
using System.Collections.Generic;
using ClientServerCommon;

public class UIElemDungeonShopIcon : MonoBehaviour
{
	public UIElemAssetIcon shopIcon1;

	public void SetData(CampaignConfig.TravelTrader travelTrader)
	{
		shopIcon1.Data = travelTrader;


		var dungeonCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(travelTrader.dungeonId);//获取配置关卡
		var dungeonRecord = SysLocalDataBase.Inst.LocalPlayer.CampaignData.SearchDungeon(dungeonCfg.ZoneId, dungeonCfg.dungeonId);//获取出通过关卡
		var travelCfg = ConfigDatabase.DefaultCfg.CampaignConfig.GetTravelTradeByDungeonId(travelTrader.dungeonId);//获取出本关奖励
		var travelRecrod = SysLocalDataBase.Inst.LocalPlayer.CampaignData.GetDungeonTravelDataByDungeonId(travelTrader.dungeonId);//获取已经购买的奖励

		//判断是否没有通关或者是通关星级没有达到开启云游商人的星级限制
		if (dungeonRecord == null || dungeonRecord.BestRecord < travelTrader.openNeedStars)
			shopIcon1.SetData(ConfigDatabase.DefaultCfg.CampaignConfig.travelTraderIconCloseId);
		else
		{
			//判断已经购买的是否是空的
			//是：说明还没有购买过
			//否：进行长度以及开启时间比较
			if (travelRecrod == null)
				shopIcon1.SetData(ConfigDatabase.DefaultCfg.CampaignConfig.travelTraderIconOpenId);
			else
			{
				if (travelRecrod.AlreadyBuyGoodIds.Count < travelCfg.canBuyGoodsIds.Count && travelRecrod.OpenTime > 0)
					shopIcon1.SetData(ConfigDatabase.DefaultCfg.CampaignConfig.travelTraderIconOpenId);
				else
					shopIcon1.SetData(ConfigDatabase.DefaultCfg.CampaignConfig.travelTraderIconBuyAllId);
			}
		}
	}
}