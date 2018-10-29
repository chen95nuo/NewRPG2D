//功能： 事件管理需要的其他杂项，统一存放
//创建者: 胡海辉
//创建时间：

using Assets.Script.Battle;
using UnityEngine;

public class EventHelper
{
}
public enum EventDefineEnum : uint
{
    AddRole,
    SwitchRoleAction,
    LoadLevel,
    ClickMyRole,
    ClickEnemyRole,
    CreateRole,
    HpChange,
    DragStart,
    Draging,
    DragEnd,
    AnimationHit,
    AnimationEnd,
    GameOver,
    EventMax,
}

public enum SwitchStatusEnum
{
    OpenStatus,
    CloseStatus,
}

public class AddRoleParam
{
   
}

public class LoadLevelParam
{

}

public class HpChangeParam
{
    public RoleBase role;
    public float changeValue;
}

public class SelectTargetParam
{
    public Transform OriginalTransform;
    public Transform TargetTransform;
}

public class SwitchRoleActionParam
{
    public RoleBase SwitchRole;
    //public RoleActionEnum NextAction;

    //public SwitchRoleActionParam(RoleBase switchRole, RoleActionEnum nextAction)
    //{
    //    SwitchRole = switchRole;
    //    NextAction = nextAction;
    //}
}

public delegate void TDelegate();
public delegate void TDelegate<T>(T t);
//public delegate void TDelegate<T, K>(T t, K k);
