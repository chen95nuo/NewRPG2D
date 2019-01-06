using System;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using ClientCost = KodGames.ClientClass.ItemEx;

public class UIElemGuildShopActivityItem : MonoBehaviour
{
	public List<UIElemAssetIcon> costItems;
	public List<SpriteText> costCurrentCounts;
	public List<SpriteText> costTotalAmounts;
	public List<SpriteText> costNames;
	public List<SpriteText> costBreakLables;
	public UIBox exchangeStatus;
	public SpriteText exchangeConditionLabel;
	public SpriteText endUpTimeLabel;
	public SpriteText exchangeCountLabel;
	public UIElemAssetIcon rewardItem;
	public SpriteText rewardCountLabel;
	public UIButton exchangeBtn;

	private KodGames.ClientClass.GuildExchangeGoods exchange;
	public KodGames.ClientClass.GuildExchangeGoods Exchange { get { return exchange; } }

	private Dictionary<object, List<string>> selectedGuids = new Dictionary<object, List<string>>();
	private bool currentlyCanChange;

	public void SetData(KodGames.ClientClass.GuildExchangeGoods exchange)
	{
		this.exchange = exchange;

		// Set reward Icon.
		rewardItem.SetData(exchange.RewardView.Id);
		rewardItem.Data = exchange.RewardView;
		rewardCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ItemRewardCount", exchange.RewardView.Count);

		ResetData();
	}

