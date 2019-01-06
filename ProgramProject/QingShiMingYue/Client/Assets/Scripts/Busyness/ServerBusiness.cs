#if UNITY_EDITOR
#define SERVER_BUSSINESS_ENABLE_LOG
#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using KodGames.ClientClass;
using KodGames.ClientHelper;
using KodGames.Common;
using KodGames.Network;
using UnityEngine;

public class ServerBusiness : IBusiness
{
	private ClientHelper clHlp = new ClientHelper();

	private string lgAcc; // Login account.
	private Dictionary<int, string> prtEnum = new Dictionary<int, string>(); // Protocol enum values.

	public void Initialze()
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Initialze");
#endif

		InitPrtEnum();

#if UNITY_IPHONE
		clHlp.Initialize(true, true);
#else
		clHlp.Initialize(true, true);
#endif

		//AuthServer
		clHlp.onCreateAccount = OnCreateAccountRes;
		clHlp.OnLogin = OnLoginRes;
		clHlp.OnAuthActivityCode = OnAuthActivityCode;

		//GameServer
		clHlp.OnReturnExceptionCallback = OnReturnExceptionCallback;
		clHlp.OnISConnected = OnISConnected;
		clHlp.OnISDisconnected = OnISDisconnected;
		clHlp.OnIC_ClientDisconnect = OnIC_ClientDisconnect;
		clHlp.OnQueryManifest = OnQueryManifest;

		clHlp.OnResetPassword = OnResetPasswordRes;
		clHlp.OnBindAccount = OnBindAccountRes;
		clHlp.OnQueryInitInfo = OnQueryInitInfo;

		//InterfaceServer
		clHlp.OnEquipBreakthought = OnEquipBreakthought;
		clHlp.OnEquipLevelUp = OnEquipLevelUp;
		// Diner.
		clHlp.OnQueryDinerList = OnQueryDinerList;
		clHlp.OnHireDiner = OnHireDiner;
		clHlp.OnFireDiner = OnFireDiner;
		clHlp.OnRenewDiner = OnRenewDiner;
		clHlp.OnRefreshDinerList = OnRefreshDinerList;

		// Position
		clHlp.OnQueryPositionList = OnQueryPositionList;
		clHlp.OnOpenPosition = OnOpenPosition;
		clHlp.OnSetMasterPosition = OnSetMasterPosition;
		clHlp.OnEmBattle = OnEmBattle;
		clHlp.OnChangeLocation = OnChangeLocation;
		clHlp.OnOneClickPositionOff = OnOneClickPositionOff;

		// Partner
		clHlp.OnPartnerOpen = OnPartnerOpen;
		clHlp.OnPartnerSetup = OnPartnerSetup;

		// Avatar
		clHlp.OnAvatarLevelUp = OnAvatarLevelUp;
		clHlp.OnAvatarBreakthought = OnAvatarBreakthought;

		clHlp.OnChangeDomineer = OnChangeDomineer;
		clHlp.OnSaveDomineer = OnSaveDomineer;

		clHlp.OnChangeMeridian = OnChangeMeridian;
		clHlp.OnSaveMeridian = OnSaveMeridian;

		//Skill response handler.
		clHlp.OnSkillLevelUp = OnSkillLevelUp;

		//Shop
		clHlp.OnQueryGoodsList = OnQueryGoodsList;
		clHlp.OnBuyGoods = OnBuyGoods;
		clHlp.OnBuyAndUse = OnBuyAndUse;
		//购买体力
		clHlp.OnBuySpecialGoods = OnBuySpecialGoods;

		//Package item
		clHlp.OnConsumeItem = OnConsumeItem;
		clHlp.OnSellItem = OnSellItem;

		//Dungeon
		clHlp.OnQueryDungeonList = OnQueryDungeonList;
		clHlp.OnCombat = OnCombat;
		clHlp.OnSetZoneStatus = OnSetZoneStatus;
		clHlp.OnSetDungeonStatus = OnSetDungeonStatus;
		clHlp.OnResetDungeonCompleteTimes = OnResetDungeonCompleteTimes;
		clHlp.OnDungeonGetReward = OnDungeonGetReward;
		clHlp.OnSetDungeonDialogState = OnSetDungeonDialogState;
		clHlp.OnContinueCombat = OnContinueCombat;
		clHlp.OnQueryTravel = OnQueryTravel;
		clHlp.OnBuyTravel = OnBuyTravel;
		clHlp.OnQueryDungeonGuide = OnQueryDungeonGuide;
		clHlp.OnQueryRecruiteNpc = OnQueryRecruiteNpc;

		//Account
		clHlp.OnQueryEmailListInfo = OnQueryEmailListInfo;
		clHlp.OnGetAttachments = OnGetAttachments;
		clHlp.OnSyncNewEmailCount = OnSyncNewEmailCount;

		//Daily SignIn
		clHlp.OnSignIn = OnSignIn;

		//Chat
		clHlp.OnChat = OnChat;
		//clHlp.OnQueryPlayerInfo = OnQueryPlayerInfo;
		clHlp.OnQueryPlayerMessage = OnQueryPlayerInfo;
		clHlp.OnQueryChatMessageList = OnQueryChatMessageList;
		clHlp.OnSyncWorldAndFlowMessage = OnSyncWorldChatAndFlowMessages;
		clHlp.OnCloseChatMessageDialog = OnCloseChatMessageDialog;

		//Arena
		clHlp.OnQueryArenaRank = OnQueryArenaRank;
		clHlp.OnArenaCombat = OnArenaCombat;
		clHlp.OnQueryArenaPlayerInfo = OnQueryArenaPlayerInfo;
		clHlp.OnQueryRankTopFew = OnQueryRankToFew;

#if ENABLE_AUCTION
		clHlp.OnBuyConsignment = OnBuyConsignment;
		clHlp.OnConsignment = OnConsignment;
		clHlp.OnDownshelf = OnDownshelf;
		clHlp.OnQueryConsignmentList = OnQueryConsignmentList;
		clHlp.OnQuerySelfConsignment = OnQuerySelfConsignment;
		clHlp.OnQueryAvatarList = OnQueryAvatarList;
#endif

		clHlp.OnExchangeCode = OnExchangeCode;
		clHlp.OnSettingFeedback = OnSettingFeedback;

		//Activity
		clHlp.OnQueryGetFixedTimeActivityReward = OnQueryFixedTimeActivityReward;
		clHlp.OnGetFixedTimeActivityReward = OnGetFixedTimeActivityReward;  //FixedTime
		clHlp.OnQueryExchangeList = OnQueryExchangeList;
		clHlp.OnExchange = OnExchangeRes;
		clHlp.OnQueryQinInfo = OnQueryQinInfo;
		clHlp.OnAnswerQinInfo = OnAnswerQinInfo;
		clHlp.OnGetQinInfoContinueReward = OnGetQinInfoContinueReward;
		clHlp.OnMonthCardQueryRes = OnQueryMonthCardInfo;
		clHlp.OnMonthCardPickRewardRes = OnMonthCardPickReward;

		//Tavern
		clHlp.OnTavernBuy = OnTavernBuy;
		clHlp.OnTavernQuery = OnTavernQuery;

		//Recharge
		clHlp.OnApplePurchase = OnApplePurchase;

		//Level activity.
		clHlp.OnLevelRewardGetReward = OnLevelRewardGetReward;
		clHlp.OnQueryLevelReward = OnQueryLevelReward;
		clHlp.OnQueryGradePoint = OnQueryGradePoint;

		//Tutorial.
		clHlp.OnGetTutorialAvatarAndSetPlayerName = OnGetTutorialAvatarAndSetPlayerName;
		clHlp.OnCompleteTutorial = OnCompleteTutorial;
		clHlp.OnNoviceCombat = OnNoviceCombat;
		clHlp.OnFetchRandomPlayerNames = OnFetchRandomPlayerNames;

		// Quest : DailyGuid
		clHlp.OnQueryQuestInfo = OnQueryQuestInfo;
		clHlp.OnPickQuestReward = OnPickQuestReward;

		// PlayerLevelUp
		clHlp.OnGetLevelUpReward = OnGetLevelUpReward;
		clHlp.OnPickStartServerReward = OnPickStartServerReward;

		// MysteryShops
		clHlp.OnMysteryShopQuery = OnQueryMysteryShopInfoRes;
		clHlp.OnMysteryShopRefresh = OnChangeMysteryShopRes;
		clHlp.OnMysteryShopBuy = OnMysteryShopBuy;

		// HandBook
		clHlp.OnMergeIllustration = OnMergeIllustration;
		clHlp.OnQueryIllustration = OnQueryIllustration;

		// Assistant
		clHlp.OnQueryTaskList = OnQueryTaskList;
		clHlp.OnTaskCondition = OnTaskCondition;

		//Tower
		clHlp.OnQueryMelaleucaFloorPlayerInfo = OnQueryMelaleucaFloorPlayerInfo;
		clHlp.OnQueryMelaleucaFloorInfo = OnQueryMelaleucaFloorInfo;
		clHlp.OnMelaleucaFloorCombat = OnMelaleucaFloorCombat;
		clHlp.OnMelaleucaFloorConsequentCombat = OnMelaleucaFloorConsequentCombat;
		clHlp.OnQueryMelaleucaFloorThisWeekRank = OnQueryMelaleucaFloorThisWeekRank;
		clHlp.OnQueryMelaleucaFloorLastWeekRank = OnQueryMelaleucaFloorLastWeekRank;
		clHlp.OnGetMelaleucaFloorWeekReward = OnGetMelaleucaFloorWeekReward;
		clHlp.OnQueryMelaleucaFloorWeekRewardInfo = OnQueryMelaleucaFloorWeekRewardInfo;

		//Wolf
		clHlp.OnQueryWolfSmoke = OnQueryWolfSmoke;
		clHlp.OnQueryWolfSmokeEnemy = OnQueryWolfSmokeEnemy;
		clHlp.OnQueryWolfSmokePosition = OnQueryWolfSmokePosition;
		clHlp.OnQueryWolfSmokeShop = OnQueryWolfSmokeShop;
		clHlp.OnRefreshWolfSmokeShop = OnRefreshWolfSmokeShop;
		clHlp.OnResetWolfSmoke = OnResetWolfSmoke;
		clHlp.OnJoinWolfSmoke = OnJoinWolfSmoke;
		clHlp.OnBuyWolfSmokeShop = OnBuyWolfSmokeShop;
		clHlp.OnCombatWolfSmoke = OnCombatWolfSmoke;

		// IOS Notify.
		clHlp.OnSendNotificationToken = OnSendNotificationToken;

		//Friend
		clHlp.OnQueryFriendList = OnQueryFriendList;
		clHlp.OnRandomFriend = OnRandomFriend;
		clHlp.OnQueryPlayerName = OnQueryPlayerName;
		clHlp.OnInviteFriend = OnInviteFriend;
		clHlp.OnAnswerFriend = OnAnswerFriend;
		clHlp.OnQueryFriendPlayerInfo = OnQueryFriendPlayerInfo;
		clHlp.OnRemoveFriend = OnRemoveFriend;
		clHlp.OnCombatFriend = OnCombatFriend;
		clHlp.OnAddFriend = OnAddFriend;
		clHlp.OnDelFriend = OnDelFriend;

		// ���
		clHlp.OnQueryNotify = OnQueryNotify;

		clHlp.OnGiveFiveStarsEvaluate = OnGiveFiveStarsEvaluate;

		//累计充值
		clHlp.OnOperationActivityQuery = OnOperationActivityQueryRes;
		clHlp.OnOperationActivityPickReward = OnOperationActivityPickRewardRes;

		//奇遇
		clHlp.OnMarvellousNextMarvellousRes = OnMarvellousNextMarvellousRes;
		clHlp.OnMarvellousPickDelayRewardRes = OnMarvellousPickDelayRewardRes;
		clHlp.OnMarvellousQueryDelayRewardRes = OnMarvellousQueryDelayRewardRes;
		clHlp.OnMarvellousQueryRes = OnMarvellousQueryRes;

		//好友战斗系统
		clHlp.OnJoinFriendCampaign = OnJoinFriendCampaignRes;	//参战
		clHlp.OnCombatFriendCampaign = OnCombatFriendCampaignRes; //战斗
		clHlp.OnQueryFriendCampaign = OnQueryFriendCampaignRes;	//主查询
		clHlp.OnResetFriendCampaign = OnResetFriendCampaignRes; //重置
		clHlp.OnQueryFriendCampaignHelpPlayerInfo = OnQueryFriendCampaignHelpPlayerInfoRes;//查看好友

		clHlp.OnQueryFCRank = OnQueryFCRankRes;//排行榜查询协议
		clHlp.OnQueryFCPointDetail = OnQueryFCPointDetailRes;//情义值详细查询
		clHlp.OnQueryFCRankReward = OnQueryFCRankRewarRes;//奖励查询协议
		clHlp.OnFCRankGetReward = OnFCRankGetRewardRes;//领取奖励协议

		//幻化
		clHlp.OnQueryIllusion = OnQueryIllusionRes;
		clHlp.OnActivateIllusion = OnActivateIllusionRes;
		clHlp.OnIllusion = OnIllusionRes;
		clHlp.OnActivateAndIllusion = OnActivateAndIllusion;

		//新神秘商店
		clHlp.OnQueryMysteryer = OnQueryMysteryerRes;
		clHlp.OnRefreshMysteryer = OnRefreshMysteryerRes;
		clHlp.OnBuyMysteryer = OnBuyMysteryerRes;
		clHlp.OnSyncMysteryer = OnSyncMysteryerRes;

		//海外兑换码
		clHlp.OnQueryInviteCodeInfo = OnQueryInviteCodeInfoRes;
		clHlp.OnVerifyInviteCodeAndPickReward = OnVerifyInviteCodeAndPickRewardRes;
		clHlp.OnPickInviteCodeReward = OnPickInviteCodeRewardRes;
		clHlp.OnFacebookShare = OnFacebookShareRes;

		//711活动
		clHlp.OnQuerySevenElevenGift = OnQuerySevenElevenGiftRes;
		clHlp.OnTurnNumber = OnTurnNumberRes;
		clHlp.OnNumberConvert = OnNumberconvertRes;
		//东海寻仙
		clHlp.OnQueryZentia = OnQueryZentia;
		clHlp.OnQueryZentiaFlowMessage = OnQueryZentiaFlowMessage;
		clHlp.OnExchangeZentiaItem = OnExchangeZentiaItem;

		clHlp.OnRefreshZentia = OnRefreshZentia;
		clHlp.OnQueryZentiaGood = OnQueryZentiaGood;
		clHlp.OnBuyZentiaGood = OnBuyZentiaGood;
		clHlp.OnQueryServerZentiaReward = OnQueryServerZentiaReward;

		clHlp.OnGetServerZentiaReward = OnGetServerZentiaReward;
		clHlp.OnQueryZentiaRank = OnQueryZentiaRank;
		clHlp.OnSyncZentia = OnSyncZentia;

		//炼丹房
		clHlp.OnQueryAlchemy = OnQueryAlchemy;
		clHlp.OnPickAlchemyBox = OnPickAlchemyBox;
		clHlp.OnAlchemy = OnAlchemy;
		clHlp.OnQueryDanActivity = OnQueryDanActivity;
		clHlp.OnQueryDanHome = OnQueryDanHome;
		clHlp.OnDanLevelUp = OnDanLevelUp;
		clHlp.OnDanBreakthought = OnDanBreakthought;
		clHlp.OnDanAttributeRefresh = OnDanAttributeRefresh;
		clHlp.OnQueryDanStore = OnQueryDanStore;

		//分解
		clHlp.OnDanDecompose = OnDanDecompose;
		clHlp.OnQueryDanDecompose = OnQueryDanDecompose;
		clHlp.OnLockDan = OnQueryLockDan;

		// 门派
		clHlp.OnAccomplishInvisibleTaskNotify = OnAccomplishInvisibleTaskNotify;
		clHlp.OnGuildNewsNotify = OnGuildNewsNotify;
		clHlp.OnGuildApplyNotify = OnGuildApplyNotify;
		clHlp.OnGuildKickNotify = OnGuildKickNotify;
		clHlp.OnGuildMsgNotify = OnGuildMsgNotify;
		clHlp.OnGuildQuery = OnGuildQuery;
		clHlp.OnGuildSetAnnouncement = OnGuildSetAnnouncement;
		clHlp.OnGuildCreate = OnGuildCreate;
		clHlp.OnGuildQueryGuildList = OnGuildQueryGuildList;
		clHlp.OnGuildApply = OnGuildApply;
		clHlp.OnGuildQuickJoin = OnGuildQuickJoin;
		clHlp.OnGuildViewSimple = OnGuildViewSimple;
		clHlp.OnGuildQueryRankList = OnGuildQueryRankList;
		clHlp.OnGuildQueryMsg = OnGuildQueryMsg;
		clHlp.OnGuildAddMsg = OnGuildAddMsg;
		clHlp.OnGuildQueryNews = OnGuildQueryNews;
		clHlp.OnGuildSetDeclaration = OnGuildSetDeclaration;
		clHlp.OnGuildQueryTransferMember = OnGuildQueryTransferMember;
		clHlp.OnGuildTransfer = OnGuildTransfer;
		clHlp.OnGuildQuit = OnGuildQuit;
		clHlp.OnGuildQueryMember = OnGuildQueryMember;
		clHlp.OnGuildQueryApplyList = OnGuildQueryApplyList;
		clHlp.OnGuildReviewApply = OnGuildReviewApply;
		clHlp.OnGuildOneKeyRefuse = OnGuildOneKeyRefuse;
		clHlp.OnGuildKickPlayer = OnGuildKickPlayer;
		clHlp.OnGuildSetPlayerRole = OnGuildSetPlayerRole;
		clHlp.OnGuildSetAutoEnter = OnGuildSetAutoEnter;
		clHlp.OnQueryConstructionTask = OnQueryConstructionTask;
		clHlp.OnAcceptConstructionTask = OnAcceptConstructionTask;
		clHlp.OnGiveUpConstructionTask = OnGiveUpConstructionTask;
		clHlp.OnCompleteConstructionTask = OnCompleteConstructionTask;
		clHlp.OnRefreshConstructionTask = OnRefreshConstructionTask;
		clHlp.OnQueryGuildPublicShop = OnQueryGuildPublicShop;
		clHlp.OnBuyGuildPublicGoods = OnBuyGuildPublicGoods;
		clHlp.OnQueryGuildPrivateShop = OnQueryGuildPrivateShop;
		clHlp.OnBuyGuildPrivateGoods = OnBuyGuildPrivateGoods;
		clHlp.OnQueryGuildExchangeShop = OnQueryGuildExchangeShop;
		clHlp.OnExchangeGuildExchangeGoods = OnExchangeGuildExchangeGoods;
		clHlp.OnQueryGuildTask = OnQueryGuildTask;
		clHlp.OnDice = OnDice;
		clHlp.OnRefreshGuildTask = OnRefreshGuildTask;

		//门派关卡
		clHlp.OnOpenGuildStage = OnOpenGuildStage;
		clHlp.OnQueryGuildStage = OnQueryGuildStage;
		clHlp.OnGuildStageExplore = OnGuildStageExplore;
		clHlp.OnGuildStageCombatBoss = OnGuildStageCombatBoss;
		clHlp.OnGuildStageGiveBox = OnGuildStageGiveBox;
		clHlp.OnGuildStageReset = OnGuildStageReset;
		clHlp.OnGuildStageQueryMsg = OnGuildStageQueryMsg;
		clHlp.OnGuildStageQueryBossRank = OnGuildStageQueryBossRank;
		clHlp.OnGuildStageQueryBossRankDetail = OnGuildStageQueryBossRankDetail;
		clHlp.OnGuildStageQueryExploreRank = OnGuildStageQueryExploreRank;
		clHlp.OnGuildStageQueryRank = OnGuildStageQueryRank;
		clHlp.OnGuildStageQueryTalent = OnGuildStageQueryTalent;
		clHlp.OnGuildStageTalentReset = OnGuildStageTalentReset;
		clHlp.OnGuildStageTalentAdd = OnGuildStageTalentAdd;

		clHlp.OnQueryFacebook = OnQueryFacebook;
		clHlp.OnFacebookReward = OnFacebookReward;

		//修改玩家名称
		clHlp.OnSetPlayerName = OnSetPlayerNameRes;
		clHlp.OnSetGuildName = OnSetGuildNameRes;

