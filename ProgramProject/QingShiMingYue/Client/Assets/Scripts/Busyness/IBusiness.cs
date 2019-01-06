using System;
using System.Collections;
using System.Collections.Generic;
using KodGames.ClientHelper;
using UnityEngine;

public interface IBusiness
{
	void Initialze();

	void Dispose();

	void Update();

	bool DoesSupprotReconnect();

	string PtrErrStr(int prtVal);

	bool Logout(int callback);

	bool DisconnectAS();

	int GetNetStatus();

	bool DisconnectIS();

	bool SendTimeout();

	bool ConnectIS(string hostname, int port, NetType netType, int areaID, int callback);

	//AuthServer
	bool CreateAccount(string authServerHostName, int port, string email, string password, int channelId, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, int callback);

	bool QueryManifest(string authServerHostName, int port, int resourceVersion, int version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, int callback, int subChannelID);

	//login and quick login
	//bool AuthonActivityCode(string authServerHostName, int port, string email, string password, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, string userInput, int callback);
	bool AuthActivityCode(string authServerHostName, int port, int accountId, string activityCode, int callback);

	bool Login(string authServerHostName, int port, String email, String password, string version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, string channelUniqueId, string token, AccountChannel accountChannel, int callback);

	bool QuickLogin(string authServerHostName, int port, int version, int channelID, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, string bindedAccount, int callback);

	//GameServer
	bool ResetPassword(string authServerHostName, int port, string email, string oldPassword, string newPassword, int callback);

	bool BindAccount(string authServerHostName, int port, string email, string password, string mobile, KodGames.ClientClass.DeviceInfo deviceInfo, string klsso, int callback);

	bool QueryInitInfo(int callback);

	#region Position

	bool QueryPositionList(int callback);

	bool OpenPosition(int callback, int positionId);

	bool SetMasterPosition(int callback, int positionId);

	bool EmBattle(int callback, int positionId, int locationId1, int locationId2);

	bool ChangeLocation(int callback, string guid, int resourceId, string offGuid, int positionId, int location, int index);

	bool OneClickPositionOff(int callback, int positionId);

	#endregion Position

	#region Partner

	bool PartnerOpen(int callback, int partnerId);

	bool PartnerSetup(int callback, int positionId, int partnerId, string avatarGuid);

	#endregion Partner

	#region Hire

	bool QueryDinerList(int callback);

	bool HireDiner(int callback, int qualityType, int dinerId);

	bool RenewDiner(int callback, int qualityType, int dinerId);

	bool FireDiner(int callback, int qualityType, int dinerId);

	bool RefreshDinerList(int callback, int bagId);

	#endregion Hire

	#region Avatar

	// Avatar Power Up.
	bool AvatarLevelUp(int callback, String avatarGUID, bool levelType);

	// Avatar BreakThought.
	bool AvatarBreakthought(int callback, string avatarGUID, List<string> destroyAvatarGUIDs);

	// Avatar Meridian.
	bool ChangeMeridian(int callback, int meridianId, string avatarGuid);

	bool SaveMeridian(int callback, string avatarGuid, bool saveOrNot, int meridianId);

	//Domineer
	bool ChangeDomineer(int callback, string avatarGuid, List<string> destroyAvatarGuids);

	bool SaveDomineer(int callback, string avatarGuid, bool isSave);

	#endregion Avatar

	#region Equipment

	// Equipment Power Up.
	bool EquipLevelUp(int callback, String equipGUID, bool levelType);

	// Equipment BreakThought.
	bool EquipBreakthought(int callback, string equipGUID, List<string> destroyEquipGUIDs);

	#endregion Equipment

	#region Notify

	bool SendAPNToken(int callback, byte[] token);

	#endregion Notify

	//shop
	bool QueryGoodsList(int callback);

	bool BuyGoods(int callback, KodGames.ClientClass.GoodRecord goodRecord, int statusIndex);

	//
	bool ConsumeItem(int callback, int itemId, int amount, int groupIndex, string phoneNumber);

	bool SellItem(int callback, List<KodGames.ClientClass.Cost> items);

	//Dungeon
	bool QueryDungeonList(int callback);

	bool SetZoneStatus(int callback, int zoneId, int status);

	bool SetDungeonStatus(int callback, int dungeonId, int status);

	bool Combat(int callback, int dungeonId, KodGames.ClientClass.Position position, int npcId);

	bool ResetDungeonCompleteTimes(int callback, int dungeonId);

	bool DungeonGetReward(int callback, int zoneId, int dungeonDifficulty, int boxIndex);

