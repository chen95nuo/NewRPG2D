using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType
{
    Nothing,//无
    Egg,//蛋
    Prop,//道具
    Equip,//装备
    Role//角色
}

public enum EquipType
{
    Nothing,//空
    Weapon, //武器
    Armor, //防具
    Necklace, //项链
    Ring //戒指

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

public enum UIEventDefineEnum
{
    UpdateEggsEvent,
    UpdatePropsEvent,
    UpdateEquipsEvent,
    UpdateRolesEvent,
    EventMax
}
