using UnityEngine;
using ClientServerCommon;

// TODO : 处理不透明底板
public class UIPnlMainMenuBot : UIModule
{
	public UIButton menuBotBgBtn;
	public AutoSpriteControlBase homeBtn;
	public UIScrollList menuList;
	public UIBox activityNotify;
	public UIBox handBookNotify;//图鉴
	public UIBox avatarLineUpNotify;

	private const int ACTIVITY_MENU_INDEX = 4;
	private const int DEFAULT_ACTIVITY_JUMP_DES = _UIType.UIPnlActivityInnTab;
	private System.DateTime nextRefreshUILabelTime;

	private int[] MenuItem = new int[]
	{
		_UIType.UIPnlAvatar,//阵容 1
		_UIType.UIPnlPackageItemTab,//背包 1
		_UIType.UI_Dungeon,//历练 1
		_UIType.UIPnlHandBook,//图鉴 
		_UIType.UnKonw,//
		_UIType.UIPnlActivityExchangeTab,//兑换		 1
		_UIType.UI_ActivityDungeon,//秘境 1
		_UIType.UIPnlShopProp,//商城	1
		_UIType.UIPnlFriendTab,//好友 1		
		_UIType.UIPnlGuide,//帮助 1
		_UIType.UIPnlSetting,//设置 1
	};

	public void SetActivityJumpDest(int uitype)
	{
		int typeToSet = uitype;
		if (!ActivityManager.Instance.IsActivityTabAccessible(uitype))
			typeToSet = DEFAULT_ACTIVITY_JUMP_DES;

		MenuItem[ACTIVITY_MENU_INDEX] = typeToSet;
		menuList.sceneItems[ACTIVITY_MENU_INDEX].GetComponent<UIElemMainMenuBotItem>().LnkUI = typeToSet;
	}

	public override bool Initialize()
	{
		if (!base.Initialize())
			return false;

		// 注册阵容绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Avatar, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Equipment, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.CombatTurn, UpdateAvatarLineUpNotify);

		// 注册图鉴绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Item, UpdateHandBookNotify);

		// 注册活动按钮绿点的数据监听
		SysLocalDataBase.Inst.RegisterDataChangedDel(IDSeg._AssetType.Unknown, UpdateActivityNotify);

		return true;
	}

	public override void Dispose()
	{
		base.Dispose();

		// 取消注册阵容绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Avatar, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Equipment, UpdateAvatarLineUpNotify);
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.CombatTurn, UpdateAvatarLineUpNotify);

		// 取消注册活动绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Item, UpdateHandBookNotify);

