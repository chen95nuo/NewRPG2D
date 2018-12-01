using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ServerBuildData
{
    public int id;//数据ID
    public int RoomId;//房间ID
    public Vector2 buildingPoint;//房间位置
    public float Stock = 0;
    public bool ConstructionType = false;
    public int levelUPTime;
    public int levelUpId;
    public int[] roleID;

    public ServerBuildData() { }
    public ServerBuildData(LocalBuildingData data)
    {
        this.id = data.id;
        this.RoomId = data.buildingData.ItemId;
        this.buildingPoint = data.buildingPoint;
        this.Stock = data.Stock;
    }
    public ServerBuildData(int id, int RoomId, Vector2 point, float Stock, int levelUPTime, int levelUpId)
    {
        this.id = id;
        this.RoomId = RoomId;
        this.buildingPoint = point;
        this.Stock = Stock;
        this.levelUPTime = levelUPTime;
        this.levelUpId = levelUpId;
        if (levelUPTime != 0)
        {
            ConstructionType = true;
        }
    }
}

public class ServerHallRoleData
{
    public int RoomId;
    public HallRoleData role;
    public ServerHallRoleData(int id, HallRoleData data)
    {
        this.RoomId = id;
        this.role = data;
    }
}

public class ServerRoleData
{
    public int sex;//性别
    public int[] Skin;//皮肤ID

}

[System.Serializable]
public class LocalBuildingData
{
    public int id;//房间ID
    public Vector2 buildingPoint;//房间位置
    public BuildingData buildingData;//房间ID
    public float Stock = 0;
    public float speedProd = 0;//产出速度
    public bool ConstructionType = false;
    public HallRoleData[] roleData;
    public RoomMgr currentRoom;
    public int leftTime;//升级时间

    public float AllRoleProduction()
    {
        float index = 0;
        for (int i = 0; i < roleData.Length; i++)
        {
            if (roleData[i] != null)
            {
                switch (buildingData.RoomName)
                {
                    case BuildRoomName.Gold:
                        index += roleData[i].GoldProduce;
                        break;
                    case BuildRoomName.Food:
                        index += roleData[i].FoodProduce;
                        break;
                    case BuildRoomName.Mana:
                        index += roleData[i].ManaProduce;
                        break;
                    case BuildRoomName.Wood:
                        index += roleData[i].WoodProduce;
                        break;
                    case BuildRoomName.Iron:
                        index += roleData[i].IronProduce;
                        break;
                    default:
                        break;
                }
            }
        }
        return index;
    }
    public float AllRomeProduction()
    {
        float temp = 0;
        temp += AllRoleProduction();
        temp += buildingData.Param1;

        return temp;
    }

    public int ScreenAllYeild(RoleAttribute type, bool isUp)
    {
        Debug.Log("角色产量筛选");
        int index = -1;
        float temp = 0;
        if (isUp)
        {
            temp = 0;
        }
        else
        {
            temp = 100;
        }
        //RoleAttribute type = BuildingRoleHelper();
        if (type == RoleAttribute.Nothing)
        {
            return roleData.Length - 1;
        }
        for (int i = 0; i < roleData.Length; i++)
        {
            if (roleData[i] == null)
            {
                return -1;
            }
            if (isUp)
            {
                float temp_1 = ScreenProduceHelper(type, roleData[i]);
                if (temp < temp_1)
                {
                    temp = temp_1;
                    index = i;
                }
            }
            else
            {
                float temp_1 = ScreenProduceHelper(type, roleData[i]);
                if (temp > temp_1)
                {
                    temp = temp_1;
                    index = i;
                }
            }
        }
        return index;
    }
    public float ScreenProduceHelper(RoleAttribute type, HallRoleData data)
    {
        switch (type)
        {
            case RoleAttribute.Gold:
                return data.GoldProduce;
            case RoleAttribute.Food:
                return data.FoodProduce;
            case RoleAttribute.Mana:
                return data.ManaProduce;
            case RoleAttribute.ManaSpeed:
                return data.ManaSpeed;
            case RoleAttribute.Wood:
                return data.WoodProduce;
            case RoleAttribute.Iron:
                return data.IronProduce;
            case RoleAttribute.DPS:
                return data.Attack;
            case RoleAttribute.HP:
                return data.HP;
            case RoleAttribute.Max:
                return data.Star;
            default:
                break;
        }
        return 0;
    }
    public LocalBuildingData() { }
    public LocalBuildingData(Vector2 point, BuildingData data)
    {
        this.buildingPoint = point;
        this.buildingData = data;
        int maxRole = CheckPlayerInfo.instance.ChickRoomSize(data);
        roleData = new HallRoleData[maxRole];
    }
    public LocalBuildingData(int id, Vector2 point, BuildingData roomData, bool ConstructionType)
    {
        this.id = id;
        this.buildingPoint = point;
        this.buildingData = roomData;
        this.ConstructionType = ConstructionType;
        int maxRole = CheckPlayerInfo.instance.ChickRoomSize(roomData);
        roleData = new HallRoleData[maxRole];
    }
    public LocalBuildingData(int id, Vector2 point, BuildingData roomData, float Stock)
    {
        this.id = id;
        this.buildingPoint = point;
        this.buildingData = roomData;
        this.Stock = Stock;
        int maxRole = CheckPlayerInfo.instance.ChickRoomSize(roomData);
        roleData = new HallRoleData[maxRole];
    }
    public LocalBuildingData(proto.SLGV1.RoomInfo data, int id)
    {
        this.id = id;
        this.buildingData = BuildingDataMgr.instance.GetDataByItemId<BuildingData>(data.id);
        this.buildingPoint = new Vector2(data.xFloorOriginOffset[0], data.xFloorOriginOffset[1]);
        this.leftTime = data.leftTime;

        int maxRole = CheckPlayerInfo.instance.ChickRoomSize(buildingData);
        this.roleData = new HallRoleData[maxRole];

        for (int i = 0; i < data.residents.Count; i++)
        {
            if (data.residents[i] != 0)
            {
                HallRoleData roleData = HallRoleMgr.instance.GetRoleData(data.residents[i]);
                this.roleData[i] = roleData;
            }
        }
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
    //添加是否有障碍物
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



