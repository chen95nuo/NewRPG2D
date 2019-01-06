using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using ClientServerCommon;
using KodGames;
using UnityEngine;

public static class GameDefines
{
	//-------------------------------------------------------------------------
	// Path.
	//-------------------------------------------------------------------------
	public const string objPath = "objects"; // Object path.
	public const string chrPath = objPath + "/character"; // Character path.
	public const string pfxPath = objPath + "/particle"; // Particle path.
	public const string uiPath = objPath + "/ui"; // UI path.
	public const string uiModulePath = uiPath + "/common"; // Common ui path.
	public const string uiEffectPath = uiPath + "/common";
	public const string otherObjPath = objPath + "/Other"; // Other objects path.
	public const string musicPath = "Audio/Music"; // Music path.
	public const string soundPath = "Audio/Sound"; // Sound path.
	public const string txtPath = "texts"; // Text path.
	public const string cfgPath = txtPath + "/configs"; // Config path.
	public const string localDataPath = txtPath + "/localdata"; // Local data path.
	public const string langPath = txtPath + "/Language"; // language path.
	public const string streamAssetPath = "Assets/StreamingAssets";
	public const string audioFormat = ".mp3";

	//-------------------------------------------------------------------------
	// Config files.
	//-------------------------------------------------------------------------
#if UNITY_EDITOR
#if UNITY_ANDROID
	public const string assetBundleFolder = "AssetBundle/Android";
#elif UNITY_IPHONE
	public const string assetBundleFolder = "AssetBundle/iPhone";
#else
	public const string assetBundleFolder = "AssetBundle";
#endif
#else
	public const string assetBundleFolder = "Asset";
#endif

	public static ConfigSetting SetupConfigSetting(ConfigSetting cfgSetting)
	{
		cfgSetting.ActionConfig = PathUtility.Combine(cfgPath, "ActionConfig");
		cfgSetting.AnimationConfig = PathUtility.Combine(cfgPath, "AnimationConfig");
		cfgSetting.AppleGoodConfig = PathUtility.Combine(cfgPath, "AppleGoodConfig");
		cfgSetting.ArenaConfig = PathUtility.Combine(cfgPath, "ArenaConfig");
		cfgSetting.AssetDescConfig = PathUtility.Combine(cfgPath, "AssetDescConfig");
		cfgSetting.AvatarAssetConfig = PathUtility.Combine(cfgPath, "AvatarAssetConfig");
		cfgSetting.AvatarConfig = PathUtility.Combine(cfgPath, "AvatarConfig");
		cfgSetting.CampaignConfig = PathUtility.Combine(cfgPath, "CampaignConfig");
		cfgSetting.ClientConfig = PathUtility.Combine(cfgPath, "ClientConfig");
		cfgSetting.ClientManifest = PathUtility.Combine(cfgPath, "ClientManifest");
		cfgSetting.DailySignInConfig = PathUtility.Combine(cfgPath, "DailySignInConfig");
		cfgSetting.EquipmentConfig = PathUtility.Combine(cfgPath, "EquipmentConfig");
		cfgSetting.GameConfig = PathUtility.Combine(cfgPath, "GameConfig");
		cfgSetting.GuideConfig = PathUtility.Combine(cfgPath, "GuideConfig");
		cfgSetting.GoodConfig = PathUtility.Combine(cfgPath, "GoodConfig");
		cfgSetting.ItemConfig = PathUtility.Combine(cfgPath, "ItemConfig");
		cfgSetting.LevelConfig = PathUtility.Combine(cfgPath, "LevelConfig");
		cfgSetting.PveConfig = PathUtility.Combine(cfgPath, "PveConfig");
		cfgSetting.SceneConfig = PathUtility.Combine(cfgPath, "SceneConfig");
		cfgSetting.SkillConfig = PathUtility.Combine(cfgPath, "SkillConfig");
		cfgSetting.StringsConfig = PathUtility.Combine(cfgPath, "Text");
		cfgSetting.VipConfig = PathUtility.Combine(cfgPath, "VipConfig");
		cfgSetting.DialogueConfig = PathUtility.Combine(cfgPath, "DialogueConfig");
		cfgSetting.LocalNotificationConfig = PathUtility.Combine(cfgPath, "LocalNotificationConfig");
		cfgSetting.LevelRewardConfig = PathUtility.Combine(cfgPath, "LevelRewardConfig");
		cfgSetting.TavernConfig = PathUtility.Combine(cfgPath, "TavernConfig");
		cfgSetting.TutorialConfig = PathUtility.Combine(cfgPath, "TutorialConfig");
		cfgSetting.QuestConfig = PathUtility.Combine(cfgPath, "QuestConfig");
		cfgSetting.MeridianConfig = PathUtility.Combine(cfgPath, "MeridianConfig");
		cfgSetting.StartServerRewardConfig = PathUtility.Combine(cfgPath, "StartServerRewardConfig");
		cfgSetting.MysteryShopConfig = PathUtility.Combine(cfgPath, "MysteryShopConfig");
		cfgSetting.InitPlayerConfig = PathUtility.Combine(cfgPath, "InitPlayerConfig");
		//cfgSetting.NpcConfig = PathUtility.Combine(cfgPath, "NpcConfig");
		cfgSetting.PositionConfig = PathUtility.Combine(cfgPath, "PositionConfig");
		cfgSetting.DomineerConfig = PathUtility.Combine(cfgPath, "DomineerConfig");
		cfgSetting.SuiteConfig = PathUtility.Combine(cfgPath, "SuiteConfig");
		cfgSetting.PartnerConfig = PathUtility.Combine(cfgPath, "PartnerConfig");
		cfgSetting.DinerConfig = PathUtility.Combine(cfgPath, "DinerConfig");
		cfgSetting.IllustrationConfig = PathUtility.Combine(cfgPath, "IllustrationConfig");
		cfgSetting.TaskConfig = PathUtility.Combine(cfgPath, "TaskConfig");
		cfgSetting.MelaleucaFloorConfig = PathUtility.Combine(cfgPath, "MelaleucaFloorConfig");
		cfgSetting.TreasureBowlConfig = PathUtility.Combine(cfgPath, "TreasureBowlConfig");
		cfgSetting.WolfSmokeConfig = PathUtility.Combine(cfgPath, "WolfSmokeConfig");
		cfgSetting.SpecialGoodsConfig = PathUtility.Combine(cfgPath, "SpecialGoodsConfig");
		cfgSetting.QinInfoConfig = PathUtility.Combine(cfgPath, "QinInfoConfig");
		cfgSetting.MonthCardConfig = PathUtility.Combine(cfgPath, "MonthCardConfig");
		cfgSetting.OperationConfig = PathUtility.Combine(cfgPath, "OperationConfig");
		cfgSetting.FriendCampaignConfig = PathUtility.Combine(cfgPath, "FriendCampaignConfig");
		cfgSetting.MarvellousAdventureConfig = PathUtility.Combine(cfgPath, "MarvellousAdventureConfig");
		cfgSetting.IllusionConfig = PathUtility.Combine(cfgPath, "IllusionConfig");
		cfgSetting.SevenElevenGiftConfig = PathUtility.Combine(cfgPath, "SevenElevenGiftConfig");
		cfgSetting.DanConfig = PathUtility.Combine(cfgPath, "DanConfig");
		cfgSetting.GuildConfig = PathUtility.Combine(cfgPath, "GuildConfig");
		cfgSetting.GuildPublicShopConfig = PathUtility.Combine(cfgPath, "GuildPublicShopConfig");
		cfgSetting.GuildPrivateShopConfig = PathUtility.Combine(cfgPath, "GuildPrivateShopConfig");
		cfgSetting.GuildExchangeShopConfig = PathUtility.Combine(cfgPath, "GuildExchangeShopConfig");
		cfgSetting.GuildStageConfig = PathUtility.Combine(cfgPath, "GuildStageConfig");
		cfgSetting.PowerConfig = PathUtility.Combine(cfgPath, "PowerConfig");
		cfgSetting.ChangeNameConfig = PathUtility.Combine(cfgPath, "ChangeNameConfig");
		cfgSetting.BeastConfig = PathUtility.Combine(cfgPath, "BeastConfig");

		return cfgSetting;
	}

	//-------------------------------------------------------------------------
	// Language.
	//-------------------------------------------------------------------------
	public const string strSrvErr = "ServerError";
	public const string strBlkUI = "UI";
	public const string strBlkNPCText = "NPC";
	public const string strBlkGuide = "Guide";
	public const string strBlkAssetDescs = "AssetDesc";
	public const string strBlkLoadingTips = "LoadingTips";

	//-------------------------------------------------------------------------
	// Game data key.
	//-------------------------------------------------------------------------
	public const string gdHasSvDt = "HasSaveData";
	public const string gdMscVlm = "MusicVolume";
	public const string gdSndVlm = "SoundVolume";
	public const string gdVdoQlt = "vdoQuality";
	public const string gdMscSwt = "MusicSwitch";
	public const string gdSndSwt = "SoundSwitch";
	public const string gdQuickLogin = "QuickLogin";
	public const string gdAcc = "Account";
	public const string gdPwdLength = "PasswordLength";
	public const string gdPwd = "Password";
	public const string gdisPasswordEncrypted = "IsPasswordEncrypted";
	public const string gdHasAccountLogined = "HasAccountLogined";
	public const string gdTtrl = "Tutorial";
	public const string gdRemoteIcon = "RemoteIcon";
	public const string gdGameConfigVersion = "GameConfigVersion";
	public const string gdCampaignCombatFinish = "CampaignCombatFinish";
	public const string gdQuickEquipHint = "QuickEquipHint";

	//-------------------------------------------------------------------------
	// Player markers.
	//-------------------------------------------------------------------------
	public const string lftFoot = "Bip01 L Foot"; // Left foot skeleton.
	public const string rftFoot = "Bip01 R Foot"; // Right foot skeleton.
	public const string handL = "marker_weaponL";
	public const string handR = "marker_weaponR";
	public const string weaponViewL = "Marker_C_L";
	public const string weaponViewR = "Marker_C_R";

	//-------------------------------------------------------------------------
	// Menu.
	//-------------------------------------------------------------------------
	public const float mnWalkSpd = 3; // Menu move speed.
	public const float mnRunSpd = 8; // Menu run speed.
	public static readonly Vector3 mnRoleScale = new Vector3(1.3f, 1.3f, 1.3f);
	public static readonly Vector3 btRoleScale = new Vector3(1.2f, 1.2f, 1.2f);

	//-------------------------------------------------------------------------
	// Battle.
	//-------------------------------------------------------------------------
	public const string avtShadow = otherObjPath + "/shadow_Player"; // Avatar shadow.
	public const float btAvatarRadius = 2.7f; // Battle avatar space.
	public const string btPfxClnCurWpn = "CloneCurrentWeapon"; // Pfx model name to clone current weapon.

