/*
 * 这是本地模拟的服务器 将服务器数据存放于此
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

    public List<ServerBuildData> ServerRoom
    {
        get
        {
            return serverRoom;
        }
    }

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
        ChickPlayerInfo.instance.ChickBuildDicAdd(data);
        if (data.buildingData.RoomType == RoomType.Production)
        {
            PlayerData playData = GetPlayerData.Instance.GetData();
            Debug.Log("新建生产类建筑,可能为仓库 若为仓库将物资转移");
            switch (data.buildingData.RoomName)
            {
                case BuildRoomName.GoldSpace:
                    int index = (int)data.buildingData.Param2 - playData.Gold;
                    if (index < 0)
                    {
                        data.Stock = data.buildingData.Param2;
                        playData.Gold -= (int)data.buildingData.Param2;
                        HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, data);
                        return;
                    }
                    data.Stock = playData.Gold;
                    playData.Gold = 0;
                    break;
                case BuildRoomName.FoodSpace:
                    int index_1 = (int)data.buildingData.Param2 - playData.Food;
                    if (index_1 < 0)
                    {
                        data.Stock = data.buildingData.Param2;
                        playData.Food -= (int)data.buildingData.Param2;
                        HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, data);
                        return;
                    }
                    data.Stock = playData.Food;
                    playData.Food = 0;
                    break;
                case BuildRoomName.WoodSpace:
                    int index_2 = (int)data.buildingData.Param2 - playData.Wood;
                    if (index_2 < 0)
                    {
                        data.Stock = data.buildingData.Param2;
                        playData.Wood -= (int)data.buildingData.Param2;
                        HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, data);
                        return;
                    }
                    data.Stock = playData.Wood;
                    playData.Wood = 0;
                    break;
                case BuildRoomName.ManaSpace:
                    int index_3 = (int)data.buildingData.Param2 - playData.Mana;
                    if (index_3 < 0)
                    {
                        data.Stock = data.buildingData.Param2;
                        playData.Mana -= (int)data.buildingData.Param2;
                        HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, data);
                        return;
                    }
                    data.Stock = playData.Mana;
                    playData.Mana = 0;

                    break;
                case BuildRoomName.IronSpace:
                    int index_4 = (int)data.buildingData.Param2 - playData.Iron;
                    if (index_4 < 0)
                    {
                        data.Stock = data.buildingData.Param2;
                        playData.Iron -= (int)data.buildingData.Param2;
                        HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, data);
                        return;
                    }
                    data.Stock = playData.Iron;
                    playData.Iron = 0;
                    break;
                default:
                    break;
            }
        }
        HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, data.buildingData.RoomName);
    }
    public void GetNewRoom(List<ServerBuildData> rooms)
    {
        serverRoom.Clear();
        for (int i = 0; i < rooms.Count; i++)
        {
            serverRoom.Add(rooms[i]);
        }
        HallEventManager.instance.SendEvent<List<ServerBuildData>>(HallEventDefineEnum.AddBuild, serverRoom);
        ChickPlayerInfo.instance.ChickBuildDic(serverRoom);
    }

    /// <summary>
    /// 删除房间
    /// </summary>
    /// <param name="data"></param>
    public bool RemoveRoom(ServerBuildData data)
    {
        bool isTrue = serverRoom.Remove(data);
        return isTrue;
    }

    /// <summary>
    /// 删除1 添加2
    /// </summary>
    /// <param name="data_1"></param>
    /// <param name="data_2"></param>
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
        if (thisRoom.levelUp == true)
        {
            return;
        }
        IProduction index = thisRoom.GetComponent<IProduction>();

        index.Stock += index.Yield / 60 / 60 * 30;
        index.Stock = Mathf.Clamp(index.Stock, 0, thisRoom.buildingData.Param2);
        if (index.Stock / thisRoom.buildingData.Param1 * 100 > 3)
        {
            thisRoom.GetNumber((int)index.Stock);
        }
    }

    public void RoomLevelTime(RoomMgr data, int time)
    {
        Debug.Log("升级需要时间" + time);
        int index = CTimerManager.instance.AddListener(1f, time, LevelTime);
        buildNumber.Add(index, data);
    }

    public void RemoveLevelTime(int sequenceTime)
    {
        CTimerManager.instance.RemoveLister(sequenceTime);
    }

    public void LevelTime(int key)
    {
        RoomMgr thisRoom = buildNumber[key];
        thisRoom.LevelNowTime();
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
                && serverRoom[i].buildingData.RoomName.ToString() == data.RoomName + "Space")
            {
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
                StorageRoom.Stock = index;
                dataIndex.Stock -= index;
                HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, StorageRoom);
                return true;
            }
            int temp = (int)Mathf.Clamp(StorageRoom.buildingData.Param2 - index, 0, StorageRoom.buildingData.Param2);
            Debug.Log(temp);
            dataIndex.Stock -= temp;
            index += temp;
            StorageRoom.Stock = index;
            HallEventManager.instance.SendEvent<ServerBuildData>(HallEventDefineEnum.ChickStock, StorageRoom);
        }

        #region 区分类型
        if (data.RoomName == BuildRoomName.Gold)
        {
            if (player.Gold + (int)dataIndex.Stock <= player.GoldSpace)
            {
                player.Gold += (int)dataIndex.Stock;
                dataIndex.Stock -= (int)dataIndex.Stock;
                Debug.Log(player.Gold);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.GoldSpace - player.Gold, 0, player.GoldSpace);
            player.Gold += temp;
            dataIndex.Stock -= temp;
            return false;
        }
        else if (data.RoomName == BuildRoomName.Food)
        {
            if (player.Food + (int)dataIndex.Stock <= player.GoldSpace)
            {
                player.Food += (int)dataIndex.Stock;
                dataIndex.Stock -= (int)dataIndex.Stock;
                Debug.Log(player.Food);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.FoodSpace - player.Food, 0, player.FoodSpace);
            player.Food += temp;
            dataIndex.Stock -= temp;
            return false;
        }
        else if (data.RoomName == BuildRoomName.Mana)
        {
            if (player.Mana + (int)dataIndex.Stock <= player.ManaSpace)
            {
                player.Mana += (int)dataIndex.Stock;
                dataIndex.Stock -= (int)dataIndex.Stock;

                Debug.Log(player.Mana);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.ManaSpace - player.Mana, 0, player.ManaSpace);
            player.Mana += temp;
            dataIndex.Stock -= temp;
            return false;
        }
        else if (data.RoomName == BuildRoomName.Wood)
        {
            if (player.Wood + (int)dataIndex.Stock <= player.WoodSpace)
            {
                player.Wood += (int)dataIndex.Stock;
                dataIndex.Stock -= (int)dataIndex.Stock;

                Debug.Log(player.Wood);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.WoodSpace - player.Wood, 0, player.WoodSpace);
            player.Wood += temp;
            dataIndex.Stock -= temp;
            return false;
        }
        else if (data.RoomName == BuildRoomName.Iron)
        {
            if (player.Iron + (int)dataIndex.Stock <= player.IronSpace)
            {
                player.Iron += (int)dataIndex.Stock;
                dataIndex.Stock -= (int)dataIndex.Stock;
                Debug.Log(player.Iron);
                return true;
            }
            int temp = (int)Mathf.Clamp(player.IronSpace - player.Iron, 0, player.IronSpace);
            player.Iron += temp;
            dataIndex.Stock -= temp;
            return false;
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
}
