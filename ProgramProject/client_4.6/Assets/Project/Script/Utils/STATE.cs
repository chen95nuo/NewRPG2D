using UnityEngine;
using System.Collections;

public class STATE {
	//-------------------------------------- 和服务器通信的类型-------------------------------//
	public const int UI_PLAYER 													= 0;
	public const int UI_PACK													= 1;
	public const int UI_PACK_SALE												= 2;
	public const int UI_MAIN													= 3;
	public const int UI_CARDGROUP												= 4;
	public const int UI_CARDGROUP2												= 5;
	public const int UI_CARDGROUP_SET_FIGHT										= 6;
	public const int UI_Intensify												= 7;
	public const int UI_Intensify2												= 8;
	public const int UI_PACK_DETAIL												= 9;
	public const int UI_MISSION													= 10;
	public const int UI_REFRESH_HELPER											= 11;
	public const int UI_Compose													= 12; 		// request compose list
	public const int UI_Compose2												= 14; 		// request compose item list
	
	//--cuixl 获取扭曲空间中迷宫的数据--//
	public const int UI_WARPSPACE												= 18;		//获取扭曲空间数据//
	
	
	//--cuixl 迷宫请求编号--//
		public const int UI_MAZE_INTO											= 19;		//进入迷宫//
	public const int UI_MAZE_GETDATA											= 20;		//获取每个格子信息//
	public const int UI_MAZE_RESET												= 27;		//重置迷宫信息//
	public const int UI_MAZE_USEBLOOD											= 54;		//使用血瓶js//
	public const int UI_MAZE_WISH												= 56;  		//许愿//
	public const int UI_MAZE_BOSSREWARD               							= 57;		//通关迷宫的boss奖励//
	
	
	//--cuixl 灵界（冥想）--//
	public const int UI_SPRITEWROLD_INTO										= 21;		//进入冥想界面//
	public const int UI_RESTORE_POWER											= 22;		//自动回复体力//
	public const int UI_MAP														= 23;		//进入地图选择//
	//--cuixl 活动副本（异世界）--//
	public const int UI_EVENT													= 24;		//进入活动副本界面（异世界）//
	public const int UI_SELECT_EVENT											= 25;		//进入副本后选择关卡//
	public const int UI_ACHIEVEMENT												= 26;		//成就界面//
	public const int UI_LOT														= 28;		//抽卡//
	public const int UI_SIGN													= 29;		//签到//
	
	public const int UI_CardBreak												= 30; /* 突破-可以突破的卡*/
	public const int UI_CardBreak1												= 31; /* 突破-可以被突破*/
	public const int UI_Unlock													= 32; //解锁模块 //
	public const int UI_Mail													= 33;		//邮箱//
	
	public const int UI_HEADSET_SHOWDATA										= 34; 		//获得头像设置的界面信息//
	public const int UI_HEADSET_CHANGEICON										= 35;		//头像设置//
	public const int UI_HEADSET_CHANGENAME										= 36;		//名称设置//
	
	public const int UI_DROP_GUIDE												= 37;		//掉落指引//
	public const int UI_CARDINFO													= 38;		//卡牌装备和被动技能信息//
	public const int UI_CHARGE													=39;		//获取充值界面信息
	public const int UI_CHARGESPECIAL										=40;		//获取玩家充值特权界面//
    public const int UI_Gift                                                    = 41;		//获取玩家在线礼包的信息//
	public const int UI_KO_EXCHANGE1											= 42;  //ko兑换界面1//
	public const int UI_KO_EXCHANGE2											= 43;  //ko兑换界面2//
	public const int UI_KO_EXCHANGE												= 44;  //ko兑换操作//

    public const int UI_SHOP                                                    = 45;   //商店操作//

    public const int UI_SEVEN_PRIZE                                             = 47;   //七天登录奖励界面//
    public const int UI_SEVEN_BUT                                               = 48;   //七天奖励领取按钮//

