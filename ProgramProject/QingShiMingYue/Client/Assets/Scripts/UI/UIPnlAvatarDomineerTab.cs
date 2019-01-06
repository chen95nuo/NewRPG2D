using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarDomineerTab : UIModule
{
	public SpriteText TitleText;
	public GameObject domineerRoot;//霸气界面
	public GameObject domineerChangedResultRoot;//升级霸气结果界面
	public GameObject changedomineerRoot;//升级消耗界面
	public GameObject domineerIconsRoot;//霸气列表界面

	public UIElemAssetIcon avatarIconBtn;//Avatar头像Icon
	public SpriteText avatarNameLabel;//名字
	public UIElemBreakThroughBtn avatarQualityLabel;//星级

	public UIElemDomineerItem[] domineerIcons;//霸气列表
	public SpriteText hezongName;//合众名字
	public SpriteText hezongDesc;//合众说明
	public SpriteText lianhengName;//连横名字
	public SpriteText lianhengDesc;//连横说明

	public UIElemAssetIcon costItemIcon;//霸气单icon
	public SpriteText costItemCount;//消耗霸气丹数量

	public SpriteText costCardCount;//消耗同名卡数量
	public SpriteText costCoinCount;//消耗铜币数量

	public UIElemAssetIcon costItem1Icon;//精魄icon
	public SpriteText costItem1Count;//消耗精魄数量

	public UIScrollList avatarList;//同名卡列表
	public GameObjectPool avatarObjectPool;
	public SpriteText emptyTip;
	public SpriteText canNotDoTip;//未达到突破等级
	public SpriteText sameCardTip;//霸气丹说明

	public UIElemDomineerItem[] preDomineerIcons;
	public UIElemDomineerItem[] changedDomineerIcons;

	//public UIButton DomineerDescBtn;
	public UIButton SaveButton;
	public UIButton UseButton;
	public UIButton TrainButton;

	private KodGames.ClientClass.Avatar avatarLocalData;

	private float avatarPower;
	private float positionPower;

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		avatarPower = 0f;
		positionPower = 0f;

		avatarLocalData = userDatas[0] as KodGames.ClientClass.Avatar;
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarDomineerTab, avatarLocalData);

		InitData();
		ShowDomineerUI();

		return true;
	}

	public void OnChangeDomineerResponse(KodGames.ClientClass.Avatar changedAvatar)
	{
		avatarLocalData = changedAvatar;
		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarDomineerTab, avatarLocalData);
		InitData();
		ShowDomineerUI();

		float tempAvatarPower = PlayerDataUtility.CalculateAvatarPower(avatarLocalData);
		if (tempAvatarPower > avatarPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempAvatarPower - avatarPower)));
		else if (tempAvatarPower < avatarPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(avatarPower - tempAvatarPower)));

		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarLocalData.Guid, avatarLocalData.ResourceId))
		{
			float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
			if (tempPositionPower > positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
			else if (tempPositionPower < positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));
		}
	}

	public void OnSaveDomineerResponse(KodGames.ClientClass.Avatar changedAvatar)
	{
		OnChangeDomineerResponse(changedAvatar);
	}

	private void InitData()
	{
		avatarLocalData.Domineer.Domineers.Sort((d1, d2) =>
		{
			int type1 = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(d1.DomineerId).Type;
			int type2 = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(d2.DomineerId).Type;
			return type2 - type1;
		});

		avatarLocalData.Domineer.UnsaveDomineers.Sort((d1, d2) =>
		{
			int type1 = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(d1.DomineerId).Type;
			int type2 = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerById(d2.DomineerId).Type;
			return type2 - type1;
		});
	}

	private void ShowDomineerUI()
	{

		SetCommonUI();
		//判断显示的界面
		if (avatarLocalData.Domineer.UnsaveDomineers.Count > 0)
		{
			TitleText.Text = GameUtility.FormatUIString("E_GAME_CHANGE_DOMINEER_XILIANJIEGUO");
			domineerRoot.SetActive(false);
			//DomineerDescBtn.Hide(true);
			TrainButton.Hide(true);
			SaveButton.Hide(false);
			UseButton.Hide(false);
			domineerChangedResultRoot.SetActive(true);
			domineerIconsRoot.SetActive(false);
			SetDomineerResultUI();
		}
		else
		{
			TitleText.Text = GameUtility.FormatUIString("E_GAME_CHANGE_DOMINEER_XILIANBAQI");
			domineerRoot.SetActive(true);
			//DomineerDescBtn.Hide(false);
			TrainButton.Hide(false);
			SaveButton.Hide(true);
			UseButton.Hide(true);
			domineerChangedResultRoot.SetActive(false);
			domineerIconsRoot.SetActive(true);
			SetDomineerDescUI();
			SetDomineerChangeUI();
		}
	}

	private void SetCommonUI()
	{
		//加载Avatar信息

		//Set performance icon.
		avatarIconBtn.SetData(avatarLocalData);

		//Set avatar name.
		avatarNameLabel.Text = ItemInfoUtility.GetAssetName(avatarLocalData.ResourceId);

		//Set avatar quality
		avatarQualityLabel.SetBreakThroughIcon(avatarLocalData.BreakthoughtLevel);

		if (SysLocalDataBase.Inst.LocalPlayer.VipLevel < ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.RetainOriginalDomineerSkill))
			SaveButton.Text = GameUtility.FormatUIString("UIPnlAvatarDomineerTab_KeepOriginalWithVipLevel", ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.RetainOriginalDomineerSkill));
		else
			SaveButton.Text = GameUtility.GetUIString("UIPnlAvatarDomineerTab_KeepOriginal");


	}

	private void SetDomineerDescUI()
	{
		//加载Avatar霸气列表数据
		for (int i = 0; i < domineerIcons.Length; i++)
		{
			domineerIcons[i].gameObject.SetActive(false);
		}

		if (avatarLocalData.Domineer.Domineers.Count > 0)
		{
			for (int i = 0; i < avatarLocalData.Domineer.Domineers.Count; i++)
			{
				int id = avatarLocalData.Domineer.Domineers[i].DomineerId;
				int level = avatarLocalData.Domineer.Domineers[i].Level;
				domineerIcons[i].SetData(id, level);
				domineerIcons[i].gameObject.SetActive(true);
			}
		}
		else
		{
			int index = 0;
			for (int i = 0; i < ConfigDatabase.DefaultCfg.DomineerConfig.DefalultDomineers.Count; i++)
			{
				DomineerConfig.DefaultDomineer defaultDomineer = ConfigDatabase.DefaultCfg.DomineerConfig.DefalultDomineers[i];
				AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
				if (defaultDomineer.AvatarCountryType == avatarConfig.countryType)
				{
					domineerIcons[index].SetData(defaultDomineer.DomineerId, defaultDomineer.Level);
					domineerIcons[index++].gameObject.SetActive(true);
				}
			}
		}
	}

	private static int Compare(KodGames.ClientClass.Avatar avatarX, KodGames.ClientClass.Avatar avatarY)
	{
		if (avatarX.BreakthoughtLevel != avatarY.BreakthoughtLevel)
		{
			return avatarX.BreakthoughtLevel.CompareTo(avatarY.BreakthoughtLevel);
		}
		else
		{
			return avatarX.LevelAttrib.Level.CompareTo(avatarY.LevelAttrib.Level);
		}
	}

	private List<KodGames.ClientClass.Avatar> GetSameIdAvatars(KodGames.ClientClass.Avatar avatar)
	{
		//同名卡列表
		List<KodGames.ClientClass.Avatar> avatars = new List<KodGames.ClientClass.Avatar>();
		foreach (KodGames.ClientClass.Avatar avatarItem in SysLocalDataBase.Inst.LocalPlayer.Avatars)
		{
			if (avatarItem.IsAvatar == false || avatar == avatarItem || ItemInfoUtility.IsAvatarEquipped(avatarItem) || ItemInfoUtility.IsAvatarCheered(avatarItem))
				continue;

			if (avatarItem.ResourceId == avatar.ResourceId)
			{
				avatars.Add(avatarItem);
			}
		}

		avatars.Sort(Compare);
		return avatars;
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator FillAvatarList(List<KodGames.ClientClass.Avatar> avatars)
	{
		yield return null;

		//Fill the scroll list control.

		foreach (KodGames.ClientClass.Avatar avatar in avatars)
		{
			UIListItemContainer itemContainer = avatarObjectPool.AllocateItem().GetComponent<UIListItemContainer>();
			UIElemAvatarSelectToggleItem item = itemContainer.gameObject.GetComponent<UIElemAvatarSelectToggleItem>();
			itemContainer.data = item;
			item.ResetToggleState(false);
			item.SetData(avatar);
			avatarList.AddItem(itemContainer);
		}

		if (avatarList.Count <= 0 && !emptyTip.Text.Equals(GameUtility.GetUIString("UIEmptyList_Avatar")))
			emptyTip.Text = GameUtility.GetUIString("UIEmptyList_Avatar");
		else if (avatarList.Count > 0 && !emptyTip.Text.Equals(""))
			emptyTip.Text = "";
	}

	private void ClearCardList()
	{
		StopCoroutine("FillAvatarList");
		avatarList.ClearList(false);
		avatarList.ScrollListTo(0f);

		emptyTip.Text = "";
	}

	private void SetDomineerChangeUI()
	{
		ClearCardList();

		if (avatarLocalData.BreakthoughtLevel < ConfigDatabase.DefaultCfg.DomineerConfig.NeedAvatarBreakThroughLevel || avatarLocalData.LevelAttrib.Level < ConfigDatabase.DefaultCfg.LevelConfig.GetPlayerLevelByOpenFunciton(ClientServerCommon._OpenFunctionType.Domineer))
		{
			changedomineerRoot.SetActive(false);
			SaveButton.Hide(true);
			UseButton.Hide(true);
			TrainButton.Hide(true);
			canNotDoTip.Text = string.Format(GameUtility.GetUIString("UIPnlAvatarDomineerTab_Tips2"), ConfigDatabase.DefaultCfg.DomineerConfig.NeedAvatarBreakThroughLevel);
		}
		else
		{
			changedomineerRoot.SetActive(true);
			canNotDoTip.Text = "";
			// Fill avatar list.
			StartCoroutine("FillAvatarList", GetSameIdAvatars(avatarLocalData));
			SetDomineerChangeCostUI();
		}
	}

	private int GetFirstDomineerLevel()
	{
		int firstDomineerLevel = 0;
		if (avatarLocalData.Domineer.Domineers.Count > 0)
		{
			firstDomineerLevel = avatarLocalData.Domineer.Domineers[0].Level;
		}
		else
		{
			for (int i = 0; i < ConfigDatabase.DefaultCfg.DomineerConfig.DefalultDomineers.Count; i++)
			{
				DomineerConfig.DefaultDomineer defaultDomineer = ConfigDatabase.DefaultCfg.DomineerConfig.DefalultDomineers[i];
				AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
				if (defaultDomineer.AvatarCountryType == avatarConfig.countryType)
				{
					firstDomineerLevel = defaultDomineer.Level;
					break;
				}
			}
		}

		return firstDomineerLevel;
	}

	private int GetFirstDomineerId()
	{
		int firstDomineerid = 0;
		if (avatarLocalData.Domineer.Domineers.Count > 0)
		{
			firstDomineerid = avatarLocalData.Domineer.Domineers[0].DomineerId;
		}
		else
		{
			for (int i = 0; i < ConfigDatabase.DefaultCfg.DomineerConfig.DefalultDomineers.Count; i++)
			{
				DomineerConfig.DefaultDomineer defaultDomineer = ConfigDatabase.DefaultCfg.DomineerConfig.DefalultDomineers[i];
				AvatarConfig.Avatar avatarConfig = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId);
				if (defaultDomineer.AvatarCountryType == avatarConfig.countryType)
				{
					firstDomineerid = defaultDomineer.DomineerId;
					break;
				}
			}
		}

		return firstDomineerid;
	}

	private void SetDomineerChangeCostUI()
	{
		int qualityLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).qualityLevel;
		DomineerConfig.DomineerCost costConfig = ConfigDatabase.DefaultCfg.DomineerConfig.GetDomineerCostSetByQualityLevel(qualityLevel).GetDomineerCostByLevel(GetFirstDomineerLevel());

		int selectedAvatarsCount = GetSelectedAvatars().Count;
		int itemCount = costConfig.ItemCostItemCount - selectedAvatarsCount * costConfig.SameCardDeductItemCount;

		costCardCount.Text = selectedAvatarsCount.ToString();

		KodGames.ClientClass.Consumable consumable = SysLocalDataBase.Inst.LocalPlayer.SearchConsumable(costConfig.ItemCostItemId);
		int itemAllCount = 0;
		if (consumable != null)
		{
			itemAllCount = consumable.Amount;
		}

		costItemIcon.SetData(ConfigDatabase.DefaultCfg.ItemConfig.domineerItemId);

		costItemCount.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob2"),
											itemAllCount >= itemCount ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											itemAllCount, itemCount);
		string costItemName = ConfigDatabase.DefaultCfg.AssetDescConfig.GetAssetDescById(costConfig.ItemCostItemId).name;


		//消耗铜币
		int costCoin = 0;
		int costSprite = 0;
		foreach (var cost in costConfig.OtherCosts)
		{
			if (cost.id == IDSeg._SpecialId.Spirit)
				costSprite = cost.count;
			else if (cost.id == IDSeg._SpecialId.GameMoney)
				costCoin = cost.count;
		}

		costItem1Icon.SetData(IDSeg._SpecialId.Spirit);//精魄
		costItem1Count.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob2"),
											SysLocalDataBase.Inst.LocalPlayer.Spirit >= costSprite ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											SysLocalDataBase.Inst.LocalPlayer.Spirit, costSprite);

		costCoinCount.Text = costCoin.ToString();
		costCoinCount.Text = string.Format(GameUtility.GetUIString("UIPnlIndiana_Label_Rob3"),
											SysLocalDataBase.Inst.LocalPlayer.GameMoney >= costCoin ? GameDefines.textColorWhite.ToString() : GameDefines.textColorRed.ToString(),
											costCoin);

		sameCardTip.Text = string.Format(GameUtility.GetUIString("UIPnlAvatarDomineerTab_Tips1"), costConfig.SameCardDeductItemCount, costItemName, costConfig.ItemCostItemCount, costItemName);
	}

	private List<KodGames.ClientClass.Avatar> GetSelectedAvatars()
	{
		List<KodGames.ClientClass.Avatar> selectedAvatars = new List<KodGames.ClientClass.Avatar>();
		for (int index = 0; index < avatarList.Count; index++)
		{
			UIElemAvatarSelectToggleItem toggleItem = avatarList.GetItem(index).Data as UIElemAvatarSelectToggleItem;
			if (toggleItem.IsSelected && toggleItem.AvatarData != null)
			{
				selectedAvatars.Add(toggleItem.AvatarData);
			}
		}

		return selectedAvatars;
	}

	private void SetDomineerResultUI()
	{
		for (int i = 0; i < preDomineerIcons.Length; i++)
		{
			preDomineerIcons[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < changedDomineerIcons.Length; i++)
		{
			changedDomineerIcons[i].gameObject.SetActive(false);
		}

		for (int i = 0; i < avatarLocalData.Domineer.Domineers.Count; i++)
		{
			int id = avatarLocalData.Domineer.Domineers[i].DomineerId;
			int level = avatarLocalData.Domineer.Domineers[i].Level;
			preDomineerIcons[i].SetData(id, level);
			preDomineerIcons[i].gameObject.SetActive(true);
		}

		for (int i = 0; i < avatarLocalData.Domineer.UnsaveDomineers.Count; i++)
		{
			int id = avatarLocalData.Domineer.UnsaveDomineers[i].DomineerId;
			int level = avatarLocalData.Domineer.UnsaveDomineers[i].Level;
			changedDomineerIcons[i].SetData(id, level);
			changedDomineerIcons[i].gameObject.SetActive(true);
		}
	}

	//计算战力
	private void CalculatePower()
	{
		avatarPower = PlayerDataUtility.CalculateAvatarPower(avatarLocalData);
		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarLocalData.Guid, avatarLocalData.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnChangeClick(UIButton btn)
	{
		CalculatePower();

		List<string> destroyAvatarGuids = new List<string>();
		List<KodGames.ClientClass.Avatar> selectAvatars = GetSelectedAvatars();
		for (int i = 0; i < selectAvatars.Count; i++)
		{
			KodGames.ClientClass.Avatar selectAvatar = selectAvatars[i];
			destroyAvatarGuids.Add(selectAvatar.Guid);
		}

		RequestMgr.Inst.Request(new ChangeDomineerReq(avatarLocalData.Guid, destroyAvatarGuids));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnIntroduceClick(UIButton btn)
	{
		//霸气介绍界面
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlDomineerDescTab /*GetFirstDomineerId()*/);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnKeepClick(UIButton btn)
	{
		//caculate change level
		int changedLevel = 0;

		for (int i = 0; i < avatarLocalData.Domineer.Domineers.Count; i += 1)
		{
			changedLevel += avatarLocalData.Domineer.Domineers[i].Level;
		}

		for (int j = 0; j < avatarLocalData.Domineer.UnsaveDomineers.Count; j += 1)
		{
			changedLevel -= avatarLocalData.Domineer.UnsaveDomineers[j].Level;
		}

		//avatarLocalData.Domineer.UnsaveDomineers
		if (changedLevel != 0)
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTipFlow, GameUtility.GetUIString("UIPnlAvatarDomineerTab_CanNotSavePre"));
		}
		else if (SysLocalDataBase.Inst.LocalPlayer.VipLevel < ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.RetainOriginalDomineerSkill))
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTipFlow, GameUtility.FormatUIString("UIPnlAvatarDomineerTab_VipLevelNotEnough", ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.RetainOriginalDomineerSkill)));
		}
		else
		{
			CalculatePower();

			//保留之前属性
			RequestMgr.Inst.Request(new SaveDomineerReq(avatarLocalData.Guid, false));
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnUseClick(UIButton btn)
	{
		CalculatePower();
		//使用突破后属性
		RequestMgr.Inst.Request(new SaveDomineerReq(avatarLocalData.Guid, true));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnAvatarItemClick(UIButton btn)
	{
		int qualityLevel = ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarLocalData.ResourceId).qualityLevel;
		int costAvatarCount = ConfigDatabase.DefaultCfg.DomineerConfig.GetCostAvatarCount(qualityLevel, GetFirstDomineerLevel());
		UIElemAssetIcon assetIcon = btn.data as UIElemAssetIcon;
		UIElemAvatarSelectToggleItem avatarListItem = assetIcon.Data as UIElemAvatarSelectToggleItem;
		avatarListItem.ToggleState();
		List<KodGames.ClientClass.Avatar> selectedAvatars = GetSelectedAvatars();

		if (selectedAvatars.Count > costAvatarCount)
		{
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAvatarBreakThroughTab_Tip_SelectMore"));
			avatarListItem.ToggleState();
			return;
		}

		SetDomineerChangeCostUI();
	}
}