	public void ResetData()
	{
		// Set exchange Status.
		UIUtility.CopyIcon(exchangeStatus, exchange.IsActive ? UIElemTemplate.Inst.iconBorderTemplate.iconGuildShopActive : UIElemTemplate.Inst.iconBorderTemplate.iconGuildShopInActive);

		// Set exchange count.
		exchangeCountLabel.Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ItemExchangeCount", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, exchange.BuyCountLimitPerActive - exchange.BuyCount, exchange.BuyCountLimitPerActive);

		// Set exchange condition.
		exchangeConditionLabel.Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ItemActiveCondition1", GameDefines.textColorBtnYellow, GameDefines.textColorWhite, exchange.ConditionDesc);

		ConstructDefaultSelect();

		SetCostIcons();

		UpdateTimeLabel(SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	public void UpdateTimeLabel(long nowTime)
	{
		if (exchange.IsActive && exchange.EndTime > 0)
			endUpTimeLabel.Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ItemEndTime", GameDefines.textColorWhite, GameDefines.textColorBtnYellow, GameUtility.Time2String(exchange.EndTime - nowTime));
		else
			endUpTimeLabel.Text = string.Empty;
	}

	private void SetCostIcons()
	{
		int notEnoughCount = 0;

		int iconIndex = 0;
		for (int i = 0; iconIndex < costItems.Count && i < exchange.CostAssets.Count; i++)
		{
			if (SetCostIconData(iconIndex, exchange.CostAssets[i]) == false)
				notEnoughCount++;

			iconIndex++;
		}

		for (int i = 0; iconIndex < costItems.Count && i < exchange.Costs.Count; i++)
		{
			if (SetCostIconData(iconIndex, exchange.Costs[i]) == false)
				notEnoughCount++;

			iconIndex++;
		}

		for (int i = iconIndex; i < costItems.Count; i++)
			costItems[i].gameObject.SetActive(false);

		currentlyCanChange = notEnoughCount == 0;
	}

	private bool SetCostIconData(int iconIndex, object cost)
	{
		if (cost == null)
			return true;

		if ((cost is KodGames.ClientClass.ItemEx) == false && (cost is KodGames.ClientClass.CostAsset) == false)
			return true;

		bool enough = true;

		costItems[iconIndex].gameObject.SetActive(true);

		// Set Cost Icon.
		if (cost is ClientCost)
			costItems[iconIndex].SetData((cost as ClientCost).Id, (cost as ClientCost).ExtensionBreakThroughLevelFrom, (cost as ClientCost).ExtensionLevelFrom, false);
		else
			costItems[iconIndex].SetData((cost as KodGames.ClientClass.CostAsset).IconId);

		costItems[iconIndex].Data = cost;

		// Set Cost Name.
		costNames[iconIndex].Text = ItemInfoUtility.ExchangeGetCostName(cost);

		// Set Break Level Label.
		costBreakLables[iconIndex].Text = ItemInfoUtility.ExchangeGetBreakLabelDesc(cost);

		if (SelectedCount(cost) < ItemInfoUtility.ExchangeGetCostCount(cost))
			enough = false;

		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
			costCurrentCounts[iconIndex].Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ItemCurrentCount", "{0}", SelectedCount(cost), ItemInfoUtility.ExchangeGetCostCount(cost));
		else
			costCurrentCounts[iconIndex].Text = "{0}" + ItemInfoUtility.ExchangeGetCostCount(cost).ToString();

		if (enough)
			costCurrentCounts[iconIndex].Text = string.Format(costCurrentCounts[iconIndex].Text, GameDefines.txColorGreen.ToString());
		else
			costCurrentCounts[iconIndex].Text = string.Format(costCurrentCounts[iconIndex].Text, GameDefines.txColorRed.ToString());

		costTotalAmounts[iconIndex].Text = GameUtility.FormatUIString("UIPnlGuildShopActivity_ItemTotalCount", ItemInfoUtility.GetItemCountStr(CalAvaliableCount(cost)));

		return enough;
	}

	private int SelectedCount(object costOption)
	{
		if (ItemInfoUtility.ExchangeIsOptionCost(costOption))
		{
			if (selectedGuids == null || !selectedGuids.ContainsKey(costOption) || selectedGuids[costOption].Count <= 0)
				return 0;
			else
				return selectedGuids[costOption].Count;
		}

		int avaliableCount = CalAvaliableCount(costOption);
		int costCount = ItemInfoUtility.ExchangeGetCostCount(costOption);

		return avaliableCount < costCount ? avaliableCount : costCount;
	}

	private int CalAvaliableCount(object costOption)
	{
		//获取可兑换的数量
		if (ItemInfoUtility.ExchangeIsOptionCost(costOption))
		{
			List<string> avaliableGuids = UIPnlGuildShopActivity.AvaliableGuids(costOption);
			if (avaliableGuids == null || avaliableGuids.Count <= 0)
				return 0;
			else
				return avaliableGuids.Count;
		}

		if (costOption is ClientCost)
			return ItemInfoUtility.GetGameItemCount((costOption as ClientCost).Id);

		return 0;
	}

	private void ConstructDefaultSelect()
	{
		selectedGuids = new Dictionary<object, List<string>>();

		foreach (var cost in exchange.Costs)
			ConstructDefaultSelect(cost);

		foreach (var costAsset in exchange.CostAssets)
			ConstructDefaultSelect(costAsset);
	}

	private void ConstructDefaultSelect(object costOption)
	{
		List<string> defaultSelect = new List<string>();
		if (selectedGuids.ContainsKey(costOption))
			selectedGuids[costOption] = defaultSelect;
		else
			selectedGuids.Add(costOption, defaultSelect);
	}

	private void ResetSelectGuids(object costOption, List<string> guids)
	{
		if (costOption == null)
			return;

		if (selectedGuids == null)
			selectedGuids = new Dictionary<object, List<string>>();

		if (selectedGuids.ContainsKey(costOption))
			selectedGuids[costOption].Clear();
		else
			selectedGuids.Add(costOption, new List<string>());

		foreach (var guid in guids)
		{
			if (!selectedGuids[costOption].Contains(guid))
				selectedGuids[costOption].Add(guid);
		}

		SetCostIcons();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCostIconClick(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;

		if (assetIcon.Data == null)
			return;

		if (assetIcon.Data is ClientCost)
			ShowCostView(assetIcon.Data as ClientCost);

		if (assetIcon.Data is KodGames.ClientClass.CostAsset)
			ShowCostView(assetIcon.Data as KodGames.ClientClass.CostAsset);
	}

	private void ShowCostView(ClientCost cost)
	{
		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChooseCard), cost, new UIPnlChooseCard.OnChooseCardSuccessDel(ResetSelectGuids));

		//显示cost 信息界面
		if (IDSeg.ToAssetType(cost.Id) == IDSeg._AssetType.Item)
			GameUtility.ShowAssetInfoUI(cost.Id);
	}

	private void ShowCostView(KodGames.ClientClass.CostAsset cost)
	{
		//AssetCost显示挑选卡牌界面
		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChooseCard), cost, new UIPnlChooseCard.OnChooseCardSuccessDel(ResetSelectGuids));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardIconClick(UIButton btn)
	{
		var showReward = (btn.Data as UIElemAssetIcon).Data as KodGames.ClientClass.RewardView;

		var assetReward = new ClientServerCommon.Reward();
		assetReward.id = showReward.Id;
		assetReward.count = showReward.Count;
		assetReward.breakthoughtLevel = showReward.BreakthoughtLevel;
		assetReward.level = showReward.Level;

		//显示奖励物品具体信息
		GameUtility.ShowAssetInfoUI(assetReward);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnExchangeClcik(UIButton btn)
	{
		//点击兑换 弹出条件不足tips或兑换确定Dlg
		if (currentlyCanChange)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgConfirmExchange), exchange.RewardView.Id, new UIDlgConfirmExchange.OnConfrimDel(DoExchange));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlGuildShopActivity_ItemCanNotExchange"));
	}

	private void DoExchange()
	{
		List<KodGames.ClientClass.Cost> costs = new List<KodGames.ClientClass.Cost>();

		foreach (var cost in exchange.Costs)
		{
			if (ItemInfoUtility.ExchangeIsOptionCost(cost) == false)
			{
				KodGames.ClientClass.Cost kd_cost = new KodGames.ClientClass.Cost(cost.Id, cost.Count, string.Empty);
				costs.Add(kd_cost);
			}
		}

		if (selectedGuids != null && selectedGuids.Count > 0)
		{
			foreach (var d in selectedGuids)
			{
				if (d.Value == null)
					continue;

				foreach (string g in d.Value)
				{
					KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost(ItemInfoUtility.GetResourceIdByGuid(g), 1, g);

					if (!costs.Contains(cost))
						costs.Add(cost);
				}
			}
		}

		RequestMgr.Inst.Request(new ExchangeGuildExchangeGoodsReq(exchange.GoodsId, costs));
	}
}