    public const int UI_LEVEL_PRIZE                                             = 52;   //请求等级奖励界面//
    public const int UI_LEVEL_BUT                                               = 53;   //等级奖励领取按钮//
	
	public const int UI_ACT_WHEEL														=58;	//请求活动界面里面的大风车//
	public const int UI_ACT_GEAR_WHEEL												=59;	//请求当前档位上一次风车转动情况//
	public const int UI_ACT_WHEEL_REWARD										=60;	//请求领取风车转动奖励//
	public const int UI_ACT_LOTCARD												= 61;	//请求显示抽卡界面信息//
	
	public const int UI_UNITESKILL_UNLOCK										= 51;	//请求服务器获取是否有新合体技解锁//
	//-------------------------- 添加prefab的类型 --------------------------//
	public const int PREFABS_TYPE_CARD 											= 0;
	public const int PREFABS_TYPE_EFFECT 										= 1;
	public const int PREFABS_TYPE_ITEM 											= 2;
	public const int PREFABS_TYPE_UI 											= 3;
	
	//-------------------------- 层的id ------------------------//
	public const int LAYER_ID_DEFAULT											= 0;		//default 层//
	public const int LAYER_ID_NGUI												= 8;
	public const int LAYER_ID_COMBOACTION										= 11;
	public const int LAYER_ID_UIEFFECT										= 13;
	
	//-------------------------- 活动抽卡类型 --------------------------//
	public const int ActLotCardType_Free 										= 11;		//免费抽卡 lotjson的t//
	public const int ActLotCardType_Crystal										= 12;		//钻石抽卡 lotjson的t//
	
	//---------------------------- 打开scrollview 的类型------------------------//
	//0 卡牌按钮， 1 主动技能按钮， 2 被动技能按钮， 3防具， 4 武器， 5 饰品 , 6 被动技能按钮1, 7 被动技能按钮2, 8被动技能按钮3 //
	public const int SHOW_SCROLLVIEW_TYPE_CARD 									= 0;
	public const int SHOW_SCROLLVIEW_TYPE_ACTIVESKILL							= 1;
	public const int SHOW_SCROLLVIEW_TYPE_UNACTIVESKILL							= 2;
	public const int SHOW_SCROLLVIEW_TYPE_WEAPON								= 3;
	public const int SHOW_SCROLLVIEW_TYPE_PROWEAPON								= 4;
	public const int SHOW_SCROLLVIEW_TYPE_ORNAMENTS								= 5;
	
	public const int SHOW_SCROLLVIEW_PS1													= 6;
	public const int SHOW_SCROLLVIEW_PS2													= 7;
	public const int SHOW_SCROLLVIEW_PS3													= 8;
	
	//------------------------- 图集的名字 -------------------------//
	public const string UIATLAS_HERO_CARD_ICON									= "HeroCardIcon";
	public const string UIATLAS_EQUIP_ICON										= "EquipIcon";
	public const string UIATLAS_SKILL_ICON										= "SkillIcom";
	public const string UIATLAS_UNITE_SKILL_ICON								= "UnitSkillIcon";
	public const string UIATLAS_ITEM_ICON										= "ItemIcon";
	
	//------------------------- 当前经验条的类型 -------------------------//
	//卡牌的经验值//
	public const int EXP_TYPE_RESULT_CARD										= 0;		//结算界面卡牌的经验条,动//
	public const int EXP_TYPE_OTHER_CARD										= 10;		//经验条是静止的不动//
	//人物的经验条//
	public const int EXP_TYPE_RESULT_PLAYER										= 1;		//结算界面人物的经验条，动//
	public const int EXP_TYPE_OTHER_PALYER										= 11;		//其他界面人物的经验条。不动//
	// skill exp bar//
	public const int EXP_TYPE_MOVE_SKILL 										= 2;		// Skill的经验条，动//
	public const int EXP_TYPE_STATIC_SKILL 										= 12;		// Skill的经验条，不动//
	// passskill exp bar //
	public const int EXP_TYPE_MOVE_PASSIVESKILL									= 3;		// PassiveSkill的经验条，动//
	public const int EXP_TYPE_STATIC_PASSIVESKILL								= 13;		// PassiveSkill的经验条，不动//
	
