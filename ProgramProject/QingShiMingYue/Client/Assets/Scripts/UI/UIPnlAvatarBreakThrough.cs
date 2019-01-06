using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ClientServerCommon;

public class UIPnlAvatarBreakThrough : UIModule
{
	//About List.
	public UIScrollList avatarList;//同名卡列表
	public GameObjectPool avatarObjectPool;
	public SpriteText emptyTip;
	public SpriteText sameCardTip;//消耗说明

	public UIBox cost1Box;
	public UIBox cost2Box;
	//精魄
	public GameObject costItemObject;//精魄
	public SpriteText cryItemName;
	public SpriteText cryItemCount;//消耗精魂
	public UIElemAssetIcon cryItemIcon;//精魂icon

	//突破丹
	public SpriteText costItemCount;//突破丹数量
	public UIElemAssetIcon costItemIcon;//突破丹icon
	public SpriteText costItemLabel;	//突破静态字

	public SpriteText costCoinCount;//消耗铜币数量
	public SpriteText cadCoinCount;//消耗同名卡
	public SpriteText costCoinLabel;

	public UIBox activityNotity;	//绿点

	//等级显示方面
	public UIBox maxLevelTipBox;//满级提示
	public UIBox LevelUpTipBox;//升级提示
	public UIButton LeveUpBtn;//升级按钮

	//左边数据
	public SpriteText avatarHPLabelLift;
	public SpriteText avatarATKLabelLift;
	public SpriteText avatarSpeedLabelLift;
	public SpriteText avatarLVLabelLift;

	//右边数据
	public SpriteText avatarHPLabelRight;
	public SpriteText avatarATKLabelRight;
	public SpriteText avatarSpeedLabelRight;
	public SpriteText avatarLVLabelRight;

	//突破等级显示
	public UIElemBreakThroughBtn avatarBreakProgressLift;
	public UIElemBreakThroughBtn avatarBreakProgressRight;

	//角色卡片显示方面
	public UIElemAvatarCard avatarCardLift;
	public UIElemAvatarCard avatarCardRight;

	// 组合技图标
	public UIElemAvatarInfoSkill leftCompositeSkillItem;
	public UIElemAvatarInfoSkill rightCompositeSkillItem;

	//条目位置
	public Transform twoBoxTransform;
	public Transform oneBoxTransform;

	private KodGames.ClientClass.Avatar avatarLocalData;
	private KodGames.ClientClass.Player currentPlayer { get { return SysLocalDataBase.Inst.LocalPlayer; } }

	private float avatarPower;
	private float positionPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		avatarPower = 0f;
		positionPower = 0f;

