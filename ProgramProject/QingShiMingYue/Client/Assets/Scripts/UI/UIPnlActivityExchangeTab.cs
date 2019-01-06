using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

using ClientCost = KodGames.ClientClass.ItemEx;

public class UIPnlActivityExchangeTab : UIModule
{
	public UIScrollList exchangeList;
	public UIListItemContainer tipContainer;
	public GameObjectPool exchangePool;

	private System.DateTime nextUpdateTime;
	private List<Exchange> exchanges;
	protected bool queryNew;

	public static Dictionary<object, List<string>> itemGuids = new Dictionary<object, List<string>>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		//回调主城，修改颜色显示
		SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlActivityExchangeTab);

		queryNew = false;

		RequestMgr.Inst.Request(new QueryExchangeList());

		nextUpdateTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillList");

		exchangeList.ClearList(false);

		queryNew = false;

		if (exchanges != null)
			exchanges.Clear();
		base.OnHide();
	}

	private void Update()
	{
		if (!IsShown || queryNew)
			return;

		if (SysLocalDataBase.Inst.LoginInfo.NowDateTime < nextUpdateTime)
			return;

		long nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;

		for (int i = 0; i < exchangeList.Count; i++)
		{
			UIListItemContainer container = exchangeList.GetItem(i) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemExchangeItem exchange = container.Data as UIElemExchangeItem;
			if (exchange == null)
				continue;

			if (exchange.Exchange.NextRefreshTime > 0 && exchange.Exchange.NextRefreshTime <= SysLocalDataBase.Inst.LoginInfo.NowTime)
			{
				queryNew = true;
				RequestMgr.Inst.Request(new QueryExchangeList());
				return;
			}

			if (exchange.Exchange.EndTime > SysLocalDataBase.Inst.LoginInfo.NowTime || exchange.Exchange.EndTime <= 0)
				exchange.UpDateUI(nowTime);
			else
			{
				exchangeList.RemoveItem(i, false);
				i--;
			}
		}

		nextUpdateTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime.AddSeconds(1f);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		exchangeList.ClearList(false);

		exchangeList.AddItem(tipContainer);

		foreach (var ex in exchanges)
		{
			if (SysLocalDataBase.Inst.LoginInfo.NowTime < ex.OpenTime || (ex.EndTime > 0 && SysLocalDataBase.Inst.LoginInfo.NowTime > ex.EndTime))
				continue;

			UIListItemContainer container = exchangePool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemExchangeItem exchangeItem = container.gameObject.GetComponent<UIElemExchangeItem>();
			exchangeItem.SetData(ex);
			container.Data = exchangeItem;

			exchangeList.AddItem(container);
		}
	}

	public void OnQueryExchangeListSuccess(List<Exchange> exchange)
	{
		queryNew = false;
		this.exchanges = exchange;
		RefreshItemGuids();
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}

	public void OnExchangeRes(int exchangeId, long nextOpenTime, KodGames.ClientClass.Reward reward)
	{
		if (reward != null)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(reward, true, true));

		RefreshList(exchangeId, nextOpenTime);
	}

	private void RefreshList(int exchangeId, long nextOpenTime)
	{
		RefreshItemGuids();

		for (int i = 0; i < exchangeList.Count; i++)
		{
			UIListItemContainer container = exchangeList.GetItem(i) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemExchangeItem exchangeItem = container.Data as UIElemExchangeItem;
			if (exchangeItem == null)
				continue;

			if (exchangeItem.Exchange.ExchangeId == exchangeId)
				exchangeItem.OnExchangeSuccess(nextOpenTime);
			else
				exchangeItem.ResetData();
		}
	}

	private void RefreshItemGuids()
	{
		itemGuids = new Dictionary<object, List<string>>();

		foreach (var exc in exchanges)
		{
			if (SysLocalDataBase.Inst.LoginInfo.NowTime < exc.OpenTime || (exc.EndTime > 0 && SysLocalDataBase.Inst.LoginInfo.NowTime > exc.EndTime))
				continue;

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
		if ((key is ClientCost || key is KodGames.ClientClass.CostAsset) && itemGuids.ContainsKey(key))
			return itemGuids[key];

		return null;
	}
}