	bool SetDungeonDialogState(int callback, int dungeonId, int state);

	bool ContinueCombat(int callback, int zoneId, int dungeonId);

	bool QueryDungeonGuide(int callback, int dungeonId);

	bool QueryTravel(int callback, int dungeonId);

	bool BuyTravel(int callback, int dungeonId, int goodId);

	bool QueryRecruiteNpc(int callback, int dungeonId);

	// Assistant
	bool QueryTaskList(int callback);

	bool TaskCondition(int callback, int gotoUI);

	//Tower
	bool QueryMelaleucaFloorPlayerInfo(int callback);

	bool QueryMelaleucaFloorInfo(int callback, int layers);

	bool MelaleucaFloorCombat(int callback, int layers, KodGames.ClientClass.Position position);

	bool MelaleucaFloorConsequentCombat(int callback, int layers, int combatCount, KodGames.ClientClass.Position position);

	bool MelaleucaFloorThisWeekRank(int callback);

	bool MelaleucaFloorLastWeekRank(int callback);

	bool MelaleucaFloorGetReward(int callback);

	bool MelaleucaFloorWeekRewardInfo(int callback);

	//WolfSmoke
	bool QueryBuyWolfSmokeShop(int callback, int goodsId, int goodsIndex);

	bool QueryCombatWolfSmoke(int callback, int additionId, KodGames.ClientClass.Position position);

	bool QueryJoinWolfSmoke(int callback, int positionId);

	bool QueryWolfSmoke(int callback);

	bool QueryWolfSmokeEnemy(int callback);

	bool QueryWolfSmokePosition(int callback);

	bool QueryWolfSmokeShop(int callback);

	bool QueryRefreshWolfSmokeShop(int callback);

	bool QueryResetWolfSmoke(int callback);

	//Email
	bool QueryEmailListInfo(int callback, int emailType);

	bool GetAttachments(int callback, long emailId);

	// Start Server Reward.
	bool PickStartServerReward(int callback, int pickId);

	//HandBook
	bool MergeIllustration(int callback, int id, int count);

	bool QueryIllustration(int callback);

	//Daily SignIn
	bool SignIn(int callback, int signType);

	bool ApplePurchase(byte[] paymentTransactionReceipt, int callback);

	bool ApplePurchaseTest(int goodsID, int count, int callback, string additionalData);

	bool SkillLevelUp(int callback, string skillGUID, List<string> destroySkillGUIDs);

	// Chat
	bool Chat(int callback, com.kodgames.corgi.protocol.ChatMessage chatMessage);

	bool QueryChatMessageList(int callback);

	bool CloseChatMessageDialog(int callback);

	// Arena
	bool QueryArenaRank(int callback);

	bool ArenaCombat(int callback, int rank, KodGames.ClientClass.Position position);

	bool QueryPlayerInfo(int callback, int playerId);

	bool QueryArenaPlayerInfo(int callback, int rank, int arenaGradeId);

	bool QueryRankToFew(int callback);

	// Auction
	bool BuyConsignment(int callback, long consignmentIdx, List<KodGames.ClientClass.Cost> costs);

	bool Consignment(int callback, string guid, List<KodGames.ClientClass.Cost> costs);

	bool Downshelf(int callback, long consignmentIdx);

	bool QueryConsignmentList(int callback, int type, int subType, int isPrevNotNext, long consignIdx);

	bool QuerySelfConsignment(int callback);

	bool QueryAvatarList(int callback, int type);

	// Setting
	bool ExchangeCode(int callback, string strRewardKey);

	bool SettingFeedback(int callback, int type, string strInfo);

	// Activity
	bool QueryFixedTimeActivityReward(int callback);

	bool GetFixedTimeActivityReward(int callback);//FixedTime

	bool QueryExchangeList(int callback);

	bool ExchangeReq(int callback, int exchangeId, List<KodGames.ClientClass.Cost> costs, int groupId);

	//Tavern
	bool TavernQuery(int callback);

	bool TavernBuy(int callback, int tavernId, int tavernType);

	bool LevelRewardGetReward(int callback, int levelRewardId);

	bool QueryLevelReward(int callback);

	bool QueryGradePoint(int callback);

	bool BuyAndUse(int callback, int goodsId, int statusIndex);

	bool BuySpecialGoods(int callback, int goodsId);

	// Tutorial
	bool GetTutorialAvatarAndSetPlayerName(int callback, int resourceId, string playerName, int tutorialId);

	bool CompleteTutorial(int callback, int tutorialId);

	bool NoviceCombat(int callback);

	bool FetchRandomPlayerNames(int callback);

