using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlViewAvatar : UIPnlItemInfoBase
{
	// 排名.
	public SpriteText rankLvlLabel;

	// 角色List.
	public UIScrollList avatarIconList;
	public GameObjectPool avatarIconPool;
	public GameObjectPool splitIconPool;
	public SpriteText powerValue;

	// 上阵UI.
	public GameObject avatarOnRoot;
	public GameObject avatarDetailRoot;
	public UIElemAvatarCard avatarCardIcon;
	public UIElemBreakThroughBtn avatarBreakThougthBtn;
	public UIBox countryImage;
	public SpriteText avatarLvlLabel;
	public UIBox avatarTraitBox;
	public UIButton avatarDetailBtn;
	public SpriteText avatarHPTitle;
	public SpriteText hpLabel;
	public SpriteText avatarATKTitle;
	public SpriteText dpsLabel;
	public SpriteText avatarSpeedTitle;
	public SpriteText speedLabel;
	public GameObject avatarMaker;
	public UIBox avatarModelColorBox;
	public UIScrollList activeSkillList;
	public GameObjectPool activeSkillPool;
	public SpriteText assembleLable;
	public UIButton equipChangeBtn;
	public UIButton skillChangeBtn;
	public UIBox noDefendDanBox;
	public UIBox noDefendBeastBox;	
	public UIButton danChangeBtn;
	public UIButton beastChangeBtn;	
	public UIBox beastBg;
	public UIElemAssetIcon beastIcon;
	public SpriteText beastTips;
	public UIScrollList equipOrSkillList;
	public GameObjectPool equipOrSkillPool;
	public UIElemAvatarCard avatarImage;

	// 小伙伴.
	public GameObject cheerAvatarRoot;
	public List<UIElemAssetIcon> cheerAvatarIcons;
	public UIScrollList cheerAvatarList;
	public GameObjectPool cheerAvatarPool;
	public SpriteText addATKByFriends;
	public SpriteText addHPByFriends;
	public SpriteText addSpeedByFriends;
	public float avatarModelZ;

	private const int C_EQUIP_COUNT = 5;
	private KodGames.ClientClass.Player CurrentPlayer;
	private Avatar avatarModel;
	private int rankLevel;

	private int currentPositionId
	{
		get { return CurrentPlayer.PositionData.ActivePositionId; }
	}
	private int currentShowLocationId;

	private bool IsCheerAvatarOpened
	{
		get { return ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Partner) <= SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level; }
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		this.CurrentPlayer = userDatas[0] as KodGames.ClientClass.Player;
		//不显示排名
		if (userDatas.Length > 1)
			this.rankLevel = (int)userDatas[1];

		InitUI();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	public override void Overlay()
	{
		base.Overlay();
	}

	public override void RemoveOverlay()
	{
		base.RemoveOverlay();

		if (!cheerAvatarRoot.activeInHierarchy)
		{
			if (avatarModel != null)
			{
				var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle, 0);
				if (actionCfg != null)
					avatarModel.PlayAnim(actionCfg.GetAnimationName(avatarModel.AvatarAssetId));
			}
		}
	}

	private void ClearData()
	{
		rankLevel = 0;
		CurrentPlayer = null;

		// Clear AvatarIcon.

		for (int index = 0; index < avatarIconList.Count; index++)
		{
			var avatarIconItem = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;
			if (avatarIconItem == null)
				continue;

			avatarIconItem.ClearData();
		}

		avatarIconList.ClearList(false);
		avatarIconList.ScrollPosition = 0f;

		// Clear activeSkill.
		activeSkillList.ClearList(false);
		activeSkillList.ScrollPosition = 0f;

		// Clear CheerAvatarIcon List.
		cheerAvatarList.ClearList(false);
		cheerAvatarList.ScrollPosition = 0f;

		StopCoroutine("LoadAvatarModel");

		if (avatarModel != null)
			avatarModel.Destroy();
	}

	private void InitUI()
	{
		//blackBg show
		if (SysGameStateMachine.Instance.CurrentState is GameState_Tower)
		{
			if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlMainMenuBot)))
				SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().OnHide();
		}

		//计算阵容战力
		int value = (int)PlayerDataUtility.CalculatePlayerPower(CurrentPlayer, currentPositionId);

		powerValue.Text = GameUtility.FormatUIString("UIPnlAvatar_Label_Power", PlayerDataUtility.GetPowerString(value));

		// Init Rank Level.
		if (rankLevel > 0)
			rankLvlLabel.Text = GameUtility.FormatUIString("UIPnlViewAvatar_RankLevel", rankLevel, CurrentPlayer.Name);
		else
			rankLvlLabel.Text = GameUtility.FormatUIString("UIPnlViewAvatar_NormalLevel", CurrentPlayer.Name); ;
		// Show Position UI.
		ShowAvatarOnUI();
		SetAvatarInfo();
	}

	#region  AvatarOn UI

	private int SetAvatarInfo()
	{
		for (int i = 0; i < avatarIconList.Count; i++)
		{
			UIListItemContainer container = avatarIconList.GetItem(i) as UIListItemContainer;
			UIElemLineUpAvatar avatarElem = container.Data as UIElemLineUpAvatar;

			if (avatarElem == null)
				continue;

			if (avatarElem.avatarIcon.Data is KodGames.ClientClass.Location)
			{
				var avatarLocation = avatarElem.avatarIcon.Data as KodGames.ClientClass.Location;
				SetAvatarControls(avatarLocation.ShowLocationId);

				return container.Index;
			}
		}
		return 0;
	}

	private void ShowAvatarOnUI()
	{
		// Clear AvatarIconList.
		avatarIconList.ClearList(false);

		// Get the local avatar list.
		List<KodGames.ClientClass.Location> avartarLocations = PlayerDataUtility.GetAvatarLocations(CurrentPlayer, this.currentPositionId);

		//Sort by battle position.
		avartarLocations.Sort(DataCompare.CompareLocationByShowPos);

		var positionCfg = ConfigDatabase.DefaultCfg.PositionConfig.GetPositionById(currentPositionId);
		int maxLineUpAvatarsCount = ConfigDatabase.DefaultCfg.GameConfig.maxColumnInFormation * ConfigDatabase.DefaultCfg.GameConfig.maxRowInFormation;
		int employIndexPos = PlayerDataUtility.GetIndexPosByBattlePos(CurrentPlayer.PositionData.GetPositionById(currentPositionId).EmployShowLocationId);

		for (int index = 0; index < maxLineUpAvatarsCount; index++)
		{
			UIElemLineUpAvatar item = null;

			if (avatarIconList.Count <= index)
			{
				UIListItemContainer itemContainer = avatarIconPool.AllocateItem().GetComponent<UIListItemContainer>();
				item = itemContainer.gameObject.GetComponent<UIElemLineUpAvatar>();
				itemContainer.Data = item;
			}
			else
				item = avatarIconList.GetItem(index).Data as UIElemLineUpAvatar;

			bool isOpen = CurrentPlayer.LevelAttrib.Level >= positionCfg.PositionNums[index].Level;

			KodGames.ClientClass.Location avatarLocation = null;
			for (int i = 0; i < avartarLocations.Count; i++)
			{
				if (avartarLocations[i].ShowLocationId == PlayerDataUtility.GetBattlePosByIndexPos(index))
				{
					avatarLocation = avartarLocations[i];
					break;
				}
			}

			// Add Split Icon.
			if (index == employIndexPos)
				avatarIconList.AddItem(splitIconPool.AllocateItem());

			if (avatarLocation != null)
				item.SetData(avatarLocation, this, "OnClickAvatarIcon");	// Set avatar
			else
				item.SetData(index, isOpen, index == employIndexPos, null, null);

			avatarIconList.AddItem(item.gameObject);
		}

		// If CheerAvatar Opened ,add cheer Icon.
		if (IsCheerAvatarOpened)
		{
			UIListItemContainer itemContainer = avatarIconPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemLineUpAvatar item = itemContainer.gameObject.GetComponent<UIElemLineUpAvatar>();
			item.avatarIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerBtn, string.Empty);
			itemContainer.Data = item;

			item.SetData(this, "OnClickCheerAvatar");
			item.Index = avatarIconList.Count + 1;
			avatarIconList.AddItem(itemContainer);
		}

		// Show First Item.
		UIElemLineUpAvatar lineUpAvatar = avatarIconList.GetItem(0).Data as UIElemLineUpAvatar;
		SetAvatarControls(lineUpAvatar.avatarIcon.Data is KodGames.ClientClass.Location ? (lineUpAvatar.avatarIcon.Data as KodGames.ClientClass.Location).ShowLocationId : 0);
	}

	private void SetAvatarControls(int avatarShowLocationId)
	{
		this.currentShowLocationId = avatarShowLocationId;
		var location = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, avatarShowLocationId);
		var avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, avatarShowLocationId);

		// Init View State.
		avatarOnRoot.SetActive(true);
		avatarDetailRoot.SetActive(avatar != null);
		cheerAvatarRoot.SetActive(false);

		// clear list.
		activeSkillList.ClearList(false);

		// Set Equip Or Skill Data.
		SetEquipOrSkillUI(SelectChange.Equipment);

		// Set Light.
		SetLight(PlayerDataUtility.GetIndexPosByBattlePos(location != null ? location.ShowLocationId : avatarShowLocationId));

		// Set avatar UI.
		if (avatar != null)
		{
			// Load Avatar Model.
			StartCoroutine("LoadAvatarModel", avatar);

			AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

			// Breakthrough
			avatarBreakThougthBtn.SetBreakThroughIcon(avatar.BreakthoughtLevel);

			// Set Model Color.
			switch (avatarConfig.qualityLevel)
			{
				case 3:
					avatarModelColorBox.SetState(0);
					break;
				case 4:
					avatarModelColorBox.SetState(1);
					break;
				case 5:
					avatarModelColorBox.SetState(2);
					break;
			}

			// Set AvatarCardIcon.
			avatarCardIcon.SetData(avatarConfig.id, false, false, null);

			//设置国家image
			UIElemTemplate.Inst.SetAvatarCountryIcon(countryImage, ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).countryType);

			// Set Avatar Trait icon.
			UIElemTemplate.Inst.SetAvatarTraitIcon(avatarTraitBox, avatarConfig.traitType);

			// Set Avatar Level.
			avatarLvlLabel.Text = GameUtility.FormatUIString(
											"UIPnlAvatar_AvatarLevel",
											avatar.LevelAttrib.Level,
											GameDefines.textColorInOrgYew.ToString(),
											avatarConfig.GetAvatarBreakthrough(avatar.BreakthoughtLevel).breakThrough.powerUpLevelLimit);

			// Avatar Attribute.
			UpdateAvatarAttribute();

			// Avatar Assemble.
			string assembleDesc = string.Empty;
			for (int i = 0; i < avatarConfig.assemableIds.Count; i++)
			{
				SuiteConfig.AssembleSetting assembleSetting = ConfigDatabase.DefaultCfg.SuiteConfig.GetAssembleSettingById(avatarConfig.assemableIds[i]);
				if (assembleSetting == null)
					continue;

				bool isAssembleActive = PlayerDataUtility.CheckAvatarAssemble(assembleSetting, avatar, CurrentPlayer, currentPositionId, avatarShowLocationId);
				assembleDesc += string.Format(GameUtility.GetUIString("UIDlgViewAvatar_YuanFen"), isAssembleActive ? GameDefines.textColorGreen : GameDefines.textColorInBlack, assembleSetting.Name);
			}
			assembleLable.Text = assembleDesc;

			// Avatar Active skill.
			activeSkillList.ClearList(false);
			foreach (var activeSkill in PlayerDataUtility.GetAvatarActiveSkill(avatar.ResourceId, avatar.BreakthoughtLevel))
			{
				var skillItem = activeSkillPool.AllocateItem().GetComponent<UIElemAvatarInfoSkill>();
				skillItem.SetData(activeSkill);

				activeSkillList.AddItem(skillItem.gameObject);
			}

			// Avatar detail
			avatarDetailBtn.Data = avatar;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator LoadAvatarModel(KodGames.ClientClass.Avatar avatar)
	{
		yield return null;

		if (avatarModel != null)
			avatarModel.Destroy();

		var avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId);

		avatarModel = Avatar.CreateAvatar(avatarCfg.id);
		int avatarAssetId = IDSeg.InvalidId;
		if (avatarCfg.id != IDSeg.InvalidId)
		{
			avatarAssetId = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarCfg.id).GetAvatarAssetId(avatar.BreakthoughtLevel);
			// Load avatar.
			if (avatarModel.Load(avatarAssetId, false, true) == false)
				yield break;
		}

		// Set to current layer.
		avatarModel.SetGameObjectLayer(GameDefines.AvatarCaptureLayer);

		// Put to mount bone.
		ObjectUtility.AttachToParentAndKeepLocalTrans(avatarMaker, avatarModel.gameObject);

		avatarMaker.transform.localPosition = new Vector3(avatarMaker.transform.localPosition.x, avatarMaker.transform.localPosition.y, avatarModelZ);
		avatarMaker.transform.localRotation = new Quaternion(avatarMaker.transform.localRotation.x, 180f, avatarMaker.transform.localRotation.z, avatarMaker.transform.localRotation.w);

		//Play Idle animation.
		var actionCfg = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle, 0);
		if (actionCfg != null)
		{
			avatarModel.PlayAnim(actionCfg.GetAnimationName(avatarAssetId));

			//添加武器
			foreach (AvatarConfig.WeaponAsset weaponAsset in avatarCfg.showWeaponAssets)
				avatarModel.UseComponent(weaponAsset.avatarAssetId, weaponAsset.mountBone);
		}

	}

	private void UpdateAvatarAttribute()
	{
		var attributes = PlayerDataUtility.GetLocationAvatarAttributes(PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId), CurrentPlayer);

		// Set Attribute label.
		avatarHPTitle.Text = GameUtility.GetUIString("AvatarAttribute_HP");
		avatarATKTitle.Text = GameUtility.GetUIString("AvatarAttribute_AP");
		avatarSpeedTitle.Text = GameUtility.GetUIString("AvatarAttribute_Speed");

		hpLabel.Text = "0";
		speedLabel.Text = "0";
		dpsLabel.Text = "0";

		for (int i = 0; i < attributes.Count; i++)
		{
			switch (attributes[i].type)
			{
				case _AvatarAttributeType.Speed:
					speedLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
					break;

				case _AvatarAttributeType.MAP:
				case _AvatarAttributeType.PAP:
					dpsLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
					break;

				case _AvatarAttributeType.MaxHP:
					hpLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
					break;
			}
		}
	}

	private void SetEquipOrSkillUI(SelectChange selectType)
	{
		equipChangeBtn.controlIsEnabled = selectType != SelectChange.Equipment;
		skillChangeBtn.controlIsEnabled = selectType != SelectChange.Skill;

		if (ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.DanHome) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
		{
			danChangeBtn.controlIsEnabled = false;
			if (noDefendDanBox != null)
				noDefendDanBox.Hide(false);
		}
		else
		{
			danChangeBtn.controlIsEnabled = selectType != SelectChange.Dan;
			if (noDefendDanBox != null)
				noDefendDanBox.Hide(true);
		}

		if (ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(_OpenFunctionType.Beast) > SysLocalDataBase.Inst.LocalPlayer.LevelAttrib.Level)
		{
			beastChangeBtn.controlIsEnabled = false;
			if (noDefendBeastBox != null)
				noDefendBeastBox.Hide(false);
		}
		else
		{
			beastChangeBtn.controlIsEnabled = selectType != SelectChange.Beast;
			if (noDefendBeastBox != null)
				noDefendBeastBox.Hide(true);
		}

		// Clear bottom list.
		equipOrSkillList.ClearList(false);
		equipOrSkillList.ScrollPosition = 0f;

		beastBg.gameObject.SetActive(selectType == SelectChange.Beast);

		// Set Weapon and Skills.
		switch (selectType)
		{
			case SelectChange.Equipment:
				var equipLocations = PlayerDataUtility.GetEquipmentLocations(CurrentPlayer, currentPositionId, currentShowLocationId);
				equipLocations.Sort((l1, l2) =>
				{
					var e1 = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(l1.ResourceId);
					var e2 = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(l2.ResourceId);

					return e1.type - e2.type;
				});

				for (int i = 0; i < C_EQUIP_COUNT; i++)
				{
					UIElemAvatarBottomItem item = equipOrSkillPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
					item.SetTriggerMethod(this, "OnClickChangeWeapon");

					int equipType = EquipmentConfig._Type.Weapon + i;
					KodGames.ClientClass.Location location = null;

					int j = 0;
					for (; j < equipLocations.Count; j++)
					{
						var equipConfig = ConfigDatabase.DefaultCfg.EquipmentConfig.GetEquipmentById(equipLocations[j].ResourceId);

						if (equipConfig.type == equipType)
						{
							location = equipLocations[j];
							break;
						}
					}

					if (j >= equipLocations.Count)
					{
						location = new KodGames.ClientClass.Location();
						location.PositionId = currentPositionId;
						location.ResourceId = IDSeg.InvalidId;
						location.ShowLocationId = currentShowLocationId;
						location.Guid = string.Empty;
					}

					item.SetData(CurrentPlayer, location, IDSeg._AssetType.Equipment, equipType);
					equipOrSkillList.AddItem(item.container);
				}
				break;
			case SelectChange.Skill:
				var skillLocations = PlayerDataUtility.GetSkillLocations(CurrentPlayer, currentPositionId, currentShowLocationId);

				for (int i = 0; i < C_EQUIP_COUNT; i++)
				{
					UIElemAvatarBottomItem item = equipOrSkillPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
					// Set Invoke Method.
					item.SetTriggerMethod(this, "OnClickChangePassiveSkill");

					KodGames.ClientClass.Location location = null;
					if (i < skillLocations.Count)
						location = skillLocations[i];
					else
					{
						location = new KodGames.ClientClass.Location();
						location.PositionId = currentPositionId;
						location.ResourceId = IDSeg.InvalidId;
						location.ShowLocationId = currentShowLocationId;
						location.Guid = string.Empty;
					}

					item.SetData(CurrentPlayer, location, IDSeg._AssetType.CombatTurn, i);
					equipOrSkillList.AddItem(item.container);
				}
				break;
			case SelectChange.Dan:
				var danLocations = PlayerDataUtility.GetDanLocations(CurrentPlayer, currentPositionId, currentShowLocationId);
				danLocations.Sort((l1, l2) =>
				{
					var d1 = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(l1.ResourceId);
					var d2 = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(l2.ResourceId);

					return d1.Type - d2.Type;
				});

				for (int i = 0; i < C_EQUIP_COUNT; i++)
				{
					UIElemAvatarBottomItem item = equipOrSkillPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
					item.SetTriggerMethod(this, "OnClickChangeSearchDan");

					int danType = DanConfig._DanType.GetRegisterTypeByIndex(i);
					KodGames.ClientClass.Location location = null;

					int j = 0;
					for (; j < danLocations.Count; j++)
					{
						var danConfig = ConfigDatabase.DefaultCfg.DanConfig.GetDanById(danLocations[j].ResourceId);
						if (danConfig.Type == danType && CurrentPlayer.SearchDan(danLocations[j].Guid) != null)
						{
							location = danLocations[j];
							break;
						}
					}

					if (j >= danLocations.Count)
					{
						location = new KodGames.ClientClass.Location();
						location.PositionId = currentPositionId;
						location.ResourceId = IDSeg.InvalidId;
						location.ShowLocationId = currentShowLocationId;
						location.Guid = string.Empty;
					}

					item.SetData(CurrentPlayer, location, IDSeg._AssetType.Dan, danType);
					equipOrSkillList.AddItem(item.container);
				}			
				break;
			case SelectChange.Beast:
				var beastLocations = PlayerDataUtility.GetBeastLocations(CurrentPlayer, currentPositionId, currentShowLocationId);

				KodGames.ClientClass.Location location = null;
				beastIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
				beastTips.Text = GameUtility.GetUIString("UIPnlOrganInfo_Avatar_Attribute_Default");

				if (beastLocations != null && beastLocations.Count > 0)
				{
					if (CurrentPlayer.SearchBeast(beastLocations[0].Guid) == null)
					{
						beastIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconAddBgBtn, string.Empty);
					}
					else
					{
						var beastShow = CurrentPlayer.SearchBeast(beastLocations[0].Guid);
						var beastBaseInfo = ConfigDatabase.DefaultCfg.BeastConfig.GetBaseInfoByBeastId(beastShow.ResourceId);

						beastIcon.SetData(beastShow);
						var beastAvatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, currentShowLocationId);

						beastTips.Text = string.Format(GameUtility.GetUIString("UIPnlOrganInfo_Avatar_Attribute"), beastBaseInfo.BeastName, ItemInfoUtility.GetAssetName(beastAvatar.ResourceId));
					}
					location = beastLocations[0];
				}

				if (location == null)
				{
					location = new KodGames.ClientClass.Location();
					location.PositionId = currentPositionId;
					location.ResourceId = IDSeg.InvalidId;
					location.ShowLocationId = currentShowLocationId;
					location.Index = 0;
					location.Guid = string.Empty;
				}

				beastIcon.Data = location;
				break;
		}

		equipOrSkillList.ScrollToItem(0, 0f);
	}

	public void SetLight(int avatarIconIndex)
	{
		SetLight(avatarIconIndex, true);
	}

	public void SetLight(int avatarIconIndex, bool controllClip)
	{
		for (int i = 0; i < avatarIconList.Count; i++)
		{
			UIListItemContainer container = avatarIconList.GetItem(i) as UIListItemContainer;
			UIElemLineUpAvatar avatarElem = (UIElemLineUpAvatar)container.Data;

			if (avatarElem == null)
				continue;

			int index = -1;
			if (avatarElem.avatarIcon.Data is KodGames.ClientClass.Location)
				index = PlayerDataUtility.GetIndexPosByBattlePos((avatarElem.avatarIcon.Data as KodGames.ClientClass.Location).ShowLocationId);
			else if (avatarElem.avatarIcon.Data is int)
				index = (int)avatarElem.avatarIcon.Data;

			if (index == avatarIconIndex)
			{
				avatarElem.SetSelectedStat(true);

				if (controllClip)
				{
					if (avatarElem.avatarIcon.GetComponent<UIButton>().Clipped)
						avatarIconList.ScrollToItem(i, 0.3f, EZAnimation.EASING_TYPE.Linear);
				}
				else
					avatarIconList.ScrollToItem(i, 0f, EZAnimation.EASING_TYPE.Linear);
			}
			else
				avatarElem.SetSelectedStat(false);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarDetail(UIButton btn)
	{
		var avatarLocation = PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId);
		var attributes = PlayerDataUtility.GetLocationAvatarAttributes(avatarLocation, CurrentPlayer, false);

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAttributeDetailTip), attributes, CurrentPlayer.SearchAvatar(avatarLocation.Guid), true, currentPositionId, currentShowLocationId, CurrentPlayer);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCheerAvatar(UIButton btn)
	{
		InitPartnerUI();
		SetLight(avatarIconList.Count - 2);
		CaculateAddPropertyByFriends();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var avatarLocation = assetIcon.Data as KodGames.ClientClass.Location;
		SetAvatarControls(avatarLocation.ShowLocationId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeEquip(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Equipment);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeSkill(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Skill);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeDan(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Dan);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeBeast(UIButton btn)
	{
		SetEquipOrSkillUI(SelectChange.Beast);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeWeapon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		List<object> datas = assetIcon.Data as List<object>;
		var location = datas[1] as KodGames.ClientClass.Location;

		if (!string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlEquipmentInfo, location, false, true, false, false, null, true, CurrentPlayer);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeSearchDan(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		List<object> datas = assetIcon.Data as List<object>;

		//int danType = (int)datas[0];
		var location = datas[1] as KodGames.ClientClass.Location;

		//var avatar = PlayerDataUtility.GetLineUpAvatar(CurrentPlayer, currentPositionId, location.ShowLocationId);
		if (!string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule<UIPnlDanInfo>(CurrentPlayer.SearchDan(location.Guid), -1, false, false, true, false, false);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangePassiveSkill(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		List<object> datas = assetIcon.Data as List<object>;

		int skillIndex = (int)datas[0];
		var location = datas[1] as KodGames.ClientClass.Location;

		if (!string.IsNullOrEmpty(location.Guid))
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, location, false, true, false, false, null, true, skillIndex, CurrentPlayer);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeBeastBtn(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var location = assetIcon.Data as KodGames.ClientClass.Location;

		KodGames.ClientClass.Beast playerBeast= CurrentPlayer.SearchBeast(location.Guid);
		if(playerBeast != null)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlOrganInfo), playerBeast, true);	
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarCard(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarInfo), PlayerDataUtility.GetAvatarLocation(CurrentPlayer, currentPositionId, currentShowLocationId), false, true, false, false, null, CurrentPlayer, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickActiveSkillIcon(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var skill = assetIcon.Data as KodGames.ClientClass.Skill;
		if (skill != null)
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, skill, false, true, false, false, null, false);
	}

	#endregion

	#region CheerAvatar
	private void InitPartnerUI()
	{
		// InActive avatarCrontroll .
		avatarOnRoot.SetActive(false);
		cheerAvatarRoot.SetActive(true);

		// Init Partner. 
		var partners = ConfigDatabase.DefaultCfg.PartnerConfig.Partners;
		var dic_partners = GetPartnerAvatars();

		// Set partner icon.
		for (int i = 0; i < partners.Count; i++)
		{
			cheerAvatarIcons[i].Data = partners[i].PartnerId;
			cheerAvatarIcons[i].Hide(!partners[i].IsOpen);

			if (!partners[i].IsOpen)
				continue;

			KodGames.ClientClass.Partner partner = null;
			if (dic_partners.ContainsKey(partners[i].PartnerId))
				partner = dic_partners[partners[i].PartnerId];

			SetPartnerIcon(cheerAvatarIcons[i], partner);
		}

		// Set CheerAvatar Icon.
		cheerAvatarList.ClearList(false);
		cheerAvatarList.ScrollPosition = 0f;

		for (int i = 0; i < partners.Count; i++)
		{
			if (!partners[i].IsOpen)
				continue;

			UIElemAvatarBottomItem item = cheerAvatarPool.AllocateItem().GetComponent<UIElemAvatarBottomItem>();
			item.assetIcon.Data = partners[i].PartnerId;

			KodGames.ClientClass.Partner partner = null;
			if (dic_partners.ContainsKey(partners[i].PartnerId))
				partner = dic_partners[partners[i].PartnerId];

			SetPartnerBottomIcon(item, partner);

			cheerAvatarList.AddItem(item.container);
		}

		SnapToPartner(partners[0].PartnerId);
	}

	private void SetPartnerIcon(UIElemAssetIcon partIcon, KodGames.ClientClass.Partner partner)
	{
		int partnerId = (int)partIcon.Data;

		Color whiteColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1.0f);
		Color whiteAlphaColor = new Color(255f / 255f, 255f / 255f, 255f / 255f, 0.5f);

		if (partner == null)
		{
			partIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerAvatarClose, string.Empty);
			partIcon.assetNameLabel.SetColor(whiteAlphaColor);
		}
		else if (string.IsNullOrEmpty(partner.AvatarGuid))
		{
			partIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerAvatarActive, string.Empty);
			partIcon.assetNameLabel.SetColor(whiteAlphaColor);
		}
		else
		{
			partIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCheerAvatarOpen, string.Empty);
			partIcon.assetNameLabel.SetColor(whiteColor);
		}

		partIcon.SetTriggerMethod(this, "OnClickPartnerIconForShowDesc");
		partIcon.Text = ItemInfoUtility.GetAssetName(partnerId).Substring(0, 1);
	}

	private void SetPartnerBottomIcon(UIElemAvatarBottomItem item, KodGames.ClientClass.Partner partner)
	{
		int partnerId = (int)item.assetIcon.Data;

		if (partner == null)
		{
			item.assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, GameUtility.FormatUIString("UIPnlAvatarPartner_Level", ConfigDatabase.DefaultCfg.PartnerConfig.GetPartnerById(partnerId).RequirePlayerLevel));
			item.SetTriggerMethod(this, "OnClickPartnerIconForShowDesc");
		}
		else
		{
			if (string.IsNullOrEmpty(partner.AvatarGuid))
				item.assetIcon.SetEmpty(UIElemTemplate.Inst.iconBorderTemplate.iconCardEmpty, ItemInfoUtility.GetAssetName(partnerId));
			else
				item.assetIcon.SetData(CurrentPlayer.SearchAvatar(partner.AvatarGuid));

			item.SetTriggerMethod(this, "OnClickPartnerIconForChangeAvatar");
		}

		item.SetSelectedStat(false);
	}

	private void SnapToPartner(int partnerId)
	{
		for (int i = 0; i < cheerAvatarList.Count; i++)
		{
			UIElemAvatarBottomItem item = cheerAvatarList.GetItem(i).Data as UIElemAvatarBottomItem;

			if (item == null)
				continue;

			item.SetSelectedStat((int)item.assetIcon.Data == partnerId);

			if ((int)item.assetIcon.Data == partnerId)
			{
				item.SetSelectedStat(true);
				cheerAvatarList.ScrollToItem(i, 0.3f, EZAnimation.EASING_TYPE.Linear);
			}
			else
				item.SetSelectedStat(false);
		}
	}

	private Dictionary<int, KodGames.ClientClass.Partner> GetPartnerAvatars()
	{
		Dictionary<int, KodGames.ClientClass.Partner> dic_partners = new Dictionary<int, KodGames.ClientClass.Partner>();

		var partners = CurrentPlayer.PositionData.GetPositionById(currentPositionId).Partners;
		for (int i = 0; i < partners.Count; i++)
		{
			if (dic_partners.ContainsKey(partners[i].PartnerId))
				continue;

			dic_partners.Add(partners[i].PartnerId, partners[i]);
		}

		return dic_partners;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPartnerIconForShowDesc(UIButton btn)
	{
		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		int partnerId = (int)assetIcon.Data;

		// Snap Partner Icon.
		SnapToPartner(partnerId);

		// Show Partner Description.
		string tips = ItemInfoUtility.GetAssetDesc(partnerId);
		UIPnlTip.ShowData showData = new UIPnlTip.ShowData();
		showData.SetData(tips, true, false);
		SysUIEnv.Instance.GetUIModule<UIPnlTip>().ShowTip(showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPartnerIconForChangeAvatar(UIButton btn)
	{
		int partnerId = (int)(btn.Data as UIElemAssetIcon).Data;

		// Snap Partner Icon.
		SnapToPartner(partnerId);

		var partner = CurrentPlayer.PositionData.GetPositionById(currentPositionId).GetPartnerById(partnerId);

		KodGames.ClientClass.Partner tempPartner = new KodGames.ClientClass.Partner();
		tempPartner.PartnerId = partnerId;
		tempPartner.AvatarGuid = partner == null ? string.Empty : partner.AvatarGuid;
		tempPartner.PositionId = currentPositionId;

		if (!string.IsNullOrEmpty(tempPartner.AvatarGuid))
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlAvatarInfo), tempPartner, false, true, false, false, null, CurrentPlayer);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickCheerAvatarDetail(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAvatarCheerDetail), CurrentPlayer, currentPositionId);
	}

	public void CaculateAddPropertyByFriends()
	{
		double atkProperty = 0;
		double hpProperty = 0;
		double speedProperty = 0;

		var parterAvatars = PlayerDataUtility.GetCheerCalculatorAvatars(CurrentPlayer, currentPositionId);

		for (int i = 0; i < parterAvatars.Count; i++)
		{
			var attributes = PlayerDataUtility.GetAvatarAttributesForAssistant(parterAvatars[i]);

			for (int j = 0; j < attributes.Count; j++)
			{
				var attrib = attributes[j];

				switch (attrib.type)
				{
					case _AvatarAttributeType.Speed:
						speedProperty += attrib.value;
						break;
					case _AvatarAttributeType.MaxHP:
						hpProperty += attrib.value;
						break;
					case _AvatarAttributeType.MAP:
					case _AvatarAttributeType.PAP:
						atkProperty += attrib.value;
						break;
				}
			}
		}

		addATKByFriends.Text = atkProperty.ToString();
		addHPByFriends.Text = hpProperty.ToString();
		addSpeedByFriends.Text = speedProperty.ToString();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	public void OnClick3DAvatar(UIButton btn)
	{
		var actionCfgSelected = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.SelectRole, 0);
		var actionCfgIdle = ConfigDatabase.DefaultCfg.ActionConfig.GetActionInTypeByIndex(EquipmentConfig._WeaponType.Empty, _CombatStateType.Default, AvatarAction._Type.Idle, 0);

		string idleAnimationName = actionCfgIdle.GetAnimationName(avatarModel.AvatarAssetId);
		string selectAnimationName = actionCfgSelected.GetAnimationName(avatarModel.AvatarAssetId);

		//AvatarConfig.Avatar avatar = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarModel.AvatarId);

		//播放点击后的动画
		if (actionCfgSelected != null && !avatarModel.transform.GetComponentInChildren<Animation>().IsPlaying(selectAnimationName))
		{
			avatarImage.StopVoice();
			avatarModel.PlayAnim(selectAnimationName);

			avatarImage.SetData(avatarModel.AvatarId, true, true, null);
		}
		//播放idle动画（还原）
		avatarModel.SetAnimationFinishDeletage(
			(e1, e2) =>
			{
				if (actionCfgIdle != null && !avatarModel.transform.GetComponentInChildren<Animation>().IsPlaying(idleAnimationName))
				{
					avatarModel.PlayAnim(idleAnimationName);
					//avatarImage.SetData(avatarModel.AvatarId, true, true, null);
				}
			}, idleAnimationName, selectAnimationName
			);
	}
	#endregion
}