		// 取消活动按钮绿点的数据监听
		SysLocalDataBase.Inst.DeleteDataChangedDel(IDSeg._AssetType.Unknown, UpdateActivityNotify);
	}

	public override bool OnShow(_UILayer layer, params object[] userDatas)
	{
		if (base.OnShow(layer, userDatas) == false)
			return false;

		if (menuList.sceneItems.Length <= 0)
			return true;

		for (int i = 0; i < menuList.sceneItems.Length && i < MenuItem.Length; i++)
		{
			if (i != ACTIVITY_MENU_INDEX)
				menuList.sceneItems[i].GetComponent<UIElemMainMenuBotItem>().LnkUI = MenuItem[i];
		}

		menuList.sceneItems[ACTIVITY_MENU_INDEX].GetComponent<UIElemMainMenuBotItem>().LnkUI = ActivityManager.Instance.ActivityJumpUI;

		// 阵容绿点
		UpdateAvatarLineUpNotify();

		// 活动绿点
		UpdateActivityNotify();

		// 图鉴绿点
		UpdateHandBookNotify();

		nextRefreshUILabelTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;

		return true;
	}

	private void Update()
	{
		System.DateTime nowTime = SysLocalDataBase.Inst.LoginInfo.NowDateTime;

		if (nowTime < nextRefreshUILabelTime || ActivityManager.Instance == null)
			return;

		if (false == ActivityManager.Instance.IsActivityTabAccessible(menuList.sceneItems[ACTIVITY_MENU_INDEX].GetComponent<UIElemMainMenuBotItem>().LnkUI))
			SetActivityJumpDest(ActivityManager.Instance.ActivityJumpUI);

		nextRefreshUILabelTime = nowTime.AddSeconds(1);

	}

	public void ShowFadeBg()
	{
		// 显示半透明背景, 横向纵向拉伸
		menuBotBgBtn.controlIsEnabled = true;
		menuBotBgBtn.xTextureTile = false;
		menuBotBgBtn.yTextureTile = false;
		menuBotBgBtn.SetSize(menuBotBgBtn.width, menuBotBgBtn.height);
	}

	public void ShowColorBg()
	{
		// 显示不透明底纹, 横向平铺
		menuBotBgBtn.controlIsEnabled = false;
		menuBotBgBtn.xTextureTile = true;
		menuBotBgBtn.yTextureTile = true;
		menuBotBgBtn.SetSize(menuBotBgBtn.width, menuBotBgBtn.height);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnIndexMenuClick(UIButton btn)
	{
		ShowMainScene();
	}

	public void ShowMainScene()
	{
		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlChatTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlChatTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlViewAvatar)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlViewAvatar));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlStartServerReward)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlStartServerReward));

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlRecharge))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlRecharge);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlRechargeVip))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlRechargeVip);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgDailyReward))
			SysUIEnv.Instance.HideUIModule(_UIType.UIDlgDailyReward);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIDlgAnnouncement))
			SysUIEnv.Instance.HideUIModule(_UIType.UIDlgAnnouncement);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlShop))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlShop);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAssistant))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlAssistant);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlWolfExpedition)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlWolfExpedition));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatarInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatarInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlEquipmentInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlEquipmentInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlSkillInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlSkillInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlConsumableInfo)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlConsumableInfo));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAvatar)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAvatar));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlHandBook)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlHandBook));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlAdventureMain)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlAdventureMain));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCombatTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendCombatTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCampaginWeekReward)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendCampaginWeekReward));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCampaginLastWeekRank)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendCampaginLastWeekRank));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendCampaginThisWeekRank)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendCampaginThisWeekRank));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendInfoTab)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendInfoTab));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendSelectFriends)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendSelectFriends));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendStart)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendStart));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendGuide)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendGuide));

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlFriendGuideDetail_1)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlFriendGuideDetail_1));

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlIllusionGuideDetail))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlIllusionGuideDetail);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlIllusionGuide))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlIllusionGuide);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAvatarIllusion))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlAvatarIllusion);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlIllusion))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlIllusion);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAdventureDelayReward))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlAdventureDelayReward);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlAdventureMain))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlAdventureMain);

		if (SysUIEnv.Instance.IsUIModuleShown(typeof(UIPnlDanMain)))
			SysUIEnv.Instance.HideUIModule(typeof(UIPnlDanMain));

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyList))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlGuildApplyList);

		if (SysUIEnv.Instance.IsUIModuleShown(_UIType.UIPnlGuildApplyInfo))
			SysUIEnv.Instance.HideUIModule(_UIType.UIPnlGuildApplyInfo);

		SetLight(_UIType.UnKonw);
		SysUIEnv.Instance.ShowUIModule(_UIType.UIPnlMainScene);

	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void OnMenuButtonClick(UIButton btn)
	{
		GameUtility.JumpUIPanel((int)btn.data);
	}

	public void SetLight(int uitype)
	{
		if (menuList.Count <= 0)
			return;

		for (int i = 0; i < menuList.Count; i++)
		{
			UIElemMainMenuBotItem botbtn = menuList.GetItem(i).gameObject.GetComponent<UIElemMainMenuBotItem>();
			if (botbtn == null)
				continue;

			botbtn.SetSelectedStat(botbtn.LnkUI == uitype);
		}
	}

	public void UpdateAvatarLineUpNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
			avatarLineUpNotify.Hide(!UIPnlAvatar.CheckUIAvatarNotify());
	}

	public void UpdateHandBookNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
			handBookNotify.Hide(!ItemInfoUtility.HaveMergeIllustration());
	}

	public void UpdateActivityNotify()
	{
		if (this.IsShown && !this.IsOverlayed)
		{
			bool showNotify = false;

			foreach (var state in SysLocalDataBase.Inst.LocalPlayer.FunctionStates)
			{
				if (state.id != GreenPointType.FixedTimeActivity &&
					state.id != GreenPointType.LevelRewardActivity &&
					state.id != GreenPointType.QinInfo &&
					state.id != GreenPointType.MonthCardFeedBack &&
					state.id != GreenPointType.InviteCodeReward)
					continue;

				if (state.isOpen)
				{
					showNotify = true;
					break;
				}
			}

			activityNotify.Hide(!showNotify);
		}
	}
}
