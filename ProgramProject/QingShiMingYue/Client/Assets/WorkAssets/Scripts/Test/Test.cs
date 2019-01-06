#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using System.Threading;
using ClientServerCommon;

public class Test : MonoBehaviour
{
	public enum TestType
	{
		LoadMainScene,
		GamePreamble,
		Dialogue,
		SelectRole,
		ClassType,
		CampaignAutoScroll,
		CampaignActivityAutoScroll,
		BattleSpecialReward,
		ShopBuyTipDialog,
		ItemPreviewDialog,
		UITest,
	}

	public class SceneChangeDel : SysSceneManager.ISceneManagerListener
	{
		public void OnSceneWillChange(SysSceneManager manager, string currentScene, string newScene)
		{
			SysUIEnv uiEvn = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
			if (uiEvn != null)
				uiEvn.DisposeUIModules();

			// Free UI memory
			if (SysUIEnv.Instance != null)
				SysUIEnv.Instance.UnloadHidenUI();

			// Free Fx pool
			if (SysFx.Instance != null)
				SysFx.Instance.ReleaseAllFxPool();

			if (AudioManager.Instance != null)
				AudioManager.Instance.FreeMemory();

			// Free game object pool
			GameObjectPoolManager.Singleton.FreeMemory();

			// Free icon memory
			if (SysIconManger.Instance != null)
				SysIconManger.Instance.DestroyUnusedIcon();

			// Free cache
			if (ResourceCache.Instance != null)
				ResourceCache.Instance.FreeCache();

			// Free unused assert
			Resources.UnloadUnusedAssets();

			// Collect GC
			System.GC.Collect();
		}

		public void OnSceneChanged(SysSceneManager manager, string oldScene, string currentScene)
		{

		}
	}

	public TestType testType;

	private bool campaignAutoScroll = false;
	private bool campaignActivityAutoScroll = false;

	private static Test sInst;
	public static Test Inst { get { return sInst; } }

	public void Awake()
	{
		// If game main is restart, destroy it, GameMain must be a singleton.
		if (sInst != null)
		{
			GameObject.Destroy(gameObject);
			return;
		}

		sInst = this;
		DontDestroyOnLoad(this);

		// Splash.
		if (guiTexture != null)
		{
			float width = Screen.height * guiTexture.texture.width / guiTexture.texture.height;
			float x = (Screen.width - width) / 2;
			guiTexture.pixelInset = new Rect(x, 0, width, Screen.height);
		}
	}

	public void Start()
	{

		// Initialize timer
		KodGames.TimeEx.Initialize();

		// Set max frame rate
		Time.timeScale = 1;
		Application.targetFrameRate = 30;

		//if (GameDefines.resLoadingMode == ResouceLoadingMode.Folder)
		SysModuleManager.Instance.Initialize(this.gameObject);
		SysModuleManager.Instance.AddSysModule<SysGameStateMachine>(true);
		SysModuleManager.Instance.AddSysModule<SysPrefs>(true);

		// Add Resource manager
		SysModuleManager.Instance.AddSysModule<ResourceManager>(true);
		SysModuleManager.Instance.AddSysModule<ResourceCache>(true);

		// Add scene manager.
		var sysSceneManager = SysModuleManager.Instance.AddSysModule<SysSceneManager>(true);
		sysSceneManager.AddSceneManagerListener(new SceneChangeDel());

		// Add UI environment.
		SysModuleManager.Instance.AddSysModule<SysUIEnv>(true);

		// Initialize UI modules
		SysUIEnv uiEnv = SysModuleManager.Instance.GetSysModule<SysUIEnv>();
		foreach (var ui in GameDefines.GetAllUIModuleDatas())
			uiEnv.RegisterUIModule(ui.type, ui.prefabName, ui.moduleType, ui.linkedTypes, ui.hideOtherModules, ui.ignoreMutexTypes);

		SysModuleManager.Instance.AddSysModule<SysIconManger>(true);

		// Add Input system, need add after SysUIEvn.
		SysModuleManager.Instance.AddSysModule<SysInput>(true);

		// Add Fx system.
		SysModuleManager.Instance.AddSysModule<SysFx>(true);

		// Add AudioManager.
		SysModuleManager.Instance.AddSysModule<AudioManager>(true);

		// Add LocalDatabase system.
		SysModuleManager.Instance.AddSysModule<SysLocalDataBase>(true);

		// Notification system
		SysModuleManager.Instance.AddSysModule<SysNotification>(true);

		SysModuleManager.Instance.AddSysModule<SysTutorial>(true);

		SysModuleManager.Instance.AddSysModule<ActivityManager>(true);

		// Load GameConfig.
		LoadGameConfigFromAB();

		// Initialize request manager.
		RequestMgr.Inst.Initialze(10f, Test.Inst.OnRequestManagerBroken, Test.Inst.OnRequestManagerBusy, Test.Inst.OnRequestManagerTimeOut, null, null, null);

		// Init Player Data.
		InitLocalPlayer();

		StartCoroutine("LoadProcess");
	}