		avatarLocalData = userDatas[0] as KodGames.ClientClass.Avatar;

		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarBreakThrough, avatarLocalData);

		ShowUI();

		return true;
	}

	public override void OnHide()
	{
		ClearCardList();

		base.OnHide();
	}

	private void ClearCardList()
	{
		StopCoroutine("FillAvatarList");
		avatarList.ClearList(false);
		avatarList.ScrollListTo(0f);

		emptyTip.Text = "";
	}

	private void ShowUI()
	{
		ClearCardList();

		//设置2D信息面板
		avatarCardLift.SetData(avatarLocalData.ResourceId, false, false, null);
		avatarCardRight.SetData(avatarLocalData.ResourceId, false, false, null);

		int maxBreakThroughLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetMaxBreakthoughtLevel();

		// Set Common UI. 
		maxLevelTipBox.Hide(avatarLocalData.BreakthoughtLevel < maxBreakThroughLevel);
		LevelUpTipBox.Hide(avatarLocalData.BreakthoughtLevel >= maxBreakThroughLevel);
		LeveUpBtn.controlIsEnabled = avatarLocalData.BreakthoughtLevel < maxBreakThroughLevel;

		// Set BreakThought Level Progress.
		avatarBreakProgressLift.SetBreakThroughIcon(avatarLocalData.BreakthoughtLevel);
		avatarBreakProgressRight.SetBreakThroughIcon(avatarLocalData.BreakthoughtLevel >= maxBreakThroughLevel ? avatarLocalData.BreakthoughtLevel : avatarLocalData.BreakthoughtLevel + 1);

		//判断是否消耗同名卡和突破丹
		if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough.isCostSameCardItem)
		{
			//Set UI.
			cost1Box.Hide(false);
			costItemCount.Hide(false);
			costItemIcon.Hide(false);
			cadCoinCount.Hide(false);
			costItemLabel.Hide(false);
			costCoinLabel.Hide(false);

			sameCardTip.Hide(false);

			cost2Box.transform.localPosition = twoBoxTransform.localPosition;

			StartCoroutine("FillAvatarList", GetSameIdAvatars(avatarLocalData));
		}
		else
		{
			if (avatarLocalData.BreakthoughtLevel + 1 <= maxBreakThroughLevel)
				emptyTip.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Tips_isCostSameCardItem_01", GameDefines.textColorGreen.ToString(),
										avatarLocalData.BreakthoughtLevel,
										avatarLocalData.BreakthoughtLevel + 1);
			else
				emptyTip.Text = string.Empty;

			cost1Box.Hide(true);
			costItemCount.Hide(true);
			costItemIcon.Hide(true);
			cadCoinCount.Hide(true);
			costItemLabel.Hide(true);
			costCoinLabel.Hide(true);

			sameCardTip.Text = string.Empty;
			sameCardTip.Hide(true);

			cost2Box.transform.localPosition = oneBoxTransform.localPosition;

			SetBreakThroughCostUI();
		}

		// Set Before and After UI.
		SetLift();
		SetRight();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillAvatarList(List<KodGames.ClientClass.Avatar> avatars)
	{
		yield return null;

		//加载同名卡List
		foreach (KodGames.ClientClass.Avatar avatar in avatars)
		{
			var itemContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			var item = itemContainer.gameObject.GetComponent<UIElemAvatarSelectToggleItem>();

			itemContainer.data = item;
			item.ResetToggleState(false);
			item.SetData(avatar);

			avatarList.AddItem(itemContainer);
		}

		if (avatarList.Count <= 0 && !GameUtility.EqualsFormatString(emptyTip.Text, GameUtility.GetUIString("UIEmptyList_Avatar")))
			emptyTip.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Tips_isCostSameCardItem", GameDefines.textColorBtnYellow.ToString());
		else if (avatarList.Count > 0 && !string.IsNullOrEmpty(emptyTip.Text))
			emptyTip.Text = "";

		SetDefalutSelectedAvatars();

		SetBreakThroughCostUI();
	}

	private void SetDefalutSelectedAvatars()
	{
		if (avatarList.Count <= 0)
			return;

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		if (avatarLocalData.BreakthoughtLevel >= avatarCfg.GetMaxBreakthoughtLevel())
			return;

		var breakCfg = avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel);

		// 配置了最少同名卡限制，那么才有默认选中功能
		if (breakCfg.leastSameCardCount > 0)
			DefaultSelectedAvatars(Mathf.Min(breakCfg.leastSameCardCount, avatarList.Count));
	}

	private void SetBreakThroughCostUI()
	{
		// Get BreakThrough Config.
		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		var avatarBreakCfg = avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel >= avatarCfg.GetMaxBreakthoughtLevel() ? avatarLocalData.BreakthoughtLevel - 1 : avatarLocalData.BreakthoughtLevel);
		var breakCfg = avatarBreakCfg.breakThrough;

		if (breakCfg.isCostSameCardItem)
		{
			// Same Card Number.
			int selectedAvatarsCount = GetSelectedAvatars().Count;

			// Item Number Required Left.
			int itemRequireCount = breakCfg.itemCostItemCount - selectedAvatarsCount * breakCfg.sameCardDeductItemCount;

			// Set Same Card And Item Relation Label.
			if (avatarLocalData.BreakthoughtLevel < avatarCfg.GetMaxBreakthoughtLevel())
			{
				if (avatarBreakCfg.leastSameCardCount > 0)
					sameCardTip.Text = GameUtility.FormatUIString(
						"UIPnlAvatarBreakThrough_SameCardItemReduct1",
						avatarBreakCfg.leastSameCardCount,
						breakCfg.itemCostItemCount - avatarBreakCfg.leastSameCardCount * breakCfg.sameCardDeductItemCount,
						ItemInfoUtility.GetAssetName(breakCfg.itemCostItemId));
				else
					sameCardTip.Text = GameUtility.FormatUIString(
						"UIPnlAvatarBreakThrough_SameCardItemReduct2",
						breakCfg.sameCardDeductItemCount,
						ItemInfoUtility.GetAssetName(breakCfg.itemCostItemId),
						breakCfg.itemCostItemCount);
			}
			else
				sameCardTip.Text = "";

			// Set Same Card Number Label.
			cadCoinCount.Text = selectedAvatarsCount.ToString();

			// Set Item Icon And Label.
			int itemOwerCount = ItemInfoUtility.GetGameItemCount(breakCfg.itemCostItemId);
			costItemIcon.SetData(breakCfg.itemCostItemId);
			costItemCount.Text = GameUtility.FormatUIString(
				"UIPnlIndiana_Label_Rob2",
				itemOwerCount >= itemRequireCount ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
				itemOwerCount,
				itemRequireCount);
		}


		// Get Require GameMoney And Spirit.
		int gameMoneyRequire = 0;
		int spiritRequire = 0;
		int avatarUpDan = 0;

		foreach (var cost in breakCfg.otherCosts)
		{
			if (cost.id == IDSeg._SpecialId.GameMoney)
				gameMoneyRequire = cost.count;
			else if (cost.id == IDSeg._SpecialId.Spirit)
				spiritRequire = cost.count;
			else if (cost.id == ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan)
				avatarUpDan = cost.count;
		}

		// Set Spirit UI (Icon and Label).
		costItemObject.SetActive(spiritRequire > 0 || avatarUpDan > 0);
		if (spiritRequire > 0)
		{
			cryItemIcon.SetData(IDSeg._SpecialId.Spirit);
			cryItemName.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Cost_Label", ItemInfoUtility.GetAssetName(IDSeg._SpecialId.Spirit));
			cryItemCount.Text = GameUtility.FormatUIString("UIPnlIndiana_Label_Rob2",
						currentPlayer.Spirit >= spiritRequire ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
						currentPlayer.Spirit,
						spiritRequire);
		}

		//Set Shengxingdan UI.
		if (avatarUpDan > 0)
		{
			cryItemIcon.SetData(ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan);
			cryItemName.Text = GameUtility.FormatUIString("UIPnlAvatarBreakThroughTab_Cost_Label", ItemInfoUtility.GetAssetName(ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan));
			cryItemCount.Text = GameUtility.FormatUIString("UIPnlIndiana_Label_Rob2",
						avatarUpDan <= ItemInfoUtility.GetGameItemCount(ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan) ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
						ItemInfoUtility.GetGameItemCount(ConfigDatabase.DefaultCfg.ItemConfig.avatarUpDan),
						avatarUpDan);
		}

		//Set GameMoney Label.
		costCoinCount.Text = GameUtility.FormatUIString("UIPnlIndiana_Label_Rob3",
						currentPlayer.GameMoney >= gameMoneyRequire ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
						gameMoneyRequire);

		// Set Notify.
		activityNotity.Hide(!ItemInfoUtility.IsBreakNotifyActivity_Avatar(avatarLocalData));
	}

	private void SetLift()
	{
		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		avatarLVLabelLift.Text = GameUtility.FormatUIString("UIPnlAvatar_AvatarLevel", avatarLocalData.LevelAttrib.Level,
															GameDefines.txColorWhite.ToString(),
															avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

		avatarLVLabelRight.Text = GameUtility.FormatUIString("UIPnlAvatar_AvatarLevel", avatarLocalData.LevelAttrib.Level,
													avatarCfg.GetMaxBreakthoughtLevel() > avatarLocalData.LevelAttrib.Level ?
													GameDefines.textColorGreen.ToString() : GameDefines.txColorWhite.ToString(),
													avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel + 1) != null ?
													avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel + 1).breakThrough.powerUpLevelLimit : avatarCfg.GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

		avatarHPLabelLift.Text = "0";
		avatarSpeedLabelLift.Text = "0";
		avatarATKLabelLift.Text = "0";
		List<AttributeCalculator.Attribute> attributes = null;
		attributes = PlayerDataUtility.GetAvatarAttributes(avatarLocalData, true);

		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				switch (attributes[i].type)
				{
					case _AvatarAttributeType.MaxHP:
						avatarHPLabelLift.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
						break;
					case _AvatarAttributeType.PAP:
					case _AvatarAttributeType.MAP:
						avatarATKLabelLift.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
						break;
					case _AvatarAttributeType.Speed:
						avatarSpeedLabelLift.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
						break;
				}
			}
		}

		SetCompositeSkillItem(leftCompositeSkillItem, avatarLocalData.BreakthoughtLevel);
	}

	private void SetRight()
	{
		int maxBreakThroughLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetMaxBreakthoughtLevel();

		KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
		avatar.LevelAttrib.Level = avatarLocalData.LevelAttrib.Level;
		avatar.BreakthoughtLevel = avatarLocalData.BreakthoughtLevel >= maxBreakThroughLevel ? avatarLocalData.BreakthoughtLevel : avatarLocalData.BreakthoughtLevel + 1;
		avatar.ResourceId = avatarLocalData.ResourceId;

		List<AttributeCalculator.Attribute> attributes = null;

		avatarHPLabelRight.Text = "0";
		avatarSpeedLabelRight.Text = "0";
		avatarATKLabelRight.Text = "0";

		attributes = PlayerDataUtility.GetAvatarAttributes(avatar, true);

		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				switch (attributes[i].type)
				{
					case _AvatarAttributeType.MaxHP:
						avatarHPLabelRight.Text = string.Format("{0}{1}", avatarLocalData.BreakthoughtLevel == maxBreakThroughLevel ? GameDefines.textColorWhite.ToString() : GameDefines.textColorGreen.ToString(),
																ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
						break;
					case _AvatarAttributeType.PAP:
					case _AvatarAttributeType.MAP:
						avatarATKLabelRight.Text = string.Format("{0}{1}", avatarLocalData.BreakthoughtLevel == maxBreakThroughLevel ? GameDefines.textColorWhite.ToString() : GameDefines.textColorGreen.ToString(),
																ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
						break;
					case _AvatarAttributeType.Speed:
						avatarSpeedLabelRight.Text = string.Format("{0}{1}", avatarLocalData.BreakthoughtLevel == maxBreakThroughLevel ? GameDefines.textColorWhite.ToString() : GameDefines.textColorGreen.ToString(),
																ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value));
						break;
				}
			}
		}

		SetCompositeSkillItem(rightCompositeSkillItem, Mathf.Min(avatarLocalData.BreakthoughtLevel + 1, maxBreakThroughLevel));
	}

	private void SetCompositeSkillItem(UIElemAvatarInfoSkill compositeSkillItem, int breakLevel)
	{
		var avatarCompositeSkill = PlayerDataUtility.GetAvatarCompositeSkill(avatarLocalData.ResourceId, breakLevel);
		compositeSkillItem.Hide(avatarCompositeSkill == null || avatarCompositeSkill.LevelAttrib.Level <= 0);
		compositeSkillItem.SetData(avatarCompositeSkill);
	}

	//获取同名卡
	private List<KodGames.ClientClass.Avatar> GetSameIdAvatars(KodGames.ClientClass.Avatar avatar)
	{
		List<KodGames.ClientClass.Avatar> avatars = new List<KodGames.ClientClass.Avatar>();
		foreach (KodGames.ClientClass.Avatar avatarItem in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (avatarItem.IsAvatar == false || avatar.Guid == avatarItem.Guid || ItemInfoUtility.IsAvatarEquipped(avatarItem) || ItemInfoUtility.IsAvatarCheered(avatarItem))
				continue;

			if (avatarItem.ResourceId == avatar.ResourceId)
				avatars.Add(avatarItem);
		}

		avatars.Sort((d1, d2) =>
		{
			if (d1.BreakthoughtLevel != d2.BreakthoughtLevel)
				return d1.BreakthoughtLevel.CompareTo(d2.BreakthoughtLevel);
			else
				return d1.LevelAttrib.Level.CompareTo(d2.LevelAttrib.Level);
		});

		return avatars;
	}

	//获取已经选择的同名卡
	private List<KodGames.ClientClass.Avatar> GetSelectedAvatars()
	{
		List<KodGames.ClientClass.Avatar> selectedAvatars = new List<KodGames.ClientClass.Avatar>();
		for (int index = 0; index < avatarList.Count; index++)
		{
			var toggleItem = avatarList.GetItem(index).Data as UIElemAvatarSelectToggleItem;
			if (toggleItem.IsSelected && toggleItem.AvatarData != null)
				selectedAvatars.Add(toggleItem.AvatarData);
		}

		return selectedAvatars;
	}

	private void DefaultSelectedAvatars(int selectCount)
	{
		for (int index = 0; index < selectCount; index++)
		{
			var avatarListItem = avatarList.GetItem(index).Data as UIElemAvatarSelectToggleItem;
			if (avatarListItem != null && avatarListItem.AvatarData.LevelAttrib.Level == 1)
				avatarListItem.ToggleState();
		}
	}

	//突破成功
	public void OnBreakThroughSuccess()
	{
		//突破成功tips
		SysUIEnv.Instance.GetUIModule<UIEffectPowerUp>().SetEffectHideCallback(
			(DataCompare) =>
			{
				ShowUI();

				UIPnlAvatarAttributeUpdateDetail.ChangeData data0 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
				data0.Level = avatarLocalData.LevelAttrib.Level;
				data0.BreakthoughtLevel = avatarLocalData.BreakthoughtLevel - 1;
				data0.ResourceId = avatarLocalData.ResourceId;

				UIPnlAvatarAttributeUpdateDetail.ChangeData data1 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
				data1.Level = avatarLocalData.LevelAttrib.Level;
				data1.BreakthoughtLevel = avatarLocalData.BreakthoughtLevel;
				data1.ResourceId = avatarLocalData.ResourceId;

				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarAttributeUpdateDetail), true, UIPnlAvatarAttributeUpdateDetail._UIShowType.BreakThroughDetail, data0, data1);

				float tempAvatarPower = PlayerDataUtility.CalculateAvatarPower(avatarLocalData);
				if (tempAvatarPower > avatarPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempAvatarPower - avatarPower)));
				else if (tempAvatarPower < avatarPower)
					SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(avatarPower - tempAvatarPower)));

				avatarPower = tempAvatarPower;

				if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarLocalData.Guid, avatarLocalData.ResourceId))
				{
					float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
					if (tempPositionPower > positionPower)
						SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
					else if (tempPositionPower < positionPower)
						SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));

					positionPower = tempPositionPower;
				}
			}
		);

		SysUIEnv.Instance.ShowUIModule(typeof(UIEffectPowerUp), avatarLocalData.ResourceId, UIEffectPowerUp.LabelType.Success);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCompositeSkillIcon(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		var skill = assetIcon.Data as KodGames.ClientClass.Skill;

		if (skill != null)
		{
			string compositeSkillInfo = GameUtility.FormatUIString(
				"UIPnlAvatarBreakThrough_CompositeSkillInfo",
				GameDefines.textColorOrange.ToString(),
				ItemInfoUtility.GetAssetName(skill.ResourceId),
				skill.LevelAttrib.Level,
				GameDefines.textColorBtnYellow.ToString(),
				ItemInfoUtility.GetSkillLevelDesc(skill.ResourceId, skill.LevelAttrib.Level));

			UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
			showData.SetData(compositeSkillInfo, true, false);
			SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDetail(UIButton btn)
	{
		int breakThroughLevel = avatarLocalData.BreakthoughtLevel;
		int maxBreakThroughLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetMaxBreakthoughtLevel();

		//详细信息界面
		UIPnlAvatarAttributeUpdateDetail.ChangeData data0 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data0.Level = avatarLocalData.LevelAttrib.Level;
		data0.BreakthoughtLevel = breakThroughLevel;
		data0.ResourceId = avatarLocalData.ResourceId;

		UIPnlAvatarAttributeUpdateDetail.ChangeData data1 = new UIPnlAvatarAttributeUpdateDetail.ChangeData();
		data1.Level = avatarLocalData.LevelAttrib.Level;
		data1.BreakthoughtLevel = breakThroughLevel >= maxBreakThroughLevel ? breakThroughLevel : breakThroughLevel + 1;
		data1.ResourceId = avatarLocalData.ResourceId;

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarAttributeUpdateDetail), false, UIPnlAvatarAttributeUpdateDetail._UIShowType.BreakThroughDetail, data0, data1);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAvatarItemClick(UIButton btn)
	{
		var breakThroughConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).GetAvatarBreakthrough(avatarLocalData.BreakthoughtLevel).breakThrough;
		int sameCardDeductItemCount = breakThroughConfig.sameCardDeductItemCount;
		int itemCostItemCount = breakThroughConfig.itemCostItemCount;
		int costAvatarCount = sameCardDeductItemCount > 0 ? (int)(itemCostItemCount / sameCardDeductItemCount) : 0;

		// Set Selected State.
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		UIElemAvatarSelectToggleItem avatarListItem = assetIcon.Data as UIElemAvatarSelectToggleItem;
		avatarListItem.ToggleState();

		// Ignore Select Operator If Same Card Count is max value.
		List<KodGames.ClientClass.Avatar> selectedAvatars = GetSelectedAvatars();
		if (selectedAvatars.Count > costAvatarCount)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAvatarBreakThroughTab_Tip_SelectMore"));
			avatarListItem.ToggleState();
			return;
		}

		SetBreakThroughCostUI();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickUpdate(UIButton btn)
	{
		var selectAvatars = GetSelectedAvatars();

		var destroyAvatarGuids = new List<string>();
		for (int i = 0; i < selectAvatars.Count; i++)
			destroyAvatarGuids.Add(selectAvatars[i].Guid);

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
		int selectAvatarBreakLevelMax = -1;
		foreach (var avatar in selectAvatars)
		{
			if (SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatar.Guid) == null)
				continue;

			if (selectAvatarBreakLevelMax < avatar.BreakthoughtLevel)
				selectAvatarBreakLevelMax = avatar.BreakthoughtLevel;
		}

		avatarPower = PlayerDataUtility.CalculateAvatarPower(avatarLocalData);
		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarLocalData.Guid, avatarLocalData.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		if (selectAvatarBreakLevelMax < 0)
			RequestMgr.Inst.Request(new AvatarBreakthoughtReq(avatarLocalData.Guid, destroyAvatarGuids));
		else
		{
			string message = string.Empty;
			if (avatarCfg.qualityLevel == 5)
			{
				message = GameUtility.GetUIString("UIDlgMessage_Message_AvatarBreakWarn1");
				if (selectAvatarBreakLevelMax > 0)
					message = GameUtility.FormatUIString("UIDlgMessage_Message_AvatarBreakWarn2", selectAvatarBreakLevelMax, message);
			}
			else if (selectAvatarBreakLevelMax > 0)
				message = GameUtility.GetUIString("UIDlgMessage_Message_AvatarBreakWarn");

			if (string.IsNullOrEmpty(message))
				RequestMgr.Inst.Request(new AvatarBreakthoughtReq(avatarLocalData.Guid, destroyAvatarGuids));
			else
			{
				UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
				string title = GameUtility.GetUIString("UIDlgMessage_Title_Tips");

				MainMenuItem okCallback = new MainMenuItem();
				okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
				okCallback.Callback =
					(data) =>
					{
						RequestMgr.Inst.Request(new AvatarBreakthoughtReq(avatarLocalData.Guid, destroyAvatarGuids));
						return true;
					};

				MainMenuItem cancelCallback = new MainMenuItem();
				cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");
				showData.SetData(title, message, cancelCallback, okCallback);

				SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
			}
		}
	}
}