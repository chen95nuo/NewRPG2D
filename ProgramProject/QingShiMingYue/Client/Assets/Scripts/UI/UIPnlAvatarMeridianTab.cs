using System;
using UnityEngine;
using System.Collections.Generic;
using ClientServerCommon;

public class UIPnlAvatarMeridianTab : UIModule
{
	public UIScrollList scrollList;
	public UIElemAvatarMeridianItem twelveExtraMeridianContainer;
	public UIElemAvatarMeridianItem EightExtraMeridianContainer;
	public SpriteText attributeLabel;
	public SpriteText notSupportMeridianLabel;
	public GameObject meridianObj;

	public UIBox attributeBg1;
	public UIBox attributeBg2;

	public List<UIButton> tabButtons;

	private KodGames.ClientClass.Avatar avatarData;
	private int currentTab;

	private float avatarPower;
	private float positionPower;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		avatarPower = 0f;
		positionPower = 0f;

		tabButtons[0].Data = MeridianConfig._MeridianType.Twelve;
		tabButtons[1].Data = MeridianConfig._MeridianType.Eight;

		foreach (var btn in tabButtons)
			btn.controlIsEnabled = false;

		twelveExtraMeridianContainer.GetComponent<UIListItemContainer>().ScanChildren();
		EightExtraMeridianContainer.GetComponent<UIListItemContainer>().ScanChildren();

		scrollList.AddItemSnappedDelegate(ItemSnappedDelegate);

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		ClearData();

		avatarData = userDatas[0] as KodGames.ClientClass.Avatar;

		SysUIEnv.Instance.GetUIModule<UIPnlAvatarPowerUpTab>().UpdateTabStatus(_UIType.UIPnlAvatarMeridianTab, avatarData);