		//机关兽
		clHlp.OnActiveBeast = OnActiveBeast;
		clHlp.OnEquipBeastPart = OnEquipBeastPart;
		clHlp.OnBeastBreakthought = OnBeastBreakthought;
		clHlp.OnBeastLevelUp = OnBeastLevelUp;
		clHlp.OnQueryBeastExchangeShop = OnQueryBeastExchangeShop;
		clHlp.OnBeastExchangeShop = OnBeastExchangeShop;
		clHlp.OnRefreshBeastExchangeShop = OnRefreshBeastExchangeShop;

	}

	public bool DoesSupprotReconnect()
	{
		return clHlp.getReconnectFlag();
	}

	private void OnReturnExceptionCallback(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnReturnExceptionCallback " + result);
#endif
		RequestMgr.Inst.Response(new ReturnExceptionCallbackRes(callback, result, PtrErrStr(result)));
	}

	public void Dispose()
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Dispose");
#endif

		// Disconnected.
		clHlp.DisconnectIS();

		prtEnum.Clear();
	}

	public void Update()
	{
		clHlp.Update();
	}

	public string PtrErrStr(int prtVal)
	{
		string errKey = "";
		string str = "";

		if (!prtEnum.ContainsKey(prtVal))
			errKey = string.Format("UndefinedErrorCode_{0:X8}", prtVal);
		else
			errKey = prtEnum[prtVal];

		ClientServerCommon.StringsConfig strCfg = ClientServerCommon.ConfigDatabase.DefaultCfg.StringsConfig;

		if (strCfg.HasString(GameDefines.strSrvErr, errKey))
			str = strCfg.GetString(GameDefines.strSrvErr, errKey);
		else
			str = string.Format("Error Code: {0}", prtVal);

		Debug.Log(str);
		return str;
	}

	private void InitPrtEnum()
	{
		Type prtTp = typeof(com.kodgames.corgi.protocol.Protocols);

		FieldInfo[] fds = prtTp.GetFields();

		for (int i = 0; i < fds.Length; i++)
		{
			FieldInfo fd = fds[i];

			if (!fd.IsStatic)
				continue;

			object val = fd.GetValue(null);

			if (!(val is int))
				continue;

			prtEnum[(int)val] = fd.Name;
		}
	}

	#region AuthServer

	public bool CreateAccount(string authServerHostName, int port, string email, string password, int channelId, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] CreateAccountReq");
#endif
		return clHlp.createAccount(authServerHostName, port, email, password, mobile, deviceInfo, klsso, callback, channelId);
	}

	private void OnCreateAccountRes(int result, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnCreateAccountRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_CREATE_ACCOUNT_SUCCESS)
			RequestMgr.Inst.Response(new CreateAccountRes(callback));
		else
			RequestMgr.Inst.Response(new CreateAccountRes(callback, result, PtrErrStr(result)));
	}

	public bool AuthActivityCode(string authServerHostName, int port, int accountId, string activityCode, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] AuthActivityCode " + activityCode);
#endif
		return clHlp.authActivityCode(authServerHostName, port, accountId, activityCode, callback);
	}

	private void OnAuthActivityCode(int result, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAuthActivityCode " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_ACTIVITY_CODE_SUCCESS)
			RequestMgr.Inst.Response(new AuthActivityCodeRes(callback));
		else
			RequestMgr.Inst.Response(new AuthActivityCodeRes(callback, result, PtrErrStr(result)));
	}

	public bool Login(string authServerHostName, int port, string email, string password, string version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, string channelUniqueId, string token, AccountChannel accountChannel, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] LoginReq " + email + " " + password);
#endif
		return clHlp.login(authServerHostName, port, email, password, version, channelID, deviceInfo, channelUniqueId, token, accountChannel, callback);
	}

	public bool QuickLogin(string authServerHostName, int port, int version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, string bindedAccount, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QuickLoginReq");
#endif
		return clHlp.quickLogin(authServerHostName, port, version, channelID, deviceInfo, klsso, bindedAccount, callback);
	}

	private void OnLoginRes(int result, int accountID, List<Area> areas, string token, int lastAreaID, int callback, bool isFirstQuickLogin, bool isShowActivityInterface, ChannelMessage channelMessage)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnLoginRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_LOGIN_SUCCESS)
			RequestMgr.Inst.Response(new LoginRes(callback, accountID, areas, token, lastAreaID, false, isFirstQuickLogin, isShowActivityInterface, channelMessage));
		else if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_QUICK_LOGIN_SUCCESS)
			RequestMgr.Inst.Response(new LoginRes(callback, accountID, areas, token, lastAreaID, true, isFirstQuickLogin, isShowActivityInterface, channelMessage));
		else
			RequestMgr.Inst.Response(new LoginRes(callback, result, PtrErrStr(result)));
	}

	public bool DisconnectAS()
	{
		return clHlp.DisconnectAS();
	}

	public int GetNetStatus()
	{
		return clHlp.getNetStatus();
	}

	public bool DisconnectIS()
	{
		return clHlp.DisconnectIS();
	}

	public bool SendTimeout()
	{
		return clHlp.SendTimeout();
	}

	public bool ConnectIS(string hostname, int port, NetType netType, int areaID, int callback)
	{
		Debug.Log(string.Format("[ServerBusiness] ConnectIS : Host:{0} Port:{1}", hostname, port));
		return clHlp.ConnectIS(hostname, port, netType, areaID, callback);
	}

	private void OnISConnected(int result, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnISConnected " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_INTERFACE_AUTH_TOKEN_SUCCESS)
			RequestMgr.Inst.Response(new ConnectISRes(callback));
		else
			RequestMgr.Inst.Response(new ConnectISRes(callback, result, PtrErrStr(result)));
	}

	public void OnISDisconnected(int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnISDisconnecd " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RECONNECT_SEQID_ERROR)
			// SeqID异常，需要重新QueryInitInfo
			RequestMgr.Inst.ConnectionOutOfSync();
		else if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RECONNECT_RELOGIN)
			// 数据异常，需要重新登录
			RequestMgr.Inst.Broke(PtrErrStr(result), true);

		// 其他情况不做处理,等待RequestManger超时
	}

	private void OnIC_ClientDisconnect(string reason, int playerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnIC_ClientDisconnect ");
#endif
		RequestMgr.Inst.Broke(reason, true);
	}

	public bool Logout(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Logout");
#endif
		//Disconnected.
		return clHlp.DisconnectIS();
	}

	public bool QueryManifest(string authServerHostName, int port, int resourceVersion, int version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, int callback, int subChannelID)
	{
		Debug.Log(string.Format("[ServerBusiness] QueryManifestReq : Host:{0} Port:{1}", authServerHostName, port));
		return clHlp.queryManifest(authServerHostName, port, resourceVersion, version, channelID, deviceInfo, callback, subChannelID);
	}

	private void OnQueryManifest(int result, int callback, int appVersion, string appDownloadURL, string appUpdateDesc, string baseResourceUpdateURL, string gameConfigName, int gameConfigSize, int gameConfigUncompressedSize, int timeZone, string maintainNotice, bool isForcedUpdate)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryManifestRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_QUERY_MANIFEST_SUCCESS)
			RequestMgr.Inst.Response(new QueryManifestRes(callback, appVersion, appDownloadURL, appUpdateDesc, baseResourceUpdateURL, gameConfigName, gameConfigSize, gameConfigUncompressedSize, isForcedUpdate));
		else if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_QUERY_MANIFEST_FAILED_SERVER_IS_UNDER_MAINTENANCE)
			RequestMgr.Inst.Response(new QueryManifestRes(callback, result, maintainNotice));
		else
			RequestMgr.Inst.Response(new QueryManifestRes(callback, result, PtrErrStr(result)));
	}
	#endregion

	#region GameServer
	public bool ResetPassword(string authServerHostName, int port, string email, string oldPassword, string newPassword, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ResetPasswordReq");
#endif
		return clHlp.resetPassword(authServerHostName, port, email, oldPassword, newPassword, callback);
	}

	private void OnResetPasswordRes(int result, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnResetPasswordRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_RESET_PASSWORD_SUCCESS)
			RequestMgr.Inst.Response(new ResetPasswordRes(callback));
		else
			RequestMgr.Inst.Response(new ResetPasswordRes(callback, result, PtrErrStr(result)));
	}

	public bool BindAccount(string authServerHostName, int port, string email, string password, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BindAccountReq");
#endif
		return clHlp.bindAccount(authServerHostName, port, email, password, mobile, deviceInfo, klsso, callback);
	}

	public void OnBindAccountRes(int result, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBindAccountRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_AUTH_BIND_ACCOUNT_SUCCESS)
			RequestMgr.Inst.Response(new BindAccountRes(callback));
		else
			RequestMgr.Inst.Response(new BindAccountRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryInitInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryInitInfoReq");
#endif
		return clHlp.queryInitInfo(callback);
	}

	private void OnQueryInitInfo(int callback, int result, KodGames.ClientClass.Player player, List<com.kodgames.corgi.protocol.ActivityData> activityData, List<com.kodgames.corgi.protocol.Notice> notices, bool showNotice, int serverType, KodGames.ClientClass.Function isFunctionOpen, int assistantNum)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryInitInfoRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_INIT_INFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryInitInfoRes(callback, player, activityData, serverType, notices, showNotice, isFunctionOpen, assistantNum));
		else
			RequestMgr.Inst.Response(new QueryInitInfoRes(callback, result, PtrErrStr(result)));
	}

	#endregion

	#region Notification

	public bool SendAPNToken(int callback, byte[] token)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SendAPNToken");
#endif

		return clHlp.sendNotificationToken(callback, token);
	}

	private void OnSendNotificationToken(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSendNotificationToken " + result);
#endif
		RequestMgr.Inst.Response(new GenericResponse(callback));
	}

	#endregion Notification

	#region Level Reward

	public bool LevelRewardGetReward(int callback, int levelRewardId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GetReward");
#endif
		return clHlp.levelRewardGetReward(callback, levelRewardId);
	}

	public void OnLevelRewardGetReward(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnLevelRewardGetReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_LEVELREWARD_GET_REWARDS_SUCCESS)
			RequestMgr.Inst.Response(new LevelRewardGetRewardRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new LevelRewardGetRewardRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool QueryLevelReward(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryLevelReward");
#endif
		return clHlp.queryLevelReward(callback);
	}

	public void OnQueryLevelReward(int callback, int result, List<KodGames.ClientClass.LevelReward> levelRewards)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryLevelReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_LEVELREWARD_SUCCESS)
			RequestMgr.Inst.Response(new QueryLevelRewardRes(callback, levelRewards));
		else
			RequestMgr.Inst.Response(new QueryLevelRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion Level Reward

	#region Position

	public bool QueryPositionList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryPositionList");
#endif
		return clHlp.QueryPositionList(callback);
	}

	public void OnQueryPositionList(int callback, int result, KodGames.ClientClass.PositionData positonData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryPositionList " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_POSITION_LIST_SUCCESS)
			RequestMgr.Inst.Response(new QueryPositionListRes(callback, positonData));
		else
			RequestMgr.Inst.Response(new QueryPositionListRes(callback, result, PtrErrStr(result)));
	}

	public bool OpenPosition(int callback, int positionId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OpenPosition");
#endif

		return clHlp.OpenPosition(callback, positionId);
	}

	public void OnOpenPosition(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardSync, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnOpenPosition " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_OPEN_POSITION_SUCCESS)
			RequestMgr.Inst.Response(new OpenPositionRes(callback, costAndRewardSync, position));
		else
			RequestMgr.Inst.Response(new OpenPositionRes(callback, result, PtrErrStr(result), costAndRewardSync));
	}

	public bool SetMasterPosition(int callback, int positionId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetMasterPosition");
#endif
		return clHlp.SetMasterPosition(callback, positionId);
	}

	public void OnSetMasterPosition(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSetMasterPosition " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_MASTER_POSITION_SUCCESS)
			RequestMgr.Inst.Response(new SetMasterPositionRes(callback));
		else
			RequestMgr.Inst.Response(new SetMasterPositionRes(callback, result, PtrErrStr(result)));
	}

	public bool EmBattle(int callback, int positionId, int locationId1, int locationId2)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] EmBattle");
#endif
		return clHlp.EmBattle(callback, positionId, locationId1, locationId2);
	}

	public void OnEmBattle(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnEmBattle " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EM_BATTLE_SUCCESS)
			RequestMgr.Inst.Response(new EmBattleRes(callback));
		else
			RequestMgr.Inst.Response(new EmBattleRes(callback, result, PtrErrStr(result)));
	}

	public bool ChangeLocation(int callback, string guid, int resourceId, string offGuid, int positionId, int location, int index)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ChangeLocation");
