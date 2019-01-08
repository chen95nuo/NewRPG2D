using UnityEngine;
using System.Collections;

namespace Zealm
{
	public enum ParameterType :short
	{
		TableName = 0,
		TableKey = 1,
		TableSql = 2,
		ItemID = 3,
		UserID = 4,
		UserPro = 5,
		UserNickName = 6,
		UserPwd = 7,
		UserPwdNew = 8,
		UserEmail = 9,
		UserNumber = 10,
		DeviceID = 11,
		PlayerType = 12,
		IsGetIP = 13,
		PicID = 14,
		ServerTime = 15,
		NumStart = 16,
		NumEnd = 17,
		NumRandom = 18,
		ServerName = 19,
		ServerIp = 20,
		ServerHost = 21,
		ServerNickName = 22,
		ServerActorNum = 23,
		ServerPeerID = 24,
		MessageType = 25,
		MessageText = 26,
		MessageBodys = 27,
		MessageSender = 28,
		MessageSenderID = 29,
		SetFriendType = 30,
		TeamName = 31,
		TeamID = 32,
		TeamHeadID = 33,
		TeamDepHeadID = 34,
		TeamLevel = 35,
		TeamMemver = 36,
		TeamInfo = 37,
		TeamRanking = 38,
		TeamPlayerMaxNum = 39,
		RequestType = 40,
		RetureRequestType = 41,
		TransactionID = 42,
		LegionType = 43,
		DataBeas = 44,
		MoneyType = 45,
		MailID = 46,
		MailTitle = 47,
		MailAddressee = 48,
		MailSender = 49,
		MailText = 50,
		MailTool1 = 51,
		isPaymentPickup = 52,
		Gold = 53,
		BloodStone = 54,
		MailType = 55,
		Fraction = 56,
		RankingType = 57,
		PlayerPurview = 58,
		HonorType = 59,
		Value = 60,
		HonorID = 61,
		MyTeamID = 62,
		OtherTeamID = 63,
		Time = 64,
		CorpType = 65,
		License = 66,
		PlayerBehaviorValue = 67,
		PlayerBehaviorType = 68,
		TitleType = 69,
		LogonPlayerID = 70,
		LegionOneID = 71,
		LegionOneMap = 72,
		LegionOnePlayerNum = 73,
		GameVersion = 74,
		LegionOneType = 75,
		LegionOneMapName = 76,
		RoomName = 77,
		IsReOnline = 78,
		IsWin = 79,
		RuntimePlatform = 80,
		BundleIdentifier = 81,
		BundleVersion = 82,
		ErrorInfo = 83,
		LangugeVersion = 84,
		Host = 85,
		PlayerLevel = 86,
		TableWhere = 87,
		RankTable = 88,
		MyRank = 89,
		RankRowName = 90,
		TableType = 91,
		PageNum = 92,
		Safety = 93,
		HoleID = 94,
		GemID = 95,
		StoreType = 96,
		SDK = 97,
		ActivityType = 98,
		ActivityRewardItmes = 99,
		ActivityID = 100,
		ActivityState = 101,
		BattlefieldID = 102,
		BattlefieldSpawns = 103,
		ActivityCanFinish = 104,
		BattlefieldOpenDoorTime = 105,
		FlagID = 106,
		hitValue = 107,
		ActivityBossHP = 108,
		ActivityBossStart = 109,
		BattlefieldEndTime = 110,
		ItemIDS = 111,
        /// <summary>
        /// 客户端类型（Android和iOS）
        /// </summary>
        PhoneType = 112,
        /// <summary>
        /// 精铁粉末
        /// </summary>
        MarrowIron = 113,
        /// <summary>
        /// 精金粉末
        /// </summary>
        MarrowGold = 114,
        /**
         * 是否使用血石（装备拆分时用作数据协议）
         */
        UseBlood = 115,

        GuildID = 116,
        GuildName = 117,
        //资金数量
        MoneyNumb = 118,

		x = 119,
		y = 120,
		z = 121,

        SolutionNum = 122,

        /// <summary>
        /// 上线和挂机加体力，请求服务器增加的体力值
        /// </summary>
        power = 123,

        /// <summary>
        /// 宝藏九宫格，每次翻牌消耗血石的数据协议
        /// </summary>
	    cardBlood = 124,
      
        /// <summary>
        /// 宝藏九宫格，开始抽奖消耗血石的数据协议
        /// </summary>
        lotteryBlood = 125,
        HeroStone = 126,
        /// <summary>
        /// 客户端购买商品的ID
        /// </summary>
        StoreID = 127,
		/// <summary>
		/// 只要是组队多给百分之20收益
		/// </summary>
		ShouYi = 128,
	}
}
