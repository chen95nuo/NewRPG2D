using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType : byte
{
    Nothing,
    Wall,
    Full
}

public enum BuildRoomName
{
    Nothing,
    Gold,//贸易厅
    GoldSpace,//金库
    Food,//食堂
    FoodSpace,//地窖
    Mana,//炼金房
    ManaSpace,//法力池
    Wood,//伐木场
    WoodSpace,//木材库
    Iron,//铁矿场
    IronSpace,//铁矿库
    FighterRoom,//战斗训练场
    Mint,//账房 会计训练
    Kitchen,//厨房 厨师训练
    Laboratory,//实验室 炼金训练
    Crafting,//手工房 伐木等级
    Foundry,//健身房 挖矿等级
    LivingRoom,//起居室
    TrophyRoom,//荣耀大厅
    Hospital,//医院
    ClanHall,//氏族大厅
    MagicWorkShop,//魔法制造
    MagicLab,//魔法升级
    WeaponsWorkShop,//武器工坊
    ArmorWorkShop,//护甲工坊
    GemWorkSpho,//宝石工坊
    Stairs,//楼梯
    ThroneRoom,//国王大厅
    Barracks,//军营
    BabyRoom,//婴儿房
    MaxRoom
}
public enum RoomType
{
    Nothing,
    Production,//生产类
    Training,//训练类
    Support,//功能类
    Max
}


public enum CastleType
{
    main,
    edit
}

public enum MapType
{
    MainMap,
    EditMap
}

public enum ThroneInfoType
{
    Upgraded,
    Build
}

public enum MaterialName
{
    Gold,
    Food,
    Mana,
    Wood,
    Iron
}