using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace yuan
{
    namespace YuanPhoton
    {
        public class CJohnIsShowLog
        {
            public bool IsShowLog()
            {
                return false;
            }

            public string GetGameConnectString()
            {
                return m_strGameConnect;

            }
            private string m_strGameConnect;
            public void SetGameConnectString(string _str)
            {
                m_strGameConnect = _str;

            }

        }

        /// <summary>
        /// 主协议
        /// </summary>
        public enum OperationCode
        {
            YuanDBGet = 3,
            YuanDBUpdate,
            Login,
            FastLogin,
            Logon,
            FastLogon,
            PlayerCreat,
            PwdGet,
            PwdUpdate,
            BindUserID,
            GetServerTime,
            RandomNum,
            GetServerActor,
            SendMessage,
            GetPlayers,
            DeletePlayer,
            SetID,
            GetPlayerList,
            SetFirend,
            GetPlayerID,
            TeamCreate,
            TeamAdd,
            GetTeam,
            GetMyTeam,
            GetMyTeamPlayers,
            Request,
            ReturnRequest,
            GropsCreate,
            GropsAdd,
            GetGrops,
            GropsRemove,
            TransactionRequest,
            GetTransactionInfo,
            SendTransactionID,
            SendTransactionInfo,
            TransactionClose,
            LegionTempCreate,
            LegionTempAdd,
            LegionDBCreate,
            LegionDBAdd,
            GetLegion,
            GuildCreate,
            GuildAdd,
            GetGuildAll,
            GetGuild,
            TeamPlayerRemove,
            GuildPlayerRemove,
            GetFavorableItem,
            GetItems,
            GetEquipment,
            BuyItem,
            GuildBuild,
            GuildFund,
            MailSend,
            MailGetOut,
            MailGetIn,
            MailRead,
            MailDelete,
            GetMailTool,
            GetActivity,
            SendRanking,
            GetDailyBenefits,
            TeamInviteAdd,
            GetPlayerTeamID,
            TeamPlayerUp,
            GuildPlayerPurview,
            SetHonor,
            SendPVPInfo,
            AddLegionPVPQueue,
            SendLegionPVPInfo,
            PVPCreate,
            PVPDissolve,
            SendTeamTeamInfo,
            SendTeamTeamLeave,
            TempTeamOK,
            TempTeamWait,
            AddTempTeam,
            RemoveTempTeam,
            GetTempTeam,
            GetMyGrop,
            GetMyLegion,
            RemoveLegion,
            TeamRemove,
            PVPInviteAdd,
            LegionInviteAdd,
            PVPPlayerUp,
            LegionPlayerUp,
            GuildInviteAdd,
            GuildRemove,
            GuildStopTalk,
            MailOtherGet,
            AddLegionQueue,
            SendLegionInfo,
            SetPlayerBehavior,
            SendTVMessage,
            SendBenefitsInfo,
            SetTitle,
            GetPVETeamList,
            TeamDissolve,
            GetPlayerGold,
            LegionOneAdd,
            LegionOneTeamAdd,
            LegionOneRemove,
            LegionOneList,
            GuildLevelUp,
            SendGameVersion,
            SendPVPOneInfo,
            PVPGO,
            LegionOneClose,
            HeartPage,
            InRoom,
            LeaveRoom,
            Login91,//登录91
            ActivityPVPAdd,
            ActivityPVPRemove,
            GetBlood91,
            ServerPlayerMax,
            OtherLogin,
            RedemptionCode,
            PVP1Invite,
            PVP1InviteRemove,
            InviteGoPVE,
            PVP1Fruit,
            SendError,
            BindDevice,
            OtherBindDevice,
            TeamHeadIn,
            LoginUC,//登录UC
            SetLicense,
            SendGMTV,
            LoginDL,//登录当乐
            RefershTable,
            RefershTableSome,
            LoginMI,//登录小米
            GetRank,
            LoginTSZ,//登录360
            LoginZSY,//登录中手游
            ClientCallServer,//客户端请求服务器
            GetRankOne,
            ClientBuyItem,
            ClientGetItemSome,
            ClientMoney,
            GetTableForID,
            GetPlayerForName,
            GetRankTopYT,
            GetRandomPlayer,
            GetSystemMail,
            GetMyMail,
            EquepmentBuild,//装备强化
            EquepmentHole,//装备打孔
            EquepmentMosaic,//装备镶嵌
            Training,//训练
            TrainingSave,//训练保存
            EquepmentProduce,//装备打造
            GetStoreList,//获取商店列表
            BuyStoreClient,//购买商店物品
            GetRandomItem,//获取随机物品
            PlayerInMap,//玩家所在地图
            AddExperience,//增加经验值
            GetTablesSomeForIDs,//根据ids获取表内某些字段数据
            PlayerLook,
            LoginOPPO,//登录OPPO
            DoneCard,//副本翻牌
            OpenCard,//副本获取牌
            SendPacks,//发送礼包
            DoneCardPVP,//PVP翻牌
            IsSaveDate,
            ServerPlayerOut,//服务器踢出其他玩家
            LoginZSYAll,//登录中手游联运SDK
            ActivityFirstCharge,//首充活动
            ActivityGetInfo,//获取活动内容
            ActivityGetReward,//领取活动奖励
            GetClientParms,//获取客户端同步参数
            GetFirstPacks,//获得新手礼包
            UseItem,//使用道具
            TaskCompleted,//完成任务
            GetServerList,//获取服务器列表
            HangUpAddExp,//挂机加经验
            SetFirstServer,//设置优先服务器
            AuctionCompany,//拍卖行主协议
            /// <summary>
            /// 奖池主协议
            /// </summary>
            JackPot,
            /// <summary>
            /// 任务主协议
            /// </summary>
            Task,
            /// <summary>
            /// 活动类主协议
            /// </summary>
            Activity,
            /// <summary>
            /// GM类主协议
            /// </summary>
            GM,
            Firends,
            /// <summary>
            /// 验证类主协议
            /// </summary>
            Validation,
            /// <summary>
            /// 使用金钱主协议
            /// </summary>
            UseMoney,

            /// <summary>
            /// 活动触发协议
            /// </summary>
            TriggerActivity,
            JoinActivity,
            CanFinishActivity,
            FinishActivity,

            BattlefieldReady,
            BattlefieldSpawnInfo,
            BattlefieldInfo,
            BattlefieldKill,
            BattlefieldDie,
            BattlefieldGetFlag,
            BattlefieldHitBoss,
            BattlefieldExit,
            ActivityBossResult,
            ActivityBoosHP,
            ActivityBossDamage,
            ActivityJoinSuccess,
            /// <summary>
            /// 联运支付信息的协议
            /// </summary>
            Payinformation,

            PayGameRole,//客户端请求支付协议；

			//todo battle,activity协议整合,协议命名
            BattlefieldResult,
            /// <summary>
            /// GM查询
            /// </summary>
            GMSearch,
            BattlefieldScoreInfo,
            payBack,//支付成功后，修改数据库内容sign字段为1；
            BattlefieldBossInfo,
            BattlefieldResultBoss,
            BattlefieldInfoBoss,
           /// <summary>
           /// 数据库所对应的那条支付数据id
           /// </summary>
            payID,
            /// <summary>
            /// 添加血石成功之后，传递给登录服务器，修改内存
            /// </summary>
            payInfo,
            /// <summary>
            /// 购买栏位的协议；
            /// </summary>
            payLanwei,
            /// <summary>
            /// 服务器返回角色栏位的数量
            /// </summary>
            lanweiNumber,
            /// <summary>
            /// 修改角色栏位数量的协议
            /// </summary>
            correctLanwei,
            /// <summary>
            /// 中手游的需求日志协议
            /// </summary>
            LogInfo,
            /// <summary>
            /// 装备拆分
            /// </summary>
            EquipmentResolve,
            /// <summary>
            /// 在线宝箱
            /// </summary>
            OnlineChests,
        }

        public enum OnlineChestsParams
        {
            /// <summary>
            /// 在线宝箱当天累计时间
            /// </summary>
            OnlineChestsTime,
            /// <summary>
            /// 当天已经打开的宝箱数量
            /// </summary>
            OpenedChests,
        }

        /// <summary>
        /// 装备拆分数据协议
        /// </summary>
        public enum EquipmentResolveParams
        { 
            /// <summary>
            /// 装备id
            /// </summary>
            ItemID,
            /// <summary>
            /// 使用血石
            /// </summary>
            UseBlood,
        }

        public enum LogInfoParams
        {
            /// <summary>
            /// 设备ID
            /// </summary>
            PHONEID = 128,
            /// <summary>
            /// 帐号ID
            /// </summary>
            ACCID,
            /// <summary>
            /// 帐号名称
            /// </summary>
            ACCNAME,
            /// <summary>
            /// 渠道
            /// </summary>
            CHANNELID,
            /// <summary>
            /// 角色id
            /// </summary>
            ROLEID,
            /// <summary>
            /// 角色名
            /// </summary>
            ROLENAME,
            /// <summary>
            /// 级别
            /// </summary>
            LEVEL,
            /// <summary>
            /// 元宝数
            /// </summary>
            IMONEY,
            /// <summary>
            /// 充值人民币金额
            /// </summary>
            RMB,
            /// <summary>
            /// 充值时间
            /// </summary>
            TIME,
            /// <summary>
            /// 当前余额(元宝余额)
            /// </summary>
            BALANCE,
            /// <summary>
            /// 金币
            /// </summary>
            MONEY,
            /// <summary>
            /// 创建时间
            /// </summary>
            CRAETETIME,
            /// <summary>
            /// 最后活动时间
            /// </summary>
            LASTTIME,
            /// <summary>
            /// 玩家IP地址
            /// </summary>
            IP,
            /// <summary>
            /// 客户端类型（Android和iOS）
            /// </summary>
            PHONETYPE,
            /// <summary>
            /// 玩家手机号
            /// </summary>
            PHONENUMBER,
            /// <summary>
            /// 游戏版本号
            /// </summary>
            GAMEVERSION,
            /// <summary>
            /// 角色性别
            /// </summary>
            ROLEGENDER,
            /// <summary>
            /// 角色信息
            /// </summary>
            ROLEINFO,
        }

        /// <summary>
        /// 使用金钱数据协议
        /// </summary>
        public enum UseMoneyParams
        {
            /// <summary>
            /// 使用道具类型
            /// </summary>
            UseMoneyType = 0,
            Num1 = 1,
            Num2 = 2,
            ItemID = 3,
            /// <summary>
            /// 金币
            /// </summary>
            GoldNum,
            /// <summary>
            /// 血石
            /// </summary>
            BloodNum,
            /// <summary>
            /// 返回表键
            /// </summary>
            TableKey,
            /// <summary>
            /// 返回表值
            /// </summary>
            TableSql,
        }

        /// <summary>
        /// 验证类子协议
        /// </summary>
        public enum ValidationType
        {
            /// <summary>
            /// 登陆验证
            /// </summary>
            Login,
        }

        /// <summary>
        /// 验证类数据协议
        /// </summary>
        public enum ValidationParams
        {
            /// <summary>
            /// 验证类子协议
            /// </summary>
            ValidationType,
            /// <summary>
            /// 包验证码
            /// </summary>
            KeyStore,

        }

        /// <summary>
        /// 好友类子协议
        /// </summary>
        public enum FirendsType
        {
            /// <summary>
            /// 请求加好友
            /// </summary>
            FirendsAddInvit,
            /// <summary>
            /// 返回加好友请求
            /// </summary>
            RetrunFirendsAddInvit,
            /// <summary>
            /// 接受请求加好友信息
            /// </summary>
            GetFirendsAddInvitInfo,
            /// <summary>
            /// 接受返回请求加好友信息
            /// </summary>
            GetRetrunFirendsAddInvitInfo,
            /// <summary>
            /// 请求加好友从玩家名
            /// </summary>
            FirendsAddInvitForName,
        }

        /// <summary>
        /// 好友类数据协议
        /// </summary>
        public enum FirendsParams
        {
            /// <summary>
            /// 好友类子协议
            /// </summary>
            FirendsType,
            /// <summary>
            /// 好友ID
            /// </summary>
            FirendID,
            /// <summary>
            /// 好友名称
            /// </summary>
            FirendName,
            /// <summary>
            /// 返回表键
            /// </summary>
            TableKey,
            /// <summary>
            /// 返回表值
            /// </summary>
            TableSql,
            /// <summary>
            /// 返回应答类型
            /// </summary>
            RetrunType,
        }

        /// <summary>
        /// 邮件类子协议
        /// </summary>
        public enum MailType
        {
            Out,
            In,

        }

        /// <summary>
        /// GM类子协议
        /// </summary>
        public enum GMType
        {
            /// <summary> 
            /// GM回复邮件
            /// </summary>
            GMSendMail,
            /// <summary>
            /// 获取邮件列表
            /// </summary>
            GMMailGetList,
            /// <summary>
            /// GM登录
            /// </summary>
            GMLogin,
            /// <summary>
            /// 被其他GM挤下线
            /// </summary>
            GMOtherLogin,
            /// <summary>
            /// 搜索玩家列表
            /// </summary>
            GMSearchPlayers,
            /// <summary>
            /// 发送滚屏消息
            /// </summary>
            GMSendTVMessage,
            /// <summary>
            /// 发送礼包
            /// </summary>
            GMSendPack,
        }


        /// <summary>
        /// GM数据协议
        /// </summary>
        public enum GMParams
        {
            /// <summary>
            /// 邮件类型
            /// </summary>
            GMType,
            /// <summary>
            /// 账号ID
            /// </summary>
            UserID,
            /// <summary>
            /// 账号密码
            /// </summary>
            UserPwd,
            /// <summary>
            /// 账号名称
            /// </summary>
            UserName,
            /// <summary>
            /// 分区服务器标示
            /// </summary>
            ServerID,
            /// <summary>
            /// 搜索类型
            /// </summary>
            SearchType,
            /// <summary>
            /// 搜索内容
            /// </summary>
            SearchValue,
            /// <summary>
            /// 服务器列表
            /// </summary>
            ServerList,
            /// <summary>
            /// 特指GM账号ID
            /// </summary>
            GMUserID,
            /// <summary>
            /// 通告内容
            /// </summary>
            TVText,
            /// <summary>
            /// 道具ID
            /// </summary>
            ItemID

        }

        /// <summary>
        /// 搜索类型
        /// </summary>
        public enum SearchType
        {
            /// <summary>
            /// 角色ID
            /// </summary>
            PlayerID,
            /// <summary>
            /// 角色名称
            /// </summary>
            PlayerName,
            /// <summary>
            /// 通行证ID
            /// </summary>
            UserID,
            /// <summary>
            /// 角色等级
            /// </summary>
            PlayerLevel,
            /// <summary>
            /// 角色职业
            /// </summary>
            PlayerPro,
        }

        /// <summary>
        /// 活动类子协议
        /// </summary>
        public enum ActivityType
        {
            ActivityInfo,//活动内容
            ActivityTime,//活动时间
            ChargeDays, //累计充值天数q
            ActivityReward,//活动奖励
            ActivityLevel,//冲级活动
            /// <summary>
            /// 奖池抽奖
            /// </summary>
            JockPotLottery,
            /// <summary>
            /// 获取奖池信息
            /// </summary>
            JockPotShowInfo,
            /// <summary>
            /// GM修改奖池参数
            /// </summary>
            JockPotGMSet,
            /// <summary>
            /// GM向奖池加注
            /// </summary>
            JockPotGMAddBlood,
        }

        /// <summary>
        /// 活动数据协议
        /// </summary>
        public enum ActivityParams
        {
            /// <summary>
            /// 活动子协议
            /// </summary>
            ActivityType,
            /// <summary>
            /// 返回表键
            /// </summary>
            TableKey,
            /// <summary>
            /// 返回表值
            /// </summary>
            TableSql,
            /// <summary>
            ///血石数量
            /// </summary>
            BloodNum,
            /// <summary>
            /// 抽奖次数
            /// </summary>
            LotteryTimes,
            /// <summary>
            /// 奖池奖金(血石)
            /// </summary>
            PoolBlood,
            /// <summary>
            /// 抽奖一次需要的血石
            /// </summary>
            NeedBlood,
            /// <summary>
            /// 抽成比例
            /// </summary>
            Commission,
            /// <summary>
            /// 中奖几率
            /// </summary>
            LotteryOdds,
            /// <summary>
            /// 开关
            /// </summary>
            IsEnable,
            /// <summary>
            /// 金币数量
            /// </summary>
            GoldNum,
            /// <summary>
            /// 奖池奖金(金币)
            /// </summary>
            PoolGold,
            /// <summary>
            /// 抽奖一次需要的金币
            /// </summary>
            NeedGold,
            /// <summary>
            /// 奖池类型（0为金币，1为血石）
            /// </summary>
            PoolType,
            /// <summary>
            /// 改变金币
            /// </summary>
            ChangeGold,
            /// <summary>
            /// 改变血石
            /// </summary>
            ChangeBlood,
        }

        /// <summary>
        /// 任务子协议
        /// </summary>
        public enum TaskType
        {
            /// <summary>
            /// 接受任务方法
            /// </summary>
            TaskAcceptedAsID,
            /// <summary>
            /// 达成任务所需条目
            /// </summary>
            TaskAddNumsAsID,
            /// <summary>
            /// 放弃任务方法
            /// </summary>
            TaskGiveUpAsID,
        }

        /// <summary>
        /// 任务数据协议
        /// </summary>
        public enum TaskParams
        {
            /// <summary>
            /// 任务子协议
            /// </summary>
            TaskType,
            /// <summary>
            /// 任务ID
            /// </summary>
            TaskID,
            /// <summary>
            /// 返回表键
            /// </summary>
            TableKey,
            /// <summary>
            /// 返回表值
            /// </summary>
            TableSql,
        }




        /// <summary>
        /// 拍卖行子协议
        /// </summary>
        public enum AuctionCompanyType
        {
            /// <summary>
            /// 一口价拍卖
            /// </summary>
            FixedPriceAuction,
            /// <summary>
            /// 拍卖搜索
            /// </summary>
            AuctionSearch,
            /// <summary>
            /// 购买拍卖品
            /// </summary>
            BuyAuctions,
            /// <summary>
            /// 玩家的拍卖信息
            /// </summary>
            PlayerAuctionInfo,
            /// <summary>
            /// 购买拍卖次数
            /// </summary>
            BuyAuctionSlot,
        }

        /// <summary>
        /// 拍卖行数据协议
        /// </summary>
        public enum AuctionCompanyParams
        {
            /// <summary>
            /// 拍卖行子协议
            /// </summary>
            AuctionCompanyType,
            /// <summary>
            /// 物品ID和数量
            /// </summary>
            ItemIDAndCount,
            /// <summary>
            /// 一口价的固定价格
            /// </summary>
            FixedPrice,
            /// <summary>
            /// 竞拍时长
            /// </summary>
            AuctionTime,
            /// <summary>
            /// 物品最小等级
            /// </summary>
            MinLvl,
            /// <summary>
            /// 物品最大等级
            /// </summary>
            MaxLvl,
            /// <summary>
            /// 物品品质
            /// </summary>
            ItemQuality,
            /// <summary>
            /// 装备类型
            /// </summary>
            EquipType,
            /// <summary>
            /// 材料类型
            /// </summary>
            MatType,
            /// <summary>
            /// 购买的拍卖品的id,即表的主键
            /// </summary>
            AuctionID,
            /// <summary>
            /// 一次性可拍卖的次数
            /// </summary>
            AuctionCount,
            /// <summary>
            /// 剩余拍卖次数
            /// </summary>
            UsedAuctionCount,
            /// <summary>
            /// 一次所购买的拍卖位数量
            /// </summary>
            AuctionSlotCount,
        }

/*
        /// <summary>
        /// 购买子协议
        /// </summary>
        public enum PayinfoType
        {
            BuyID,
            Qudao,
        }
*/
        /// <summary>
        /// 携带栏位数量协议
        /// </summary>
        public enum lanweiNumber
        {
            
            itemID,
        }

        /// <summary>
        /// 购买数据协议
        /// </summary>
        public enum PayinfoParams
        {
            BuyID,
            Qudao,
        }

        /// <summary>
        /// 购买数据协议
        /// </summary>
        public enum PayLanWeiParams
        {
            BuyID,
            Qudao,
            /// <summary>
            /// 点击购买栏位的时候，传递当前用户名的协议；
            /// </summary>
            userName,
            userID,
            order,
            /// <summary>
            /// 此id为支付订单内存的id，支付成功后根据id在数据库修改sign值为1；
            /// </summary>
            ID,
            peerId,
           
            
        }
        /// <summary>
        /// 购买信息请求参数返回协议
        /// </summary>
        public enum PayinfoParamsBack
        {
            propOne,
            propTwo,
            propThree,
            propFour,
            propFive,
            peerID,
			propSix,

        }

        public enum StoreType
        {
            Blacksmith = 0,//铁匠
            GroceryStore,//杂货店
            GuildStore,//公会商店
            HonorStore,//荣誉商店
            PVPStore,//PVP商店
            RandomStore,//随机商店
        }

        //!玩家登录状态
        public enum PlayerLoginState
        {
            //!离线
            OffLine = 0,
            //!登录成功
            LoginOk = 1,
        }

        public enum PlayerType
        {
            Robber = 0,
            Master,
            Soldier,
        }

        public enum MessageType
        {
            All = 0,
            Somebody,
            Team,
            Guild,
            System,
        }

        public enum ReturnCode
        {
            Yes = 0,
            No,
            Nothing,
            Error,
            HasID,
            HasNickName,
            HasEmail,
            HasRegister,
            HasDevice,
            HasTeam,
            PlayerNumMax,
            GetServer,
            NoBloodStone,
            NoGold,
            NoInventory,
            Create,
            Join,
            NeedLicense,
            ZSYBack,
            /// <summary>
            /// 已完成
            /// </summary>
            IsDone,
            /// <summary>
            /// 是本身
            /// </summary>
            IsMine,
            /// <summary>
            /// 已经登陆
            /// </summary>
            IsLogin,
            /// <summary>
            /// 其他玩家登陆，被挤下线
            /// </summary>
            OtherLogin,
            /// <summary>
            /// 没有相应服务器
            /// </summary>
            NoServer,
            /// <summary>
            /// 没有空位(用于角色栏位和拍卖次数)
            /// </summary>
            NoSlot,
            /// <summary>
            /// 精铁粉末材料不够
            /// </summary>
            NoMarrowIron,
            /// <summary>
            /// 精金粉末材料不够
            /// </summary>
            NoMarrowGold,
            /// <summary>
            /// 本日金币建设次数满
            /// </summary>
            NoNum,
            /// <summary>
            /// 金币建设CD没好(5分钟)
            /// </summary>
            NoTime,
            NOHeroStone=31,
            NOConquerStone=32,
            /// <summary>
            ///  此商品id存在
            /// </summary>
            NOStoreID = 33,
			isReceiveCardreward=34,

			/**
     * 临时队伍有人加入
     */
			TempTeamNewPlayerAdd = 35,
			
			/**
     * 队长已进入地图，是否跟随加入
     */
			TempTeamHeadGoMap = 36,

			/**
     * 队长可以进入
     */
			HeadYes = 37,

			YesDo = 38,

        }

        public enum ParameterCode
        {
            Update = 0,
            GetTable = 1,
        }

        public enum ErrorParameterCode
        {
            TableName = 0,
            ErrorText = 1,
        }

        public enum ParameterType
        {
            TableName = 0,
            TableKey = 1,
            TableSql,
			ItemID,
            UserID,
            UserPro,
            UserNickName,
            UserPwd,
            UserPwdNew,
            UserEmail,
            UserNumber,
            DeviceID,
            PlayerType,
            IsGetIP,
            PicID,
            ServerTime,
            NumStart,
            NumEnd,
            NumRandom,
            ServerName,
            ServerIp,
            ServerHost,
            ServerNickName,
            ServerActorNum,
            ServerPeerID,
            MessageType,
            MessageText,
            MessageBodys,
            MessageSender,
            MessageSenderID,
            SetFriendType,
            TeamName,
            TeamID,
            TeamHeadID,
            TeamDepHeadID,
            TeamLevel,
            TeamMemver,
            TeamInfo,
            TeamRanking,
            TeamPlayerMaxNum,
            RequestType,
            RetureRequestType,
            TransactionID,
            LegionType,
			DataBeas,
            MoneyType,
            MailID,
            MailTitle,
            MailAddressee,
            MailSender,
            MailText,
            MailTool1,
            isPaymentPickup,
            Gold,
            BloodStone,
            MailType,
            Fraction,
            RankingType,
            PlayerPurview,
            HonorType,
            Value,
            HonorID,
            MyTeamID,
            OtherTeamID,
            Time,
            CorpType,
            License,
            PlayerBehaviorValue,
            PlayerBehaviorType,
            TitleType,
            LogonPlayerID,
            LegionOneID,
            LegionOneMap,
            LegionOnePlayerNum,
            GameVersion,
            LegionOneType,
            LegionOneMapName,
            RoomName,
            IsReOnline,
            IsWin,
            RuntimePlatform,
            BundleIdentifier,
            BundleVersion,
            ErrorInfo,
            LangugeVersion,
            Host,
            PlayerLevel,
            TableWhere,
            RankTable,
            MyRank,
            RankRowName,
            TableType,
            PageNum,
            Safety,//保险
            HoleID,//装备孔ID
            GemID,//宝石ID
            StoreType,//商店类型
            SDK,//渠道类型
            ActivityType,//活动类型
            ActivityRewardItmes,//领取的奖励物品
            ActivityID,
            ActivityState,
            BattlefieldID,
            BattlefieldSpawns,
            ActivityCanFinish,
            BattlefieldOpenDoorTime,
            FlagID,
            hitValue,
            ActivityBossHP,
            ActivityBossStart,
            BattlefieldEndTime,
            /// <summary>
            /// 客户端类型（Android和iOS）
            /// </summary>
            PhoneType,
            /// <summary>
            /// 精铁粉末
            /// </summary>
            MarrowIron,
            /// <summary>
            /// 精金粉末
            /// </summary>
            MarrowGold,
        }

        public enum BattlefiledType
        {
            PlayerCount = 200,
            RedTeamScore,
            BlueTeamScore,
            RedTeamFlag,
            BlueTeamFlag,
            
            RedTeamBossHP,
            BlueTeamBossHP,
            winTeamLabel,
        }

        /// <summary>
        /// 客户端同步参数
        /// </summary>
        public enum ClientParmsType
        {
            ServiceMianCtiy,//主城数据同步频率
            ServiceDuplicate,//副本同步频率
            ServicePVP,//PVP同步频率
            ClientParms,//预备参数
        }

        public enum TableType
        {
            GuildInfo,
            Corps,
            PlayerInfo,
        }

        public enum LegionOneType
        {
            City = 0,
            Out = 1,
        }


        public enum HonorType
        {
            LevelReach = 0,//等级达到
            ComTaskNum,//完成任务个数
            ComTask,//完成任务
            WinSoldierNum,//战胜战士
            WinRobberNum,//战胜游侠
            WinMasterNum,//战胜法师
            PerfectVS,//完胜
            NoseVS,//险胜
            Rank,//取得军衔
            GetEquipItem,//获得装备
            FishingNum,//钓鱼成功次数
            CookingNum,//烹饪成功次数
            MiningNum,//采矿成功次数
            MakeNum,//制造装备成功次数
            ServerON1VIP,//服务器第一达到VIP等级
            ServerON1PlayerLevel,//服务器第一达到角色等级
            ServerON1VIPMoney,//服务器第一达到充值金额
            ServerON1Arena,//服务器第一达到竞技场
            ServerON1Rank,//服务器第一达到军衔
            ServerON1Guild,//服务器第一达到公会
			TowerNum,
        }

        public enum TitleType
        {
            PVEPoint = 0,//声望值
            PVPPoint,//荣耀值
            HonorPoint,//成就点数
            LegionPoint,//竞技场点数
            LegionRank,//竞技场排名
            VIP,//VIP等级
            GetHonor,//获得成就
        }

        public enum MoneyType
        {
            Gold,
            BloodStone,
        }

        public enum LegionType
        {
            DBLegion = 10,
            TempLegion,
        }


        /// <summary>
        /// 需要对方确认协议
        /// </summary>
        public enum RequstType
        {
            TeamAdd = 0,
            ReturnTeamAdd,
            GropsAdd,
            ReturnCropsAdd,
            LegionTempAdd,
            ReturnLegionTempAdd,
            LegionDBAdd,
            ReturnLegionDBAdd,
            TransactionRequest,
            ReturnTransactionRequest,
            TransactionOK,
            GuildAdd,
            ReturnGuildAdd,
            TeamInviteAdd,
            ReturnTeamInviteAdd,
            PVPInviteAdd,
            ReturnPVPInviteAdd,
            LegionInviteAdd,
            ReturnLegionInviteAdd,
            GuildInviteAdd,
            ReturnGuildInviteAdd,
            PVP1Invite,
            ReturnPVP1Invite,
            InviteGoPVE,
            ReturnInviteGoPVE,
        }

        public enum TransactionInfo
        {
            ItemID = 0,
            BloodStone,
            Golds,
            IsReady,
            TransactionID,
            IsTransaction,

        }

        public enum SetFirendType
        {
            AddPlayer = 0,
            BlackPlayer,
        }

        public enum EventCode
        {
            BeOffline = 0,
            SendMessage,
            SendTVMessage,
        }

        public enum RankingType : int
        {
            VIP = 0,//VIP
            Level,//等级
            Duplicate,//副本
            Abattoir,//角斗场
            Arena,//竞技场
            Battlefield,//战场
            Objective,//成就
            Guild,//公会
            PVP2,//竞技场2v2
            PVP4,//竞技场4v4
            Rank,//军衔
            Exp,//经验值
        }

        public enum CorpType
        {
            PVP2 = 0,
            PVP4,
        }

        public enum PlayerBehaviorType
        {
            Gold = 0,//金钱变更
            Blood,//血石变更
            GetItem,//获得装备
            OutItem,//支出装备
            KillEnemy,//杀死敌人
            Death,//死亡
            GetHonor,//获得成就
            Online,//登录
            Offline,//下线
            Transaction,//交易
            UpLevel,//升级
            ServerBuy,//商城商品购买
            ClientBuy,//客户端商品购买
            BloodSolution,//使用血瓶
            BloodRevival,//血石复活
            AutoPlay,//托管
            Raids,//扫荡
            Strengthen,//强化
            Training,//训练
            Gamble,//抽奖
            GameFunction,//打开功能
            GameSchedule,//游戏进度
            Transformation,//物品转化
            AddExp,//增加经验值
        }

        public enum GameScheduleType
        {
            OpenGame = 0,//打开游戏
            Logon,//创建账号
            Login,//登录
            CreatePlayer,//创建人物
            StartGame,//进入游戏
            StartTraining,//进入训练关卡
            StartMap111,//进入新手营地
            ClickGOBtn,//点击GO按钮
            AcceptFirstTask,//接受第一个任务
        //start===========新手引导教程===================
            /// <summary>
            /// 第一次进入游戏，强制点GO
            /// </summary>
            FirstClickGO,
            /// <summary>
            /// 点击接受第一个任务
            /// </summary>
            AcceptTask,
            /// <summary>
            /// 点击继续按钮
            /// </summary>
            ContinueBtn,
            /// <summary>
            /// 点击接受按钮
            /// </summary>
            AcceptBtn,
            /// <summary>
            /// 点击完成第一个任务
            /// </summary>
            CompleteTask,
            /// <summary>
            /// 点击完成任务按钮
            /// </summary>
            CompleteBtn,
            /// <summary>
            /// 点击NPC对话按钮
            /// </summary>
            TalkBtn,
        //end===========新手引导教程===================
            /// <summary>
            /// 房间服连接失败
            /// </summary>
            PhotonRoomConFail,
        }

        public enum GameFunction
        {
            GameShop = 0,//游戏商店
            ServerShop,//服务器商店(商城)
            Mounts,//坐骑
            StudySkill,//学习技能
            ProChange,//职业转换
            StudySkillBiased,//学习技能偏向
            RefreshSkillPoint,//洗技能点
            RefreshTalentPoint,//洗天赋点
            OpenInv,//开启包裹位
            OpenBankInv,//开启仓库位
            UpdateEquipBtnOpne,//开启强化按钮
            UpdateEquip,//装备强化
            UpdateEquipBlood,//使用血石强化装备
            UpdaetEquipRefreshCool,//冷却装备强化时间
            GemBtnOpen,//开启宝石按钮
            EquipPunch,//成功装备打孔
            EquipMosaic,//成功镶嵌宝石
            OpenMakeBtn,//开启制造按钮
            EquipMake,//成功制造装备
            QueuePVP1,//排队单人角斗场
            OpenOfflinePlayerBtn,//开启影魔挑战按钮
            QueuePVP8,//排队多人战场
            RefreshPVP1Num,//重置单人角斗场可进入次数
            RefreshOfflinePlayerNum,//重置影魔挑战可进入次数
            RefreshPVP8Num,//重置多人战场可进入次数
            OpenMakeSoulBtn,//开启炼魂按钮
            MakeSoulBtn,//使用炼魂按钮功能
            OpenMoreMuisek,//开启更多携带魔魂位
            EatSoul,//吞噬灵魂
            LevelUpMuisek,//升级灵魂精华
            EatMuisek,//熔碎灵魂精华
            OpenCookBtn,//开启烤鱼按钮
            Training1,//使用平民训练
            Training2,//使用士兵训练
            Training3,//使用勇士训练
            Training4,//使用骑士训练
            Training5,//使用督军训练
            AutoPlay,//使用自动打怪
            BuyAutoPlay,//购买自动打怪时间
            DuplicateFlop,//副本翻牌
            DuplicateRaids,//扫荡
            DuplicateRaidsNow,//使用立即完成扫荡副本
            FightingOut,//使用战斗中返回基地
        }



        public enum ConsumptionType
        {
            CreatePlayer = 0,//创建角色
            DeletePlayer,//删除角色
            Online,//登录账号
            PwdUpdate,//修改密码
            GameSchedule,//游戏进度
        }


        /// <summary>
        /// 客户端相关验证数据
        /// </summary>
        public enum BenefitsType
        {
            Salaries = 0,//每日薪金
            Rank,//军衔奖励
            Guild,//公会分红
            TVMessageBlood,//小喇叭花费血石
            HangUpExp,//挂机经验倍乘
            LogonStatus,//服务器注册权限
            PlayerInvite,//玩家邀请开关
            InviterGold,//邀请者奖励金币数
            InviterBlood,//邀请者奖励血石数
            InviteesGold,//被邀请者奖励金币数
            InviteesBlood,//被邀请者奖励血石数
            SystemInfo,//系统通告
            LegionWaitTime,//战场等待时间（分钟）
            LegionTime,//战场进行时间（分钟）
            GuildLevelUp,//公会升级
            GameVersion,//游戏版本
            EverydayAims,//每日目标奖励
            MakeGoldTime,//炼金加成时间段
            MakeGoldScale,//炼金固定比例
            MakeGoldAddtion,//炼金加成
            LegionStartNum,//战场开始人数
            PlayerMaxLevel,//玩家最大等级
            DataVersion,//数据版本
            ServerPlayerMax,//服务器是否达到最大人数
            Yuan91SDKBuy,//91购买开关
            GameLanguage,//游戏语言
            BloodTarn,//血石交易
            OfflinePlayerSwitch,//挑战影魔开关
            GambleSwitch,//宝藏开关
            EqupmentBuildSwitch,//制造开关
            EqupmentHoleSwitch,//宝石开关
            MailSwitch,//邮件开关
            TransactionSwitch,//交易开关
            AutoPlaySwitch,//托管开关
            PVPSwitch,//PVP开关
            InviteGoPVESwitch,//邀请至副本开关
            InvitePVP1Switch,//邀请决斗开关
            HeroPVESwitch,//英雄副本开关
            MakeSoulSwitch,//炼魂开关
            PVP4Switch,//4v4（战场军团）开关
            PVP2Switch,//2v2(竞技场战队)开关
            GuildSwitch,//公会开关
            PetSwitch,//坐骑开关
            PetBuySwitch,//宠物购买开关
            RedemptionCodeSwitch,//兑换码开关
            EqupmentUpdateSwitch,//强化开关
            CookSwitch,//烤鱼开关
            TrainingSwitch,//训练开关
            PlayerMaxNum,//最大玩家数
            FirstServer,//优先服务器
            /// <summary>
            /// 包验证密钥
            /// </summary>
            KeyStore,
            /// <summary>
            /// 七日奖励，七日奖励所达奖励要求的总天数
            /// </summary>
            DailyBenefits = 51,
            /// <summary>
            /// 七日奖励已领取奖励的天数
            /// </summary>
            CanDailyBenefits = 52,
        }

        public enum DailyBenefitsParams
        {
            /// <summary>
            /// 七日奖励所达奖励要求的总天数
            /// </summary>
            DailyBenefits,
            /// <summary>
            /// 七日奖励已领取奖励的天数
            /// </summary>
            CanDailyBenefits,
        }

        public enum EverydayAimsType
        {
            AimLogin = 0,//登陆游戏
            AimGetLogin,//领取登陆奖励
            AimOpenBox,//开启宝箱
            AimTrain,//训练
            AimUpdateEquip,//强化装备
            AimFishing,//钓鱼
            AimMining,//挖矿
            AimCooking,//烹饪
            AimMakeSoul,//炼魂
            AimFinshTaskNum,//完成任务数量
            AimFinshMission,//打通副本
            AimTeamMission,//组队副本
            AimHeroMission,//英雄副本
            AimPVP1,//角斗
            AimPVP8,//战场
            AimPVPN,//金矿战
            AimPVP24,//竞技场
			AimActivity,
			AimBuyStore,
			AimOfflinePlayer,
			AimMakeGold,
			AimPay,
        }

        public enum GetType
        {
			HP = 0,
			Item,
			Gold,
            BloodStrone,
        }

        public enum GuildLevelUp
        {
            Level = 0,
            Build,
            Fund,
        }

        /// <summary>
        /// 使用血石金币类型
        /// </summary>
        public enum UseMoneyType
        {
            Sell1 = 0,									//卖出物品1
            WashAttributePoints = 1,				//洗技能点						
            SaveBranch = 2,							//保存转职
            YesBuyCard = 3,							//副本翻牌
            YesDoCD = 4,								//重置铁毡冷却
            Sell = 5,										//卖出物品
            Buy = 6,										//买入物品
            YesOpenCangKu = 7,					//开启仓库格子
            CostGold = 8,								//花费金币
            OpenSpreeItemAsID = 9,				//打开礼包
            YesOpenPackage = 10,				//打开道具包
            YAddGold = 11,							//增加金币
            YAddBlood = 12,							//增加血石
            YesPVP8Times = 13,					//重置8人pvp
            YesPVPTimes = 14,						//重置pvp
            Resurrection2 = 15,						//复活2
            Resurrection3 = 16,						//复活3
            mmm31 = 17,								//内购1
            mmm32 = 18,								//内购2
            mmm33 = 19,								//内购3
            mmm34 = 20,								//内购4
            UpDateOneSkill = 21,					//升级技能
            UpDateOneSoulItemSkill = 22,		//升级魔魂技能
            ButtonBuildSoulAndDigest = 23,	//生产魔魂&灵魂精华
            NumDigestButtonsPlus = 24,			//开启灵魂精华包裹
            PlusBottle = 25,							//增加药水
            ApplyPickup1 = 26,						//拾取1
            ApplyPickup2 = 27,						//拾取2
            TaskDone = 28,							//完成任务
            SpendingReturns11 = 29,				//花费返回
            RaidsStart = 30,							//扫荡开始
            CostRaidsNow = 31,					//立即完成扫荡
            Cost = 32,									//花费
            UpLevelBottle = 33,						//升级药水等级
            PlusBottle1 = 34,							//增加药水1
            FullBottle = 35,							//补满药水
            YesReturn = 36,							//确认返回
            YesManufacture = 37,					//打开制造按钮
            YesSoul = 38,								//打开魔魂按钮
            YesGem = 39,								//打开宝石镶嵌按钮
            YesDuplicate = 40,						//确认副本
            YesResetPVP = 41,						//重置pvp
            yesCook = 42,								//打开烤鱼按钮
            YesBuy = 43,								//确认购买
            Getreward = 44,							//获得奖励
            doneCard = 45,							//完成副本
            /// <summary>
            /// 奖励-每日薪金
            /// </summary>
            BenefitsSalaries=46,
            /// <summary>
            /// 奖励-军衔奖励
            /// </summary>
            BenefitsRank = 47,
            /// <summary>
            /// 奖励-工会分红
            /// </summary>
            BenefitsGuild = 48,
            /// <summary>
            /// 奖励-开宝箱
            /// </summary>
            OpenChest=49,
            /// <summary>
            /// 奖励-邀请玩家奖励
            /// </summary>
            PlayerInvite=50,
            /// <summary>
            /// 炼金
            /// </summary>
            MakeGold=51,
			/// <summary>
            /// 奖励-被邀请奖励
            /// </summary>
            PlayerInvitees,
            /// <summary>
            /// 邀请到副本
            /// </summary>
            InviteGoPVE,
			SKILLDeviation = 55,							//技能偏向
			SoulAndDigestGold = 56,						//生产魔魂
			TipsOpenCard = 57,							//翻牌花费
			TipsEquepmentBuild = 58, 					//装备打造花费
			TipsBuyStoreClient = 59,						//购买装备物品
			TipsEquepmentHole = 60,					//装备打孔
			TipsYesUpdateDS = 61,						//升级灵魂精华
			Non = 62,						//升级灵魂精华
			TipsDoTraining=63,//
			TipsHuntingUp=64,//狩猎模式
            TipsResurrection2 = 65,// 死亡面板显示的金币和血石数
		}
		/// <summary>
		/// 使用体力类型
		/// </summary>
		public enum CostPowerType
		{
			NormalDungeon = 1,						//普通副本
			EliteDungeon = 2,							//精英副本
			ShadowDemonWin = 3,					//影魔胜利
			ShadowDemonlose = 4,					//影魔失败
			PowerSolution = 5,							//吃体力药
			PowerOnline = 6,							//在线挂机
			EatFish = 7,										//吃鱼
			Raids = 8,										//扫荡副本
			InviteShadowDemon = 9,					//邀请好友影魔
			LoginPower = 10,							//上线加体力
			EveryDayAim = 11,							//每日目标
		}
    }
}