	public const int EXP_TYPE_MOVE_SWEEP_CARD									= 4;
	
	// break exp bar //
	public const int EXP_TYPE_MOVE_BREAK											= 4;
	
	//-------------------------- 结算场景中各个界面 ---------------------------//
	public const int RESULT_BATTLERESULT										= 0;		//战斗结果界面//
	public const int RESULT_CARDLEVELUP											= 1;		//卡牌升级界面//
	public const int RESULT_BATTLESETTLEMENT									= 2; 		//战斗结算界面//
	public const int RESULT_PLAYERLEVELUP										= 3;		//人物升级界面//
	public const int RESULT_GETCARD												= 4; 		//人物获得卡牌界面//
	
	//---------------------------- 掉落物品的类型---------------------------//
	public const int DROPS_TYPE_ITEM											= 1;		//材料//
	public const int DROPS_TYPE_EQUIP											= 2;		//装备//
	public const int DROPS_TYPE_CARD											= 3;		//卡牌//
	public const int DROPS_TYPE_SKILL											= 4;		//主动技能//
	public const int DROPS_TYPE_PASSIVESKILL									= 5;		//被动技能//
	
	//---------------------------- 掉落物品对应的卡牌的名字 ---------------------------//
	public const string DROPS_TYPE_NAME_ITEM									= "result_card_material";
	public const string DROPS_TYPE_NAME_EQUIP									= "result_card_equip";
	public const string DROPS_TYPE_NAME_CARD									= "result_card_hero";
	public const string DROPS_TYPE_NAME_SKILL									= "result_card_skill";
	public const string DROPS_TYPE_NAME_PASSIVESKILL							= "result_card_skill";
	
	//------------------------------掉落物品根据不同的星级对应不同颜色的箱子-----------------------------//
	public const string DROPS_BOX_STAR_COLOR_1									= "treasurebox_11";
	public const string DROPS_BOX_STAR_COLOR_2									= "treasurebox_21";
	public const string DROPS_BOX_STAR_COLOR_3									= "treasurebox_31";
	public const string DROPS_BOX_STAR_COLOR_4									= "treasurebox_41";
	public const string DROPS_BOX_STAR_COLOR_5									= "treasurebox_51";
	public const string DROPS_BOX_STAR_COLOR_6									= "treasurebox_61";
	
	
	//----------------------------速度线合体技界面的类型----------------------------//
	public const int SHOW_TYPE_UNITESKILL										= 0;		//合体技界面//
	public const int SHOW_TYPE_BOUNES											= 1;		//bounes界面//
	
	
	
	//----------------------------显示的提示框的内容----------------------------//
	public const int SHOW_TIP_TYPE_CARD											= 0;		//卡牌的提示框//
	public const int SHOW_TIP_TYPE_UNITESKILL									= 1;		//合体技的提示框//
	
	
	//----------------------------结算系统切换界面方式----------------------------//
	public const int RESULT_CHANGE_NEXT_PAGE_AUTO								= 0;		//自动跳转//
	public const int RESULT_CHANGE_NEXT_PAGE_CLICK_CHANGE						= 1;		//点击切换//
	public const int RESULT_CHANGE_NEXT_PAGE_CLICK_CLOSE						= 2;		//点击关闭//
	
	//----------------------------战斗结果----------------------------//
	public const int BATTLE_RESULT_WIN											= 1;		//胜利//
	public const int BATTLE_RESULT_LOSE											= 2;		//失败//
	
