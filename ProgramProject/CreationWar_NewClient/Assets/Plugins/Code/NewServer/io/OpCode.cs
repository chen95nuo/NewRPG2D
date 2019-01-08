
using System.Collections;
/// <summary>
/// 协议ID -
/// </summary>
public	enum OpCode :short
{
	YuanDBGet = 3,
	YuanDBUpdate = 4,
	Login = 5,
	FastLogin = 6,
	Logon = 7,
	FastLogon = 8,
	PlayerCreat = 9,
	PwdGet = 10,
	PwdUpdate = 11,
	BindUserID = 12,
	GetServerTime = 13,
	RandomNum = 14,
	GetServerActor = 15,
	SendMessage = 16,
	GetPlayers = 17,
	DeletePlayer = 18,
	EnterGame = 19,
	GetPlayerList = 20,
	SetFirend = 21,
	GetPlayerID = 22,
	TeamCreate = 23,
	TeamAdd = 24,
	GetTeam = 25,
	GetMyTeam = 26,
	GetMyTeamPlayers = 27,
	Request = 28,
	ReturnRequest = 29,
	GropsCreate = 30,
	GropsAdd = 31,
	GetGrops = 32,
	GropsRemove = 33,
	TransactionRequest = 34,
	GetTransactionInfo = 35,
	SendTransactionID = 36,
	SendTransactionInfo = 37,
	TransactionClose = 38,
	LegionTempCreate = 39,
	LegionTempAdd = 40,
	LegionDBCreate = 41,
	LegionDBAdd = 42,
	GetLegion = 43,
	GuildCreate = 44,
	GuildAdd = 45,
	GetGuildAll = 46,
	GetGuild = 47,
	TeamPlayerRemove = 48,
	GuildPlayerRemove = 49,
	GetFavorableItem = 50,
	GetItems = 51,
	GetEquipment = 52,
	BuyItem = 53,
	GuildBuild = 54,
	GuildFund = 55,
	MailSend = 56,
	MailGetOut = 57,
	MailGetIn = 58,
	MailRead = 59,
	MailDelete = 60,
	GetMailTool = 61,
	GetActivity = 62,
	SendRanking = 63,
	GetDailyBenefits = 64,
	TeamInviteAdd = 65,
	GetPlayerTeamID = 66,
	TeamPlayerUp = 67,
	GuildPlayerPurview = 68,
	SetHonor = 69,
	SendPVPInfo = 70,
	AddLegionPVPQueue = 71,
	SendLegionPVPInfo = 72,
	PVPCreate = 73,
	PVPDissolve = 74,
	SendTeamTeamInfo = 75,
	SendTeamTeamLeave = 76,
	TempTeamOK = 77,
	TempTeamWait = 78,
	AddTempTeam = 79,
	RemoveTempTeam = 80,
	GetTempTeam = 81,
	GetMyGrop = 82,
	GetMyLegion = 83,
	RemoveLegion = 84,
	TeamRemove = 85,
	PVPInviteAdd = 86,
	LegionInviteAdd = 87,
	PVPPlayerUp = 88,
	LegionPlayerUp = 89,
	GuildInviteAdd = 90,
	GuildRemove = 91,
	GuildStopTalk = 92,
	MailOtherGet = 93,
	AddLegionQueue = 94,
	SendLegionInfo = 95,
	SetPlayerBehavior = 96,
	SendTVMessage = 97,
	SendBenefitsInfo = 98,
	SetTitle = 99,
	GetPVETeamList = 100,
	TeamDissolve = 101,
	GetPlayerGold = 102,
	LegionOneAdd = 103,
	LegionOneTeamAdd = 104,
	LegionOneRemove = 105,
	LegionOneList = 106,
	GuildLevelUp = 107,
	SendGameVersion = 108,
	SendPVPOneInfo = 109,
	PVPGO = 110,
	LegionOneClose = 111,
	HeartPage = 112,
	InRoom = 113,
	LeaveRoom = 114,
	Login91 = 115,
	ActivityPVPAdd = 116,
	ActivityPVPRemove = 117,
	GetBlood91 = 118,
	ServerPlayerMax = 119,
	OtherLogin = 120,
	RedemptionCode = 121,
	PVP1Invite = 122,
	PVP1InviteRemove = 123,
	InviteGoPVE = 124,
	PVP1Fruit = 125,
	SendError = 126,
	BindDevice = 127,
	OtherBindDevice = 128,
	TeamHeadIn = 129,
	LoginUC = 130,
	SetLicense = 131,
	SendGMTV = 132,
	LoginDL = 133,
	RefershTable = 134,
	RefershTableSome = 135,
	LoginMI = 136,
	GetRank = 137,
	LoginTSZ = 138,
	LoginZSY = 139,
	ClientCallServer = 140,
	GetRankOne = 141,
	ClientBuyItem = 142,
	ClientGetItemSome = 143,
	ClientMoney = 144,
	GetTableForID = 145,
	GetPlayerForName = 146,
	GetRankTopYT = 147,
	GetRandomPlayer = 148,
	GetSystemMail = 149,
	GetMyMail = 150,
	EquepmentBuild = 151,
	EquepmentHole = 152,
	EquepmentMosaic = 153,
	Training = 154,
	TrainingSave = 155,
	EquepmentProduce = 156,
	GetStoreList = 157,
	BuyStoreClient = 158,
	GetRandomItem = 159,
	PlayerInMap = 160,
	AddExperience = 161,
	GetTablesSomeForIDs = 162,
	PlayerLook = 163,
	LoginOPPO = 164,
	DoneCard = 165,
	OpenCard = 166,
	SendPacks = 167,
	DoneCardPVP = 168,
	IsSaveDate = 169,
	ServerPlayerOut = 170,
	LoginZSYAll = 171,
	ActivityFirstCharge = 172,
	ActivityGetInfo = 173,
	ActivityGetReward = 174,
	GetClientParms = 175,
	GetFirstPacks = 176,
	UseItem = 177,
	TaskCompleted = 178,
	GetServerList = 179,
	HangUpAddExp = 180,
	SetFirstServer = 181,
	AuctionCompany = 182,
	//
	// 摘要: 
	//     奖池主协议
	JackPot = 183,
	//
	// 摘要: 
	//     任务主协议
	Task = 184,
	//
	// 摘要: 
	//     活动类主协议
	Activity = 185,
	//
	// 摘要: 
	//     GM类主协议
	GM = 186,
	Firends = 187,
	//
	// 摘要: 
	//     验证类主协议
	Validation = 188,
	//
	// 摘要: 
	//     使用金钱主协议
	UseMoney = 189,
	//
	// 摘要: 
	//     活动触发协议
	TriggerActivity = 190,
	JoinActivity = 191,
	CanFinishActivity = 192,
	FinishActivity = 193,
	BattlefieldReady = 194,
	BattlefieldSpawnInfo = 195,
	BattlefieldInfo = 196,
	BattlefieldKill = 197,
	BattlefieldDie = 198,
	BattlefieldGetFlag = 199,
	BattlefieldHitBoss = 200,
	BattlefieldExit = 201,
	ActivityBossResult = 202,
	ActivityBoosHP = 203,
	ActivityBossDamage = 204,
	ActivityJoinSuccess = 205,
    ActivityBossHPMax = 251,
	ActivityExit = 252,
	//
	// 摘要: 
	//     联运支付信息的协议
	Payinformation = 206,
	PayGameRole = 207,
	BattlefieldResult = 208,
	//
	// 摘要: 
	//     GM查询
	GMSearch = 209,
	BattlefieldScoreInfo = 210,
	payBack = 211,
	BattlefieldBossInfo = 212,
	BattlefieldResultBoss = 213,
	BattlefieldInfoBoss = 214,
   