#endif
		return clHlp.ChangeLocation(callback, guid, resourceId, offGuid, positionId, location, index);
	}

	public void OnChangeLocation(int callback, int result, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ChangeLocation " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHANGE_LOCATION_SUCCESS)
			RequestMgr.Inst.Response(new ChangeLocationRes(callback, position));
		else
			RequestMgr.Inst.Response(new ChangeLocationRes(callback, result, PtrErrStr(result)));
	}

	public bool OneClickPositionOff(int callback, int positionId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OneClickPositionOff");
#endif
		return clHlp.OneClickPositionOff(callback, positionId);
	}

	public void OnOneClickPositionOff(int callback, int result, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnOneClickPositionOff " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ONE_CLICK_POSITIOIN_OFF_SUCCESS)
			RequestMgr.Inst.Response(new OneClickPositionOffRes(callback, position));
		else
			RequestMgr.Inst.Response(new OneClickPositionOffRes(callback, result, PtrErrStr(result)));
	}

	#endregion Position

	#region Partner

	public bool PartnerOpen(int callback, int partnerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PartnerOpen ");
#endif
		return clHlp.partnerOpen(callback, partnerId);
	}

	public void OnPartnerOpen(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnPartnerOpen " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_PARTNER_OPEN_SUCCESS)
			RequestMgr.Inst.Response(new PartnerOpenRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new PartnerOpenRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool PartnerSetup(int callback, int positionId, int partnerId, string avatarGuid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PartnerOpen ");
#endif
		return clHlp.partnerSetup(callback, positionId, partnerId, avatarGuid);
	}

	public void OnPartnerSetup(int callback, int result, List<KodGames.ClientClass.Partner> partners)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PartnerOpen ");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_PARTNER_SETUP_SUCCESS)
			RequestMgr.Inst.Response(new PartnerSetupRes(callback, partners));
		else
			RequestMgr.Inst.Response(new PartnerSetupRes(callback, result, PtrErrStr(result)));
	}

	#endregion Partner

	#region Avatar

	public bool AvatarLevelUp(int callback, string avatarGUID, bool levelUpType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] AvatarLevelUp");
#endif
		return clHlp.avatarLevelUp(callback, avatarGUID, levelUpType);
	}

	private void OnAvatarLevelUp(int callback, int result, int levelAfter, CostAndRewardAndSync costAndRewardAndSync, int critCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAvatarLevelUp " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_AVATAR_LEVEL_UP_SUCCESS)
			RequestMgr.Inst.Response(new AvatarLevelUpRes(callback, levelAfter, costAndRewardAndSync, critCount));
		else
			RequestMgr.Inst.Response(new AvatarLevelUpRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool AvatarBreakthought(int callback, string avatarGUID, List<string> destroyAvatarGUIDs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BreakthoughtAvatar");
#endif
		return clHlp.avatarBreakthought(callback, avatarGUID, destroyAvatarGUIDs);
	}

	private void OnAvatarBreakthought(int callback, int result, int breakThoughLevel, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBreakthoughtRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_AVATAR_BREAKTHOUGHT_SUCCESS)
			RequestMgr.Inst.Response(new AvatarBreakthoughtRes(callback, breakThoughLevel, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new AvatarBreakthoughtRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	// ChangeMeridianReq : Request
	public bool ChangeMeridian(int callback, int meridianId, string avatarGuid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ChangeMeridian");
#endif
		return clHlp.ChangeMeridian(callback, meridianId, avatarGuid);
	}

	// ChangeMeridianRes : Response
	public void OnChangeMeridian(int callback, int result, List<KodGames.ClientClass.PropertyModifier> newModifiers, KodGames.ClientClass.CostAndRewardAndSync costAndReward, int meridianTimes, int bufferId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnChangeMeridian " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHANGE_MEIRIDIAN_SUCCESS)
			RequestMgr.Inst.Response(new ChangeMeridianRes(callback, newModifiers, costAndReward, meridianTimes, bufferId));
		else
			RequestMgr.Inst.Response(new ChangeMeridianRes(callback, result, PtrErrStr(result), costAndReward));
	}

	// SaveMeridianReq : Request
	public bool SaveMeridian(int callback, string avatarGuid, bool saveOrNot, int meridianId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SaveMeridian");
#endif
		return clHlp.SaveMeridian(callback, avatarGuid, saveOrNot, meridianId);
	}

	// SaveMeridianRes : Response
	public void OnSaveMeridian(int callback, int result, List<KodGames.ClientClass.PropertyModifier> newModifiers, int bufferId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSaveMeridian " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SAVE_MERIDIAN_SUCCESS)
			RequestMgr.Inst.Response(new SaveMeridianRes(callback, newModifiers, bufferId));
		else
			RequestMgr.Inst.Response(new SaveMeridianRes(callback, result, PtrErrStr(result)));
	}

	#endregion Avatar

	#region Equipment

	// Equipment LevelUp Request
	public bool EquipLevelUp(int callback, String equipGUID, bool strengthenType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] EquipLevelUp");
#endif
		return clHlp.equipLevelUp(callback, equipGUID, strengthenType);
	}

	public void OnEquipLevelUp(int callback, int result, int levelAfter, CostAndRewardAndSync costAndRewardAndSync, int critCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Equipment] OnEquipLevelUp");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EQUIP_LEVEL_UP_SUCCESS)
			RequestMgr.Inst.Response(new EquipmentLevelUpRes(callback, levelAfter, costAndRewardAndSync, critCount));
		else
			RequestMgr.Inst.Response(new EquipmentLevelUpRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	// Equipment Breakthought Request
	public bool EquipBreakthought(int callback, string equipGUID, List<string> destroyEquipGUIDs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] EquipBreakthought");
#endif
		return clHlp.equipBreakthought(callback, equipGUID, destroyEquipGUIDs);
	}

	// Equipment Breakthought Response
	public void OnEquipBreakthought(int callback, int result, Equipment equipment, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Equipment] OnEquipBreakthought " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EQUIP_BREAKTHOUGHT_SUCCESS)
			RequestMgr.Inst.Response(new EquipmentBreakthoutRes(callback, equipment, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new EquipmentBreakthoutRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion Equipment

	#region Skill

	public void OnSkillLevelUp(int callback, int result, Skill skill, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Skill] OnSkillLevelUp" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SKILL_LEVEL_UP_SUCCESS)
			RequestMgr.Inst.Response(new SkillLevelUpRes(callback, skill, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new SkillLevelUpRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool SkillLevelUp(int callback, string skillGUID, List<string> destroySkillGUIDs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SkillLevelUp");
#endif
		return clHlp.skillLevelUp(callback, skillGUID, destroySkillGUIDs);
	}
	#endregion

	#region Hire

	public bool QueryDinerList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDinerList ");
#endif
		return clHlp.QueryDinerList(callback);
	}

	public void OnQueryDinerList(int callback, int result, List<KodGames.ClientClass.DinerPackage> dinerPackages)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshDinerList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DINER_LIST_DINER_SUCCESS)
			RequestMgr.Inst.Response(new QueryDinerListRes(callback, dinerPackages));
		else
			RequestMgr.Inst.Response(new QueryDinerListRes(callback, result, PtrErrStr(result)));
	}

	public bool HireDiner(int callback, int qualityType, int dinerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] HireDiner ");
#endif
		return clHlp.HireDiner(callback, qualityType, dinerId);
	}

	public void OnHireDiner(int callback, int result, KodGames.ClientClass.HiredDiner hiredDiner, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnHireDiner " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_HIRE_DINER_SUCCESS)
			RequestMgr.Inst.Response(new HireDinerRes(callback, hiredDiner, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new HireDinerRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool RenewDiner(int callback, int qualityType, int dinerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RenewDiner ");
#endif
		return clHlp.RenewDiner(callback, qualityType, dinerId);
	}

	public void OnRenewDiner(int callback, int result, KodGames.ClientClass.HiredDiner hiredDiner, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRenewDiner " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RENEW_DINER_SUCCESS)
			RequestMgr.Inst.Response(new RenewDinerRes(callback, hiredDiner, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new RenewDinerRes(callback, result, PtrErrStr(result)));
	}

	public bool FireDiner(int callback, int qualityType, int dinerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] FireDiner ");
#endif
		return clHlp.FireDiner(callback, qualityType, dinerId);
	}

	public void OnFireDiner(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnFireDiner " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_FIRE_DINER_SUCCESS)
			RequestMgr.Inst.Response(new FireDinerRes(callback));
		else
			RequestMgr.Inst.Response(new FireDinerRes(callback, result, PtrErrStr(result)));
	}

	public bool RefreshDinerList(int callback, int bagId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RefreshDinerList ");
#endif
		return clHlp.RefreshDinerList(callback, bagId);
	}

	public void OnRefreshDinerList(int callback, int result, KodGames.ClientClass.DinerPackage dinerPackage, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshDinerList " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_DINER_LIST_SUCCESS)
			RequestMgr.Inst.Response(new RefreshDinerListRes(callback, dinerPackage, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new RefreshDinerListRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion Hire

	#region bag

	public bool ConsumeItem(int callback, int itemId, int amount, int groupIndex, string phoneNumber)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ConsumeItem");
#endif
		return clHlp.consumeItem(callback, itemId, amount, groupIndex, phoneNumber);
	}

	public void OnConsumeItem(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[bag] OnConsumeItemRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CONSUME_ITEM_SUCCESS)
			RequestMgr.Inst.Response(new ConsumeItemRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ConsumeItemRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool SellItem(int callback, List<KodGames.ClientClass.Cost> items)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SellItem");
#endif
		return clHlp.sellItem(callback, items);
	}

	public void OnSellItem(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[bag] OnSellItemRes " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ITEM_SELL_SUCCESS)
			RequestMgr.Inst.Response(new SellItemRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new SellItemRes(callback, result, PtrErrStr(result)));
	}

	#endregion bag

	#region Daily SignIn

	public bool SignIn(int callback, int signType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SignIn");
#endif
		return clHlp.signIn(callback, signType);
	}

	private void OnSignIn(int callback, int result, int signType, Reward reward, Reward specialReward, CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.SignData signData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[SignIn] SignInRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SIGN_IN_SUCCESS)
			RequestMgr.Inst.Response(new SignInRes(callback, signType, reward, specialReward, costAndRewardAndSync, signData));
		else
			RequestMgr.Inst.Response(new SignInRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion Daily SignIn

	#region FixedTime

	public bool QueryFixedTimeActivityReward(int callback)//FixedTime
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFixedTimeActivityReward");
#endif
		return clHlp.queryGetFixedTimeActivityReward(callback);
	}

	private void OnQueryFixedTimeActivityReward(int callback, int result, List<KodGames.ClientClass.Reward> reward, long lastGetTime, int resetType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnQueryFixedTimeActivityReward" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.P_GAME_CG_QUERY_GET_FIXEDTIME_ACTIVITY_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new QueryFixedTimeActivityRewardRes(callback, reward, lastGetTime, resetType));
		else
			RequestMgr.Inst.Response(new QueryFixedTimeActivityRewardRes(callback, result, PtrErrStr(result)));
	}

	public bool GetFixedTimeActivityReward(int callback)//FixedTime
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GetFixedTimeActivityReward");
#endif
		return clHlp.getFixedTimeActivityReward(callback);
	}

	private void OnGetFixedTimeActivityReward(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, long lastGetTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnGetFixedTimeActivityReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GET_FIXEDTIME_ACTIVITY_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new GetFixedTimeActivityRewardRes(callback, costAndRewardAndSync, lastGetTime));
		else
			RequestMgr.Inst.Response(new GetFixedTimeActivityRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion FixedTime

	#region Recharge

	public bool ApplePurchase(byte[] paymentTransactionReceipt, int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ApplePurchase");
#endif
		return clHlp.ApplePurchase(callback, paymentTransactionReceipt);
	}

	public bool ApplePurchaseTest(int goodsID, int count, int callback, string additionalData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ApplePurchaseTest");
#endif
		return clHlp.ApplePurchase(callback, goodsID, count, additionalData);
	}

	public void OnApplePurchase(int callback, int result, int commodityID, int commodityCount, string transactionIdentifier, int realMoneyDelta, int totalConsumedRMB, int vipLevel, int remainingRMB, List<int> payStatus)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnApplePurchase " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_PURCHASE_SUCCESS || result == com.kodgames.corgi.protocol.Protocols.E_GAME_PURCHASE_MONTH_CARD_SUCCESS)
			RequestMgr.Inst.Response(new OnApplePurchaseRes(callback, commodityID, commodityCount, transactionIdentifier, realMoneyDelta, totalConsumedRMB, vipLevel, remainingRMB, PtrErrStr(result), payStatus));
		else
			RequestMgr.Inst.Response(new OnApplePurchaseRes(callback, result, PtrErrStr(result)));
	}

	#endregion Recharge

	#region PlayerLevelUp

	public bool GetLevelUpReward(int callback, int wantPickLevel)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GetLevelUpReward");
#endif
		return clHlp.getLevelUpReward(callback, wantPickLevel);
	}

	private void OnGetLevelUpReward(int callabck, int result, int currentPickedLevel, KodGames.ClientClass.LevelAttrib levelAttri, KodGames.ClientClass.CostAndRewardAndSync crs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGetLevelUpReward result : " + result);
#endif

		//RequestMgr.Inst.Response(new OnNotifyLevelUpRewardRes(levelAttri, crs));
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GET_LEVELUP_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new GetLevelUpRewardRes(callabck, levelAttri, crs));
		else
			RequestMgr.Inst.Response(new GetLevelUpRewardRes(callabck, result, PtrErrStr(result)));
	}

	#endregion PlayerLevelUp

	#region Goods

	public bool QueryGoodsList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGoodsList");
#endif
		return clHlp.queryGoodsList(callback);
	}

	private void OnQueryGoodsList(int callback, int result, List<KodGames.ClientClass.Goods> goods, long nextRefreshTime, bool isMelaleucaShopOpen)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Shop] OnQueryGoodsList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GOODS_LIST_SUCCESS)
			RequestMgr.Inst.Response(new QueryGoodsRes(callback, goods, nextRefreshTime, isMelaleucaShopOpen));
		else
			RequestMgr.Inst.Response(new QueryGoodsRes(callback, result, PtrErrStr(result)));
	}

	public bool BuyGoods(int callback, KodGames.ClientClass.GoodRecord goodRecord, int statusIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyGoods");
#endif
		return clHlp.buyGoods(callback, goodRecord, statusIndex);
	}

	private void OnBuyGoods(int callback, int result, KodGames.ClientClass.Goods good, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, string notEnoughText)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Shop] OnBuyGoodsRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GOODS_SUCCESS)
			RequestMgr.Inst.Response(new BuyGoodsRes(callback, good, costAndRewardAndSync, notEnoughText));
		else
			RequestMgr.Inst.Response(new BuyGoodsRes(callback, result, PtrErrStr(result), good, costAndRewardAndSync, notEnoughText));
	}

	public bool BuyAndUse(int callback, int goodsId, int statusIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyAndUse");
#endif
		return clHlp.buyAndUse(callback, goodsId, statusIndex);
	}

	private void OnBuyAndUse(int callback, int result, KodGames.ClientClass.Goods good, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyAndUse " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_AND_USE_SUCCESS)
			RequestMgr.Inst.Response(new BuyAndUseRes(callback, good, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new BuyAndUseRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool BuySpecialGoods(int callback, int goodId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuySpecialGoods goodsId=" + goodId);
#endif
		return clHlp.buySpecialGoods(callback, goodId);
	}

	private void OnBuySpecialGoods(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuySpecialGoods goodId= " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_SPECIAL_GOODS_SUCCESS)
			RequestMgr.Inst.Response(new BuySpecialGoodsRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new BuySpecialGoodsRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion Goods

	#region Dungeon

	// Requeset QueryDungeonList
	public bool QueryDungeonList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDungeonList");
#endif
		return clHlp.queryDungeonList(callback);
	}

	// Respones QueryDungeonList
	public void OnQueryDungeonList(int callback, int result, List<Zone> zones, List<Zone> secretZones, int lastDungeonId, int lastZoneId, int lastSecretDungeonId, int lastSecretZoneId, int positionId, long lastResetDungeonTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryDungeonList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DUNGEON_LIST_SUCCESS)
			RequestMgr.Inst.Response(new QueryDungeonListRes(callback, zones, secretZones, lastDungeonId, lastZoneId, lastSecretDungeonId, lastSecretZoneId, positionId, lastResetDungeonTime));
		else
			RequestMgr.Inst.Response(new QueryDungeonListRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryRecruiteNpc(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryRecruiteNpc ");
#endif
		return clHlp.queryRecruiteNpc(callback, dungeonId);
	}

	public void OnQueryRecruiteNpc(int callback, int result, List<RecruiteNpc> recruiteNpcs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryRecruiteNpc " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_RECRUITE_NPC_SUCCESS)
			RequestMgr.Inst.Response(new QueryRecruiteNpcRes(callback, recruiteNpcs));
		else
			RequestMgr.Inst.Response(new QueryRecruiteNpcRes(callback, result, PtrErrStr(result)));
	}

	// OnCombat.
	public bool Combat(int callback, int dungeonId, KodGames.ClientClass.Position position, int npcId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Combat] Combat");
#endif
		return clHlp.combat(callback, dungeonId, position, npcId);
	}

	private void OnCombat(int callback, int result, CombatResultAndReward combatResultAndReward, CostAndRewardAndSync costAndRewardAndSync, int zoneStatus, Dungeon dungeon, TravelData travelData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[OnCombat] OnCombatRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_COMBAT_SUCCESS)
			RequestMgr.Inst.Response(new OnCombatRes(callback, combatResultAndReward, costAndRewardAndSync, zoneStatus, dungeon, travelData));
		else
			RequestMgr.Inst.Response(new OnCombatRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	// Request SetZoneStatus .
	public bool SetZoneStatus(int callback, int zoneId, int status)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetZoneStatusReq");
#endif
		return clHlp.setZoneStatus(callback, zoneId, status);
	}

	// Response SetZoneStatus .
	public void OnSetZoneStatus(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetZoneStatusRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_ZONE_STATUS_SUCCESS)
			RequestMgr.Inst.Response(new SetZoneStatusRes(callback));
		else
			RequestMgr.Inst.Response(new SetZoneStatusRes(callback, result, PtrErrStr(result)));
	}

	// Request SetDungeonStatus.
	public bool SetDungeonStatus(int callback, int dungeonId, int status)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetDungeonStatus");
#endif
		return clHlp.setDungeonStatus(callback, dungeonId, status);
	}

	// Response SetDungeonStatus.
	public void OnSetDungeonStatus(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSetDungeonStatus " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_DUNGEON_STATUS_SUCCESS)
			RequestMgr.Inst.Response(new SetDungeonStatusRes(callback));
		else
			RequestMgr.Inst.Response(new SetDungeonStatusRes(callback, result, PtrErrStr(result)));
	}

	// Request Reset DungeonComplete Times.
	public bool ResetDungeonCompleteTimes(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ResetDungeonCompleteTimes");
#endif
		return clHlp.resetDungeonCompleteTimes(callback, dungeonId);
	}

	// Response Reset DungeonComplete Times.
	public void OnResetDungeonCompleteTimes(int callback, int result, int todayCompleteTimes, int alreadyResetTimes, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnResetDungeonCompleteTimes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RESET_DUNGEON_COMPLETE_TIMES_SUCCESS)
			RequestMgr.Inst.Response(new ResetDungeonCompleteTimesRes(callback, todayCompleteTimes, alreadyResetTimes, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ResetDungeonCompleteTimesRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	// Request GetDungeonComplementReward.
	public bool DungeonGetReward(int callback, int zoneId, int dungeonDifficulty, int boxIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] DungeonGetReward");
#endif
		return clHlp.dungeonGetReward(callback, zoneId, dungeonDifficulty, boxIndex);
	}

	// Response GetDungeonComplementReward.
	public void OnDungeonGetReward(int callback, int result, List<int> boxPickedIndexs, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnDungeonGetReward " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_DUNGEON_GET_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new GetDungeonRewardRes(callback, boxPickedIndexs, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new GetDungeonRewardRes(callback, result, PtrErrStr(result)));
	}

	// Request SetDungeonDialogue
	public bool SetDungeonDialogState(int callback, int dungeonId, int state)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetDungeonDialogState ");
#endif
		return clHlp.setDungeonDialogState(callback, dungeonId, state);
	}

	// Response SetDungeonDialogue
	public void OnSetDungeonDialogState(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetDungeonDialogState ");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_DUNGEON_DIALOG_STATE_SUCCESS)
			RequestMgr.Inst.Response(new GenericResponse(callback));
		else
			RequestMgr.Inst.Response(new GenericResponse(callback, result, PtrErrStr(result)));
	}

	// Request ContinueCombat
	public bool ContinueCombat(int callback, int zoneId, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ContinueCombat ");
#endif
		return clHlp.continueCombat(callback, zoneId, dungeonId);
	}

	// Response ContinueCombat
	public void OnContinueCombat(int callback, int result, Dungeon dungeon, List<CombatResultAndReward> rewards, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ContinueCombat " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CONTINUE_COMBAT_SUCCESS)
			RequestMgr.Inst.Response(new ContinueCombatRes(callback, dungeon, rewards, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ContinueCombatRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	// Query Guide.
	public bool QueryDungeonGuide(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDungeonGuide ");
#endif
		return clHlp.queryDungeonGuide(callback, dungeonId);
	}

	public void OnQueryDungeonGuide(int callback, int result, List<KodGames.ClientClass.DungeonGuideNpc> dungeonGuideNpc)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryDungeonGuide " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DUNGEON_GUIDE_SUCCESS)
			RequestMgr.Inst.Response(new QueryDungeonGuideRes(callback, dungeonGuideNpc));
		else
			RequestMgr.Inst.Response(new QueryDungeonGuideRes(callback, result, PtrErrStr(result)));
	}

	// Travel Shop.
	public bool QueryTravel(int callback, int dungeonId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryTravel ");
#endif
		return clHlp.queryTravel(callback, dungeonId);
	}

	public void OnQueryTravel(int callback, int result, TravelData travelData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryTravel " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_TRAVEL_SUCCESS)
			RequestMgr.Inst.Response(new QueryTravelRes(callback, travelData));
		else
			RequestMgr.Inst.Response(new QueryTravelRes(callback, result, PtrErrStr(result)));
	}

	public bool BuyTravel(int callback, int dungeonId, int goodId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyTravel ");
#endif
		return clHlp.buyTravel(callback, dungeonId, goodId);
	}

	public void OnBuyTravel(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, TravelData travelData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyTravel " + result);
#endif

		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_TRAVEL_SUCCESS)
			RequestMgr.Inst.Response(new BuyTravelRes(callback, costAndRewardAndSync, travelData));
		else
			RequestMgr.Inst.Response(new BuyTravelRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion Dungeon

	#region Email

	public bool QueryEmailListInfo(int callback, int emailType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryEmailListInfo_By_Type");
#endif
		return clHlp.queryEmailListInfo(callback, emailType);
	}

	private void OnQueryEmailListInfo(int callback, int result, long lastQueryTime, List<EmailPlayer> emailPlayers)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Email] OnQuerySingleEmailListInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_EMAIL_LIST_INFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryEmailListsRes(callback, lastQueryTime, emailPlayers));
		else
			RequestMgr.Inst.Response(new QueryEmailListsRes(callback, result, PtrErrStr(callback)));
	}

	public bool GetAttachments(int callback, long emailId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GetAttachments");
#endif
		return clHlp.getAttachments(callback, emailId);
	}

	private void OnGetAttachments(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Email] OnGetAttachmentsRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GET_ATTACHMENTS_SUCCESS)
			RequestMgr.Inst.Response(new GetAttachmetnsRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new GetAttachmetnsRes(callback, result, PtrErrStr(result)));
	}

	private void OnSyncNewEmailCount(int count, int emailType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Email] OnSyncNewEmailCountRes ");
#endif
		RequestMgr.Inst.Response(new SyncNewEmailCountRes(Request.InvalidID, count, emailType));
	}

	#endregion Email

	#region MergeIllustration

	public bool MergeIllustration(int callback, int id, int count)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] HandBook");
#endif
		return clHlp.MergeIllustration(callback, id, count);
	}

	private void OnMergeIllustration(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[HandBook] HandBook " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MERGE_ILLUSTRATION_SUCCESS)
			RequestMgr.Inst.Response(new MergeIllustrationRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new MergeIllustrationRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion MergeIllustration

	#region QueryIllustration

	public bool QueryIllustration(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryIllustration");
#endif
		return clHlp.QueryIllustration(callback);
	}

	private void OnQueryIllustration(int callback, int result, List<int> cardIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[HandBook] OnQueryIllustration " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ILLUSTRATION_SUCCESS)
			RequestMgr.Inst.Response(new QueryIllustrationRes(callback, cardIds));
		else
			RequestMgr.Inst.Response(new QueryIllustrationRes(callback, result, PtrErrStr(result)));
	}

	#endregion QueryIllustration

	#region Chat

	public bool QueryChatMessageList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] queryChatMessageList");
#endif
		return clHlp.queryChatMessageList(callback);
	}

	private void OnQueryChatMessageList(int callback, int result, List<com.kodgames.corgi.protocol.ChatMessage> chatMessages, int privateMessageCount, int worldMsgCount, int guildMsgCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnQueryChatMessageList " + result.ToString("X"));
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_CHATMESSAGE_SUCCESS)
		{
			RequestMgr.Inst.Response(new OnQueryChatMessageList(callback, chatMessages, privateMessageCount, worldMsgCount, guildMsgCount));
		}
		else
		{
			RequestMgr.Inst.Response(new OnQueryChatMessageList(callback, result, PtrErrStr(result)));
		}
	}

	public bool CloseChatMessageDialog(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] closeChatMessageDialog");
#endif
		return clHlp.closeChatMessageDialog(callback);
	}

	private void OnCloseChatMessageDialog(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnCloseChatMessageDialog " + result.ToString("X"));
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_CLOSECHATMESSAGEDIALOG_SUCCESS)
		{
			RequestMgr.Inst.Response(new CloseChatMessageDialogRes(callback));
		}
		else
		{
			RequestMgr.Inst.Response(new CloseChatMessageDialogRes(callback, result, PtrErrStr(result)));
		}
	}

	public bool Chat(int callback, com.kodgames.corgi.protocol.ChatMessage chatMessage)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Chat");
