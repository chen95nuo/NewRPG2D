using UnityEngine;
using System;
using System.Collections.Generic;
using KodGames;
using ClientServerCommon;

public class UIPnlAvatarDiner : UIModule
{
	public enum TabEnum
	{
		UnKnow,
		Dinered,
		DinerList,
	}

	public class ShowData
	{
		public int dinerId;
		public int qualityType;
		public int state;
		public bool hired;
		public KodGames.ClientClass.Avatar avatar;
	}

	public AutoSpriteControlBase blueBorderIcon;
	public AutoSpriteControlBase purpleBorderIcon;
	public AutoSpriteControlBase orangeBorderIcon;

	public List<AutoSpriteControlBase> tabButtons;
	public List<AutoSpriteControlBase> subTabButtons;
	public List<UIElemAvatarDinerItem> dinerItems;

	// Empty Desc.
	public SpriteText emptyLabel; // Use For HireList.
	public UIElemAssetIcon recommendAvatarIcon; // Use For HiredList.
	public AutoSpriteControlBase recommendButton;

	// 雇佣列表
	public GameObject dinerInfoRoot;
	public UIChildLayoutControl refreshAllLayout;
	public SpriteText refreshAllLabel;
	public SpriteText refreshByCountryLabel;
	public UIElemAssetIcon refreshAllCostBox;
	public SpriteText autoRefreshTimeLable;

	// 已雇佣
	public GameObject dinerdInfoRoot;
	public AutoSpriteControlBase prePageBtn;
	public AutoSpriteControlBase nextPageBtn;
	public SpriteText pageLabel;

	public GameObject hideRoot;

	private KodGames.ClientClass.HireDinerData DinerData { get { return SysLocalDataBase.Inst.LocalPlayer.HireDinerData; } }
	private List<ShowData> showDatas = new List<ShowData>();
	private float updateDelta;
	private int currentPage;
	private bool waitForQuaryList = false;

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		tabButtons[0].Data = TabEnum.Dinered;
		tabButtons[1].Data = TabEnum.DinerList;

		return true;
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (!base.OnShow(layer, userDatas))
			return false;

		hideRoot.SetActive(DinerData.DinerPackages != null);

		InitView();

		OnChangeTab(TabEnum.DinerList);

