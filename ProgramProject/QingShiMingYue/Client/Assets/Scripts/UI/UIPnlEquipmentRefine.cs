using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlEquipmentRefine : UIModule
{
	// Equipment common view.
	public UIElemAssetIcon equipIcon;
	public SpriteText equipNameLabel;
	public SpriteText equipQualityLabel;
	//public SpriteText equipRefineTimesLabel;		// Dynamite Value.
	public SpriteText equipRefineTimesCue;
	//public SpriteText equipRefineFunctionLabel;  // Dynamite Value.本次精炼效果

	//Equipment Refine Level view
	public SpriteText equipLevelBefore;
	public SpriteText equipLevelAfter;

	// Equipment RefineBefore view.
	public GameObject RefineCostRoot;
	public GameObject RefineLevelRoot;

	// Refine Cost view.
	public SpriteText breakInfoLabel;
	public SpriteText pillRefineCostLabel;	//Pill
	public SpriteText cardRefineCostLabel;	//Card
	public List<SpriteText> otherCostLabel;

	public UIScrollList cardScrollList;
	public GameObjectPool cardItemPool;

	// Equipment RefineDown view.
	public SpriteText refineDownLabel;

	public SpriteText emptyTip;

	//突破等级显示
	public UIElemBreakThroughBtn equipmentBreakProgressLift;
	public UIElemBreakThroughBtn equipmentBreakProgressRight;

	//铜币显示连带控件
	public SpriteText gameMoneyLable;
	public UIBox gameMoneyBox;

	//陨铁显示连带控件
	public SpriteText pillLabel;
	public UIBox pillBox;
	public UIBox activityNotify;

	private bool isInit = false;

	private KodGames.ClientClass.Equipment equipment;

	private float equipmentPower;
	private float positionPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		equipmentPower = 0f;
		positionPower = 0f;

		this.equipment = userDatas[0] as KodGames.ClientClass.Equipment;

		SysUIEnv.Instance.GetUIModule<UIPnlEquipmentPowerUpTab>().ChangeTabButtons(_UIType.UIPnlEquipmentRefine, this.equipment);
		isInit = true;
		//ShowRefineUI();
		StartCoroutine("FillRefineBeforeCardList");
		emptyTip.Text = string.Empty;

		return true;
	}
	public override void OnHide()
	{

		ClearList();
		base.OnHide();
	}

	private void ClearList()
	{
		StopCoroutine("FillRefineBeforeCardList");
		cardScrollList.ClearList(false);
	}


	#region  RefineUIView
	private void ShowRefineUI()
	{
		// Set the Equipment view info.
		// Set equipment Icon.
		equipIcon.SetData(equipment);

		// Set equipment name.
		equipNameLabel.Text = ItemInfoUtility.GetAssetName(equipment.ResourceId);

		// Set equipment quality.
		equipQualityLabel.Text = ItemInfoUtility.GetAssetQualityLongColorDesc(equipment.ResourceId);

		// Set equipment break up cue
		equipRefineTimesCue.Text = string.Format(GameUtility.GetUIString("UIPnlEquipmentRefine_CardUp"), ItemInfoUtility.GetLevelCN(equipment.BreakthoughtLevel));

		//Set equipment max breakthought level
		if (equipment.BreakthoughtLevel >= ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetMaxBreakthoughtLevel())
		{
			// Hide root
			RefineCostRoot.SetActive(false);
			RefineLevelRoot.SetActive(false);

			equipmentBreakProgressLift.SetBreakThroughIcon(equipment.BreakthoughtLevel);
			equipmentBreakProgressRight.SetBreakThroughIcon(equipment.BreakthoughtLevel);

			//Show the refineDown message
			refineDownLabel.Hide(false);

			refineDownLabel.Text = GameUtility.GetUIString("UIPnlEquipmentReifne_BreakMax");
			cardScrollList.ClearList(false);
		}
		else
		{
			EquipmentConfig.Equipment equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId);
			EquipmentConfig.EquipBreakthrough equipBreakThrough = equipConfig.GetBreakthroughByTimes(equipment.BreakthoughtLevel);


			//Show root 
			RefineLevelRoot.SetActive(true);
			RefineCostRoot.SetActive(true);

			equipmentBreakProgressLift.SetBreakThroughIcon(equipment.BreakthoughtLevel);
			equipmentBreakProgressRight.SetBreakThroughIcon(equipment.BreakthoughtLevel + 1);

			//Hide refineDown message
			refineDownLabel.Text = string.Empty;


			equipLevelBefore.Text = string.Format(GameUtility.GetUIString("UIPnlEquipmentReifne_LevelInfo"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, equipment.LevelAttrib.Level, GameDefines.textColorInOrgYew, equipBreakThrough.breakThrough.powerUpLevelLimit);
			equipLevelAfter.Text = string.Format(GameUtility.GetUIString("UIPnlEquipmentReifne_LevelInfo"), GameDefines.textColorBtnYellow, GameDefines.colorGoldYellow, equipment.LevelAttrib.Level, GameDefines.textColorInOrgYew, equipConfig.GetBreakthroughByTimes(equipment.BreakthoughtLevel + 1).breakThrough.powerUpLevelLimit);

			//Show the refine cost label
			breakInfoLabel.Text = string.Format(GameUtility.GetUIString("UIPnlEquipmentReifne_BreakInfo"), equipBreakThrough.breakThrough.sameCardDeductItemCount, equipBreakThrough.breakThrough.itemCostItemCount);

			//精炼石
			KodGames.ClientClass.Consumable consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(equipBreakThrough.breakThrough.itemCostItemId);
			int itemAllCount = 0;

			int maxSelectCount = 0;

			int ascensionNum = equipBreakThrough.breakThrough.itemCostItemCount;

			if (consumable != null)
			{
				itemAllCount = consumable.Amount;
			}
			int selectCount = 0;

			if (equipment.BreakthoughtLevel < ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetMaxBreakthoughtLevel())
			{

				if (cardScrollList.Count > 0 && isInit)
				{
					maxSelectCount = equipBreakThrough.breakThrough.itemCostItemCount / equipBreakThrough.breakThrough.sameCardDeductItemCount;
					if (maxSelectCount > cardScrollList.Count)
					{
						maxSelectCount = cardScrollList.Count;
					}

					for (int index = 0; index < maxSelectCount; index++)
					{
						UIElemEquipmentRefineCardItem item = cardScrollList.GetItem(index).Data as UIElemEquipmentRefineCardItem;
						if (item != null && item.Equip.LevelAttrib.Level == 1)
						{
							item.SetIconSelected(true);
							selectCount++;
						}

					}
					//DefaultSelectedAvatars(maxSelectCount);
					ascensionNum -= equipBreakThrough.breakThrough.sameCardDeductItemCount * selectCount;
					isInit = false;
				}
			}
			pillRefineCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob2"),
				itemAllCount >= ascensionNum ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											itemAllCount, ascensionNum);



			cardRefineCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlEquipmentRefine_CostSameCard"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, selectCount.ToString());
			//获取玩家信息
			KodGames.ClientClass.Player player = SysLocalDataBase.Inst.LocalPlayer;
			//如果有两个，显示铜币和陨铁石
			if (equipBreakThrough.breakThrough.otherCosts.Count == 2)
			{
				otherCostLabel[1].Hide(false);
				gameMoneyBox.Hide(false);
				gameMoneyLable.Hide(false);
				otherCostLabel[1].Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
											player.GameMoney >= equipBreakThrough.breakThrough.otherCosts[1].count ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											equipBreakThrough.breakThrough.otherCosts[1].count);

				otherCostLabel[0].Hide(false);
				pillBox.Hide(false);
				pillLabel.Hide(false);
				otherCostLabel[0].Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob2"),
											player.Iron >= equipBreakThrough.breakThrough.otherCosts[0].count ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											player.Iron, equipBreakThrough.breakThrough.otherCosts[0].count);
			}
			//只有一个，显示铜币
			else if (equipBreakThrough.breakThrough.otherCosts.Count == 1)
			{
				otherCostLabel[0].Hide(true);
				pillBox.Hide(true);
				pillLabel.Hide(true);
				otherCostLabel[0].Text = "";

				otherCostLabel[1].Hide(false);
				gameMoneyBox.Hide(false);
				gameMoneyLable.Hide(false);
				otherCostLabel[1].Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
											player.GameMoney >= equipBreakThrough.breakThrough.otherCosts[0].count ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											equipBreakThrough.breakThrough.otherCosts[0].count);

			}
			//一个都没有，全部不现实
			else
			{
				otherCostLabel[0].Hide(true);
				pillBox.Hide(true);
				pillLabel.Hide(true);
				otherCostLabel[0].Text = "";

				otherCostLabel[1].Hide(true);
				gameMoneyBox.Hide(true);
				gameMoneyLable.Hide(true);
				otherCostLabel[1].Text = "";
			}

			if (activityNotify != null)
			{
				activityNotify.Hide(!ItemInfoUtility.IsBreakNotifyActivity_Equip(equipment, ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId)));
			}


		}
	}
	private void DefaultSelectedAvatars(int selectCount)
	{
		for (int index = 0; index < selectCount; index++)
		{
			UIElemEquipmentRefineCardItem item = cardScrollList.GetItem(index).Data as UIElemEquipmentRefineCardItem;
			if (item != null && item.Equip.LevelAttrib.Level == 1)
				item.SetIconSelected(true);

		}

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillRefineBeforeCardList()
	{
		yield return null;
		cardScrollList.ClearList(false);
		cardScrollList.ScrollPosition = 0f;

		List<KodGames.ClientClass.Equipment> tempEquipments = new List<KodGames.ClientClass.Equipment>();

		//Set the SameCardList
		foreach (var equip in SysLocalDataBase.Inst.LocalPlayer.Equipments)
		{
			if (equip.ResourceId != equipment.ResourceId ||
				equip.Guid.Equals(this.equipment.Guid) ||
				PlayerDataUtility.IsLineUpInPosition(SysLocalDataBase.Inst.LocalPlayer, equip))
				continue;

			tempEquipments.Add(equip);
		}
		tempEquipments.Sort(DataCompare.CompareEquipmentReverse);

		foreach (KodGames.ClientClass.Equipment tempEquipment in tempEquipments)
		{
			UIListItemContainer container = cardItemPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemEquipmentRefineCardItem item = container.GetComponent<UIElemEquipmentRefineCardItem>();
			container.Data = item;

			item.SetData(tempEquipment);
			cardScrollList.AddItem(container);
		}

		if (cardScrollList.Count <= 0 && !emptyTip.Text.Equals(GameUtility.GetUIString("UIEmptyList_Equip")))
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Equip");
		else if (cardScrollList.Count > 0 && !emptyTip.Text.Equals(string.Empty))
			emptyTip.Text = string.Empty;
		cardScrollList.ScrollToItem(0, 0);

		ShowRefineUI();

	}

	private List<string> GetSelectedCards()
	{
		List<string> selectedCardIds = new List<string>();
		for (int index = 0; index < cardScrollList.Count; index++)
		{
			UIElemEquipmentRefineCardItem item = cardScrollList.GetItem(index).Data as UIElemEquipmentRefineCardItem;
			if (item.IsIconSelected())
				selectedCardIds.Add(item.Equip.Guid);
		}



		return selectedCardIds;
	}

	////单击选择同名卡
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCardSelect(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		UIElemEquipmentRefineCardItem item = assetIcon.Data as UIElemEquipmentRefineCardItem;

		EquipmentConfig.EquipBreakthrough breakThoughtConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel);

		if (item.IsIconSelected())
		{
			item.SetIconSelected(false);
		}
		else if (breakThoughtConfig.breakThrough.sameCardDeductItemCount > GetSelectedCards().Count && breakThoughtConfig.breakThrough.sameCardDeductItemCount * GetSelectedCards().Count < breakThoughtConfig.breakThrough.itemCostItemCount)
		{
			item.SetIconSelected(true);
		}
		else
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAvatarBreakThroughTab_Tip_SelectMore"));
		}
		int cardSelect = GetSelectedCards().Count;
		//Cost pill = pillCost - sameCardDeductItemCount*selectedSameCard
		int pillCountChange = breakThoughtConfig.breakThrough.itemCostItemCount - breakThoughtConfig.breakThrough.sameCardDeductItemCount * cardSelect;
		cardRefineCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlEquipmentRefine_CostSameCard"), GameDefines.textColorBtnYellow, GameDefines.textColorWhite, cardSelect.ToString());

		int totalSprites = 0;//精炼石数量
		var consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(breakThoughtConfig.breakThrough.itemCostItemId);
		if (consumable != null)
			totalSprites = consumable.Amount;

		pillRefineCostLabel.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob2"),
											totalSprites >= pillCountChange ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											totalSprites, pillCountChange);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBreakInfo(UIButton btn)
	{
		UIPnlAvatarAttributeUpdateDetail.ChangeData data0 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data0.Level = equipment.LevelAttrib.Level;
		data0.ResourceId = equipment.ResourceId;
		data0.BreakthoughtLevel = equipment.BreakthoughtLevel;

		UIPnlAvatarAttributeUpdateDetail.ChangeData data1 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data1.Level = equipment.LevelAttrib.Level;
		data1.ResourceId = equipment.ResourceId;
		data1.BreakthoughtLevel = equipment.BreakthoughtLevel + 1;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarAttributeUpdateDetail), false, UIPnlAvatarAttributeUpdateDetail._UIShowType.EquipmentRefine, data0, data1);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCardRefine(UIButton btn)
	{
		List<string> selectedCardIds = GetSelectedCards();
		//EquipmentConfig.EquipBreakthrough breakThoughtConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipment.ResourceId).GetBreakthroughByTimes(equipment.BreakthoughtLevel);
		//int cardCost = GetSelectedCards().Count * breakThoughtConfig.breakThrough.sameCardDeductItemCount;
		//int pillCount = 0;
		//if (SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(breakThoughtConfig.breakThrough.itemCostItemId) != null)
		//    pillCount = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(breakThoughtConfig.breakThrough.itemCostItemId).Amount;
		//if (cardCost + pillCount < breakThoughtConfig.breakThrough.itemCostItemCount)
		//{
		//    string message = GameUtility.GetUIString("UIPnlEquipmentRefine_PillUp");
		//    SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), message);
		//}
		//else
		//    if (equipment.LevelAttrib.Level < breakThoughtConfig.breakThrough.powerUpLevelLimit)
		//    {
		//        string message = string.Format(GameUtility.GetUIString("UIPnlEquipmentRefine_CardUp"), ItemInfoUtility.GetLevelCN(equipment.BreakthoughtLevel));
		//        SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), message);
		//    }
		//    else
		//    {
		//        foreach (var equipGuid in selectedCardIds)
		//        {
		//            //Warn player selectEquipment contains already refiened equipment
		//            if (SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(equipGuid).BreakthoughtLevel > 0)
		//            {
		//                SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		//                UIDlgMessage dlgMessage = uiEnv.GetUIModule<UIDlgMessage>();
		//                UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

		//                string title = GameUtility.GetUIString("UIDlgMessage_Title_Tips");
		//                string message = GameUtility.GetUIString("UIDlgMessage_Message_EquipCardRefine");

		//                MainMenuItem okCallback = new MainMenuItem();
		//                okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
		//                okCallback.Callback =
		//                    (data) =>
		//                    {
		//                        RequestMgr.Inst.Request(new EquipmentBreakthoutReq(equipment.Guid, selectedCardIds));
		//                        return true;
		//                    }; ;

		//                MainMenuItem cancelCallback = new MainMenuItem();
		//                cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");
		//                showData.SetData(title, message, okCallback, cancelCallback);
		//                dlgMessage.ShowDlg(showData);
		//                return;
		//            }
		//        }
		//        RequestMgr.Inst.Request(new EquipmentBreakthoutReq(equipment.Guid, selectedCardIds));
		//    }

		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, equipment.Guid, equipment.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		equipmentPower = ConfigDatabase.DefaultCfg.EquipmentConfig.GetOneEquipmentBasePower(equipment.ResourceId, equipment.LevelAttrib.Level, equipment.BreakthoughtLevel);

		foreach (var equipGuid in selectedCardIds)
		{
			//Warn player selectEquipment contains already refiened equipment
			if (SysLocalDataBase.Inst.LocalPlayer.SearchEquipment(equipGuid).BreakthoughtLevel > 0)
			{
				SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
				UIDlgMessage dlgMessage = uiEnv.GetUIModule<UIDlgMessage>();
				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

				string title = GameUtility.GetUIString("UIDlgMessage_Title_Tips");
				string message = GameUtility.GetUIString("UIDlgMessage_Message_EquipCardRefine");

				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
				okCallback.Callback =
					(data) =>
					{
						RequestMgr.Inst.Request(new EquipmentBreakthoutReq(equipment.Guid, selectedCardIds));
						return true;
					}; ;

				MainMenuItem cancelCallback = new MainMenuItem();
				cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");
				showData.SetData(title, message, cancelCallback, okCallback);
				dlgMessage.ShowDlg(showData);
				return;
			}


		}

		//StartCoroutine("FillRefineBeforeCardList");

		//SysUIEnv.Instance.ShowUIModule(typeof(UIPnlEquipmentRefine), equipment);

		RequestMgr.Inst.Request(new EquipmentBreakthoutReq(equipment.Guid, selectedCardIds));
	}

	public void OnResponseEquipmentBreakthoutSuccess()
	{
		SysUIEnv.Instance.GetUIModule<UIEffectPowerUp>().SetEffectHideCallback(
		(DataCompare) =>
		{
			ShowRefineUI();
			//SysUIEnv.Instance.ShowUIModule(typeof(UIDlgEquipRefineAttrbute), equipment, true);

			UIPnlAvatarAttributeUpdateDetail.ChangeData data0 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
			data0.Level = equipment.LevelAttrib.Level;
			data0.ResourceId = equipment.ResourceId;
			data0.BreakthoughtLevel = equipment.BreakthoughtLevel - 1;

			UIPnlAvatarAttributeUpdateDetail.ChangeData data1 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
			data1.Level = equipment.LevelAttrib.Level;
			data1.ResourceId = equipment.ResourceId;
			data1.BreakthoughtLevel = equipment.BreakthoughtLevel;

			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarAttributeUpdateDetail), true, UIPnlAvatarAttributeUpdateDetail._UIShowType.EquipmentRefine, data0, data1);

			float tempEquipmentPower = ConfigDatabase.DefaultCfg.EquipmentConfig.GetOneEquipmentBasePower(equipment.ResourceId, equipment.LevelAttrib.Level, equipment.BreakthoughtLevel);
			if (tempEquipmentPower > equipmentPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempEquipmentPower - equipmentPower)));
			else if (tempEquipmentPower < equipmentPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneDown", (int)(equipmentPower - tempEquipmentPower)));

			if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, equipment.Guid, equipment.ResourceId))
			{
				float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
				if (tempPositionPower > positionPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
				else if (tempPositionPower < positionPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));
			}
			equipmentPower = 0f;
			positionPower = 0;

		}
		);
		StartCoroutine("FillRefineBeforeCardList");
		SysUIEnv.Instance.ShowUIModule(typeof(UIEffectPowerUp), equipment.ResourceId, UIEffectPowerUp.LabelType.Success);
	}
	#endregion
}