#endif
		return clHlp.chat(callback, chatMessage);
	}

	private void OnChat(int callback, int result, com.kodgames.corgi.protocol.ChatMessage chatMessage, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnChat " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_PRIVATE_SUCCESS
			|| result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_PRIVATE_SUCCESS_RECEIVER_NOT_ONLINE
			|| result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_WORLD_SUCCESS
			|| result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS
			|| result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHAT_SUCCESS_GUILD_SUCCESS)
			RequestMgr.Inst.Response(new ChatRes(callback, result, chatMessage, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ChatRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public void OnSyncWorldChatAndFlowMessages(List<com.kodgames.corgi.protocol.ChatMessage> messages)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnSyncWorldChatAndFlowMessagesRes ");
#endif
		RequestMgr.Inst.Response(new SyncWorldChatAndFlowMessagesRes(messages));
	}

	public bool QueryPlayerInfo(int callback, int playerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryPlayerInfo");
#endif
		return clHlp.queryPlayerMessage(callback, playerId);
	}

	private void OnQueryPlayerInfo(int callback, int result, KodGames.ClientClass.Player player)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Chat] OnQueryPlayerInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_PLAYER_MESSAGE_SUCCESS)
			RequestMgr.Inst.Response(new QueryPlayerInfoRes(callback, player));
		else
			RequestMgr.Inst.Response(new QueryPlayerInfoRes(callback, result, PtrErrStr(result)));
	}

	#endregion Chat

	#region Arena

	public bool QueryArenaRank(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryArenaRank");
#endif
		return clHlp.queryArenaRank(callback);
	}

	private void OnQueryArenaRank(int callback, int result, int challengeTimes, int arenaGradeId, long gradePoint, long lastResetChallengeTime, int selfRank, int speed, List<com.kodgames.corgi.protocol.PlayerRecord> playerRecords)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[OnQueryArenaRank] OnQueryArenaRank " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ARENA_RANK_SUCCESS)
			RequestMgr.Inst.Response(new QueryArenaRankRes(callback, challengeTimes, arenaGradeId, gradePoint, lastResetChallengeTime, selfRank, speed, playerRecords));
		else
			RequestMgr.Inst.Response(new QueryArenaRankRes(callback, result, PtrErrStr(result)));
	}

	public bool ArenaCombat(int callback, int rank, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ArenaCombat");
#endif
		return clHlp.arenaCombat(callback, rank, position);
	}

	private void OnArenaCombat(int callback, int result, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, int rank, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[OnArenaCombat] OnArenaCombat " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ARENA_COMBAT_SUCCESS)
			RequestMgr.Inst.Response(new ArenaCombatRes(callback, combatResultAndReward, rank, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ArenaCombatRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool QueryGradePoint(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGradePoint");
#endif
		return clHlp.queryGradePoint(callback);
	}

	private void OnQueryGradePoint(int callback, int result, long gradePoint)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGradePoint " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GRADE_POINT_SUCCESS)
			RequestMgr.Inst.Response(new QueryHonorPointRes(callback, gradePoint));
		else
			RequestMgr.Inst.Response(new QueryHonorPointRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryArenaPlayerInfo(int callback, int rank, int arenaGradeId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryArenaPlayerInfo");
#endif
		return clHlp.queryArenaPlayerInfo(callback, rank, arenaGradeId);
	}

	private void OnQueryArenaPlayerInfo(int callback, int result, Player player)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryArenaPlayerInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ARENA_PLAYERINIFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryArenaPlayerInfoRes(callback, player));
		else
			RequestMgr.Inst.Response(new QueryArenaPlayerInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryRankToFew(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryRankToFew");
#endif
		return clHlp.queryRankTopFew(callback);
	}

	private void OnQueryRankToFew(int callback, int result, List<com.kodgames.corgi.protocol.PlayerRecord> topFew)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryRankToFew " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_RANK_TOPFEW_SUCCESS)
			RequestMgr.Inst.Response(new QueryRankToFewRes(callback, topFew));
		else
			RequestMgr.Inst.Response(new QueryRankToFewRes(callback, result, PtrErrStr(result)));
	}

	#endregion Arena

	#region Auction

	public bool BuyConsignment(int callback, long consignmentIdx, List<KodGames.ClientClass.Cost> costs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyConsignment");
#endif
		return true;
	}

	public bool Consignment(int callback, string guid, List<KodGames.ClientClass.Cost> costs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Consignment");
#endif
		return true;
	}

	public bool Downshelf(int callback, long consignmentIdx)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Downshelf");
#endif
		return true;
	}

	public bool QueryConsignmentList(int callback, int type, int subType, int isPrevNotNext, long consignIdx)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryConsignmentList");
#endif
		return true;
	}

	public bool QuerySelfConsignment(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QuerySelfConsignment");
#endif
		return true;
	}

	public bool QueryAvatarList(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryAvatarList");
#endif
		return true;
	}

	#endregion Auction

	#region Setting

	public bool ExchangeCode(int callback, string strRewardKey)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ExchangeCodeReward");
#endif
		return clHlp.exchangeCode(callback, strRewardKey);
	}

	private void OnExchangeCode(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, string strConver, string strGetway)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Setting] OnExchangeCodeReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EXCHANGECODE_SUCCESS)
			RequestMgr.Inst.Response(new OnExchangeCodeRes(callback, costAndRewardAndSync, strConver, strGetway));
		else
			RequestMgr.Inst.Response(new OnExchangeCodeRes(callback, result, PtrErrStr(result), costAndRewardAndSync, strConver, strGetway));
	}

	public bool SettingFeedback(int callback, int type, string strInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SettingFeedback");
#endif
		return clHlp.SettingFeedback(callback, type, strInfo);
	}

	private void OnSettingFeedback(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Setting] OnSettingExchangeReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SETTING_FEEDBACK_SUCCESS)
			RequestMgr.Inst.Response(new SettingFeedbackRes(callback));
		else
			RequestMgr.Inst.Response(new SettingFeedbackRes(callback, result, PtrErrStr(result)));
	}

	#endregion Setting

	#region Tavern

	public bool TavernQuery(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] TavernQuery");
#endif
		return clHlp.TavernQuery(callback);
	}

	private void OnTavernQuery(int callback, int result, List<com.kodgames.corgi.protocol.TavernInfo> tavernInfos, List<int> mysteryerResourceIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Tavern] OnTavernQuery " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_TAVERN_QUERY_SUCCESS)
		{
			RequestMgr.Inst.Response(new QueryTavernInfoRes(callback, tavernInfos, mysteryerResourceIds));
		}
		else
		{
			RequestMgr.Inst.Response(new QueryTavernInfoRes(callback, result, PtrErrStr(result)));
		}
	}

	public bool TavernBuy(int callback, int tavernId, int tavernType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] TavernBuy");
#endif
		return clHlp.TavernBuy(callback, tavernId, tavernType);
	}

	private void OnTavernBuy(int callback, int result, com.kodgames.corgi.protocol.TavernInfo tavernInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[Tavern] OnTavernBuy " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_TAVERN_BUY_SUCCESS)
		{
			RequestMgr.Inst.Response(new TavernBuyRes(callback, tavernInfo, costAndRewardAndSync));
		}
		else
		{
			RequestMgr.Inst.Response(new TavernBuyRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
		}
	}

	#endregion Tavern

	#region ActivityExchange

	public bool QueryExchangeList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryExchangeList");
#endif
		return clHlp.queryExchangeList(callback);
	}

	private void OnQueryExchangeList(int callback, int result, List<Exchange> exchanges)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[OnQueryExchangeList] OnQueryExchangeList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_EXCHANGE_LIST_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryExchangeListRes(callback, exchanges));
		else
			RequestMgr.Inst.Response(new OnQueryExchangeListRes(callback, result, PtrErrStr(result)));
	}

	public bool ExchangeReq(int callback, int exchangeId, List<KodGames.ClientClass.Cost> costs, int groupId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ExchangeReq");
#endif
		return clHlp.exchange(callback, exchangeId, costs, groupId);
	}

	private void OnExchangeRes(int callback, int result, int exchangeId, long nextOpenTime, KodGames.ClientClass.CostAndRewardAndSync costAReward)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[OnExchangeRes] OnExchangeRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EXCHANGE_SUCCESS)
			RequestMgr.Inst.Response(new OnExchangeRes(callback, exchangeId, nextOpenTime, costAReward));
		else
			RequestMgr.Inst.Response(new OnExchangeRes(callback, result, PtrErrStr(result)));
	}

	#endregion ActivityExchange

	#region Tutorial

	// CreateAvatar and Set Player name. Request
	public bool GetTutorialAvatarAndSetPlayerName(int callback, int resourceId, string playerName, int tutorialId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[GetTutorialAvatarAndSetPlayerName] GetTutorialAvatarAndSetPlayerName");
#endif
		return clHlp.getTutorialAvatarAndSetPlayerName(callback, resourceId, playerName, tutorialId);
	}

	// CreateAvatar and Set Player name. Response
	public void OnGetTutorialAvatarAndSetPlayerName(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, PositionData positonData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGetTutorialAvatarAndSetPlayerName " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GET_TUTORIAL_AVATAR_AND_SET_PLAYERNAME_SUCCESS)
			RequestMgr.Inst.Response(new GetTutorialAvatarAndSetPlayerNameRes(callback, positonData, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new GetTutorialAvatarAndSetPlayerNameRes(callback, result, PtrErrStr(result)));
	}

	// Set Tutorial State. Request
	public bool CompleteTutorial(int callback, int tutorialId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[CompleteTutorial] CompleteTutorial");
#endif
		return clHlp.completeTutorial(callback, tutorialId);
	}

	// Set Tutorial State. Request
	public void OnCompleteTutorial(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnCompleteTutorial " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_COMPLETE_TUTORIAL_SUCCESS)
			RequestMgr.Inst.Response(new GenericResponse(callback));
		else
			RequestMgr.Inst.Response(new GenericResponse(callback, result, PtrErrStr(result)));
	}

	// Get Tutorial Combat request.
	public bool NoviceCombat(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[NoviceCombat] NoviceCombat");
#endif
		return clHlp.noviceCombat(callback);
	}

	public void OnNoviceCombat(int callback, int result, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnNoviceCombat " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_NOVICECOMBAT_SUCCESS)
			RequestMgr.Inst.Response(new NoviceCombatRes(callback, combatResultAndReward, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new NoviceCombatRes(callback, result, PtrErrStr(result)));
	}

	// Fetch Player Name.
	public bool FetchRandomPlayerNames(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[FetchRandomPlayerNames] FetchRandomPlayerNames");
#endif
		return clHlp.fetchRandomPlayerNames(callback);
	}

	public void OnFetchRandomPlayerNames(int callback, int result, List<string> playerNames)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnFetchRandomPlayerNames " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_FETCH_RANDOM_NAMES_SUCCESS)
			RequestMgr.Inst.Response(new FetchRandomPlayerNameRes(callback, playerNames));
		else
			RequestMgr.Inst.Response(new FetchRandomPlayerNameRes(callback, result, PtrErrStr(result)));
	}

	#endregion Tutorial

	#region Quest : DialyGuid

	// QueryQuestInfo : Request
	public bool QueryQuestInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[QueryQuestInfo] QueryQuestInfo");
#endif
		return clHlp.QueryQuestInfo(callback);
	}

	// QueryQuestInfo : Response
	public void OnQueryQuestInfo(int callback, int result, List<KodGames.ClientClass.Quest> quests, KodGames.ClientClass.QuestQuick questQuick)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryQuestInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_QUESTINFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryQuestInfoRes(callback, quests, questQuick));
		else
			RequestMgr.Inst.Response(new QueryQuestInfoRes(callback, result, PtrErrStr(result)));
	}

	// GetQuestReward : Request.
	public bool PickQuestReward(int callback, int questId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[PickQuestReward] PickQuestReward");
#endif
		return clHlp.PickQuestReward(callback, questId);
	}

	// GetQuestReward : Response.
	public void OnPickQuestReward(int callback, int result, int questId, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, List<KodGames.ClientClass.Quest> changedQuests, KodGames.ClientClass.QuestQuick questQuick)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnPickQuestReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_PICK_QUEST_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new PickQuestRewardRes(callback, questId, costAndRewardAndSync, changedQuests, questQuick));
		else
			RequestMgr.Inst.Response(new PickQuestRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion Quest : DialyGuid

	#region StarServereReward

	// PickStartServerRewardReq : Request
	public bool PickStartServerReward(int callback, int pickId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PickStartServerReward");
#endif
		return clHlp.pickStartServerReward(callback, pickId);
	}

	// PickStartServerRewardRes : Response
	public void OnPickStartServerReward(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnPickStartServerReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_PICK_START_SERVER_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new PickStartServerRewardRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new PickStartServerRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion StarServereReward

	#region MysteryShop

	public bool QueryMysteryShopInfo(int callback, int shopType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryMysteryShopInfo");
#endif
		return clHlp.queryMysteryShop(callback, shopType);
	}

	public void OnQueryMysteryShopInfoRes(int callback, int result, int shopType, KodGames.ClientClass.MysteryShopInfo mysteryShopInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMysteryShopInfoRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MYSTERY_SHOP_QUERY_SUCCESS)
			RequestMgr.Inst.Response(new QueryMysteryShopRes(callback, mysteryShopInfo));
		else
			RequestMgr.Inst.Response(new QueryMysteryShopRes(callback, result, PtrErrStr(result)));
	}

	public bool ChangeMysteryShopInfo(int callback, int shopType, int refreshId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ChangeMysteryShopInfo");
#endif
		return clHlp.refreshMysteryShop(callback, shopType, refreshId);
	}

	public void OnChangeMysteryShopRes(int callback, int result, int shopType, KodGames.ClientClass.MysteryShopInfo mysteryShopInfo, CostAndRewardAndSync constAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnChangeMysteryShopRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MYSTERY_SHOP_REFRESH_SUCCESS)
			RequestMgr.Inst.Response(new ChangeMysteryShopRes(callback, mysteryShopInfo, constAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ChangeMysteryShopRes(callback, result, PtrErrStr(result), constAndRewardAndSync));
	}

	public bool BuyMysteryGoods(int callback, int shopType, int goodsId, int goodsIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyMysteryGood");
#endif
		return clHlp.buyMysteryShop(callback, shopType, goodsId, goodsIndex);
	}

	public void OnMysteryShopBuy(int callback, int result, int shopType, MysteryGoodInfo goodsInfo, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyMysteryGoodRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MYSTERY_SHOP_BUY_SUCCESS)
			RequestMgr.Inst.Response(new BuyMysteryGoodRes(callback, goodsInfo, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new BuyMysteryGoodRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	#endregion MysteryShop

	#region AvatarScrollMix

	public bool ChangeDomineer(int callback, string avatarGuid, List<string> destroyAvatarGuids)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ChangeDomineer");
#endif
		return clHlp.ChangeDomineer(callback, avatarGuid, destroyAvatarGuids);
	}

	public void OnChangeDomineer(int callback, int result, KodGames.ClientClass.Avatar changedAvatar, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnChangeDomineer " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_CHANGE_DOMINEER_SUCCESS)
			RequestMgr.Inst.Response(new ChangeDomineerRes(callback, changedAvatar, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new ChangeDomineerRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool SaveDomineer(int callback, string avatarGuid, bool isSave)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SaveDomineer");
#endif
		return clHlp.SaveDomineer(callback, avatarGuid, isSave);
	}

	public void OnSaveDomineer(int callback, int result, KodGames.ClientClass.Avatar changedAvatar)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSaveDomineer " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SAVE_DOMINEER_SUCCESS)
			RequestMgr.Inst.Response(new SaveDomineerRes(callback, changedAvatar));
		else
			RequestMgr.Inst.Response(new SaveDomineerRes(callback, result, PtrErrStr(result)));
	}

	#endregion AvatarScrollMix

	#region Assistant

	public bool QueryTaskList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryTaskList ");
#endif
		return clHlp.queryTaskList(callback);
	}

	public void OnQueryTaskList(int callback, int result, List<com.kodgames.corgi.protocol.Task> tasks)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryTaskList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_TASK_LIST_SUCCESS)
			RequestMgr.Inst.Response(new QueryTaskListRes(callback, tasks));
		else
			RequestMgr.Inst.Response(new QueryTaskListRes(callback, result, PtrErrStr(result)));
	}

	public bool TaskCondition(int callback, int gotoUI)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] TaskCondition ");
#endif
		return clHlp.taskCondition(callback, gotoUI);
	}

	public void OnTaskCondition(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] TaskCondition " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_TASK_CONDITION_SUCCESS)
			RequestMgr.Inst.Response(new TaskConditionRes(callback));
		else
			RequestMgr.Inst.Response(new TaskConditionRes(callback, result, PtrErrStr(result)));
	}

	#endregion Assistant

	#region Tower

	public bool QueryMelaleucaFloorPlayerInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryMelaleucaFloorPlayerInfo ");
#endif
		return clHlp.queryMelaleucaFloorPlayerInfo(callback);
	}

	public void OnQueryMelaleucaFloorPlayerInfo(int callback, int result, KodGames.ClientClass.MelaleucaFloorData mfInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMelaleucaFloorPlayerInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_MELALEUCA_FLOOR_PLAYER_INFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryMelaleucaFloorPlayerInfoRes(callback, mfInfo));
		else
			RequestMgr.Inst.Response(new QueryMelaleucaFloorPlayerInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryMelaleucaFloorInfo(int callback, int layers)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryMelaleucaFloorInfo ");
#endif
		return clHlp.queryMelaleucaFloorInfo(callback, layers);
	}

	public void OnQueryMelaleucaFloorInfo(int callback, int result, List<com.kodgames.corgi.protocol.NpcInfo> npcInfos, List<KodGames.ClientClass.Reward> passRewards, List<KodGames.ClientClass.Reward> firstPassRewards)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMelaleucaFloorInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_MELALEUCA_FLOOR_INFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryMelaleucaFloorInfoRes(callback, npcInfos, passRewards, firstPassRewards));
		else
			RequestMgr.Inst.Response(new QueryMelaleucaFloorInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool MelaleucaFloorCombat(int callback, int layers, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MelaleucaFloorCombat ");
#endif
		return clHlp.melaleucaFloorCombat(callback, layers, position);
	}

	public void OnMelaleucaFloorCombat(int callback, int result, com.kodgames.corgi.protocol.MelaleucaFloorInfo mfInfo, KodGames.ClientClass.CostAndRewardAndSync perDayCrs, List<KodGames.ClientClass.CostAndRewardAndSync> passCrs, List<KodGames.ClientClass.CostAndRewardAndSync> firstPassCrs, KodGames.ClientClass.CombatResultAndReward combatResult, List<int> firstPassLayers, int layers)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMelaleucaFloorCombat " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MELALEUCA_FLOOR_COMBAT_SUCCESS)
			RequestMgr.Inst.Response(new OnMelaleucaFloorCombatRes(callback, mfInfo, perDayCrs, passCrs, firstPassCrs, combatResult, firstPassLayers, layers));
		else
			RequestMgr.Inst.Response(new OnMelaleucaFloorCombatRes(callback, result, PtrErrStr(result)));
	}

	public bool MelaleucaFloorConsequentCombat(int callback, int layers, int combatCount, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MelaleucaFloorConsequentCombat ");
#endif
		return clHlp.melaleucaFloorConsequentCombat(callback, layers, combatCount, position);
	}

	public void OnMelaleucaFloorConsequentCombat(int callback, int result, com.kodgames.corgi.protocol.MelaleucaFloorInfo mfInfo, KodGames.ClientClass.CostAndRewardAndSync perDayCrs, List<KodGames.ClientClass.CostAndRewardAndSync> passCrs, List<KodGames.ClientClass.CostAndRewardAndSync> firstPassCrs, KodGames.ClientClass.CombatResultAndReward combatResult, List<int> firstPassLayers, int layers, int combatCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMelaleucaFloorConsequentCombat " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MELALEUCA_FLOOR_CONSEQUENT_COMBAT_SUCCESS)
			RequestMgr.Inst.Response(new MelaleucaFloorConsequentCombatRes(callback, mfInfo, perDayCrs, passCrs, firstPassCrs, combatResult, firstPassLayers, layers, combatCount));
		else
			RequestMgr.Inst.Response(new MelaleucaFloorConsequentCombatRes(callback, result, PtrErrStr(result)));
	}

	public bool MelaleucaFloorThisWeekRank(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MelaleucaFloorThisWeekRank ");
#endif
		return clHlp.queryMelaleucaFloorThisWeekRank(callback);
	}

	public void OnQueryMelaleucaFloorThisWeekRank(int callback, int result, int layer, int point, int maxPointWeek, int predictRank, List<com.kodgames.corgi.protocol.MfRankInfo> rankInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMelaleucaFloorThisWeekRank " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MELALEUCA_FLOOR_THIS_WEEK_RANK_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryMelaleucaFloorThisWeekRankRes(callback, layer, point, maxPointWeek, predictRank, rankInfos));
		else
			RequestMgr.Inst.Response(new OnQueryMelaleucaFloorThisWeekRankRes(callback, result, PtrErrStr(result)));
	}

	public bool MelaleucaFloorLastWeekRank(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MelaleucaFloorLastWeekRank ");
#endif
		return clHlp.queryMelaleucaFloorLastWeekRank(callback);
	}

	public void OnQueryMelaleucaFloorLastWeekRank(int callback, int result, int rank, int point, int layer, List<com.kodgames.corgi.protocol.MfRankInfo> rankInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMelaleucaFloorLastWeekRank " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MELALEUCA_FLOOR_LAST_WEEK_RANK_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryMelaleucaFloorLastWeekRankRes(callback, rank, point, layer, rankInfos));
		else
			RequestMgr.Inst.Response(new OnQueryMelaleucaFloorLastWeekRankRes(callback, result, PtrErrStr(result)));
	}

	public bool MelaleucaFloorWeekRewardInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MelaleucaFloorWeekRewardInfo ");
