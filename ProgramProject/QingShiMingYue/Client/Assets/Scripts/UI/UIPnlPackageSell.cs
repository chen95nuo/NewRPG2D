using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageSell : UIPnlPackageBase
{
	public UIElemAssetIcon totalGoldBtn;

	public GameObjectPool avatarObjectPool;
	public GameObjectPool skillObjectPool;
	public GameObjectPool equipObjectPool;
	public GameObjectPool bottomItemPool;
	public GameObjectPool topItemPool;
	public UIScrollList scrollList;
	public UIChildLayoutControl childLayout;

	public UIElemSelectItem selectAllButton;

	public SpriteText filterTips;
	public SpriteText notCardLabel;
	public SpriteText selectedText;

	private List<KodGames.ClientClass.Equipment> equipments = new List<KodGames.ClientClass.Equipment>();
	private List<KodGames.ClientClass.Avatar> avatars = new List<KodGames.ClientClass.Avatar>();
	private List<KodGames.ClientClass.Skill> skills = new List<KodGames.ClientClass.Skill>();

	private List<UIElemSellBase.SellData> selectSellDatas = new List<UIElemSellBase.SellData>();

	private int type;
	public int Type
	{
		get { return type; }
	}

	private int maxRowsInPage;
	private int maxRows;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		maxRowsInPage = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.piecePageCount;
		maxRows = ConfigDatabase.DefaultCfg.GameConfig.uiShowSetting.maxCountItemInUI;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		this.type = (int)userDatas[0];

		//从GameUtility里Jump过来时会直接显示这个界面，此时要同步背包UI的Tab按钮的选中状态
		SysUIEnv.Instance.GetUIModule<UIPnlPackageTab>().ChangeTabButtons(this.type);

		InitView();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void InitView()
	{
		ClearData();
		InitData();
	}

	void ClearData()
	{
		StopCoroutine("FillList");
		scrollList.ClearList(false);
		scrollList.ScrollPosition = 0;

		avatars.Clear();
		equipments.Clear();
		skills.Clear();

		selectSellDatas.Clear();
	}

	#region  InitData
	protected override void InitData()
	{
		base.InitData();

		switch (this.type)
		{
			case _UIType.UIPnlPackageAvatarTab: InitAvatarData(); break;
			case _UIType.UIPnlPackageEquipTab: InitEquipData(); break;
			case _UIType.UIPnlPackageSkillTab: InitSkillData(); break;
		}

		//selectAllButton.Text = GameUtility.GetUIString("UIPnlPackageSell_SelectAll");
		selectAllButton.InitState("", OnClickSelectAll);
		notCardLabel.Text = "";

		if (CheckAllSelected(this.type))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");

		StartCoroutine("FillList", false);
	}

	private void InitAvatarData()
	{
		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.PackageAvatar);
		var avatarTraitFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType);
		var avatarQualityFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);
		var avatarCountryFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType);
		AvatarConfig.Avatar avatarConfig;

		foreach (var avatar in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (!avatar.IsAvatar)
				continue;

			if (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, avatar))
				continue;

			if (PlayerDataUtility.IsLineUpInParter(SysLocalDataBase.Inst.LocalPlayer, avatar))
				continue;

			//Filter
			avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

			if (!avatarTraitFilter.Contains(avatarConfig.traitType))
				continue;

			if (!avatarQualityFilter.Contains(avatarConfig.qualityLevel))
				continue;

			if (!avatarCountryFilter.Contains(avatarConfig.countryType))
				continue;

			avatars.Add(avatar);
		}

		if (avatars.Count > 0)
		{
			filterTips.Text = string.Empty;
			avatars.Sort(DataCompare.CompareAvatarReverse);
		}
		else filterTips.Text = GameUtility.GetUIString("UIPnlPackage_Tips_AvatarFilter");



	}

	private void InitEquipData()
	{
		var equipFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.PackageEquip);
		var equipTypeFilter = equipFilter.GetFilterDataByType(PackageFilterData._FilterType.EquipType);
		var equipQualityFilter = equipFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);

		foreach (var equipment in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			if (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, equipment))
				continue;

			var equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId);

			// 数据筛选 装备类型
			if (!equipTypeFilter.Contains(equipConfig.type))
				continue;

			// 数据筛选 品质
			if (!equipQualityFilter.Contains(equipConfig.qualityLevel))
				continue;

			equipments.Add(equipment);
		}

		if (equipments.Count > 0)
		{
			equipments.Sort(DataCompare.CompareEquipmentReverse);
			filterTips.Text = string.Empty;
		}
		else filterTips.Text = GameUtility.GetUIString("UIPnlPackage_Tips_EquipFilter");

	}

	private void InitSkillData()
	{
		var skillFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.PackageSkill);
		var skillQualityFilter = skillFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);

		foreach (var skill in SysLocalDataBase.Inst.LocalPlayer.Skills)
		{
			if (PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, skill))
				continue;

			var skillConfig = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId);
			// 数据筛选 品质
			if (!skillQualityFilter.Contains(skillConfig.qualityLevel))
				continue;

			skills.Add(skill);

		}
		if (skills.Count > 0)
		{
			skills.Sort(DataCompare.CompareSkillReverse);
			filterTips.Text = string.Empty;
		}
		else filterTips.Text = GameUtility.GetUIString("UIPnlPackage_Tips_SkillFilter");
	}
	#endregion

	#region  MoreButton Controll.

	private int GetDataToFillListCount()
	{
		switch (this.type)
		{
			case _UIType.UIPnlPackageAvatarTab: return avatars.Count;
			case _UIType.UIPnlPackageEquipTab: return equipments.Count;
			case _UIType.UIPnlPackageSkillTab: return skills.Count;
		}
		return 0;
	}

	private int GetScrollListItemCount()
	{
		if (scrollList.Count <= 0)
			return 0;

		int count = scrollList.Count;
		if (scrollList.GetItem(0).Data == null)
			count--;

		if (scrollList.Count <= 1)
			return count;

		if (scrollList.GetItem(scrollList.Count - 1).Data == null)
			count--;

		return count;
	}

	private UIElemSellBase GetFirstScrollListItem(bool headNotTail)
	{
		if (headNotTail)
		{
			if (scrollList.Count <= 0)
				return null;

			if (scrollList.GetItem(0).Data != null)
			{
				Debug.Assert(scrollList.GetItem(0).Data is UIElemSellBase);
				return scrollList.GetItem(0).Data as UIElemSellBase;
			}

			if (scrollList.Count <= 1)
				return null;

			if (scrollList.GetItem(1).Data is UIElemSellBase)
				return scrollList.GetItem(1).Data as UIElemSellBase;
			else
				return null;
		}
		else
		{
			if (scrollList.Count <= 0)
				return null;

			if (scrollList.GetItem(scrollList.Count - 1).Data != null)
			{
				Debug.Assert(scrollList.GetItem(scrollList.Count - 1).Data is UIElemSellBase);
				return scrollList.GetItem(scrollList.Count - 1).Data as UIElemSellBase;
			}

			if (scrollList.Count <= 1)
				return null;

			if (scrollList.GetItem(scrollList.Count - 2).Data is UIElemSellBase)
				return scrollList.GetItem(scrollList.Count - 2).Data as UIElemSellBase;
			else
				return null;
		}
	}

	private bool HasShowMoreButton(bool headNotTail)
	{
		if (scrollList.Count == 0)
			return false;

		if (headNotTail)
			return scrollList.GetItem(0).Data == null;
		else
			return scrollList.GetItem(scrollList.Count - 1).Data == null;
	}

	private void AddShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true))
				return;

			UIListItemContainer viewMoreContainer = topItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			scrollList.InsertItem(viewMoreContainer, 0, false, "", false);
		}
		else
		{
			if (HasShowMoreButton(false))
				return;

			UIListItemContainer viewMoreContainer = bottomItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			scrollList.InsertItem(viewMoreContainer, scrollList.Count, true, "", false);
		}
	}

	private void RemoveShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true) == false)
				return;

			scrollList.RemoveItem(0, false, false, false);
		}
		else
		{
			if (HasShowMoreButton(false) == false)
				return;

			scrollList.RemoveItem(scrollList.Count - 1, false, true, false);
		}
	}

	private void UpdateShowMoreButton(bool headNotTail)
	{
		int dataCount = GetDataToFillListCount();

		if (headNotTail)
		{
			if (dataCount == 0)
				// û��Ҫ���Ľ�ɫ��
				RemoveShowMoreButton(true);
			else
			{
				var item = GetFirstScrollListItem(true);
				Debug.Assert(item != null);
				if (item.indexInItemList > 0)
					AddShowMoreButton(true);
				else
					RemoveShowMoreButton(true);
			}
		}
		else
		{
			if (dataCount == 0)
				// û��Ҫ���Ľ�ɫ��
				RemoveShowMoreButton(false);
			else
			{
				var item = GetFirstScrollListItem(false);
				Debug.Assert(item != null);
				if (item.indexInItemList < dataCount - 1)
					AddShowMoreButton(false);
				else
					RemoveShowMoreButton(false);
			}
		}
	}

	private bool IsItemSelectForSell(UIElemSellBase.SellData sellData)
	{
		foreach (var data in selectSellDatas)
			if (data.Equals(sellData))
				return true;

		return false;
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(bool headNotTail)
	{
		yield return null;

		if (GetDataToFillListCount() <= 0)
		{
			switch (this.type)
			{
				case _UIType.UIPnlPackageAvatarTab:
					notCardLabel.Text = GameUtility.FormatUIString("UIPnlPackageSell_NothingToSell", GameUtility.GetUIString("UIPnlPackage_Avatar"));
					break;

				case _UIType.UIPnlPackageEquipTab:
					notCardLabel.Text = GameUtility.FormatUIString("UIPnlPackageSell_NothingToSell", GameUtility.GetUIString("UIPnlPackage_Equip"));
					break;

				case _UIType.UIPnlPackageSkillTab:
					notCardLabel.Text = GameUtility.FormatUIString("UIPnlPackageSell_NothingToSell", GameUtility.GetUIString("UIPnlPackage_Skill"));
					break;
			}

			yield break;
		}

		if (headNotTail)
		{
			var firstItem = GetFirstScrollListItem(true);

			int firstIndex = firstItem == null ? 0 : firstItem.indexInItemList - 1; ;

			int firstStartindex = firstIndex;
			for (; firstStartindex > firstIndex - maxRowsInPage && firstStartindex >= 0; firstStartindex--)
			{
				UIListItemContainer firstContainer = null;

				switch (this.type)
				{
					case _UIType.UIPnlPackageAvatarTab:
						firstContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
						break;
					case _UIType.UIPnlPackageEquipTab:
						firstContainer = equipObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
						break;
					case _UIType.UIPnlPackageSkillTab:
						firstContainer = skillObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
						break;
				}

				UIElemSellBase firstInsertItem = firstContainer.gameObject.GetComponent<UIElemSellBase>();
				firstInsertItem.indexInItemList = firstStartindex;

				switch (this.type)
				{
					case _UIType.UIPnlPackageAvatarTab:
						(firstInsertItem as UIElemSellAvatarItem).SetData(avatars[firstStartindex]);
						(firstInsertItem as UIElemSellAvatarItem).SetToggleState(IsItemSelectForSell(firstInsertItem.sellData));
						break;
					case _UIType.UIPnlPackageEquipTab:
						(firstInsertItem as UIElemSellEquipItem).SetData(equipments[firstStartindex]);
						(firstInsertItem as UIElemSellEquipItem).SetToggleState(IsItemSelectForSell(firstInsertItem.sellData));
						break;
					case _UIType.UIPnlPackageSkillTab:
						(firstInsertItem as UIElemSellSkillItem).SetData(skills[firstStartindex]);
						(firstInsertItem as UIElemSellSkillItem).SetToggleState(IsItemSelectForSell(firstInsertItem.sellData));
						break;
				}

				if (HasShowMoreButton(true) == false)
					scrollList.InsertItem(firstContainer, 0, false, "", false);
				else
					scrollList.InsertItem(firstContainer, 1, false, "", false);
			}

			UpdateShowMoreButton(true);
		}
		else
		{
			var lastItem = GetFirstScrollListItem(false);

			int lastIndex = lastItem == null ? 0 : lastItem.indexInItemList + 1;
			int lastStartindex = lastIndex;
			for (; lastStartindex < lastIndex + maxRowsInPage && lastStartindex < GetDataToFillListCount(); lastStartindex++)
			{
				UIListItemContainer lastContainer = null;

				switch (this.type)
				{
					case _UIType.UIPnlPackageAvatarTab:
						lastContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
						break;
					case _UIType.UIPnlPackageEquipTab:
						lastContainer = equipObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
						break;
					case _UIType.UIPnlPackageSkillTab:
						lastContainer = skillObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
						break;
				}

				UIElemSellBase lastInsertItem = lastContainer.gameObject.GetComponent<UIElemSellBase>();
				lastInsertItem.indexInItemList = lastStartindex;

				switch (this.type)
				{
					case _UIType.UIPnlPackageAvatarTab:
						(lastInsertItem as UIElemSellAvatarItem).SetData(avatars[lastStartindex]);
						(lastInsertItem as UIElemSellAvatarItem).SetToggleState(IsItemSelectForSell(lastInsertItem.sellData));
						break;
					case _UIType.UIPnlPackageEquipTab:
						(lastInsertItem as UIElemSellEquipItem).SetData(equipments[lastStartindex]);
						(lastInsertItem as UIElemSellEquipItem).SetToggleState(IsItemSelectForSell(lastInsertItem.sellData));
						break;
					case _UIType.UIPnlPackageSkillTab:
						(lastInsertItem as UIElemSellSkillItem).SetData(skills[lastStartindex]);
						(lastInsertItem as UIElemSellSkillItem).SetToggleState(IsItemSelectForSell(lastInsertItem.sellData));
						break;
				}

				// If "view more" button is in the list, insert before it
				if (HasShowMoreButton(false) == false)
					scrollList.InsertItem(lastContainer, scrollList.Count, true, "", false);
				else
					scrollList.InsertItem(lastContainer, scrollList.Count - 1, true, "", false);
			}

			UpdateShowMoreButton(false);
		}

		yield return null;

		int avatarItemCount = GetScrollListItemCount();
		if (avatarItemCount > maxRows)
		{
			bool hasHeadShowMore = HasShowMoreButton(true);

			if (headNotTail)
			{
				for (int headIndex = 0; headIndex < avatarItemCount - maxRows; ++headIndex)
				{
					int headRemoveIndex = hasHeadShowMore ? maxRows + 1 : maxRows;
					scrollList.RemoveItem(headRemoveIndex, false, true, false);
				}

				UpdateShowMoreButton(false);
			}
			else
			{
				for (int tailIndex = avatarItemCount - maxRows - 1; tailIndex >= 0; --tailIndex)
				{
					int tailRemoveIndex = hasHeadShowMore ? tailIndex + 1 : tailIndex;
					scrollList.RemoveItem(tailRemoveIndex, false, false, false);
				}

				UpdateShowMoreButton(true);
			}
		}

		if (scrollList.Count > 0)
			notCardLabel.Text = "";
	}

	// Click For Show MoreItem in ScrollList.
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

	// Show the AvatarInfo or EquipInfo or SkillInfo.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickItemIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		GameUtility.ShowAssetInfoUI((int)assetIcon.Data);
	}

	public void SeletScrollItemByObj(object obj)
	{
		for (int index = 0; index < scrollList.Count; index++)
		{
			if (scrollList.GetItem(index).Data != null)
			{
				int resourceId = IDSeg.InvalidId;
				string guid = "";

				if (obj is KodGames.ClientClass.Avatar)
				{
					resourceId = (obj as KodGames.ClientClass.Avatar).ResourceId;
					guid = (obj as KodGames.ClientClass.Avatar).Guid;
				}
				else if (obj is KodGames.ClientClass.Equipment)
				{
					resourceId = (obj as KodGames.ClientClass.Equipment).ResourceId;
					guid = (obj as KodGames.ClientClass.Equipment).Guid;
				}
				else if (obj is KodGames.ClientClass.Skill)
				{
					resourceId = (obj as KodGames.ClientClass.Skill).ResourceId;
					guid = (obj as KodGames.ClientClass.Skill).Guid;
				}

				UIElemSellBase sellItem = scrollList.GetItem(index).Data as UIElemSellBase;

				if (sellItem.sellData.sellGUID.Equals(guid) && sellItem.sellData.sellResourceID == resourceId && !IsItemSelectForSell(sellItem.sellData))
				{
					sellItem.SetToggleState(true);
					selectSellDatas.Add(sellItem.sellData);
				}
			}
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelectBtn(UIButton btn)
	{
		UIElemSellBase sellItem = btn.data as UIElemSellBase;
		sellItem.SetToggleState(!sellItem.IsItemSelected());

		// Remove or Add item Guid.
		if (sellItem.IsItemSelected())
		{
			if (IsItemSelectForSell(sellItem.sellData) == false)
				selectSellDatas.Add(sellItem.sellData);
		}
		else
		{
			if (IsItemSelectForSell(sellItem.sellData))
				selectSellDatas.Remove(sellItem.sellData);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterButton(UIButton btn)
	{
		if (this.type == _UIType.UIPnlPackageAvatarTab)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageAvatarFilter), PackageFilterData._DataType.PackageAvatar, new UIDlgPackageAvatarFilter.OnSelectFilterAvatar(InitView));
		else if (this.type == _UIType.UIPnlPackageEquipTab)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageEquipFilter), PackageFilterData._DataType.PackageEquip, new UIDlgPackageEquipFilter.OnSelectFilterEquip(InitView));
		else if (this.type == _UIType.UIPnlPackageSkillTab)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageSkillFilter), PackageFilterData._DataType.PackageSkill, new UIDlgPackageSkillFilter.OnSelectFilterSkill(InitView));
	}

	private bool IsAllSelected()
	{
		switch (type)
		{
			case _UIType.UIPnlPackageAvatarTab:
				return selectSellDatas.Count == avatars.Count;
			case _UIType.UIPnlPackageEquipTab:
				return selectSellDatas.Count == equipments.Count;
			case _UIType.UIPnlPackageSkillTab:
				return selectSellDatas.Count == skills.Count;
		}

		return false;
	}

	#region OnClickSelectAll
	//[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelectAll(bool a, object data)//UIButton btn)
	{
		if (scrollList.Count <= 0)
			return;

		bool selectAll = selectAllButton.IsSelected;

		//将为选中的item加入到selectSellDatas中
		for (int index = 0; index < scrollList.Count; index++)
		{
			if (scrollList.GetItem(index).Data == null)
				continue;

			UIElemSellBase sellItem = scrollList.GetItem(index).Data as UIElemSellBase;

			if (IsItemSelectForSell(sellItem.sellData) == false)
				selectSellDatas.Add(sellItem.sellData);
		}

		if (!selectAll)
			selectSellDatas.Clear();

		//全部置成选中状态
		for (int index = 0; index < scrollList.Count; index++)
		{
			UIListItemContainer container = scrollList.GetItem(index) as UIListItemContainer;
			if (container == null)
				continue;

			UIElemSellBase sellItem = container.data as UIElemSellBase;
			if (sellItem == null)
				continue;

			sellItem.SetToggleState(selectAll);
		}

		//totalGoldBtn.border.Text = GetTotalGoldNumbers()[0].ToString();
		//totalIgnoredBtn.border.Text = GetTotalGoldNumbers()[1].ToString();
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCancel(UIButton btn)
	{
		HideCurrentPanel();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSellBtn(UIButton btn)
	{
		// If no card Selected , hide this panel.
		if (selectSellDatas.Count <= 0)
		{
			switch (this.type)
			{
				case _UIType.UIPnlPackageAvatarTab:
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlTipFlow_Tip_NoAvatarSelectedToShow"));
					break;
				case _UIType.UIPnlPackageEquipTab:
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlTipFlow_Tip_NoEquipSelectedToShow"));
					break;
				case _UIType.UIPnlPackageSkillTab:
					SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlTipFlow_Tip_NoSkillSelectedToShow"));
					break;
			}
			return;
		}

		ShowConfirmDlg();
	}

	//private int GetTotalGoldNumber()
	//{

	//    int totalGold = 0;

	//    foreach (var data in selectSellDatas)
	//    {
	//        switch (this.type)
	//        {
	//            case _UIType.UIPnlPackageAvatarTab:
	//                {
	//                    AvatarScroll avatarScroll = new AvatarScroll();
	//                    avatarScroll.ResourceId = data.sellResourceID;
	//                    avatarScroll.Amount = data.sellCount;

	//                    avatarScroll.BreakthoughtLevel = 0;
	//                    avatarScroll.LevelAttrib.Level = 0;

	//                    totalGold += MathFactory.ExpressionCalculate.GetValue_AvatarSellPrice(avatarScroll);
	//                }
	//                break;

	//            case _UIType.UIPnlPackageEquipTab:
	//                {
	//                    EquipScroll equipScroll = new EquipScroll();
	//                    equipScroll.ResourceId = data.sellResourceID;
	//                    equipScroll.Amount = data.sellCount;

	//                    equipScroll.BreakthoughtLevel = 0;
	//                    equipScroll.LevelAttrib.Level = 0;

	//                    totalGold += MathFactory.ExpressionCalculate.GetValue_EquipScrollSellItemPrice(equipScroll);
	//                }

	//                break;

	//            case _UIType.UIPnlPackageSkillTab:
	//                KodGames.ClientClass.Skill skill = SysLocalDataBase.Inst.LocalPlayer.SearchSkill(data.sellGUID);
	//            //	totalGold += ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(skill.ResourceId).GetUpgrateSettingByLevel(skill.LevelAttrib.Level).sellRewards;
	//                totalGold += MathFactory.ExpressionCalculate.GetValue_SkillSellPrice(skill);

	//                break;
	//        }
	//    }

	//    return totalGold;
	//}

	// Hide the UIPnlPackageSell(this Panel).
	private void HideCurrentPanel()
	{
		switch (this.type)
		{
			case _UIType.UIPnlPackageAvatarTab:
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlPackageAvatarTab));
				break;
			case _UIType.UIPnlPackageEquipTab:
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlPackageEquipTab));
				break;
			case _UIType.UIPnlPackageSkillTab:
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlPackageSkillTab));
				break;
		}
	}

	public bool OnSell(object obj)
	{
		List<KodGames.ClientClass.Cost> costs = new List<KodGames.ClientClass.Cost>();

		foreach (var deleteData in selectSellDatas)
		{
			KodGames.ClientClass.Cost cost = new KodGames.ClientClass.Cost();
			cost.Id = deleteData.sellResourceID;
			cost.Guid = deleteData.sellGUID;
			cost.Count = deleteData.sellCount;

			costs.Add(cost);
		}

		RequestMgr.Inst.Request(new SellItemReq(costs));
		return true;
	}

	public void OnSellSuccess(KodGames.ClientClass.Reward reward)
	{
		ShowSellSuccessMessage(reward);
	}

	public static List<Reward> GetAvatarSellRewards(KodGames.ClientClass.Avatar avatar)
	{
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);
		List<Reward> sellRewards = new List<Reward>();
		List<Reward> preProcessed = new List<Reward>();

		if (avatarCfg == null)
		{
			Debug.LogError("Can't find avatarConfiguration by ResourceId=" + avatar.ResourceId.ToString("X"));
			return sellRewards;
		}

		AvatarConfig.AvatarBreakthrough avatarBreakthrough = avatarCfg.GetAvatarBreakthrough(avatar.BreakthoughtLevel);
		if (avatarBreakthrough == null)
		{
			Debug.LogError(string.Format("Avatar Breakthrough level dosen't exist. resourceId={0} breakthroughLevel={1}", avatar.ResourceId, avatar.BreakthoughtLevel));
			return sellRewards;
		}

		preProcessed.AddRange(avatarBreakthrough.sellRewards);

		//如果canGetSellItemGeneralRewards为True，突破（精炼）后的价格需按公式：按强化等级查表贩卖价格×（1+当前突破次数）计算。
		if (avatarBreakthrough.canGetSellItemGeneralRewards)
		{
			List<Reward> generalRewards = ConfigDatabase.DefaultCfg.AvatarConfig.GetSellGeneralRewardsByLevel(avatar.LevelAttrib.Level, avatarCfg.qualityLevel);
			Reward sellRewardWithPrice;
			foreach (var generalReward in generalRewards)
			{
				sellRewardWithPrice = new Reward();
				sellRewardWithPrice.id = generalReward.id;
				//贩卖价格×（1+当前突破次数）
				sellRewardWithPrice.count = generalReward.count * (avatar.BreakthoughtLevel + 1);
				preProcessed.Add(sellRewardWithPrice);
			}
		}

		MergerReward(preProcessed, ref sellRewards);

		return sellRewards;
	}

	public static List<Reward> GetEquipmentSellRewards(KodGames.ClientClass.Equipment equipment)
	{
		EquipmentConfig.Equipment equipCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId);
		List<Reward> sellRewards = new List<Reward>();
		List<Reward> preProcessed = new List<Reward>();

		if (equipCfg == null)
		{
			Debug.LogError("Can't find equipmentconfig by ResourceId=" + equipment.ResourceId.ToString("X"));
			return sellRewards;
		}

		EquipmentConfig.EquipBreakthrough equipBreakthrough = equipCfg.GetBreakthroughByTimes(equipment.BreakthoughtLevel);

		if (equipBreakthrough == null)
		{
			Debug.LogError(string.Format("Equipment Breakthrough level dosen't exist. resourceId={0} breakthroughLevel={1}", equipment.ResourceId, equipment.BreakthoughtLevel));
			return sellRewards;
		}

		preProcessed.AddRange(equipBreakthrough.sellRewards);

		//如果canGetSellItemGeneralRewards为True，突破（精炼）后的价格需按公式：按强化等级查表贩卖价格×（1+当前突破次数）计算。
		if (equipBreakthrough.canGetSellItemGeneralRewards)
		{
			List<Reward> generalRewards = ConfigDatabase.DefaultCfg.EquipmentConfig.GetSellGeneralRewardsByLevel(equipment.LevelAttrib.Level, equipCfg.qualityLevel);
			Reward sellRewardWithPrice;
			foreach (var generalReward in generalRewards)
			{
				sellRewardWithPrice = new Reward();
				sellRewardWithPrice.id = generalReward.id;
				//贩卖价格×（1+当前突破次数）
				sellRewardWithPrice.count = generalReward.count * (equipment.BreakthoughtLevel + 1);
				preProcessed.Add(sellRewardWithPrice);
			}
		}

		MergerReward(preProcessed, ref sellRewards);

		return sellRewards;
	}

	private static void MergerReward(List<Reward> rewards, ref List<Reward> toReards)
	{
		if (rewards == null)
			return;

		for (int i = 0; i < rewards.Count; i++)
		{
			if (rewards[i].count <= 0)
				continue;

			bool find = false;
			for (int j = 0; j < toReards.Count; j++)
			{
				if (toReards[j].id == rewards[i].id)
				{
					find = true;
					toReards[j].count += rewards[i].count;
					break;
				}
			}

			if (!find && rewards[i].count > 0)
			{
				var tempReawrd = new Reward();
				tempReawrd.id = rewards[i].id;
				tempReawrd.count = rewards[i].count;
				toReards.Add(tempReawrd);
			}
		}
	}

	private void ShowConfirmDlg()
	{
		var selectQualtiy = new Dictionary<int, int>();
		var sellRewards = new List<Reward>();

		selectSellDatas.Sort((a1, a2) =>
			{
				return ItemInfoUtility.GetAssetQualityLevel(a1.sellResourceID) - ItemInfoUtility.GetAssetQualityLevel(a2.sellResourceID);
			});

		for (int i = 0; i < selectSellDatas.Count; i++)
		{
			int qualityLevel = ItemInfoUtility.GetAssetQualityLevel(selectSellDatas[i].sellResourceID);

			if (selectQualtiy.ContainsKey(qualityLevel))
				selectQualtiy[qualityLevel]++;
			else
				selectQualtiy.Add(qualityLevel, 1);

			List<Reward> rewards = null;

			switch (this.type)
			{
				case _UIType.UIPnlPackageAvatarTab:
					rewards = GetAvatarSellRewards(SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(selectSellDatas[i].sellGUID));
					break;

				case _UIType.UIPnlPackageEquipTab:
					rewards = GetEquipmentSellRewards(SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(selectSellDatas[i].sellGUID));
					break;

				case _UIType.UIPnlPackageSkillTab:
					rewards = ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(selectSellDatas[i].sellResourceID).GetSellRewards(selectSellDatas[i].breakhroughOrUpgradeLevel);
					break;
			}

			MergerReward(rewards, ref sellRewards);
		}

		//Show confirm dlg
		List<string> selectItem = new List<string>();
		List<string> getBySell = new List<string>();

		foreach (var qualityRecord in selectQualtiy)
			selectItem.Add(GameUtility.FormatUIString("UIPnlPackageSell_SelectCount_Confirm_Color", ItemInfoUtility.GetAssetQualityLevelCNDesc(qualityRecord.Key), IDSeg._AssetType.GetDisplayNameByType(IDSeg.ToAssetType(selectSellDatas[0].sellResourceID), ConfigDatabase.DefaultCfg), qualityRecord.Value));

		foreach (var reward in sellRewards)
			getBySell.Add(reward.count.ToString() + ItemInfoUtility.GetAssetName(reward.id));

		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgPackageSellSceondSure, selectItem, getBySell);
	}

	// If sell success ,show the result message
	private void ShowSellSuccessMessage(KodGames.ClientClass.Reward reward)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), SysLocalDataBase.GetRewardDesc(reward, true));

		RefreshList();
	}

	private void RefreshList()
	{
		UIElemSellBase startItem = GetFirstScrollListItem(true);
		UIElemSellBase endItem = GetFirstScrollListItem(false);

		if (startItem == null)
			return;

		// Remove Item From scrollToListItems.
		switch (type)
		{
			case _UIType.UIPnlPackageAvatarTab:

				for (int index = avatars.Count - 1; index >= 0; index--)
				{
					foreach (var sellItem in selectSellDatas)
						if (avatars[index].Guid.Equals(sellItem.sellGUID) && avatars[index].ResourceId == sellItem.sellResourceID)
						{
							avatars.RemoveAt(index);
							break;
						}
				}

				break;
			case _UIType.UIPnlPackageEquipTab:

				for (int index = equipments.Count - 1; index >= 0; index--)
				{
					foreach (var sellItem in selectSellDatas)
						if (equipments[index].Guid.Equals(sellItem.sellGUID) && equipments[index].ResourceId == sellItem.sellResourceID)
						{
							equipments.RemoveAt(index);
							break;
						}
				}

				break;
			case _UIType.UIPnlPackageSkillTab:

				for (int index = skills.Count - 1; index >= 0; index--)
				{
					foreach (var sellItem in selectSellDatas)
						if (skills[index].Guid.Equals(sellItem.sellGUID) && skills[index].ResourceId == sellItem.sellResourceID)
						{
							skills.RemoveAt(index);
							break;
						}
				}
				break;
		}

		// Delete item which has used for sell.
		List<UIListItemContainer> deleteContainers = new List<UIListItemContainer>();
		for (int index = 0; index < scrollList.Count; index++)
		{
			if (scrollList.GetItem(index).Data == null)
				continue;

			UIElemSellBase sellItem = scrollList.GetItem(index).Data as UIElemSellBase;

			if (sellItem == null)
				continue;

			foreach (var selectItem in selectSellDatas)
				if (selectItem.sellGUID.Equals(sellItem.sellData.sellGUID) && selectItem.sellResourceID == sellItem.sellData.sellResourceID)
				{
					deleteContainers.Add(sellItem.container);
					break;
				}
		}

		foreach (var container in deleteContainers)
			scrollList.RemoveItem(container, false);

		// Clear Data.
		selectSellDatas.Clear();

		// Refresh the scroll's first and end item 's indexInItemList if scrollList has valid item.
		startItem = GetFirstScrollListItem(true);
		endItem = GetFirstScrollListItem(false);
		if (GetScrollListItemCount() > 0)
		{
			switch (type)
			{
				case _UIType.UIPnlPackageAvatarTab:

					for (int index = 0; index < avatars.Count; index++)
					{
						if (avatars[index].Guid.Equals(startItem.sellData.sellGUID) && avatars[index].ResourceId == startItem.sellData.sellResourceID)
							startItem.indexInItemList = index;

						if (avatars[index].Guid.Equals(endItem.sellData.sellGUID) && avatars[index].ResourceId == endItem.sellData.sellResourceID)
							endItem.indexInItemList = index;
					}
					break;
				case _UIType.UIPnlPackageEquipTab:

					for (int index = 0; index < equipments.Count; index++)
					{
						if (equipments[index].Guid.Equals(startItem.sellData.sellGUID) && equipments[index].ResourceId == startItem.sellData.sellResourceID)
							startItem.indexInItemList = index;

						if (equipments[index].Guid.Equals(endItem.sellData.sellGUID) && equipments[index].ResourceId == endItem.sellData.sellResourceID)
							endItem.indexInItemList = index;
					}
					break;
				case _UIType.UIPnlPackageSkillTab:
					for (int index = 0; index < skills.Count; index++)
					{
						if (skills[index].Guid.Equals(startItem.sellData.sellGUID) && skills[index].ResourceId == startItem.sellData.sellResourceID)
							startItem.indexInItemList = index;

						if (skills[index].Guid.Equals(endItem.sellData.sellGUID) && skills[index].ResourceId == endItem.sellData.sellResourceID)
							endItem.indexInItemList = index;
					}
					break;
			}
		}
		else // Refresh all ScrollList item.
		{
			ClearData();
			InitData();
		}
	}



	/// <summary>
	/// 验证是否全选
	/// </summary>
	/// <returns></returns>
	private bool CheckAllSelected(int type)
	{
		switch (type)
		{
			case _UIType.UIPnlPackageAvatarTab:
				return ItemInfoUtility.CheckAvatarAllSelected(PackageFilterData._DataType.PackageAvatar);
			case _UIType.UIPnlPackageEquipTab:
				return ItemInfoUtility.CheckEquipAllSelected(PackageFilterData._DataType.PackageEquip);
			case _UIType.UIPnlPackageSkillTab:
				return ItemInfoUtility.CheckSkillAllSelected(PackageFilterData._DataType.PackageSkill);
		}
		return true;
	}

}
