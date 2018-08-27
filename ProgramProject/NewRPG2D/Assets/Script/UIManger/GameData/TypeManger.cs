using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Nothing,//无
    Egg,//蛋
    Prop,//道具
    Equip,//装备
    Role,//角色
    Reward//奖励界面
}

public enum GridType
{
    Nothing,//其他
    Use,//使用道具
    Store,//商城道具
    Explore,//探险
    Team,//小队
    NoClick,//禁止点击
}

public enum EquipType
{
    Nothing,//空
    Weapon, //武器
    Armor, //防具
    Necklace, //首饰
}

public enum EquipAffixName
{
    att,
    con,
    def,
    dex,
    crt,
    sbl,
    stun,
    spd,
    mspd,
    bld,
    cd,
    weak,
}

public enum TeamType
{
    Nothing,//无
    第一编队,
    第二遍队,
    第三编队,
}

public enum MenuType
{
    Nothing,//无
    stars,//星级
    hatchingTime,//孵化时间
    quality,//品质
    isUse,//可用级别
    equipType,//装备类型
    roleType//角色排序
}

public enum PropType
{
    Nothing,//无
    AllOff,//全不显示
    OnlyUse,//只能用
    OnlySell,//只能卖
    AllOn//能用也能卖
}

public enum RoleAtrType
{
    Nothing,
    Health,
    Attack,
    Agile,
    Defense,
}

public enum CurrencyType
{
    Nothing,//无
    GoldCoin,//金币道具
    Diamonds,//钻石道具
}

public enum FurnaceType
{
    Nothing,//无
    Run,//运行中
    End//结束
}

public enum ExploreType
{
    Nothing,//无
    Run,//运行中
    End//结束
}

public enum HatcheryType
{
    Nothing,//无
    Open,//开启状态
    Run,//运行中
    End//结束
}

public enum PropMeltingType
{
    isFalse,//不可熔炼
    isTrue,//可熔炼
}

public enum ItemMaterialType
{
    Nothing,//无
    Iron,//铁
    Wood,//木头
    Stone,//石头
    Leatherwear,//布料
    Magic,//魔法
    Diamonds//钻石
}

public enum TipType
{
    Nothing,
    Explore,//冒险
}

public enum DifficultyType
{
    Nothing,//空
    Easy,//简单
    Normal,//普通
    Difficult,//困难
}

public enum UIMessageType
{
    Nothing,//空
    OnlyTip,//仅仅文字提示
    BuyOrSell,//购买或者出售

}

public enum UITalkType
{
    Nothing,//空
    Furnace,//炼金
    Store,//商店
    EggStore,//扭蛋
    Explore//探险
}

public enum UIEventDefineEnum
{
    UpdateUsePage,
    UpdateSellItem,
    UpdateBuyItem,
    UpdateEggsEvent,
    UpdatePropsEvent,
    UpdateEquipsEvent,
    UpdateRolesEvent,
    UpdateRoundEvent,
    UpdateRoleTipEvent,
    UpdateCardMessageEvent,
    UpdateBagItemMessageEvent,
    UpdateGainTipEvent,
    UpdateMaterialEvent,
    UpdateFurnaceEvent,
    UpdateFurnaceMenuEvent,
    UpdatePlayerExp,
    UpdatePlayerGoldCoin,
    UpdatePlayerDiamonds,
    UpdatePlayerFatigue,
    UpdateStoreEvent,
    UpdateExploreEvent,
    UpdateTalkEvent,
    UpdateExploreTipEvent,
    UpdateHatcheryEvent,
    UpdateLittleTipEvent,
    UpdateMessageTipEvent,
    UpdateMessageTipGoEvent,
    UpdateMessagePopTipEvent,
    UpdateRewardMessageEvent,
    MissionComplete,
    MissionFailed,
    FightMessage,
    EventMax
}