	//
	// 摘要: 
	//     数据库所对应的那条支付数据id
	payID = 215,
	//
	// 摘要: 
	//     添加血石成功之后，传递给登录服务器，修改内存
	payInfo = 216,
	//
	// 摘要: 
	//     购买栏位的协议；
	payLanwei = 217,
	//
	// 摘要: 
	//     服务器返回角色栏位的数量
	lanweiNumber = 218,
	//
	// 摘要: 
	//     修改角色栏位数量的协议
	correctLanwei = 219,

	//
	// 摘要: 
	//     中手游的需求日志协议
	LogInfo = 220,
	//
	// 摘要: 
	//     装备拆分
	EquipmentResolve = 221, 
	/// <summary>
	/// 摘要: 
	///    在线宝箱
	/// </summary>
	OnlineChests = 222,

	/// <summary>
	/// 设置公会公告
	/// </summary>
	SetGuildNotice=223,

	/// <summary>
	/// 获取公会公告
	/// </summary>
	GetGuildNotice=224,

	/// <summary>
	/// 设置公会宣言
	/// </summary>
	SetGuildDeclaration=225,

	/// <summary>
	/// 获取公会宣言
	/// </summary>
	GetGuildDeclaration=226,
    BattlefieldNotifyExit = 227,
    BattlefieldTimeOut = 231,
    DefenceBattleStart = 232,
    DefenceBattleStartCommit = 233,
    BallApplyDamage = 234,
    BallCommitDamage = 235,
	DefenceFinish = 281,
    PVPCancel = 228,

