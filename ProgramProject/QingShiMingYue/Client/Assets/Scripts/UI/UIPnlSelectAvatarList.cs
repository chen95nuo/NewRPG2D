using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlSelectAvatarList : UIPnlItemInfoBase
{
	// Avatar list control.
	public UIScrollList avatarList;
	public GameObjectPool avatarObjectPool;
	public GameObjectPool topItemPool;
	public GameObjectPool bottomItemPool;
	public GameObjectPool getObjectPool;
	public UIButton tabBtn;
	public UIButton gotoBtn;
	public SpriteText emptyTip;
	public SpriteText selectedText;

	private KodGames.ClientClass.Location avatarLocation;
	private KodGames.ClientClass.Partner avatarPartner;
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

		if (userDatas[0] is KodGames.ClientClass.Location)
			this.avatarLocation = userDatas[0] as KodGames.ClientClass.Location;
		else if (userDatas[0] is KodGames.ClientClass.Partner)
			this.avatarPartner = userDatas[0] as KodGames.ClientClass.Partner;

		InitView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearList();
	}

	private void ClearList()
	{
		// Clear list and stop the coroutine.
		StopCoroutine("FillList");
		avatarList.ClearList(false);
		avatarList.ScrollListTo(0);

		// Clear Data.
		avatarToFillList.Clear();
		avatarLocation = null;
		avatarPartner = null;


		emptyTip.Text = "";
	}

	private void InitView()
	{
		// Clear Data For Filter Data.
		StopCoroutine("FillList");
		avatarList.ClearList(false);
		avatarList.ScrollListTo(0);
		avatarToFillList.Clear();

		int positionId = avatarLocation == null ? avatarPartner.PositionId : avatarLocation.PositionId;
		var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(positionId);

		bool isDinerSelect = avatarLocation != null && avatarLocation.ShowLocationId == position.EmployShowLocationId;

		// Filter By FilterFunction.
		var avatarFilter = PackageFilterData.Instance.GetPackgetFilterByType(PackageFilterData._DataType.SelectAvatar);
		var avatarTraitFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType);
		var avatarQualityFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);
		var avatarCountryFilter = avatarFilter.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType);

		// Filter Data List.
		List<int> filterIds = new List<int>();
		List<string> filterGuids = new List<string>();
		if (!isDinerSelect)
		{
			for (int i = 0; i < position.AvatarLocations.Count; i++)
			{
				// 门客不进行阵位上阵筛选
				if (position.AvatarLocations[i].ShowLocationId == position.EmployShowLocationId)
					continue;

				if (!filterIds.Contains(position.AvatarLocations[i].ResourceId))
					filterIds.Add(position.AvatarLocations[i].ResourceId);
			}

			for (int i = 0; i < position.Partners.Count; i++)
			{
				if (!filterIds.Contains(position.Partners[i].ResourceId))
					filterIds.Add(position.Partners[i].ResourceId);
			}
		}
		else
		{
			for (int i = 0; i < position.AvatarLocations.Count; i++)
			{
				if (position.AvatarLocations[i].ShowLocationId == position.EmployShowLocationId)
				{
					filterGuids.Add(position.AvatarLocations[i].Guid);
					break;
				}
			}
		}

		// Filter Data.
		for (int i = 0; i < SysLocalDataBase.Inst.LocalPlayer.Avatars.Count; i++)
		{
			if (filterIds.Contains(SysLocalDataBase.Inst.LocalPlayer.Avatars[i].ResourceId))
				continue;

			if (filterGuids.Contains(SysLocalDataBase.Inst.LocalPlayer.Avatars[i].Guid))
				continue;

			// 如果是雇佣，剔除不是雇佣npc的avatar
			// 如果不是雇佣，剔除雇佣avatar
			if (isDinerSelect == SysLocalDataBase.Inst.LocalPlayer.Avatars[i].IsAvatar)
				continue;

			AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(SysLocalDataBase.Inst.LocalPlayer.Avatars[i].ResourceId);

			if (!avatarTraitFilter.Contains(avatarConfig.traitType))
				continue;

			if (!avatarQualityFilter.Contains(avatarConfig.qualityLevel))
				continue;

			if (!avatarCountryFilter.Contains(avatarConfig.countryType))
				continue;

			avatarToFillList.Add(SysLocalDataBase.Inst.LocalPlayer.Avatars[i]);
		}

		avatarToFillList.Sort(DataCompare.CompareAvatar);

		//设置标签
		tabBtn.Text = isDinerSelect ? GameUtility.GetUIString("UIPnlSelectAvatarList_Tab_AvatarDiner") : GameUtility.GetUIString("UIPnlSelectAvatarList_Tab_Avatar");

		// 设置跳转按钮
		gotoBtn.Data = isDinerSelect;
		if (avatarToFillList.Count <= 0)
		{
			gotoBtn.Hide(false);
			gotoBtn.Text = GameUtility.GetUIString(isDinerSelect ? "UIPnlSelectAvatarList_GoToAvatarDiner" : "UIPnlSelectAvatarList_GoToAvatarShopWine");
		}
		else
		{
			gotoBtn.Hide(true);
		}

		// 设置空信息提示
		if (avatarToFillList.Count > 0)
		{
			StartCoroutine("FillList", false);
			emptyTip.Text = string.Empty;
		}
		else
			emptyTip.Text = isDinerSelect ? GameUtility.GetUIString("UIEmptyList_AvatarNpc") : GameUtility.GetUIString("UIEmptyList_Avatar");

		if (ItemInfoUtility.CheckAvatarAllSelected(PackageFilterData._DataType.SelectAvatar))
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_AllSelected");
		else
			selectedText.Text = GameUtility.GetUIString("UIEmptyList_NoAllSelected");

	}

	#region More button control
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

	private UIElemAvatarSelectListItem GetFirstAvatarListItem(bool headNotTail)
	{
		if (headNotTail)
		{
			if (avatarList.Count <= 0)
				return null;

			if (avatarList.GetItem(0).Data != null)
			{
				Debug.Assert(avatarList.GetItem(0).Data is UIElemAvatarSelectListItem);
				return avatarList.GetItem(0).Data as UIElemAvatarSelectListItem;
			}

			if (avatarList.Count <= 1)
				return null;

			if (avatarList.GetItem(1).Data is UIElemAvatarSelectListItem)
				return avatarList.GetItem(1).Data as UIElemAvatarSelectListItem;
			else
				return null;
		}
		else
		{
			if (avatarList.Count <= 0)
				return null;

			if (avatarList.GetItem(avatarList.Count - 1).Data != null)
			{
				Debug.Assert(avatarList.GetItem(avatarList.Count - 1).Data is UIElemAvatarSelectListItem);
				return avatarList.GetItem(avatarList.Count - 1).Data as UIElemAvatarSelectListItem;
			}

			if (avatarList.Count <= 1)
				return null;

			if (avatarList.GetItem(avatarList.Count - 2).Data is UIElemAvatarSelectListItem)
				return avatarList.GetItem(avatarList.Count - 2).Data as UIElemAvatarSelectListItem;
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
				return;

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
			int positionId = avatarLocation == null ? avatarPartner.PositionId : avatarLocation.PositionId;
			var position = SysLocalDataBase.Inst.LocalPlayer.PositionData.GetPositionById(positionId);
			var getContainer = getObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = getContainer.GetComponent<UIElemSelectAvatarListGetItem>();
			item.SetData(avatarLocation != null && avatarLocation.ShowLocationId == position.EmployShowLocationId);

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
				// 没有要填充的角色，
				RemoveShowMoreButton(true);
			else
			{
				var item = GetFirstAvatarListItem(true);
				Debug.Assert(item != null);
				if (item.indexInAvatarList > 0)
					AddShowMoreButton(true);
				else
					RemoveShowMoreButton(true);
			}
		}
		else
		{
			if (avatarToFillList.Count == 0)
				// 没有要填充的角色，
				RemoveShowMoreButton(false);
			else
			{
				var item = GetFirstAvatarListItem(false);
				Debug.Assert(item != null);
				if (item.indexInAvatarList < avatarToFillList.Count - 1)
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
			// 查找最后一个Item
			var firstItem = GetFirstAvatarListItem(true);

			// 如果没有，从第一个开始加载, 否则从第一个的前一个开始加载
			int firstIndex = firstItem == null ? 0 : firstItem.indexInAvatarList - 1; ;

			int firstStartindex = firstIndex;
			for (; firstStartindex > firstIndex - maxRowsInPage && firstStartindex >= 0; firstStartindex--)
			{
				UIListItemContainer firstContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemAvatarSelectListItem firstInsertItem = firstContainer.gameObject.GetComponent<UIElemAvatarSelectListItem>();
				firstContainer.data = firstInsertItem;

				firstInsertItem.SetData(avatarToFillList[firstStartindex]);
				firstInsertItem.indexInAvatarList = firstStartindex;

				// 向前插入
				if (HasShowMoreButton(true) == false)
					avatarList.InsertItem(firstContainer, 0, false, "", false);
				else
					avatarList.InsertItem(firstContainer, 1, false, "", false);
			}

			UpdateShowMoreButton(true);
		}
		else
		{
			// 查找最后一个Item
			var lastItem = GetFirstAvatarListItem(false);

			// 如果没有，从第一个开始加载, 否则从最后一个的下一个开始加载
			int lastIndex = lastItem == null ? 0 : lastItem.indexInAvatarList + 1;
			int lastStartindex = lastIndex;
			for (; lastStartindex < lastIndex + maxRowsInPage && lastStartindex < avatarToFillList.Count; lastStartindex++)
			{
				UIListItemContainer lastContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
				UIElemAvatarSelectListItem lastInsertItem = lastContainer.gameObject.GetComponent<UIElemAvatarSelectListItem>();
				lastContainer.data = lastInsertItem;

				lastInsertItem.SetData(avatarToFillList[lastStartindex]);
				lastInsertItem.indexInAvatarList = lastStartindex;

				// If "view more" button is in the list, insert before it
				if (HasShowMoreButton(false) == false)
					avatarList.InsertItem(lastContainer, avatarList.Count, true, "", false);
				else
					avatarList.InsertItem(lastContainer, avatarList.Count - 1, true, "", false);
			}

			UpdateShowMoreButton(false);
		}

		yield return null;

		int avatarItemCount = GetAvatarListItemCount();
		if (avatarItemCount > maxRows)
		{
			bool hasHeadShowMore = HasShowMoreButton(true);

			if (headNotTail)
			{
				// 从开始删除超过显示数量的item
				for (int headIndex = 0; headIndex < avatarItemCount - maxRows; ++headIndex)
				{
					int headRemoveIndex = hasHeadShowMore ? maxRows + 1 : maxRows;
					Debug.Assert(headRemoveIndex >= 0 && headRemoveIndex < avatarList.Count);
					avatarList.RemoveItem(headRemoveIndex, false, true, false);
				}

				UpdateShowMoreButton(false);
			}
			else
			{
				// 从开始删除超过显示数量的item
				for (int tailIndex = avatarItemCount - maxRows - 1; tailIndex >= 0; --tailIndex)
				{
					int tailRemoveIndex = hasHeadShowMore ? tailIndex + 1 : tailIndex;
					Debug.Assert(tailRemoveIndex >= 0 && tailRemoveIndex < avatarList.Count);
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

	#endregion

	//点击图标，显示详细内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAvatarIconClick(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		//KodGames.ClientClass.Avatar avatar = assetIcon.Data as KodGames.ClientClass.Avatar;
		UIElemAvatarSelectListItem item = assetIcon.Data as UIElemAvatarSelectListItem;

		UIPnlAvatarInfo.SelectDelegate selectDel = new UIPnlAvatarInfo.SelectDelegate(SeletAvatarItemByAvatar);
		SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAvatarInfo, item.AvatarData, false, true, false, false, selectDel);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoBtn(UIButton btn)
	{
		if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
		{
			if ((bool)btn.Data)
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatarDiner);
			else
				SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlShopWine);
		}
		else
		{
			//跳转场景，打开酒馆
			SysModuleManager.Instance.GetSysModule<SysGameStateMachine>().EnterState<GameState_CentralCity>(new UserData_ShowUI((((bool)btn.Data) ? _UIType.UIPnlAvatarDiner : _UIType.UIPnlShopWine)));
		}
	}

	public void OnChangeAvatarSuccess(KodGames.ClientClass.Location location)
	{
		HideSelf();
		SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnChangeAvatarSuccess(location);
	}

	public void OnChangeParterSuccess(int parterId, string avatarOffGuid, string avatarOnGuid, List<KodGames.ClientClass.Partner> partners)
	{
		HideSelf();
		SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().OnChangeParternSuccess(parterId, avatarOffGuid, avatarOnGuid, partners);
		//刷新属性加成信息
		SysUIEnv.Instance.GetUIModule<UIPnlAvatar>().CaculateAddPropertyByFriends();
	}

	//Click to return to UIPnlGuide.
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnBackClick(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickFilterAvatar(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgPackageAvatarFilter), PackageFilterData._DataType.SelectAvatar, new UIDlgPackageAvatarFilter.OnSelectFilterAvatar(InitView));
	}

	//点击更换角色
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnSelectBtnClick(UIButton btn)
	{
		UIElemAvatarSelectListItem item = btn.data as UIElemAvatarSelectListItem;
		if (null == item)
			return;
		else
			SeletAvatarItemByAvatar(item.AvatarData);
	}

	public void SeletAvatarItemByAvatar(KodGames.ClientClass.Avatar avatar)
	{
		if (avatarLocation != null)
			RequestMgr.Inst.Request(new ChangeLocationReq(avatar.Guid, avatar.ResourceId, avatarLocation.Guid, avatarLocation.PositionId, avatarLocation.ShowLocationId));
		else if (avatarPartner != null)
			RequestMgr.Inst.Request(new PartnerSetupReq(avatarPartner.PositionId, avatarPartner.PartnerId, avatarPartner.AvatarGuid, avatar.Guid));

	}




}

