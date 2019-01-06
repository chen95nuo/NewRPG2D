using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarInfo : UIPnlItemInfoBase
{
	public delegate void SelectDelegate(KodGames.ClientClass.Avatar selected);

	//Avatar base prop
	public SpriteText titleLabel;
	public UIElemBreakThroughBtn avatarBreakProgress;

	//TraitType
	public UIBox AvatarGrowthBtn;

	//Avatar model part
	public UIElemAvatarCard avatarCard;
	public UIBox avatarCountryBox;

	//Avatar detail part
	public UIButton avatarDetailBtn;
	public SpriteText avatarHPTitle, avatarATKTitle, avatarSpeedTitle;
	public SpriteText avatarHPLabel;
	public SpriteText avatarATKLabel;
	public SpriteText avatarSpeedLabel;
	public SpriteText avatarLVLabel;

	//Avatar trait desc part: Domineer|Assemble description
	public UIScrollList avatarTraitList;
	public GameObjectPool avatarTraitItemPool;

	//Avatar action buttons part
	public UIChildLayoutControl actionButtonLayout;

	//Skill
	public UIScrollList SkillList;
	public GameObjectPool SkillIconPool;

	//Buttons
	public UIButton changeAvatarBtn;
	public UIButton bigCloseBtn;
	public UIButton gotoPackageBtn;
	public UIButton powerUpAvatarBtn;
	public UIButton selectBtn;
	public UIButton prevBtn;
	public UIButton nextBtn;
	public UIBox activityNotify;
	//Local avatarData data.
	private KodGames.ClientClass.Avatar avatarData;
	private KodGames.ClientClass.Location avatarLocation;
	private KodGames.ClientClass.Partner avatarPartner;
	private KodGames.ClientClass.Player currentPlayer;

	//When click "Select"
	private SelectDelegate selectDel;

	private readonly Color AVATAR_TRAITDESC_TXCOLOR_BROWN = GameDefines.textColorInBlack;
	private readonly Color AVATAR_TRAITDESC_TXCOLOR_GREEN = GameDefines.txColorGreen;

	private bool isCardPic;
	private bool isScroll;
	private bool showPositionPower = false;
	private bool showPowerUp;//显示能不能洗练霸气

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		currentPlayer = SysLocalDataBase.Inst.LocalPlayer;

		if (userDatas[0] is AvatarConfig.Avatar)
		{
			AvatarConfig.Avatar avatarCfg = userDatas[0] as AvatarConfig.Avatar;
			isCardPic = userDatas.Length > 1 ? (bool)userDatas[1] : false;

			if (isCardPic)
			{
				List<AvatarConfig.Avatar> cardPicAvatars = GetCardPictureAvatars();
				int index = cardPicAvatars.IndexOf(avatarCfg);
				SetCardPicAvatarUI(index, cardPicAvatars.Count, true);
			}
			else
				SetCardPicAvatarUI(avatarCfg, 0, 0, false);
		}
		else if (userDatas[0] is KodGames.ClientClass.Avatar)
		{
			avatarData = userDatas[0] as KodGames.ClientClass.Avatar;
			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			showPowerUp = (bool)userDatas[4];
			selectDel = userDatas[5] as SelectDelegate;

			// Show action button
			ShowActionButtons(showChange, showBigClose, showGotoPackage, showPowerUp, selectDel != null, false, false);

			FillData();
		}
		else if (userDatas[0] is KodGames.ClientClass.Location)
		{
			avatarLocation = userDatas[0] as KodGames.ClientClass.Location;
			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			selectDel = userDatas[5] as SelectDelegate;

			if (userDatas.Length > 6)
			{
				if (userDatas[6] is bool)
					showPositionPower = (bool)userDatas[6];
				else if (userDatas[6] is KodGames.ClientClass.Player)
					currentPlayer = userDatas[6] as KodGames.ClientClass.Player;
			}

			if (userDatas.Length > 7)
			{
				if (userDatas[7] is bool)
					showPositionPower = (bool)userDatas[7];
			}

			avatarData = currentPlayer.SearchAvatar(avatarLocation.Guid);
			showPowerUp = avatarData.IsAvatar ? (bool)userDatas[4] : false;

			// Show action button
			ShowActionButtons(showChange, showBigClose, showGotoPackage, showPowerUp, selectDel != null, false, false);

			FillData();
		}
		else if (userDatas[0] is KodGames.ClientClass.Partner)
		{
			avatarPartner = userDatas[0] as KodGames.ClientClass.Partner;
			bool showChange = (bool)userDatas[1];
			bool showBigClose = (bool)userDatas[2];
			bool showGotoPackage = (bool)userDatas[3];
			showPowerUp = (bool)userDatas[4];
			selectDel = userDatas[5] as SelectDelegate;

			if (userDatas.Length > 6)
				currentPlayer = userDatas[6] as KodGames.ClientClass.Player;

			avatarData = currentPlayer.SearchAvatar(avatarPartner.AvatarGuid);

			// Show action button
			ShowActionButtons(showChange, showBigClose, showGotoPackage, showPowerUp, selectDel != null, false, false);

			FillData();
		}
		else if (userDatas[0] is int)
		{
			int avatarResourceId;

			if (IDSeg.ToAssetType((int)userDatas[0]) != IDSeg._AssetType.Avatar)
			{
				avatarResourceId = ConfigDatabase.DefaultCfg.IllustrationConfig.GetIllustrationByFragmentId((int)userDatas[0]).Id;
				this.isScroll = true;
			}
			else
				avatarResourceId = (int)userDatas[0];
			AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarResourceId);
			if (avatarCfg == null)
				return true;


			SetCardPicAvatarUI(avatarCfg, 0, 0, false);
		}

		return true;
	}

	public override void OnHide()
	{
		ClearList();
		ClearData();
		base.OnHide();
	}

	private void ClearList()
	{
		avatarCard.Clear();
		avatarTraitList.ClearList(false);
		SkillList.ClearList(false);
	}

	private void ClearData()
	{
		avatarData = null;
		avatarLocation = null;
		avatarPartner = null;
		currentPlayer = null;
		selectDel = null;
		isCardPic = false;
		isScroll = false;
		showPowerUp = false;
		showPositionPower = false;
	}

	private void FillData()
	{
		//When glancing over cards,data need to be cleared.
		ClearList();

		AvatarConfig.Avatar avatarCfg = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId);
		if (avatarCfg == null)
		{
			Debug.LogWarning("Lose Avatar config with Id : " + avatarData.ResourceId.ToString("X"));
			return;
		}

		if (isScroll)
			titleLabel.Text = GameUtility.GetUIString("UIDlgAvatarInfo_ScrollTitle");
		else
			titleLabel.Text = GameUtility.GetUIString("UIDlgAvatarInfo_Title");

		// Set BreakThought.
		avatarBreakProgress.SetBreakThroughIcon(avatarData.BreakthoughtLevel);

		// Avatar card, 加载完成之前隐藏卡牌控件
		avatarCard.SetData(avatarData.ResourceId, false, false, null);

		int currentAvatarLevel = avatarData.LevelAttrib.Level;
		int maxAvatarLevel = avatarCfg.GetAvatarBreakthrough(avatarData.BreakthoughtLevel).breakThrough.powerUpLevelLimit;

		//Level
		avatarLVLabel.Text = GameUtility.FormatUIString("UIPnlAvatar_AvatarLevel",
											currentAvatarLevel,
											GameDefines.textColorInOrgYew.ToString(),
											maxAvatarLevel);

		activityNotify.Hide(!ItemInfoUtility.IsAbilityUpImprove_Avatar(avatarData));
		//country
		//国家
		if (avatarCfg != null)
			UIElemTemplate.Inst.SetAvatarCountryIcon(avatarCountryBox, avatarCfg.countryType);

		//Growth
		if (AvatarGrowthBtn != null)
			UIElemTemplate.Inst.SetAvatarTraitIcon(AvatarGrowthBtn, avatarCfg.traitType);

		//Set avatarData attributes.
		avatarHPTitle.Text = GameUtility.GetUIString("AvatarAttribute_HP");
		avatarATKTitle.Text = GameUtility.GetUIString("AvatarAttribute_AP");
		avatarSpeedTitle.Text = GameUtility.GetUIString("AvatarAttribute_Speed");

		avatarHPLabel.Text = "0";
		avatarSpeedLabel.Text = "0";
		avatarATKLabel.Text = "0";
		List<AttributeCalculator.Attribute> attributes = null;

		if (avatarLocation != null)
			attributes = PlayerDataUtility.GetLocationAvatarAttributes(avatarLocation, currentPlayer);
		else
			//attributes = PlayerDataUtility.GetAvatarAttributes(avatarData, false);
			attributes = PlayerDataUtility.GetAvatarAttributes(currentPlayer, avatarData, false, true, true);

		if (attributes != null)
		{
			for (int i = 0; i < attributes.Count; i++)
			{
				switch (attributes[i].type)
				{
					case _AvatarAttributeType.MaxHP:
						avatarHPLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
						break;
					case _AvatarAttributeType.PAP:
					case _AvatarAttributeType.MAP:
						avatarATKLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
						break;
					case _AvatarAttributeType.Speed:
						avatarSpeedLabel.Text = ItemInfoUtility.GetAttribDisplayString(attributes[i].type, attributes[i].value);
						break;
				}
			}
		}

		// Set Active Skill.
		foreach (var activeSkill in PlayerDataUtility.GetAvatarActiveSkill(avatarData.ResourceId, avatarData.BreakthoughtLevel))
		{
			var skillItem = SkillIconPool.AllocateItem().GetComponent<UIElemAvatarInfoSkill>();
			skillItem.SetData(activeSkill);

			SkillList.AddItem(skillItem.gameObject);
		}

		//Domineer
		string tempStr = string.Empty;
		List<KodGames.ClientClass.Domineer> domineers = new List<KodGames.ClientClass.Domineer>();
		if (avatarData.Domineer == null || avatarData.Domineer.Domineers.Count <= 0)
		{
			//Default Domineer
			DomineerConfig.DefaultDomineer defaultDomineer = ConfigDatabase.DefaultCfg.DomineerConfig.GetDefaultDomineerByCountryType(avatarCfg.countryType);
			if (defaultDomineer != null)
			{
				KodGames.ClientClass.Domineer domineer = new KodGames.ClientClass.Domineer();
				domineer.DomineerId = defaultDomineer.DomineerId;
				domineer.Level = defaultDomineer.Level;
				domineers.Add(domineer);
			}
		}
		else
			domineers = avatarData.Domineer.Domineers;

		for (int i = 0; i < domineers.Count; i++)
		{
			var domineerCfg = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(domineers[i].DomineerId);
			if (domineerCfg == null)
				continue;

			var dominnerLevel = domineerCfg.GetDomineerLevelByLevel(domineers[i].Level);
			if (dominnerLevel == null)
				continue;

			tempStr += string.Format("{0}{1}:{2}\n", GameDefines.textColorBtnYellow, ItemInfoUtility.GetAssetName(domineers[i].DomineerId), dominnerLevel.Desc);
		}

		//霸气
		if (!string.IsNullOrEmpty(tempStr))
		{
			UIElemAvatarTraitDesc dominnerDesc = avatarTraitItemPool.AllocateItem().GetComponent<UIElemAvatarTraitDesc>();
			dominnerDesc.DescTitle.Text = GameUtility.GetUIString("UIPnlAvatarInfo_TraitDesc_DominnerTile");
			dominnerDesc.DescContent.Text = tempStr;
			dominnerDesc.DescButton.Hide(!showPowerUp);
			avatarTraitList.AddItem(dominnerDesc.gameObject);
		}

		//avatarAssemble
		tempStr = string.Empty;

		int positionId = IDSeg.InvalidId;
		int showLocationId = -1;

		if (avatarLocation != null)
		{
			positionId = avatarLocation.PositionId;
			showLocationId = avatarLocation.ShowLocationId;
		}
		else if (avatarPartner != null)
			positionId = avatarPartner.PositionId;

		// 缘分
		List<SuiteConfig.AssembleSetting> assembleSettings = ConfigDatabase.DefaultCfg.SuiteConfig.GetAvatarAssembleByAvatarId(avatarCfg.id);
		bool showAssembleProcess = avatarPartner == null;
		var matchIds = new List<int>();

		if (avatarLocation != null && showAssembleProcess)
		{
			var equipedAvatars = PlayerDataUtility.GetLineUpAvatars(currentPlayer, avatarLocation.PositionId);
			var equipedCheerAvatars = PlayerDataUtility.GetCheerAvatars(currentPlayer, avatarLocation.PositionId);
			var equipedEquipment = PlayerDataUtility.GetLineUpEquipments(currentPlayer, avatarLocation.PositionId, avatarLocation.ShowLocationId);
			var equipedSkills = PlayerDataUtility.GetLineUpSkills(currentPlayer, avatarLocation.PositionId, avatarLocation.ShowLocationId);

			for (int i = 0; i < equipedAvatars.Count; i++)
				if (!matchIds.Contains(equipedAvatars[i].ResourceId))
					matchIds.Add(equipedAvatars[i].ResourceId);

			for (int i = 0; i < equipedCheerAvatars.Count; i++)
				if (!matchIds.Contains(equipedCheerAvatars[i].ResourceId))
					matchIds.Add(equipedCheerAvatars[i].ResourceId);

			for (int i = 0; i < equipedEquipment.Count; i++)
				if (!matchIds.Contains(equipedEquipment[i].ResourceId))
					matchIds.Add(equipedEquipment[i].ResourceId);

			for (int i = 0; i < equipedSkills.Count; i++)
				if (!matchIds.Contains(equipedSkills[i].ResourceId))
					matchIds.Add(equipedSkills[i].ResourceId);
		}

		for (int i = 0; i < assembleSettings.Count; i++)
		{
			bool isAssembleAvaliable = showAssembleProcess && PlayerDataUtility.CheckAvatarAssemble(assembleSettings[i], avatarData, currentPlayer, positionId, showLocationId);
			tempStr += BuildAssembleSettingStr(assembleSettings[i], isAssembleAvaliable, ref matchIds) + "\n";
		}

		if (!string.IsNullOrEmpty(tempStr))
		{
			UIElemAvatarTraitDesc avatarAssembleDesc = avatarTraitItemPool.AllocateItem().GetComponent<UIElemAvatarTraitDesc>();
			avatarAssembleDesc.DescTitle.Text = GameUtility.GetUIString("UIPnlAvatarInfo_TraitDesc_AssembleTile");
			avatarAssembleDesc.DescContent.Text = tempStr;
			avatarAssembleDesc.DescButton.Hide(true);
			avatarTraitList.AddItem(avatarAssembleDesc.gameObject);
		}

		// Meridian.
		tempStr = string.Empty;
		var meridianCfg = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianConfigSettingByQualityLevel(avatarCfg.qualityLevel);

		foreach (var meridianGroup in meridianCfg.meridianGroups)
		{
			var modifiersStr = string.Empty;
			var modifiers = new List<KodGames.ClientClass.PropertyModifier>();
			foreach (var meridian in meridianGroup.meridians)
			{
				if (avatarData.GetMeridianByID(meridian.id) == null)
					continue;

				modifiers.AddRange(avatarData.GetMeridianByID(meridian.id).Modifiers);
			}

			if (modifiers.Count <= 0)
				continue;

			modifiersStr = GameUtility.FormatUIString("UIPnlAvatarInfo_TraitDesc_MeridianLable", GameDefines.textColorWhite, MeridianConfig._MeridianType.GetDisplayNameByType(meridianGroup.type, ConfigDatabase.DefaultCfg));
			modifiers = PlayerDataUtility.MergeModifiers(modifiers, true, true);

			for (int i = 0; i < modifiers.Count; i++)
			{
				modifiersStr = GameUtility.AppendString(modifiersStr,
									string.Format(
									"{0}{1}{2}+{3}" + ((i == modifiers.Count - 1) ? "\n" : GameUtility.GetUIString("UI_Dot")),
									GameDefines.textColorBtnYellow.ToString(),
									_AvatarAttributeType.GetDisplayNameByType(modifiers[i].AttributeType, ConfigDatabase.DefaultCfg),
									GameDefines.textColorWhite,
									ItemInfoUtility.GetAttribDisplayString(modifiers[i])),
									false);
			}

			tempStr += modifiersStr;
		}

		if (!string.IsNullOrEmpty(tempStr))
		{
			UIElemAvatarTraitDesc avatarAssembleDesc = avatarTraitItemPool.AllocateItem().GetComponent<UIElemAvatarTraitDesc>();
			avatarAssembleDesc.DescTitle.Text = GameUtility.GetUIString("UIPnlAvatarInfo_TraitDesc_MeridianTile");
			avatarAssembleDesc.DescContent.Text = tempStr;
			avatarAssembleDesc.DescButton.Hide(true);
			avatarTraitList.AddItem(avatarAssembleDesc.gameObject);
		}

		avatarTraitList.PositionItems();
		avatarTraitList.ScrollPosition = 0;
	}

	private string BuildAssembleSettingStr(SuiteConfig.AssembleSetting avatarAssemble, bool mached, ref List<int> matchIds)
	{
		List<string> formatParams = new List<string>();

		for (int i = 0; i < avatarAssemble.Parts.Count; i++)
		{
			var part = avatarAssemble.Parts[i];
			for (int j = 0; j < part.Requiremets.Count; j++)
			{
				var require = part.Requiremets[j];

				if (!mached && matchIds.Contains(require.Value))
					formatParams.Add(string.Format("{0}{1}{2}", AVATAR_TRAITDESC_TXCOLOR_GREEN, ItemInfoUtility.GetAssetName(require.Value), AVATAR_TRAITDESC_TXCOLOR_BROWN));
				else
					formatParams.Add(ItemInfoUtility.GetAssetName(require.Value));
			}
		}

		return string.Format("{0}{1} : {2}", mached ? AVATAR_TRAITDESC_TXCOLOR_GREEN : AVATAR_TRAITDESC_TXCOLOR_BROWN, avatarAssemble.Name, GameUtility.FormatStringOnlyWithParams(avatarAssemble.Assembles[0].AssembleEffectDesc, formatParams.ToArray()));
	}

	private void ShowActionButtons(bool showChange, bool showBigClose, bool showGotoPackage, bool showPowerUp, bool showSelect, bool prev, bool next)
	{
		actionButtonLayout.HideChildObj(changeAvatarBtn.gameObject, !showChange);
		actionButtonLayout.HideChildObj(bigCloseBtn.gameObject, !showBigClose);
		actionButtonLayout.HideChildObj(gotoPackageBtn.gameObject, !showGotoPackage);
		actionButtonLayout.HideChildObj(powerUpAvatarBtn.gameObject, !showPowerUp);
		actionButtonLayout.HideChildObj(selectBtn.gameObject, !showSelect);
		actionButtonLayout.HideChildObj(prevBtn.gameObject, !prev);
		actionButtonLayout.HideChildObj(nextBtn.gameObject, !next);
	}

	#region CardPicture

	private void SetCardPicAvatarUI(int index, int count, bool isCardPic)
	{
		List<AvatarConfig.Avatar> avatars = GetCardPictureAvatars();
		AvatarConfig.Avatar avatarCfg = avatars[index];

		SetCardPicAvatarUI(avatarCfg, index, count, isCardPic);
	}

	private void SetCardPicAvatarUI(AvatarConfig.Avatar avatarCfg, int index, int count, bool isCardPic)
	{
		avatarData = new KodGames.ClientClass.Avatar();
		avatarData.ResourceId = avatarCfg.id;
		avatarData.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
		avatarData.LevelAttrib.Level = 1;
		avatarData.LevelAttrib.Experience = 0;
		avatarData.BreakthoughtLevel = 0;
		avatarData.Domineer = new KodGames.ClientClass.DomineerData();

		// Disable selecting
		selectDel = null;

		if (isCardPic)
			// Show action button, only show "Close"
			ShowActionButtons(false, false, false, false, false, index != 0, index != (count - 1));
		else
			ShowActionButtons(false, true, false, false, false, false, false);

		FillData();
	}

	private List<int> GetCardPictureIDs()
	{
		List<int> cardPictures = new List<int>();
		foreach (AvatarConfig.Avatar avatar in ConfigDatabase.DefaultCfg.AvatarConfig.avatars)
			cardPictures.Add(avatar.id);

		cardPictures.Sort(DataCompare.CompareAvatar);

		return cardPictures;
	}

	private List<AvatarConfig.Avatar> GetCardPictureAvatars()
	{
		List<AvatarConfig.Avatar> cardPictures = new List<AvatarConfig.Avatar>();
		foreach (AvatarConfig.Avatar avatar in ConfigDatabase.DefaultCfg.AvatarConfig.avatars)
			cardPictures.Add(avatar);

		cardPictures.Sort(DataCompare.CompareAvatar);

		return cardPictures;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardPicturePrevClick(UIButton btn)
	{
		List<int> cardPictures = GetCardPictureIDs();
		int index = cardPictures.IndexOf(avatarData.ResourceId);
		int prevIndex = Mathf.Max(0, index - 1);

		if (index == prevIndex)
			return;

		SetCardPicAvatarUI(prevIndex, cardPictures.Count, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnCardPictureNextClick(UIButton btn)
	{
		List<int> cardPictures = GetCardPictureIDs();
		int index = cardPictures.IndexOf(avatarData.ResourceId);
		int nextIndex = Mathf.Min(cardPictures.Count - 1, index + 1);

		if (index == nextIndex)
			return;

		SetCardPicAvatarUI(nextIndex, cardPictures.Count, true);
	}

	#endregion

	#region Btn funcs

	//显示详细
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickAvatarDetail(UIButton btn)
	{
		List<AttributeCalculator.Attribute> attributes = null;

		if (avatarLocation != null)
			attributes = PlayerDataUtility.GetLocationAvatarAttributes(avatarLocation, currentPlayer, false);
		else
			//attributes = PlayerDataUtility.GetAvatarAttributes(avatarData, true, false);
			attributes = PlayerDataUtility.GetAvatarAttributes(currentPlayer, avatarData, true, true, false);

		if (this.ShowLayer != _UILayer.Top)
		{
			if (showPositionPower)
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAttributeDetailTip), attributes, avatarData, true, avatarLocation.PositionId, avatarLocation.ShowLocationId, currentPlayer);
			else
				SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAttributeDetailTip), attributes, avatarData, true);
		}

		if (showPositionPower)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgAttributeDetailTip), _UILayer.Top, attributes, avatarData, true, avatarLocation.PositionId, avatarLocation.ShowLocationId, currentPlayer);
		else
			SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgAttributeDetailTip), _UILayer.Top, attributes, avatarData, true);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickClose(UIButton btn)
	{
		HideSelf();
	}

	//
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickChangeAvatar(UIButton btn)
	{
		if (avatarLocation != null)
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectAvatarList, avatarLocation);
		}
		else if (avatarPartner != null)
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlSelectAvatarList, avatarPartner);
		}

		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPowerUp(UIButton btn)
	{
		if (SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatarLevelUp, avatarData))
			HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGotoPackage(UIButton btn)
	{
		HideSelf();
		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlPackageAvatarTab));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSelect(UIButton btn)
	{
		if (selectDel != null)
			selectDel(avatarData);

		HideSelf();
	}

	//点击技能
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSkill(UIButton btn)
	{
		if (avatarCard.CardMaterial == null)
			return;

		UIElemAssetIcon assetIcon = btn.Data as UIElemAssetIcon;
		var skill = assetIcon.Data as KodGames.ClientClass.Skill;
		if (skill != null)
		{
			if (this.ShowLayer != _UILayer.Top)
				SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgSkillInfo, skill, false, true, false, false, null, false);
			else
				SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlSkillInfo), _UILayer.Top, skill, false, true, false, false, null, false);
		}
	}

	//点击获得途径
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickGetWay(UIButton btn)
	{
		if (this.ShowLayer != _UILayer.Top)
			SysUIEnv.Instance.ShowUIModule(typeof(UIDlgItemGetWay), avatarData.ResourceId);
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIDlgItemGetWay), _UILayer.Top, avatarData.ResourceId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickOpen(UIButton btn)
	{
		if (this.ShowLayer != _UILayer.Top)
			SysUIEnv.Instance.ShowUIModule(typeof(UIPnlCardImage), avatarData.ResourceId, false, true, avatarCard.CardMaterial);
		SysUIEnv.Instance.ShowUIModuleWithLayer(typeof(UIPnlCardImage), _UILayer.Top, avatarData.ResourceId, false, true, avatarCard.CardMaterial);
	}

	//去往洗炼霸气里面
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickArrogance(UIButton btn)
	{
		if (SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatarDomineerTab, avatarData))
			HideSelf();
	}


	//点击下方导航栏提示内容
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickMenuBot(UIButton btn)
	{
	}

	#endregion
}
