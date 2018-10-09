using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum HallEventDefineEnum
{
    AddBuild,//添加建筑
    ChickBuild,//检查建筑
    ChickStock,//检查空间
    ChickLevelUpRoom,//检查升级的房间
    CameraMove,//相机移动了
    EditMode,//建造模式
    EditMgr,//建造模式辅助
    InEditMode,//进入建造模式
    ClearAllRoom,//清除所有房间
    diamondsSpace,//钻石检测
    ChickRoomMerge,//检查房间合并
    EventMax
}

public enum RoleHurtType
{
    Nothing,
    AttackDamage,
    AbilityDamage,
}

public enum RoleAttribute
{
    Nothing,
    Fight,//战斗属性
    Gold,//财务属性
    Food,//烹饪属性
    Mana,//炼金属性
    ManaSpeed,//炼金加速
    Wood,//木工属性
    Iron,//矿工属性
    HurtType,//伤害类型
    DPS,//秒伤
    Crt,//暴击率
    PArmor,//物理护甲
    MArmor,//魔法护甲
    Dodge,//闪避
    HIT,//命中
    INT,//法强
    HP,//血量
    Max
}

public enum TrainType
{
    Nothing,
    Fight,//战斗等级
    Gold,//财务等级
    Food,//烹饪等级
    Mana,//炼金等级
    Wood,//木工等级
    Iron,//矿工等级
    Max
}

