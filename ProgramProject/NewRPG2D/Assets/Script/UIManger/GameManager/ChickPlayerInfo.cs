/*
 * 这是一个本地玩家参数单例
 * 用于整合玩家全部资源
 * 整合建筑类型和数量
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickPlayerInfo : TSingleton<ChickPlayerInfo>
{
    public Dictionary<BuildRoomName, ServerBuildData[]> dic = new Dictionary<BuildRoomName, ServerBuildData[]>();

    /// <summary>
    /// 将建筑数量和信息格式化
    /// </summary>
    public void ChickBuilding()
    {
        dic.Clear();
        var index = BuildingDataMgr.instance.GetBuilding();
        foreach (var item in index)
        {
            if (item.Key == BuildRoomName.Stairs)
            {
                dic.Add(item.Key, new ServerBuildData[30]);
                break;
            }
            dic.Add(item.Key, new ServerBuildData[item.Value.Length]);
        }
        Debug.Log(dic);
    }

    /// <summary>
    /// 获取服务器上的建筑数据后 将其转为本地信息
    /// </summary>
    /// <param name="s_BuildData"></param>
    public void ChickBuildDic(List<ServerBuildData> s_BuildData)
    {
        for (int i = 0; i < s_BuildData.Count; i++)
        {
            for (int j = 0; j < dic[s_BuildData[i].buildingData.RoomName].Length; j++)
            {
                if (dic[s_BuildData[i].buildingData.RoomName][j] == null)
                {
                    dic[s_BuildData[i].buildingData.RoomName][j] = s_BuildData[i];
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 有新的建筑 给字典添加信息
    /// </summary>
    public void ChickBuildDicAdd(ServerBuildData data)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i] == null)
            {
                Debug.Log("有空位 添加");
                dic[data.buildingData.RoomName][i] = data;
                HallEventManager.instance.SendEvent<RoomType>(HallEventDefineEnum.ChickBuild, data.buildingData.RoomType);
                return;
            }
        }
        Debug.LogError("房间数量超限");
    }

    /// <summary>
    /// 建筑升级或者改变 改变字典中的信息
    /// </summary>
    /// <param name="data">原数据</param>
    /// <param name="changeData">改变后的数据</param>
    public void ChickBuildDicChange(ServerBuildData data, ServerBuildData changeData)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i].buildingData == data.buildingData
                && dic[data.buildingData.RoomName][i].buildingPoint == data.buildingPoint)
            {
                Debug.Log("找到了 转换");
                dic[data.buildingData.RoomName][i] = changeData;
            }
        }
    }

    /// <summary>
    /// 删除建筑信息
    /// </summary>
    public void ChickBuildMerge(ServerBuildData data)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i].buildingData == data.buildingData
                && dic[data.buildingData.RoomName][i].buildingPoint == data.buildingPoint)
            {
                Debug.Log("找到了 删除");
                dic[data.buildingData.RoomName][i] = null;
            }
        }
    }

    /// <summary>
    /// 给主菜单建造房间提供当前房间数量
    /// </summary>
    /// <returns></returns>
    public int[] GetBuildiDicInfo(BuildingData data)
    {
        int[] index = new int[2];
        PlayerData player = GetPlayerData.Instance.GetData();
        if (data.RoomName != BuildRoomName.Stairs)
        {
            for (int i = 0; i < data.UnlockLevel.Length; i++)
            {
                if (data.UnlockLevel[i] <= player.MainHallLevel)
                {
                    index[1]++;//获取当前等级的可建造数
                }
            }
        }
        else
        {
            index[1] = 4 + player.MainHallLevel;
        }
        for (int i = 0; i < dic[data.RoomName].Length; i++)
        {
            if (dic[data.RoomName][i] != null)
            {
                index[0]++;//获取该类建筑已建造数量
                if (dic[data.RoomName][i].buildingData.RoomType == RoomType.Production)
                {
                    switch (dic[data.RoomName][i].buildingData.RoomSize)
                    {
                        case 6: index[0]++; break;
                        case 9: index[0] += 2; break;
                        default:
                            break;
                    }
                }
            }
        }
        return index;
    }

    /// <summary>
    /// 获取某种建筑的全部产量
    /// </summary>
    /// <returns></returns>
    public int GetAllYield(BuildRoomName name)
    {
        int index = 0;
        for (int i = 0; i < dic[name].Length; i++)
        {
            if (dic[name][i] != null)
            {
                index += (int)dic[name][i].Yield;
            }
        }
        return index;
    }

    /// <summary>
    /// 获取某类资源的总值
    /// </summary>
    /// <returns></returns>
    public int GetAllStock(BuildRoomName name)
    {
        int index = 0;
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (name)
        {
            case BuildRoomName.Gold:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    index = player.Gold;
                    return index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.Food:
                if (dic[BuildRoomName.FoodSpace][0] == null)
                {
                    index = player.Food;
                    return index;
                }
                index = (int)dic[BuildRoomName.FoodSpace][0].Stock + player.Food;
                break;
            case BuildRoomName.Wood:
                if (dic[BuildRoomName.WoodSpace][0] == null)
                {
                    index = player.Wood;
                }
                index = (int)dic[BuildRoomName.WoodSpace][0].Stock + player.Wood;
                break;
            case BuildRoomName.Mana:
                if (dic[BuildRoomName.ManaSpace][0] == null)
                {
                    index = player.Mana;
                    return index;
                }
                index = (int)dic[BuildRoomName.ManaSpace][0].Stock + player.Mana;
                break;
            case BuildRoomName.Iron:
                if (dic[BuildRoomName.IronSpace][0] == null)
                {
                    index = player.Iron;
                    return index;
                }
                index = (int)dic[BuildRoomName.IronSpace][0].Stock + player.Iron;
                break;
            default:
                break;
        }
        return index;
    }

    /// <summary>
    /// 使用某类资源
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public void UseAllStock(BuildRoomName name, int index)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (name)
        {
            case BuildRoomName.Gold:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.GoldSpace:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.Food:
                if (dic[BuildRoomName.FoodSpace][0] == null)
                {
                    player.Food -= index;
                }
                if (dic[BuildRoomName.FoodSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.FoodSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.FoodSpace][0].Stock;
                    dic[BuildRoomName.FoodSpace][0].Stock = 0;
                    player.Food -= index;
                }
                index = (int)dic[BuildRoomName.FoodSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.FoodSpace:

                break;
            case BuildRoomName.Mana:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.ManaSpace:
                break;
            case BuildRoomName.Wood:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.WoodSpace:
                break;
            case BuildRoomName.Iron:
                if (dic[BuildRoomName.GoldSpace][0] == null)
                {
                    player.Gold -= index;
                }
                if (dic[BuildRoomName.GoldSpace][0].Stock - index > 0)
                {
                    dic[BuildRoomName.GoldSpace][0].Stock -= index;
                }
                else
                {
                    index -= (int)dic[BuildRoomName.GoldSpace][0].Stock;
                    dic[BuildRoomName.GoldSpace][0].Stock = 0;
                    player.Gold -= index;
                }
                index = (int)dic[BuildRoomName.GoldSpace][0].Stock + player.Gold;
                break;
            case BuildRoomName.IronSpace:
                break;
            default:
                break;
        }
    }
}