	//----------------------------主菜单页面的按钮对应的界面----------------------------//
	public const int MAINMENU_PAGE_MAINMENU										= 0;		//军团（主菜单）//
	public const int MAINMENU_PAGE_EMBATTLE										= 1;		//卡组//
	public const int MAINMENU_PAGE_SHOP											= 2;		//商城//
	public const int MAINMENU_PAGE_OTHER										= 3;		//other//
	public const int MAINMENU_PAGE_BATTLE										= 4;		//战斗//
	
	//----------------------------军团界面各个按钮----------------------------//
	public const int MAINCARDSET_TYPE_COMBINATION								= 0;		//军团（卡组）//
	public const int MAINCARDSET_TYPE_BAG										= 1;		//背包//
	public const int MAINCARDSET_TYPE_INTENSIFY									= 2;		//吞噬//
	public const int MAINCARDSET_TYPE_COMPOSE  									= 3;		//合成//
	public const int MAINCARDSET_TYPE_TALENT									= 4;		//符文//
	public const int MAINCARDSET_TYPE_ICON										= 5;		//图鉴//
	
	
	//----------------------------战斗界面五个小人的按钮的类型----------------------------//
	public const int MAINUI_TYPE_BASICWORLD										= 0;		//基础世界//
	public const int MAINUI_TYPE_BOSSWORLD										= 1;		//世界boss//
	public const int MAINUI_TYPE_ARENA											= 2;		//竞技场//
	public const int MAINUI_TYPE_SPRITEWORLD									= 3;		//灵界//
	public const int MAINUI_TYPE_WARPSPACE										= 4;		//扭曲空间//
	public const int MAINUI_TYPE_DEMONGIRL										= 5;		//魔女//
	
	
	//----------------------------游戏场景名字----------------------------//
	public const string GAME_SCENE_NAME_LOADING 								= "loading";			//loading场景//
	public const string GAME_SCENE_NAME_GAME	 								= "unity";				//loading场景//
//	public const string GAME_SCENE_NAME_BATTLERESULT							= "battleResult";		//结算界面//
	public const string GAME_SCENE_NAME_UI										= "UIScene";			//ui界面//
	
	//----------------------------card race--------------------------//
	public const int RACE_CHINA 												= 1;
	public const int RACE_ASIA													= 2;
	public const int RACE_GREECE 												= 3;
	public const int RACE_NORTHERN_EUROPE 										= 4;
	public const int RACE_EXP													= 5;
	public const int RACE_BREAK							 						= 6;
	public const int RACE_COIN							 						= 7;
	
	
	//----------------------------当前战斗的类型----------------------------//
	public const int BATTLE_TYPE_NORMAL											= 1;	//pve//
	public const int BATTLE_TYPE_MAZE											= 2;	//迷宫中的战斗//
	public const int BATTLE_TYPE_PVP											= 3;	//pvp//
	public const int BATTLE_TYPE_EVENT											= 4;	//活动副本战斗//
	public const int BATTLE_TYPE_DEMO											= 5; // demo battle //
	
	
	//----------------------------迷宫中战斗的类型----------------------------//
	public const int MAZE_BATTLE_TYPE_DROPS										= 1;	//随机掉落的战斗//
	public const int MAZE_BATTLE_TYPE_BOSS										= 2;	//boss战斗//
	
	//---------------------------- 服务器返回错误代码 -------------------------//
	
	//---------------------------- Camera Data ID-------------------------//
	public const int WIN_CAMERA_DATA_ID = 12;
	public const int START_FIGHT_CAMERA_DATA_ID = 14;
	