#endif
		return clHlp.queryMelaleucaFloorWeekRewardInfo(callback);
	}

	public void OnQueryMelaleucaFloorWeekRewardInfo(int callback, int result, int weekRank, bool isGetWeekReward)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMelaleucaFloorWeekRewardInfo" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_MELALEUCA_FLOOR_WEEK_REWARD_INFO_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryMelaleucaFloorWeekRewardInfoRes(callback, weekRank, isGetWeekReward));
		else
			RequestMgr.Inst.Response(new OnQueryMelaleucaFloorWeekRewardInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool MelaleucaFloorGetReward(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MelaleucaFloorGetReward ");
#endif
		return clHlp.getMelaleucaFloorWeekReward(callback);
	}

	public void OnGetMelaleucaFloorWeekReward(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, bool isGetWeekReward)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGetMelaleucaFloorWeekReward" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MELALEUCA_FLOOR_GET_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new OnGetMelaleucaFloorWeekRewardRes(callback, costAndRewardAndSync, isGetWeekReward));
		else
			RequestMgr.Inst.Response(new OnGetMelaleucaFloorWeekRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion Tower

	#region WolfSmoke

	public bool QueryBuyWolfSmokeShop(int callback, int goodsId, int goodsIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryBuyWolfSmokeShop ");
#endif
		return clHlp.buyWolfSmokeShop(callback, goodsId, goodsIndex);
	}

	public void OnBuyWolfSmokeShop(int callback, int result, bool isJoin, WolfSmokeGoodsInfo goodsInfo, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyWolfSmokeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_WOLF_SMOKE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new BuyWolfSmokeShopRes(callback, isJoin, goodsInfo, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new BuyWolfSmokeShopRes(callback, result, costAndRewardAndSync, PtrErrStr(result)));
	}

	public bool QueryCombatWolfSmoke(int callback, int additionId, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryCombatWolfSmoke ");
#endif
		return clHlp.combatWolfSmoke(callback, additionId, position);
	}

	public void OnCombatWolfSmoke(int callback, int result, bool isJoin, CombatResultAndReward combatResultAndReward, CostAndRewardAndSync costAndRewardAndSync, int alreadyFailedTimes, int stageId, List<WolfEggs> wolfEggs, CostAndRewardAndSync eggsCostAndRewardAndSync, com.kodgames.corgi.protocol.Avatar showAvatar, int alreadyResetTimes)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnCombatWolfSmoke " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_COMBAT_WOLF_SMOKE_SUCCESS)
			RequestMgr.Inst.Response(new OnCombatWolfSmokeRes(callback, isJoin, combatResultAndReward, costAndRewardAndSync, alreadyFailedTimes, stageId, wolfEggs, eggsCostAndRewardAndSync, showAvatar, alreadyResetTimes));
		else
			RequestMgr.Inst.Response(new OnCombatWolfSmokeRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryJoinWolfSmoke(int callback, int positionId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryJoinWolfSmoke ");
#endif
		return clHlp.joinWolfSmoke(callback, positionId);
	}

	public void OnJoinWolfSmoke(int callback, int result, bool isJoin)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnJoinWolfSmoke " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_JOIN_WOLF_SMOKE_SUCCESS)
			RequestMgr.Inst.Response(new QueryJoinWolfSmokeRes(callback, isJoin));
		else
			RequestMgr.Inst.Response(new QueryJoinWolfSmokeRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryWolfSmoke(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryWolfSmoke ");
#endif
		return clHlp.queryWolfSmoke(callback);
	}

	public void OnQueryWolfSmoke(int callback, int result, bool isJoin, WolfInfo wolfInfo, Player wolfPlayer, List<Location> locations, Player enemyPlayer, List<WolfSmokeAddition> wolfSmokeAdditions, List<WolfSelectedAddition> wolfSelectedAdditions, List<WolfAvatar> wolfAvatars, int lastPositionId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryWolfSmoke " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_WOLF_SMOKE_SUCCESS)
			RequestMgr.Inst.Response(new QueryWolfSmokeRes(callback, isJoin, wolfInfo, wolfPlayer, locations, enemyPlayer, wolfSmokeAdditions, wolfSelectedAdditions, wolfAvatars, lastPositionId));
		else
			RequestMgr.Inst.Response(new QueryWolfSmokeRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryWolfSmokeEnemy(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryWolfSmokeEnemy ");
#endif
		return clHlp.queryWolfSmokeEnemy(callback);
	}

	public void OnQueryWolfSmokeEnemy(int callback, int result, bool isJoin, WolfSmokePlayer wolfsmokeplayer)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryWolfSmokeEnemy " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_WOLF_SMOKE_ENEMY_SUCCESS)
			RequestMgr.Inst.Response(new QueryWolfSmokeEnemyRes(callback, isJoin, wolfsmokeplayer));
		else
			RequestMgr.Inst.Response(new QueryWolfSmokeEnemyRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryWolfSmokePosition(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryWolfSmokePosition ");
#endif
		return clHlp.queryWolfSmokePosition(callback);
	}

	public void OnQueryWolfSmokePosition(int callback, int result, bool isJoin, WolfSmokePlayer WolfSmokePlayer, List<WolfSmokeAddition> wolfSmokeAdditions, List<WolfSelectedAddition> wolfSelectedAdditions)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryWolfSmokePosition " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_WOLF_SMOKE_POSITION_SUCCESS)
			RequestMgr.Inst.Response(new QueryWolfSmokePositionRes(callback, isJoin, WolfSmokePlayer, wolfSmokeAdditions, wolfSelectedAdditions));
		else
			RequestMgr.Inst.Response(new QueryWolfSmokePositionRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryWolfSmokeShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryWolfSmokeShop ");
#endif
		return clHlp.queryWolfSmokeShop(callback);
	}

	public void OnQueryWolfSmokeShop(int callback, int result, bool isJoin, List<WolfSmokeGoodsInfo> goodsInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryWolfSmokeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_WOLF_SMOKE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new QueryWolfSmokeShopRes(callback, isJoin, goodsInfos));
		else
			RequestMgr.Inst.Response(new QueryWolfSmokeShopRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryRefreshWolfSmokeShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryRefreshWolfSmokeShop ");
#endif
		return clHlp.refreshWolfSmokeShop(callback);
	}

	public void OnRefreshWolfSmokeShop(int callback, int result, bool isJoin, List<WolfSmokeGoodsInfo> goodsInfos, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshWolfSmokeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_WOLF_SMOKE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new QueryRefreshWolfSmokeShopRes(callback, isJoin, goodsInfos, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new QueryRefreshWolfSmokeShopRes(callback, result, costAndRewardAndSync, PtrErrStr(result)));
	}

	public bool QueryResetWolfSmoke(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryResetWolfSmoke ");
#endif
		return clHlp.resetWolfSmoke(callback);
	}

	public void OnResetWolfSmoke(int callback, int result, bool isJoin, WolfInfo wolfInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnResetWolfSmoke " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RESET_WOLF_SMOKE_SUCCESS)
			RequestMgr.Inst.Response(new QueryResetWolfSmokeRes(callback, isJoin, wolfInfo));
		else
			RequestMgr.Inst.Response(new QueryResetWolfSmokeRes(callback, result, PtrErrStr(result)));
	}

	#endregion WolfSmoke

	#region Friend

	public bool QueryFriendList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFriendList ");
#endif
		return clHlp.QueryFriendList(callback);
	}

	public void OnQueryFriendList(int callback, int result, List<FriendInfo> friendInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFriendList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FRIEND_LIST_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryFriendListRes(callback, friendInfos));
		else
			RequestMgr.Inst.Response(new OnQueryFriendListRes(callback, result, PtrErrStr(result)));
	}

	public bool RandomFriend(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RandomFriend ");
#endif
		return clHlp.RandomFriend(callback);
	}

	public void OnRandomFriend(int callback, int result, List<FriendInfo> friendInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRandomFriend " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RANDOM_FRIEND_SUCCESS)
			RequestMgr.Inst.Response(new OnRandomFriendRes(callback, friendInfos));
		else
			RequestMgr.Inst.Response(new OnRandomFriendRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryPlayerName(int callback, string name)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryPlayerName ");
#endif
		return clHlp.QueryPlayerName(callback, name);
	}

	public void OnQueryPlayerName(int callback, int result, List<FriendInfo> friendInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryPlayerName " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_PLAYER_NAME_SUCCESS)
			RequestMgr.Inst.Response(new OnRandomFriendRes(callback, friendInfos));
		else
			RequestMgr.Inst.Response(new OnRandomFriendRes(callback, result, PtrErrStr(result)));
	}

	public bool InviteFriend(int callback, int playerId, string name)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] InviteFriend ");
#endif
		return clHlp.InviteFriend(callback, playerId, name);
	}

	public void OnInviteFriend(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnInviteFriend " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_INVITE_FRIEND_SUCCESS)
			RequestMgr.Inst.Response(new OnInviteFriendRes(callback));
		else
			RequestMgr.Inst.Response(new OnInviteFriendRes(callback, result, PtrErrStr(result)));
	}

	public bool AnswerFriend(int callback, int playerId, long emailId, bool agree)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] AnswerFriend ");
#endif
		return clHlp.AnswerFriend(callback, playerId, emailId, agree);
	}
	public void OnAnswerFriend(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAnswerFriend " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ANSWER_FRIEND_SUCCESS)
			RequestMgr.Inst.Response(new OnAnswerFriendRes(callback, true));
		else if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ANSWER_FRIEND_SUCCESS_REFUSE)
			RequestMgr.Inst.Response(new OnAnswerFriendRes(callback, false));
		else if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ANSWER_FRIEND_ALREADY_BE_FRIEND_FAILED)
			RequestMgr.Inst.Response(new OnAnswerFriendRes(callback, true, PtrErrStr(result)));
		else
			RequestMgr.Inst.Response(new OnAnswerFriendRes(callback, result, PtrErrStr(result)));
	}

	public bool QueryFriendPlayerInfo(int callback, int playerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFriendPlayerInfo ");
#endif
		return clHlp.QueryFriendPlayerInfo(callback, playerId);
	}

	public void OnQueryFriendPlayerInfo(int callback, int result, Player player)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFriendPlayerInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FRIEND_PLAYER_INFO_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryFriendPlayerInfoRes(callback, player));
		else
			RequestMgr.Inst.Response(new OnQueryFriendPlayerInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool RemoveFriend(int callback, int playerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RemoveFriend ");
#endif
		return clHlp.RemoveFriend(callback, playerId);
	}

	public void OnRemoveFriend(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRemoveFriend " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REMOVE_FRIEND_SUCCESS)
			RequestMgr.Inst.Response(new OnRemoveFriendRes(callback));
		else
			RequestMgr.Inst.Response(new OnRemoveFriendRes(callback, result, PtrErrStr(result)));
	}

	public bool CombatFriend(int callback, int friendId, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] CombatFriend ");
#endif
		return clHlp.CombatFriend(callback, friendId, position);
	}

	public void OnCombatFriend(int callback, int result, CombatResultAndReward combatResultAndReward, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnCombatFriend " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_COMBAT_FRIEND_SUCCESS)
			RequestMgr.Inst.Response(new OnCombatFriendRes(callback, combatResultAndReward, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnCombatFriendRes(callback, result, PtrErrStr(result)));
	}

	public void OnDelFriend(int delPlayerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnDelFriend ");
#endif
		RequestMgr.Inst.Response(new OnDelFriendRes(delPlayerId));
	}

	public void OnAddFriend(FriendInfo friendInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAddFriend ");
#endif
		RequestMgr.Inst.Response(new OnAddFriendRes(friendInfo));
	}

	#endregion Friend

	#region QinInfo

	public bool QueryQinInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryQinInfo ");
#endif
		return clHlp.queryQinInfo(callback);
	}

	public void OnQueryQinInfo(int callback, int result, QinInfo qinInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryQinInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_QIN_INFO_SUCCESS)
			RequestMgr.Inst.Response(new QueryQinInfoRes(callback, qinInfo));
		else
			RequestMgr.Inst.Response(new QueryQinInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool AnswerQinInfo(int callback, int questionId, int answerNum)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] AnswerQinInfo ");
#endif
		return clHlp.answerQinInfo(callback, questionId, answerNum);
	}

	public void OnAnswerQinInfo(int callback, int result, bool rightFalse, CostAndRewardAndSync costAndRewardAndSync, QinInfo qinInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAnswerQinInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ANSWER_QIN_INFO_SUCCESS)
			RequestMgr.Inst.Response(new AnswerQinInfoRes(callback, rightFalse, costAndRewardAndSync, qinInfo));
		else
			RequestMgr.Inst.Response(new AnswerQinInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool GetQinInfoContinueReward(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GetQinInfoContinueReward ");
#endif
		return clHlp.getQinInfoContinueReward(callback);
	}

	public void OnGetQinInfoContinueReward(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward fixReward, KodGames.ClientClass.Reward randomReward, QinInfo qinInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGetQinInfoContinueReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GET_QIN_INFO_CONTINUE_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new GetQinInfoContinueRewardRes(callback, costAndRewardAndSync, fixReward, randomReward, qinInfo));
		else
			RequestMgr.Inst.Response(new GetQinInfoContinueRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion QinInfo

	#region MonthCard

	public bool QueryMonthCardInfo(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryMonthCardInfo ");
#endif
		return clHlp.monthCardQueryReq(callback);
	}

	public void OnQueryMonthCardInfo(int callback, int result, List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos, long lastResetTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMonthCardInfo " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MONTHCARD_QUERY_SUCCESS)
			RequestMgr.Inst.Response(new QueryMonthCardInfoRes(callback, monthCardInfos, lastResetTime));
		else
			RequestMgr.Inst.Response(new QueryMonthCardInfoRes(callback, result, PtrErrStr(result)));
	}

	public bool MonthCardPickReward(int callback, int monthCardId, int rewardType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] MonthCardPickReward");
#endif
		return clHlp.monthCardPickRewardReq(callback, monthCardId, rewardType);
	}

	public void OnMonthCardPickReward(int callback, int result, int type, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward fixReward, KodGames.ClientClass.Reward randomReward, List<com.kodgames.corgi.protocol.OneMonthCardInfo> monthCardInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMonthCardPickReward" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MONTHCARD_PICK_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new MonthCardPickRewardRes(callback, type, costAndRewardAndSync, fixReward, randomReward, monthCardInfos));
		else
			RequestMgr.Inst.Response(new MonthCardPickRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion MonthCard

	#region 小助手等推送协议

	public void OnQueryNotify(int callback, int result, int assisantNum, List<com.kodgames.corgi.protocol.ActivityData> activityData, List<com.kodgames.corgi.protocol.State> functionStates, List<KodGames.ClientClass.Quest> changedQuests, KodGames.ClientClass.QuestQuick questQuick)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryNotify " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_NOTIFY_SUCCESS)
			RequestMgr.Inst.Response(new QueryNotifyRes(callback, assisantNum, activityData, functionStates, changedQuests, questQuick));
	}

	#endregion

	#region GiveMeFive

	public bool GiveFiveStarsEvaluate(int callback, bool isEvaluate)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GiveFiveStarsEvaluate");
#endif
		return clHlp.giveFiveStarsEvaluate(callback, isEvaluate);
	}

	private void OnGiveFiveStarsEvaluate(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGiveFiveStarsEvaluate " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GIVE_FIVE_STARS_EVALUATE_SUCCESS)
			RequestMgr.Inst.Response(new GiveFiveStarsEvaluateRes(callback));
		else
			RequestMgr.Inst.Response(new GiveFiveStarsEvaluateRes(callback, result, PtrErrStr(result)));
	}

	#endregion GiveMeFive

	#region Runactivity

	//主查询发送协议
	public bool OperationActivityQueryReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OperationActivityQueryReq");
#endif
		return clHlp.operationActivityQuery(callback);
	}

	//主查询接收协议
	private void OnOperationActivityQueryRes(int callback, int result, int activityId, int index, int money, List<com.kodgames.corgi.protocol.OperationActivityItem> operationActivityItems, long rechargeStart, long rechargeEnd, long rewardStart, long rewardEnd)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnOperationActivityQueryRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_OPERATION_ACTIVITY_QUERY_SUCCESS)
			RequestMgr.Inst.Response(new OperationActivityQueryRes(callback, activityId, index, money, operationActivityItems, rechargeStart, rechargeEnd, rewardStart, rewardEnd));
		else
			RequestMgr.Inst.Response(new OperationActivityQueryRes(callback, result, PtrErrStr(result)));
	}

	//领取奖励发送协议
	public bool OperationActivityPickRewardReq(int callback, int operationId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OperationActivityPickRewardReq");
#endif
		return clHlp.operationActivityPickReward(callback, operationId);
	}

	//领取奖励接收协议
	private void OnOperationActivityPickRewardRes(int callback, int result, com.kodgames.corgi.protocol.OperationActivityItem operationActivityItem, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnOperationActivityPickRewardRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_OPERATION_ACTIVITY_PICK_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new OnOperationActivityPickRewardRes(callback, operationActivityItem, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnOperationActivityPickRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion Runactivity

	#region FriendCombatSystem

	//好友战斗系统参战协议
	public bool JoinFriendCampaignReq(int callback, int positionId, List<int> friendIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnJoinFriendCampaignReq");
#endif
		return clHlp.JoinFriendCampaign(callback, positionId, friendIds);
	}

	private void OnJoinFriendCampaignRes(int callback, int result, string deletedFriendName)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnJoinFriendCampaignRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_JOIN_FRIEND_CAMPAIGN_SUCCESS)
			RequestMgr.Inst.Response(new OnJoinFriendCampaignRes(callback, string.Empty));
		else
		{
			//判断是否是因为好友呗删除掉的情况
			if (deletedFriendName.Equals(string.Empty))
				RequestMgr.Inst.Response(new OnJoinFriendCampaignRes(callback, result, PtrErrStr(result)));
			else
				RequestMgr.Inst.Response(new OnJoinFriendCampaignRes(callback, deletedFriendName));
		}
	}

	//好友战斗系统重置协议
	public bool ResetFriendCampaignReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ResetFriendCampaignReq");
#endif
		return clHlp.ResetFriendCampaign(callback);
	}

	private void OnResetFriendCampaignRes(int callback, int result, bool isJoin)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnResetFriendCampaignRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_RESET_FRIEND_CAMPAIGN_SUCCESS)
			RequestMgr.Inst.Response(new OnResetFriendCampaignRes(callback, isJoin));
		else
			RequestMgr.Inst.Response(new OnResetFriendCampaignRes(callback, result, PtrErrStr(result)));
	}

	//好友战斗系统战斗协议
	public bool CombatFriendCampaignReq(int callback, int friendId, KodGames.ClientClass.Position position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] CombatFriendCampaignReq");
#endif
		return clHlp.CombatFriendCampaign(callback, friendId, position);
	}

	private void OnCombatFriendCampaignRes(int callback, int result, bool isJoin, KodGames.ClientClass.CombatResultAndReward combatResultAndReward,
		KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, int passStageId, KodGames.ClientClass.Player enemyPlayer, List<KodGames.ClientClass.HpInfo> enemyHpInfos,
		com.kodgames.corgi.protocol.RobotInfo robotInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnCombatFriendCampaignRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_COMBAT_FRIEND_CAMPAIGN_SUCCESS)
			RequestMgr.Inst.Response(new OnCombatFriendCampaignRes(callback, isJoin, combatResultAndReward, costAndRewardAndSync, passStageId, enemyPlayer, enemyHpInfos, robotInfo));
		else
			RequestMgr.Inst.Response(new OnCombatFriendCampaignRes(callback, result, PtrErrStr(result)));
	}

	//好友系统主查询协议
	public bool QueryFriendCampaignReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFriendCampaignReq");
#endif
		return clHlp.QueryFriendCampaign(callback);
	}

	private void OnQueryFriendCampaignRes(int callback, int result, bool isJoin, int passStageId, int lastPositionId, List<int> lastFriendIds, int historyMaxDungeonId, int alreadyResetCount,
		KodGames.ClientClass.Player enemyPlayer, List<KodGames.ClientClass.HpInfo> enemyHpInfos, List<KodGames.ClientClass.FriendCampaignPosition> friendPositions, int lastFriendPositionId,
		com.kodgames.corgi.protocol.RobotInfo robotInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFriendCampaignRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FRIEND_CAMPAIGN_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryFriendCampaignRes(callback, isJoin, passStageId, lastPositionId, lastFriendIds, historyMaxDungeonId, alreadyResetCount,
				enemyPlayer, enemyHpInfos, friendPositions, lastFriendPositionId, robotInfo));
		else
			RequestMgr.Inst.Response(new OnQueryFriendCampaignRes(callback, result, PtrErrStr(result)));
	}

	//好友头像申请
	public bool QueryFriendCampaignHelpFriendInfoReq(int callback, List<int> friendIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFriendCampaignHelpFriendInfoReq");
#endif
		return clHlp.QueryFriendCampaignHelpPlayerInfo(callback, friendIds);
	}

	private void OnQueryFriendCampaignHelpPlayerInfoRes(int callback, int result, List<FriendInfo> friendInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFriendCampaignHelpPlayerInfoRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FRIEND_CAMPAIGN_HELP_PLAYER_INFO_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryFriendCampaignHelpPlayerInfoRes(callback, friendInfos));
		else
			RequestMgr.Inst.Response(new OnQueryFriendCampaignHelpPlayerInfoRes(callback, result, PtrErrStr(result)));
	}

	//好友副本排行榜查询协议
	public bool QueryFCRankReq(int callback, int rankType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFCRankReq");
#endif
		return clHlp.QueryFCRank(callback, rankType);
	}

	private void OnQueryFCRankRes(int callback, int result, int rankMaxSize, List<com.kodgames.corgi.protocol.FCRankInfo> rankInfos, string desc, long nextResetTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFCRankRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FC_RANK_SUCCESS)
			RequestMgr.Inst.Response(new QueryFCRankRes(callback, rankMaxSize, rankInfos, desc, nextResetTime));
		else
			RequestMgr.Inst.Response(new QueryFCRankRes(callback, result, PtrErrStr(result)));
	}

	//好友副本情义值详细查询协议
	public bool QueryFCPointDetailReq(int callback, int rankType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFCPointDetailReq");
#endif
		return clHlp.QueryFCPointDetail(callback, rankType);
	}

	private void OnQueryFCPointDetailRes(int callback, int result, int friendMaxCount, int totalPoint, List<com.kodgames.corgi.protocol.FCPointInfo> pointInfos, string desc, long nextResetTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFCPointDetailRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FC_POINT_DETAIL_SUCCESS)
			RequestMgr.Inst.Response(new QueryFCPointDetailRes(callback, friendMaxCount, totalPoint, pointInfos, desc));
		else
			RequestMgr.Inst.Response(new QueryFCPointDetailRes(callback, result, PtrErrStr(result)));
	}

	//奖励查询协议
	public bool QueryFCRankRewarReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFCRankRewarReq");
