using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using UnityEngine;

public class UIPnlGuildShopActivity : UIModule
{
	public UIScrollList exchangeList;
	public SpriteText tipLabel;
	public GameObjectPool exchangePool;

	public static Dictionary<object, List<string>> itemGuids = new Dictionary<object, List<string>>();
	private List<KodGames.ClientClass.GuildExchangeGoods> exchanges;
	private bool waitControl;

	// Refresh Time.
	private float deltaTime;
	private long lastRefreshTime;
	private long nextRefreshTime;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlGuildShopTab>().ChangeTabButtons(_UIType.UIPnlGuildShopActivity);

		QueryExchangeList();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();

		ClearData();
	}

	private void ClearData()
	{
		StopCoroutine("FillExchangeList");
		exchangeList.ClearList(false);
		exchangeList.ScrollPosition = 0f;

		if (this.exchanges != null)
			this.exchanges.Clear();
		this.exchanges = null;
		this.waitControl = false;
		this.deltaTime = 0f;
		this.lastRefreshTime = 0;
		this.nextRefreshTime = 0;
	}

	private void QueryExchangeList()
	{
		this.waitControl = false;

		RequestMgr.Inst.Request(new QueryGuildExchangeShopReq());
	}

	private void Update()
	{
		if (this.IsShown == false || this.IsOverlayed || this.waitControl)
			return;

		deltaTime += Time.deltaTime;
		if (deltaTime > 1.0f)
		{
			deltaTime = 0f;
			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

			if (nextRefreshTime > 0 && nowTime > nextRefreshTime)
				QueryExchangeList();
			else
			{
				for (int index = 0; index < exchangeList.Count; index++)
				{
					var item = exchangeList.GetItem(index).Data as UIElemGuildShopActivityItem;
					item.UpdateTimeLabel(nowTime);
					if (item.Exchange.IsActive && item.Exchange.EndTime > 0 && item.Exchange.EndTime < nowTime)
					{
						QueryExchangeList();
						break;
					}
				}
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillExchangeList()
	{
		yield return null;

		// Add Tip Container.
		var activityInfo = ActivityManager.Instance.GetActivity<ActivityGuildShop>();
		if (activityInfo != null && activityInfo.ActivityInfo != null)
		{
			if (activityInfo.ActivityInfo.CloseTime > 0)
			{
				var startTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(activityInfo.ActivityInfo.OpenTime);
				var endTime = SysLocalDataBase.Inst.LoginInfo.ToServerDateTime(activityInfo.ActivityInfo.CloseTime);

				if (startTime.DayOfYear != endTime.DayOfYear)
					tipLabel.Text = GameUtility.FormatUIString(
											"UIPnlGuildShopActivity_ActivityTime1",
											GameDefines.textColorBtnYellow,
											GameDefines.textColorWhite,
											startTime.ToString(GameUtility.GetUIString("UIPnlGuildShopActivity_TimeFormat")),
											endTime.ToString(GameUtility.GetUIString("UIPnlGuildShopActivity_TimeFormat")));
				else
					tipLabel.Text = GameUtility.FormatUIString(
											"UIPnlGuildShopActivity_ActivityTime3",
											GameDefines.textColorBtnYellow,
											GameDefines.textColorWhite,
											startTime.ToString(GameUtility.GetUIString("UIPnlGuildShopActivity_TimeFormat")));

			}
			else
				tipLabel.Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ActivityTime2", GameDefines.textColorBtnYellow, GameDefines.textColorWhite);
		}
		else
			tipLabel.Text = string.Empty;

		// Fill Exchange List.
		foreach (var exchange in this.exchanges)
		{
			var container = exchangePool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = container.GetComponent<UIElemGuildShopActivityItem>();

			container.Data = item;
			item.SetData(exchange);

			exchangeList.AddItem(container);
		}
	}

	private void RefreshItemGuids()
	{
		itemGuids = new Dictionary<object, List<string>>();
		foreach (var exc in exchanges)
		{
			foreach (var cost in exc.Costs)
			{
				if (IDSeg.ToAssetType(cost.Id) != IDSeg._AssetType.Avatar
					&& IDSeg.ToAssetType(cost.Id) != IDSeg._AssetType.Equipment
					&& IDSeg.ToAssetType(cost.Id) != IDSeg._AssetType.CombatTurn
					&& IDSeg.ToAssetType(cost.Id) != IDSeg._AssetType.Dan)
					continue;

				if (!itemGuids.ContainsKey(cost))
					itemGuids.Add(cost, ItemInfoUtility.ExchangeGetItemAvaliableGuids(cost));
			}

			foreach (var costAsset in exc.CostAssets)
			{
				if (!itemGuids.ContainsKey(costAsset))
					itemGuids.Add(costAsset, ItemInfoUtility.ExchangeGetItemAvaliableGuids(costAsset));
			}

		}
	}

	public static List<string> AvaliableGuids(object key)
	{
		if ((key is KodGames.ClientClass.ItemEx || key is KodGames.ClientClass.CostAsset) && itemGuids.ContainsKey(key))
			return itemGuids[key];

		return null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickInfoButton(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlGuildGuideDetail, ConfigDatabase.DefaultCfg.GuildConfig.GetMainTypeByGuideType(GuildConfig._GuideType.ExchangeShopGuide));
	}

	public void OnQueryExchangeListSuccess(List<KodGames.ClientClass.GuildExchangeGoods> exchanges)
	{
		ClearData();

		this.waitControl = false;
		this.lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		this.nextRefreshTime = ActivityManager.Instance.GetActivity<ActivityGuildShop>().NextRefreshTime(lastRefreshTime);
		this.exchanges = exchanges;
		this.exchanges.Sort((e1, e2) =>
			{
				int e1Active = e1.IsActive ? 1 : -1;
				int e2Active = e2.IsActive ? 1 : -1;

				if (e1.IsActive != e2.IsActive)
					return e2Active - e1Active;
				else
					return e1.ShowIndex - e2.ShowIndex;
			});

		RefreshItemGuids();

		StartCoroutine("FillExchangeList");
	}

	public void OnQueryExchangeListFail()
	{
		this.waitControl = false;
		this.lastRefreshTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
		this.nextRefreshTime = ActivityManager.Instance.GetActivity<ActivityGuildShop>().NextRefreshTime(lastRefreshTime);
	}

	public void ResetWaitControlValue()
	{
		waitControl = false;
		nextRefreshTime = ActivityManager.Instance.GetActivity<ActivityGuildShop>().NextRefreshTime(lastRefreshTime);
	}

	public void OnExchangeSuccess(KodGames.ClientClass.GuildExchangeGoods exchange, KodGames.ClientClass.Reward reward)
	{
		if (reward != null)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(reward, true, true));

		RefreshItemGuids();

		for (int i = 0; i < exchangeList.Count; i++)
		{
			UIListItemContainer container = exchangeList.GetItem(i) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemGuildShopActivityItem exchangeItem = container.Data as UIElemGuildShopActivityItem;
			if (exchangeItem == null)
				continue;

			if (exchangeItem.Exchange.GoodsId == exchange.GoodsId)
				exchangeItem.SetData(exchange);
			else
				exchangeItem.ResetData();
		}
	}
}
