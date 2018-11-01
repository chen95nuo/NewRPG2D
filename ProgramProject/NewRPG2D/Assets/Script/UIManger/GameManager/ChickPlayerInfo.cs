/*
 * 这是一个本地玩家参数单例
 * 用于整合玩家全部资源
 * 整合建筑类型和数量
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;
using Assets.Script.UIManger;

public class ChickPlayerInfo : TSingleton<ChickPlayerInfo>
{
    public Dictionary<BuildRoomName, LocalBuildingData[]> dic = new Dictionary<BuildRoomName, LocalBuildingData[]>();
    private Dictionary<int, LevelUPHelper> buildNumber = new Dictionary<int, LevelUPHelper>();//房间序号 用于储存施工中的房间
    private List<LocalBuildingData> AllBuilding = new List<LocalBuildingData>();//储存全部已经建造的房间
    private Dictionary<int, LocalBuildingData> production = new Dictionary<int, LocalBuildingData>();//产出房间
    private Dictionary<MagicName, int> MagicLevel = new Dictionary<MagicName, int>();
    private List<LocalBuildingData> storage;
    private int buildingIdIndex = 0;

    private int EventKey = 0;

    private int BuildingIdIndex
    {
        get
        {
            buildingIdIndex++;
            return buildingIdIndex;
        }
    }

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
                dic.Add(item.Key, new LocalBuildingData[30]);
                continue;
            }
            dic.Add(item.Key, new LocalBuildingData[item.Value.Length]);
        }
    }

    /// <summary>
    /// 编辑模式保存了
    /// </summary>
    public void ChickEditSave(List<LocalBuildingData> editAllBuilding, List<LocalBuildingData> ChangeBuilding)
    {
        List<LocalBuildingData> newRoom = new List<LocalBuildingData>();
        Debug.Log("筛选出新的房间和将移位的房间位置改变");
        for (int i = 0; i < editAllBuilding.Count; i++)
        {
            if (editAllBuilding[i].id == 0)
            {
                newRoom.Add(editAllBuilding[i]);
            }
            else
            {
                for (int j = 0; j < AllBuilding.Count; j++)
                {
                    if (editAllBuilding[i].id == AllBuilding[j].id)
                    {
                        AllBuilding[j].buildingPoint = editAllBuilding[i].buildingPoint;
                        break;
                    }
                }
            }
        }
        editAllBuilding.Clear();
        List<LocalBuildingData> ChangeMainBuilding = new List<LocalBuildingData>();
        //找到所有改变过的房间
        for (int i = 0; i < ChangeBuilding.Count; i++)
        {
            for (int j = 0; j < AllBuilding.Count; j++)
            {
                if (ChangeBuilding[i].id == AllBuilding[j].id)
                {
                    ChangeMainBuilding.Add(AllBuilding[j]);
                    break;
                }
            }
        }
        ChangeBuilding.Clear();
        List<EditSaveHelper> ChangeData = new List<EditSaveHelper>();
        Debug.Log("将原房间筛选一遍 让同类型的放在一起");
        for (int i = 0; i < newRoom.Count; i++)
        {
            int size = newRoom[i].buildingData.RoomSize / 3;
            for (int j = 0; j < ChangeMainBuilding.Count; j++)
            {
                if (newRoom[i].buildingData.ItemId == ChangeMainBuilding[j].buildingData.SplitID)
                {
                    float stock = ChangeMainBuilding[j].Stock / (ChangeMainBuilding[j].buildingData.RoomSize / 3);
                    newRoom[i].Stock = stock;
                    ChickEditSaveRoleHelper(newRoom[i], ChangeMainBuilding[j].roleData);
                    break;
                }
                else if (newRoom[i].buildingData.MergeID == ChangeMainBuilding[j].buildingData.ItemId)
                {
                    float stock = ChangeMainBuilding[j].Stock / 3;
                    newRoom[i].Stock = stock * size;
                    if (size != 2)
                    {
                        Debug.LogError("错误  Size不等于2");
                    }
                    ChickEditSaveRoleHelper(newRoom[i], ChangeMainBuilding[j].roleData);
                    break;
                }
                else if (newRoom[i].buildingData.SplitID == ChangeMainBuilding[j].buildingData.ItemId)
                {
                    newRoom[i].Stock += ChangeMainBuilding[j].Stock;
                    ChickEditSaveRoleHelper(newRoom[i], ChangeMainBuilding[j].roleData);
                    RemoveBuilding(ChangeMainBuilding[j]);
                    ChangeMainBuilding.RemoveAt(j);
                    j--;
                }
            }
        }
        for (int i = 0; i < ChangeMainBuilding.Count; i++)
        {
            RemoveBuilding(ChangeMainBuilding[i]);
        }
        MainCastle.instance.RefreshBuilding(AllBuilding, newRoom);
    }
    public void ChickEditSaveRoleHelper(LocalBuildingData data, HallRoleData[] datas)
    {
        int index = 0;
        for (int i = 0; i < datas.Length; i++)
        {
            if (index < data.roleData.Length
                && datas[i] != null)
            {
                for (int j = index; j < data.roleData.Length; j++)
                {
                    if (data.roleData[j] == null)
                    {
                        index = j;
                        data.roleData[j] = datas[i];
                        data.roleData[j].currentRoom = null;
                        datas[i] = null;
                        break;
                    }
                }
            }
        }
    }



    /// <summary>
    /// 获取服务器上的建筑数据后 将其转为本地信息
    /// </summary>
    /// <param name="s_BuildData"></param>
    public void ChickBuildDic(List<ServerBuildData> s_BuildData)
    {
        for (int i = 0; i < s_BuildData.Count; i++)
        {
            BuildingData data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(s_BuildData[i].RoomId);
            if (s_BuildData[i].id > buildingIdIndex)
            {
                buildingIdIndex = s_BuildData[i].id;
            }
            for (int j = 0; j < dic[data.RoomName].Length; j++)
            {
                if (dic[data.RoomName][j] == null)
                {
                    dic[data.RoomName][j] = new LocalBuildingData(s_BuildData[i].id, s_BuildData[i].buildingPoint, data, s_BuildData[i].Stock);
                    AllBuilding.Add(dic[data.RoomName][j]);
                    MainCastle.instance.InstanceRoom(dic[data.RoomName][j], s_BuildData[i]);
                    if (ChickStorage(dic[data.RoomName][j]))
                    {
                        Debug.Log(dic[data.RoomName][j]);
                        dic[data.RoomName][j].Stock = s_BuildData[i].Stock;
                        ThisStorage(dic[data.RoomName][j]);
                        HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, data.RoomName);
                    }
                    break;
                }
            }
        }
    }
    public void ChickRoleDic(int roomID, HallRole role)
    {
        List<RoomMgr> allRoom = MainCastle.instance.allroom;
        for (int i = 0; i < allRoom.Count; i++)
        {
            if (allRoom[i].Id == roomID)
            {
                allRoom[i].AddRole(role);
            }
        }
    }
    public void RemoveBaby(HallRole role)
    {
        List<RoomMgr> allRoom = MainCastle.instance.allroom;
        for (int i = 0; i < allRoom.Count; i++)
        {
            if (allRoom[i].RoomName == BuildRoomName.BabyRoom)
            {
                allRoom[i].RemoveRole(role);
            }
        }
    }
    public void ChickBabyDic(HallRole role)
    {
        List<RoomMgr> allRoom = MainCastle.instance.allroom;
        for (int i = 0; i < allRoom.Count; i++)
        {
            if (allRoom[i].RoomName == BuildRoomName.BabyRoom)
            {
                allRoom[i].AddRole(role);
            }
        }
    }

    /// <summary>
    /// 上传本地所有房间信息
    /// </summary>
    public void UpLoadAllRoom()
    {
        LocalServer.instance.saveRoomData.Clear();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            ServerBuildData s_Data = new ServerBuildData(AllBuilding[i]);
            LocalServer.instance.saveRoomData.Add(s_Data);
        }
        AllBuilding.Clear();
    }

    /// <summary>
    /// 有新的建筑 给字典添加信息
    /// </summary>
    public void ChickBuildDicAdd(LocalBuildingData data)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i] == null)
            {
                Debug.Log("有空位 添加");
                dic[data.buildingData.RoomName][i] = data;
                HallEventManager.instance.SendEvent<RoomType>(HallEventDefineEnum.ChickBuild, data.buildingData.RoomType);
                if (ChickStorage(data))
                {
                    ThisStorage(data);
                }
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
    public LocalBuildingData ChickBuildDicChange(LocalBuildingData data, BuildingData changeData)
    {
        for (int i = 0; i < dic[data.buildingData.RoomName].Length; i++)
        {
            if (dic[data.buildingData.RoomName][i].id == data.id)
            {
                dic[data.buildingData.RoomName][i].buildingData = changeData;
                return dic[data.buildingData.RoomName][i];
            }
        }
        Debug.LogError("没有找到匹配的 信息错误");
        return null;
    }

    /// <summary>
    /// 删除建筑信息
    /// </summary>
    public void ChickBuildMerge(ServerBuildData data)
    {
        BuildingData TempData = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.id);
        for (int i = 0; i < dic[TempData.RoomName].Length; i++)
        {
            if (dic[TempData.RoomName][i] != null
                && dic[TempData.RoomName][i].id == data.id
                && dic[TempData.RoomName][i].buildingPoint == data.buildingPoint)
            {
                Debug.Log("找到了 删除");
                dic[TempData.RoomName][i] = null;
                return;
            }
        }
        Debug.LogError("没有找到要删除的建筑 :" + TempData.RoomName);
    }

    /// <summary>
    /// 检查是否是生产类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool ChickProduction(LocalBuildingData data)
    {
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.Gold:
                return true;
            case BuildRoomName.Food:
                return true;
            case BuildRoomName.Mana:
                return true;
            case BuildRoomName.Wood:
                return true;
            case BuildRoomName.Iron:
                return true;
            default:
                break;
        }
        return false;
    }

    /// <summary>
    /// 检查是否是储存类
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public bool ChickStorage(LocalBuildingData data)
    {
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.GoldSpace:
                return true;
            case BuildRoomName.FoodSpace:
                return true;

            case BuildRoomName.ManaSpace:
                return true;

            case BuildRoomName.WoodSpace:
                return true;

            case BuildRoomName.IronSpace:
                return true;
            default:
                break;
        }
        return false;
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
                //这里应该计算房间内人物技能加房间默认产量
                index += (int)(dic[name][i].buildingData.Param1 + dic[name][i].AllRoleProduction());
            }
        }
        return index;
    }

    /// <summary>
    /// 获取某类建筑的总空间
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int GetAllStockSpace(BuildRoomName name)
    {
        int index = 0;
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (name)
        {
            case BuildRoomName.Gold:
                if (dic[BuildRoomName.GoldSpace][0] != null)
                {
                    index += (int)dic[BuildRoomName.GoldSpace][0].buildingData.Param2;
                }
                index += player.GoldSpace;
                break;
            case BuildRoomName.Food:
                if (dic[BuildRoomName.FoodSpace][0] != null)
                {
                    index += (int)dic[BuildRoomName.FoodSpace][0].buildingData.Param2;
                }
                index += player.FoodSpace;
                break;
            case BuildRoomName.Wood:
                if (dic[BuildRoomName.WoodSpace][0] != null)
                {
                    index += (int)dic[BuildRoomName.WoodSpace][0].buildingData.Param2;
                }
                index += player.WoodSpace;
                break;
            case BuildRoomName.Mana:
                if (dic[BuildRoomName.ManaSpace][0] == null)
                {
                    index += (int)dic[BuildRoomName.ManaSpace][0].buildingData.Param2;
                }
                index += player.ManaSpace;
                break;
            case BuildRoomName.Iron:
                if (dic[BuildRoomName.IronSpace][0] == null)
                {
                    index += (int)dic[BuildRoomName.IronSpace][0].buildingData.Param2;
                }
                index += player.IronSpace;
                break;
            default:
                break;
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
        //如果总值等于总空间 那么 建筑提示框改变
        if (index >= GetAllStockSpace(name))
        {
            RoomStockFullHelper data = new RoomStockFullHelper(name, true);
            HallEventManager.instance.SendEvent<RoomStockFullHelper>(HallEventDefineEnum.ChickStockFull, data);
        }
        else
        {
            RoomStockFullHelper data = new RoomStockFullHelper(name, false);
            HallEventManager.instance.SendEvent<RoomStockFullHelper>(HallEventDefineEnum.ChickStockFull, data);
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

    /// <summary>
    /// 增加某类资源
    /// </summary>
    public void AddStock(BuildRoomName name, int index)
    {
        float temp = 0;
        PlayerData play = GetPlayerData.Instance.GetData();
        if (dic.ContainsKey(name))
        {
            temp += dic[name][0].buildingData.Param2 - dic[name][0].Stock;
            if (index - (int)temp > 0)
            {
                index -= (int)temp;
                dic[name][0].Stock += temp;
            }
            else
            {
                dic[name][0].Stock += index;
                HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, name);
                return;
            }
        }

        switch (name)
        {
            case BuildRoomName.Gold:
                play.Gold += index;
                play.Gold = Mathf.Clamp(play.Gold, 0, 5000);
                break;
            case BuildRoomName.Food:
                play.Food += index;
                play.Food = Mathf.Clamp(play.Food, 0, 5000);
                break;
            case BuildRoomName.Mana:
                play.Mana += index;
                play.Mana = Mathf.Clamp(play.Mana, 0, 5000);
                break;
            case BuildRoomName.Wood:
                play.Wood += index;
                play.Wood = Mathf.Clamp(play.Wood, 0, 5000);
                break;
            case BuildRoomName.Iron:
                play.Iron += index;
                play.Iron = Mathf.Clamp(play.Iron, 0, 5000);
                break;
            default:
                break;
        }
        HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, name);
    }
    public void AddStock(int Id, int index)
    {
        Debug.Log("此处ID需优化");
        BuildRoomName name = BuildRoomName.Nothing;
        switch (Id)
        {
            case 1011: name = BuildRoomName.GoldSpace; break;
            case 1012: name = BuildRoomName.FoodSpace; break;
            case 1013: name = BuildRoomName.ManaSpace; break;
            case 1014: name = BuildRoomName.WoodSpace; break;
            case 1015: name = BuildRoomName.IronSpace; break;
            default:
                break;
        }
        AddStock(name, index);
    }

    /// <summary>
    /// 新建建筑 添加建筑 注:无ID
    /// </summary>
    /// <param name="data"></param>
    public void AddBuilding(LocalBuildingData data)
    {
        data.id = BuildingIdIndex;
        AllBuilding.Add(data);
        ChickBuildDicAdd(data);

        //若新建的房间内拥有角色 那么将角色加入房间
        RoomMgr mgr = MainCastle.instance.InstanceRoom(data);
        for (int i = 0; i < data.roleData.Length; i++)
        {
            if (data.roleData[i] != null)
            {
                Debug.Log("房间内发现角色");
                HallRole role = HallRoleMgr.instance.GetRole(data.roleData[i]);
                data.roleData[i] = null;
                mgr.AddRole(role);
            }
        }
    }

    /// <summary>
    /// 删除建筑
    /// </summary>
    /// <param name="data"></param>
    public void RemoveBuilding(LocalBuildingData data)
    {
        if (ChickProduction(data))
        {
            ClostProduction(data);
        }
        bool isTrue = AllBuilding.Remove(data);
        if (isTrue == false)
        {
            Debug.LogError("没找到要删除的建筑 :" + data.buildingData.RoomName);
        }
    }
    public void RemoveBuilding(int id)
    {
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            if (AllBuilding[i].id == id)
            {
                if (ChickProduction(AllBuilding[i]))
                {
                    ClostProduction(AllBuilding[i]);
                }
                AllBuilding.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 房间合并
    /// </summary>
    /// <param name="data_1">合并一号</param>
    /// <param name="data_2">合并二号</param>
    /// <param name="data_3">合并结果</param>
    public void MergeRoom(LocalBuildingData data_1, LocalBuildingData data_2, LocalBuildingData data_3)
    {
        RemoveBuilding(data_1);
        RemoveBuilding(data_2);
        data_3.Stock = data_1.Stock + data_2.Stock;
        AddBuilding(data_3);
    }

    /// <summary>
    /// 建造模式数据保存
    /// </summary>
    /// <param name="datas"></param>
    public void SaveEditModInfo(List<LocalBuildingData> datas)
    {

    }

    /// <summary>
    /// 计时器
    /// </summary>
    /// <param name="data"></param>
    /// <param name="time"></param>
    public int Timer(int id, int time, int nextID, int TipID = -1)
    {
        int index = CTimerManager.instance.AddListener(1f, time, ChickTime);
        LevelUPHelper helper = new LevelUPHelper(id, TipID, time, nextID);
        buildNumber.Add(index, helper);
        if (UILevelUpTip.instance == null)
        {
            UIPanelManager.instance.ShowPage<UILevelUpTip>();
        }
        UILevelUpTip.instance.UpdateTime(helper);
        return index;
    }

    /// <summary>
    /// 计时器返回的信息 本地返回信息只是用于显示计时器信息
    /// </summary>
    /// <param name="key"></param>
    public void ChickTime(int key)
    {
        buildNumber[key].needTime--;
        if (UILevelUpTip.instance == null)
        {
            UIPanelManager.instance.ShowPage<UILevelUpTip>();
        }
        UILevelUpTip.instance.UpdateTime(buildNumber[key]);
        if (buildNumber[key].needTime <= 0)
        {
            UILevelUpTip.instance.RemoveLister(buildNumber[key].tipID);
            ChickLeveUp(buildNumber[key]);
            RemoveThisTime(key);
        }
    }
    /// <summary>
    /// 删除这个计时事件
    /// </summary>
    /// <param name="sequenceTime"></param>
    public void RemoveThisTime(int sequenceTime)
    {
        CTimerManager.instance.RemoveLister(sequenceTime);
    }

    public void ChangeBuildNumber(LevelUPHelper data, int newTip)
    {
        foreach (var item in buildNumber)
        {
            if (item.Value.roomID == data.roomID)
            {
                item.Value.tipID = newTip;
                return;
            }
        }
        Debug.LogError("没有找到要更改的数据");
    }

    public void ChickNowComplete(int id)
    {
        foreach (var item in buildNumber)
        {
            if (item.Value.roomID == id)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                UILevelUpTip.instance.RemoveLister(item.Value.tipID);
                ChickLeveUp(item.Value);
                buildNumber.Remove(item.Key);
            }
        }
    }

    public void ChickLeveUp(LevelUPHelper data)
    {
        LocalBuildingData LocalData = GetBuilding(data.roomID);

        if (LocalData.currentRoom != null)
        {
            LocalData.currentRoom.ConstructionComplete(data);
        }
        else
        {
            BuildingData data_1 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(LocalData.buildingData.NextLevelID);
            ChickPlayerInfo.instance.ChickBuildDicChange(LocalData, data_1);
        }
    }

    public LocalBuildingData GetBuilding(int RoomID)
    {
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            if (AllBuilding[i].id == RoomID)
            {
                return AllBuilding[i];
            }
        }
        Debug.LogError("没有找到建筑");
        return null;
    }



    /// <summary>
    /// 新建生产房间 添加生产事件
    /// </summary>
    /// <param name="data"></param>
    public void ThisProduction(LocalBuildingData data)
    {
        int index = CTimerManager.instance.AddListener(1f, -1, ChickProduction);
        production.Add(index, data);
    }

    /// <summary>
    /// 生产事件 CallBack
    /// </summary>
    /// <param name="key"></param>
    public void ChickProduction(int key)
    {
        float roleNumber = production[key].AllRoleProduction();
        float Yeild = roleNumber + production[key].buildingData.Param1;
        float number = Yeild / 60 / 60;
        production[key].Stock += number * 30f;
        if (production[key].Stock > production[key].buildingData.Param2 * 0.005f)
        {
            MainCastle.instance.FindRoom(production[key].id);
        }
        if (EventKey == key)
        {
            HallEventManager.instance.SendEvent(HallEventDefineEnum.ChickStock);
        }
    }

    /// <summary>
    /// 删除这个事件
    /// </summary>
    /// <param name="data"></param>
    public void ClostProduction(LocalBuildingData data)
    {
        foreach (var item in production)
        {
            if (item.Value == data)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                production.Remove(item.Key);
                return;
            }
        }
        Debug.LogError("删除产出事件时没有找到对象");
    }

    /// <summary>
    /// 获取该生产类房间资源
    /// </summary>
    /// <param name="data"></param>
    public void GetProductionStock(LocalBuildingData data)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        LocalBuildingData space = null;
        int playerIndex = 0;
        int playerSpace = 0;
        float allStock = 0;
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.Gold:
                space = dic[BuildRoomName.GoldSpace][0];
                playerIndex = player.Gold;
                playerSpace = player.GoldSpace;
                allStock = data.Stock;
                player.Gold += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Gold);
                break;
            case BuildRoomName.Food:
                space = dic[BuildRoomName.FoodSpace][0];
                playerIndex = player.Food;
                playerSpace = player.FoodSpace;
                allStock = data.Stock;
                player.Food += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Food);
                break;
            case BuildRoomName.Mana:
                space = dic[BuildRoomName.ManaSpace][0];
                playerIndex = player.Mana;
                playerSpace = player.ManaSpace;
                allStock = data.Stock;
                player.Mana += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Mana);
                break;
            case BuildRoomName.Wood:
                space = dic[BuildRoomName.WoodSpace][0];
                playerIndex = player.Wood;
                playerSpace = player.WoodSpace;
                allStock = data.Stock;
                player.Wood += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Wood);
                break;
            case BuildRoomName.Iron:
                space = dic[BuildRoomName.IronSpace][0];
                playerIndex = player.Iron;
                playerSpace = player.IronSpace;
                allStock = data.Stock;
                player.Iron += GetProductionStockHpr(data, space, playerIndex, playerSpace);
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Iron);
                break;
            default:
                break;
        }
    }
    private int GetProductionStockHpr(LocalBuildingData data, LocalBuildingData space, int playerIndex, int playerSpace)
    {
        float allStock = data.Stock;
        if (space != null)
        {
            float index = (space.buildingData.Param2 - space.Stock);//剩余空间
            if (index < 0)
            {
                Debug.LogError("库存出错");
                return 0;
            }
            if (index - allStock > 0)//如果仓库容量足够
            {
                space.Stock += (int)data.Stock;
                data.Stock = 0;
                return 0;
            }
            else
            {
                data.Stock = ((int)allStock - index);
                allStock = data.Stock;
            }
        }
        //如果没有仓库或者仓库容量不足
        int temp = playerSpace - playerIndex;
        if (temp < 0)
        {
            Debug.LogError("库存出错");
            return 0;
        }
        else if (temp - allStock > 0)
        {
            //playerIndex += (int)data.Stock;
            int number = (int)data.Stock;
            data.Stock = 0;
            return number;
        }
        else
        {
            data.Stock = ((int)allStock - temp);
            //playerIndex += temp;
            return temp;
        }
    }

    /// <summary>
    /// 新建仓库 检查库存
    /// </summary>
    /// <param name="data"></param>
    public void ThisStorage(LocalBuildingData data)
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.GoldSpace:
                player.Gold = ChickStock(data, player.Gold);
                break;
            case BuildRoomName.FoodSpace:
                player.Food = ChickStock(data, player.Food);
                break;
            case BuildRoomName.ManaSpace:
                player.Mana = ChickStock(data, player.Mana);
                break;
            case BuildRoomName.WoodSpace:
                player.Wood = ChickStock(data, player.Wood);
                break;
            case BuildRoomName.IronSpace:
                player.Iron = ChickStock(data, player.Iron);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 检查房间库存
    /// </summary>
    /// <param name="data">仓库</param>
    /// <param name="number">转入数值</param>
    /// <returns></returns>
    public int ChickStock(LocalBuildingData data, int number)
    {
        data.Stock += number;
        float index = data.Stock - data.buildingData.Param2;
        if (index > 0)
        {
            data.Stock = data.buildingData.Param2;
            return (int)index;
        }
        return 0;
    }

    /// <summary>
    /// 返回该类资源剩余仓库空间
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int ChickAllStock(BuildRoomName name)
    {
        float index = 0;
        PlayerData play = GetPlayerData.Instance.GetData();
        if (dic.ContainsKey(name))
        {
            LocalBuildingData[] data = dic[name];
            float p2 = dic[name][0].buildingData.Param2;
            float stock = dic[name][0].Stock;
            index += dic[name][0].buildingData.Param2 - dic[name][0].Stock;
        }
        switch (name)
        {
            case BuildRoomName.Gold:
                index += (play.GoldSpace - play.Gold);
                break;
            case BuildRoomName.Food:
                index += (play.FoodSpace - play.Food);
                break;
            case BuildRoomName.Mana:
                index += (play.ManaSpace - play.Mana);
                break;
            case BuildRoomName.Wood:
                index += (play.WoodSpace - play.Wood);
                break;
            case BuildRoomName.Iron:
                index += (play.IronSpace - play.Iron);
                break;
            default:
                break;
        }
        return (int)index;
    }

    /// <summary>
    /// 获取某建筑的库存值
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public int ThisRoomStock(RoomMgr room)
    {
        AllBuilding.IndexOf(room.currentBuildData);
        return 0;
    }

    /// <summary>
    /// 用于确认是否有该类型仓库
    /// </summary>
    public bool ThisRoomStock()
    {
        return true;
    }

    /// <summary>
    /// 获取房间事件
    /// </summary>
    /// <param name="data"></param>
    public void GetRoomEvent(LocalBuildingData data)
    {
        foreach (var item in production)
        {
            if (item.Value == data)
            {
                EventKey = item.Key;
                return;
            }
        }
        Debug.LogError("没有找到需要监听的对象");
    }

    /// <summary>
    /// 删除房间事件
    /// </summary>
    public void RemoveRoomEvent()
    {
        EventKey = 0;
    }

    /// <summary>
    /// 给予所有已建造房间
    /// </summary>
    /// <returns></returns>
    public List<LocalBuildingData> GetAllBuilding()
    {
        return AllBuilding;
    }

    /// <summary>
    /// 用于国王大厅升级
    /// </summary>
    /// <param name="data">升级后的信息</param>
    public Dictionary<ThroneInfoType, List<BuildingData>> ThroneLeveUpRoomInfo(BuildingData data)
    {
        List<BuildingData> allBuiliding = BuildingDataMgr.instance.AllRoomData();
        if (data.NextLevelID == 0)
        {
            return null;
        }
        int id = data.NextLevelID;
        BuildingData newData = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(id);
        Dictionary<ThroneInfoType, List<BuildingData>> temp = new Dictionary<ThroneInfoType, List<BuildingData>>();
        temp.Add(ThroneInfoType.Build, new List<BuildingData>());
        temp.Add(ThroneInfoType.Upgraded, new List<BuildingData>());

        for (int i = 0; i < allBuiliding.Count; i++)
        {
            if (allBuiliding[i].RoomName == BuildRoomName.ThroneRoom)
            {
                continue;
            }
            //如果这个房间需要登记等于当前等级，且无法拆分 避免可拆分的房间重复出现
            if (allBuiliding[i].NeedLevel == newData.Level
                && allBuiliding[i].SplitID == 0)
            {
                temp[ThroneInfoType.Upgraded].Add(allBuiliding[i]);
            }
            if (allBuiliding[i].UnlockLevel == null)
            {
                continue;
            }
            //可建造
            for (int j = 0; j < allBuiliding[i].UnlockLevel.Length; j++)
            {
                if (allBuiliding[i].UnlockLevel[j] == newData.Level)
                {
                    temp[ThroneInfoType.Build].Add(allBuiliding[i]);
                }
            }
        }
        return temp;
    }

    /// <summary>
    /// 返回房间对应人数
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int ChickRoomSize(BuildingData data)
    {
        int roomSize = data.RoomSize;
        switch (roomSize)
        {
            case 1:
                break;
            case 3:
                return 2;
            case 6:
                return 4;
            case 9:
                return 6;
            default:
                break;
        }
        return 0;
    }

    /// <summary>
    /// 检查当前等级可显示的属性数目
    /// </summary>
    public int ChickAtrNumber()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        int index = 0;
        if (playerData.MainHallLevel > 0)
        {
            index = 2;
        }
        else if (playerData.MainHallLevel >= 4)
        {
            index = 3;
        }
        else if (playerData.MainHallLevel >= 6)
        {
            index = 4;
        }
        else if (playerData.MainHallLevel >= 9)
        {
            index = 5;
        }
        return index;
    }

    /// <summary>
    /// 获取技能等级
    /// </summary>
    public void SetMagicLevel(Dictionary<MagicName, int> MagicData)
    {
        this.MagicLevel = MagicData;
    }
    /// <summary>
    /// 获取技能等级
    /// </summary>
    /// <returns></returns>
    public int GetMagicLevel(MagicName name)
    {
        return MagicLevel[name];
    }
    /// <summary>
    /// 修改技能等级
    /// </summary>
    public void ChangeMagicLevel(MagicName name, int ChangeLevel) { }

    /// <summary>
    /// 获取房间内角色对应ICon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public string ChickRoomIcon(BuildRoomName name)
    {
        string st = "";
        switch (name)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                st = "Gold";
                break;
            case BuildRoomName.Food:
                st = "Food";
                break;
            case BuildRoomName.Mana:
                st = "Mana";
                break;
            case BuildRoomName.Wood:
                st = "Wood";
                break;
            case BuildRoomName.Iron:
                st = "Iron";
                break;
            case BuildRoomName.FighterRoom:
                st = "Fight";
                break;
            case BuildRoomName.Mint:
                st = "Gold";
                break;
            case BuildRoomName.Kitchen:
                st = "Food";
                break;
            case BuildRoomName.Laboratory:
                st = "Mana";
                break;
            case BuildRoomName.Crafting:
                st = "Wood";
                break;
            case BuildRoomName.Foundry:
                st = "Iron";
                break;
            case BuildRoomName.Barracks:
                st = "Fight";
                break;
            default:
                break;
        }
        return st;
    }
}

public class EditSaveHelper
{
    public List<LocalBuildingData> ChangeData;
    public int number;
    public int Size { get { return ChangeData[0].buildingData.RoomSize; } }
    public int Level { get { return ChangeData[0].buildingData.Level; } }
    public BuildRoomName Name { get { return ChangeData[0].buildingData.RoomName; } }

    public EditSaveHelper(LocalBuildingData data)
    {
        ChangeData = new List<LocalBuildingData>();
        int number = data.buildingData.RoomSize / 3;
        ChangeData.Add(data);
    }
}

public class RoomStockFullHelper
{
    public BuildRoomName name;
    public bool isFull;
    public RoomStockFullHelper(BuildRoomName name, bool isFull)
    {
        this.name = name;
        this.isFull = isFull;
    }
}

public class LevelUPHelper
{
    public int roomID;
    public int tipID;
    public int allTime;
    public int needTime;
    public int nextID;

    public LevelUPHelper(int roomID, int tipID, int needTime, int nextID)
    {
        this.roomID = roomID;
        this.tipID = tipID;
        this.allTime = needTime;
        this.needTime = needTime;
        this.nextID = nextID;
    }
}
