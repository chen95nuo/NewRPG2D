using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class ServerBuildData
{
    public Vector2 buildingPoint;//房间位置
    public BuildingData buildingData;//房间ID
    public float Yield = 0;
    public float Stock = 0;
    public bool levelUp = false;

    public ServerBuildData() { }
    public ServerBuildData(Vector2 point, BuildingData data)
    {
        this.buildingPoint = point;
        this.buildingData = data;
    }
    public ServerBuildData(Vector2 point, BuildingData data, float Yield, float Stock)
    {
        this.buildingPoint = point;
        this.buildingData = data;
        this.Yield = Yield;
        this.Stock = Stock;
    }
}

[System.Serializable]
public class LocalBuildingData
{
    public int id;//房间ID
    public Vector2 buildingPoint;//房间位置
    public BuildingData buildingData;//房间ID
    public float Stock = 0;
    public bool ConstructionType = false;

    public LocalBuildingData() { }
    public LocalBuildingData(int id, Vector2 point, BuildingData data)
    {
        this.id = id;
        this.buildingPoint = point;
        this.buildingData = data;
    }
    public LocalBuildingData(int id, Vector2 point, BuildingData data, float Stock)
    {
        this.id = id;
        this.buildingPoint = point;
        this.buildingData = data;
        this.Stock = Stock;
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

[System.Serializable]
public class EditMergeRoomData
{
    public LocalBuildingData room_1;
    public LocalBuildingData room_2;
    public LocalBuildingData room_3;
    public LocalBuildingData mergeRoom;

    public EditMergeRoomData() { }
}