	/**
     * 玩家召唤骷髅
     */
	SummonSkull=229,

	/**
     * 根据名称搜索表中相关数据
     */
	 GetTablesSomeForNames = 230,

	/**
     * boss死亡
     */
	Boss2WasDie = 236,

        /**
     * 公会解散
     */
    GuildDismiss = 237,
    
    /**
     * 会长删号
     */
    GuildHeadMiss = 238,
    ChangeBottle = 239,

	/**
     * 移除骷髅
     */
	SkullRemove = 240,

    /**
     * 下发首次礼包
     * 
     */
    PlayerFirstCharge = 241,
    /**
     * 连续登陆奖励
     */
    StartLogonTime = 242,
        /**
     * 是否是小队队长
     */
    IsTeamHead = 243,

	/**
     * 获取成长福利信息
     */
	GrowthWelfareInfo=264,
	
	/**
     * 获取成长福利奖励
     */
	GetGrowthWelfare=265,

    /**
     * 战力值改变
     */
    ChangeForceValue = 267,

	/**
     * 一键分解
     */
	EquipmentResolveAll = 268,
	
	/**
     * 一键出售
     */
	EquipmentSellAll = 269,

	/**
     * 临时队伍玩家退队
     */
	TempTeamPlayerRemove = 270,

	/**
     * 临时副本队长进入副本
     */
	TempTeamHeadGoMap = 271,

	/**
     * 临时小队队员改动
     */
	TempTeamPlayerChange = 272,

	/**
     * 临时小队解散
     */
	TempTeamDissolve=273,

	/**
     * 获取等级礼包信息
     */
	GetLevelPackInfo=274,
	
	/**
     * 获取等级礼包
     */
	GetLevelPack=275,

    ACTOR_LOGIN_SERVER = 700,
    MOVE_CLIENT = 801,
    PLAYER_MOVE_SERVER = 802,
    UNIT_MULTI_REFRESH_SERVER = 803,
    UNIT_REFRESH_SERVER = 804,
    CLIENT_ADD_TO_MAP = 810,
    CLIENT_ADD_TO_INSTANCE_MAP = 811,
    CAN_ADD_TO_INSTANCE_MAP = 812,
    CHANGE_MAP_STATE = 813,
    BROADCAST_USE_SKILL = 820,
	ATTACK_TARGET = 821,
    SET_MAX_HP = 822,
	SET_CUR_HP = 823,
	ADD_HP = 824,
	FORCE_MOVE = 825,
	IS_RUNNING = 826,
    IS_HANGUP = 827,
    MAP_PLAYER_COUNT = 830,
    LOADING_FINISHED_CLIENT = 833,
    HP_CHANGED = 840,
	/**
	 * 攻击怪物
	 */
    AttackMonster = 834,
	
	/**
	 * 被攻击
	 */
	BeAttacked= 835,
	
	/**
	 * 碰撞怪物碰撞体
	 */
	HitMonsterPoint= 836,
	
	/**
	 * 决策
	 */
	Decision=837,
	
	/**
	 * 转发决策
	 */
	DecisionForward=838,

	/**
	 * 特殊AI决策：撤退
	 */
	 DecisionFallBack=839,
		/**
	 * 清除怪物仇恨
	 */
	RemoveMosterHate = 850,

    /**
	 * 清空仇恨列表
	 */
	MonsterClearHate = 851,

