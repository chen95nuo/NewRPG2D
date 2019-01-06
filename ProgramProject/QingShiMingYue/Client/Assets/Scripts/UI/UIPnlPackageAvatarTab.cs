using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlPackageAvatarTab : UIPnlPackageBase
{
	public UIScrollList avatarList;
	public GameObjectPool avatarObjectPool;
	public GameObjectPool getObjectPool;
	public GameObjectPool topItemPool;
	public GameObjectPool bottomItemPool;
	public SpriteText emptyTip;
	public SpriteText selectedText;

	private List<KodGames.ClientClass.Avatar> avatarToFillList = new List<KodGames.ClientClass.Avatar>();

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

		SysUIEnv.Instance.GetUIModule<UIPnlPackageTab>().ChangeTabButtons(_UIType.UIPnlPackageAvatarTab);

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		ClearData();
		base.OnHide();
	}

	private void ClearData()
	{
		StopCoroutine("FillList");
		avatarList.ClearList(false);
		avatarList.ScrollPosition = 0f;
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

		avatarToFillList.Clear();

		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.PackageAvatar);
		var avatarTraitFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType);
		var avatarQualityFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);
		var avatarCountryFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType);

		foreach (var avatar in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (!avatar.IsAvatar)
				continue;

			AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

			if (!avatarTraitFilter.Contains(avatarConfig.traitType))
				continue;

			if (!avatarQualityFilter.Contains(avatarConfig.qualityLevel))
				continue;

			if (!avatarCountryFilter.Contains(avatarConfig.countryType))
				continue;

			avatarToFillList.Add(avatar);
		}

		avatarToFillList.Sort(DataCompare.CompareAvatar);
		// 设置空信息提示
		if (avatarToFillList.Count > 0)
		{
			emptyTip.Text = string.Empty;
		}
		else
		{
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Avatar");
		}

		if (ItemInfoUtility.CheckAvatarAllSelected(PackageFilterData._DataType.PackageAvatar))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");
	}

	#region  MoreButton Controll.
	private int GetAvatarListItemCount()
	{
		if (avatarList.Count <= 0)
			return 0;

		int count = avatarList.Count;
		if (avatarList.GetItem(0).Data == null)
			count--;

		if (avatarList.Count <= 1)
			return count;

		if (avatarList.GetItem(avatarList.Count - 1).Data == null)
			count--;

		return count;
	}

	private UIElemPackageItemBase GetFirstAvatarListItem(bool headNotTail)
	{
		if (headNotTail)
		{
			if (avatarList.Count <= 0)
				return null;

			if (avatarList.GetItem(0).Data != null)
			{
				return avatarList.GetItem(0).Data as UIElemPackageItemBase;
			}

			if (avatarList.Count <= 1)
				return null;

			if (avatarList.GetItem(1).Data is UIElemPackageItemBase)
				return avatarList.GetItem(1).Data as UIElemPackageItemBase;
			else
				return null;
		}
		else
		{
			if (avatarList.Count <= 0)
				return null;

			if (avatarList.GetItem(avatarList.Count - 1).Data != null)
			{
				return avatarList.GetItem(avatarList.Count - 1).Data as UIElemPackageItemBase;
			}

			if (avatarList.Count <= 1)
				return null;

			if (avatarList.GetItem(avatarList.Count - 2).Data is UIElemPackageItemBase)
				return avatarList.GetItem(avatarList.Count - 2).Data as UIElemPackageItemBase;
			else
				return null;
		}
	}

	private bool HasShowMoreButton(bool headNotTail)
	{
		if (avatarList.Count == 0)
			return false;

		if (headNotTail)
			return avatarList.GetItem(0).Data == null;
		else
			return avatarList.GetItem(avatarList.Count - 1).Data == null;
	}

	private void AddShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true))
				return;

			UIListItemContainer viewMoreContainer = topItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			avatarList.InsertItem(viewMoreContainer, 0, false, "", false);
		}
		else
		{
			if (HasShowMoreButton(false))
				avatarList.RemoveItem(avatarList.Count - 1, false, true, false);

			UIListItemContainer viewMoreContainer = bottomItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			avatarList.InsertItem(viewMoreContainer, avatarList.Count, true, "", false);
		}
	}

	private void RemoveShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (HasShowMoreButton(true) == false)
				return;

			avatarList.RemoveItem(0, false, false, false);
		}
		else
		{
			UIListItemContainer getContainer = getObjectPool.AllocateItem().GetComponent<UIListItemContainer>();

			if (HasShowMoreButton(false) == false)
			{
				avatarList.InsertItem(getContainer, avatarList.Count, true, "", false);
				return;
			}

			avatarList.RemoveItem(avatarList.Count - 1, false, true, false);
			avatarList.InsertItem(getContainer, avatarList.Count, true, "", false);
		}
	}

	private void UpdateShowMoreButton(bool headNotTail)
	{
		if (headNotTail)
		{
			if (avatarToFillList.Count == 0)
				RemoveShowMoreButton(true);
			else
			{
				var item = GetFirstAvatarListItem(true);
				if (item.indexInList > 0)
					AddShowMoreButton(true);
				else
					RemoveShowMoreButton(true);
			}
		}
		else
		{
			if (avatarToFillList.Count == 0)
				RemoveShowMoreButton(false);
			else
			{
				var item = GetFirstAvatarListItem(false);
				if (item.indexInList < avatarToFillList.Count - 1)
					AddShowMoreButton(false);
				else
					RemoveShowMoreButton(false);
			}
		}
	}
	#endregion

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillList(bool headNotTail)
	{
		yield return null;

		if (headNotTail)
		{
			var firstItem = GetFirstAvatarListItem(true);

			int firstIndex = firstItem == null ? 0 : firstItem.indexInList - 1;

			int firstStartindex = firstIndex;
			for (; firstStartindex > firstIndex - maxRowsInPage && firstStartindex >= 0; firstStartindex--)
			{
				UIListItemContainer firstContainer = null;

				firstContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemPackageAvatarItem item = firstContainer.GetComponent<UIElemPackageAvatarItem>();
				item.SetData(avatarToFillList[firstStartindex]);
				item.indexInList = firstStartindex;

				if (HasShowMoreButton(true) == false)
					avatarList.InsertItem(firstContainer, 0, false, "", false);
				else
					avatarList.InsertItem(firstContainer, 1, false, "", false);
			}

			UpdateShowMoreButton(true);
		}
		else
		{
			var lastItem = GetFirstAvatarListItem(false);

			int lastIndex = lastItem == null ? 0 : lastItem.indexInList + 1;
			int lastStartindex = lastIndex;
			for (; lastStartindex < lastIndex + maxRowsInPage && lastStartindex < avatarToFillList.Count; lastStartindex++)
			{
				UIListItemContainer endContainer = null;

				endContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemPackageAvatarItem item = endContainer.GetComponent<UIElemPackageAvatarItem>();
				item.SetData(avatarToFillList[lastStartindex]);
				item.indexInList = lastStartindex;

				// If "view more" button is in the list, insert before it
				if (HasShowMoreButton(false) == false)
					avatarList.InsertItem(endContainer, avatarList.Count, true, "", false);
				else
					avatarList.InsertItem(endContainer, avatarList.Count - 1, true, "", false);
			}

			UpdateShowMoreButton(false);
		}

		int avatarItemCount = GetAvatarListItemCount();
		if (avatarItemCount > maxRows)
		{
			bool hasHeadShowMore = HasShowMoreButton(true);

			if (headNotTail)
			{
				for (int headIndex = 0; headIndex < avatarItemCount - maxRows; ++headIndex)
				{
					int headRemoveIndex = hasHeadShowMore ? maxRows + 1 : maxRows;
					avatarList.RemoveItem(headRemoveIndex, false, true, false);
				}

				UpdateShowMoreButton(false);
			}
			else
			{
				for (int tailIndex = avatarItemCount - maxRows - 1; tailIndex >= 0; --tailIndex)
				{
					int tailRemoveIndex = hasHeadShowMore ? tailIndex + 1 : tailIndex;
					avatarList.RemoveItem(tailRemoveIndex, false, false, false);
				}

				UpdateShowMoreButton(true);
			}
		}
	}

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
	private void OnClickAvatarIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		KodGames.ClientClass.Avatar avatar = assetIcon.Data as KodGames.ClientClass.Avatar;
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIDlgAvatarInfo, avatar, false, true, false, true, null);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickEvelate(UIButton btn)
	{
		KodGames.ClientClass.Avatar avatar = btn.data as KodGames.ClientClass.Avatar;
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlAvatarLevelUp, avatar);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClick_ShowAvatarDetail(UIButton btn)
	{
		ItemInfoUtility.ShowLineUpAvatarDesc(btn.Data as KodGames.ClientClass.Avatar);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSell(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(ClientServerCommon._UIType.UIPnlPackageSell, _UIType.UIPnlPackageAvatarTab);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterAvatar(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageAvatarFilter), PackageFilterData._DataType.PackageAvatar, new UIDlgPackageAvatarFilter.OnSelectFilterAvatar(InitUI));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetAvatar(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlShopWine));
	}

	public void RefreshView(KodGames.ClientClass.Avatar refreshAvatar)
	{
		// 刷新界面数据
		int refreshAvatarIndex = 0;
		var deleteContainers = new List<UIListItemContainer>();

		for (int index = 0; index < avatarList.Count; index++)
		{
			object itemData = avatarList.GetItem(index).Data;
			if (itemData == null)
				continue;

			if (itemData is UIElemPackageAvatarItem)
			{
				var avatarItem = itemData as UIElemPackageAvatarItem;

				if (SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatarItem.Avatar.Guid) == null)
					deleteContainers.Add(avatarItem.container);
				else if (refreshAvatar != null && avatarItem.Avatar.Guid.Equals(refreshAvatar.Guid))
				{
					refreshAvatarIndex = avatarItem.indexInList;
					avatarItem.SetData(refreshAvatar);
				}
			}
		}

		// Delete Not Exist Container.
		foreach (var container in deleteContainers)
		{
			UIElemPackageItemBase item = container.Data as UIElemPackageItemBase;

			if (item.indexInList < refreshAvatarIndex)
				avatarList.RemoveItem(item.container, false, false, false);
			else
				avatarList.RemoveItem(item.container, false, true, false);

		}

		// Refresh LocalData.
		for (int index = avatarToFillList.Count - 1; index >= 0; index--)
		{
			var avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatarToFillList[index].Guid);

			if (avatar == null)
				avatarToFillList.RemoveAt(index);
			else if (refreshAvatar != null && avatar.Guid == refreshAvatar.Guid)
				avatarToFillList[index] = avatar;
		}

		// Reset the first and end item position.
		for (int index = 0; index < avatarToFillList.Count; index++)
		{
			for (int subIndex = 0; subIndex < avatarList.Count; subIndex++)
			{
				var itemData = avatarList.GetItem(subIndex).Data;
				if (itemData == null)
					continue;

				if (itemData is UIElemPackageAvatarItem && (itemData as UIElemPackageAvatarItem).Avatar.Guid.Equals(avatarToFillList[index].Guid))
					(itemData as UIElemPackageAvatarItem).indexInList = index;
			}
		}
	}


}