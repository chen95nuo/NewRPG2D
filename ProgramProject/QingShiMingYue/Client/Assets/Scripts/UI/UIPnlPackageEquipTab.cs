using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageEquipTab : UIPnlPackageBase
{
	public UIScrollList equipList;
	public GameObjectPool equipObjectPool;
	public GameObjectPool getPool;
	public GameObjectPool topItemPool;
	public GameObjectPool bottomItemPool;
	public UIButton goToGetMore;
	public SpriteText emptyTip;
	public SpriteText selectedText;

	private List<KodGames.ClientClass.Equipment> equipToFillList = new List<KodGames.ClientClass.Equipment>();

	private int maxRowsInPage;
	private int maxRows;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		goToGetMore.Text = GameUtility.GetUIString("UIPnlPackageEquipment_BotBtn");//去历练获取装备
		maxRowsInPage = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.piecePageCount;
		maxRows = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.maxCountItemInUI;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		SysUIEnv.Instance.GetUIModule<UIPnlPackageTab>().ChangeTabButtons(_UIType.UIPnlPackageEquipTab);
		InitUI();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	public void ClearData()
	{
		StopCoroutine("FillList");
		equipToFillList.Clear();
		equipList.ClearList(false);
		equipList.ScrollPosition = 0f;
		emptyTip.Text = "";
	}

	private void InitUI()
	{
		ClearData();
		InitData();
		StartCoroutine("FillList", false);
	}

	protected override void InitData()
	{
		base.InitData();

		equipToFillList.Clear();

		var equipFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.PackageEquip);
		var equipTypeFilter = equipFilter.GetFilterDataByType(PackageFilterData._FilterType.EquipType);
		var equipQualityFilter = equipFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);

		foreach (var equip in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			var equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equip.ResourceId);

			// 数据筛选 装备类型
			if (!equipTypeFilter.Contains(equipConfig.type))
				continue;

			// 数据筛选 品质
			if (!equipQualityFilter.Contains(equipConfig.qualityLevel))
				continue;

			equipToFillList.Add(equip);
		}

		if (equipToFillList.Count > 0)
			equipToFillList.Sort(DataCompare.CompareEquipment);

		// 设置空信息提示
		if (equipToFillList.Count > 0)
		{
			emptyTip.Text = string.Empty;
		}
		else
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Equip");

		if (ItemInfoUtility.CheckEquipAllSelected(PackageFilterData._DataType.PackageEquip))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");
	}

	#region MoreButton Controll

	private int GetEquipListItemCount()
	{
		if (equipList.Count <= 0)
			return 0;

		int count = equipList.Count;
		if (equipList.GetItem(0).Data == null)
			count--;

		if (equipList.Count <= 1)
			return count;

		if (equipList.GetItem(equipList.Count - 1).Data == null)
			count--;

		return count;
	}

	private UIElemPackageItemBase GetFirstEquipListItem(bool headNotTail)
	{
		if (headNotTail)
		{
			if (equipList.Count <= 0)
				return null;

			if (equipList.GetItem(0).Data != null)
				return equipList.GetItem(0).Data as UIElemPackageItemBase;

			if (equipList.Count <= 1)
				return null;

			if (equipList.GetItem(1).Data is UIElemPackageItemBase)
				return equipList.GetItem(1).Data as UIElemPackageItemBase;
			else
				return null;
		}
		else
		{
			if (equipList.Count <= 0)
				return null;

			if (equipList.GetItem(equipList.Count - 1).Data != null)
				return equipList.GetItem(equipList.Count - 1).Data as UIElemPackageItemBase;

			if (equipList.Count <= 1)
				return null;

			if (equipList.GetItem(equipList.Count - 2).Data is UIElemPackageItemBase)
				return equipList.GetItem(equipList.Count - 2).Data as UIElemPackageItemBase;
			else
				return null;
		}
	}

	private bool HasShowMoreButton(bool headNotTail)
	{
		if (equipList.Count == 0)
			return false;

		if (headNotTail)
			return equipList.GetItem(0).Data == null;
		else
			return equipList.GetItem(equipList.Count - 1).Data == null;
	}

	private void AddShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true))
				return;

			UIListItemContainer viewMoreContainer = topItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			equipList.InsertItem(viewMoreContainer, 0, false, "", false);
		}
		else
		{
			if (HasShowMoreButton(false))
				equipList.RemoveItem(equipList.Count - 1, false, true, false);

			UIListItemContainer viewMoreContainer = bottomItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			equipList.InsertItem(viewMoreContainer, equipList.Count, true, "", false);
		}
	}

	private void RemoveShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true) == false)
				return;

			equipList.RemoveItem(0, false, false, false);
		}
		else
		{
			UIListItemContainer getContainer = getPool.AllocateItem().GetComponent<UIListItemContainer>();

			if (HasShowMoreButton(false) == false)
			{
				// ��� ȥ�������װ�� ��ť
				equipList.InsertItem(getContainer, equipList.Count, true, "", false);
				return;
			}

			equipList.RemoveItem(equipList.Count - 1, false, true, false);
			// ��� ȥ�������װ�� ��ť
			equipList.InsertItem(getContainer, equipList.Count, true, "", false);
		}
	}

	private void UpdateShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (equipToFillList.Count == 0)
				// û��Ҫ���Ľ�ɫ��
				RemoveShowMoreButton(true);
			else
			{
				var item = GetFirstEquipListItem(true);
				Debug.Assert(item != null);
				if (item.indexInList > 0)
					AddShowMoreButton(true);
				else
					RemoveShowMoreButton(true);
			}
		}
		else
		{
			if (equipToFillList.Count == 0)
				// û��Ҫ���Ľ�ɫ��
				RemoveShowMoreButton(false);
			else
			{
				var item = GetFirstEquipListItem(false);
				Debug.Assert(item != null);
				if (item.indexInList < equipToFillList.Count - 1)
					AddShowMoreButton(false);
				else
					RemoveShowMoreButton(false);
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(bool headNotTail)
	{
		yield return null;

		if (headNotTail)
		{
			// �������һ��Item
			var firstItem = GetFirstEquipListItem(true);

			// ���û�У��ӵ�һ����ʼ����, ����ӵ�һ����ǰһ����ʼ����
			int firstIndex = firstItem == null ? 0 : firstItem.indexInList - 1; ;

			int firstStartindex = firstIndex;
			for (; firstStartindex > firstIndex - maxRowsInPage && firstStartindex >= 0; firstStartindex--)
			{
				UIListItemContainer firstContainer = null;

				firstContainer = equipObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemPackageEquipItem item = firstContainer.gameObject.GetComponent<UIElemPackageEquipItem>();
				item.SetData(equipToFillList[firstStartindex]);
				item.indexInList = firstStartindex;


				// ��ǰ����
				if (HasShowMoreButton(true) == false)
					equipList.InsertItem(firstContainer, 0, false, "", false);
				else
					equipList.InsertItem(firstContainer, 1, false, "", false);
			}

			UpdateShowMoreButton(true);
		}
		else
		{
			// �������һ��Item
			var lastItem = GetFirstEquipListItem(false);

			// ���û�У��ӵ�һ����ʼ����, ��������һ������һ����ʼ����
			int lastIndex = lastItem == null ? 0 : lastItem.indexInList + 1;
			int lastStartindex = lastIndex;
			for (; lastStartindex < lastIndex + maxRowsInPage && lastStartindex < equipToFillList.Count; lastStartindex++)
			{
				UIListItemContainer lastContainer = null;
				lastContainer = equipObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemPackageEquipItem item = lastContainer.gameObject.GetComponent<UIElemPackageEquipItem>();
				item.SetData(equipToFillList[lastStartindex]);
				item.indexInList = lastStartindex;


				// If "view more" button is in the list, insert before it
				if (HasShowMoreButton(false) == false)
					equipList.InsertItem(lastContainer, equipList.Count, true, "", false);
				else
					equipList.InsertItem(lastContainer, equipList.Count - 1, true, "", false);
			}

			UpdateShowMoreButton(false);
		}

		int avatarItemCount = GetEquipListItemCount();
		if (avatarItemCount > maxRows)
		{
			bool hasHeadShowMore = HasShowMoreButton(true);

			if (headNotTail)
			{
				// �ӿ�ʼɾ��������ʾ������item
				for (int headIndex = 0; headIndex < avatarItemCount - maxRows; ++headIndex)
				{
					int headRemoveIndex = hasHeadShowMore ? maxRows + 1 : maxRows;
					equipList.RemoveItem(headRemoveIndex, false, true, false);
				}

				UpdateShowMoreButton(false);
			}
			else
			{
				// �ӿ�ʼɾ��������ʾ������item
				for (int tailIndex = avatarItemCount - maxRows - 1; tailIndex >= 0; --tailIndex)
				{
					int tailRemoveIndex = hasHeadShowMore ? tailIndex + 1 : tailIndex;
					equipList.RemoveItem(tailRemoveIndex, false, false, false);
				}

				UpdateShowMoreButton(true);
			}
		}
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnPrePageClick(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList", true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnNextPageClick(UIButton btn)
	{
		StopCoroutine("FillList");
		StartCoroutine("FillList", false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickScrollIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI((int)assetIcon.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEquipIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		KodGames.ClientClass.Equipment equipment = assetIcon.Data as KodGames.ClientClass.Equipment;

		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlEquipmentInfo, equipment, false, true, false, true, null, false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefineEquip(UIButton btn)
	{
		KodGames.ClientClass.Equipment equip = (KodGames.ClientClass.Equipment)btn.data;

		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlEquipmentLevelup, equip);
	}

	//Show Equipment Who Is Equiped
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickExplicitEquip(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpEquipDesc(btn.data as KodGames.ClientClass.Equipment);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterEquipment(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageEquipFilter), PackageFilterData._DataType.PackageEquip, new UIDlgPackageEquipFilter.OnSelectFilterEquip(InitUI));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSell(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlPackageSell, _UIType.UIPnlPackageEquipTab);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetEquipment(UIButton btn)
	{
		GameUtility.JumpUIPanel(_UIType.UI_Dungeon);
	}

	public void RefreshView(KodGames.ClientClass.Equipment refreshEquipment)
	{
		// 刷新界面数据
		int refreshEquipIndex = 0;
		var deleteContainers = new List<UIListItemContainer>();

		for (int index = 0; index < equipList.Count; index++)
		{
			object itemData = equipList.GetItem(index).Data;
			if (itemData == null)
				continue;

			if (itemData is UIElemPackageEquipItem)
			{
				var equipItem = equipList.GetItem(index).Data as UIElemPackageEquipItem;

				if (SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(equipItem.Equip.Guid) == null)
					deleteContainers.Add(equipItem.container);
				else if (refreshEquipment != null && refreshEquipment.Guid.Equals(equipItem.Equip.Guid))
				{
					refreshEquipIndex = equipItem.indexInList;
					equipItem.SetData(refreshEquipment);
				}
			}
		}

		// Delete Not Exist Container.
		foreach (var container in deleteContainers)
		{
			UIElemPackageItemBase item = container.Data as UIElemPackageItemBase;

			if (item.indexInList < refreshEquipIndex)
				equipList.RemoveItem(item.container, false, false, false);
			else
				equipList.RemoveItem(item.container, false, true, false);

		}

		// Refresh LocalData.
		for (int index = equipToFillList.Count - 1; index >= 0; index--)
		{
			var equip = SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(equipToFillList[index].Guid);
			if (equip == null)
				equipToFillList.RemoveAt(index);
			else if (refreshEquipment != null && equip.Guid == refreshEquipment.Guid)
				equipToFillList[index] = equip;
		}

		// Reset the first and end item position.
		for (int index = 0; index < equipToFillList.Count; index++)
		{
			for (int subIndex = 0; subIndex < equipList.Count; subIndex++)
			{
				if (equipList.GetItem(subIndex).Data == null)
					continue;

				if (equipList.GetItem(subIndex).Data is UIElemPackageEquipItem
					&& (equipList.GetItem(subIndex).Data as UIElemPackageEquipItem).Equip.Guid.Equals(equipToFillList[index].Guid))
					(equipList.GetItem(subIndex).Data as UIElemPackageEquipItem).indexInList = index;
			}
		}
	}

	
}

