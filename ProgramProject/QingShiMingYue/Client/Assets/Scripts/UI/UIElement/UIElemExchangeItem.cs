using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

using ClientCost = KodGames.ClientClass.ItemEx;

public class UIElemExchangeItem : MonoBehaviour
{
	public List<UIElemAssetIcon> costItems;
	public List<SpriteText> costCurrentCounts;
	public List<SpriteText> costTotalAmounts;
	public List<SpriteText> costNames;
	public List<SpriteText> costBreakLables;
	public UIElemAssetIcon rewardItem;
	public SpriteText endUpTimeLabel;
	public SpriteText exchangeCountLabel;
	public SpriteText rewardCountLabel;
	public UIButton exchangeBtn;
	public SpriteText countDownLabel;
	public SpriteText renovateTimeLabel;

	private Exchange exchange;
	public Exchange Exchange
	{
		get { return exchange; }
	}

	private Dictionary<object, List<string>> selectedGuids = new Dictionary<object, List<string>>();
	private Dictionary<object, int> displayLevel;

	private static Color TXT_GRAY = GameDefines.txColorBrown;
	private static Color TXT_NORMAL = GameDefines.txColorWhite;
	private static Color TXT_ENOUGH = GameDefines.txColorGreen;
	private static Color TXT_NOT_ENOUGH = GameDefines.txColorRed;

	private bool currentlyCanChange = false;

	public bool LevelMeeted
	{
		//是否满足兑换等级条件
		get
		{
			return SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level >= exchange.PlayerLevel
				&& SysLocalDataBase.Inst.LocalPlayer.VipLevel >= exchange.VipLevel;
		}
	}

	//exchange是否有兑换等级要求
	public bool LevelRequirementExists { get { return exchange.PlayerLevel > 0 || exchange.VipLevel > 0; } }