	//---------------------------- 不同界面对应的音乐的type -------------------------//
	public const int MUSIC_TYPE_BATTLE											= 1;		//战斗//
	public const int MUSIC_TYPE_MENU											= 2;		//主菜单//
	public const int MUSIC_TYPE_MISSION											= 3;		//小地图//	
	public const int MUSIC_TYPE_SPLASH											= 4;		//封面//
	public const int MUSIC_TYPE_MAZE											= 5;		//迷宫//
	public const int MUSIC_TYPE_PVP												= 6;		//PVP//
	public const int MUSIC_TYPE_WIN												= 7;		//战斗胜利//
	public const int MUSIC_TYPE_LOSE											= 8;		//战斗失败//
	public const int MUSIC_TYPE_SPRITEWORLD										= 9;		//冥想//
//	public const int MUISC_TYPE_LOGIN											= 10;		//登陆界面//
	
	//----------------------------- 音效类型的id ------------------------------------//
	public const int SOUND_EFFECT_ID_COMMON										= 1;		//除back外的通用按钮音效//
	public const int SOUND_EFFECT_ID_BACK										= 2;		//点击back的音效//
	public const int SOUND_EFFECT_ID_CARDGROUP									= 3;		//卡组界面和卡牌相关操作的音效//
	public const int SOUND_EFFECT_ID_STAR										= 4;		//战斗胜利后星星落下的音效//
	public const int SOUND_EFFECT_ID_COIN										= 5;		//所有金币有修改的地方//
	public const int SOUND_EFFECT_ID_LEVELUP									= 6;		//玩家和卡牌升级的音效//
	public const int SOUND_EFFECT_ID_QH_SURE									= 7;		//强化确定按钮音效//
	public const int SOUND_EFFECT_ID_QH_SUCC									= 8;		//强化成功音效//
	public const int SOUND_EFFECT_ID_QH_FAIL									= 9;		//强化失败音效//
	public const int SOUND_EFFECT_ID_EXP_CHANGE									= 10;		//玩家和卡牌经验值增加时，循环播放//
	public const int SOUND_EFFECT_ID_HC_SURE									= 11;		//合成确定按钮//
	public const int SOUND_EFFECT_ID_CARDGET_MODEL								= 12;		//抽卡时欧若拉的特效的音效//
	public const int SOUND_EFFECT_ID_GETCARD									= 13;		//获得卡牌的特效，结算，bonus, 抽卡//
	public const int SOUND_EFFECT_ID_FW_SUCC									= 14;		//符文点亮成功时//
	public const int SOUND_EFFECT_ID_STARTFIGHT									= 15;		//战斗开始时出现fight文字//
	public const int SOUND_EFFECT_ID_BONUS_CLIKC								= 16;		//bonus平时点击模型的音效//
	public const int SOUND_EFFECT_ID_BONUS_ADD_TIMES							= 17;  		//BONUS增加金币倍数时//
	public const int SOUND_EFFECT_ID_UNITESKILL_NAME							= 18;		//合体技和bonus出现名字时//
	
	//---------------------------   卡牌音效类型 -------------------------------------//
	public const int CARD_MUSIC_EFFECT_TYPE_ACTION									= 0;		//卡牌动作音效//
	public const int CARD_MUSIC_EFFECT_TYPE_DIE										= 1;		//卡牌死亡音效//
	
	//---------------------------   技能音效类型 -------------------------------------//
	public const int SKILL_MUSIC_EFFECT_TYPE_CHARGE									= 0;		//技能蓄力音效//
	public const int SKILL_MUSIC_EFFECT_TYPE_PLAY									= 1;		//技能释放音效//
	public const int SKILL_MUSIC_EFFECT_TYPE_HURT									= 2;		//技能受伤音效//
	
	
	//---------------------------- 战斗结束返回界面的id -------------------------//
	//1 返回map界面， 2 返回迷宫界面， 3返回扭曲空间界面， 4 返回pvp（竞技场界面） 5 返回副本选关卡界面//
	public const int BATTLE_BACK_MAP											= 1;		//1 返回map界面//
	public const int BATTLE_BACK_MAZE											= 2;		//1 返回迷宫界面//
	public const int BATTLE_BACK_WRAPSPACE										= 3;		//1 返回扭曲空间界面//
	public const int BATTLE_BACK_PVP											= 4;		//1 返回迷宫pvp界面//
	public const int BATTLE_BACK_EVENT											= 5;		//1 返回活动副本选关界面//
	public const int BATTLE_BACK_QH												= 6;		// 返回强化界面//
    public const int BATTLE_BACK_ZH                                             = 7;		// 返回召唤界面//
	
