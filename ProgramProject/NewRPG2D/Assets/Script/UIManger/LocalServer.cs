/*
 * 这是一个假的本地服务器 用于测试时模拟服务器发送的消息
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class LocalServer : TSingleton<LocalServer>
{
    private List<ServerBuildData> serverRoom = new List<ServerBuildData>();
    private Dictionary<int, RoomMgr> buildNumber = new Dictionary<int, RoomMgr>();

    public void AddRoom(ServerBuildData data)
    {
        for (int i = 0; i < serverRoom.Count; i++)
        {
            if (serverRoom[i] == data)
            {
                return;
            }
        }
        serverRoom.Add(data);
        Debug.Log(data.buildingData.RoomType);
        SendSpaceEvent(data);
    }
    public void ReplaceRoom(ServerBuildData data_1, ServerBuildData data_2)
    {
        serverRoom.Remove(data_1);
        serverRoom.Add(data_2);
    }

    public void GetNumber(RoomMgr data)
    {
        int a = CTimerManager.instance.AddListener(1f, -1, JustTime);
        buildNumber.Add(a, data);
    }

    private void JustTime(int key)
    {
        RoomMgr thisRoom = buildNumber[key];
        thisRoom.buildingData.Param4 += thisRoom.buildingData.Param1 / 60 / 60 * 30;
        if (thisRoom.buildingData.Param4 / thisRoom.buildingData.Param1 * 100 > 3)
        {
            thisRoom.GetRoomMaterial((int)thisRoom.buildingData.Param4);
        }
    }
    public bool SetNumber(BuildingData data)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        BuildingData thisRoom = new BuildingData();
        BuildRoomType type = BuildRoomType.Nothing;

        switch (data.RoomType)
        {
            case BuildRoomType.Restaurant:
                type = BuildRoomType.FoodSpace;
                break;
            default:
                break;
        }
        for (int i = 0; i < serverRoom.Count; i++)
        {
            if (serverRoom[i].buildingData.RoomType == type)
            {
                thisRoom = serverRoom[i].buildingData;
            }
        }
        switch (data.RoomType)
        {
            case BuildRoomType.Restaurant:
                //如果食物空间加上仓库空间 小于食物数量 那么说明仓库满了
                if (player.foodSpace + (int)thisRoom.Param2
                    < player.food + data.Param4)
                {
                    //填满仓库
                    data.Param4 -= (player.food - (player.foodSpace + (int)thisRoom.Param2));
                    player.food = player.foodSpace + (int)thisRoom.Param2;
                    return true;
                }
                else
                {
                    player.food += (int)data.Param4;
                    data.Param4 -= (int)data.Param4;
                    Debug.Log("剩余" + data.Param4);
                    return true;
                }
            default:
                break;
        }
        return false;
    }

    private void SendSpaceEvent(ServerBuildData data)
    {
        switch (data.buildingData.RoomType)
        {
            case BuildRoomType.GlodSpace:
                HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.GlodSpace, data.buildingData);
                break;
            case BuildRoomType.FoodSpace:
                HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.FoodSpace, data.buildingData);
                break;
            case BuildRoomType.ManaSpace:
                HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.ManaSpace, data.buildingData);
                break;
            case BuildRoomType.WoodSpace:
                HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.WoodSpace, data.buildingData);
                break;
            case BuildRoomType.IronSpace:
                HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.IronSpace, data.buildingData);
                break;
            default:
                break;
        }
    }
}
