using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;
using KodGames.ClientClass;

using ClientCost = KodGames.ClientClass.ItemEx;

public class UIPnlChooseCard : UIModule
{
	public delegate void OnChooseCardSuccessDel(object costOption, List<string> selectedGuids);
	public delegate void OnChooseCardFailDel();

	public SpriteText tabLabel;
	public UIScrollList cardList;
	public GameObjectPool avatarPool;
	public GameObjectPool equipPool;
	public GameObjectPool skillPool;
	public GameObjectPool danPool;
	public UIButton okButton;
	public UIButton backButton;
	public SpriteText totalNeed;
	public SpriteText needName;
	public SpriteText currentSelectNum;
	public SpriteText tip;
	public SpriteText noCard;

	private object costOption = null;
	private int totalCount = -1;
	private int selectedCount = 0;
	private List<string> selectedGuids;
	private OnChooseCardSuccessDel onChooseCardSuccessDel;
	private OnChooseCardFailDel onChooseCardFailDel;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (userDatas.Length < 2)
			return true;

		// Trans Data.
		costOption = userDatas[0];
		onChooseCardSuccessDel = userDatas[1] as OnChooseCardSuccessDel;

		if (userDatas.Length > 2)
			onChooseCardFailDel = userDatas[2] as OnChooseCardFailDel;

		totalCount = ItemInfoUtility.ExchangeGetCostCount(costOption);

		if (costOption == null || ItemInfoUtility.ExchangeIsOptionCost(costOption) == false || totalCount < 0)
			return true;

		if (selectedGuids == null)
			selectedGuids = new List<string>();

		selectedGuids.Clear();
		totalCount = ItemInfoUtility.ExchangeGetCostCount(costOption);
		tabLabel.Text = GameUtility.GetUIString(ItemInfoUtility.ExchangeGetCostType(costOption) == IDSeg._AssetType.Dan ? "UIPnlChooseCard_TabDanName" : "UIPnlChooseCard_TabCardName");
		totalNeed.Text = GameUtility.FormatUIString(ItemInfoUtility.ExchangeGetCostType(costOption) == IDSeg._AssetType.Dan ? "UIPnlChooseCard_TotalNeedDan" : "UIPnlChooseCard_TotalNeed", totalCount, ItemInfoUtility.ExchangeGetBreakLabelDesc(costOption));
		needName.Text = ItemInfoUtility.ExchangeGetCostName(costOption);
		noCard.Text = string.Empty;

		StartCoroutine("FillList");