	public void SetData(Exchange ex)
	{
		exchange = ex;

		rewardItem.border.Data = exchange.GainItem;
		rewardItem.SetData(exchange.GainItem.Id);
		rewardCountLabel.Text = string.Format("x{0}", exchange.GainItem.Count);

		ResetData();

		UpDateUI(SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	public void OnExchangeSuccess(long nextOpenTime)
	{
		//兑换完成后刷新物品信息
		exchange.NextOpenTime = nextOpenTime;
		exchange.AlreadyExchangeCount++;

		ResetData();

		UpDateUI(SysLocalDataBase.Inst.LoginInfo.NowTime);
	}

	public void ResetData()
	{
		//默认选中
		ConstructDefaultSelect();
		UpdateDisplayLevel();

		SetCostIconData();

		//物品兑换次数显示
		if (exchange.ExchangeCount < 0)
			exchangeCountLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_NoExchangeCount", TXT_GRAY.ToString());
		else
			exchangeCountLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_ExchangeCount", TXT_GRAY.ToString(), TXT_NORMAL.ToString(), exchange.AlreadyExchangeCount, exchange.ExchangeCount);

		if (LevelRequirementExists && !LevelMeeted)
			SetLevelRequirement();
	}

	public void ResetSelectGuid(object costOption, List<string> guids)
	{
		if (selectedGuids == null)
			selectedGuids = new Dictionary<object, List<string>>();


		if (selectedGuids.ContainsKey(costOption))
			selectedGuids[costOption] = new List<string>();
		else
			selectedGuids.Add(costOption, new List<string>());

		foreach (var guid in guids)
		{
			if (!selectedGuids[costOption].Contains(guid))
				selectedGuids[costOption].Add(guid);
		}

		UpdateDisplayLevel(costOption);

		SetCostIconData();
	}

	public void UpDateUI(long nowTime)
	{
		if (LevelRequirementExists && !LevelMeeted)
			SetLevelRequirement();
		else
			UpdateCountDown(nowTime);

		UpdateEndUpText(nowTime);
		UpdateRenovateTimeText(nowTime);
	}

	private void SetLevelRequirement()
	{
		//显示兑换等级要求和冷却时间
		string require = "";
		if (exchange.VipLevel > 0 && exchange.VipLevel > SysLocalDataBase.Inst.LocalPlayer.VipLevel)
			require = TXT_NOT_ENOUGH.ToString() + GameUtility.FormatUIString("UIPnlActivityExchangeTab_RequireVIPLevel", exchange.VipLevel);
		else if (exchange.PlayerLevel > 0 && exchange.PlayerLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
			require = TXT_NOT_ENOUGH.ToString() + GameUtility.FormatUIString("UIPnlActivityExchangeTab_RequirePlayerLevel", exchange.PlayerLevel);

		if (!require.Equals("") && !countDownLabel.Text.Equals(require))
			countDownLabel.Text = require;

		exchangeBtn.Hide(true);
		countDownLabel.Hide(false);
	}

	private void UpdateCountDown(long nowTimeTicks)
	{
		//判断是否还有剩余兑换次数
		if (exchange.ExchangeCount > 0 && exchange.AlreadyExchangeCount >= exchange.ExchangeCount)
		{
			exchangeBtn.Hide(true);
			countDownLabel.Hide(true);
			return;
		}

		//更新物品重置时间
		if (nowTimeTicks < exchange.NextOpenTime)
		{
			long leftSecond = exchange.NextOpenTime - nowTimeTicks;
			leftSecond = leftSecond / 1000;

			if (leftSecond > 0)
				countDownLabel.Text = TXT_NORMAL.ToString() + GameUtility.FormatUIString("UIPnlActivityExchangeTab_CountDown",
					string.Format("{0:D2}:{1:D2}:{2:D2}", leftSecond / 3600, (leftSecond % 3600) / 60, leftSecond % 60));
			else
				countDownLabel.Text = "";
		}

		exchangeBtn.Hide(nowTimeTicks < exchange.NextOpenTime);
		countDownLabel.Hide(nowTimeTicks >= exchange.NextOpenTime);
	}

	private void UpdateEndUpText(long nowTime)
	{
		//更新物品关闭兑换的时间
		if (exchange.EndTime <= 0)
		{
			endUpTimeLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_NoEndTime", TXT_GRAY.ToString());
			return;
		}

		long leftSecond = (exchange.EndTime - nowTime) / 1000;

		if (leftSecond > 86400)
			endUpTimeLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_EndUpTime_MoreThanADat", TXT_GRAY.ToString(), TXT_NORMAL.ToString(), leftSecond / 86400, (leftSecond % 86400) / 3600);
		else
			endUpTimeLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_EndUpTime_LessThanADay", TXT_GRAY.ToString(), TXT_NORMAL.ToString(), leftSecond / 3600, (leftSecond % 3600) / 60, leftSecond % 60);
	}

	private void UpdateRenovateTimeText(long nowTime)
	{
		//更新物品刷新时间
		if (exchange.NextRefreshTime < 0)
		{
			renovateTimeLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_NoRenovateTime", TXT_GRAY.ToString());
			return;
		}

		if (exchange.NextRefreshTime > SysLocalDataBase.Inst.LoginInfo.NowTime)
			renovateTimeLabel.Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_RenovateTime", TXT_GRAY.ToString(), TXT_NORMAL.ToString(), GameUtility.Time2String(exchange.NextRefreshTime - SysLocalDataBase.Inst.LoginInfo.NowTime));
		else
		{
			renovateTimeLabel.Text = "";
		}
	}

	public void DoExchange()
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

		RequestMgr.Inst.Request(new ExchangeReq(exchange.ExchangeId, costs, exchange.GroupId));
	}

	private void SetCostIconData()
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

		if (notEnoughCount == 0 && SysLocalDataBase.Inst.LocalPlayer.VipLevel >= exchange.VipLevel)
			currentlyCanChange = true;
		else
			currentlyCanChange = false;
	}

	private bool SetCostIconData(int iconIndex, object cost)
	{
		if ((cost is ClientCost) == false && (cost is KodGames.ClientClass.CostAsset) == false)
			return true;

		bool enough = true;

		costItems[iconIndex].border.Data = cost;

		if (cost is ClientCost)
			costItems[iconIndex].SetData((cost as ClientCost).Id, (cost as ClientCost).ExtensionBreakThroughLevelFrom, (cost as ClientCost).ExtensionLevelFrom, false);
		else
			costItems[iconIndex].SetData((cost as KodGames.ClientClass.CostAsset).IconId);

		costNames[iconIndex].Text = ItemInfoUtility.ExchangeGetCostName(cost);

		// Set Break Level Label.
		costBreakLables[iconIndex].Text = ItemInfoUtility.ExchangeGetBreakLabelDesc(cost);

		if (SelectedCount(cost) < ItemInfoUtility.ExchangeGetCostCount(cost))
			enough = false;

		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
			costCurrentCounts[iconIndex].Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_CurrentCount", "{0}", SelectedCount(cost), ItemInfoUtility.ExchangeGetCostCount(cost));
		else
			costCurrentCounts[iconIndex].Text = "{0}" + ItemInfoUtility.ExchangeGetCostCount(cost).ToString();

		if (enough)
			costCurrentCounts[iconIndex].Text = string.Format(costCurrentCounts[iconIndex].Text, TXT_ENOUGH.ToString());
		else
			costCurrentCounts[iconIndex].Text = string.Format(costCurrentCounts[iconIndex].Text, TXT_NOT_ENOUGH.ToString());

		costTotalAmounts[iconIndex].Text = GameUtility.FormatUIString("UIPnlActivityExchangeTab_TotalCount", ItemInfoUtility.GetItemCountStr(CalAvaliableCount(cost)));

		return enough;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCostIconClick(UIButton btn)
	{
		if (btn.Data == null)
			return;

		if (btn.Data is ClientCost)
			ShowCostView(btn.Data as ClientCost);

		if (btn.Data is KodGames.ClientClass.CostAsset)
			ShowCostView(btn.Data as KodGames.ClientClass.CostAsset);
	}

	private void ShowCostView(ClientCost cost)
	{
		//显示cost 挑选卡牌界面
		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChooseCard), cost, new UIPnlChooseCard.OnChooseCardSuccessDel(ResetSelectGuid));

		//显示cost 信息界面
		if (IDSeg.ToAssetType(cost.Id) == IDSeg._AssetType.Item)
			GameUtility.ShowAssetInfoUI(cost.Id);
	}