	private void InitLocalPlayer()
	{
		SysLocalDataBase.Inst.LoginInfo.LoginTime = DateTime.Now.Millisecond;
		SysLocalDataBase.Inst.LocalPlayer = new KodGames.ClientClass.Player();
		var player = SysLocalDataBase.Inst.LocalPlayer;
		player.LevelAttrib = new KodGames.ClientClass.LevelAttrib();
		player.LevelAttrib.Level = 1;
		player.Stamina.MaxPoint = 0;
		player.Stamina.GenerateTime = ConfigDatabase.DefaultCfg.GameConfig.staminaGenerateTime;
		//player.PVPStamina.MaxPoint = 0;
		//player.PVPStamina.GenerateTime = ConfigDatabase.DefaultCfg.GameConfig.pvpStaminaGenerateTime;

		var zones = new List<KodGames.ClientClass.Zone>();
		foreach (var zone in ConfigDatabase.DefaultCfg.CampaignConfig.zones)
		{
			var tempZone = new KodGames.ClientClass.Zone();
			tempZone.ZoneId = zone.zoneId;
			zones.Add(tempZone);
		}

		player.CampaignData.SetZones(zones);
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadProcess()
	{
		yield return null;

		if (Test.Inst.guiTexture != null)
			GameObject.Destroy(Test.Inst.guiTexture);

		switch (testType)
		{
			case TestType.LoadMainScene:
				StartCoroutine("LoadMainScene");
				break;
			case TestType.GamePreamble:
				SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGamePreamble));
				break;
			case TestType.Dialogue:
				TestCampaignDialogue();
				break;
			case TestType.SelectRole:
				TestTutorialSelectRole();
				break;
			case TestType.ClassType:
				TestClassType();
				break;
			case TestType.CampaignAutoScroll:
				TestCampaignAutoScroll();
				break;
			case TestType.CampaignActivityAutoScroll:
				TestCampaignActivityAutoScroll();
				break;
			case TestType.BattleSpecialReward:
				TestBattleSpecialReward();
				break;
			case TestType.ShopBuyTipDialog:
				TestShopBuyTipDialog();
				break;
			case TestType.ItemPreviewDialog:
				TestItemPreviewDialog();
				break;
			case TestType.UITest:
				TestUI();
				break;
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private System.Collections.IEnumerator LoadMainScene()
	{
		yield return null;

		int sceneId = ConfigDatabase.DefaultCfg.SceneConfig.mainSceneId;

		SceneConfig.Scene sceneCfg = ConfigDatabase.DefaultCfg.SceneConfig.GetSceneByID(sceneId);
		bool sceneLoaded = SysSceneManager.Instance.IsSceneLoaded(sceneCfg.levelName);
		Debug.Log("Loaded level name:" + Application.loadedLevelName);
		if (sceneLoaded == false)
		{
			yield return SysSceneManager.Instance.ChangeSceneAsync(sceneCfg.levelName);
			//			SysSceneManager.Instance.OnChangeSceneCompleted();
		}

		SysUIEnv.Instance.ShowUIModule(typeof(UIPnlGamePreamble));
	}

	private void TestCampaignDialogue()
	{
		int dialogId = ConfigDatabase.DefaultCfg.DialogueConfig.dialogueSets[0].dialogueSetId;
		SysUIEnv.Instance.GetUIModule<UITipAdviser>().ShowDialogue(dialogId, null);
	}

	private void TestTutorialSelectRole()
	{
		SysGameStateMachine.Instance.EnterState<GameState_SelectPlayerAvatar>();
	}

	private void TestClassType()
	{
		string currentStateName = "GameState_SelectPlayerAvatar";
		GameStateBase currentState = SysGameStateMachine.Instance.CurrentState;
		System.Type type = System.Type.GetType(currentStateName);
		Debug.Log(currentState.GetType().FullName);
		Debug.Log(type.FullName);
		if (currentState.GetType() == type)
			Debug.Log("currentState " + currentStateName + " " + true);
	}

	private void TestCampaignAutoScroll()
	{
		SysGameStateMachine.Instance.EnterState<GameState_Dungeon>();
		campaignAutoScroll = true;
	}

	private void TestCampaignActivityAutoScroll()
	{
		SysGameStateMachine.Instance.EnterState<GameState_ActivityDungeon>();
		campaignActivityAutoScroll = true;
	}

	private void TestBattleSpecialReward()
	{
		//int dungeonId = ConfigDatabase.DefaultCfg.CampaignConfig.zones[0].dungeons[0].id;
		//int resourceId = ConfigDatabase.DefaultCfg.AvatarConfig.avatars[0].id;
		//SysUIEnv.Instance.ShowUIModule(typeof(UIDlgBattleSpecialReward),resourceId,dungeonId);
	}

	private void TestShopBuyTipDialog()
	{
		int goodId = ConfigDatabase.DefaultCfg.GoodConfig.goods[0].id;

		// Show UIDlgShopBuyTips
		UIDlgShopBuyTips.ShowData showData = new UIDlgShopBuyTips.ShowData();
		showData.goodsId = goodId;
		showData.unitPrice = SysLocalDataBase.Inst.GetGoodsPriceAfterDiscount(goodId);
		showData.totalCount = 1;

		SysUIEnv.Instance.GetUIModule<UIDlgShopBuyTips>().ShowDialog(showData);
	}

	private void TestItemPreviewDialog()
	{
	}

	private void TestUI()
	{
		// 测试UIBistateInteractivePanel
		SysUIEnv.Instance.ShowUIModule(typeof(UITipAssistant));

		// 测试 UIdlgMessage.
		//string message = string.Empty;
		//for (int i = 0; i < 4; i++)
		//    message += GameUtility.GetUIString("DlgPinCheck_Warnning");
		//SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg("Title", message);

		// 测试 ProgressItem
		//SysUIEnv.Instance.ShowUIModule(typeof(UIPnlTest));

		// 测试包裹筛选
		//Debug.Log(PlayerPrefs.GetString(PlayerPrefs.GetString(GameDefines.gdAcc), string.Empty));

		//PackageFilterData.PackageFilter filterData =  PackageFilterData.Instance.GetPackgetFilterByType(IDSeg._AssetType.Avatar);

		//List<int> datas = filterData.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType);
		//datas.Add(AvatarConfig.Avatar._AvatarCountryType.QiGuo);
		//datas.Add(AvatarConfig.Avatar._AvatarCountryType.HanGuo);
		//datas.Add(AvatarConfig.Avatar._AvatarCountryType.ChuGuo);

		//datas = filterData.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType);
		//datas.Add(AvatarConfig.Avatar._AvatarTraitType.Heal);

		//datas = filterData.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);
		//datas.Add(1);
		//datas.Add(3);

		//filterData.Save();


		//PackageFilterData.PackageFilter filterData = PackageFilterData.Instance.GetPackgetFilterByType(IDSeg._AssetType.Avatar);

		//List<int> datas = filterData.GetFilterDataByType(PackageFilterData._FilterType.AvatarCountryType);
		//Debug.Log("Country");
		//foreach (var data in datas)
		//    Debug.Log(data);

		//datas = filterData.GetFilterDataByType(PackageFilterData._FilterType.AvatarTraitType);
		//Debug.Log("AvatarTraitType");
		//foreach (var data in datas)
		//    Debug.Log(data);

		//datas = filterData.GetFilterDataByType(PackageFilterData._FilterType.QualityLevel);
		//Debug.Log("QualityLevel");
		//foreach (var data in datas)
		//    Debug.Log(data);

		// 测试连战
		//List<KodGames.ClientClass.CombatResultAndReward> combatResultAndReward = new List<KodGames.ClientClass.CombatResultAndReward>();
		//{
		//    KodGames.ClientClass.CombatResultAndReward combat1 = new KodGames.ClientClass.CombatResultAndReward();
		//    combat1.DungeonReward = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward.Avatar = new List<KodGames.ClientClass.Avatar>();

		//    for (int index = 1; index < 4; index++)
		//    {
		//        KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
		//        avatar.ResourceId = ConfigDatabase.DefaultCfg.AvatarConfig.avatars[index].id;

		//        combat1.DungeonReward.Avatar.Add(avatar);
		//    }

		//    combat1.SurpriseReward = new KodGames.ClientClass.Reward();
		//    combat1.SurpriseReward.Equip = new List<KodGames.ClientClass.Equipment>();
		//    KodGames.ClientClass.Equipment equipment = new KodGames.ClientClass.Equipment();
		//    equipment.ResourceId = ConfigDatabase.DefaultCfg.EquipmentConfig.equipments[0].id;
		//    combat1.SurpriseReward.Equip.Add(equipment);

		//    combat1.DungeonReward_ExpSilver = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward_ExpSilver.Consumable = new List<KodGames.ClientClass.Consumable>();

		//    KodGames.ClientClass.Consumable consumable1 = new KodGames.ClientClass.Consumable();
		//    consumable1.Id = IDSeg._SpecialId.GameMoney;
		//    consumable1.Amount = 20;
		//    KodGames.ClientClass.Consumable consumable2 = new KodGames.ClientClass.Consumable();
		//    consumable2.Id = IDSeg._SpecialId.Experience;
		//    consumable2.Amount = 30;

		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable1);
		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable2);

