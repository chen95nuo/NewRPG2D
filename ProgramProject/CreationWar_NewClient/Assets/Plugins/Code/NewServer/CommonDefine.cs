
using System.Collections;

public static class CommonDefine {
    //无方向技能类型
    public static int SKILL_TYPE_NO_DIRECT = 0;

    //有方向技能类型
    public static int SKILL_TYPE_HAVE_DIRECT = 1;

    public static int ACT_FISH = 1;
	public static int ACT_MINE = 2;
	public static int ACT_OPENCHEST = 3;
	public const int ACT_JUMP = 4;
	public static int ACT_EAT = 5;
	public static int ACT_RIDE = 6;
	public static int ACT_UNRIDE = 7;
	public static int ACT_SetDirection = 8;
	public static int ACT_Behit = 9;
	public static int ACT_PlayEffect = 10;
	public static int ACT_HitAnimation = 11;
	public static int ACT_PlaySelfAudio = 12;
	public static int ACT_ApplyBuff = 13;
	public static int ACT_ApplyBuff_Down = 14;
	public static int ACT_ApplyBuff_Ground = 15;
	public static int ACT_ApplyBuff_JiTui = 16;
	public static int ACT_PlayAnimat = 17;
	public static int ACT_PlaySynMax = 18;
	public static int ACT_PlaySynAttr = 19;
	public static int ACT_PlayOnFight = 20;
	public static int ACT_RideOn = 21;
	public static int ACT_RideOff = 22;
	public static int ACT_PlaybuffEffect = 23;
	public static int ACT_BigBoom = 24;
	public static int ACT_PlayskillEffect = 25;
	public static int ACT_Huandon = 26;
	public static int ACT_Showbody = 27;
	public static int ACT_rideClose = 28;
	public static int ACT_PlayerSpawn = 29;
	public static int ACT_Mover = 30;
	public static int ACT_ChangeWeapon = 31;
	public static int ACT_GetAnimation = 32;
	public static int ACT_SyyAnimation = 33;
	public static int ACT_CrossAnimation2 = 34;
	public static int ACT_CrossAnimation = 35;
	public static int ACT_synAspeed = 36;
	public static int ACT_RPCShowWeaponFirst = 37;
	public static int ACT_RPCRemoveWeapon = 38;
	public static int ACT_lookTouRPC = 39;
	public static int ACT_ObjCloseAsType = 40;
	public static int ACT_PlayAnimation = 41;
	public static int ACT_CallObjectSoul = 42;
	public static int ACT_ReMoveSoul = 43;
	public static int ACT_SoulPlayEffect = 44;
	public static int ACT_SoulSyncAnimation = 45;
	public static int ACT_SoulShoot = 46;
	public static int ACT_SoulSkill = 47;
	public static int ACT_PlayrideAnimation = 48;
	public static int ACT_SynMana = 49;
	public static int ACT_Callloop = 50;
	public static int ACT_changelayer = 51;
	public static int ACT_skillContinue = 52;
	public static int ACT_roll = 53;
	public static int ACT_FontEffect = 54;
//	public static int ACT_ = ;
//	public static int ACT_ = ;
//	public static int ACT_ = ;
//	public static int ACT_ = ;
	public static int Pay_Interval_Time = 5;

    public static int ACTIVITY_TYPE_BATTLEFIELD = 1; //"战场活动";
	public static int ACTIVITY_TYPE_TASK = 2; //"任务活动";
	public static int ACTIVITY_TYPE_NORAML = 4; //"玩法活动";
	public static int ACTIVITY_TYPE_BOSS = 3; //"BOSS活动";

    public static int BattlefieldOpenDoorTime = 20;
    public static int PVP_END_TIME = 60 * 40;
	public static string PVP_ONE = "4";

	public static int AutoAttackSimple = 1;
	public static int AutoTrusteeship = 2;
	public static int AutoNON = 0;