#endif
		return clHlp.QueryFCRankReward(callback);
	}

	private void OnQueryFCRankRewarRes(int callback, int result, bool isGetReward, int maxRank, int rankNumber, List<com.kodgames.corgi.protocol.FCRewardInfo> rewardInfos, string desc, long nextResetTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFCRankRewarRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FC_RANK_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new QueryFCRankRewarRes(callback, isGetReward, rankNumber, maxRank, rewardInfos, desc, nextResetTime));
		else
			RequestMgr.Inst.Response(new QueryFCPointDetailRes(callback, result, PtrErrStr(result)));
	}

	//领取奖励
	public bool FCRankGetRewardReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] FCRankGetRewardReq");
#endif
		return clHlp.FCRankGetReward(callback);
	}

	private void OnFCRankGetRewardRes(int callback, int result, CostAndRewardAndSync reward, bool isGetReward)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnFCRankGetRewardRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_FC_RANK_GET_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new FCRankGetRewardRes(callback, reward, isGetReward));
		else
			RequestMgr.Inst.Response(new FCRankGetRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion FriendCombatSystem

	#region Illusion

	public bool QueryIllusionReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryIllsionReq");
#endif
		return clHlp.queryIllusion(callback);
	}

	private void OnQueryIllusionRes(int callback, int result, com.kodgames.corgi.protocol.IllusionData illusionData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryIllusionRes" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ILLUSION_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryIllusionRes(callback, illusionData));
		else
			RequestMgr.Inst.Response(new OnQueryIllusionRes(callback, result, PtrErrStr(result)));
	}

	public bool ActivateIllusionReq(int callback, int avatarId, int illusionId, int activateType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ActivateIllusionReq");
#endif
		return clHlp.activateIllusion(callback, avatarId, illusionId, activateType);
	}

	private void OnActivateIllusionRes(int callback, int result, com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnActivateIllusionRes");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ACTIVATE_ILLUSION_SUCCESS)
			RequestMgr.Inst.Response(new OnActivateIllusionRes(callback, illusionAvatar, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnActivateIllusionRes(callback, result, PtrErrStr(result)));
	}

	public bool IllusionReq(int callback, int avatarId, int illusionId, int type, int useStatusType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] IllusionReq");
#endif
		return clHlp.illusion(callback, avatarId, illusionId, type, useStatusType);
	}

	private void OnIllusionRes(int callback, int result, com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnIllusionRes");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ILLUSION_SUCCESS)
			RequestMgr.Inst.Response(new OnIllusionRes(callback, illusionAvatar));
		else
			RequestMgr.Inst.Response(new OnIllusionRes(callback, result, PtrErrStr(result)));
	}

	public bool ActivateAndIllusionReq(int callback, int avatarId, int illusionId, int useStatus)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ActivateAndIllusionReq");
#endif
		return clHlp.activateAndIllusion(callback, avatarId, illusionId, useStatus);
	}

	private void OnActivateAndIllusion(int callback, int result, com.kodgames.corgi.protocol.IllusionAvatar illusionAvatar, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnActivateAndIllusion");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ACTIVATE_AND_ILLUSION_SUCCESS)
			RequestMgr.Inst.Response(new OnActivateAndIllusionRes(callback, illusionAvatar, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnActivateAndIllusionRes(callback, result, PtrErrStr(result)));
	}

	#endregion Illusion

	#region 炼丹房

	//丹炉主查询
	public bool QueryAlchemy(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryAlchemy");
#endif
		return clHlp.queryAlchemy(callback);
	}

	private void OnQueryAlchemy(int callback, int result, int todayAlchemyCount, List<KodGames.ClientClass.Cost> alchemyCosts, List<KodGames.ClientClass.Cost> batchAlchemyCosts, List<com.kodgames.corgi.protocol.BoxReward> boxRewards, com.kodgames.corgi.protocol.ShowCounter showCounter, com.kodgames.corgi.protocol.AlchemyClientIcon alchemyClientIcon, long nextRefreshTime, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryAlchemy" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ALCHEMY_SUCCESS)
			RequestMgr.Inst.Response(new QueryAlchemyRes(callback, todayAlchemyCount, alchemyCosts, batchAlchemyCosts, boxRewards, showCounter, alchemyClientIcon, nextRefreshTime, decomposeInfo));
		else
			RequestMgr.Inst.Response(new QueryAlchemyRes(callback, result, PtrErrStr(result)));
	}

	//领取奖励
	public bool PickAlchemyBox(int callback, int countRewardId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PickAlchemyBox");
#endif
		return clHlp.pickAlchemyBox(callback, countRewardId);
	}

	public bool DanLevelUp(int callback, string guid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PickAlchemyBox");
#endif
		return clHlp.DanLevelUp(callback, guid);
	}

	public bool DanBreakthought(int callback, string guid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PickAlchemyBox");
#endif
		return clHlp.DanBreakthought(callback, guid);
	}

	public bool DanAttributeRefresh(int callback, string guid, List<int> attributeGroupIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PickAlchemyBox");
#endif
		return clHlp.DanAttributeRefresh(callback, guid, attributeGroupIds);
	}


	private void OnPickAlchemyBox(int callback, int result, bool hasPicked, CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward randomReward, bool isNeedRefresh)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnPickAlchemyBox" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_PICK_ALCHEMY_BOX_SUCCESS)
			RequestMgr.Inst.Response(new PickAlchemyBoxRes(callback, hasPicked, costAndRewardAndSync, reward, randomReward, isNeedRefresh));
		else
			RequestMgr.Inst.Response(new PickAlchemyBoxRes(callback, result, PtrErrStr(result)));
	}

	//活动详情协议
	public bool QueryDanActivity(int callback, int activityType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDanActivity");
#endif
		return clHlp.queryDanActivity(callback, activityType);
	}

	private void OnQueryDanActivity(int callback, int result, string acitvityName, List<com.kodgames.corgi.protocol.DanActivityTap> danActivityTaps, bool isNeedRefresh)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryDanActivity" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DAN_ACTIVITY_SUCCESS)
			RequestMgr.Inst.Response(new QueryDanActivityRes(callback, acitvityName, danActivityTaps, isNeedRefresh));
		else
			RequestMgr.Inst.Response(new QueryDanActivityRes(callback, result, PtrErrStr(result)));
	}

	//炼丹批量炼丹协议
	public bool Alchemy(int callback, int chatType, List<KodGames.ClientClass.Cost> cost)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] Alchemy");
#endif
		return clHlp.alchemy(callback, chatType, cost);
	}

	private void OnAlchemy(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.Reward reward, KodGames.ClientClass.Reward extraReward, com.kodgames.corgi.protocol.ShowCounter showCounter, int todayAlchemyCount, List<KodGames.ClientClass.Cost> alchemyCosts, List<KodGames.ClientClass.Cost> batchAlchemyCosts, bool isNeedRefresh, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAlchemy" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ALCHEMY_SUCCESS)
			RequestMgr.Inst.Response(new AlchemyRes(callback, costAndRewardAndSync, reward, extraReward, showCounter, todayAlchemyCount, alchemyCosts, batchAlchemyCosts, isNeedRefresh, decomposeInfo));
		else
			RequestMgr.Inst.Response(new AlchemyRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	//主界面查询协议
	public bool QueryDanHome(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDanHome");
#endif
		return clHlp.queryDanHome(callback);
	}

	private void OnQueryDanHome(int callback, int result, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryDanHome" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DAN_HOME_SUCCESS)
			RequestMgr.Inst.Response(new QueryDanHomeRes(callback, decomposeInfo));
		else
			RequestMgr.Inst.Response(new QueryDanHomeRes(callback, result, PtrErrStr(result)));
	}

	//分解主查询
	public bool QueryDanDecompose(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDanDecompose");
#endif
		return clHlp.queryDanDecompose(callback);
	}

	private void OnQueryDanDecompose(int callback, int result, string acitvityName, long activityStartTime, long activityEndTime, long nextRefreshTime, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryDanDecompose" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DAN_DECOMPOSE_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryDanDecomposeRes(callback, acitvityName, activityStartTime, activityEndTime, nextRefreshTime, decomposeInfo));
		else
			RequestMgr.Inst.Response(new OnQueryDanDecomposeRes(callback, result, PtrErrStr(result)));
	}

	//分解协议
	public bool DanDecompose(int callback, int type, List<string> guids, KodGames.ClientClass.Cost cost)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] DanDecompose");
#endif
		return clHlp.danDecompose(callback, type, guids, cost);
	}

	private void OnDanDecompose(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, bool isNeedRefresh, com.kodgames.corgi.protocol.DecomposeInfo decomposeInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnDanDecompose" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_DAN_DECOMPOSE_SUCCESS)
			RequestMgr.Inst.Response(new OnDanDecomposeRes(callback, costAndRewardAndSync, isNeedRefresh, decomposeInfo));
		else
			RequestMgr.Inst.Response(new OnDanDecomposeRes(callback, result, PtrErrStr(result)));
	}

	//锁定协议
	public bool QueryLockDan(int callback, int type, string guid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryLockDan");
#endif
		return clHlp.lockDan(callback, type, guid);
	}

	private void OnQueryLockDan(int callback, int result, bool isNeedRefresh)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryLockDan" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_LOCK_DAN_SUCCESS)
			RequestMgr.Inst.Response(new OnDanLockRes(callback, isNeedRefresh));
		else
			RequestMgr.Inst.Response(new OnDanLockRes(callback, result, PtrErrStr(result)));
	}

	//灵丹阁查询
	public bool QueryDanStore(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryDanStore");
#endif
		return clHlp.queryDanStore(callback, type);
	}

	private void OnQueryDanStore(int callback, int result, List<com.kodgames.corgi.protocol.DanStoreQueryTime> danStoreQueryTimes)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryDanStore" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_DAN_STORE_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryDanStoreRes(callback, danStoreQueryTimes));
		else
			RequestMgr.Inst.Response(new OnQueryDanStoreRes(callback, result, PtrErrStr(result)));
	}

	private void OnDanLevelUp(int callback, int result, Dan dan, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryLockDan" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_DAN_LEVEL_UP_SUCCESS)
			RequestMgr.Inst.Response(new DanLevelUpRes(callback, dan, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new DanLevelUpRes(callback, result, PtrErrStr(result)));
	}

	private void OnDanBreakthought(int callback, int result, Dan dan, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryLockDan" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_DAN_BREAKTHOUGHT_SUCCESS)
			RequestMgr.Inst.Response(new DanBreakthoughtRes(callback, dan, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new DanBreakthoughtRes(callback, result, PtrErrStr(result)));
	}

	private void OnDanAttributeRefresh(int callback, int result, Dan dan, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryLockDan" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_DAN_ATTRIBUTE_REFRESH_SUCCESS)
			RequestMgr.Inst.Response(new DanAttributeRefreshRes(callback, dan, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new DanAttributeRefreshRes(callback, result, PtrErrStr(result)));
	}


	#endregion 炼丹房

	#region 奇遇 Adventure
	public bool QueryMarvellousNextMarvellousReq(int callback, int selectType, int nextZone, KodGames.ClientClass.Position position)
	{
		return clHlp.marvellousNextMarvellousReq(callback, selectType, nextZone, position);
	}

	public bool QueryMarvellousPickDelayRewardReq(int callback, int eventId, long couldPickTime)
	{
		return clHlp.marvellousPickDelayRewardReq(callback, eventId, couldPickTime);
	}

	public bool QueryMarvellousQueryDelayRewardReq(int callback)
	{
		return clHlp.queryMarvellousQueryDelayRewardReq(callback);
	}

	public bool QueryMarvellousQueryReq(int callback)
	{
		return clHlp.queryMarvellousQueryReq(callback);
	}

	public void OnMarvellousNextMarvellousRes(int callback, int result,
		com.kodgames.corgi.protocol.MarvellousProto marvellousProto,
		KodGames.ClientClass.CombatResultAndReward combatResultAndReward,
		List<com.kodgames.corgi.protocol.DelayReward> delayRewards,
		KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync,
		KodGames.ClientClass.CostAndRewardAndSync fixRewardPackage,
		KodGames.ClientClass.CostAndRewardAndSync randRewardPackage,
		KodGames.ClientClass.CostAndRewardAndSync normalTipsReward)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMarvellousNextMarvellousRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MARVELLOUSNEXTRE_SUCCESS)
			RequestMgr.Inst.Response(new MarvellousNextMarvellousRes(callback, marvellousProto, combatResultAndReward, delayRewards, costAndRewardAndSync, fixRewardPackage, randRewardPackage, normalTipsReward));
		else
			RequestMgr.Inst.Response(new MarvellousNextMarvellousRes(callback, result, PtrErrStr(result)));
	}

	public void OnMarvellousPickDelayRewardRes(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMarvellousPickDelayRewardRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MARVELLOUS_PICKDELAYREWARD_SUCCESS)
			RequestMgr.Inst.Response(new MarvellousPickDelayRewardRes(callback, costAndRewardAndSync, delayRewards));
		else
			RequestMgr.Inst.Response(new MarvellousPickDelayRewardRes(callback, result, PtrErrStr(result)));
	}

	public void OnMarvellousQueryDelayRewardRes(int callback, int result, List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMarvellousQueryDelayRewardRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MARVELLOUS_QUERY_DELAY_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new MarvellousQueryDelayRewardRes(callback, delayRewards));
		else
			RequestMgr.Inst.Response(new MarvellousQueryDelayRewardRes(callback, result, PtrErrStr(result)));
	}

	public void OnMarvellousQueryRes(int callback, int result, com.kodgames.corgi.protocol.MarvellousProto marvellousProto, List<com.kodgames.corgi.protocol.DelayReward> delayRewards)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnMarvellousQueryRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_MARVELLOUSQUERY_SUCCESS)
			RequestMgr.Inst.Response(new MarvellousQueryRes(callback, marvellousProto, delayRewards));
		else
			RequestMgr.Inst.Response(new MarvellousQueryRes(callback, result, PtrErrStr(result)));
	}

	#endregion

	#region 新神秘商店

	//主查询协议
	public bool QueryMysteryerReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryMysteryerReq");
#endif
		return clHlp.queryMysteryer(callback);
	}

	public void OnQueryMysteryerRes(int callback, int result, List<com.kodgames.corgi.protocol.MysteryerGood> goods, List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh, int deleteItemId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryMysteryerRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_MYSTERYER_SUCCESS)
			RequestMgr.Inst.Response(new QueryMysteryerRes(callback, goods, refresh, deleteItemId));
		else
			RequestMgr.Inst.Response(new QueryMysteryerRes(callback, result, PtrErrStr(result)));
	}

	//刷新
	public bool RefreshMysteryerReq(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RefreshMysteryerReq");
#endif
		return clHlp.refreshMysteryer(callback, type);
	}

	public void OnRefreshMysteryerRes(int callback, int result, List<com.kodgames.corgi.protocol.MysteryerGood> goods, List<com.kodgames.corgi.protocol.MysteryerRefresh> refresh, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshMysteryerRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_MYSTERYER_SUCCESS)
			RequestMgr.Inst.Response(new RefreshMysteryerRes(callback, goods, refresh, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new RefreshMysteryerRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	//购买
	public bool BuyMysteryerReq(int callback, int goodId, int place)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyMysteryerReq");
#endif
		return clHlp.buyMysteryer(callback, goodId, place);
	}

	public void OnBuyMysteryerRes(int callback, int result, com.kodgames.corgi.protocol.MysteryerGood goods, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyMysteryerRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_MYSTERYER_SUCCESS)
			RequestMgr.Inst.Response(new BuyMysteryerRes(callback, goods, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new BuyMysteryerRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public void OnSyncMysteryerRes(int count)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSyncMysteryerRes ");
#endif
		RequestMgr.Inst.Response(new SyncMysteryerRes(0, count));
	}

	#endregion

	#region Invite海外兑换码
	//主查询协议
	public bool QueryInviteCodeInfoReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryInviteCodeInfoReq");
#endif
		return clHlp.queryInviteCodeInfo(callback);
	}

	public void OnQueryInviteCodeInfoRes(int callback, int result, Reward inviteSingleRewards, string selfInviteCode, string rewardDesc, List<InviteReward> inviteRewards,
									bool useCodeRewardHasPick, string codeOwnerName, int iconId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryInviteCodeInfoRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_INVITE_CODE_QUERY_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryInviteCodeInfoRes(callback, inviteSingleRewards, selfInviteCode, rewardDesc, inviteRewards, useCodeRewardHasPick, codeOwnerName, iconId));
		else
			RequestMgr.Inst.Response(new OnQueryInviteCodeInfoRes(callback, result, PtrErrStr(result)));
	}

	//领取奖励
	public bool VerifyInviteCodeAndPickRewardReq(int callback, string code)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] VerifyInviteCodeAndPickRewardReq");
#endif
		return clHlp.verifyInviteCodeAndPickReward(callback, code);
	}

	public void OnVerifyInviteCodeAndPickRewardRes(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, string codeOwnername)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnVerifyInviteCodeAndPickRewardRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_INVITE_CODE_VERIFY_SUCCESS)
			RequestMgr.Inst.Response(new OnVerifyInviteCodeAndPickRewardRes(callback, costAndRewardAndSync, codeOwnername));
		else
			RequestMgr.Inst.Response(new OnVerifyInviteCodeAndPickRewardRes(callback, result, PtrErrStr(result)));
	}

	//领取Item奖励
	public bool PickInviteCodeRewardReq(int callback, int rewardId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] PickInviteCodeRewardReq");
#endif
		return clHlp.pickInviteCodeReward(callback, rewardId);
	}

	public void OnPickInviteCodeRewardRes(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnPickInviteCodeRewardRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_INVITE_CODE_PICK_REWARD_QUERY_SUCCESS)
			RequestMgr.Inst.Response(new OnPickInviteCodeRewardRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnPickInviteCodeRewardRes(callback, result, PtrErrStr(result)));
	}

	public bool FacebookShareReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] FacebookShareRes");
#endif
		return clHlp.facebookShare(callback);
	}

	public void OnFacebookShareRes(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnFacebookShareRes");
#endif
	}
	#endregion

	#region 711活动

	//主查询协议
	public bool QuerySevenElevenGiftReq(int callback, string deviceId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QuerySevenElevenGiftReq");
#endif
		return clHlp.QuerySevenElevenGift(callback, deviceId);
	}

	public void OnQuerySevenElevenGiftRes(int callback, int result, int handredPos, int decadePos, int unitPos, KodGames.ClientClass.SevenElevenGift sevenElevenGift, bool isConvertArea, string areaName, bool isConvert)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQuerySevenElevenGiftRes");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_SEVEN_ELEVEN_GIFT_SUCCESS)
			RequestMgr.Inst.Response(new OnQuerySevenElevenGiftRes(callback, handredPos, decadePos, unitPos, sevenElevenGift, isConvertArea, areaName, isConvert));
		else
			RequestMgr.Inst.Response(new OnQuerySevenElevenGiftRes(callback, result, PtrErrStr(result)));
	}

	//摇数协议
	public bool TurnNumberReq(int callback, string deviceId, int position)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] TurnNumberReq");
#endif
		return clHlp.TurnNumber(callback, deviceId, position);
	}

	public void OnTurnNumberRes(int callback, int result, int number)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnTurnNumberRes");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_TURN_NUMBER_SUCCESS)
			RequestMgr.Inst.Response(new OnTurnNumberRes(callback, number));
		else
			RequestMgr.Inst.Response(new OnTurnNumberRes(callback, result, PtrErrStr(result)));
	}

	//领取奖励
	public bool NumberConvertReq(int callback, int contertType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] NumberConvertReq");