		//    combatResultAndReward.Add(combat1);
		//}
		//{
		//    KodGames.ClientClass.CombatResultAndReward combat1 = new KodGames.ClientClass.CombatResultAndReward();
		//    combat1.DungeonReward = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward.Avatar = new List<KodGames.ClientClass.Avatar>();

		//    for (int index = 1; index < 5; index++)
		//    {
		//        KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
		//        avatar.ResourceId = ConfigDatabase.DefaultCfg.AvatarConfig.avatars[index].id;

		//        combat1.DungeonReward.Avatar.Add(avatar);
		//    }

		//    combat1.SurpriseReward = new KodGames.ClientClass.Reward();
		//    combat1.SurpriseReward.Equip = new List<KodGames.ClientClass.Equipment>();
		//    KodGames.ClientClass.Equipment equipment = new KodGames.ClientClass.Equipment();
		//    equipment.ResourceId = ConfigDatabase.DefaultCfg.EquipmentConfig.equipments[1].id;
		//    combat1.SurpriseReward.Equip.Add(equipment);

		//    combat1.DungeonReward_ExpSilver = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward_ExpSilver.Consumable = new List<KodGames.ClientClass.Consumable>();

		//    KodGames.ClientClass.Consumable consumable1 = new KodGames.ClientClass.Consumable();
		//    consumable1.Id = IDSeg._SpecialId.GameMoney;
		//    consumable1.Amount = 10;
		//    KodGames.ClientClass.Consumable consumable2 = new KodGames.ClientClass.Consumable();
		//    consumable2.Id = IDSeg._SpecialId.Experience;
		//    consumable2.Amount = 20;

		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable1);
		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable2);

		//    combatResultAndReward.Add(combat1);
		//}
		//{
		//    KodGames.ClientClass.CombatResultAndReward combat1 = new KodGames.ClientClass.CombatResultAndReward();
		//    combat1.DungeonReward = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward.Avatar = new List<KodGames.ClientClass.Avatar>();

		//    for (int index = 1; index < 4; index++)
		//    {
		//        KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
		//        avatar.ResourceId = ConfigDatabase.DefaultCfg.AvatarConfig.avatars[index].id;

		//        combat1.DungeonReward.Avatar.Add(avatar);
		//    }

		//    combat1.SurpriseReward = new KodGames.ClientClass.Reward();
		//    combat1.SurpriseReward.Equip = new List<KodGames.ClientClass.Equipment>();
		//    KodGames.ClientClass.Equipment equipment = new KodGames.ClientClass.Equipment();
		//    equipment.ResourceId = ConfigDatabase.DefaultCfg.EquipmentConfig.equipments[2].id;
		//    combat1.SurpriseReward.Equip.Add(equipment);

		//    combat1.DungeonReward_ExpSilver = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward_ExpSilver.Consumable = new List<KodGames.ClientClass.Consumable>();

		//    KodGames.ClientClass.Consumable consumable1 = new KodGames.ClientClass.Consumable();
		//    consumable1.Id = IDSeg._SpecialId.GameMoney;
		//    consumable1.Amount = 20;
		//    KodGames.ClientClass.Consumable consumable2 = new KodGames.ClientClass.Consumable();
		//    consumable2.Id = IDSeg._SpecialId.Experience;
		//    consumable2.Amount = 30;

		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable1);
		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable2);

		//    combatResultAndReward.Add(combat1);
		//}
		//{
		//    KodGames.ClientClass.CombatResultAndReward combat1 = new KodGames.ClientClass.CombatResultAndReward();
		//    combat1.DungeonReward = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward.Avatar = new List<KodGames.ClientClass.Avatar>();

		//    for (int index = 1; index < 4; index++)
		//    {
		//        KodGames.ClientClass.Avatar avatar = new KodGames.ClientClass.Avatar();
		//        avatar.ResourceId = ConfigDatabase.DefaultCfg.AvatarConfig.avatars[index].id;

		//        combat1.DungeonReward.Avatar.Add(avatar);
		//    }

		//    combat1.DungeonReward_ExpSilver = new KodGames.ClientClass.Reward();
		//    combat1.DungeonReward_ExpSilver.Consumable = new List<KodGames.ClientClass.Consumable>();

		//    KodGames.ClientClass.Consumable consumable1 = new KodGames.ClientClass.Consumable();
		//    consumable1.Id = IDSeg._SpecialId.GameMoney;
		//    consumable1.Amount = 20;
		//    KodGames.ClientClass.Consumable consumable2 = new KodGames.ClientClass.Consumable();
		//    consumable2.Id = IDSeg._SpecialId.Experience;
		//    consumable2.Amount = 30;

		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable1);
		//    combat1.DungeonReward_ExpSilver.Consumable.Add(consumable2);

		//    combatResultAndReward.Add(combat1);
		//}

		//SysUIEnv.Instance.ShowUIModule(_UIType.UIDlgContinueCombatResultDetail, combatResultAndReward);
	}

	public void OnResponseCombatErrOfEnterTimes(int zoneId, int dungeonId)
	{
		//Cost resetCost = ConfigDatabase.DefaultCfg.CampaignConfig.GetDungeonById(dungeonId).resetCosts[0];
		//int resetVipLvl = ConfigDatabase.DefaultCfg.VipConfig.GetVipLevelByOpenFunctionType(_OpenFunctionType.ClearCampaignCombatWithRealmoney);

		//string title = GameUtility.GetUIString("GamePreamble_6_2");
		//string message = GameUtility.GetUIString("GamePreamble_6_2");
		////string message = GameUtility.FormatUIString("UIDlgMessage_Mesksage_CampaignEnterTimesNotEngouth",resetCost.count,ItemInfoUtility.GetAssetName(resetCost.id),resetVipLvl);

		//UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();

		//MainMenuItem rechargeItem = new MainMenuItem();
		//rechargeItem.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Cancel");

		//MainMenuItem resetItem = new MainMenuItem();
		//resetItem.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_Use");
		//resetItem.Callback =
		//    (data) =>
		//    {
		//        RequestMgr.Inst.Request(new ResetDungeonCompleteTimesReq(zoneId, dungeonId));
		//        return true;
		//    };

		//showData.SetData(title, message, rechargeItem, resetItem);
		//SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
	}

	private void Update()
	{
		// Update game logic.
		Test.Inst.GameUpdate();
	}

	private void OnGUI()
	{
		if (campaignAutoScroll)
		{
			if (GUI.Button(new Rect(10, 250, 150, 100), "Start"))
			{
				StartCoroutine("CampaignScrollSequence");

				//CampaignSceneData.Instance.scroller.ScrollPosition = new Vector2(0, 0);

				//CampaignSceneData.Instance.ScrollView(
				//                    5,
				//                    0,
				//                    false,
				//                    null);

				//Invoke("CampaignAutoScroll", 1f);
			}
		}
		else if (campaignActivityAutoScroll)
		{
			if (GUI.Button(new Rect(10, 250, 150, 100), "Start"))
				StartCoroutine("CampaignActivityScrollSequence");
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator CampaignScrollSequence()
	{
		yield return null;

		for (int index = 0; index < 15; index++)
		{
			CampaignSceneData.Instance.ScrollView(
								index,
								0,
								false,
								null);

			yield return new WaitForSeconds(1f);

			CampaignSceneData.Instance.ScrollView(
				index + 1,
				index,
				true,
				null);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private IEnumerator CampaignActivityScrollSequence()
	{
		yield return null;

		for (int index = 0; index < 5; index++)
		{
			CampaignSceneData.Instance.ScrollView(
								index,
								0,
								false,
								null);

			yield return new WaitForSeconds(1f);

			CampaignSceneData.Instance.ScrollView(
				index,
				0,
				false,
				null);
		}
	}

	[System.Reflection.Obfuscation(Exclude = true, Feature = "renaming")]
	private void CampaignAutoScroll()
	{
		CampaignSceneData.Instance.ScrollView(
						5,
						4,
						true,
						null);
	}

	private void GameUpdate()
	{
		// Update Request manager.
		RequestMgr.Inst.OnUpdate();

		SysModuleManager.Instance.OnUpdate();
	}

	public void OnRequestManagerBusy(bool busy)
	{
		if (busy)
			SysUIEnv.Instance.ShowUIModule(typeof(UITipIndicator));
		else
			SysUIEnv.Instance.HideUIModule(typeof(UITipIndicator));
	}

	public void OnRequestManagerBroken(string brokenMessage, bool isRelogin)
	{
		// Remove busy indicator
		SysUIEnv.Instance.HideUIModule(typeof(UITipIndicator));

		UIDlgMessage.ShowData showData = new UIDlgMessage.ShowData();
		string title = GameUtility.GetUIString("ErrorTitle");
		string message = GameUtility.GetUIString("ErrorConnectionLost");

		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("ErrorConnectionReload");
		okCallback.CallbackData = null;
		okCallback.Callback = delegate(object data)
		{
			SysGameStateMachine.Instance.EnterState<GameState_Upgrading>();
			return true;
		};

		showData.SetData(title, message, false, null, okCallback);

		SysUIEnv.Instance.GetUIModule<UIDlgMessage>().ShowDlg(showData);
		SysUIEnv.Instance.UnlockUIInput();
	}

	// 网络超时回调
	public void OnRequestManagerTimeOut()
	{
		// 提示用户重试
		MainMenuItem okCallback = new MainMenuItem();
		okCallback.ControlText = GameUtility.GetUIString("UIDlgMessage_CtrlBtn_ReConnect");
		okCallback.Callback = (userData) =>
		{
			// 重试
			RequestMgr.Inst.Reconnect();
			return true;
		};
	}

	private bool LoadGameConfigFromAB()
	{
		// Load config
		ConfigDatabase.Initialize(new MathParserFactory(), false, false);
		ConfigDatabase.AddLogger(new ClientServerCommon.UnityLogger());
		ConfigDatabase.DelayLoadFileDel = ConfigDelayLoader.DelayLoadConfig;

		// Load manifest.
		string filePath = ResourceManager.Instance.GetLocalFilePath(PlayerPrefs.GetString("BuildProduct.GameConfigName", ""));
		AssetBundle ab = AssetBundle.CreateFromFile(filePath);
		if (ab == null)
		{
			Debug.LogError("Load Game Config failed : " + filePath);
			return false;
		}

		IFileLoader fileLoader = new FileLoaderFromAssetBundle(ab);
		ConfigSetting cfgSetting = GameDefines.SetupConfigSetting(new ConfigSetting(Configuration._FileFormat.Xml));
		ConfigDatabase.DefaultCfg.LoadConfig<ClientManifest>(fileLoader, cfgSetting);

		return true;
	}
}
#endif