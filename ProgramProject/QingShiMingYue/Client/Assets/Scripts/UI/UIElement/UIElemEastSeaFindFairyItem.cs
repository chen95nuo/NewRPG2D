//#define MONTHCARD_LOG
using UnityEngine;
using ClientServerCommon;
using KodGames.ClientClass;
using System.Collections;
using System.Collections.Generic;

using ClientCost = KodGames.ClientClass.ItemEx;

public class UIElemEastSeaFindFairyItem : MonoBehaviour
{
	public UIElemAssetIcon getIcon;
	public SpriteText getSelectNumLable;
	public SpriteText eastSeaCountLable;
	public UIElemAssetIcon overflowIcon;
	public UIButton exchangeBtn;

	public List<UIElemAssetIcon> requiredIconList;
	public List<SpriteText> requiredSelectNumLableList;
	public List<SpriteText> nameTextList;

	[HideInInspector]
	public List<KodGames.ClientClass.Cost> costs = new List<KodGames.ClientClass.Cost>();
	[HideInInspector]
	public KodGames.ClientClass.ZentiaExchange zentiaExchange;

	private List<object> zentiaCostDataList = new List<object>();
	private Dictionary<object, List<string>> selectedGuids = new Dictionary<object, List<string>>();

	public void ClearData(bool isAll)
	{
		if (isAll)
			selectedGuids.Clear();

		costs.Clear();
		zentiaCostDataList.Clear();
	}

