using System;
using System.Collections.Generic;
using System.Collections;
using ClientServerCommon;
using UnityEngine;
using KodGames;

public class UIPnlHandBook : UIPnlItemInfoBase
{
	public List<UIButton> tagButtons; // tab标签
	public List<UIBox> tabNotifyList; // tab标签上面的通知标
	public UIButton closeButton; // 关闭按就
	public UIChildLayoutControl childLayout; // 两个list布局的控制器

	public AutoSpriteControlBase typeScrollBorder; // 类型列表的地板
	public UIScrollList typeScrollList; // 类型列表
	public GameObjectPool countryObjectPool;

	public AutoSpriteControlBase cardScrollBorder; // 碎片列表地板
	public UIScrollList cardScrollList; // 碎片列表
	public GameObjectPool finishedTitleObjectPool;
	public GameObjectPool rewardObjectPool;

	// 图标控件
	public UIElemNewEquipTemplate newEquipTemplate;

	//下面返回秘境历练
	public UIButton mainBackBtn;

	private float scrollBorderSmallHeight; // 记录两个list时碎片列表的高度
	private float scrollBorderFullHeight; // 记录一个list时碎片列表的高度

	private float scrollListMinHeight;
	private float scrollListMaxHeight;

	private int currentPageAssetType; // 当前选中的tab签
	private List<int> subTypeList = new List<int>(); // 当前tab签下面的子标签列表
	private object[] userDatas;

	private int rowIcons = 3;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// 初始化标签
		tabNotifyList[0].Data = tagButtons[0].Data = IDSeg._AssetType.Avatar;
		tabNotifyList[1].Data = tagButtons[1].Data = IDSeg._AssetType.Equipment;
		tabNotifyList[2].Data = tagButtons[2].Data = IDSeg._AssetType.CombatTurn;

		// 获取布局高度
		scrollBorderSmallHeight = cardScrollBorder.height;
		scrollBorderFullHeight = typeScrollBorder.height + childLayout.childLayoutControls[0].bottomRightOffset + childLayout.childLayoutControls[1].topLeftOffset + cardScrollBorder.height;

		scrollListMinHeight = cardScrollList.viewableArea.y;
		scrollListMaxHeight = cardScrollList.viewableArea.y + typeScrollBorder.height;
		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		InitMainBot();

		newEquipTemplate.gameObject.SetActive(false);

		this.userDatas = userDatas;

		RequestMgr.Inst.Request(new QueryIllustrationReq());

