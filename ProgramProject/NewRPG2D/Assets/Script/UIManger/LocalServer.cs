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
    private PlayerData player;

    public void AddRoom(ServerBuildData data)
    {
        for (int i = 0; i < serverRoom.Count; i++)
        {
            if (serverRoom[i].buildingData == data.buildingData
                && serverRoom[i].buildingPoint == data.buildingPoint)
            {
                Debug.LogError("重复了跳过");
                return;
            }
        }
        Debug.Log("没有重复继续运行");
        serverRoom.Add(data);
        SendSpaceEvent(data.buildingData);
    }
    public void GetNewRoom(List<ServerBuildData> rooms)
    {
        serverRoom = rooms;
        HallEventManager.instance.SendEvent<List<ServerBuildData>>(HallEventDefineEnum.AddBuild, serverRoom);
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
        IProduction index = thisRoom.GetComponent<IProduction>();

        index.Stock += index.Yield / 60 / 60 * 30;
        index.Stock = Mathf.Clamp(index.Stock, 0, thisRoom.buildingData.Param2);
        if (index.Stock / thisRoom.buildingData.Param1 * 100 > 3)
        {
            index.GetNumber((int)index.Stock);
        }
    }
    /*  
     *  添加的时候先加仓库仓库不够在使用玩家储存
     *  减少的时候先减少玩家库存在减少仓库
     *  返回值是是否使用成功
     */
    public bool SetNumber(RoomMgr roomData)
    {
        BuildingData data = roomData.buildingData;
        if (player == null)
        {
            player = GetPlayerData.Instance.GetData();//获取玩家数据
        }
        ServerBuildData StorageRoom = null;//对应仓库

        float index = 0;
        for (int i = 0; i < serverRoom.Count; i++)
        {
            if (serverRoom[i].buildingData.RoomType == RoomType.Production
                && serverRoom[i].buildingData.RoomName == data.RoomName + "Space")
            {
                Debug.Log("有仓库 仓库Name :" + serverRoom[i].Stock);
                StorageRoom = serverRoom[i];
                index = serverRoom[i].Stock;
                break;
            }
        }
        IProduction dataIndex = (IProduction)roomData;
        if (StorageRoom != null)
        {
            if (index + (int)dataIndex.Stock <= StorageRoom.buildingData.Param2)
            {
                index = (int)Mathf.Clamp(index + dataIndex.Stock, 0, data.Param2);
                Debug.Log(index);
                StorageRoom.Stock = index;
                HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, StorageRoom);
                SendSpaceEvent(data);
                return true;
            }
            int temp = (int)Mathf.Clamp(StorageRoom.buildingData.Param2 - index, 0, StorageRoom.buildingData.Param2);
            Debug.Log(temp);
            dataIndex.Stock -= temp;
            index += temp;
            StorageRoom.Stock = index;
            HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, StorageRoom);
            SendSpaceEvent(data);
        }

        #region 区分类型
        if (data.RoomName == "Gold")
        {
            if (player.Gold + (int)dataIndex.Stock <= player.GoldSpace)
            {
                player.Gold += (int)dataIndex.Stock;
                Debug.Log(player.Gold);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.GoldSpace - player.Gold, 0, player.GoldSpace);
            player.Gold += temp;
            dataIndex.Stock -= temp;
            return false;
            //return SetNumberType(player.Gold, player.GoldSpace, dataIndex);
        }
        else if (data.RoomName == "Food")
        {
            if (player.Food + (int)dataIndex.Stock <= player.GoldSpace)
            {
                player.Food += (int)dataIndex.Stock;
                Debug.Log(player.Food);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.FoodSpace - player.Food, 0, player.FoodSpace);
            player.Food += temp;
            dataIndex.Stock -= temp;
            return false;
            //return SetNumberType(player.Food, player.FoodSpace, dataIndex);
        }
        else if (data.RoomName == "Mana")
        {
            if (player.Mana + (int)dataIndex.Stock <= player.ManaSpace)
            {
                player.Mana += (int)dataIndex.Stock;
                Debug.Log(player.Mana);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.ManaSpace - player.Mana, 0, player.ManaSpace);
            player.Mana += temp;
            dataIndex.Stock -= temp;
            return false;
            //return SetNumberType(player.Mana, player.ManaSpace, dataIndex);
        }
        else if (data.RoomName == "Wood")
        {
            if (player.Wood + (int)dataIndex.Stock <= player.WoodSpace)
            {
                player.Wood += (int)dataIndex.Stock;
                Debug.Log(player.Wood);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.WoodSpace - player.Wood, 0, player.WoodSpace);
            player.Wood += temp;
            dataIndex.Stock -= temp;
            return false;
            //return SetNumberType(player.Wood, player.WoodSpace, dataIndex);
        }
        else if (data.RoomName == "Iron")
        {
            if (player.Iron + (int)dataIndex.Stock <= player.IronSpace)
            {
                player.Iron += (int)dataIndex.Stock;
                Debug.Log(player.Iron);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.IronSpace - player.Iron, 0, player.IronSpace);
            player.Iron += temp;
            dataIndex.Stock -= temp;
            return false;
            //return SetNumberType(player.Iron, player.IronSpace, dataIndex);
        }
        #endregion
        return false;
    }

    private bool SetNumberType(int index, int indexSpace, IProduction dataIndex)
    {
        if (index + (int)dataIndex.Stock <= indexSpace)
        {
            index += (int)dataIndex.Stock;
            Debug.Log(index);
            return true;
        }
        int temp = (int)Mathf.Clamp(indexSpace - index, 0, indexSpace);
        index += temp;
        dataIndex.Stock -= temp;
        return false;
    }

    private void SendSpaceEvent(BuildingData data)
    {
        if (data.RoomName == "Gold")
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.GoldSpace);
        }
        else if (data.RoomName == "Food")
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.FoodSpace);
        }
        else if (data.RoomName == "Mana")
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.ManaSpace);
        }
        else if (data.RoomName == "Wood")
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.WoodSpace);
        }
        else if (data.RoomName == "Iron")
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.IronSpace);
        }
    }
}