#endif
		return clHlp.NumberConvert(callback, contertType);
	}

	public void OnNumberconvertRes(int callback, int result, KodGames.ClientClass.SevenElevenGift sevenElevenGift)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnNumberconvertRes");
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_NUMBER_CONVERT_SUCCESS)
			RequestMgr.Inst.Response(new OnNumberconvertRes(callback, sevenElevenGift));
		else
			RequestMgr.Inst.Response(new OnNumberconvertRes(callback, result, PtrErrStr(result)));
	}

	#endregion

	#region 东海寻仙

	//东海寻仙
	//主查询
	public bool QueryZentiaReq(int callback)
	{
		return clHlp.QueryZentia(callback);
	}
	//东海寻仙玩家获得特殊道具跑马灯信息
	public bool QueryZentiaFlowMessageReq(int callback)
	{
		return clHlp.QueryZentiaFlowMessage(callback);
	}
	//兑换东海寻仙道具
	public bool ExchangeZentiaItemReq(int callback, int exchangeId, int index, List<Cost> costs)
	{
		return clHlp.ExchangeZentiaItem(callback, exchangeId, index, costs);
	}
	//刷新东海寻仙道具兑换
	public bool RefreshZentiaReq(int callback)
	{
		return clHlp.RefreshZentia(callback);
	}
	//查询仙缘兑换商品
	public bool QueryZentiaGoodReq(int callback)
	{
		return clHlp.QueryZentiaGood(callback);
	}
	//仙缘兑换下的商品购买
	public bool BuyZentiaGoodReq(int callback, int goodsId)
	{
		return clHlp.BuyZentiaGood(callback, goodsId);
	}
	//查询全服奖励
	public bool QueryServerZentiaRewardReq(int callback)
	{
		return clHlp.QueryServerZentiaReward(callback);
	}
	//领取全服奖励
	public bool GetServerZentiaRewardReq(int callback, int rewardLevelId)
	{
		return clHlp.GetServerZentiaReward(callback, rewardLevelId);
	}
	//排行榜查询
	public bool QueryZentiaRankReq(int callback)
	{
		return clHlp.QueryZentiaRank(callback);
	}


	public void OnQueryZentia(int callback, int result, List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges
		, Cost refreshCost, string refreshDesc, string zentiaDesc, List<string> flowMessages)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryZentia " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaQueryZentiaRes(callback, zentiaExchanges, refreshCost, refreshDesc, zentiaDesc, flowMessages));
		else
			RequestMgr.Inst.Response(new EastSeaQueryZentiaRes(callback, result, PtrErrStr(result)));
	}

	public void OnQueryZentiaFlowMessage(int callback, int result, List<string> flowMessages)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryZentiaFlowMessage " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_FLOW_MESSAGE_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaQueryZentiaFlowMessageRes(callback, flowMessages));
		else
			RequestMgr.Inst.Response(new EastSeaQueryZentiaFlowMessageRes(callback, result, PtrErrStr(result)));
	}

	public void OnExchangeZentiaItem(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnExchangeZentiaItem " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EXCHANGE_ZENTIA_ITEM_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaExchangeZentiaItemRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new EastSeaExchangeZentiaItemRes(callback, result, PtrErrStr(result)));
	}

	public void OnRefreshZentia(int callback, int result, List<KodGames.ClientClass.ZentiaExchange> zentiaExchanges
		, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshZentia " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_ZENTIA_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaRefreshZentiaRes(callback, zentiaExchanges, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new EastSeaRefreshZentiaRes(callback, result, costAndRewardAndSync, PtrErrStr(result)));
	}
	public void OnQueryZentiaGood(int callback, int result, List<KodGames.ClientClass.ZentiaGood> zentiaGoods, bool isRankOpen)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryZentiaGood " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_GOOD_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaQueryZentiaGoodRes(callback, zentiaGoods, isRankOpen));
		else
			RequestMgr.Inst.Response(new EastSeaQueryZentiaGoodRes(callback, result, PtrErrStr(result)));
	}
	public void OnBuyZentiaGood(int callback, int result, KodGames.ClientClass.ZentiaGood zentiaGood, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyZentiaGood " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_ZENTIA_GOOD_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaBuyZentiaGoodRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new EastSeaBuyZentiaGoodRes(callback, result, PtrErrStr(result)));
	}

	public void OnQueryServerZentiaReward(int callback, int result, long serverZentiaPoint, int totalZentiaPoint, string desc, List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryServerZentiaReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_SERVER_ZENTIA_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaQueryServerZentiaRewardRes(callback, serverZentiaPoint, totalZentiaPoint, desc, zentiaServerRewards));
		else
			RequestMgr.Inst.Response(new EastSeaQueryServerZentiaRewardRes(callback, result, PtrErrStr(result)));
	}
	public void OnGetServerZentiaReward(int callback, int result, long serverZentiaPoint, int totalZentiaPoint, List<KodGames.ClientClass.ZentiaServerReward> zentiaServerRewards, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGetServerZentiaReward " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GET_SERVER_ZENTIA_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaGetServerZentiaRewardRes(callback, serverZentiaPoint, totalZentiaPoint, zentiaServerRewards, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new EastSeaGetServerZentiaRewardRes(callback, result, PtrErrStr(result)));
	}
	public void OnQueryZentiaRank(int callback, int result, long totalZentiaPoint, List<KodGames.ClientClass.ZentiaRank> zentiaRanks, string desc)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryZentiaRank " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_ZENTIA_RANK_SUCCESS)
			RequestMgr.Inst.Response(new EastSeaQueryZentiaRankRes(callback, totalZentiaPoint, zentiaRanks, desc));
		else
			RequestMgr.Inst.Response(new EastSeaQueryZentiaRankRes(callback, result, PtrErrStr(result)));
	}
	public void OnSyncZentia(int count)
	{
		SysLocalDataBase.Inst.LocalPlayer.Zentia = count;
	}

	#endregion

	#region 门派

	// 门派留言的通知
	public void OnGuildMsgNotify(KodGames.ClientClass.GuildMsg guildMsg)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildMsgNotify");
#endif
		RequestMgr.Inst.Response(new OnGuildMsgNotifyRes(guildMsg));
	}

	// 门派动态的通知
	public void OnGuildNewsNotify(KodGames.ClientClass.GuildNews news)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildNewsNotify ");
#endif

		RequestMgr.Inst.Response(new OnGuildNewsNotifyRes(news));
	}

	// 完成隐藏任务的通知
	public void OnAccomplishInvisibleTaskNotify(int taskId, int taskStatus)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAccomplishInvisibleTaskNotify ");
#endif

		RequestMgr.Inst.Response(new OnAccomplishInvisibleTaskNotifyRes(taskId, taskStatus));
	}

	// 被审核和一键拒绝的通知
	public void OnGuildApplyNotify(string guildName, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildApplyNotify ");
#endif

		RequestMgr.Inst.Response(new OnGuildApplyNotifyRes(guildName, guildMiniInfo));
	}

	// 被踢出门派的通知
	public void OnGuildKickNotify(string guildName)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildKickNotify ");
#endif

		RequestMgr.Inst.Response(new OnGuildKickNotifyRes(guildName));
	}

	// 门派信息查询
	public bool GuildQuery(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQuery");
#endif
		return clHlp.GuildQuery(callback);
	}

	void OnGuildQuery(int callback, int result, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQuery " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryRes(callback, guildMiniInfo));
		else
			RequestMgr.Inst.Response(new GuildQueryRes(callback, result, PtrErrStr(result)));
	}

	// 设置公告
	public bool GuildSetAnnouncement(int callback, string announcement)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildSetAnnouncement");
#endif
		return clHlp.GuildSetAnnouncement(callback, announcement);
	}

	void OnGuildSetAnnouncement(int callback, int result, string announcement)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildSetAnnouncement " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_SET_ANNOUNCEMENT_SUCCESS)
			RequestMgr.Inst.Response(new GuildSetAnnouncementRes(callback, announcement));
		else
			RequestMgr.Inst.Response(new GuildSetAnnouncementRes(callback, result, PtrErrStr(result)));
	}

	// 创建门派
	public bool GuildCreate(int callback, string guildName, bool allowAutoEnter)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildCreate");
#endif
		return clHlp.GuildCreate(callback, guildName, allowAutoEnter);
	}

	void OnGuildCreate(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildCreate " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_CREATE_SUCCESS)
			RequestMgr.Inst.Response(new GuildCreateRes(callback, costAndRewardAndSync, guildMiniInfo));
		else
			RequestMgr.Inst.Response(new GuildCreateRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	// 查询门派列表
	public bool GuildQueryGuildList(int callback, string keyWord)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryGuildList");
#endif
		return clHlp.GuildQueryGuildList(callback, keyWord);
	}

	void OnGuildQueryGuildList(int callback, int result, List<KodGames.ClientClass.GuildRecord> guildRecords)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryGuildList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_GUILD_LIST_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryGuildListRes(callback, guildRecords));
		else
			RequestMgr.Inst.Response(new GuildQueryGuildListRes(callback, result, PtrErrStr(result)));
	}

	// 申请进入门派
	public bool GuildApply(int callback, int guildId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildApply");
#endif
		return clHlp.GuildApply(callback, guildId);
	}

	void OnGuildApply(int callback, int result, KodGames.ClientClass.GuildRecord guildRecord)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildApply " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_APPLY_SUCCESS ||
			result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_APPLY_SUCCESS_AUTO_ENTER)
			RequestMgr.Inst.Response(new GuildApplyRes(callback, result, guildRecord));
		else
			RequestMgr.Inst.Response(new GuildApplyRes(callback, result, PtrErrStr(result)));
	}

	// 快速加入门派
	public bool GuildQuickJoin(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQuickJoin");
#endif
		return clHlp.GuildQuickJoin(callback);
	}

	void OnGuildQuickJoin(int callback, int result, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQuickJoin " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUICK_JOIN_SUCCESS)
			RequestMgr.Inst.Response(new GuildQuickJoinRes(callback, guildMiniInfo));
		else
			RequestMgr.Inst.Response(new GuildQuickJoinRes(callback, result, PtrErrStr(result)));
	}

	// 查看门派简略信息
	public bool GuildViewSimple(int callback, int guildId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildViewSimple");
#endif
		return clHlp.GuildViewSimple(callback, guildId);
	}

	void OnGuildViewSimple(int callback, int result, KodGames.ClientClass.GuildInfoSimple guildInfoSimple)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildViewSimple " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_VIEW_SIMPLE_SUCCESS)
			RequestMgr.Inst.Response(new GuildViewSimpleRes(callback, guildInfoSimple));
		else
			RequestMgr.Inst.Response(new GuildViewSimpleRes(callback, result, PtrErrStr(result)));

	}

	// 查看门派排行列表
	public bool GuildQueryRankList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryRankList");
#endif
		return clHlp.GuildQueryRankList(callback);
	}

	void OnGuildQueryRankList(int callback, int result, List<KodGames.ClientClass.GuildRankRecord> guildRankRecords)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryRankList" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_RANK_LIST_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryRankListRes(callback, guildRankRecords));
		else
			RequestMgr.Inst.Response(new GuildQueryRankListRes(callback, result, PtrErrStr(result)));

	}

	// 查看门派留言
	public bool GuildQueryMsg(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryMsg");
#endif
		return clHlp.GuildQueryMsg(callback);
	}

	void OnGuildQueryMsg(int callback, int result, List<KodGames.ClientClass.GuildMsg> guildMsgs, int guildMsDay, int guildMsgCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryMsg " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_MSG_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryMsgRes(callback, guildMsgs, guildMsDay, guildMsgCount));
		else
			RequestMgr.Inst.Response(new GuildQueryMsgRes(callback, result, PtrErrStr(result)));
	}

	// 门派留言
	public bool GuildAddMsg(int callback, string msg)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildAddMsg");
#endif
		return clHlp.GuildAddMsg(callback, msg);
	}

	void OnGuildAddMsg(int callback, int result, List<KodGames.ClientClass.GuildMsg> guildMsgs, int guildMsDay, int guildMsgCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildAddMsg " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_ADD_MSG_SUCCESS)
			RequestMgr.Inst.Response(new GuildAddMsgRes(callback, guildMsgs, guildMsDay, guildMsgCount));
		else
			RequestMgr.Inst.Response(new GuildAddMsgRes(callback, result, PtrErrStr(result)));

	}

	// 查看门派动态
	public bool GuildQueryNews(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryNews");
#endif
		return clHlp.GuildQueryNews(callback);
	}

	void OnGuildQueryNews(int callback, int result, List<KodGames.ClientClass.GuildNews> guildNews)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryNews " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_NEWS_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryNewsRes(callback, guildNews));
		else
			RequestMgr.Inst.Response(new GuildQueryNewsRes(callback, result, PtrErrStr(result)));

	}

	// 修改门派宣言
	public bool GuildSetDeclaration(int callback, string declaration)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildSetDeclaration");
#endif
		return clHlp.GuildSetDeclaration(callback, declaration);
	}

	void OnGuildSetDeclaration(int callback, int result, string declaration)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildSetDeclaration " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_SET_DECLARATION_SUCCESS)
			RequestMgr.Inst.Response(new GuildSetDeclarationRes(callback, declaration));
		else
			RequestMgr.Inst.Response(new GuildSetDeclarationRes(callback, result, PtrErrStr(result)));

	}

	// 转让门派成员列表查询
	public bool GuildQueryTransferMember(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryTransferMember");
#endif
		return clHlp.GuildQueryTransferMember(callback);
	}

	void OnGuildQueryTransferMember(int callback, int result, List<KodGames.ClientClass.GuildTransferMember> guildTransferMembers)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryTransferMember " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_TRANSFER_MEMBER_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryTransferMemberRes(callback, guildTransferMembers));
		else
			RequestMgr.Inst.Response(new GuildQueryTransferMemberRes(callback, result, PtrErrStr(result)));

	}

	// 转让门派
	public bool GuildTransfer(int callback, int destPlayer)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildTransfer");
#endif
		return clHlp.GuildTransfer(callback, destPlayer);
	}

	void OnGuildTransfer(int callback, int result, int playerId, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildTransfer " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_TRANSFER_SUCCESS)
			RequestMgr.Inst.Response(new GuildTransferRes(callback, playerId, guildMiniInfo));
		else
			RequestMgr.Inst.Response(new GuildTransferRes(callback, result, PtrErrStr(result)));

	}

	// 离开门派
	public bool GuildQuit(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQuit");
#endif
		return clHlp.GuildQuit(callback);
	}

	void OnGuildQuit(int callback, int result, KodGames.ClientClass.GuildMiniInfo guildMiniInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQuit " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUIT_SUCCESS)
			RequestMgr.Inst.Response(new GuildQuitRes(callback, guildMiniInfo));
		else
			RequestMgr.Inst.Response(new GuildQuitRes(callback, result, PtrErrStr(result)));
	}

	// 查询门派成员
	public bool GuildQueryMember(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryMember");
#endif
		return clHlp.GuildQueryMember(callback);
	}

	void OnGuildQueryMember(int callback, int result, List<KodGames.ClientClass.GuildMemberInfo> guildMemberInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryMember " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_MEMBER_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryMemberRes(callback, guildMemberInfos));
		else
			RequestMgr.Inst.Response(new GuildQueryMemberRes(callback, result, PtrErrStr(result)));

	}

	// 查询申请列表
	public bool GuildQueryApplyList(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildQueryApplyList");
#endif
		return clHlp.GuildQueryApplyList(callback);
	}

	void OnGuildQueryApplyList(int callback, int result, List<KodGames.ClientClass.GuildApplyInfo> guildApplyInfos)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildQueryApplyList " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_QUERY_APPLY_LIST_SUCCESS)
			RequestMgr.Inst.Response(new GuildQueryApplyListRes(callback, guildApplyInfos));
		else
			RequestMgr.Inst.Response(new GuildQueryApplyListRes(callback, result, PtrErrStr(result)));

	}

	// 审核申请
	public bool GuildReviewApply(int callback, int playerId, bool refuse)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildReviewApply");
#endif
		return clHlp.GuildReviewApply(callback, playerId, refuse);
	}

	void OnGuildReviewApply(int callback, int result, bool refuse, int playerId, int playerName)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildReviewApply " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_REVIEW_APPLY_SUCCESS)
			RequestMgr.Inst.Response(new GuildReviewApplyRes(callback, refuse, playerId, playerName));
		else
			RequestMgr.Inst.Response(new GuildReviewApplyRes(callback, result, PtrErrStr(result)));

	}

	// 一键拒绝
	public bool GuildOneKeyRefuse(int callback, List<int> playerIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildOneKeyRefuse");
#endif
		return clHlp.GuildOneKeyRefuse(callback, playerIds);
	}

	void OnGuildOneKeyRefuse(int callback, int result, List<int> playerIds)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildOneKeyRefuse " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_ONE_KEY_REFUSE_SUCCESS)
			RequestMgr.Inst.Response(new GuildOneKeyRefuseRes(callback, playerIds));
		else
			RequestMgr.Inst.Response(new GuildOneKeyRefuseRes(callback, result, PtrErrStr(result)));

	}

	// 踢出门派
	public bool GuildKickPlayer(int callback, int playerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildKickPlayer");
#endif
		return clHlp.GuildKickPlayer(callback, playerId);
	}

	void OnGuildKickPlayer(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildKickPlayer " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_KICK_PLAYER_SUCCESS)
			RequestMgr.Inst.Response(new GuildKickPlayerRes(callback));
		else
			RequestMgr.Inst.Response(new GuildKickPlayerRes(callback, result, PtrErrStr(result)));

	}

	// 变更职位
	public bool GuildSetPlayerRole(int callback, int playerId, int roleId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildSetPlayerRole");
#endif
		return clHlp.GuildSetPlayerRole(callback, playerId, roleId);
	}

	void OnGuildSetPlayerRole(int callback, int result, int playerId, int roleId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildSetPlayerRole " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_SET_PLAYER_ROLE_SUCCESS)
			RequestMgr.Inst.Response(new GuildSetPlayerRoleRes(callback, playerId, roleId));
		else
			RequestMgr.Inst.Response(new GuildSetPlayerRoleRes(callback, result, PtrErrStr(result)));

	}

	// 更改自动批准
	public bool GuildSetAutoEnter(int callback, bool allowAutoEnter)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildSetAutoEnter");
#endif
		return clHlp.GuildSetAutoEnter(callback, allowAutoEnter);
	}

	void OnGuildSetAutoEnter(int callback, int result, bool allowAutoEnter)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildSetAutoEnter " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_SET_AUTO_ENTER_SUCCESS)
			RequestMgr.Inst.Response(new GuildSetAutoEnterRes(callback, allowAutoEnter));
		else
			RequestMgr.Inst.Response(new GuildSetAutoEnterRes(callback, result, PtrErrStr(result)));

	}

	// 门派建设请求
	public bool QueryConstructionTask(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryConstructionTask");
#endif
		return clHlp.QueryConstructionTask(callback);
	}

	void OnQueryConstructionTask(int callback, int result, KodGames.ClientClass.ConstructionInfo constructionInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryConstructionTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_CONSTRUCTION_TASK_SUCCESS)
			RequestMgr.Inst.Response(new QueryConstructionTaskRes(callback, constructionInfo));
		else
			RequestMgr.Inst.Response(new QueryConstructionTaskRes(callback, result, PtrErrStr(result)));

	}

	// 接受一个门派建设
	public bool AcceptConstructionTask(int callback, int taskId, int taskIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] AcceptConstructionTask");
#endif
		return clHlp.AcceptConstructionTask(callback, taskId, taskIndex);
	}

	void OnAcceptConstructionTask(int callback, int result, KodGames.ClientClass.ConstructionInfo constructionInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnAcceptConstructionTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ACCEPT_CONSTRUCTION_TASK_SUCCESS)
			RequestMgr.Inst.Response(new AcceptConstructionTaskRes(callback, constructionInfo));
		else
			RequestMgr.Inst.Response(new AcceptConstructionTaskRes(callback, result, PtrErrStr(result)));

	}

	// 放弃一个门派建设
	public bool GiveUpConstructionTask(int callback, int taskId, int taskIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GiveUpConstructionTask");
#endif
		return clHlp.GiveUpConstructionTask(callback, taskId, taskIndex);
	}

	void OnGiveUpConstructionTask(int callback, int result, KodGames.ClientClass.ConstructionInfo constructionInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGiveUpConstructionTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GIVE_UP_CONSTRUCTION_TASK_SUCCESS)
			RequestMgr.Inst.Response(new GiveUpConstructionTaskRes(callback, constructionInfo));
		else
			RequestMgr.Inst.Response(new GiveUpConstructionTaskRes(callback, result, PtrErrStr(result)));

	}

	// 完成一个门派建设
	public bool CompleteConstructionTask(int callback, int taskId, List<KodGames.ClientClass.Cost> costs, int taskIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] CompleteConstructionTask");