		return true;
	}

	public void Show()
	{
		if (userDatas.Length > 0)
		{
			int pageAssetType = IDSeg._AssetType.Avatar;
			int subType = AvatarConfig._AvatarCountryType.QinGuo;
			int resourceId = (int)userDatas[0];

			pageAssetType = IDSeg.ToAssetType(resourceId);

			switch (IDSeg.ToAssetType(resourceId))
			{
				case IDSeg._AssetType.Avatar:
					subType = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(resourceId).countryType;
					subType = (subType == AvatarConfig._AvatarCountryType.QinGuo ? subType : (AvatarConfig._AvatarCountryType.All & ~AvatarConfig._AvatarCountryType.QinGuo));
					break;

				case IDSeg._AssetType.Equipment:
					subType = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(resourceId).type;
					break;

				case IDSeg._AssetType.CombatTurn:
					subType = 0;
					break;
			}

			ShowPage(pageAssetType, subType);
		}
		else
			ShowPage(IDSeg._AssetType.Avatar);
	}

	public override void OnHide()
	{
		ClearTypeList();
		ClearCardList();

		base.OnHide();
	}

	private void InitMainBot()
	{
		if (SysGameStateMachine.Instance.CurrentState is GameState_CentralCity)
		{
			//show main menu bot.
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainMenuBot);

			// Set MainBotton Light.
			SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UIPnlHandBook);

			mainBackBtn.Hide(true);
			closeButton.Hide(false);
		}
		else
		{
			mainBackBtn.Hide(false);
			closeButton.Hide(true);
		}
	}

	private void ShowPage(int pageAssetType)
	{
		ClearTypeList();

		// 记录当前标签状态
		currentPageAssetType = pageAssetType;
		subTypeList = GetSubTypeList(pageAssetType);
		FillTypeList();

		// 设置tab状态
		SetPageButtonState(pageAssetType);

		// 设置布局
		if (subTypeList.Count == 0)
		{
			childLayout.HideChildObj(typeScrollBorder.gameObject, true);
			cardScrollBorder.SetSize(cardScrollBorder.width, scrollBorderFullHeight);
			cardScrollList.SetViewableArea(cardScrollList.viewableArea.x, scrollListMaxHeight);
		}
		else
		{
			childLayout.HideChildObj(typeScrollBorder.gameObject, false);
			cardScrollBorder.SetSize(cardScrollBorder.width, scrollBorderSmallHeight);
			cardScrollList.SetViewableArea(cardScrollList.viewableArea.x, scrollListMinHeight);
		}

		// 选择子类型
		int subType = subTypeList.Count != 0 ? subTypeList[0] : 0;
		ShowSubType(subType);

		RefreshTabNotifys(pageAssetType, subType);
	}

	private void ShowPage(int pageAssetType, int subType)
	{
		ClearTypeList();

		// 记录当前标签状态
		currentPageAssetType = pageAssetType;
		subTypeList = GetSubTypeList(pageAssetType);
		FillTypeList();

		// 设置tab状态
		SetPageButtonState(pageAssetType);

		// 设置布局
		if (subTypeList.Count == 0)
		{
			childLayout.HideChildObj(typeScrollBorder.gameObject, true);
			cardScrollBorder.SetSize(cardScrollBorder.width, scrollBorderFullHeight);
			cardScrollList.SetViewableArea(cardScrollList.viewableArea.x, scrollListMaxHeight);
		}
		else
		{
			childLayout.HideChildObj(typeScrollBorder.gameObject, false);
			cardScrollBorder.SetSize(cardScrollBorder.width, scrollBorderSmallHeight);
			cardScrollList.SetViewableArea(cardScrollList.viewableArea.x, scrollListMinHeight);
		}

		// 选择子类型
		ShowSubType(subType);

		RefreshTabNotifys(pageAssetType, subType);
	}

	/// <summary>
	/// 获取当前标签下的子标签列表
	/// </summary>
	private List<int> GetSubTypeList(int assetType)
	{
		var subTypeList = new List<int>();
		switch (assetType)
		{
			case IDSeg._AssetType.Avatar:
				subTypeList.Add(AvatarConfig._AvatarCountryType.QinGuo);
				subTypeList.Add(AvatarConfig._AvatarCountryType.All & ~AvatarConfig._AvatarCountryType.QinGuo); //六国
				break;

			case IDSeg._AssetType.Equipment:
				subTypeList.Add(EquipmentConfig._Type.Armor);
				subTypeList.Add(EquipmentConfig._Type.Decoration);
				subTypeList.Add(EquipmentConfig._Type.Shoe);
				subTypeList.Add(EquipmentConfig._Type.Treasure);
				subTypeList.Add(EquipmentConfig._Type.Weapon);
				break;

			case IDSeg._AssetType.CombatTurn:
				break;
		}

		return subTypeList;
	}

	/// <summary>
	/// 设置标签按钮状态
	/// </summary>
	private void SetPageButtonState(int pageAssetType)
	{
		foreach (UIButton btn in tagButtons)
			btn.controlIsEnabled = ((int)btn.Data != pageAssetType);
	}

	/// <summary>
	/// 清除标签列表
	/// </summary>
	private void ClearTypeList()
	{
		typeScrollList.ClearList(false);
		typeScrollList.ScrollPosition = 0;
		subTypeList.Clear();
	}

	private void FillTypeList()
	{
		foreach (var type in subTypeList)
		{
			//设置国家文字的显示
			UIElemHandBookCountryItem item = countryObjectPool.AllocateItem().GetComponent<UIElemHandBookCountryItem>();

			item.SetData(currentPageAssetType, type);

			//加入到list中
			typeScrollList.AddItem(item.gameObject);
		}
	}

	/// <summary>
	/// 获取某种类型的碎片是否有可以合并的
	/// </summary>
	private bool HasCombinableItemInType(int assetType)
	{
		foreach (var item in SysLocalDataBase.Inst.LocalPlayer.Consumables)
		{
			var cfg = ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationByFragmentId(item.Id);
			if (cfg == null)
				continue;

			if (IDSeg.ToAssetType(cfg.Id) != assetType)
				continue;

			if (item.Amount >= cfg.FragmentCount)
				return true;
		}

		return false;
	}

	/// <summary>
	/// 获取某种类型的碎片是否有可以合并的
	/// </summary>
	private bool HasCombinableItemInType(int assetType, int subType)
	{
		foreach (var item in SysLocalDataBase.Inst.LocalPlayer.Consumables)
		{
			var cfg = ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationByFragmentId(item.Id);
			if (cfg == null)
				continue;

			int tempSubType = IDSeg.InvalidId;
			int tempAssetType = IDSeg.ToAssetType(cfg.Id);

			if (tempAssetType != assetType)
				continue;

			switch (tempAssetType)
			{
				case IDSeg._AssetType.Avatar:
					tempSubType = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(cfg.Id) != null ? ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(cfg.Id).countryType : IDSeg.InvalidId;

					if (tempSubType != IDSeg.InvalidId && (tempSubType & subType) == 0)
						continue;

					if (item.Amount >= cfg.FragmentCount)
						return true;

					break;

				case IDSeg._AssetType.Equipment:
					tempSubType = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(cfg.Id) != null ? ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(cfg.Id).type : IDSeg.InvalidId;

					if (tempSubType != IDSeg.InvalidId && tempSubType != subType)
						continue;

					if (item.Amount >= cfg.FragmentCount)
						return true;
					break;
			}
		}

		return false;
	}

	/// <summary>
	/// 显示子标签
	/// </summary>
	private void ShowSubType(int subType)
	{
		// 跟新子类型状态
		SetSubTypeButtonState(subType);

		// 填充卡列表
		ClearCardList();
		StartCoroutine("FillCardList", GetCardList(subType));
	}

	/// <summary>
	/// 获取子标签下的碎片列表, 已排序
	/// </summary>
	private List<IllustrationConfig.Illustration> GetCardList(int subType)
	{
		var cardList = new List<IllustrationConfig.Illustration>();

		foreach (var cfg in ConfigDatabase.DefaultCfg.IllustrationConfig.Illustrations)
		{
			if (IDSeg.ToAssetType(cfg.Id) != currentPageAssetType)
				continue;

			if (currentPageAssetType == IDSeg._AssetType.Avatar)
			{
				var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(cfg.Id);
				if ((subType & avatarCfg.countryType) == 0)
					continue;
			}
			else if (currentPageAssetType == IDSeg._AssetType.Equipment)
			{
				var equipCfg = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(cfg.Id);
				if (equipCfg.type != subType)
					continue;
			}

			cardList.Add(cfg);
		}

		// 处理排序
		cardList.Sort(ComparCard);

		return cardList;
	}

	/// <summary>
	/// 判断某个碎片可否合并
	/// </summary>
	private int GetCardCombineByFragmentCount(IllustrationConfig.Illustration cfg)
	{
		return ItemInfoUtility.GetGameItemCount(cfg.FragmentId) / cfg.FragmentCount;
	}

	// 碎片排序
	private int ComparCard(IllustrationConfig.Illustration cfg1, IllustrationConfig.Illustration cfg2)
	{
		int cardConbimable1 = GetCardCombineByFragmentCount(cfg1);
		int cardConbimable2 = GetCardCombineByFragmentCount(cfg2);

		if (cardConbimable1 != cardConbimable2)
			return cardConbimable2 - cardConbimable1;
		else
		{
			switch (IDSeg.ToAssetType(cfg1.Id))
			{
				case IDSeg._AssetType.Avatar:
					return ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(cfg2.Id).sortIndex - ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(cfg1.Id).sortIndex;

				case IDSeg._AssetType.Equipment:
					return ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(cfg2.Id).sortIndex - ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(cfg1.Id).sortIndex;

				case IDSeg._AssetType.CombatTurn:
					return ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(cfg2.Id).sortIndex - ConfigDatabase.DefaultCfg.SkillConfig.GetSkillById(cfg1.Id).sortIndex;
			}

			return 0;
		}
	}

	/// <summary>
	/// 更新子标签状态
	/// </summary>	
	private void SetSubTypeButtonState(int subType)
	{
		for (int i = 0; i < typeScrollList.Count; ++i)
		{
			var container = typeScrollList.GetItem(i) as UIListItemContainer;
			UIElemHandBookCountryItem item = container.GetComponent<UIElemHandBookCountryItem>();
			item.EnableButton(item.SubType != subType);
			item.ActiveLightBox(item.SubType == subType);
		}
	}

	/// <summary>
	/// 清除子标签
	/// </summary>
	private void ClearCardList()
	{
		StopCoroutine("FillCardList");

		//图鉴中显示数据会有减少的情况，需要clearList的时候清除助手数据
		for (int i = 0; i < cardScrollList.Count; i++)
		{
			var item = cardScrollList.GetItem(i) as UIElemHandBookRewardProcessor;
			if (item == null)
				continue;

			item.ClearData();
		}

		cardScrollList.ClearList(false);
		cardScrollList.ScrollPosition = 0;
	}

	/// <summary>
	/// 填充碎片列表
	/// </summary>
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillCardList(List<IllustrationConfig.Illustration> cardList)
	{
		yield return null;

		//int fillIndex = 0;
		//bool fillingCombinable = true;

		List<IllustrationConfig.Illustration> itemCombinableLists = new List<IllustrationConfig.Illustration>();

		for (int i = 0; i < cardList.Count; )
		{
			if (GetCardCombineByFragmentCount(cardList[i]) > 0)
			{
				itemCombinableLists.Add(cardList[i]);
				cardList.Remove(cardList[i]);
			}
			else i++;
		}

		if (itemCombinableLists.Count > 0)
		{
			UIListItemContainer finishedTitleContainer = finishedTitleObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			switch (currentPageAssetType)
			{
				case IDSeg._AssetType.Avatar://以下角色可以合成
					finishedTitleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Avatar_CanFix");
					break;
				case IDSeg._AssetType.Equipment://以下装备可以合成
					finishedTitleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Equip_CanFix");
					break;
				case IDSeg._AssetType.CombatTurn://以下书籍可以合成
					finishedTitleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Book_CanFix");
					break;
			}
			cardScrollList.AddItem(finishedTitleContainer);
		}

		for (int i = 0; i < itemCombinableLists.Count; )
		{
			var item = rewardObjectPool.AllocateItem(false).GetComponent<UIElemHandBookRewardProcessor>();

			List<IllustrationConfig.Illustration> illustrationCfgs = new List<IllustrationConfig.Illustration>();

			while (illustrationCfgs.Count < rowIcons && i < itemCombinableLists.Count)
			{
				illustrationCfgs.Add(itemCombinableLists[i]);
				i++;
			}

			item.SetData(illustrationCfgs);
			cardScrollList.AddItem(item);
		}

		UIListItemContainer titleContainer = finishedTitleObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		switch (currentPageAssetType)
		{
			case IDSeg._AssetType.Avatar://以下角色暂时不可合成
				titleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Avatar_CanNotFix");
				break;
			case IDSeg._AssetType.Equipment://以下装备暂时不可合成
				titleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Equip_CanNotFix");
				break;
			case IDSeg._AssetType.CombatTurn://以下书籍暂时不可合成
				titleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Book_CanNotFix");
				break;
		}
		cardScrollList.AddItem(titleContainer);

		for (int i = 0; i < cardList.Count; )
		{
			var item = rewardObjectPool.AllocateItem(false).GetComponent<UIElemHandBookRewardProcessor>();

			List<IllustrationConfig.Illustration> illustrationCfgs = new List<IllustrationConfig.Illustration>();

			while (illustrationCfgs.Count < rowIcons && i < cardList.Count)
			{
				illustrationCfgs.Add(cardList[i]);
				i++;
			}

			item.SetData(illustrationCfgs);
			cardScrollList.AddItem(item);
		}

		//for (int index = 0; index < cardScrollList.Count; index++ )
		//{
		//    var item = cardScrollList.GetItem(index) as UIElemHandBookRewardProcessor;

		//    //if(item != null)
		//    //    item.ShowNameTest();			
		//}

		//for (; fillIndex < cardList.Count; fillIndex++)
		//{	
		//    var cfg = cardList[fillIndex];

		//    var itemCombinable = GetCardCombineByFragmentCount(cfg) > 0;
		//    if (fillingCombinable)
		//    {
		//        // 如果当前处于显示可合并碎片状态
		//        if (itemCombinable)
		//        {
		//            // 填充可合并碎片, 但是list中没有item, 增加标题
		//            if (cardScrollList.Count == 0)
		//            {

		//            }
		//        }
		//        else
		//        {
		//            // 填充可合并碎片状态, 但是当前碎片不能合并, 添加分割标题
		//            fillingCombinable = false;

		//            UIListItemContainer finishedTitleContainer = finishedTitleObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
		//            switch (currentPageAssetType)
		//            {
		//                case IDSeg._AssetType.Avatar://以下角色暂时不可合成
		//                    finishedTitleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Avatar_CanNotFix");
		//                    break;
		//                case IDSeg._AssetType.Equipment://以下装备暂时不可合成
		//                    finishedTitleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Equip_CanNotFix");
		//                    break;
		//                case IDSeg._AssetType.CombatTurn://以下书籍暂时不可合成
		//                    finishedTitleContainer.spriteText.Text = GameUtility.GetUIString("UIPnlHandBook_Book_CanNotFix");
		//                    break;
		//            }
		//            cardScrollList.AddItem(finishedTitleContainer);
		//        }
		//    }

		//    var item = rewardObjectPool.AllocateItem(false).GetComponent<UIElemHandBookRewardProcessor>();

		//    List<IllustrationConfig.Illustration> illustrationCfgs = new List<IllustrationConfig.Illustration>();
		//    for (int i = 0; i < 3; ++i)
		//    {			
		//        if (fillIndex < cardList.Count)
		//        {
		//            if (fillingCombinable == (GetCardCombineByFragmentCount(cardList[fillIndex]) > 0))
		//            {
		//                illustrationCfgs.Add(cardList[fillIndex]);
		//                fillIndex++;
		//            }
		//            else break;
		//        }
		//        else break;
		//    }

		//    item.SetData(illustrationCfgs);

		//    cardScrollList.AddItem(item);
		//}
	}

	// 刷新当前操作的list的显示状态
	private void RefrushListCardState(int illustrationId)
	{
		for (int i = 0; i < cardScrollList.Count; i++)
		{
			var item = cardScrollList.GetItem(i).gameObject.GetComponent<UIElemHandBookRewardProcessor>();

			if (item == null)
				continue;

			item.ItemRefresh(illustrationId);
		}
	}

	// 切换页签
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabButtonClick(UIButton clickBtn)
	{
		ShowPage((int)clickBtn.Data);
	}

	private void RefreshTabNotifys(int pageType, int subType)
	{
		foreach (UIBox box in tabNotifyList)//遍历页签
		{
			if ((int)box.Data != pageType)//不是当前点击的页签
				box.Hide(HasCombinableItemInType((int)box.Data) == false);
			else //是当前点击的页签，直接不显示notify组件{
				box.Hide(true);
		}

		RefreshSubTypeNotifys(pageType, subType);
	}

	private void RefreshSubTypeNotifys(int pageType, int subType)
	{
		for (int i = 0; i < typeScrollList.Count; i++)
		{
			UIElemHandBookCountryItem subTypeItem = (typeScrollList.GetItem(i) as UIListItemContainer).GetComponent<UIElemHandBookCountryItem>();
			if (subTypeItem.SubType != subType)
			{
				subTypeItem.notify.Hide(HasCombinableItemInType(pageType, subTypeItem.SubType) == false);
			}
			else
			{
				subTypeItem.notify.Hide(true);
			}
		}
	}

	// 切换国家类型或者装备类型
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTypeIconClick(UIButton btn)
	{
		ShowSubType((btn.data as UIElemHandBookCountryItem).SubType);
		RefreshSubTypeNotifys(currentPageAssetType, (btn.data as UIElemHandBookCountryItem).SubType);
	}

	// 点击icon查看信息
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRewardIcon(UIButton btn)
	{
		var item = btn.Data as UIElemHandBookAssetIcon;
		GameUtility.ShowAssetInfoUI(item.IllustrationCfg.Id);
	}

	// 点击合成
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPickReawrd(UIButton btn)
	{
		var illustrationCfg = (btn.Data as UIElemHandBookAssetIcon).IllustrationCfg;
		int cardCombineCount = GetCardCombineByFragmentCount(illustrationCfg);

		if (cardCombineCount > 0)
		{
			int minCombineCount = 2;
			bool canCardCombine = true;
			foreach (var cost in illustrationCfg.Cost)
			{
				if (ItemInfoUtility.GetGameItemCount(cost.id) < cost.count * minCombineCount)
				{
					canCardCombine = false;
					break;
				}
			}
			if (canCardCombine && cardCombineCount >= minCombineCount)
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgIllustrationBatchSynthesis), new object[] { illustrationCfg, cardCombineCount });
			else
			{
				if (UIElemPackageCapacity.TotalCount >= UIElemPackageCapacity.TotalCapacity)
				{
					SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlHandBook_PackageFull"));
					return;
				}
				// Show Confirm Dialogue.
				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				string title = GameUtility.GetUIString("UIPnlHandBook_HeCheng_Confirm");
				string message = GameUtility.FormatUIString("UIPnlHandBook_ConfirmPrompt",
					GameDefines.textColorWhite,
					ItemInfoUtility.GetAssetName(illustrationCfg.Id),
					GameDefines.txColorLightOrangeColor,
					ItemInfoUtility.GetAssetName(illustrationCfg.FragmentId),
					illustrationCfg.FragmentCount.ToString(),
					ItemInfoUtility.GetAssetName(illustrationCfg.Cost[0].id),
					//ItemInfoUtility.GetGameItemCount(illustrationCfg.Cost[0].id));
					illustrationCfg.Cost[0].count);
				MainMenuItem cancelCallback = new MainMenuItem();
				cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIPnlHandBook_HeCheng");

				okCallback.CallbackData = illustrationCfg.Id;
				okCallback.Callback = (data) =>
				{
					RequestMgr.Inst.Request(new MergeIllustrationReq((int)data, 1));
					return true;
				};

				showData.SetData(title, message, cancelCallback, okCallback);
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgMessage), showData, SpriteText.Alignment_Type.Center);
			}

		}
		else
		{
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgItemGetWay), illustrationCfg.Id);
		}
	}

	public void OnPickRewardSuccess(int illustrationId, int count)
	{
		RefrushListCardState(illustrationId);

		SysUIEnv.Instance.GetUIModule<UIEffectPowerUp>().SetEffectHideCallback((data) =>
		{
			if (IDSeg.ToAssetType(illustrationId) == IDSeg._AssetType.Avatar && count == 1)
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCardImage), illustrationId, false, true, null);
			else
			{
				List<Pair<int, int>> rewardPackagePars = new List<Pair<int, int>>();
				Pair<int, int> pair = new Pair<int, int>();
				pair.second = count;
				pair.first = illustrationId;
				rewardPackagePars.Add(pair);
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgComposeReward), rewardPackagePars, GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_GongXiGet_Title"), GameUtility.GetUIString("UIPnlHandBook_PiLiangHeCheng_HeChengGet_Title"));
			}
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMainMenuBot))
				SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().UpdateHandBookNotify();
		});

		SysUIEnv.Instance.AddShowEvent(new SysUIEnv.UIModleShowEvent(typeof(UIEffectPowerUp), illustrationId, UIEffectPowerUp.LabelType.Success, true));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCloseButtonClick(UIButton btn)
	{
		if (SysSceneManager.Instance.IsSceneLoaded("QSMY_01"))
		{
			if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMainMenuBot))
				SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UnKonw);
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
		}
		else
		{
			if (SysGameStateMachine.Instance.CurrentState is GameState_Dungeon)
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignScene));
			if (SysGameStateMachine.Instance.CurrentState is GameState_ActivityDungeon)
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCampaignActivityScene));
		}

		this.HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		this.HideSelf();
	}
}