	// Battle effect text.
	public const string uiTxtHurt = "UIFxHurt_HPdown";
	public const string uiTxtSP_Hurt = "UIFxHurt_SPdown";
	public const string uiFXBattleBar = "UIFxAvatarBattleBar";
	public const string uiFxSkillStart = "UIFxSkillStart";
	public const string uiFxSuperSkillStart_Sponsor = "UIFxSuperSkillStart_s";
	public const string uiFxSuperSkillStart_Opponent = "UIFxSuperSkillStart_o";
	public const string uiTxtHeal = "UIFxHurt_HPup";
	public const string uiTxtMiss = "UIFxHurt_ShanBi";
	public const string uiTxtCrtc = "UIFxHurt_BaoJi";
	public const string uiTxtSP_CrtcHurt = "UIFxHurt_SPdown_BaoJi";
	public const string uiTxtBlck = "UILblBlock";
	public const string uiTxtSklStart = "UILblSkillStart";
	public const string uiFXCounterAttackStart = "UIFxHurt_FanJi";
	public const string uiFX_XR = "UIFX_XR";
	public const string uiFx_Assistant = "UIFxGuideHand";
	public const string uiFx_Lottery = "UIFX_Q_CardLottery";
	public const string UIFX_Q_CardLottery_S4ZiSe = "UIFX_Q_CardLottery_S4ZiSe";
	public const string UIFX_Q_CardLottery_S5ChengSe = "UIFX_Q_CardLottery_S5ChengSe";
	public const string UIFX_Q_IconLottery_S4ZiSe = "UIFX_Q_IconLottery_S4ZiSe";
	public const string UIFX_Q_IconLottery_S5ChengSe = "UIFX_Q_IconLottery_S5ChengSe";
	public const string UIFX_Q_IconShinning_S4ZiSe = "UIFX_Q_IconShinning_S4ZiSe";
	public const string UIFX_Q_IconShinning_S5ChengSe = "UIFX_Q_IconShinning_S5ChengSe";
	public const string UIFX_Q_CardLottery_10timeCheng = "UIFX_Q_CardLottery_10timeCheng";//十连抽橙卡特效
	public const string UIFX_SelectedPlayer = "UIFX_SelectedPlayer";
	public const string UIFX_Q_IconUnlock = "UIFX_Q_IconUnlock";
	public const string UIFx_QinInfo_Right = "UIFX_Q_BaiKe_Right";
	public const string UIFx_QinInfo_Wrong = "UIFX_Q_BaiKe_Wrong";

	//Avatar Act number effect
	public const string fxAvatarBattleNum_1 = "P_Battle_Num_1";
	public const string fxAvatarBattleNum_2 = "P_Battle_Num_2";
	public const string fxAvatarBattleNum_3 = "P_Battle_Num_3";
	public const string fxAvatarBattleNum_4 = "P_Battle_Num_4";
	public const string fxAvatarBattleNum_5 = "P_Battle_Num_5";
	public const string fxAvatarBattleNum_6 = "P_Battle_Num_6";

	//-------------------------------------------------------------------------
	// Culling layer.
	//-------------------------------------------------------------------------
	public const int DefaultLayer = 0;
	public const int ignoreRayTestLayer = 2;
	public const int UIRaycastLayer = 8;
	public const int UIIgnoreRaycastLayer = 9;
	public const int UIInvisableLayer = 10;
	public const int UISceneRaycastLayer = 11;
	public const int SceneMaskLayer = 12;
	public const int AvatarCaptureLayer = 13;

	public const int lockedUILayer = 14;
	public const int lockedSceneUILayer = 15;
	public const int lockedInvisableUILayer = 16;

	//-------------------------------------------------------------------------
	// Animation.
	//-------------------------------------------------------------------------
	public const string anmCrvObj = otherObjPath + "/AnimationCurve"; // Other objects path.
	public const string anmFocusObj = "FocusObjAnim"; // Focus object animation.
	public const float animCrossFadeTime = 0.1f; // Animation cross time.
	public static Color defaultAvatarColor = new Color32(130, 130, 130, 255);

	//-------------------------------------------------------------------------
	// Audios.
	//-------------------------------------------------------------------------
	public const string uiSndOnOver = "";
	public const string uiSndOnClick = "";
	public const string sndLose = "YouLose";
	public const string sndWin = "YouWin";
	public const string meridianOpen = "SkillStart";
	public const string danAlchemy = "SkillStart_00";
	public const string menu_Blade2 = "Menu_Blade2";
	public const string money = "Money";
	//-------------------------------------------------------------------------
	// UIs.
	//-------------------------------------------------------------------------
	public static readonly Vector2 uiDefaultScreenSize = new Vector2(320, 480); // Default ui screen size.
	public const string uiMgr = "UIManager";
	public const string uiCam = "UICamera";
	public const string uiCnt = "UIContainer";
	public const string uiFntMgr = "UIFontManager";
	public const string uiPoolMgr = "PoolManager";
	public const string uiScreenMask = uiPath + "/Common/UIScreenMask";

	public const string defaultUIAvatarMaleAnim = "Idle_01";
	public const string defaultUIAvatarFemaleAnim = "Idle_01";

	public const string assistantParticle = "UIFX_Q_IconActive";
	public const string assistantParticleStrength = "UIFX_Q_IconActive_PiXiu";
	public const string campaignGuidParticle = "p_JiangHU";
	public const string tavernGuidParticle = "UIFX_JiuGuan";
	public const string centalButtonParticle = "UIFX_IconAlive";

	public const string campaignUnLockParticle = "p_FuBenJieSuo";
	public const string campaignNewPatricle = "p_FuBenNew";
	public const string campaignDispearPatricle = "p_Q_ShowBuilding";

	public const string centralCityBuildingHighlightFx = "p_MainCityBuilding";
	public const string centralCityFirstRecharge = "UIFX_Q_ShouYeAcitve";

	public const string tutorialPatricle = "UIFxGuidePoint";
	public const string tutotialNPC = "UIFX_PiXiu";
	public const string tutorialHandStatic = "UIFXGuideHand_Static";

	public const string equipOpenParticle = "UIFX_JingMaiEmpty";
	public const string equipActiveParticle = "UIFX_Q_IconUnlock";//"UIFX_ZhuWeiKaiQi";

	public const string combatFailButtonParticle = "UIFx_Upgrade";
	public const string activityCycleGetRewardParticle = "UIFX_WinItem";

	public const string campaignDungeonOpenParticle = "UIFX_ZhuWeiKaiQi";
	public const string campaignDungeonStarReward = "UIFX_Q_ShouYeAcitve";
	public const string campaignZoneLine = "BIGMAP_OBJ_11";

	public const string battleResultWin = "UIFX_Q_BattleResult_Win";
	public const string battleResultLose = "UIFX_Q_BattleResult_Lose";
	public const string battleStarParticle = "UIFX_Q_BattleResultStar";

	public const string levelUpParticle = "UIFX_Q_LevelUp_FontFX";

	public const string getBoxParticle = "p_Q_GetBox";

	public const string wolfClickMe_Font = "p_Q_ClickMe_Font";
	public const string wolfClickMe = "p_Q_ClickMe";

	public const string collisionQianJiLou = "p_Q_Spe_QianJiLou_Get";
	public const string resetBtnEffect = "UIFX_Q_ShouYeAcitve";

	public const string p_Q_GGYD_Mission = "p_Q_GGYD_Mission";
	public const string p_Q_GGYD_Select = "p_Q_GGYD_Select";

	public const string towerSceneDoor = "TowerSceneDoor3DUI";

	public const string wolfDoorPlate = "NumberPlate";
	public const string wolfDoorName = "NameLabel";
	public const string danCreateEffect = "UIFX_Q_LianDan_Create";
	public const string danDanEffect = "UIFX_Q_LianDan_Dan";
	public const string danGetEffect = "UIFX_Q_IconGetItem2";
	public const string furnaceEffect = "UIFX_Q_LianDan_Furnace";
	public const string danFlyEffect = "UIFX_Q_LianDan_DanFly";
	public const string danFurnace = "UIFX_Q_LianDan_Static";

	public const string towerSceneDoor3DUI = "TowerSceneDoor3DUI";
	public const string numberPlate = "NameLabel";
	public const string ghmg_shadow = "_shadow";
	public const string monsterIcon = "MonsterIcon";

	public const string equipPart = "UIFX_Q_IconLVUP";
	public const string equipZoom = "UIFX_Q_IconZoom";
	public const string partFlyEffect = "UIFX_Q_LianDan_DanFly2";
	public const string partGetEffect = "UIFX_Q_IconGetItem";
	public const string partLevelUp_Success = "UIFX_Q_LevelUp_Success";
	public const string part_big = "UIFX_Q_IconFx_big";

	public struct UIModuleData
	{
		public Type type;
		public string prefabName;
		public int moduleType;
		public Type[] linkedTypes;
		public bool hideOtherModules;
		public Type[] ignoreMutexTypes;

		public UIModuleData(System.Type type, string prefabName) : this(type, prefabName, _UIType.UnKonw, null, false, null) { }

		public UIModuleData(System.Type type, string prefabName, int moduleType) : this(type, prefabName, moduleType, null, false, null) { }

		public UIModuleData(System.Type type, string prefabName, int moduleType, System.Type[] linkedTypes) : this(type, prefabName, moduleType, linkedTypes, false, null) { }

		public UIModuleData(System.Type type, string prefabName, int moduleType, System.Type[] linkedTypes, bool hideOtherModules) : this(type, prefabName, moduleType, linkedTypes, hideOtherModules, null) { }

		public UIModuleData(System.Type type, string prefabName, int moduleType, System.Type[] linkedTypes, bool hideOtherModules, System.Type[] ignoreMutexTypes)
		{
			this.type = type;
			this.prefabName = prefabName;
			this.moduleType = moduleType;
			this.linkedTypes = linkedTypes;
			this.hideOtherModules = hideOtherModules;
			this.ignoreMutexTypes = ignoreMutexTypes;
		}
	}