		return true;
	}

	public override void OnHide()
	{
		StopCoroutine("FillList");
		cardList.ClearList(false);
		cardList.ScrollPosition = 0f;

		selectedCount = 0;
		totalCount = -1;
		costOption = null;
		onChooseCardSuccessDel = null;
		onChooseCardFailDel = null;
		selectedGuids = null;

		base.OnHide();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		switch (ItemInfoUtility.ExchangeGetCostType(costOption))
		{
			case IDSeg._AssetType.Avatar:
				FillAvatarList();
				break;

			case IDSeg._AssetType.Equipment:
				FillEquipList();
				break;

			case IDSeg._AssetType.CombatTurn:
				FillSkillList();
				break;

			case IDSeg._AssetType.Dan:
				FillDanList();
				break;
		}

		currentSelectNum.Text = GameUtility.FormatUIString("UIPnlChooseCard_Tab", selectedCount, totalCount);
	}

	private void SetControl(bool haveCard)
	{
		okButton.Hide(!haveCard);

		if (haveCard == false)
			noCard.Text = GameUtility.GetUIString(ItemInfoUtility.ExchangeGetCostType(costOption) == IDSeg._AssetType.Dan ? "UIPnlChooseCard_DanWarning" : "UIPnlChooseCard_Warning");
	}

	private void FillAvatarList()
	{
		List<KodGames.ClientClass.Avatar> avaliableAvatars = null;

		if (costOption is ClientCost)
			avaliableAvatars = ItemInfoUtility.ExchangeGetAvaliableAvatar(costOption as ClientCost);
		else if (costOption is KodGames.ClientClass.CostAsset)
			avaliableAvatars = ItemInfoUtility.ExchangeGetAvaliableAvatar(costOption as KodGames.ClientClass.CostAsset);

		SetControl(avaliableAvatars.Count > 0);

		if (avaliableAvatars.Count <= 0)
			return;

		avaliableAvatars.Sort(DataCompare.CompareAvatarReverse);
		foreach (var a in avaliableAvatars)
		{
			UIListItemContainer container = avatarPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemChooseCardAvatar avatar = container.gameObject.GetComponent<UIElemChooseCardAvatar>();

			avatar.SetData(a, false);
			container.Data = avatar;
			cardList.AddItem(container);
		}
	}

	private void FillEquipList()
	{
		List<KodGames.ClientClass.Equipment> avaliableEquips = null;

		if (costOption is ClientCost)
			avaliableEquips = ItemInfoUtility.ExchangeGetAvaliableEquip(costOption as ClientCost);
		else if (costOption is KodGames.ClientClass.CostAsset)
			avaliableEquips = ItemInfoUtility.ExchangeGetAvaliableEquip(costOption as KodGames.ClientClass.CostAsset);

		SetControl(avaliableEquips.Count > 0);

		if (avaliableEquips.Count <= 0)
			return;

		avaliableEquips.Sort(DataCompare.CompareEquipmentReverse);

		foreach (var e in avaliableEquips)
		{
			UIListItemContainer container = equipPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemChooseCardEquip equip = container.gameObject.GetComponent<UIElemChooseCardEquip>();

			equip.SetData(e, false);
			container.Data = equip;
			cardList.AddItem(container);
		}
	}

	private void FillSkillList()
	{
		List<KodGames.ClientClass.Skill> avaliableSkills = null;

		if (costOption is ClientCost)
			avaliableSkills = ItemInfoUtility.ExchangeGetAvaliableSkill(costOption as ClientCost);
		else if (costOption is KodGames.ClientClass.CostAsset)
			avaliableSkills = ItemInfoUtility.ExchangeGetAvaliableSkill(costOption as KodGames.ClientClass.CostAsset);

		SetControl(avaliableSkills.Count > 0);

		if (avaliableSkills.Count <= 0)
			return;

		avaliableSkills.Sort(DataCompare.CompareSkillReverse);

		foreach (var s in avaliableSkills)
		{
			UIListItemContainer container = skillPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemChooseCardSkill skill = container.gameObject.GetComponent<UIElemChooseCardSkill>();

			skill.SetData(s, false);
			container.Data = skill;
			cardList.AddItem(container);
		}
	}

	private void FillDanList()
	{
		List<KodGames.ClientClass.Dan> avaliableDans = null;

		if (costOption is ClientCost)
			avaliableDans = ItemInfoUtility.ExchangeGetAvailiableDan(costOption as ClientCost);
		else if (costOption is KodGames.ClientClass.CostAsset)
			avaliableDans = ItemInfoUtility.ExchangeGetAvailiableDan(costOption as KodGames.ClientClass.CostAsset);

		SetControl(avaliableDans.Count > 0);

		if (avaliableDans.Count <= 0)
			return;

		avaliableDans.Sort(DataCompare.CompareDanReverse);

		foreach (var d in avaliableDans)
		{
			UIListItemContainer container = danPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemChooseCardDan dan = container.gameObject.GetComponent<UIElemChooseCardDan>();

			dan.SetData(d, false);
			container.Data = dan;
			cardList.AddItem(container);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBack(UIButton btn)
	{
		if (onChooseCardFailDel != null)
			onChooseCardFailDel();

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnOk(UIButton btn)
	{
		if (selectedCount < totalCount)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("UIPnlChooseCard_SelectInsufficient", (totalCount - selectedCount), ItemInfoUtility.ExchangeGetCostName(costOption)));
			return;
		}

		selectedGuids = new List<string>();

		for (int i = 0; i < cardList.Count; i++)
		{
			UIListItemContainer container = cardList.GetItem(i) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemChooseCardBasic elem = null;
			switch (ItemInfoUtility.ExchangeGetCostType(costOption))
			{
				case IDSeg._AssetType.Avatar:
					elem = container.Data as UIElemChooseCardAvatar;
					break;

				case IDSeg._AssetType.Equipment:
					elem = container.Data as UIElemChooseCardEquip;
					break;

				case IDSeg._AssetType.CombatTurn:
					elem = container.Data as UIElemChooseCardSkill;
					break;

				case IDSeg._AssetType.Dan:
					elem = container.Data as UIElemChooseCardDan;
					break;
			}

			if (elem != null && elem.ItemSelected && !elem.Guid.Equals("") && !selectedGuids.Contains(elem.Guid))
				selectedGuids.Add(elem.Guid);
		}

		if (onChooseCardSuccessDel != null)
			onChooseCardSuccessDel(costOption, selectedGuids);

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectClick(UIButton btn)
	{
		UIElemChooseCardBasic elem = btn.Data as UIElemChooseCardBasic;
		if (elem == null)
			return;

		if (selectedCount >= totalCount && !elem.ItemSelected)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlChooseCard_AlreadyEnough"));
		else
		{
			elem.ToggleState();

			if (elem.ItemSelected)
				selectedCount++;
			else
				selectedCount--;

			currentSelectNum.Text = GameUtility.FormatUIString("UIPnlChooseCard_Tab", selectedCount, totalCount);
		}
	}
}