	//---------------------------- 进入主菜单界面的类型 -------------------------//
	//0 第一次进入， 1 从其他界面返回//
	public const int ENTER_MAINMENU_FRIST										= 0;
	public const int ENTER_MAINMENU_BACK										= 1;
	
	//--------------------------------合体技名称展示界面id ----------------------------//
	public const int UNITESHOW_NAME_BONUS										= 0;		//bonus展示//
	public const int UNITESHOW_NAME_SKILL01										= 1;		//三角冲击//
	
	//--------------------------------- 各个模块的id -------------------------------//
	public const int UI_MODEL_CARDGROUP											= 1;		//卡组界面//
	public const int UI_MODEL_PACK												= 2;		//背包界面//
	public const int UI_MODLE_QH												= 3;		//强化界面//
	public const int UI_MODLE_HC												= 4;		//合成//
	public const int UI_MODLE_FW												= 5;		//符文//
	public const int UI_MODLE_PICTURE											= 6;		//图鉴//
	public const int UI_MODLE_BOSSWORLD											= 7;		//世界boss//
	public const int UI_MODLE_ACIVECOPY											= 8;		//活动副本（异世界）//
	public const int UI_MODLE_ARENA												= 9;		//竞技场//
	public const int UI_MODLE_SPRITEWORLD										= 10;		//灵界//
	public const int UI_MODLE_WARPSPACE											= 11;		//扭曲空间//
	public const int UI_MODLE_FRIENDHELP										= 12;		//好友援助（是模块不是好友界面）//
	public const int UI_MODLE_UNITESKILL										= 13;		//合体技模块//
	public const int UI_MODLE_BONUS												= 14;		//BONUS模块//
	public const int UI_MODLE_JYCOPY											= 15;		//精英副本//
	public const int UI_MODLE_LOTCARD											= 16;		//抽卡(召唤)//
	public const int UI_MODLE_BROKE												= 17;		//突破//
	public const int UI_MODLE_BATTLE											= 18;		//挑战//
	public const int UI_MODLE_ACTIVE											= 19;		//活动//
	public const int UI_MODLE_CJ												= 20;		//成就//
	public const int UI_MODLE_FRIENDS											= 21;		//好友//
	public const int UI_MODLE_MAIL												= 22;		//邮件//
	public const int UI_MODLE_SIGN												= 23;		//签到//
	public const int UI_MODLE_RECHARGE											= 24;		//充值//
	public const int UI_MODLE_GIFT												= 25;		//礼包//
	public const int UI_MODLE_PVE												= 26;		//PVE//
    public const int UI_MODLE_CORNUCOPIA                                        = 26;		//聚宝盆界面//
	
	//--------------------------------- 存储数据的key -------------------------------//
	public const string SAVE_KEY_MUSICBG								= "MusicVolume";	//背景音乐//
	public const string SAVE_KEY_SOUNDEFF								= "SoundEffVolume";	//音效//
	
	
	public const int BATTEL_WITH_FRIEDN_ID							= 110201; // 解锁好友功能//
	
	
	// game speed in battle
	public const float SPEED_NORMAL = 1.0f;
	public const float SPEED_BATTLE_NORMAL = 1.1f;
	public const float SPEED_BATTLE_2X = 2f;
	
	public enum SKILL_TYPE : int
	{
		E_Null = -1,
		E_Physics = 0,
		E_Fire = 1,
		E_Thunder = 2,
		E_Ice = 3,
		E_Wind = 4,
		E_All = 5,
	}
}