    /**
     * 独立仇恨
     * 
     */
    MonsterHateList = 852,
	/**
	 * 怪物加血Buff
	 */
	MonsterHealthBuff=940,
	
	/**
	 * 怪物Buff导致死亡
	 */
	 MonsterDie=841,

	SERVER_NEW_DAY = 901,

    SYNC_ACT = 1101,
	LOGIN_LENOVO=1102,
	PayTest=1104,

    /// <summary>
    /// 客户端请求服务器增加体力，上线和挂机加体力使用本协议
    /// </summary>
    AddPower = 1105,
	/// <summary>
	/// Itools login
	/// </summary>
	iTools=1106,

    /// <summary>
    /// 宝藏九宫格初始显示信息（翻牌和开始抽奖的初始血石数量）
    /// </summary>
    Gamble_Info = 1107,
    
    /// <summary>
    /// 宝藏翻牌协议
    /// </summary>
    Gamble_Card = 1108,
    
    /// <summary>
    /// 宝藏抽奖协议
    /// </summary>
    Gamble_Lottery = 1109,
	/// <summary>
	///快用登录
	/// </summary>
	KY=1110,
	/// <summary>
	///XY登录
	/// </summary>
	XY=1111,
	/// <summary>
	///爱思sdk.
	/// </summary>
	I4=1112,
	HM = 1113,
	PP = 1114,
	TB = 1115,
	iosZsy = 1116,
	playerRide = 1201,
	playerTtile = 1202,
	logoutByGM = 1301,

    /// <summary>
    /// 修改玩家的精铁粉末和精金结晶的协议
    /// </summary>
    ModifyMarrow = 1302,
    /// <summary>
    /// PVP失败
    /// </summary>
    PVPisFall = 1303,

    /// <summary>
    /// iphone正版购买协议
    /// </summary>
    IphonePay = 1304,

    /// <summary>
    /// pvp弹出框取消
    /// </summary>
    PVPInviteIsNo = 1305,

    /// <summary>
    /// 用于统计被屏蔽不让注册的ios设备数量
    /// </summary>
    IphoneType = 1306,

     /// <summary>
    ///Home按键加入与解除
     /// </summary>
    HomeQueue = 1307,
	/// <summary>
	/// 购买月卡协议
	/// </summary>
	payCard = 1310,
    /// <summary>
    /// 领取连续登陆奖励
    /// </summary>
    NumberRechargeDay = 1311,  
	/// <summary>
	/// 购买月卡
	/// </summary>
	PaycardBuy = 1312,

    DUEL_INVITE = 1401,
    DUEL_GET_PLAYERS = 1402,
    DUEL_INVITE_ERROR = 1403,
    DUEL_INVITE_FEEDBACK = 1404,
    DUEL_RESULT = 1405,
    DUEL_MAP_LIST = 1406,
    RESET_HP = 1407,
    DUEL_STATE_CHANGE = 1408,
    TOWER_OPEN = 1500,
    TOWER_FLOOR_FINISH = 1501,
    TOWER_CHALLENGE = 1502,
    TOWER_REWARD = 1503,
    TOWER_FAILED = 1504,
	 GETRANKMONEY = 1600,
	 GETRANKForceValue = 1601,
	 GETRANKPVP = 1602,
    SMELT = 1700,
    SMELT_GET_NUM = 1701,
    Training_Clear = 1702,
    QuickTraining = 1703,
	/**
     * 购买精铁粉末
     */
	buyIron = 1710,
	/**
     * 购买买精金结晶
     */
	buyGold = 1711,
	HuntingMap=1712,
	ShowAllMONEY=1713,
	/**
     * 客户端显示体力的所有处理
     */
	 ShowStrength=1714,
	
	/**
     * 服务器扣除体力的所有逻辑
     */
	DeductStrength=1715,
    /// <summary>
    /// 客户端请求动态配置活动信息
    /// </summary>
    DynamicActivity = 1716,
	ShowTraining = 1717,
    /// <summary>
    /// 转职血石消耗
    /// </summary>
    ZhuanZhi = 1718,
    /// <summary>
    /// 视角选择
    /// </summary>
    ViewSelection = 1719,
	Playerduel = 1720,
	Addtili = 2008,
}