	public static float JoyClick_pressTime = 0.3f;
	public static int JoyClick_mouseMove = 50;
	
	public static float JoyDodgeroll_HoldTime = 0.5f;
	public static int JoyDodgeroll_Movingdistance = 20;
	
	public static float JoySlide_HoldTime = 0.3f;
	public static int JoySlide_Movingdistance = 40;
	
	public static int JoyDrag_Movingdistance = 20;

    public static int DUEL_STATE_IDLE = 0;
	public static int DUEL_STATE_WAITING = 1;
	public static int DUEL_STATE_START = 2;
	
	public static int DUEL_INVITE_FEEDBACK_YES = 1;
	public static int DUEL_INVITE_FEEDBACK_NO = 2;
	public static int DUEL_INVITE_FEEDBACK_TIMEOUT = 3;

    public static int DUEL_RESULT_WIN = 1;
	public static int DUEL_RESULT_LOSE = 2;

    public static int ADD_TO_INSTANCE_MAP_OK = 1;
	public static int ADD_TO_INSTANCE_MAP_FAIL = 2;

	public static int TOWER_STATE_NOT_START = 0;
	public static int TOWER_STATE_START = 1;

	public static int TOWER_CHALLENGE_FAIL = 1;
	public static int TOWER_CHALLENGE_OK = 2;

    /// <summary>
    /// 拍卖行24小时管理费
    /// </summary>
    public static int MANAGERMENT_FEE_24H = 1000;
    /// <summary>
    /// 拍卖行48小时管理费
    /// </summary>
    public static int MANAGERMENT_FEE_48H = 2000;
    /// <summary>
    /// 成交扣除百分比（拍卖成交后会扣除成交价格一定百分比）
    /// </summary>
    public static float DEAL_PERSENT = 0.07f;
    /// <summary>
    /// 购买拍卖次数的价格（血石数量）
    /// </summary>
    public static int AUCTION_SLOT_PRICE = 50;
    /// <summary>
    /// 免费拍卖栏位数量
    /// </summary>
    public static int AUCTION_FREE_SLOT = 1;
    /// <summary>
    /// 最大拍卖栏位数
    /// </summary>
    public static int AUCTION_MAX_SLOT = 5;

	/// <summary>
	/// 装备战斗力比较
	/// </summary>
	public static int Force_NON = 0;
	public static int Force_Higher = 1;
	public static int Force_Lower = 2;


	/// <summary>
	///	go到传送门
	/// </summary>
	public static int GoDungeon = 0;
	
	/// <summary>
	///	打开训练面板
	/// </summary>
	public static int OpenTraining = 1;
	
	/// <summary>
	///	打开强化装备面板
	/// </summary>
	public static int OpenStrengthen = 2;
	
	/// <summary>
	///	自动寻路到附近鱼点
	/// </summary>
	public static int GoFishing = 3;
	
	/// <summary>
	///	打开烤鱼面板
	/// </summary>
	public static int GoCooking = 4;
	
	/// <summary>
	///	打开挑战影魔
	/// </summary>
	public static int OpenShadowFiend = 5;
	
	/// <summary>
	///	打开商城面板
	/// </summary>
	public static int OpenStore = 6;
	
	/// <summary>
	///	打开充值面板
	/// </summary>
	public static int OpenRecharge = 7;
	
	/// <summary>
	///	打开炼金面板
	/// </summary>
	public static int OpenAlchemy = 8;
	
	/// <summary>
	///	打开炼魂面板
	/// </summary>
	public static int OpenSoul = 9;
	
	/// <summary>
	///	打开活动面板
	/// </summary>
	public static int OpenActivities = 10;
	
	/// <summary>
	///	打开头像菜单PVP自动排队
	/// </summary>
	public static int OpenPVPone = 11;
	
	/// <summary>
	///	打开头像菜单战场
	/// </summary>
	public static int OpenBattle = 12;


}