	// Quest : DailyGuid
	bool QueryQuestInfo(int callback);

	bool PickQuestReward(int callback, int questId);

	// PlayerLevelUp
	bool GetLevelUpReward(int callback, int wantPickLevel);

	// Mystery Merchant
	bool QueryMysteryShopInfo(int callback, int shopType);

	bool ChangeMysteryShopInfo(int callback, int shopType, int refreshId);

	bool BuyMysteryGoods(int callback, int shopType, int goodsId, int goodsIndex);

	//QinInfo
	bool QueryQinInfo(int callback);

	bool AnswerQinInfo(int callback, int questionId, int answerNum);

	bool GetQinInfoContinueReward(int callbak);

	//MonthCard
	bool QueryMonthCardInfo(int callback);

	bool MonthCardPickReward(int callback, int monthCardId, int rewardType);

	//GiveMeFive
	bool GiveFiveStarsEvaluate(int callback, bool isEvaluate);

	//Friend
	bool QueryFriendList(int callback);

	bool RandomFriend(int callback);

	bool QueryPlayerName(int callback, string playerName);

	bool InviteFriend(int callback, int invitedPlayerId, string invitedPlayerName);

	bool AnswerFriend(int callback, int invitorPlayerId, long passiveEmailId, bool agree);

	bool QueryFriendPlayerInfo(int callback, int friendPlayerId);

	bool RemoveFriend(int callback, int removePlayerId);

	bool CombatFriend(int callback, int friendId, KodGames.ClientClass.Position position);

	bool OperationActivityQueryReq(int callback);

	bool OperationActivityPickRewardReq(int callback, int operationId);

	//Adventure
	bool QueryMarvellousNextMarvellousReq(int callback, int selectType, int nextZone, KodGames.ClientClass.Position position);

	bool QueryMarvellousPickDelayRewardReq(int callback, int eventId, long couldPickTime);

	bool QueryMarvellousQueryDelayRewardReq(int callback);

	bool QueryMarvellousQueryReq(int callback);

	//FriendCombatSystem
	bool QueryFriendCampaignReq(int callback);

	bool ResetFriendCampaignReq(int callback);

	bool JoinFriendCampaignReq(int callback, int positionId, List<int> friendIds);

	bool CombatFriendCampaignReq(int callback, int playerId, KodGames.ClientClass.Position position);

	bool QueryFriendCampaignHelpFriendInfoReq(int callback, List<int> playerIds);

	bool QueryFCRankReq(int callback, int rankType);

	bool QueryFCPointDetailReq(int callback, int rankType);

	bool QueryFCRankRewarReq(int callback);

	bool FCRankGetRewardReq(int callback);

	//Illusion
	bool QueryIllusionReq(int callback);

	bool ActivateIllusionReq(int callback, int avatarId, int illusionId, int activateType);

	bool ActivateAndIllusionReq(int callback, int avatarId, int illusionId, int useStatus);

	bool IllusionReq(int callback, int avatarId, int illusionId, int type, int useStatusType);

	//新神秘商店
	bool QueryMysteryerReq(int callback);
	bool RefreshMysteryerReq(int callback, int type);
	bool BuyMysteryerReq(int callback, int goodId, int place);


	//海外兑换码
	bool QueryInviteCodeInfoReq(int callback);
	bool VerifyInviteCodeAndPickRewardReq(int callback, string code);
	bool PickInviteCodeRewardReq(int callback, int rewardId);
	bool FacebookShareReq(int callback);

	//711活动
	bool QuerySevenElevenGiftReq(int callback, string deviceId);
	bool TurnNumberReq(int callback, string deviceId, int position);
	bool NumberConvertReq(int callback, int contertType);
	//东海寻仙
	//主查询
	bool QueryZentiaReq(int callback);
	//东海寻仙玩家获得特殊道具跑马灯信息
	bool QueryZentiaFlowMessageReq(int callback);
	//兑换东海寻仙道具
	bool ExchangeZentiaItemReq(int callback, int exchangeId, int index, List<KodGames.ClientClass.Cost> costs);
	//刷新东海寻仙道具兑换
	bool RefreshZentiaReq(int callback);
	//查询仙缘兑换商品
	bool QueryZentiaGoodReq(int callback);
	//仙缘兑换下的商品购买
	bool BuyZentiaGoodReq(int callback, int goodsId);
	//查询全服奖励
	bool QueryServerZentiaRewardReq(int callback);
	//领取全服奖励
	bool GetServerZentiaRewardReq(int callback, int rewardLevelId);
	//排行榜查询
	bool QueryZentiaRankReq(int callback);