	public void SetData(KodGames.ClientClass.ZentiaExchange zentiaExchange)
	{
		this.zentiaExchange = zentiaExchange;
		this.zentiaCostDataList.Clear();
		this.exchangeBtn.Data = this;

		// Set Exchange State.
		if (zentiaExchange.IsAlreadyExchanged)
			exchangeBtn.SetControlState(UIButton.CONTROL_STATE.DISABLED);
		else
			exchangeBtn.SetControlState(UIButton.CONTROL_STATE.NORMAL);

		List<KodGames.Pair<int, int>> pairs = SysLocalDataBase.ConvertIdCountList(zentiaExchange.ZentiaReward);
		eastSeaCountLable.Text = "+" + (pairs != null && pairs.Count > 0 ? pairs[0].second : 0);

		if (zentiaExchange.IconId != 0)
		{
			overflowIcon.gameObject.SetActive(true);
			overflowIcon.SetData(zentiaExchange.IconId);
		}
		else
		{
			overflowIcon.gameObject.SetActive(false);
		}

		foreach (var cost in zentiaExchange.CostAssets)
			zentiaCostDataList.Add(cost);
		foreach (var cost in zentiaExchange.Costs)
			zentiaCostDataList.Add(cost);

		for (int i = 0; i < requiredIconList.Count; i++)
		{
			if (i < zentiaCostDataList.Count)
			{
				requiredIconList[i].gameObject.SetActive(true);
				SetIconData(zentiaCostDataList[i], requiredIconList[i], requiredSelectNumLableList[i], nameTextList[i]);
			}
			else
				requiredIconList[i].gameObject.SetActive(false);
		}

		// Set Reward Icon.
		List<KodGames.Pair<int, int>> normalTipsRewardPars = SysLocalDataBase.ConvertIdCountList(zentiaExchange.Reward);

		int breakthoughtLevel = 0;
		switch (IDSeg.ToAssetType(normalTipsRewardPars[0].first))
		{
			case IDSeg._AssetType.Avatar:
				breakthoughtLevel = zentiaExchange.Reward.Avatar[0].BreakthoughtLevel;
				break;
			case IDSeg._AssetType.Equipment:
				breakthoughtLevel = zentiaExchange.Reward.Equip[0].BreakthoughtLevel;
				break;
			case IDSeg._AssetType.CombatTurn:
				breakthoughtLevel = 0;
				break;
		}

		getIcon.SetData(normalTipsRewardPars[0].first);
		string inBracketStr = breakthoughtLevel > 0 ? GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_GainIconContext", GameDefines.textColorTipsInBlack, breakthoughtLevel, normalTipsRewardPars.Count) : string.Format("x{0}", normalTipsRewardPars[0].second);

		getIcon.border.Data = zentiaExchange.Reward;
		getSelectNumLable.Text = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_Bracket", GameDefines.textColorTipsInBlack,
					inBracketStr, GameDefines.textColorTipsInBlack);
	}

	private void SetIconData(object data, UIElemAssetIcon icon, SpriteText lable, SpriteText name)
	{
		string inBracketStr = string.Empty;
		Color textColor = GameDefines.textColorTipsInBlack;
		int minBreakLevel = 0;
		int maxBreakLevel = 0;

		// Set Cost Icon.
		if (data is KodGames.ClientClass.CostAsset)
		{
			minBreakLevel = (data as KodGames.ClientClass.CostAsset).BreakThroughLevelFrom;
			maxBreakLevel = (data as KodGames.ClientClass.CostAsset).BreakThroughLevelTo;

			icon.SetData((data as KodGames.ClientClass.CostAsset).IconId);
		}
		else
		{
			minBreakLevel = (data as ClientCost).ExtensionBreakThroughLevelFrom;
			maxBreakLevel = (data as ClientCost).ExtensionBreakThroughLevelTo;

			icon.SetData((data as ClientCost).Id, (data as ClientCost).ExtensionBreakThroughLevelFrom);
		}

		icon.border.Data = data;

		// Set Cost Name.
		if (name != null && string.IsNullOrEmpty(name.Text))
			name.Text = ItemInfoUtility.ExchangeGetCostName(data);

		if (IsMeetCondition(data) || zentiaExchange.IsAlreadyExchanged)
			textColor = GameDefines.textColorTipsInBlack;
		else
			textColor = GameDefines.textColorRed;

		switch (ItemInfoUtility.ExchangeGetCostType(data))
		{
			case IDSeg._AssetType.Dan:

				if (minBreakLevel < 1 || maxBreakLevel < 1 || minBreakLevel > maxBreakLevel)
					inBracketStr = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_GainIconContext4", textColor, ItemInfoUtility.ExchangeGetCostCount(data), GameDefines.textColorTipsInBlack);
				else if (minBreakLevel == maxBreakLevel)
					inBracketStr = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_GainIconContext2", textColor, minBreakLevel, ItemInfoUtility.ExchangeGetCostCount(data));
				else
					inBracketStr = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_GainIconContext3", textColor, minBreakLevel, maxBreakLevel, ItemInfoUtility.ExchangeGetCostCount(data));

				break;

			default:

				if (minBreakLevel < 0 || maxBreakLevel < 0 || minBreakLevel > maxBreakLevel)
					inBracketStr = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_GainIconContext4", textColor, ItemInfoUtility.ExchangeGetCostCount(data), GameDefines.textColorTipsInBlack);
				else if (minBreakLevel == maxBreakLevel)
					inBracketStr = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_GainIconContext", textColor, minBreakLevel, ItemInfoUtility.ExchangeGetCostCount(data));
				else
					inBracketStr = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_ItemIconContext", textColor, minBreakLevel, maxBreakLevel, ItemInfoUtility.ExchangeGetCostCount(data));

				break;
		}

		lable.Text = GameUtility.FormatUIString("UIPnlEastSeaFindFairyMain_Bracket", GameDefines.textColorTipsInBlack,
					inBracketStr, GameDefines.textColorTipsInBlack);
	}

	private List<ClientServerCommon.Reward> ToAssetTypeRewardComm(object obj)
	{
		List<ClientServerCommon.Reward> rewardCommList = new List<ClientServerCommon.Reward>();

		if (obj is KodGames.ClientClass.Reward)
			rewardCommList = SysLocalDataBase.CCRewardToCSCReward(obj as KodGames.ClientClass.Reward);

		ClientServerCommon.Reward rewardComm = null;
		if (obj is ClientCost)
		{
			var item = obj as ClientCost;
			rewardComm = new ClientServerCommon.Reward();
			rewardComm.id = item.Id;
			rewardComm.count = item.Count;
			rewardComm.breakthoughtLevel = item.ExtensionBreakThroughLevelFrom;
			rewardComm.level = item.ExtensionLevelFrom;
		}
		else if (obj is KodGames.ClientClass.CostAsset)
		{
			var item = obj as KodGames.ClientClass.CostAsset;
			rewardComm = new ClientServerCommon.Reward();
			rewardComm.id = item.IconId;
			rewardComm.count = item.Count;
			rewardComm.breakthoughtLevel = item.BreakThroughLevelFrom;
			rewardComm.level = item.LevelFrom;
		}

		if (rewardComm != null)
			rewardCommList.Add(rewardComm);

		return rewardCommList;
	}

	private bool IsMeetCondition(object item)
	{
		if (ItemInfoUtility.ExchangeIsOptionCost(item))
			return selectedGuids.ContainsKey(item) && selectedGuids[item] != null && selectedGuids.Count == ItemInfoUtility.ExchangeGetCostCount(item);
		else
			return ItemInfoUtility.GetGameItemCount(GetCostId(item)) >= ItemInfoUtility.ExchangeGetCostCount(item);
	}

	public int GetIconCount()
	{
		int costAssetCount = zentiaExchange.CostAssets != null ? zentiaExchange.CostAssets.Count : 0;
		int costsCount = zentiaExchange.Costs != null ? zentiaExchange.Costs.Count : 0;
		return costAssetCount + costsCount;
	}

	private void AddCosts(object item)
	{
		if (!ItemInfoUtility.ExchangeIsOptionCost(item))
		{
			var cost = new KodGames.ClientClass.Cost();
			cost.Count = ItemInfoUtility.ExchangeGetCostCount(item);
			cost.Id = GetCostId(item);
			costs.Add(cost);
		}
	}

	private void ShowCostView(object cost)
	{
		//AssetCost显示挑选卡牌界面
		if (ItemInfoUtility.ExchangeIsOptionCost(cost))
		{
			selectedGuids.Clear();
			SysUIEnv.Instance.ShowUIModule<UIPnlChooseCard>(cost, new UIPnlChooseCard.OnChooseCardSuccessDel(OnResetSelectGuid), new UIPnlChooseCard.OnChooseCardFailDel(OnChooseNoCardDel));
		}
		else
			//显示奖励物品具体信息
			GameUtility.ShowAssetInfoUI(ToAssetTypeRewardComm(cost)[0]);
	}

	public int GetCostId(object cost)
	{
		if (cost is KodGames.ClientClass.CostAsset)
			return (cost as KodGames.ClientClass.CostAsset).IconId;
		if (cost is ClientCost)
			return (cost as ClientCost).Id;
		return 0;
	}

	public int GetCostType(object costOption)
	{
		if (costOption is KodGames.ClientClass.CostAsset)
			return (costOption as KodGames.ClientClass.CostAsset).Type;
		if (costOption is ClientCost)
			return IDSeg.ToAssetType((costOption as ClientCost).Id);

		return IDSeg.InvalidId;
	}

	public void OnClickFindEastSeaBtn()
	{
		if (costs == null)
			costs = new List<KodGames.ClientClass.Cost>();
		costs.Clear();
		for (int i = 0; i < zentiaCostDataList.Count; i++)
		{
			if (!selectedGuids.ContainsKey(zentiaCostDataList[i]) && !ItemInfoUtility.ExchangeIsOptionCost(zentiaCostDataList[i]))
			{
				List<string> strids = new List<string>();
				strids.Add(GetCostId(zentiaCostDataList[i]).ToString());
				selectedGuids[zentiaCostDataList[i]] = strids;
			}
		}
		//SetData(zentiaExchange);
		foreach (object obj in selectedGuids.Keys)
		{
			foreach (string guid in selectedGuids[obj])
			{
				KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost();
				cost.Guid = guid;
				KodGames.ClientClass.Avatar avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(guid);
				if (avatar != null)
					cost.Id = avatar.ResourceId;
				KodGames.ClientClass.Equipment equit = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(guid);
				if (equit != null)
					cost.Id = equit.ResourceId;
				KodGames.ClientClass.Skill skill = SysLocalDataBase.Inst.LocalPlayer.SearchSkill(guid);
				if (skill != null)
					cost.Id = skill.ResourceId;
				KodGames.ClientClass.Dan dan = SysLocalDataBase.Inst.LocalPlayer.SearchDan(guid);
				if (dan != null)
					cost.Id = dan.ResourceId;

				if (skill == null && equit == null && avatar == null && dan == null)
					AddCosts(obj);
				else
					costs.Add(cost);
			}
		}
	}

	//选择完角色，装备或者书籍时回调函数
	public void OnResetSelectGuid(object costOption, List<string> guids)
	{
		if (selectedGuids == null)
			selectedGuids = new Dictionary<object, List<string>>();

		if (selectedGuids.ContainsKey(costOption))
			selectedGuids[costOption] = new List<string>();
		else
			selectedGuids.Add(costOption, new List<string>());

		foreach (var guid in guids)
			if (!selectedGuids[costOption].Contains(guid))
				selectedGuids[costOption].Add(guid);

		SetData(zentiaExchange);
	}

	public void OnChooseNoCardDel()
	{
		selectedGuids = new Dictionary<object, List<string>>();
		SetData(zentiaExchange);
	}

	//Icon图标
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickNameIconBtn(UIButton btn)
	{
		if (btn.Data == null)
			return;
		ShowCostView(btn.Data);
	}

}
