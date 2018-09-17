using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ServerBuildData
{
    public Vector2 buildingPoint;
    public BuildingData buildingData;

    public ServerBuildData() { }
    public ServerBuildData(Vector2 point, BuildingData data)
    {
        this.buildingPoint = point;
        this.buildingData = data;
    }
}
/// <summary>
/// 墙面信息
/// </summary>
public class BuildPoint
{
    public BuildingType pointType;//当前位置类型
    public Transform pointWall;//当前位置墙体引用
    public RoomMgr roomMgr;//当前位置房间信息
    public BuildTip tip;//当前位置提示框信息
}

/// <summary>
/// 空位信息
/// </summary>
[System.Serializable]
public class EmptyPoint
{
    public Vector2 startPoint;//起点位置
    public int endPoint;//结束位置
    public int emptyNumber;//空位数量
    public RoomMgr roomData;//链接的建筑信息

    public EmptyPoint()
    {
        //startPoint = new Vector2(6, 1);
        //endPoint = 16;
        //emptyNumber = 9;
    }

    public EmptyPoint(Vector2 startPoint, int endPoint, int emptyNumber, RoomMgr roomData)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.emptyNumber = emptyNumber;
        this.roomData = roomData;
    }
}
