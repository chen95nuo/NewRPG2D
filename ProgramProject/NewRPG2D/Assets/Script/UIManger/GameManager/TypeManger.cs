using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum HallEventDefineEnum
{
    AddBuild,//添加建筑
    ChickBuild,//检查建筑
    ChickStock,//检查空间
    ChickStockFull,//检查空间满值
    ChickLevelUpTime,//检查升级的房间
    CameraMove,//相机移动了
    EditMode,//建造模式
    EditMgr,//建造模式辅助
    InEditMode,//进入建造模式
    ClearAllRoom,//清除所有房间
    diamondsSpace,//钻石检测
    ChickRoomMerge,//检查房间合并
    ChickRoleTrain,//角色训练时状态
    ChickRoleLove,//角色恋爱时间
    ChickChildTime,//小孩时间检测
    ChickWorkTime,//工作时间检测
    ShowEquipInfo,//显示装备信息
    ShowBoxInfo,//显示宝箱信息
    ShowPropInfo,//显示道具信息
    RefreshBagUI,//背包变化 刷新背包UI
    UiMainHight,//顶层高亮显示
    CryNewMagic,//检查新技能时间
    EventMax
}

public enum RoleAttribute
{
    Nothing,
    HP,//血量
    Fight,//战斗属性
    Gold,//财务属性
    Food,//烹饪属性
    Mana,//炼金属性
    ManaSpeed,//炼金加速
    Wood,//木工属性
    Iron,//矿工属性

    HallShowAtr,//下方至Max为外部需要显示的属性
    PArmor,//物理护甲
    MArmor,//魔法护甲
    Crt,//暴击率
    INT,//法强
    HIT,//命中
    Dodge,//闪避
    Max,

    //以下是独立读取的
    DPS,//秒伤
    MinDamage, //最小伤害
    MaxDamage, //最大伤害
    HurtType,//伤害类型
}

public enum TrainType
{
    Fight,//战斗等级
    Gold,//财务等级
    Food,//烹饪等级
    Mana,//炼金等级
    Wood,//木工等级
    Iron,//矿工等级
    Max
}

public enum RoleTrainType
{
    Nothing,
    LevelUp,
    Complete,
    MaxLevel
}

public enum RoleLoveType
{
    Nothing,
    WaitFor,
    Start,
    ChildBirth,
    boredom,
    End
}

public enum MagicName
{
    //Nothing,
    Fireball,//火球术
    Horn,//战斗号角
    Defense,//防御旗帜
    Treatment,//治疗绷带
    Call,//佣兵召唤
    FreezingTrap,//冰冻陷阱
    BlastingTrap,//爆破陷阱
    GroupResponse,//群体恢复
    Max
}

public enum MagicType
{
    AbilityDamage,
    Buff,
    Restore,
    Call,
    Attack,
}

public enum GetAccess
{
    Decomposing,//分解装备
    DailyTasks,
}

public enum ItemType
{
    All,//全部
    Box,//宝箱
    Prop,//道具
    Equip,//武器
}

public enum PropType
{
    Other,//其他
    Fragment,//碎片
    Equipment,//装备
    Resources,//资源类
}

public enum BagType
{
    AllItem,
    Weapons,
    Armor,
    Jewelry,
    Box,
    Prop,
    Max
}