using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageItemTab : UIPnlPackageBase
{
	public UIScrollList itemList;
	public GameObjectPool itemObjectPool;
	public GameObjectPool moreItemPool;

	public SpriteText emptyTip;

	private List<KodGames.ClientClass.Consumable> consumables = new List<KodGames.ClientClass.Consumable>();

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlPackageTab>().ChangeTabButtons(_UIType.UIPnlPackageItemTab);

		ClearData();
		InitData();
		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	protected override void InitData()
	{
		base.InitData();

		foreach (var consumable in SysLocalDataBase.Inst.LocalPlayer.Consumables)
		{
			if (GameUtility.CanItemShowInPackage(consumable.Id) && !consumables.Contains(consumable))
				consumables.Add(consumable);
		}
		if (consumables.Count > 0)
			consumables.Sort(DataCompare.CompareConsumable);

		StartCoroutine("FillList");
	}

	private void ClearData()
	{
		consumables.Clear();
		StopCoroutine("FillList");
		itemList.ClearList(false);
		itemList.ScrollPosition = 0;
		emptyTip.Text = string.Empty;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList()
	{
		yield return null;

		if (consumables.Count <= 0)
		{
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Consumable");
			yield break;
		}

		int maxRows = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.maxCountItemInUI;
		int currentCount = GetLastConsumableListItemIndex();
		int index = currentCount;

		for (; index < currentCount + maxRows && index < consumables.Count; index++)
		{
			UIElemPackageItemItem item = itemObjectPool.AllocateItem().GetComponent<UIElemPackageItemItem>();

			item.SetData(consumables[index]);

			if (itemList.Count == 0 || itemList.GetItem(itemList.Count - 1).Data is UIElemPackageItemItem)
			{
				itemList.AddItem(item.container);
			}
			else
			{
				itemList.InsertItem(item.container, itemList.Count - 1);
			}
		}

		if (itemList.Count > 0 && itemList.GetItem(itemList.Count - 1).Data == null)
			itemList.RemoveItem(itemList.Count - 1, false);
		if (GetLastConsumableListItemIndex() < consumables.Count)
		{
			itemList.AddItem(moreItemPool.AllocateItem());
		}

		if (itemList.Count > 0 && !emptyTip.Text.Equals(string.Empty))
			emptyTip.Text = string.Empty;
	}

	private int GetLastConsumableListItemIndex()
	{
		if (itemList.Count <= 0)
			return 0;

		if (itemList.GetItem(itemList.Count - 1).Data is UIElemPackageItemItem)
			return itemList.Count;
		else
			return itemList.Count - 1;
	}

	private UIListItemContainer GetContainerByConsumableID(int consumableID)
	{
		for (int index = 0; index < itemList.Count; index++)
		{
			UIListItemContainer container = itemList.GetItem(index) as UIListItemContainer;
			UIElemPackageItemItem packageItem = container.data as UIElemPackageItemItem;

			if (packageItem != null && packageItem.Consumable.Id == consumableID)
				return container;
		}

		return null;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMoreCard(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		UIElemPackageItemItem packageItem = assetIcon.Data as UIElemPackageItemItem;

		ItemConfig.Item itemCfg = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(packageItem.Consumable.Id);
		if (itemCfg != null && itemCfg.consumeRewardGroup != null && itemCfg.consumeRewardGroup.Count > 1)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOpenPackage), packageItem.Consumable.Id);
		else
			GameUtility.ShowAssetInfoUI(packageItem.Consumable.Id);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOpenButton(UIButton btn)
	{
		UIElemPackageItemItem item = btn.data as UIElemPackageItemItem;

		// When Open Gacha , Check Package Capacity.
		switch (ItemConfig._Type.ToItemType(item.Consumable.Id))
		{
			case ItemConfig._Type.Gacha:
			case ItemConfig._Type.Package:
			case ItemConfig._Type.KeyItem:
				if (GameUtility.CheckPackageCapacity() == false)
					return;
				break;
		}

		if (ItemConfig._Type.ToItemType(item.Consumable.Id) == ItemConfig._Type.IllusionCostItem || item.Consumable.Id == ConfigDatabase.DefaultCfg.ItemConfig.illusionStoneId)
		{
			//幻化系统有判功能是否开启的逻辑，但提示文字与通用功能不同
			if (!ConfigDatabase.DefaultCfg.LevelConfig.GetFunctionStatusByOpenFunction(_OpenFunctionType.Illusion))
			{
				//目前幻影阁正在施工中，过段时间再来吧
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("Illusion_FuncNotOpenTipText"));
				return;
			}
			else if (!GameUtility.CheckFuncOpened(_OpenFunctionType.Illusion, false, false))
			{
				//需要您达到{0}级，才能进入幻影阁哦
				int limitLevel = ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Illusion);
				if (limitLevel > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
				{
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("Illusion_FuncNotOpen_LevelNotSatisfied", limitLevel));
					return;
				}

				int vipLevel = ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.Illusion);
				if (vipLevel > SysLocalDataBase.Inst.LocalPlayer.VipLevel)
				{
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.FormatUIString("Illusion_FuncNotOpen_LevelNotSatisfied", vipLevel));
					return;
				}
			}
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlIllusion);
			return;
		}

		ItemConfig.Item itemCfg = ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(item.Consumable.Id);

		if (itemCfg != null && itemCfg.consumeRewardGroup != null && itemCfg.consumeRewardGroup.Count > 1)
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgOpenPackage), item.Consumable.Id);
		}
		else
		{
			int maxCount = GameUtility.CalMaxConsumeAmount(item.Consumable.Id);

			if (maxCount <= 1
				|| SysLocalDataBase.Inst.LocalPlayer.VipLevel < ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.BatUseConsumable))
			{
				RequestMgr.Inst.Request(new ConsumeItemReq(item.Consumable.Id, 1));
				return;
			}
			else
			{
				UIDlgOpenBox.ShowData showData = new UIDlgOpenBox.ShowData();
				showData.iconId = item.Consumable.Id;
				showData.selectCount = maxCount;
				SysUIEnv.Instance.GetUIModule<UIDlgOpenBox>().ShowDialog(showData);
			}
		}
	}

	private void ShowRewardMessage(int consumableId, int consumableCount, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
		switch (ConfigDatabase.DefaultCfg.ItemConfig.GetItemById(consumableId).type)
		{
			case ItemConfig._Type.Package:
				var showdata = new UIDlgShopGiftPreview.ShowData();
				showdata.title = GameUtility.GetUIString("UIPackage_Open_Gacha_Title");
				showdata.message = GameUtility.GetUIString("UIPackage_Open_Gacha_Message");
				showdata.rewardDatas.Add(new UIDlgShopGiftPreview.RewardData(SysLocalDataBase.CCRewardToCSCReward(costAndRewardAndSync.Reward)));

				SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(_UIType.UIDlgShopGiftPreview, showdata));
				break;
			case ItemConfig._Type.Gacha:
				SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(_UIType.UIEffectOpenBox, consumableCount, costAndRewardAndSync));
				break;
			case ItemConfig._Type.KeyItem:
				SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(_UIType.UIEffectOpenBox, consumableCount, costAndRewardAndSync));
				break;
			default:
				string tipText = string.Format(GameUtility.GetUIString("UI_ConsumItem_Reward"), ItemInfoUtility.GetAssetName(consumableId), SysLocalDataBase.GetRewardDesc(costAndRewardAndSync.Reward, true));
				SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), tipText);
				break;
		}
	}

	public void RefreshView(KodGames.ClientClass.Reward reward, List<KodGames.ClientClass.Cost> costs)
	{
		foreach (var cost in costs)
		{
			UIListItemContainer container = GetContainerByConsumableID(cost.Id);
			if (container != null)
			{
				if (SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(cost.Id) == null)
					itemList.RemoveItem(container, false);
				else
					(container.Data as UIElemPackageItemItem).RefreshView();
			}
		}

		// Remove not exist in Local Data consumables.
		for (int index = consumables.Count - 1; index >= 0; index--)
			if (consumables[index] == null || SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(consumables[index].Id) == null)
				consumables.RemoveAt(index);

		foreach (var rewardConsumable in reward.Consumable)
		{
			if (!GameUtility.CanItemShowInPackage(rewardConsumable.Id))
				continue;

			UIListItemContainer container = GetContainerByConsumableID(rewardConsumable.Id);
			if (container != null)
				(container.Data as UIElemPackageItemItem).RefreshView();
			else if (itemList.Count == 0 || (itemList.Count > 0 && itemList.GetItem(itemList.Count - 1).Data != null))
			{
				UIElemPackageItemItem item = itemObjectPool.AllocateItem().GetComponent<UIElemPackageItemItem>();
				item.SetData(rewardConsumable);

				itemList.AddItem(item.container);
			}
		}

		if (itemList.Count <= 0)
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Consumable");
	}

	public void OnConsumeItemSuccess(int consumableId, int consumableCount, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
		ShowRewardMessage(consumableId, consumableCount, costAndRewardAndSync);

		RefreshView(costAndRewardAndSync.Reward, costAndRewardAndSync.Costs);
	}
}


