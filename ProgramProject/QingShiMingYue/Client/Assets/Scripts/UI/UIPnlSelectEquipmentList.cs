using UnityEngine;
using System.Collections;
using ClientServerCommon;
using System.Collections.Generic;

public class UIPnlSelectEquipmentList : UIPnlItemInfoBase
{
	public UIScrollList scrollList;
	public GameObjectPool objectPool;
	public GameObjectPool getObjectPool;
	public GameObjectPool moreItemPool;
	public SpriteText emptyTip;
	public AutoSpriteControlBase tabControllBase;
	public SpriteText selectedText;

	private KodGames.ClientClass.Location equipLocation;
	private int equipType;

	private const int sMaxRows = 20;
	private int currentPosition = 0;
	private List<KodGames.ClientClass.Equipment> equipmentsToFillList = new List<KodGames.ClientClass.Equipment>();

	private UIListItemContainer viewMoreBtnItem;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.equipType = (int)userDatas[0];
		this.equipLocation = userDatas[1] as KodGames.ClientClass.Location;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		// Clear List.
		StopCoroutine("FillData");
		currentPosition = -1;
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
		viewMoreBtnItem = null;
		// Clear Data.
		equipmentsToFillList.Clear();
		equipLocation = null;

		emptyTip.Text = "";
	}

	private void InitView()
	{
		// Clear Data For Filter Data.
		StopCoroutine("FillData");
		currentPosition = -1;
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;
		equipmentsToFillList.Clear();
		viewMoreBtnItem = null;
		// Set the Tab Text.
		tabControllBase.Text = EquipmentConfig._Type.GetDisplayNameByType(equipType, ConfigDatabase.DefaultCfg);

		// Filter By FilterFunction.
		var equipFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.SelectEquip);
		var equipQualityFilter = equipFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);

		// Avatar Assemble Data.
		var assembleIds = ItemInfoUtility.GetAvatarAssembleRequireIds(PlayerDataUtility.GetLineUpAvatar(SysLocalDataBase.Inst.LocalPlayer, equipLocation.PositionId, equipLocation.ShowLocationId));

		// Init Data.
		foreach (var equipment in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			var equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId);

			// Skip by equipment type
			if (equipType != equipConfig.type)
				continue;

			// Skip Current equipment.
			if (equipment.Guid.Equals(equipLocation.Guid))
				continue;

			// 数据筛选 品质
			if (!equipQualityFilter.Contains(equipConfig.qualityLevel))
				continue;

			// Set Assemble Active value.
			equipment.IsAssembleActive = assembleIds.Contains(equipment.ResourceId);

			equipmentsToFillList.Add(equipment);

		}

		equipmentsToFillList.Sort(DataCompare.CompareEquipmentForLineUp);

		if (equipmentsToFillList.Count > 0)
		{
			currentPosition = 0;
			StartCoroutine("FillData");
			emptyTip.Text = string.Empty;
		}
		else
		{
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Equip");
			AddGetPoolItem();
		}

		if (ItemInfoUtility.CheckEquipAllSelected(PackageFilterData._DataType.SelectEquip))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillData()
	{
		yield return null;

		int rangeCount = Mathf.Min(sMaxRows, equipmentsToFillList.Count - currentPosition);
		List<KodGames.ClientClass.Equipment> equipments = equipmentsToFillList.GetRange(currentPosition, rangeCount);

		foreach (KodGames.ClientClass.Equipment equipment in equipments)
		{
			UIElemEquipSelectItem item = objectPool.AllocateItem().GetComponent<UIElemEquipSelectItem>();

			item.SetData(equipment, equipLocation.PositionId);


			if (viewMoreBtnItem == null)
				scrollList.AddItem(item.container);
			else
				scrollList.InsertItem(item.container, scrollList.Count - 1);
		}

		currentPosition += rangeCount;


		if (equipmentsToFillList.Count > currentPosition)
		{
			if (viewMoreBtnItem == null)
			{
				UIListItemContainer viewMoreContainer = moreItemPool.AllocateItem().GetComponent<UIListItemContainer>();
				viewMoreBtnItem = viewMoreContainer;
				scrollList.AddItem(viewMoreContainer);
			}
			//   AddShowMoreButton();
		}
		else if (viewMoreBtnItem != null)
		{
			// RemoveShowMoreButton();
			scrollList.RemoveItem(viewMoreBtnItem, false, true, false);
			viewMoreBtnItem = null;
			AddGetPoolItem();
		}

		else if (currentPosition <= equipmentsToFillList.Count)
		{

			// RemoveShowMoreButton();
			AddGetPoolItem();
		}



	}

	private bool HasShowMoreButton()
	{
		return scrollList.Count == 0 || (scrollList.GetItem(scrollList.Count - 1).Data is UIElemEquipSelectItem) == false;
	}

	private void AddShowMoreButton()
	{
		scrollList.AddItem(moreItemPool.AllocateItem());
	}

	private void RemoveShowMoreButton()
	{
		scrollList.RemoveItem(scrollList.Count - 1, false);
	}

	private void AddGetPoolItem()
	{
		UIListItemContainer getContainer = getObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		scrollList.InsertItem(getContainer, scrollList.Count, true, "", false);
	}


	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMoreCardShow(UIButton btn)
	{
		StopCoroutine("FillData");
		StartCoroutine("FillData");
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetEquipment(UIButton btn)
	{
		if (!(SysGameStateMachine.Instance.CurrentState is GameState_Dungeon))
			GameUtility.JumpUIPanel(_UIType.UI_Dungeon);
		else
		{
			this.HideSelf();

			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
				SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatar));
		}
	}


	//点击图标，显示详细内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		UIElemEquipSelectItem item = assetIcon.Data as UIElemEquipSelectItem;

		// Show equipment info dialog "with" close button and "select" button
		UIPnlEquipmentInfo.SelectDelegate selectDelegate = new UIPnlEquipmentInfo.SelectDelegate(SeletEquipmentItemByEquip);
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlEquipmentInfo, item.Equipment, false, true, false, false, selectDelegate, true, 0);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemMiddleInfo(UIButton btn)
	{
		KodGames.ClientClass.Equipment currentEquip = PlayerDataUtility.GetLineUpEquipmentByType(SysLocalDataBase.Inst.LocalPlayer, equipLocation.PositionId, equipLocation.ShowLocationId, equipType);

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAttributeComparison), currentEquip, (btn.Data as UIElemEquipSelectItem).Equipment);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemDetailInfo(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpEquipDesc((btn.Data as UIElemEquipSelectItem).Equipment);
	}

	//Click to return to UIPnlGuide.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterEquipment(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageEquipFilter), PackageFilterData._DataType.SelectEquip, new UIDlgPackageEquipFilter.OnSelectFilterEquip(InitView));
	}

	//点击更换装备
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectBtnClick(UIButton btn)
	{
		UIElemEquipSelectItem item = btn.data as UIElemEquipSelectItem;
		if (null == item)
			return;
		else
			SeletEquipmentItemByEquip(item.Equipment);
	}

	public void OnChangeEquipmentSuccess(KodGames.ClientClass.Location location)
	{
		HideSelf();

		SysModuleManager.Instance.GetSysModule<SysUIEnv>().GetUIModule<UIPnlAvatar>().OnChangeEquipmentSuccess(location, equipType);
	}

	public void SeletEquipmentItemByEquip(KodGames.ClientClass.Equipment equip)
	{
		RequestMgr.Inst.Request(new ChangeLocationReq(equip.Guid, equip.ResourceId, equipLocation.Guid, equipLocation.PositionId, equipLocation.ShowLocationId));
	}
}