	public static UIModuleData[] GetAllUIModuleDatas()
	{
		UIModuleData[] datas =
		{
            new UIModuleData(typeof(UIDlgIllustrationBatchSynthesis),"UIDlgIllustrationBatchSynthesis"),
            new UIModuleData(typeof(UIDlgComposeReward),"UIDlgComposeReward",_UIType.UIDlgComposeReward),

			new UIModuleData(typeof(UIDlgLogin), "UIDlgLogin", _UIType.UIDlgLogin),
			new UIModuleData(typeof(UIDlgLoginPlatform), "UIDlgLoginPlatform"),
			new UIModuleData(typeof(UIDlgRegister),	"UIDlgRegister", _UIType.UIDlgRegister),
			new UIModuleData(typeof(UIPnlSelectArea), "UIPnlSelectArea", _UIType.UIPnlSelectArea),
			new UIModuleData(typeof(UIPnlLoginBackground), "UIPnlLoginBackground", _UIType.UIPnlLoginBackground),

			new UIModuleData(typeof(UIDlgAffordCost), "UIDlgAffordCost", _UIType.UIDlgAffordCost),
			new UIModuleData(typeof(UIDlgAnnouncement), "UIDlgAnnouncement", _UIType.UIDlgAnnouncement),
			new UIModuleData(typeof(UIDlgAttributeDetailTip), "UIDlgAttributeDetailTip", _UIType.UIDlgAttributeDetailTip),
			new UIModuleData(typeof(UIDlgDailyReward), "UIDlgDailyReward", _UIType.UIDlgDailyReward),
			new UIModuleData(typeof(UIDlgFeedBack), "UIDlgFeedBack", _UIType.UIDlgFeedBack),
			new UIModuleData(typeof(UIDlgBeforeBattleLineUp), "UIDlgBeforeBattleLineUp", _UIType.UIDlgBeforeBattleLineUp),
			new UIModuleData(typeof(UIDlgBeforeBattleRecuite),"UIDlgBeforeBattleRecuite",_UIType.UIDlgBeforeBattleRecuite),
			//Bag EquipmentFilter
			new UIModuleData(typeof(UIDlgPackageEquipFilter),"UIDlgPackageEquipFilter"),
			//Bag SkillFilter
			new UIModuleData(typeof(UIDlgPackageSkillFilter),"UIDlgPackageSkillFilter"),
			new UIModuleData(typeof(UIDlgItemGetWay),"UIDlgItemGetWay",_UIType.UIDlgItemGetWay),

			new UIModuleData(typeof(UIDlgMessage), "UIDlgMessage", _UIType.UIDlgMessage),
			new UIModuleData(typeof(UIDlgModifyPwd), "UIDlgModifyPwd", _UIType.UIDlgModifyPwd),
			new UIModuleData(typeof(UIDlgPlayerAttrTip), "UIDlgPlayerAttrTip", _UIType.UIDlgPlayerAttrTip),

			new UIModuleData(typeof(UIDlgOpenPackage), "UIDlgOpenPackage"),
			new UIModuleData(typeof(UIDlgPhoneNumberVerify), "UIDlgPhoneNumberVerify"),
			new UIModuleData(typeof(UIDlgActivationCode), "UIDlgActivationCode"),

			new UIModuleData(typeof(UIPnlRecharge), "UIPnlRecharge", _UIType.UIPnlRecharge),
			new UIModuleData(typeof(UIPnlRechargeVip), "UIPnlRechargeVip",  _UIType.UIPnlRechargeVip, new Type[]{typeof(UIPnlRechargeVip), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),

			new UIModuleData(typeof(UIDlgSetAccontBinding),	"UIDlgSetAccontBinding", _UIType.UIDlgSetAccontBinding),
			new UIModuleData(typeof(UIDlgShopBuyTips), "UIDlgShopBuyTips", _UIType.UIDlgShopBuyTips),
			new UIModuleData(typeof(UIDlgShopGiftPreview), "UIDlgShopGiftPreview", _UIType.UIDlgShopGiftPreview),
			new UIModuleData(typeof(UIElemTemplate), "UIElemTemplate", _UIType.UIElemTemplate),

			new UIModuleData(typeof(UIPnlActivityInnTab), "UIPnlActivityInnTab", _UIType.UIPnlActivityInnTab, new Type[]{typeof(UIPnlActivityInnTab), typeof(UIPnlActivityTab)}, true),
			new UIModuleData(typeof(UIPnlActivityQinInfo), "UIPnlActivityQinInfo", _UIType.UIPnlActivityQinInfo, new Type[]{typeof(UIPnlActivityQinInfo), typeof(UIPnlActivityTab)}, true),
			new UIModuleData(typeof(UIPnlActivityMonthCardTab), "UIPnlActivityMonthCardTab", _UIType.UIPnlActivityMonthCardTab, new Type[]{typeof(UIPnlActivityMonthCardTab), typeof(UIPnlActivityTab)}, true),
			new UIModuleData(typeof(UIPnlActivityTab), "UIPnlActivityTab", _UIType.UIPnlActivityTab, new Type[]{typeof(UIPnlMainMenuBot), typeof(UIPnlActivityTab), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlArena), "UIPnlArena", _UIType.UIPnlArena, new Type[]{typeof(UIPnlArena), typeof(UIPnlPlayerInfos), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIPnlArenaBattleResult), "UIPnlArenaBattleResult", _UIType.UIPnlArenaBattleResult),
			new UIModuleData(typeof(UIPnlAvatar), "UIPnlAvatar", _UIType.UIPnlAvatar, new Type[]{typeof(UIPnlAvatar), typeof(UIPnlMessageFlow)}, true,new Type[]{typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlAvatarBreakThrough), "UIPnlAvatarBreakThrough", _UIType.UIPnlAvatarBreakThrough, new Type[]{typeof(UIPnlAvatarPowerUpTab), typeof(UIPnlAvatarBreakThrough), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageAvatarTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlAvatarInfo), "UIPnlAvatarInfo", _UIType.UIDlgAvatarInfo),
			new UIModuleData(typeof(UIPnlAvatarLevelUp), "UIPnlAvatarLevelUp", _UIType.UIPnlAvatarLevelUp, new Type[]{typeof(UIPnlAvatarPowerUpTab), typeof(UIPnlAvatarLevelUp), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageAvatarTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlAvatarPowerUpTab), "UIPnlAvatarPowerUpTab", _UIType.UIPnlAvatarPowerUpTab, new Type[]{typeof(UIPnlAvatarPowerUpTab), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAvatarTrainningTab), "UIPnlAvatarTrainningTab",_UIType.UIPnlAvatarTrainningTab, new Type[]{typeof(UIPnlAvatarPowerUpTab), typeof(UIPnlAvatarTrainningTab), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageAvatarTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlAvatarMeridianTab), "UIPnlAvatarMeridianTab",_UIType.UIPnlAvatarMeridianTab, new Type[]{typeof(UIPnlAvatarPowerUpTab), typeof(UIPnlAvatarMeridianTab), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageAvatarTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIDlgAvatarMeridian), "UIDlgAvatarMeridian",_UIType.UIDlgAvatarMeridian),
			new UIModuleData(typeof(UIPnlBattleBar), "UIPnlBattleBar", _UIType.UIPnlBattleBar),
			new UIModuleData(typeof(UIPnlBattleRoleInfo), "UIPnlBattleRoleInfo", _UIType.UIPnlBattleRoleInfo),
			new UIModuleData(typeof(UIPnlBattleTab), "UIPnlBattleTab", _UIType.UIPnlBattleTab, new Type[]{typeof(UIPnlBattleTab), typeof(UIPnlPlayerInfos), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlCampaignActivityScene), "UIPnlCampaignActivityScene", _UIType.UIPnlCampaignActivityScene,new Type[]{typeof(UIPnlCampaignCityPlayerInfo), typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlCampaignBattleResult), "UIPnlCampaignBattleResult", _UIType.UIPnlCampaignBattleResult),
			new UIModuleData(typeof(UIPnlCampaignCityPlayerInfo), "UIPnlCampaignCityPlayerInfo",_UIType.UIPnlCampaignCityPlayerInfo),
			new UIModuleData(typeof(UIPnlCampaignScene), "UIPnlCampaignScene", _UIType.UIPnlCampaignScene,new Type[]{typeof(UIPnlCampaignCityPlayerInfo), typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlCampaignSceneBottom), "UIPnlCampaignSceneBottom", _UIType.UIPnlCampaignSceneBottom),
			new UIModuleData(typeof(UIPnlCampaignSceneMid), "UIPnlCampaignSceneMid", _UIType.UIPnlCampaignSceneMid,new Type[]{typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignCityPlayerInfo)}),
			new UIModuleData(typeof(UIDlgAvatarLineUpGuide), "UIDlgAvatarLineUpGuide"),
			new UIModuleData(typeof(UIDlgDungeonTravelShop), "UIDlgDungeonTravelShop"),
			new UIModuleData(typeof(UIDlgDungeonStarReward), "UIDlgDungeonStarReward",_UIType.UIDlgDungeonStarReward),
			new UIModuleData(typeof(UIDlgCampaignContinue), "UIDlgCampaignContinue"),
			new UIModuleData(typeof(UIPnlTravelShopGuid), "UIPnlTravelShopGuid",_UIType.UIPnlTravelShopGuid,new Type[]{typeof(UIPnlTravelShopGuid) , typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlCentralCityPlayerInfo), "UIPnlCentralCityPlayerInfo", _UIType.UIPnlCentralCityPlayerInfo),
			new UIModuleData(typeof(UIPnlConsumableInfo), "UIPnlConsumableInfo", _UIType.UIDlgConsumableInfo),
			new UIModuleData(typeof(UIPnlEmail), "UIPnlEmail", _UIType.UIPnlEmail, new Type[]{typeof(UIPnlEmail), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIPnlEquipmentInfo), "UIPnlEquipmentInfo", _UIType.UIPnlEquipmentInfo),
			new UIModuleData(typeof(UIPnlEquipmentLevelup), "UIPnlEquipmentLevelup", _UIType.UIPnlEquipmentLevelup, new Type[]{typeof(UIPnlEquipmentPowerUpTab), typeof(UIPnlEquipmentLevelup), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageEquipTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlEquipmentPowerUpTab), "UIPnlEquipmentPowerUpTab", _UIType.UIPnlEquipmentPowerUpTab, new Type[]{typeof(UIPnlEquipmentPowerUpTab), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlEquipmentRefine), "UIPnlEquipmentRefine", _UIType.UIPnlEquipmentRefine, new Type[]{typeof(UIPnlEquipmentPowerUpTab), typeof(UIPnlEquipmentRefine), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageEquipTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlGamePreamble), "UIPnlGamePreamble", _UIType.UIPnlGamePreamble, null, true),
			new UIModuleData(typeof(UIPnlGuide), "UIPnlGuide", _UIType.UIPnlGuide,new Type[]{typeof(UIPnlMessageFlow), typeof(UIPnlMainMenuBot)}, true),
			new UIModuleData(typeof(UIPnlGuideDetail), "UIPnlGuideDetail", _UIType.UIPnlGuideDetail),
			new UIModuleData(typeof(UIPnlLevelRewardTab), "UIPnlLevelRewardTab", _UIType.UIPnlLevelRewardTab, new Type[]{typeof(UIPnlLevelRewardTab), typeof(UIPnlActivityTab)}, true),
			new UIModuleData(typeof(UIPnlLoading), "UIPnlLoading", _UIType.UIPnlLoading),
			new UIModuleData(typeof(UIPnlMainMenuBot), "UIPnlMainMenuBot", _UIType.UIPnlMainMenuBot),
			new UIModuleData(typeof(UIPnlMainScene), "UIPnlMainScene", _UIType.UIPnlMainScene, new Type[]{typeof(UIPnlCentralCityPlayerInfo), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIPnlMessageFlow), "UIPnlMessageFlow", _UIType.UIPnlMessageFlow),
			new UIModuleData(typeof(UIPnlPackageAvatarTab), "UIPnlPackageAvatarTab", _UIType.UIPnlPackageAvatarTab, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlPackageAvatarTab), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlPackageEquipTab), "UIPnlPackageEquipTab", _UIType.UIPnlPackageEquipTab, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlPackageEquipTab), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlPackageItemTab), "UIPnlPackageItemTab", _UIType.UIPnlPackageItemTab, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlPackageItemTab), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlPackageSell), "UIPnlPackageSell", _UIType.UIPnlPackageSell, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlPackageSell), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIDlgPackageSellSceondSure), "UIDlgPackageSellSceondSure", _UIType.UIDlgPackageSellSceondSure, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlPackageSell), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlPackageSkillTab), "UIPnlPackageSkillTab", _UIType.UIPnlPackageSkillTab, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlPackageSkillTab), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlPackageTab), "UIPnlPackageTab", _UIType.UIPnlPackageTab, new Type[]{typeof(UIPnlPackageTab), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlPlayerInfos), "UIPnlPlayerInfos", _UIType.UIPnlPlayerInfos),

			new UIModuleData(typeof(UIPnlSelectAvatarList), "UIPnlSelectAvatarList", _UIType.UIPnlSelectAvatarList,new Type[]{typeof(UIPnlSelectAvatarList),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlSelectEquipmentList), "UIPnlSelectEquipmentList", _UIType.UIPnlSelectEquipmentList,new Type[]{typeof(UIPnlSelectEquipmentList),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlSelectSkillList), "UIPnlSelectSkillList", _UIType.UIPnlSelectSkillList,new Type[]{typeof(UIPnlSelectSkillList),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlSetting), "UIPnlSetting", _UIType.UIPnlSetting, new Type[]{typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIPnlShop), "UIPnlShop", _UIType.UIPnlShop,  new Type[]{typeof(UIPnlShop), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlShopGift), "UIPnlShopGift", _UIType.UIPnlShopGift, new Type[]{typeof(UIPnlShop), typeof(UIPnlShopGift), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlShopProp), "UIPnlShopProp", _UIType.UIPnlShopProp, new Type[]{typeof(UIPnlShop), typeof(UIPnlShopProp), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlTarven), "UIPnlTarven", _UIType.UIPnlTarven, new Type[]{typeof(UIPnlTarven), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlShopWine), "UIPnlShopWine", _UIType.UIPnlShopWine, new Type[]{typeof(UIPnlTarven), typeof(UIPnlShopWine), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlRecruitShow), "UIPnlRecruitShow"),
			new UIModuleData(typeof(UIPnlShopMystery), "UIPnlShopMystery", _UIType.UIPnlShopMystery, new Type[]{typeof(UIPnlTarven), typeof(UIPnlShopMystery), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlShopMysteryRefresh),"UIPnlShopMysteryRefresh",_UIType.UIPnlShopMysteryRefresh,new Type[]{typeof(UIPnlShopMysteryRefresh),typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow), typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlSkillInfo), "UIPnlSkillInfo", _UIType.UIDlgSkillInfo),
			new UIModuleData(typeof(UIPnlSkillPowerUp), "UIPnlSkillPowerUp", _UIType.UIPnlSkillPowerUp, new Type[]{typeof(UIPnlSkillPowerUp), typeof(UIPnlPlayerInfos), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)},true,new Type[]{typeof(UIPnlPackageSkillTab),typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlTip), "UIPnlTip", _UIType.UIPnlTip),
			new UIModuleData(typeof(UIPnlTipFlow), "UIPnlTipFlow", _UIType.UIPnlTipFlow),
			new UIModuleData(typeof(UIPnlViewAvatar), "UIPnlViewAvatar", _UIType.UIDlgViewAvatar),
			new UIModuleData(typeof(UITipIndicator), "UITipIndicator", _UIType.UITipIndicator),

			new UIModuleData(typeof(UIEffectPowerUp), "UIEffectPowerUp", _UIType.UIEffectPowerUp),
			new UIModuleData(typeof(UIEffectOpenBox), "UIEffectOpenBox", _UIType.UIEffectOpenBox),

			new UIModuleData(typeof(UIPnlSelectPlayerAvatar),"UIPnlSelectPlayerAvatar", _UIType.UIPnlSelectPlayerAvatar,null,true),

			new UIModuleData(typeof(UITipAdviser),  "UITipAdviser", _UIType.UITipAdviser),
			new UIModuleData(typeof(UITipHelp), "UITipHelp", _UIType.UITipHelp),
			new UIModuleData(typeof(UITipDragHelp), "UITipDragHelp",_UIType.UITipDargHelp),
			new UIModuleData(typeof(UIPnlTutorialBattleResult), "UIPnlTutorialBattleResult", _UIType.UIPnlTutorialBattleResult),
			new UIModuleData(typeof(UIEffectLottery), "UIEffectLottery", _UIType.UIEffectLottery),

			new UIModuleData(typeof(UIDlgPlayerLevelUp), "UIDlgPlayerLevelUp", _UIType.UIDlgPlayerLevelUp),

			new UIModuleData(typeof(UIDlgAvatarCheerDetail), "UIDlgAvatarCheerDetail", _UIType.UIDlgAvatarCheerDetail),
			new UIModuleData(typeof(UIDlgConfirmExchange), "UIDlgConfirmExchange", _UIType.UIDlgConfirmExchange),
			new UIModuleData(typeof(UIPnlChooseCard), "UIPnlChooseCard", _UIType.UIPnlChooseCard, new Type[]{typeof(UIPnlMainMenuBot), typeof(UIPnlChooseCard), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlActivityExchangeTab), "UIPnlActivityExchangeTab", _UIType.UIPnlActivityExchangeTab, new Type[]{typeof(UIPnlActivityExchangeTab), typeof(UIPnlPlayerInfos), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIDlgCampaignDiffChose), "UIDlgCampaignDiffChose", _UIType.UIDlgCampaignDiffChose),

			new UIModuleData(typeof(UIDlgOpenBox), "UIDlgOpenBox", _UIType.UIDlgOpenBox),
			new UIModuleData(typeof(UIDlgContinueCombatResultDetail), "UIDlgContinueCombatResultDetail", _UIType.UIDlgContinueCombatResultDetail),
			new UIModuleData(typeof(UIDlgCampaignContinueBattleResult), "UIDlgCampaignContinueBattleResult", _UIType.UIDlgCampaignContinueBattleResult),
			new UIModuleData(typeof(UIPnlStartServerReward), "UIPnlStartServerReward", _UIType.UIPnlStartServerReward,new Type[]{typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),

			new UIModuleData(typeof(UIDlgGiveMeFive), "UIDlgGiveMeFive"),
			new UIModuleData(typeof(UIPnlBattleScene), "UIPnlBattleScene"),
// Domineer
			new UIModuleData(typeof(UIPnlAvatarDomineerTab), "UIPnlAvatarDomineerTab", _UIType.UIPnlAvatarDomineerTab, new Type[]{typeof(UIPnlAvatarPowerUpTab),typeof(UIPnlAvatarDomineerTab), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlPackageAvatarTab), typeof(UIPnlAvatar),typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIPnlDomineerDescTab), "UIPnlDomineerDescTab",_UIType.UIPnlDomineerDescTab, new Type[]{typeof(UIPnlDomineerDescTab), typeof(UIPnlPlayerInfos)}),
			//角色筛选
			new UIModuleData(typeof(UIDlgPackageAvatarFilter),"UIDlgPackageAvatarFilter"),
			new UIModuleData(typeof(UIDlgAttributeComparison),"UIDlgAttributeComparison"),
			new UIModuleData(typeof(UIPnlAvatarAttributeUpdateDetail),"UIPnlAvatarAttributeUpdateDetail",_UIType.UIPnlAvatarAttributeUpdateDetail),

			new UIModuleData(typeof(UIDlgPartnerComparison),"UIDlgPartnerComparison"),

			// Diner
			new UIModuleData(typeof(UIPnlAvatarDiner),"UIPnlAvatarDiner",_UIType.UIPnlAvatarDiner,new Type[]{typeof(UIPnlAvatarDiner), typeof(UIPnlPlayerInfos),typeof(UIPnlMainMenuBot),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlAvatarDinerRefresh),"UIPnlAvatarDinerRefresh",_UIType.UIPnlAvatarDinerRefresh,new Type[]{typeof(UIPnlAvatarDinerRefresh), typeof(UIPnlPlayerInfos),typeof(UIPnlMainMenuBot),typeof(UIPnlMessageFlow)},true,new Type[]{typeof(UIPnlAvatarDiner)}),
			new UIModuleData(typeof(UIPnlCardImage),"UIPnlCardImage",_UIType.UIPnlCardImage,new Type[]{typeof(UIPnlCardImage)}),

			new UIModuleData(typeof(UIDlgArenaRules),"UIDlgArenaRules"),

			//图鉴
			new UIModuleData(typeof(UIPnlHandBook), "UIPnlHandBook", _UIType.UIPnlHandBook,new Type[]{typeof(UIPnlHandBook), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)}, true,new Type[]{typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),

			new UIModuleData(typeof(UIDlgTutorialReward),"UIDlgTutorialReward",_UIType.UIDlgTutorialReward),
			new UIModuleData(typeof(UITipRotateHelp),"UITipRotateHelp",_UIType.UITipRotateHelp),

			//千机楼
			new UIModuleData(typeof(UIPnlTowerScene),"UIPnlTowerScene",_UIType.UIPnlTowerScene,new Type[]{typeof(UIPnlMessageFlow), typeof(UIPnlTowerPlayerInfo)},true),
			new UIModuleData(typeof(UIPnlTowerPlayerInfo),"UIPnlTowerPlayerInfo"),
			new UIModuleData(typeof(UIDlgTowerExplain),"UIDlgTowerExplain"),
			new UIModuleData(typeof(UIDlgTowerBattleData), "UIDlgTowerBattleData"),
			new UIModuleData(typeof(UIPnlTowerBattleResult),"UIPnlTowerBattleResult", _UIType.UIPnlTowerBattleResult),
			new UIModuleData(typeof(UIPnlTowerSweepBattle),"UIPnlTowerSweepBattle", _UIType.UIPnlTowerSweepBattle,new Type[]{typeof(UIPnlTowerSweepBattle)},true),
			new UIModuleData(typeof(UIPnlTowerPoint), "UIPnlTowerPoint"),
			new UIModuleData(typeof(UIPnlTowerThisWeekRank), "UIPnlTowerThisWeekRank", _UIType.UIPnlTowerThisWeekRank, new Type[]{typeof(UIPnlTowerPoint), typeof(UIPnlTowerThisWeekRank), typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlTowerLastWeekRank), "UIPnlTowerLastWeekRank", _UIType.UIPnlTowerLastWeekRank, new Type[]{typeof(UIPnlTowerPoint), typeof(UIPnlTowerLastWeekRank), typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlTowerWeekReward), "UIPnlTowerWeekReward", _UIType.UIPnlTowerWeekReward, new Type[]{typeof(UIPnlTowerPoint), typeof(UIPnlTowerWeekReward), typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlTowerShop), "UIPnlTowerShop", _UIType.UIPnlTowerShop),
			new UIModuleData(typeof(UIPnlTowerNormalShop), "UIPnlTowerNormalShop", _UIType.UIPnlTowerNormalShop,new Type[]{typeof(UIPnlTowerShop), typeof(UIPnlTowerNormalShop), typeof(UIPnlMessageFlow), typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIPnlTowerActivityShop), "UIPnlTowerActivityShop", _UIType.UIPnlTowerActivityShop,new Type[]{typeof(UIPnlTowerShop), typeof(UIPnlTowerActivityShop), typeof(UIPnlMessageFlow), typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIPnlTowerNpcLineUp),"UIPnlTowerNpcLineUp"),

			//兑换成功显示弹框
			new UIModuleData(typeof(UIDlgConver),"UIDlgConver"),
			new UIModuleData(typeof(UIDlgLevelRewards),"UIDlgLevelRewards"),

			// 剧情动画
			new UIModuleData(typeof(UIPnlGameMovie),"UIPnlGameMovie",_UIType.UIPnlGameMovie),

			//烽火狼烟
			new UIModuleData(typeof(UIPnlWolfExpedition),"UIPnlWolfExpedition",_UIType.UIPnlWolfExpedition,new Type[]{typeof(UIPnlWolfExpedition),typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)},true,new Type[]{typeof(UIPnlMainScene)}),
			new UIModuleData(typeof(UIDlgWolfExpedition),"UIDlgWolfExpedition"),
			new UIModuleData(typeof(UIDlgWolfGain),"UIDlgWolfGain"),
			new UIModuleData(typeof(UIPnlWolfGuide),"UIPnlWolfGuide"),
			new UIModuleData(typeof(UIPnlWolfGuideDetail),"UIPnlWolfGuideDetail"),
			new UIModuleData(typeof(UIPnlWolfMyBattle),"UIPnlWolfMyBattle"),
			new UIModuleData(typeof(UIDlgWolfEnemyExpedition),"UIDlgWolfEnemyExpedition"),
			new UIModuleData(typeof(UIDlgWolfFailyDeficiency),"UIDlgWolfFailyDeficiency"),
			new UIModuleData(typeof(UIDlgWolfStart),"UIDlgWolfStart"),
			new UIModuleData(typeof(UIDlgWolfParticular),"UIDlgWolfParticular"),

			new UIModuleData(typeof(UIPnlWolfInfo), "UIPnlWolfInfo", _UIType.UIPnlWolfInfo, new Type[]{typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlWolfShop), "UIPnlWolfShop", _UIType.UIPnlWolfShop),
			new UIModuleData(typeof(UIPnlWolfNormalShop), "UIPnlWolfNormalShop", _UIType.UIPnlWolfNormalShop, new Type[]{typeof(UIPnlWolfShop), typeof(UIPnlWolfNormalShop), typeof(UIPnlMessageFlow), typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIPnlWolfActivityShop),"UIPnlWolfActivityShop", _UIType.UIPnlWolfActivityShop, new Type	[]{typeof(UIPnlWolfShop),typeof(UIPnlWolfActivityShop), typeof(UIPnlMessageFlow),typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIPnlWolfBattleResult), "UIPnlWolfBattleResult", _UIType.UIPnlWolfBattleResult),

			new UIModuleData(typeof(UIDlgWolfCheckPoint), "UIDlgWolfCheckPoint"),

			new UIModuleData(typeof(UIDlgRecoverStamina_UseItem),"UIDlgRecoverStamina_UseItem",_UIType.UIDlgRecoverStamina_UseItem),

			// 小助手
			new UIModuleData(typeof(UIPnlAssistant),"UIPnlAssistant",_UIType.UIPnlAssistant,new Type[]{typeof(UIPnlAssistant), typeof(UIPnlMessageFlow), typeof(UIPnlMainMenuBot)},true,new Type[]{typeof(UIPnlMainScene)}),
			new UIModuleData(typeof(UITipAssistant),"UITipAssistant",_UIType.UITipAssistant),

			//小助手分页
			new UIModuleData(typeof(UIPnlFreshmanAdviseDetail), "UIPnlFreshmanAdviseDetail",_UIType.UIPnlFreshmanAdviseDetail,new Type[]{typeof(UIPnlFreshmanAdvise)},true),
			new UIModuleData(typeof(UIPnlFreshmanAdvise), "UIPnlFreshmanAdvise",_UIType.UIPnlFreshmanAdvise,new Type[]{typeof(UIPnlAssistant)},true),
			new UIModuleData(typeof(UIPnlAssistantTask),"UIPnlAssistantTask",_UIType.UIPnlAssistantTask,new Type[]{typeof(UIPnlAssistant)},true),
			new UIModuleData(typeof(UIPnlAssistantDailyTask),"UIPnlAssistantDailyTask",_UIType.UIPnlAssistantDailyTask,new Type[]{typeof(UIPnlAssistant)},true),
			new UIModuleData(typeof(UIPnlAssistantFixedTask),"UIPnlAssistantFixedTask",_UIType.UIPnlAssistantFixedTask,new Type[]{typeof(UIPnlAssistant)},true),
			new UIModuleData(typeof(UIDlgMonthCardView),"UIDlgMonthCardView"),

			new UIModuleData(typeof(UIPnlMonthCardDetail),"UIPnlMonthCardDetail",_UIType.UIPnlMonthCardDetail,new Type[]{typeof(UIPnlMonthCardDetail), typeof(UIPnlPlayerInfos)}),

			//好友
			new UIModuleData(typeof(UIPnlFriendTab), "UIPnlFriendTab", _UIType.UIPnlFriendTab, new Type[]{typeof(UIPnlFriendTab), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIDlgFriendMessage), "UIDlgFriendMessage"),
			new UIModuleData(typeof(UIPnlCombatFriendBattleResult), "UIPnlCombatFriendBattleResult", _UIType.UIPnlCombatFriendBattleResult),
			//运营活动
			new UIModuleData(typeof(UIPnlRunActivityTab), "UIPnlRunActivityTab", _UIType.UIPnlRunActivityTab, new Type[]{typeof(UIPnlMainMenuBot), typeof(UIPnlRunActivityTab), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)},true,new Type[]{typeof(UIPnlMainScene)}),
			//运营活动子标签
			new UIModuleData(typeof(UIPnlRunAccumulativeTab), "UIPnlRunAccumulativeTab", _UIType.UIPnlRunAccumulativeTab, new Type[]{typeof(UIPnlRunActivityTab),typeof(UIPnlRunAccumulativeTab)}, true),
			new UIModuleData(typeof(UIPnlEastSeaFindFairyMain),"UIPnlEastSeaFindFairyMain",_UIType.UIPnlEastSeaFindFairyMain,new Type[]{typeof(UIPnlRunActivityTab), typeof(UIPnlEastSeaFindFairyMain), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)}, true),
			new UIModuleData(typeof(UIPnlEastSeaCloseActivity),"UIPnlEastSeaCloseActivity",_UIType.UIPnlEastSeaCloseActivity,new Type[]{typeof(UIPnlRunActivityTab)}, true),
			
			new UIModuleData(typeof(UIDlgRunActivityRewards),"UIDlgRunActivityRewards"),
			new UIModuleData(typeof(UIDlgEastRankingReward),"UIDlgEastRankingReward"),
			new UIModuleData(typeof(UIPnlEastSeaExchangeTab),"UIPnlEastSeaExchangeTab",_UIType.UIPnlEastSeaExchangeTab,new Type[]{typeof(UIPnlEastSeaExchangeTab),typeof(UIPnlPlayerInfos),typeof(UIPnlMainMenuBot),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlEastSeaElementItem),"UIPnlEastSeaElementItem",_UIType.UIPnlEastSeaElementItem,new Type[]{typeof(UIPnlEastSeaExchangeTab)},true),
			new UIModuleData(typeof(UIPnlEastElementAllServerReward),"UIPnlEastElementAllServerReward",_UIType.UIPnlEastElementAllServerReward,new Type[]{typeof(UIPnlEastSeaExchangeTab)},true),
			new UIModuleData(typeof(UIPnlEastElementRankingList),"UIPnlEastElementRankingList",_UIType.UIPnlEastElementRankingList,new Type[]{typeof(UIPnlEastSeaExchangeTab)},true),

			new UIModuleData(typeof(UIDlgEastSeaExplain),"UIDlgEastSeaExplain"),
			new UIModuleData(typeof(UIDlgEastSeaMessages),"UIDlgEastSeaMessages"),

			//奇遇
			new UIModuleData(typeof(UIPnlAdventureMain),"UIPnlAdventureMain" , _UIType.UIPnlAdventureMain, new Type[]{typeof(UIPnlMainMenuBot), typeof(UIPnlAdventureMain), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureScene),"UIPnlAdventureScene" , _UIType.UIPnlAdventureScene, new Type[]{ typeof(UIPnlAdventureScene), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureDelayReward), "UIPnlAdventureDelayReward", _UIType.UIPnlAdventureDelayReward),
			new UIModuleData(typeof(UIDlgAdventureExplain), "UIDlgAdventureExplain"),
			new UIModuleData(typeof(UIDlgAdventureMessage), "UIDlgAdventureMessage"),

			new UIModuleData(typeof(UIPnlAdventureMessage),"UIPnlAdventureMessage",_UIType.UIPnlAdventureMessage, new Type[]{ typeof(UIPnlAdventureMessage), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureBuyReward),"UIPnlAdventureBuyReward",_UIType.UIPnlAdventureBuyReward, new Type[]{ typeof(UIPnlAdventureBuyReward), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureCombat),"UIPnlAdventureCombat",_UIType.UIPnlAdventureCombat, new Type[]{ typeof(UIPnlAdventureCombat), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureGetReward),"UIPnlAdventureGetReward",_UIType.UIPnlAdventureGetReward , new Type[]{ typeof(UIPnlAdventureGetReward), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureCombatResult),"UIPnlAdventureCombatResult",_UIType.UIPnlAdventureCombatResult, new Type[]{ typeof(UIPnlAdventureCombatResult), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlAdventureQuestionAnswer),"UIPnlAdventureQuestionAnswer",_UIType.UIPnlAdventureQuestionAnswer, new Type[]{ typeof(UIPnlAdventureQuestionAnswer), typeof(UIPnlMessageFlow)}),

			//好友战斗
			new UIModuleData(typeof(UIPnlFriendStart),"UIPnlFriendStart",_UIType.UIPnlFriendStart,new Type[]{typeof(UIPnlFriendStart),typeof(UIPnlMainMenuBot),typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)},true,new Type[]{typeof(UIPnlMainScene)}),

			new UIModuleData(typeof(UIPnlFriendSelectFriends),"UIPnlFriendSelectFriends",_UIType.UIPnlFriendSelectFriends,new Type[]{typeof(UIPnlFriendSelectFriends),typeof(UIPnlMainMenuBot),typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)},false),
			new UIModuleData(typeof(UIDlgFriendStartOne),"UIDlgFriendStartOne"),
			new UIModuleData(typeof(UIDlgFriendAvatarView),"UIDlgFriendAvatarView"),
			new UIModuleData(typeof(UIPnlFriendInfoTab),"UIPnlFriendInfoTab",_UIType.UIPnlFriendInfoTab,new Type[]{typeof(UIPnlFriendInfoTab),typeof(UIPnlMainMenuBot),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true,new Type[]{typeof(UIPnlMainScene)}),
			new UIModuleData(typeof(UIPnlFriendCombatTab),"UIPnlFriendCombatTab",_UIType.UIPnlFriendCombatTab,new Type[]{typeof(UIPnlFriendCombatTab)},true,new Type[]{typeof(UIPnlFriendInfoTab),typeof(UIDlgFriendCampaginChackPoint)}),
			new UIModuleData(typeof(UIPnlFriendCampaginThisWeekRank),"UIPnlFriendCampaginThisWeekRank",_UIType.UIPnlFriendCampaginThisWeekRank,new Type[]{typeof(UIPnlFriendCampaginThisWeekRank)},true,new Type[]{typeof(UIPnlFriendInfoTab)}),
			new UIModuleData(typeof(UIPnlFriendCampaginLastWeekRank),"UIPnlFriendCampaginLastWeekRank",_UIType.UIPnlFriendCampaginLastWeekRank,new Type[]{typeof(UIPnlFriendCampaginLastWeekRank)},true,new Type[]{typeof(UIPnlFriendInfoTab)}),
			new UIModuleData(typeof(UIPnlFriendCampaginWeekReward),"UIPnlFriendCampaginWeekReward",_UIType.UIPnlFriendCampaginWeekReward,new Type[]{typeof(UIPnlFriendCampaginWeekReward)},true,new Type[]{typeof(UIPnlFriendInfoTab)}),
			new UIModuleData(typeof(UIDlgFriendCampaginShips),"UIDlgFriendCampaginShips"),

			new UIModuleData(typeof(UIPnlFriendGuide),"UIPnlFriendGuide",_UIType.UIPnlFriendGuide,new Type[]{typeof(UIPnlFriendGuide),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlFriendGuideDetail_1),"UIPnlFriendGuideDetail_1",_UIType.UIPnlFriendGuideDetail_1,new Type[]{typeof(UIPnlFriendGuideDetail_1),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlFriendBattle),"UIPnlFriendBattle",_UIType.UIPnlFriendBattle,new Type[]{typeof(UIPnlFriendBattle),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},false),
			new UIModuleData(typeof(UIDlgFriendCampaignReset),"UIDlgFriendCampaignReset"),
			new UIModuleData(typeof(UIDlgFriendCampaginChackPoint),"UIDlgFriendCampaginChackPoint"),
			new UIModuleData(typeof(UIPnlFriendCampaginBattleResult),"UIPnlFriendCampaginBattleResult",_UIType.UIPnlFriendCampaginBattleResult),

			//内丹
			new UIModuleData(typeof(UIPnlDanMain),"UIPnlDanMain",_UIType.UIPnlDanMain,new Type[]{typeof(UIPnlDanMain),typeof(UIPnlMainMenuBot)}),
			new UIModuleData(typeof(UIPnlDanMenuBot),"UIPnlDanMenuBot",_UIType.UIPnlDanMenuBot),
			new UIModuleData(typeof(UIPnlDanFurnace),"UIPnlDanFurnace",_UIType.UIPnlDanFurnace,new Type[]{typeof(UIPnlDanMenuBot),typeof(UIPnlDanFurnace),typeof(UIPnlMessageFlow),typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIPnlDanCultureTab),"UIPnlDanCultureTab",_UIType.UIPnlDanCultureTab,new Type[]{typeof(UIPnlDanCultureTab),typeof(UIPnlMessageFlow)},false,new Type[]{typeof(UIPnlDanInfo)}),
			new UIModuleData(typeof(UIPnlDanCulture),"UIPnlDanCulture",_UIType.UIPnlDanCulture,new Type[]{typeof(UIPnlDanCultureTab),typeof(UIPnlDanCulture),typeof(UIPnlMessageFlow)},false,new Type[]{typeof(UIPnlDanInfo)}),
			new UIModuleData(typeof(UIPnlDanAttic),"UIPnlDanAttic", _UIType.UIPnlDanAttic, new Type[]{typeof(UIPnlDanMenuBot),typeof(UIPnlDanAttic),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlDanFurnaceActivity),"UIPnlDanFurnaceActivity",_UIType.UIPnlDanFurnaceActivity,new Type[]{typeof(UIPnlDanMenuBot),typeof(UIPnlDanFurnaceActivity),typeof(UIPnlMessageFlow),typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIDlgDanBoxRewardView),"UIDlgDanBoxRewardView"),
			new UIModuleData(typeof(UIPnlDanAlchemy), "UIPnlDanAlchemy", _UIType.UIPnlDanAlchemy, new Type[]{typeof(UIPnlDanAlchemy)},true),
			new UIModuleData(typeof(UIPnlDanInfo),"UIPnlDanInfo",_UIType.UIPnlDanInfo),
			new UIModuleData(typeof(UIPnlSelectDanList),"UIPnlSelectDanList"),
			new UIModuleData(typeof(UIDlgDanActivityInfo), "UIDlgDanActivityInfo"),
			new UIModuleData(typeof(UIDlgDanIntroduce), "UIDlgDanIntroduce"),
			new UIModuleData(typeof(UIDlgDanMaxCount), "UIDlgDanMaxCount"),
			new UIModuleData(typeof(UIDlgDanXiang), "UIDlgDanXiang"),
			new UIModuleData(typeof(UIPnlDanOneKeyDecompose),"UIPnlDanOneKeyDecompose", _UIType.UIPnlDanOneKeyDecompose,new Type[]{typeof(UIPnlDanOneKeyDecompose),typeof(UIPnlMessageFlow)},false),
			new UIModuleData(typeof(UIPnlDanDecompose),"UIPnlDanDecompose", _UIType.UIPnlDanDecompose,new Type[]{typeof(UIPnlDanMenuBot),typeof(UIPnlDanDecompose),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIDlgDanMaterialGet), "UIDlgDanMaterialGet"),
			new UIModuleData(typeof(UIPnlDanMaterial), "UIPnlDanMaterial", _UIType.UIPnlDanMaterial,new Type[]{typeof(UIPnlDanMenuBot),typeof(UIPnlDanMaterial),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIDlgDanDecomposeSure), "UIDlgDanDecomposeSure"),
			new UIModuleData(typeof(UIPnlDanDecomposeActivity), "UIPnlDanDecomposeActivity",_UIType.UIPnlDanDecomposeActivity,new Type[]{typeof(UIPnlDanMenuBot),typeof(UIPnlDanDecomposeActivity),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIDlgDanShowCountView), "UIDlgDanShowCountView"),			

			//幻化
			new UIModuleData(typeof(UIPnlIllusion),"UIPnlIllusion",_UIType.UIPnlIllusion,new Type[]{typeof(UIPnlIllusion),typeof(UIPnlMainMenuBot),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlAvatarIllusion),"UIPnlAvatarIllusion",_UIType.UIPnlAvatarIllusion,new Type[]{typeof(UIPnlAvatarIllusion),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlIllusionGuide),"UIPnlIllusionGuide",_UIType.UIPnlIllusionGuide),
			new UIModuleData(typeof(UIPnlIllusionGuideDetail),"UIPnlIllusionGuideDetail",_UIType.UIPnlIllusionGuideDetail),

			//好友邀请码奖励
			new UIModuleData(typeof(UIPnlActivityInvite),"UIPnlActivityInvite",_UIType.UIPnlActivityInvite,new Type[]{typeof(UIPnlActivityInvite),typeof(UIPnlActivityTab)},true),
			new UIModuleData(typeof(UIPnlActivityFaceBook),"UIPnlActivityFaceBook",_UIType.UIPnlActivityFaceBook,new Type[]{typeof(UIPnlActivityFaceBook),typeof(UIPnlActivityTab)},true),
			new UIModuleData(typeof(UIDlgGiftGoods),"UIDlgGiftGoods"),
			new UIModuleData(typeof(UIDlgShare),"UIDlgShare"),

			//711活动
			new UIModuleData(typeof(UIDlgConvertMain),"UIDlgConvertMain"),
			new UIModuleData(typeof(UIDlgConvertTips),"UIDlgConvertTips"),

			//门派
			new UIModuleData(typeof(UIPnlGuildApplyList),"UIPnlGuildApplyList",_UIType.UIPnlGuildApplyList,new Type[]{typeof(UIPnlMainMenuBot),typeof(UIPnlGuildApplyList),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlGuildTab),"UIPnlGuildTab",_UIType.UIPnlGuildTab,new Type[]{typeof(UIPnlGuildMenuBot),typeof(UIPnlGuildTab),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildIntroTab),"UIPnlGuildIntroTab",_UIType.UIPnlGuildIntroTab,new Type[]{typeof(UIPnlGuildMenuBot),typeof(UIPnlGuildIntroTab),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlGuildIntroInfo),"UIPnlGuildIntroInfo",_UIType.UIPnlGuildIntroInfo,new Type[]{typeof(UIPnlGuildIntroTab), typeof(UIPnlGuildIntroInfo), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildIntroMember),"UIPnlGuildIntroMember",_UIType.UIPnlGuildIntroMember,new Type[]{typeof(UIPnlGuildIntroTab), typeof(UIPnlGuildIntroMember), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildIntroReview),"UIPnlGuildIntroReview",_UIType.UIPnlGuildIntroReview,new Type[]{typeof(UIPnlGuildIntroTab), typeof(UIPnlGuildIntroReview), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildMessageTab),"UIPnlGuildMessageTab",_UIType.UIPnlGuildMessageTab,new Type[]{typeof(UIPnlGuildMenuBot),typeof(UIPnlGuildMessageTab)}),
			new UIModuleData(typeof(UIPnlGuildMessage),"UIPnlGuildMessage",_UIType.UIPnlGuildMessage,new Type[]{typeof(UIPnlGuildMessageTab), typeof(UIPnlGuildMessage),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildChat),"UIPnlGuildChat",_UIType.UIPnlGuildChat,new Type[]{typeof(UIPnlGuildMessageTab), typeof(UIPnlGuildChat),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildGuide),"UIPnlGuildGuide",_UIType.UIPnlGuildGuide,new Type[]{typeof(UIPnlGuildMenuBot),typeof(UIPnlGuildGuide),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildGuideDetail),"UIPnlGuildGuideDetail",_UIType.UIPnlGuildGuideDetail),

			new UIModuleData(typeof(UIPnlChatTab), "UIPnlChatTab", _UIType.UIPnlChatTab),


			new UIModuleData(typeof(UIPnlGuildRankTab),"UIPnlGuildRankTab",_UIType.UIPnlGuildRankTab,new Type[]{typeof(UIPnlGuildRankTab),typeof(UIPnlGuildMenuBot),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildRankList),"UIPnlGuildRankList",_UIType.UIPnlGuildRankList,new Type[]{typeof(UIPnlGuildRankTab),typeof(UIPnlGuildRankList),typeof(UIPnlPlayerInfos)},true,new Type[]{typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlGuildApplyInfo),"UIPnlGuildApplyInfo",_UIType.UIPnlGuildApplyInfo,new Type[]{typeof(UIPnlGuildApplyInfo),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlGuildScheduleRankList),"UIPnlGuildScheduleRankList",_UIType.UIPnlGuildScheduleRankList,new Type[]{typeof(UIPnlGuildRankTab),typeof(UIPnlGuildScheduleRankList),typeof(UIPnlPlayerInfos)},true,new Type[]{typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlGuildRacingRankList),"UIPnlGuildRacingRankList",_UIType.UIPnlGuildRacingRankList,new Type[]{typeof(UIPnlGuildRankTab),typeof(UIPnlGuildRacingRankList),typeof(UIPnlPlayerInfos)},true,new Type[]{typeof(UIPnlPlayerInfos)}),

			new UIModuleData(typeof(UIPnlGuildShopTab),"UIPnlGuildShopTab",_UIType.UIPnlGuildShopTab,new Type[]{typeof(UIPnlGuildShopTab),typeof(UIPnlGuildMenuBot),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlGuildPublicShop),"UIPnlGuildPublicShop",_UIType.UIPnlGuildPublicShop,new Type[]{typeof(UIPnlGuildShopTab), typeof(UIPnlGuildPublicShop), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildPrivateShop),"UIPnlGuildPrivateShop",_UIType.UIPnlGuildPrivateShop,new Type[]{typeof(UIPnlGuildShopTab), typeof(UIPnlGuildPrivateShop), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildShopActivity),"UIPnlGuildShopActivity",_UIType.UIPnlGuildShopActivity,new Type[]{typeof(UIPnlGuildShopTab), typeof(UIPnlGuildShopActivity), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildConstructTab),"UIPnlGuildConstructTab",_UIType.UIPnlGuildConstructTab,new Type[]{typeof(UIPnlGuildMenuBot),typeof(UIPnlGuildConstructTab),typeof(UIPnlPlayerInfos)}),
			new UIModuleData(typeof(UIPnlGuildConstruct),"UIPnlGuildConstruct",_UIType.UIPnlGuildConstruct,new Type[]{typeof(UIPnlGuildConstructTab), typeof(UIPnlGuildConstruct), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),
			new UIModuleData(typeof(UIPnlGuildTask),"UIPnlGuildTask",_UIType.UIPnlGuildTask,new Type[]{typeof(UIPnlGuildMenuBot), typeof(UIPnlGuildTask), typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)},true),

			new UIModuleData(typeof(UIPnlGuildPointPerson), "UIPnlGuildPointPerson",_UIType.UIPnlGuildPointPerson),
			new UIModuleData(typeof(UIEffectSchoolOpenBox), "UIEffectSchoolOpenBox"),
			new UIModuleData(typeof(UIPnlGuildPointGiveAway), "UIPnlGuildPointGiveAway",_UIType.UIPnlGuildPointGiveAway,new Type[]{typeof(UIPnlGuildPointGiveAway),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlGuildPointRankTab),"UIPnlGuildPointRankTab",_UIType.UIPnlGuildPointRankTab,new Type[]{typeof(UIPnlGuildPointRankTab),typeof(UIPnlPlayerInfos),typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlGuildPointBossRank),"UIPnlGuildPointBossRank",_UIType.UIPnlGuildPointBossRank),
			new UIModuleData(typeof(UIPnlGuildPointDamageRank),"UIPnlGuildPointDamageRank",_UIType.UIPnlGuildPointDamageRank),
			new UIModuleData(typeof(UIPnlGuildPointExplorationRank),"UIPnlGuildPointExplorationRank",_UIType.UIPnlGuildPointExplorationRank),
			new UIModuleData(typeof(UIDlgGuildPlayerList),"UIDlgGuildPlayerList"),

			new UIModuleData(typeof(UIDlgGuildMemberOperation),"UIDlgGuildMemberOperation"),
			new UIModuleData(typeof(UIDlgGuildNotifyModify),"UIDlgGuildNotifyModify"),
			
			new UIModuleData(typeof(UIDlgGuildFound),"UIDlgGuildFound"),
			new UIModuleData(typeof(UIDlgGuildAssignment),"UIDlgGuildAssignment"),
			new UIModuleData(typeof(UIPnlGuildMenuBot),"UIPnlGuildMenuBot"),
			new UIModuleData(typeof(UIDlgGuildChangeRole),"UIDlgGuildChangeRole"),
			new UIModuleData(typeof(UIDlgGuildPrivateBuyTips),"UIDlgGuildPrivateBuyTips"),

			//门派关卡			
			new UIModuleData(typeof(UIPnlGuildPointMain),"UIPnlGuildPointMain", _UIType.UIPnlGuildPointMain, new Type[]{typeof(UIPnlGuildPointMain), typeof(UIPnlMessageFlow), typeof(UIPnlPlayerInfos)},true),
			new UIModuleData(typeof(UIPnlGuildPointTalentTab),"UIPnlGuildPointTalentTab"),
			new UIModuleData(typeof(UIPnlGuildPointBossView),"UIPnlGuildPointBossView"),	
			new UIModuleData(typeof(UIPnlGuildPointMonsterBattleResult),"UIPnlGuildPointMonsterBattleResult", _UIType.UIPnlGuildPointMonsterBattleResult),	
			new UIModuleData(typeof(UIPnlGuildPointBossBattleResult),"UIPnlGuildPointBossBattleResult", _UIType.UIPnlGuildPointBossBattleResult),						
			new UIModuleData(typeof(UIPnlGuildPointGuide),"UIPnlGuildPointGuide"),
			new UIModuleData(typeof(UIPnlGuildPointNotEnough),"UIPnlGuildPointNotEnough"),
			new UIModuleData(typeof(UIPnlGuildPointMonsterView),"UIPnlGuildPointMonsterView"),
		
			//修改玩家名称
			new UIModuleData(typeof(UIDlgSetName),"UIDlgSetName"),
			new UIModuleData(typeof(UIDlgSetGuidName),"UIDlgSetGuidName"),

			//机关兽工坊
								
			new UIModuleData(typeof(UIPnlOrganTab),"UIPnlOrganTab", _UIType.UIPnlOrganTab, new Type[]{typeof(UIPnlOrganTab), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow)}),
			new UIModuleData(typeof(UIPnlOrganChipsTab),"UIPnlOrganChipsTab", _UIType.UIPnlOrganChipsTab, new Type[]{typeof(UIPnlOrganTab), typeof(UIPnlOrganChipsTab), typeof(UIPnlPlayerInfos)}, true),
			new UIModuleData(typeof(UIPnlOrgansBeastTab),"UIPnlOrgansBeastTab", _UIType.UIPnlOrgansBeastTab, new Type[]{typeof(UIPnlOrganTab), typeof(UIPnlOrgansBeastTab), typeof(UIPnlPlayerInfos)}, true),			
			new UIModuleData(typeof(UIPnlOrgansShopTab),"UIPnlOrgansShopTab", _UIType.UIPnlOrgansShopTab, new Type[]{typeof(UIPnlOrganTab), typeof(UIPnlOrgansShopTab), typeof(UIPnlPlayerInfos)}, true),			
			new UIModuleData(typeof(UIPnlOrganChipInfo), "UIPnlOrganChipInfo", _UIType.UIPnlOrganChipInfo),
			new UIModuleData(typeof(UIPnlOrganInfo), "UIPnlOrganInfo", _UIType.UIPnlOrganInfo),		
			new UIModuleData(typeof(UIPnlSelectOrganList), "UIPnlSelectOrganList",_UIType.UIPnlSelectOrganList,new Type[]{typeof(UIPnlSelectOrganList),typeof(UIPnlPlayerInfos)}),				
			new UIModuleData(typeof(UIPnlOrganGrowPage), "UIPnlOrganGrowPage", _UIType.UIPnlOrganGrowPage, new Type[]{typeof(UIPnlOrganGrowPage), typeof(UIPnlMainMenuBot), typeof(UIPnlMessageFlow), typeof(UIPnlPlayerInfos)}, true, new Type[]{typeof(UIPnlAvatar),typeof(UIPnlOrgansBeastTab),typeof(UIPnlOrgansShopTab), typeof(UIPnlCampaignCityPlayerInfo),typeof(UIPnlCampaignSceneMid),typeof(UIPnlCampaignSceneBottom),typeof(UIPnlCampaignActivityScene),typeof(UIPnlCampaignScene)}),
			new UIModuleData(typeof(UIDlgOrganChipSplit), "UIDlgOrganChipSplit"),	
			new UIModuleData(typeof(UIDlgOrganAttributeExplain), "UIDlgOrganAttributeExplain"),				
			new UIModuleData(typeof(UIDlgOrganGet), "UIDlgOrganGet"),		
			new UIModuleData(typeof(UIPnlOrganSelectInfo), "UIPnlOrganSelectInfo", _UIType.UIPnlOrganSelectInfo, new Type[]{typeof(UIPnlOrganSelectInfo), typeof(UIPnlPlayerInfos), typeof(UIPnlMessageFlow)} ),						
			new UIModuleData(typeof(UIDlgOrganGetWay), "UIDlgOrganGetWay"),			
			new UIModuleData(typeof(UIDlgOrganUpShow), "UIDlgOrganUpShow"),						
		};

		return datas;
	}

	// Text color
	public static readonly Color textPlaceHolderColor = new Color(128 / 255f, 128 / 255f, 128 / 255f, 0.5f);
	public static readonly Color addColor = new Color(129 / 255f, 255 / 255f, 17 / 255f, 1f);
	public static readonly Color subtractColor = new Color(255 / 255f, 111 / 255f, 82 / 255f, 1f);
	public static readonly Color colorFollowerGreen = new Color(101 / 255f, 249 / 255f, 37 / 255f, 255 / 255f);
	public static readonly Color colorDamageSpeedGreen = new Color(101 / 255f, 249 / 255f, 37 / 255f, 255 / 255f);
	public static readonly Color colorWpnNameOrange = new Color(253 / 255f, 161 / 255f, 76 / 255f, 255 / 255f);
	public static readonly Color colorGoldYellow = new Color(255 / 255f, 250 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color colorFollowerRed = new Color(255 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
	public static readonly Color colorWhite = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
	public static readonly Color colorExpBlue = new Color(82 / 255f, 247 / 255f, 255 / 255f, 255 / 255f);
	public static readonly Color colorCrtAvtarTip = new Color(253 / 255f, 161 / 255f, 76 / 255f, 255 / 255f);
	public static readonly Color colorTipsBrown = new Color(117 / 255f, 71 / 255f, 40 / 255f, 255 / 255f);
	public static readonly Color colorTipsRed = new Color(255 / 255f, 25 / 255f, 25 / 255f, 255 / 255f);
	public static readonly Color colorMaxSkillLevelText = new Color(255 / 255f, 156 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color colorAddFriendRed = new Color(255 / 255f, 138 / 255f, 0 / 255f, 255 / 255f);

	public static readonly Color txColorGray = new Color(128 / 255f, 128 / 255f, 128 / 255f, 1f);
	public static readonly Color txColorGreen = new Color(114f / 255f, 232f / 255f, 105f / 255f, 255 / 255f);
	public static readonly Color txColorWhite = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
	public static readonly Color txColorPurple = new Color(194 / 255f, 122 / 255f, 255 / 255f, 255 / 255f);
	public static readonly Color txColorRed = new Color(255f / 255f, 46f / 255f, 46f / 255f, 255 / 255f);
	public static readonly Color txColorBrown = new Color(207f / 255f, 169f / 255f, 114f / 255f, 255f / 255f);
	public static readonly Color txColorOrange = new Color(207 / 255f, 169 / 255f, 116 / 255f, 255 / 255f);
	public static readonly Color txColorBlue = new Color(82 / 255f, 247 / 255f, 255 / 255f, 255 / 255f);
	public static readonly Color txColorYellow = new Color(255 / 255f, 255 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color txColorGold = new Color(239 / 255f, 156 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color txcolorYellowAndWhite = new Color(255 / 255f, 251 / 255f, 244 / 255f, 255 / 255f);

	public static readonly Color txColorAttGreen = new Color(0 / 255f, 155 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color txColorAttRed = new Color(255 / 255f, 18 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color txColorBlack = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255 / 255f);

	public static readonly Color txColorHolder = new Color(0 / 255f, 155 / 255f, 0 / 255f, 255 / 255f);
	public static readonly Color txColorSignDayColor = new Color(255 / 255.0f, 255 / 255.0f, 255 / 255.0f, 90 / 255f);

	public static readonly Color txColorBrownColor = new Color(207 / 255f, 169 / 255f, 114 / 225f, 255 / 255f);
	public static readonly Color txColorDeepBrownColor = new Color(49f / 255.0f, 30f / 255.0f, 25f / 255.0f, 255 / 255f);
	public static readonly Color txColorLightYellowColor = new Color(248f / 255.0f, 244f / 255.0f, 0f / 255.0f, 255 / 255f);

	public static readonly Color txColorLightYellowColor2 = new Color(255f / 255.0f, 209f / 255.0f, 0f / 255.0f, 255 / 255f);
	public static readonly Color txColorLightOrangeColor = new Color(250 / 255.0f, 205f / 255.0f, 137f / 255.0f, 255 / 255f);
	public static readonly Color txColorYellow2 = new Color(247 / 255.0f, 222f / 255.0f, 137f / 255.0f, 255 / 255f);
	public static readonly Color txColorYellow3 = new Color(191 / 255.0f, 133f / 255.0f, 98f / 255.0f, 255 / 255f);
	public static readonly Color txColorYellow4 = new Color(244 / 255.0f, 227f / 255.0f, 208f / 255.0f, 255 / 255f);

	// 倒计时颜色
	public static readonly Color txCountDownColor = new Color(82f / 255.0f, 247f / 255.0f, 255f / 255.0f, 255 / 255f);

	// QinMoon
	public static readonly Color textColorGreen = new Color(33f / 255f, 172f / 255f, 55f / 255f, 255 / 255f);
	public static readonly Color textColorYellow = new Color(255f / 255f, 209f / 255f, 0f / 255f, 255 / 255f);
	public static readonly Color textColorGray = new Color(167f / 255f, 167f / 255f, 167f / 255f, 255 / 255f);
	public static readonly Color textColorOrange = new Color(243f / 255, 151f / 255f, 0f / 255f, 255 / 255f);
	public static readonly Color textColorBlue = new Color(82f / 255, 247f / 255f, 255f / 255f, 255 / 255f);
	public static readonly Color textColorRed = new Color(218f / 255f, 47f / 255f, 60f / 255f, 255 / 255f);
	public static readonly Color textColorBlack = new Color(60f / 255f, 32f / 255f, 21f / 255f, 255 / 255f);
	public static readonly Color textColorDarkBlue = new Color(108f / 255f, 137f / 255f, 147f / 255f, 255 / 255f);

	//QinMoon特殊颜色字体
	public static readonly Color textColorBtnYellow = new Color(240 / 255f, 208f / 255f, 142f / 255f, 255 / 255f);
	public static readonly Color textColorBtnYellow_font = new Color(255 / 255f, 198f / 255f, 0f / 255f, 255 / 255f);
	public static readonly Color textColorBtnBlue_font = new Color(0 / 255f, 255f / 255f, 255f / 255f, 255 / 255f);
	public static readonly Color textColorBtnBrown = new Color(178 / 255f, 144f / 255f, 94f / 255f, 255 / 255f);
	public static readonly Color textColorWhite = new Color(255 / 255f, 237f / 255f, 208f / 255f, 255 / 255f);
	public static readonly Color textColorInfoBrown = new Color(207 / 255f, 170f / 255f, 117f / 255f, 255 / 255f);
	public static readonly Color textColorInOrgYew = new Color(255 / 255f, 111 / 255f, 82 / 255f, 255 / 255f);
	public static readonly Color textColorInBlack = new Color(117 / 255f, 117 / 255f, 117 / 255f, 255 / 255f);
	public static readonly Color textColorTipsInBlack = new Color(240 / 255f, 208 / 255f, 142 / 255f, 255 / 255f);
	public static readonly Color textColorRedOrange = new Color(150 / 255f, 78 / 255f, 59 / 255f, 255 / 255f);
	public static readonly Color textColorZiSe = new Color(170f / 255f, 2f / 255f, 225f / 255f, 255 / 255f);
	public static readonly Color textColorLightRed = new Color(142f / 255f, 35f / 255f, 14f / 255f, 255 / 255f);
	public static readonly Color textColorMessWhite = new Color(255 / 255f, 255f / 255f, 255f / 255f, 255 / 255f);
	public static readonly Color textColorTitleBlue = new Color(126 / 255f, 206f / 255f, 244f / 255f, 255 / 255f);
	public static readonly Color textColorDackGray = new Color(166 / 255f, 147f / 255f, 124f / 255f, 255 / 255f);
	public static readonly Color textColorGuildChat = new Color(108f / 255f, 137 / 255f, 147f / 255f, 255f / 255f);
	public static readonly Color textColorGuildInfo = new Color(248 / 255f, 228 / 255f, 138f / 255f, 255f / 255f);

	// 卡片色
	public static readonly Color cardColorChenSe = new Color(243f / 255f, 151f / 255f, 0f / 255f, 255 / 255f);
	public static readonly Color cardColorZiSe = new Color(179f / 255f, 41f / 255f, 195f / 255f, 255 / 255f);
	public static readonly Color cardColorLanSe = new Color(26f / 255f, 160f / 255f, 244f / 255f, 255 / 255f);
	public static readonly Color cardColorLvSe = new Color(33f / 255f, 172f / 255f, 55f / 255f, 255 / 255f);
	public static readonly Color cardColorBaiSe = new Color(167f / 255f, 167f / 255f, 167f / 255f, 255 / 255f);
	public static readonly Color cardColorGray = new Color(128f / 255f, 128f / 255f, 128f / 255f, 255f / 255f);

	//东海寻仙箭头特殊颜色
	public static readonly Color zentiaColorGreen = new Color(34f / 255f, 172f / 255f, 56f / 255f, 255 / 255f);
	public static readonly Color zentiaColorBlue = new Color(52f / 255f, 143f / 255f, 173f / 255f, 255 / 255f);
	public static readonly Color zentiaColorGrey = new Color(134f / 255f, 135f / 255f, 158f / 255f, 255f / 255f);
	public static readonly Color zentiaTimerTextColor = new Color(0f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);

	// 门派建设
	public static readonly Color guildConstructionIdle = new Color(186f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
	public static readonly Color guildConstructionAccept = new Color(255f / 255f, 0 / 255f, 0f / 255f, 255f / 255f);

	public static readonly Color guildPlayerListOnline1 = new Color(215f / 255f, 185 / 255f, 126f / 255f, 255f / 255f);
	public static readonly Color guildPlayerListOnline2 = new Color(133 / 255f, 133 / 255f, 133f / 255f, 255f / 255f);

	public static readonly Color OrganExchangeToDayTimer1 = new Color(162 / 255f, 162 / 255f, 162 / 255f, 255f / 255f);
	public static readonly Color OrganExchangeToDayTimer2 = new Color(233 / 255f, 190 / 255f, 6 / 255f, 255f / 255f);
}