		return true;
	}

	public override void OnHide()
	{
		base.OnHide();
	}

	private void ClearData()
	{
		showDatas.Clear();
		waitForQuaryList = false;
		currentPage = 0;
		updateDelta = 0;
	}

	public void OnQueryDinerListSuccess()
	{
		waitForQuaryList = false;
		OnChangeTab(TabEnum.DinerList);
	}

	private void OnChangeTab(TabEnum tabEnum)
	{
		if (tabEnum == TabEnum.DinerList && DinerData.DinerPackages == null)
		{
			waitForQuaryList = true;
			RequestMgr.Inst.Request(new QueryDinerListReq());
			return;
		}

		for (int i = 0; i < tabButtons.Count; i++)
			tabButtons[i].controlIsEnabled = (TabEnum)tabButtons[i].Data != tabEnum;

		// Init SubButtons.
		InitSubTabButtons();
	}

	public TabEnum GetCurrentTabEnum()
	{
		for (int i = 0; i < tabButtons.Count; i++)
			if (!tabButtons[i].controlIsEnabled)
				return (TabEnum)tabButtons[i].Data;

		return TabEnum.UnKnow;
	}

	private void InitSubTabButtons()
	{
		// Set SubButtons.
		for (int i = 0; i < subTabButtons.Count; i++)
			subTabButtons[i].Hide(false);

		int defaultSubTab = 0;
		switch (GetCurrentTabEnum())
		{
			case TabEnum.Dinered:
				SetSubButton(subTabButtons[0], 3, GameUtility.GetUIString("UIPnlAvatarDiner_AvatarQuality1"));
				SetSubButton(subTabButtons[1], 4, GameUtility.GetUIString("UIPnlAvatarDiner_AvatarQuality2"));
				SetSubButton(subTabButtons[2], 5, GameUtility.GetUIString("UIPnlAvatarDiner_AvatarQuality3"));
				defaultSubTab = (int)subTabButtons[2].Data;

				for (int index = subTabButtons.Count - 1; index >= 0; index--)
				{
					int quality = (int)subTabButtons[index].Data;
					bool match = false;

					foreach (var avatar in DinerData.HireDiners)
					{
						var kd_Avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(avatar.AvatarGuid);
						if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(kd_Avatar.ResourceId).qualityLevel == quality)
						{
							defaultSubTab = quality;
							match = true;
							break;
						}
					}

					if (match)
						break;
				}

				break;

			case TabEnum.DinerList:
				SetSubButton(subTabButtons[0], DinerConfig._AvatarRarityType.Normal, DinerConfig._AvatarRarityType.GetDisplayNameByType(DinerConfig._AvatarRarityType.Normal, ConfigDatabase.DefaultCfg));
				SetSubButton(subTabButtons[1], DinerConfig._AvatarRarityType.Elite, DinerConfig._AvatarRarityType.GetDisplayNameByType(DinerConfig._AvatarRarityType.Elite, ConfigDatabase.DefaultCfg));
				SetSubButton(subTabButtons[2], DinerConfig._AvatarRarityType.Rare, DinerConfig._AvatarRarityType.GetDisplayNameByType(DinerConfig._AvatarRarityType.Rare, ConfigDatabase.DefaultCfg));
				defaultSubTab = (int)subTabButtons[2].Data;
				break;
		}

		SelectSubButton(defaultSubTab);
	}

	private void SetSubButton(AutoSpriteControlBase button, int data, string lable)
	{
		button.Data = data;
		button.Text = lable;
	}

	private void SelectSubButton(int data)
	{
		for (int i = 0; i < subTabButtons.Count; i++)
			subTabButtons[i].controlIsEnabled = !((int)subTabButtons[i].Data == data);

		if (GetCurrentTabEnum() == TabEnum.Dinered)
			ShowDinerdByQuality(data);
		else
			ShowDinerListByQuality(data);
	}

	private int GetCurrentSubButtonData()
	{
		for (int i = 0; i < subTabButtons.Count; i++)
			if (!subTabButtons[i].controlIsEnabled)
				return (int)subTabButtons[i].Data;

		return 0;
	}

	private UIElemAvatarDinerItem GetDinerItemByDinerId(int dinerId)
	{
		for (int index = 0; index < dinerItems.Count; index++)
		{
			if (dinerItems[index].ShowData == null)
				continue;

			if (dinerItems[index].ShowData.dinerId == dinerId)
				return dinerItems[index];
		}

		return null;
	}

	private void Update()
	{
		updateDelta += Time.deltaTime;
		if (updateDelta < 1.0f)
			return;

		updateDelta = 0f;

		for (int index = 0; index < dinerItems.Count; index++)
			dinerItems[index].RefreshDynamicView();

		for (int index = 0; index < dinerItems.Count; index++)
		{
			if (dinerItems[index].ShowData == null || !dinerItems[index].ShowData.hired)
				continue;

			if (string.IsNullOrEmpty(dinerItems[index].hireLeftTimeLabel.Text))
			{
				showDatas.Remove(dinerItems[index].ShowData);
				// Refresh Data.
				FillCard(false);
			}
		}

		if (GetCurrentTabEnum() == TabEnum.DinerList)
		{
			var nowTime = SysLocalDataBase.Inst.LoginInfo.NowTime;
			int qualityType = GetCurrentSubButtonData();
			var dinerPacakge = DinerData.GetDinerPackageByQualityType(qualityType);

			if (nowTime <= dinerPacakge.NextRefreshTime)
			{
				if (!GameUtility.EqualsFormatTimeString(autoRefreshTimeLable.Text, dinerPacakge.NextRefreshTime - nowTime))
					autoRefreshTimeLable.Text = GameUtility.Time2String(dinerPacakge.NextRefreshTime - nowTime);
			}
			else if (!waitForQuaryList)
			{
				var dinerBg = ConfigDatabase.DefaultCfg.DinerConfig.GetDinerBagByRefreshType(GetCurrentSubButtonData(), DinerConfig._DinerRefreshType.System);
				if (dinerBg == null)
				{
					Debug.Log("Not Found Common DinerBag in Quality " + GetCurrentSubButtonData());
					return;
				}
				waitForQuaryList = true;
				autoRefreshTimeLable.Text = string.Empty;

				RequestMgr.Inst.Request(new RefreshDinerListReq(dinerBg.BagId));
			}
		}
	}

	private int GetRefreshLimitCount(int qualityType, bool refreshAll)
	{
		int limitType = VipConfig._VipLimitType.DinerNorRefreshAllCount;

		switch (qualityType)
		{
			case DinerConfig._AvatarRarityType.Normal:
				limitType = refreshAll ? VipConfig._VipLimitType.DinerNorRefreshAllCount : VipConfig._VipLimitType.DinerNorRefreshSpecialCount;
				break;

			case DinerConfig._AvatarRarityType.Elite:
				limitType = refreshAll ? VipConfig._VipLimitType.DinerEliRefreshAllCount : VipConfig._VipLimitType.DinerEliRefreshSpecialCount;
				break;

			case DinerConfig._AvatarRarityType.Rare:
				limitType = refreshAll ? VipConfig._VipLimitType.DinerRareRefreshAllCount : VipConfig._VipLimitType.DinerRareRefreshSpecialCount;
				break;
		}

		return ConfigDatabase.DefaultCfg.VipConfig.GetVipLimitByVipLevel(SysLocalDataBase.Inst.LocalPlayer.VipLevel, limitType);
	}

	private void ShowDinerdByQuality(int qualityLevel)
	{
		ShowDinerInfo(false);

		// Set Hired Data.
		showDatas.Clear();
		currentPage = 0;
		for (int i = 0; i < DinerData.HireDiners.Count; i++)
		{
			var hireData = DinerData.HireDiners[i];

			var avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(hireData.AvatarGuid);
			if (ConfigDatabase.DefaultCfg.AvatarConfig.GetAvatarById(avatar.ResourceId).qualityLevel != qualityLevel)
				continue;

			ShowData showData = new ShowData();
			showData.hired = true;
			showData.dinerId = hireData.DinerId;
			showData.state = hireData.State;
			showData.qualityType = hireData.QualityType;
			showData.avatar = SysLocalDataBase.Inst.LocalPlayer.SearchAvatar(hireData.AvatarGuid);
			showDatas.Add(showData);
		}

		// No Hired Npc ,show Empty Label.
		emptyLabel.Text = showDatas.Count <= 0 ? GameUtility.FormatUIString("UIPnlAvatarDiner_Empty") : string.Empty;

		// Fill Data.
		FillCard(false);
	}

	private void ShowDinerListByQuality(int qualityType)
	{
		var dinerPackage = DinerData.GetDinerPackageByQualityType(qualityType);
		if (dinerPackage == null)
		{
			Debug.LogError("DinerPackage " + qualityType + "Not Found.");
			return;
		}

		ShowDinerInfo(true);

		var refreshCommonDinerBag = ConfigDatabase.DefaultCfg.DinerConfig.GetDinerBagByRefreshType(qualityType, DinerConfig._DinerRefreshType.Common);

		// Set Refresh All Cost.
		refreshAllLayout.HideChildObj(refreshAllLabel.gameObject, false);
		var refreshAllCosts = refreshCommonDinerBag.Costs;
		if (refreshAllCosts == null || refreshAllCosts.Count <= 0)
			refreshAllLayout.HideChildObj(refreshAllCostBox.gameObject, true);
		else
		{
			refreshAllLayout.HideChildObj(refreshAllCostBox.gameObject, false);
			refreshAllCostBox.SetData(refreshAllCosts[0].id);
			refreshAllCostBox.border.Text = refreshAllCosts[0].count.ToString();
		}

		// Set remain refresh count.
		refreshAllLabel.Text = GameUtility.GetUIString("UIPnlAvatarDiner_RefreshNoraml");
		refreshByCountryLabel.Text = GameUtility.GetUIString("UIPnlAvatarDiner_RefreshCountry");

		// Show Empty UI.
		emptyLabel.Text = dinerPackage.QueryDiners.Count <= 0 ? GameUtility.FormatUIString("UIPnlAvatarDiner_QueryListEmpty") : string.Empty;

		// Show DinerItem UI.
		this.showDatas.Clear();
		this.currentPage = 0;
		for (int i = 0; i < dinerPackage.QueryDiners.Count; i++)
		{
			var hireData = dinerPackage.QueryDiners[i];

			ShowData showData = new ShowData();
			showData.hired = false;
			showData.state = hireData.State;
			showData.dinerId = hireData.DinerId;
			showData.qualityType = qualityType;
			showData.avatar = new KodGames.ClientClass.Avatar();
			showData.avatar.ResourceId = hireData.AvatarResourceId;
			showData.avatar.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
			showData.avatar.LevelAttrib.Level = hireData.Level;
			showData.avatar.BreakthoughtLevel = hireData.BreakThroughLevel;
			showData.avatar.Domineer = hireData.DomineerData;
			showData.avatar.MeridianDatas = hireData.MeridianDatas;
			showDatas.Add(showData);
		}

		// Fill Data.
		FillCard(true);
	}

	private void FillCard(bool dinerInfo)
	{
		hideRoot.SetActive(true);

		for (int index = 0; index < dinerItems.Count; index++)
			dinerItems[index].Hide(true);

		// 重置当前页签
		int maxItemInPage = dinerItems.Count;

		int page = 0;
		if (showDatas.Count > 0)
		{
			page = showDatas.Count % maxItemInPage == 0 ? showDatas.Count / maxItemInPage - 1 : showDatas.Count / maxItemInPage;
			while (currentPage > page)
				currentPage--;
		}

		pageLabel.Text = page > 0 ? string.Format("{0}/{1}", currentPage + 1, page + 1) : string.Empty;

		// 如果是显示已雇佣列表，当前没有雇佣人物，显示推荐角色图片
		if (!dinerInfo)
		{
			recommendAvatarIcon.Hide(showDatas.Count > 0);
			recommendButton.Hide(showDatas.Count > 0);

			prePageBtn.Hide(currentPage - 1 < 0);
			nextPageBtn.Hide(currentPage + 1 > page);
		}
		else
		{
			recommendAvatarIcon.Hide(true);
			recommendButton.Hide(true);
			prePageBtn.Hide(true);
			nextPageBtn.Hide(true);
		}

		for (int index = currentPage * maxItemInPage; index < (currentPage + 1) * maxItemInPage && index < showDatas.Count; index++)
		{
			int itemIndex = index - currentPage * maxItemInPage;
			dinerItems[itemIndex].Hide(false);
			dinerItems[itemIndex].SetData(showDatas[index]);
		}
	}

	#region Event
	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickTabButton(UIButton btn)
	{
		OnChangeTab((TabEnum)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSubButton(UIButton btn)
	{
		SelectSubButton((int)btn.Data);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRecruite(UIButton btn)
	{
		OnChangeTab(TabEnum.DinerList);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickDinerInfo(UIButton btn)
	{
		UIElemAvatarDinerItem item = btn.Data as UIElemAvatarDinerItem;		
		GameUtility.ShowAssetInfoUI(item.ShowData.avatar.ResourceId);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBack(UIButton btn)
	{
		HideSelf();
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickPreButton(UIButton btn)
	{
		currentPage--;
		FillCard(GetCurrentTabEnum() == TabEnum.DinerList);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickNextButton(UIButton btn)
	{
		currentPage++;
		FillCard(GetCurrentTabEnum() == TabEnum.DinerList);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickForFire(UIButton btn)
	{
		UIElemAvatarDinerItem item = btn.Data as UIElemAvatarDinerItem;

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_OK");
		okCallback.Callback = (userData) =>
		{

			RequestMgr.Inst.Request(new FireDinerReq(item.ShowData.qualityType, item.ShowData.dinerId));
			return true;
		};

		MainMenuItem cancelCallback = new MainMenuItem();
		cancelCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		showData.SetData(
			GameUtility.GetUIString("UIDlgMessage_Title_Tips"),
			GameUtility.FormatUIString("UIDlgMessage_Message_FireDiner", ItemInfoUtility.GetAssetName(item.ShowData.avatar.ResourceId)),
			true,
			cancelCallback,
			okCallback);

		SysUIEnv.Instance.ShowUIModule(typeof(UIDlgMessage), showData);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickForHire(UIButton btn)
	{
		UIElemAvatarDinerItem item = btn.Data as UIElemAvatarDinerItem;
		RequestMgr.Inst.Request(new HireDinerReq(item.ShowData.qualityType, item.ShowData.dinerId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickForRenew(UIButton btn)
	{
		UIElemAvatarDinerItem item = btn.Data as UIElemAvatarDinerItem;
		RequestMgr.Inst.Request(new RenewDinerReq(item.ShowData.qualityType, item.ShowData.dinerId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickRefreshAll(UIButton btn)
	{
		var dinerBg = ConfigDatabase.DefaultCfg.DinerConfig.GetDinerBagByRefreshType(GetCurrentSubButtonData(), DinerConfig._DinerRefreshType.Common);
		if (dinerBg == null)
			Debug.Log("Not Found Common DinerBag in Quality " + GetCurrentSubButtonData());

		waitForQuaryList = true;
		RequestMgr.Inst.Request(new RefreshDinerListReq(dinerBg.BagId));
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickSpecialRefresh(UIButton btn)
	{
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlAvatarDinerRefresh, GetCurrentSubButtonData());
	}

	public void OnFireNpcSuccess()
	{
		// Refresh View.
		for (int i = 0; i < subTabButtons.Count; i++)
			if (subTabButtons[i].controlIsEnabled == false)
				ShowDinerdByQuality((int)subTabButtons[i].Data);
	}

	public void OnHireDinerSuccess(int dinerId)
	{
		SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAvatarDiner_HireSuccess"));

		var dinerItem = GetDinerItemByDinerId(dinerId);
		if (dinerItem == null)
			Debug.Log("Not Found DinerItem DinerId " + dinerId.ToString("X"));
		else
			dinerItem.HiredDinerSuccess();
	}

	public void OnRenewDinerSuccess(int dinerId)
	{
		var dinerItem = GetDinerItemByDinerId(dinerId);
		if (dinerItem == null)
			Debug.Log("Not Found DinerItem DinerId " + dinerId.ToString("X"));
		else
			dinerItem.HiredDinerSuccess();
	}

	public void OnRefreshListSuccess(int refreshType, int qualityType)
	{
		// Reset WaitRefresh.
		waitForQuaryList = false;

		// If Not AutoRefresh , show message.
		if (refreshType == DinerConfig._DinerRefreshType.System)
			SysUIEnv.Instance.ModulePool.ShowModule(typeof(UIPnlTipFlow), GameUtility.GetUIString("UIPnlAvatarDiner_RefreshSuccess"));

		// Refresh Type.
		ShowDinerListByQuality(qualityType);
	}

	#endregion

	private void ShowDinerInfo(bool showHireInfo)
	{
		if (dinerInfoRoot.activeInHierarchy != showHireInfo)
			dinerInfoRoot.SetActive(showHireInfo);

		if (dinerdInfoRoot.activeInHierarchy != !showHireInfo)
			dinerdInfoRoot.SetActive(!showHireInfo);
	}

	private void InitView()
	{
		// Hide Border Icon.
		blueBorderIcon.Hide(true);
		purpleBorderIcon.Hide(true);
		orangeBorderIcon.Hide(true);

		// Hide Diner Item.
		for (int index = 0; index < dinerItems.Count; index++)
			dinerItems[index].Hide(true);

		// Set Icon Data and hide.
		recommendAvatarIcon.SetData(ConfigDatabase.DefaultCfg.DinerConfig.RecommandAvatarIcon);
		recommendAvatarIcon.Hide(true);
		recommendButton.Hide(true);
		emptyLabel.Text = string.Empty;
	}

	// Set Border Icon.
	public void SetBorderIcon(AutoSpriteControlBase targetIcon, int quality)
	{
		AutoSpriteControlBase sourceIcon = null;
		switch (quality)
		{
			case 3:
				sourceIcon = blueBorderIcon;
				break;
			case 4:
				sourceIcon = purpleBorderIcon;
				break;
			case 5:
				sourceIcon = orangeBorderIcon;
				break;
		}

		if (sourceIcon != null)
			UIUtility.CopyIcon(targetIcon, sourceIcon);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnClickBackButton(UIButton button)
	{
		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlMainMenuBot))
			SysUIEnv.Instance.GetUIModule<UIPnlMainMenuBot>().SetLight(_UIType.UnKonw);
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);
	}
}