#endif
		return clHlp.CompleteConstructionTask(callback, taskId, taskIndex, costs);
	}

	void OnCompleteConstructionTask(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.ConstructionInfo constructionInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnCompleteConstructionTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_COMPLETE_CONSTRUCTION_TASK_SUCCESS)
			RequestMgr.Inst.Response(new CompleteConstructionTaskRes(callback, costAndRewardAndSync, constructionInfo));
		else
			RequestMgr.Inst.Response(new CompleteConstructionTaskRes(callback, result, PtrErrStr(result), costAndRewardAndSync));

	}

	// 手动刷新
	public bool RefreshConstructionTask(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RefreshConstructionTask");
#endif
		return clHlp.RefreshConstructionTask(callback);
	}

	void OnRefreshConstructionTask(int callback, int result, KodGames.ClientClass.ConstructionInfo constructionInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshConstructionTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_CONSTRUCTION_TASK_SUCCESS)
			RequestMgr.Inst.Response(new RefreshConstructionTaskRes(callback, constructionInfo, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new RefreshConstructionTaskRes(callback, result, PtrErrStr(result), costAndRewardAndSync));

	}

	// 门派公共商品查询
	public bool QueryGuildPublicShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGuildPublicShop");
#endif
		return clHlp.QueryGuildPublicShop(callback);
	}

	void OnQueryGuildPublicShop(int callback, int result, List<KodGames.ClientClass.GuildPublicGoods> guildPublicGoods, long nextRefreshTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildPublicShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GUILD_PUBLIC_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new QueryGuildPublicShopRes(callback, guildPublicGoods, nextRefreshTime));
		else
			RequestMgr.Inst.Response(new QueryGuildPublicShopRes(callback, result, PtrErrStr(result)));

	}

	// 门派公共商品购买
	public bool BuyGuildPublicGoods(int callback, int goodsId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyGuildPublicGoods");
#endif
		return clHlp.BuyGuildPublicGoods(callback, goodsId);
	}

	void OnBuyGuildPublicGoods(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildPublicGoods guildPublicGoods)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyGuildPublicGoods " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PUBLIC_GOODS_SUCCESS)
			RequestMgr.Inst.Response(new BuyGuildPublicGoodsRes(callback, costAndRewardAndSync, guildPublicGoods));
		else
			RequestMgr.Inst.Response(new BuyGuildPublicGoodsRes(callback, result, PtrErrStr(result), costAndRewardAndSync));

	}

	// 门派玩家商品查询
	public bool QueryGuildPrivateShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGuildPrivateShop");
#endif
		return clHlp.QueryGuildPrivateShop(callback);
	}

	void OnQueryGuildPrivateShop(int callback, int result, List<KodGames.ClientClass.GuildPrivateGoods> guildPrivateGoods)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildPrivateShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GUILD_PRIVATE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new QueryGuildPrivateShopRes(callback, guildPrivateGoods));
		else
			RequestMgr.Inst.Response(new QueryGuildPrivateShopRes(callback, result, PtrErrStr(result)));

	}

	// 门派玩家商品购买
	public bool BuyGuildPrivateGoods(int callback, int goodsId, int goodsCount)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BuyGuildPrivateGoods");
#endif
		return clHlp.BuyGuildPrivateGoods(callback, goodsId, goodsCount);
	}

	void OnBuyGuildPrivateGoods(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildPrivateGoods goods)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBuyGuildPrivateGoods " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BUY_GUILD_PRIVATE_GOODS_SUCCESS)
			RequestMgr.Inst.Response(new BuyGuildPrivateGoodsRes(callback, costAndRewardAndSync, goods));
		else
			RequestMgr.Inst.Response(new BuyGuildPrivateGoodsRes(callback, result, PtrErrStr(result), costAndRewardAndSync));

	}

	// 门派活动商品查询
	public bool QueryGuildExchangeShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGuildExchangeShop");
#endif
		return clHlp.QueryGuildExchangeShop(callback);
	}

	void OnQueryGuildExchangeShop(int callback, int result, List<KodGames.ClientClass.GuildExchangeGoods> guildExchangeGoods)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildExchangeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GUILD_EXCHANGE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new QueryGuildExchangeShopRes(callback, guildExchangeGoods));
		else
			RequestMgr.Inst.Response(new QueryGuildExchangeShopRes(callback, result, PtrErrStr(result)));

	}

	// 门派活动商品兑换
	public bool ExchangeGuildExchangeGoods(int callback, int exchangeId, List<KodGames.ClientClass.Cost> costs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ExchangeGuildExchangeGoods");
#endif
		return clHlp.ExchangeGuildExchangeGoods(callback, exchangeId, costs);
	}

	void OnExchangeGuildExchangeGoods(int callback, int result, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, KodGames.ClientClass.GuildExchangeGoods goods)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnExchangeGuildExchangeGoods " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EXCHANGE_GUILD_EXCHANGE_GOODS_SUCCESS)
			RequestMgr.Inst.Response(new ExchangeGuildExchangeGoodsRes(callback, costAndRewardAndSync, goods));
		else
			RequestMgr.Inst.Response(new ExchangeGuildExchangeGoodsRes(callback, result, PtrErrStr(result), costAndRewardAndSync));

	}

	// 门派任务查询
	public bool QueryGuildTask(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGuildTask");
#endif
		return clHlp.QueryGuildTask(callback);
	}

	void OnQueryGuildTask(int callback, int result, KodGames.ClientClass.GuildTaskInfo guildTaskInfo)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GUILD_TASK_SUCCESS)
			RequestMgr.Inst.Response(new QueryGuildTaskRes(callback, guildTaskInfo));
		else
			RequestMgr.Inst.Response(new QueryGuildTaskRes(callback, result, PtrErrStr(result)));

	}

	// 门派任务投掷
	public bool GuildTaskDice(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildTaskDice");
#endif
		return clHlp.Dice(callback);
	}

	void OnDice(int callback, int result, List<int> diceResults, KodGames.ClientClass.GuildTaskInfo guildTaskInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildTaskDice" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_DICE_SUCCESS)
			RequestMgr.Inst.Response(new GuildTaskDiceRes(callback, diceResults, guildTaskInfo, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new GuildTaskDiceRes(callback, result, PtrErrStr(result)));

	}

	// 门派任务刷新
	public bool RefreshGuildTask(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RefreshGuildTask");
#endif
		return clHlp.RefreshGuildTask(callback);
	}

	void OnRefreshGuildTask(int callback, int result, KodGames.ClientClass.GuildTaskInfo guildTaskInfo, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshGuildTask " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_GUILD_TASK_SUCCESS)
			RequestMgr.Inst.Response(new RefreshGuildTaskRes(callback, guildTaskInfo, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new RefreshGuildTaskRes(callback, result, PtrErrStr(result)));

	}

	// 开启门派关卡
	public bool OpenGuildStage(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OpenGuildStage");
#endif
		return clHlp.OpenGuildStage(callback);
	}

	private void OnOpenGuildStage(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnOpenGuildStage" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_OPEN_GUILD_STAGE_SUCCESS)
			RequestMgr.Inst.Response(new OpenGuildStageRes(callback));
		else
			RequestMgr.Inst.Response(new OpenGuildStageRes(callback, result, PtrErrStr(result)));
	}

	// 门派关卡主查询
	public bool QueryGuildStage(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryGuildStage");
#endif
		return clHlp.QueryGuildStage(callback, type);
	}

	private void OnQueryGuildStage(int callback, int result, List<com.kodgames.corgi.protocol.Stage> stages, int explorePoint, int freeChallengeCount, int itemChallengeCount, int needPassBossCount, KodGames.ClientClass.Cost cost, int mapNum, int index, string preName, string roadPreName, bool isStageOpen, long handResetTime, int handResetStatus, int needGuildLevel, bool isLastMap, int guildLevel)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildStage" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_GUILD_STAGE_SUCCESS)
			RequestMgr.Inst.Response(new QueryGuildStageRes(callback, stages, explorePoint, freeChallengeCount, itemChallengeCount, needPassBossCount, cost, mapNum, index, preName, roadPreName, isStageOpen, handResetTime, handResetStatus, needGuildLevel, isLastMap, guildLevel));
		else
			RequestMgr.Inst.Response(new QueryGuildStageRes(callback, result, PtrErrStr(result)));
	}

	//移动并探索
	public bool GuildStageExplore(int callback, int moveIndex, int exploreIndex)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageExplore");
#endif
		return clHlp.GuildStageExplore(callback, moveIndex, exploreIndex);
	}

	private void OnGuildStageExplore(int callback, int result, KodGames.ClientClass.StageInfo stageInfo, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, int operateType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildStage" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_EXPLORE_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageExploreRes(callback, stageInfo, combatResultAndReward, costAndRewardAndSync, operateType));
		else
			RequestMgr.Inst.Response(new GuildStageExploreRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	//挑战boss
	public bool GuildStageCombatBoss(int callback, int exploreIndex, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageExplore");
#endif
		return clHlp.GuildStageCombatBoss(callback, exploreIndex, type);
	}

	private void OnGuildStageCombatBoss(int callback, int result, KodGames.ClientClass.CombatResultAndReward combatResultAndReward, KodGames.ClientClass.CostAndRewardAndSync costAndRewardAndSync, com.kodgames.corgi.protocol.Rank myRank, com.kodgames.corgi.protocol.BossRank bossRank, List<com.kodgames.corgi.protocol.ShowReward> commonRewards, List<com.kodgames.corgi.protocol.ShowReward> extraRewards, bool hasActivateGoods, com.kodgames.corgi.protocol.Rank thisData)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryGuildStage" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_COMBAT_BOSS_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageCombatBossRes(callback, combatResultAndReward, costAndRewardAndSync, myRank, bossRank, commonRewards, extraRewards, hasActivateGoods, thisData));
		else
			RequestMgr.Inst.Response(new GuildStageCombatBossRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	//宝箱赠送
	public bool GuildStageGiveBox(int callback, int playerId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageGiveBox");
#endif
		return clHlp.GuildStageGiveBox(callback, playerId);
	}

	private void OnGuildStageGiveBox(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageGiveBox" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_GIVE_BOX_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageGiveBoxRes(callback));
		else
			RequestMgr.Inst.Response(new GuildStageGiveBoxRes(callback, result, PtrErrStr(result)));
	}

	//手动重置协议
	public bool GuildStageReset(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageReset");
#endif
		return clHlp.GuildStageReset(callback);
	}

	private void OnGuildStageReset(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageGiveBox" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_RESET_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageResetRes(callback));
		else
			RequestMgr.Inst.Response(new GuildStageResetRes(callback, result, PtrErrStr(result)));
	}

	//查询门派关卡奖励消息
	public bool GuildStageQueryMsg(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageQueryMsg");
#endif
		return clHlp.GuildStageQueryMsg(callback, type);
	}

	private void OnGuildStageQueryMsg(int callback, int result, List<com.kodgames.corgi.protocol.GuildStageMsg> msgs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageQueryMsg" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_QUERY_MSG_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageQueryMsgRes(callback, msgs));
		else
			RequestMgr.Inst.Response(new GuildStageQueryMsgRes(callback, result, PtrErrStr(result)));
	}

	//查询门派boss伤害排行
	public bool GuildStageQueryBossRank(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageQueryBossRank");
#endif
		return clHlp.GuildStageQueryBossRank(callback);
	}

	private void OnGuildStageQueryBossRank(int callback, int result, List<com.kodgames.corgi.protocol.BossRank> bossRanks)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageQueryBossRank" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_QUERY_BOSS_RANK_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageQueryBossRankRes(callback, bossRanks));
		else
			RequestMgr.Inst.Response(new GuildStageQueryBossRankRes(callback, result, PtrErrStr(result)));
	}

	//查询门派boss伤害排行
	public bool GuildStageQueryBossRankDetail(int callback, int mapNum, int num)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageQueryBossRankDetail");
#endif
		return clHlp.GuildStageQueryBossRankDetail(callback, mapNum, num);
	}

	private void OnGuildStageQueryBossRankDetail(int callback, int result, com.kodgames.corgi.protocol.Rank rank, com.kodgames.corgi.protocol.BossRank bossRanks)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageQueryBossRankDetail" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_QUERY_BOSS_RANK_DETAIL_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageQueryBossRankDetailRes(callback, rank, bossRanks));
		else
			RequestMgr.Inst.Response(new GuildStageQueryBossRankDetailRes(callback, result, PtrErrStr(result)));
	}

	//查询门派探索排行
	public bool GuildStageQueryExploreRank(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageQueryRank");
#endif
		return clHlp.GuildStageQueryExploreRank(callback, type);
	}

	private void OnGuildStageQueryExploreRank(int callback, int result, com.kodgames.corgi.protocol.Rank myRank, List<com.kodgames.corgi.protocol.Rank> ranks)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageQueryExploreRank" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_QUERY_EXPLORE_RANK_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageQueryExploreRankRes(callback, myRank, ranks));
		else
			RequestMgr.Inst.Response(new GuildStageQueryExploreRankRes(callback, result, PtrErrStr(result)));
	}

	//查询门派间排行
	public bool GuildStageQueryRank(int callback, int rankType)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageQueryRank");
#endif
		return clHlp.GuildStageQueryRank(callback, rankType);
	}

	private void OnGuildStageQueryRank(int callback, int result, com.kodgames.corgi.protocol.Rank myRank, List<com.kodgames.corgi.protocol.Rank> ranks)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageQueryRank" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_QUERY_RANK_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageQueryRankRes(callback, myRank, ranks));
		else
			RequestMgr.Inst.Response(new GuildStageQueryRankRes(callback, result, PtrErrStr(result)));
	}

	//查询天赋信息
	public bool GuildStageQueryTalent(int callback, int type)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageQueryTalent");
#endif
		return clHlp.GuildStageQueryTalent(callback, type);
	}

	private void OnGuildStageQueryTalent(int callback, int result, int talentPoint, List<com.kodgames.corgi.protocol.BossTalent> bossTalents, int alreadyResetTimes)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageQueryTalent" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_QUERY_TALENT_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageQueryTalentRes(callback, talentPoint, bossTalents, alreadyResetTimes));
		else
			RequestMgr.Inst.Response(new GuildStageQueryTalentRes(callback, result, PtrErrStr(result)));
	}

	//重置天赋
	public bool GuildStageTalentReset(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageTalentReset");
#endif
		return clHlp.GuildStageTalentReset(callback);
	}

	private void OnGuildStageTalentReset(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageTalentReset" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_TALENT_RESET_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageTalentResetRes(callback));
		else
			RequestMgr.Inst.Response(new GuildStageTalentResetRes(callback, result, PtrErrStr(result)));
	}

	//天赋加点
	public bool GuildStageTalentAdd(int callback, int type, int talentId)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] GuildStageTalentAdd");
#endif
		return clHlp.GuildStageTalentAdd(callback, type, talentId);
	}

	private void OnGuildStageTalentAdd(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnGuildStageTalentAdd" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_GUILD_STAGE_TALENT_ADD_SUCCESS)
			RequestMgr.Inst.Response(new GuildStageTalentAddRes(callback));
		else
			RequestMgr.Inst.Response(new GuildStageTalentAddRes(callback, result, PtrErrStr(result)));
	}

	#endregion

	#region 新马版新功能协议

	//FaceBook
	public bool QueryFacebookReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryFacebookReq");
#endif
		return clHlp.queryFacebook(callback);
	}

	public void OnQueryFacebook(int callback, int result, bool isShowIcon)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryFacebook" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_FACEBOOK_SUCCESS)
			RequestMgr.Inst.Response(new QueryFacebookRes(callback, isShowIcon));
		else
			RequestMgr.Inst.Response(new QueryFacebookRes(callback, result, PtrErrStr(result)));
	}

	//FaceBook
	public bool FacebookRewardReq(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] FacebookRewardReq");
#endif
		return clHlp.FacebookReward(callback);
	}

	public void OnFacebookReward(int callback, int result)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnFacebookReward" + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_FACEBOOK_REWARD_SUCCESS)
			RequestMgr.Inst.Response(new FacebookRewardRes(callback));
		else
			RequestMgr.Inst.Response(new FacebookRewardRes(callback, result, PtrErrStr(result)));
	}

	#endregion

	#region 修改玩家名字
	public bool SetPlayerNameReq(int callback, string playerName)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetPlayerNameReq");
#endif
		return clHlp.setPlayerName(callback, playerName);
	}

	public void OnSetPlayerNameRes(int callback, int result, string playerName, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSetPlayerNameRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_PLAYERNAME_SUCCESS)
			RequestMgr.Inst.Response(new OnSetPlayerNameRes(callback, playerName, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnSetPlayerNameRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool SetGuildNameReq(int callback, string guildName)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] SetGuildNameReq");
#endif
		return clHlp.SetGuildName(callback, guildName);
	}

	public void OnSetGuildNameRes(int callback, int result, string guildName, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnSetPlayerNameRes " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_SET_GUILDNAME_SUCCESS)
			RequestMgr.Inst.Response(new OnSetGuildNameRes(callback, guildName, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnSetGuildNameRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}
	#endregion

	#region 机关兽
	public bool ActiveBeast(int callback, int id)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] ActiveBeast");
#endif
		return clHlp.ActiveBeast(callback, id);
	}

	public void OnActiveBeast(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnActiveBeast " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_ACTIVE_BEAST_SUCCESS)
			RequestMgr.Inst.Response(new OnActiveBeastRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnActiveBeastRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool EquipBeastPart(int callback, string guid, int index)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] EquipBeastPart");
#endif
		return clHlp.EquipBeastPart(callback, guid, index);
	}

	public void OnEquipBeastPart(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, Beast beast)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnEquipBeastPart " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_EQUIP_BEAST_PART_SUCCESS)
			RequestMgr.Inst.Response(new OnEquipBeastPartRes(callback, costAndRewardAndSync, beast));
		else
			RequestMgr.Inst.Response(new OnEquipBeastPartRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}

	public bool BeastLevelUp(int callback, string guid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BeastBreakthought");
#endif
		return clHlp.BeastLevelUp(callback, guid);
	}

	public void OnBeastLevelUp(int callback, int result, Beast beast)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBeastBreakthought " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BEAST_LEVEL_UP_SUCCESS)
			RequestMgr.Inst.Response(new OnBeastLevelUpRes(callback, beast));
		else
			RequestMgr.Inst.Response(new OnBeastLevelUpRes(callback, result, PtrErrStr(result)));
	}

	public bool BeastBreakthought(int callback, string guid)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BeastStarUp");
#endif
		return clHlp.BeastBreakthought(callback, guid);
	}

	public void OnBeastBreakthought(int callback, int result, CostAndRewardAndSync costAndRewardAndSync, Beast beast)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBeastStarUp " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BEAST_BREAKTHOUGHT_SUCCESS)
			RequestMgr.Inst.Response(new OnBeastBreakthoughtRes(callback, costAndRewardAndSync, beast));
		else
			RequestMgr.Inst.Response(new OnBeastBreakthoughtRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}


	public bool QueryBeastExchangeShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] QueryBeastExchangeShop");
#endif
		return clHlp.QueryBeastExchangeShop(callback);
	}

	public void OnQueryBeastExchangeShop(int callback, int result, List<ZentiaExchange> beastExchanges, Cost refreshCost, long nextFreeStartTime)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnQueryBeastExchangeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_QUERY_BEAST_EXCHANGE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new OnQueryBeastExchangeShopRes(callback, beastExchanges, refreshCost, nextFreeStartTime));
		else
			RequestMgr.Inst.Response(new OnQueryBeastExchangeShopRes(callback, result, PtrErrStr(result)));
	}

	public bool BeastExchangeShop(int callback, int exchangeId, int index, List<KodGames.ClientClass.Cost> costs)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] BeastExchangeShop");
#endif
		return clHlp.BeastExchangeShop(callback, exchangeId, index, costs);
	}

	public void OnBeastExchangeShop(int callback, int result, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnBeastExchangeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_BEAST_EXCHANGE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new OnBeastExchangeShopRes(callback, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnBeastExchangeShopRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}


	public bool RefreshBeastExchangeShop(int callback)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] RefreshBeastExchangeShop");
#endif
		return clHlp.RefreshBeastExchangeShop(callback);
	}

	public void OnRefreshBeastExchangeShop(int callback, int result, List<ZentiaExchange> beastExchanges, CostAndRewardAndSync costAndRewardAndSync)
	{
#if SERVER_BUSSINESS_ENABLE_LOG
		Debug.Log("[ServerBusiness] OnRefreshBeastExchangeShop " + result);
#endif
		if (result == com.kodgames.corgi.protocol.Protocols.E_GAME_REFRESH_BEAST_EXCHANGE_SHOP_SUCCESS)
			RequestMgr.Inst.Response(new OnRefreshBeastExchangeShopRes(callback, beastExchanges, costAndRewardAndSync));
		else
			RequestMgr.Inst.Response(new OnRefreshBeastExchangeShopRes(callback, result, PtrErrStr(result), costAndRewardAndSync));
	}
	#endregion
}