		InitView();

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
		ClearData();
	}

	private void ClearData()
	{
		avatarData = null;
		twelveExtraMeridianContainer.ClearData();
		EightExtraMeridianContainer.ClearData();

		scrollList.ScrollPosition = 0f;

		currentTab = MeridianConfig._MeridianType.Twelve;

		avatarPower = 0f;
		positionPower = 0f;
	}

	private void InitView()
	{
		MeridianConfig.MeridianConfigSetting meridianConfigSet = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianConfigSettingByQualityLevel(ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId).qualityLevel);

		meridianObj.SetActive(meridianConfigSet != null);
		notSupportMeridianLabel.Hide(meridianConfigSet != null);

		// Init Item Data.
		twelveExtraMeridianContainer.InitData(avatarData);
		EightExtraMeridianContainer.InitData(avatarData);

		if (meridianConfigSet != null)
		{
			// Set twelveExtra.
			int index = 0;
			foreach (var meridian in meridianConfigSet.GetMeridiansByType(MeridianConfig._MeridianType.Twelve))
				twelveExtraMeridianContainer.SetMeridianItem(index++, meridian);

			// Set EightExtra.
			index = 0;
			foreach (var meridian in meridianConfigSet.GetMeridiansByType(MeridianConfig._MeridianType.Eight))
				EightExtraMeridianContainer.SetMeridianItem(index++, meridian);
		}

		// ChangeTab.
		ChangeTab(currentTab);
	}

	private void SetAttributeLabel()
	{
		MeridianConfig.MeridianConfigSetting meridianConfigSet = ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianConfigSettingByQualityLevel(ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatarData.ResourceId).qualityLevel);
		if (meridianConfigSet == null)
		{
			attributeLabel.Text = string.Empty;
			return;
		}

		// 获取经脉属性加成
		var modifiers = new List<KodGames.ClientClass.PropertyModifier>();
		foreach (var meridianConfig in meridianConfigSet.GetMeridiansByType(currentTab))
		{
			if (avatarData.GetMeridianByID(meridianConfig.id) == null)
				continue;

			modifiers.AddRange(avatarData.GetMeridianByID(meridianConfig.id).Modifiers);
		}

		modifiers = PlayerDataUtility.MergeModifiers(modifiers, true, true);

		// 显示UI
		if (modifiers.Count >= 4)
		{
			attributeBg1.Hide(false);
			attributeBg2.Hide(false);
		}
		else if (modifiers.Count > 0)
		{
			attributeBg1.Hide(false);
			attributeBg2.Hide(true);
		}
		else
		{
			attributeBg1.Hide(true);
			attributeBg2.Hide(true);
		}

		string attributeText = "";
		for (int i = 0; i < modifiers.Count; i++)
		{
			// TODO : 有潜在的问题, 顿号在其他文字里面可能不能显示
			attributeText = GameUtility.AppendString(
									attributeText,
									string.Format(
									"{0}{1}{2}+{3}" + ((i == 2 || i == modifiers.Count - 1) ? "\n" : GameUtility.GetUIString("UI_Dot")),
									GameDefines.textColorBtnYellow.ToString(),
									_AvatarAttributeType.GetDisplayNameByType(modifiers[i].AttributeType, ConfigDatabase.DefaultCfg),
									GameDefines.textColorWhite,
									ItemInfoUtility.GetAttribDisplayString(modifiers[i])),
									false);
		}

		attributeLabel.Text = GameUtility.FormatUIString("UIPnlAvatarMeridian_AvatarLevelTotlalAddProperty", GameDefines.textColorWhite.ToString()) + attributeText;
		attributeLabel.Hide(attributeText.Length <= 0);
	}

	private void ChangeTab(int type)
	{
		ChangeTabButton(type);

		// Snap ScrollList.
		scrollList.ScrollToItem(currentTab - 1, 0.5f);
	}

	private void ChangeTabButton(int type)
	{
		this.currentTab = type;

		foreach (var btn in tabButtons)
			btn.controlIsEnabled = (int)btn.data != type;

		// Set Meridian Attribute Label.
		SetAttributeLabel();
	}

	public void ItemSnappedDelegate(IUIListObject item)
	{
		if (item as UIListItemContainer == twelveExtraMeridianContainer.GetComponent<UIListItemContainer>())
			ChangeTabButton(MeridianConfig._MeridianType.Twelve);
		else
			ChangeTabButton(MeridianConfig._MeridianType.Eight);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnTabClick(UIButton btn)
	{
		ChangeTab((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMeridianIconClick(UIButton btn)
	{
		var assetIcon = btn.Data as UIElemAssetIcon;
		var meridianItem = assetIcon.Data as UIElemAvatarMeridianItem.AvatarMeridianItem;
		var meridianData = avatarData.GetMeridianByID(meridianItem.meridianConfig.id);

		avatarPower = PlayerDataUtility.CalculateAvatarPower(avatarData);
		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarData.Guid, avatarData.ResourceId))
			positionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);

		//未激活
		if (meridianData == null &&
			meridianItem.meridianConfig.preMeridianId != IDSeg.InvalidId &&
			ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(meridianItem.meridianConfig.preMeridianId) != null &&
			avatarData.GetMeridianByID(meridianItem.meridianConfig.preMeridianId) == null)
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlTipFlow, GameUtility.FormatUIString("UIPnlAvatarMeridian_AvatarLevelNeedActivedFirst", ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(meridianItem.meridianConfig.preMeridianId).name));
			return;
		}

		if (meridianData == null)
		{
			string costStr = "";
			for (int index = 0; index < meridianItem.meridianConfig.GetCostsByMeridianTimes(0).Count; index++)
			{
				costStr = GameUtility.AppendString(costStr, string.Format("{0}{1}{2}", GameDefines.colorWhite.ToString(), meridianItem.meridianConfig.GetCostsByMeridianTimes(0)[index].count, ItemInfoUtility.GetAssetName(meridianItem.meridianConfig.GetCostsByMeridianTimes(0)[index].id)), false);
				if (index + 1 < meridianItem.meridianConfig.GetCostsByMeridianTimes(0).Count)
					costStr = GameUtility.FormatUIString("UIAvatarMeridian_CostContainer", costStr, GameDefines.colorWhite.ToString());

			}

			costStr = GameUtility.FormatUIString("UIAvatarMeridian_TwelveMeridianActivied", GameDefines.colorWhite.ToString(), costStr, GameDefines.colorWhite.ToString(), meridianItem.meridianConfig.name, meridianItem.meridianConfig.buffAddition * 100);

			UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
			string title = meridianItem.meridianConfig.name;

			MainMenuItem okCallback = new MainMenuItem();
			okCallback.Callback =
				(data) =>
				{
					RequestMgr.Inst.Request(new ChangeMeridianReq(meridianItem.meridianConfig.id, avatarData.Guid));
					return true;
				};
			okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");

			MainMenuItem cancelCallback = new MainMenuItem();
			cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");
			showData.SetData(title, costStr, cancelCallback, okCallback);
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgMessage, showData, SpriteText.Alignment_Type.Center, 14.0f, 1.3f);

		}
		else
		{
			SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgAvatarMeridian, avatarData, meridianItem.meridianConfig.id, false, false);
		}
	}

	public void UpDataMeridianUI(int meridianID)
	{
		UpDataMeridianUI(meridianID, false);
	}

	public void UpDataMeridianUI(int meridianID, bool changeMeridian)
	{
		if (avatarData == null)
			return;

		float tempAvatarPower = PlayerDataUtility.CalculateAvatarPower(avatarData);
		if (tempAvatarPower > avatarPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(tempAvatarPower - avatarPower)));
		else if (tempAvatarPower < avatarPower)
			SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_OneUp", (int)(avatarPower - tempAvatarPower)));

		avatarPower = tempAvatarPower;

		if (PlayerDataUtility.IsLineUpInSpecialPosition(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId, avatarData.Guid, avatarData.ResourceId))
		{
			float tempPositionPower = PlayerDataUtility.CalculatePlayerPower(SysLocalDataBase.Inst.LocalPlayer, SysLocalDataBase.Inst.LocalPlayer.PositionData.ActivePositionId);
			if (tempPositionPower > positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionUp", (int)(tempPositionPower - positionPower)));
			else if (tempPositionPower < positionPower)
				SysUIEnv.Instance.AddTip(GameUtility.FormatUIString("UITipsPower_PositionDown", (int)(positionPower - tempPositionPower)));

			positionPower = tempPositionPower;
		}

		SetAttributeLabel();
		switch (ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(meridianID).type)
		{
			case MeridianConfig._MeridianType.Twelve:
				twelveExtraMeridianContainer.RefreshMeridianItem(meridianID, changeMeridian);
				break;
			case MeridianConfig._MeridianType.Eight:
				EightExtraMeridianContainer.RefreshMeridianItem(meridianID, changeMeridian);
				break;
		}
	}

	public void AddActiveEffect(int meridianID)
	{
		switch (ConfigDatabase.DefaultCfg.MeridianConfig.GetMeridianById(meridianID).type)
		{
			case MeridianConfig._MeridianType.Twelve:
				twelveExtraMeridianContainer.AddActionEffect(meridianID);
				break;
			case MeridianConfig._MeridianType.Eight:
				EightExtraMeridianContainer.AddActionEffect(meridianID);
				break;
		}
	}
}