	private void ShowCostView(KodGames.ClientClass.CostAsset cost)
	{
		//AssetCost显示挑选卡牌界面
		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlChooseCard), cost, new UIPnlChooseCard.OnChooseCardSuccessDel(ResetSelectGuid));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnRewardIconClick(UIButton btn)
	{
		var itemEx = btn.Data as KodGames.ClientClass.ItemEx;

		var assetReward = new ClientServerCommon.Reward();
		assetReward.id = itemEx.Id;
		assetReward.count = itemEx.Count;
		assetReward.breakthoughtLevel = itemEx.ExtensionBreakThroughLevelFrom;
		assetReward.level = itemEx.ExtensionLevelFrom;

		//显示奖励物品具体信息
		GameUtility.ShowAssetInfoUI(assetReward);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnExchangeClcik(UIButton btn)
	{
		//点击兑换 弹出条件不足tips或兑换确定Dlg
		if (currentlyCanChange)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgConfirmExchange), exchange.GainItem.Id, new UIDlgConfirmExchange.OnConfrimDel(DoExchange));
		else if (SysLocalDataBase.Inst.LocalPlayer.VipLevel < exchange.VipLevel)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityExchangeTab_HigherVipLevelRequired"));
		else
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlActivityExchangeTab_CanNotExchange"));
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

	private int DisplayLevel(object costOption)
	{
		if (displayLevel != null && displayLevel.ContainsKey(costOption))
			return displayLevel[costOption];
		return 0;
	}

	private void UpdateDisplayLevel()
	{
		foreach (var cost in exchange.Costs)
			UpdateDisplayLevel(cost);

		foreach (var costAsset in exchange.CostAssets)
			UpdateDisplayLevel(costAsset);
	}

	private void ConstructDefaultSelect()
	{
		//selectedGuilds 赋值
		selectedGuids = new Dictionary<object, List<string>>();

		foreach (var cost in exchange.Costs)
			ConstructDefaultSelect(cost);

		foreach (var costAsset in exchange.CostAssets)
			ConstructDefaultSelect(costAsset);
	}

	private void UpdateDisplayLevel(object costOption)
	{
		if (ItemInfoUtility.ExchangeIsOptionCost(costOption) == false
			|| selectedGuids == null
			|| !selectedGuids.ContainsKey(costOption)
			|| selectedGuids[costOption].Count <= 0)
			return;

		if (displayLevel == null)
			displayLevel = new Dictionary<object, int>();

		int dLevel = 0;

		foreach (var a in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (selectedGuids[costOption].Contains(a.Guid) && dLevel < a.LevelAttrib.Level)
				dLevel = a.LevelAttrib.Level;
		}

		foreach (var e in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			if (selectedGuids[costOption].Contains(e.Guid) && dLevel < e.LevelAttrib.Level)
				dLevel = e.LevelAttrib.Level;
		}

		foreach (var s in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if (selectedGuids[costOption].Contains(s.Guid) && dLevel < s.LevelAttrib.Level)
				dLevel = s.LevelAttrib.Level;
		}

		if (displayLevel.ContainsKey(costOption))
			displayLevel[costOption] = dLevel;
		else
			displayLevel.Add(costOption, dLevel);
	}

	private void ConstructDefaultSelect(object costOption)
	{
		//默认个数
		//List<string> defaultSelect = CalDefaultSelect(costOption);
		List<string> defaultSelect = new List<string>();
		if (selectedGuids.ContainsKey(costOption))
			selectedGuids[costOption] = defaultSelect;
		else
			selectedGuids.Add(costOption, defaultSelect);
	}

	private int CalAvaliableCount(object costOption)
	{
		//获取可兑换的数量
		if (ItemInfoUtility.ExchangeIsOptionCost(costOption))
		{
			List<string> avaliableGuids = UIPnlActivityExchangeTab.AvaliableGuids(costOption);
			if (avaliableGuids == null || avaliableGuids.Count <= 0)
				return 0;
			else
				return avaliableGuids.Count;
		}

		if (costOption is ClientCost)
			return ItemInfoUtility.GetGameItemCount((costOption as ClientCost).Id);

		return 0;
	}

	private List<string> CalDefaultSelect(object costOption)
	{
		if (!ItemInfoUtility.ExchangeIsOptionCost(costOption))
			return null;

		int costCount = ItemInfoUtility.ExchangeGetCostCount(costOption);

		List<string> avaliableGuids = UIPnlActivityExchangeTab.AvaliableGuids(costOption);
		if (avaliableGuids == null)
			return null;

		List<string> defaultSelect = new List<string>();
		for (int i = 0; i < avaliableGuids.Count && i < costCount; i++)
			defaultSelect.Add(avaliableGuids[i]);

		return defaultSelect;
	}
}