	//内丹系统
	bool QueryAlchemy(int callback);
	bool PickAlchemyBox(int callback, int countRewardId);
	bool QueryDanActivity(int callback, int activityType);
	bool Alchemy(int callback, int chatType, List<KodGames.ClientClass.Cost> cost);
	bool QueryDanHome(int callback);
	bool QueryDanStore(int callback, int type);

	bool DanDecompose(int callback, int type, List<string> guids, KodGames.ClientClass.Cost cost);
	bool QueryDanDecompose(int callback);
	bool QueryLockDan(int callback, int type, string guid);
	bool DanLevelUp(int callback, string guid);
	bool DanBreakthought(int callback, string guid);
	bool DanAttributeRefresh(int callback, string guid, List<int> attributeGroupIds);
	// 门派
	bool GuildQuery(int callback);
	bool GuildSetAnnouncement(int callback, string announcement);
	bool GuildCreate(int callback, string guildName, bool allowAutoEnter);
	bool GuildQueryGuildList(int callback, string keyWord);
	bool GuildApply(int callback, int guildId);
	bool GuildQuickJoin(int callback);
	bool GuildViewSimple(int callback, int guildId);
	bool GuildQueryRankList(int callback);
	bool GuildQueryMsg(int callback);
	bool GuildAddMsg(int callback, string msg);
	bool GuildQueryNews(int callback);
	bool GuildSetDeclaration(int callback, string declaration);
	bool GuildQueryTransferMember(int callback);
	bool GuildTransfer(int callback, int destPlayer);
	bool GuildQuit(int callback);
	bool GuildQueryMember(int callback);
	bool GuildQueryApplyList(int callback);
	bool GuildReviewApply(int callback, int playerId, bool refuse);
	bool GuildOneKeyRefuse(int callback, List<int> playerIds);
	bool GuildKickPlayer(int callback, int playerId);
	bool GuildSetPlayerRole(int callback, int playerId, int roleId);
	bool GuildSetAutoEnter(int callback, bool allowAutoEnter);
	bool QueryConstructionTask(int callback);
	bool AcceptConstructionTask(int callback, int taskId, int taskIndex);
	bool GiveUpConstructionTask(int callback, int taskId, int taskIndex);
	bool CompleteConstructionTask(int callback, int taskId, List<KodGames.ClientClass.Cost> costs, int taskIndex);
	bool RefreshConstructionTask(int callback);
	bool QueryGuildPublicShop(int callback);
	bool BuyGuildPublicGoods(int callback, int goodsId);
	bool QueryGuildPrivateShop(int callback);
	bool BuyGuildPrivateGoods(int callback, int goodsId, int goodsCount);
	bool QueryGuildExchangeShop(int callback);
	bool ExchangeGuildExchangeGoods(int callback, int exchangeId, List<KodGames.ClientClass.Cost> costs);
	bool QueryGuildTask(int callback);
	bool GuildTaskDice(int callback);
	bool RefreshGuildTask(int callback);

	//	门派关卡
	bool OpenGuildStage(int callback);
	bool QueryGuildStage(int callback, int type);
	bool GuildStageExplore(int callback, int moveIndex, int exploreIndex);
	bool GuildStageCombatBoss(int callback, int exploreIndex, int type);
	bool GuildStageGiveBox(int callback, int playerId);
	bool GuildStageReset(int callback);
	bool GuildStageQueryMsg(int callback, int type);
	bool GuildStageQueryBossRank(int callback);
	bool GuildStageQueryBossRankDetail(int callback, int mapNum, int num);
	bool GuildStageQueryExploreRank(int callback, int type);
	bool GuildStageQueryRank(int callback, int rankType);
	bool GuildStageQueryTalent(int callback, int type);
	bool GuildStageTalentReset(int callback);
	bool GuildStageTalentAdd(int callback, int type, int talentId);

	// FaceBook
	bool QueryFacebookReq(int callback);
	bool FacebookRewardReq(int callback);


	//玩家名称修改
	bool SetPlayerNameReq(int callback, string playerName);
	bool SetGuildNameReq(int callback, string guildName);

	//机关兽
	bool ActiveBeast(int callback, int id);
	bool EquipBeastPart(int callback, string guid, int index);
	bool BeastBreakthought(int callback, string guid);
	bool BeastLevelUp(int callback, string guid);
	bool QueryBeastExchangeShop(int callback);
	bool BeastExchangeShop(int callback, int exchangeId, int index, List<KodGames.ClientClass.Cost> costs);
	bool RefreshBeastExchangeShop(int callback);
}