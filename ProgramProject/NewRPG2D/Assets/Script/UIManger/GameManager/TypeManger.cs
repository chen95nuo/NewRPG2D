using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public enum HallEventDefineEnum
{
    AddBuild,//添加建筑
    ChickBuild,//检查建筑
    ChickStock,//检查空间
    CameraMove,//相机移动了
    EditMode,//建造模式
    EditMgr,//建造模式辅助
    InEditMode,//进入建造模式
    ClearAllRoom,//清除所有房间
    CloseRoomLock,//关闭房间锁定
    diamondsSpace,//钻石检测
    UILockRoomTip,//检查房间锁定
    ChickRoomMerge,//检查房间合并
    EventMax
}

