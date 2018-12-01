/*
 * 提供所有建筑信息
 * 建筑数据中有建筑 则建造数据中的建筑
 * 若没有建筑 则等待服务器提供建筑信息
*/

using Assets.Script.Timer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : TSingleton<BuildingManager>
{
    private Dictionary<BuildRoomName, List<LocalBuildingData>> AllBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
    private Dictionary<int, LocalBuildingData> proBuilding = new Dictionary<int, LocalBuildingData>();

    private int produceInterval = 30;

    #region 房间建设
    /// <summary>
    /// 第一次启动通过服务器或的房间
    /// </summary>
    /// <param name="s_AllRoom"></param>
    public void GetAllBuildingData(List<proto.SLGV1.RoomInfo> s_AllRoom)
    {
        AllBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
        for (int i = 0; i < s_AllRoom.Count; i++)
        {
            AddNewRoom(s_AllRoom[i]);
        }
    }

    public void AddNewRoom(proto.SLGV1.RoomInfo roomInfo)
    {
        int[] id = TypeOfRoomId(roomInfo.roomId);
        BuildRoomName name = (BuildRoomName)id[0];
        if (!AllBuilding.ContainsKey(name))
        {
            AllBuilding.Add(name, new List<LocalBuildingData>());
        }

        LocalBuildingData buildingData = new LocalBuildingData(roomInfo, id[1]);
        AllBuilding[name].Add(buildingData);
    }

    public void SetMainHallRoom()
    {
        GetPlayerData.Instance.SetThroneRoom(AllBuilding[BuildRoomName.ThroneRoom][0]);
        GetPlayerData.Instance.SetBarracks(AllBuilding[BuildRoomName.Barracks][0]);
    }

    /// <summary>
    /// 切场景回来后重新放置房间
    /// </summary>
    public void ResetBuildingData()
    {
        foreach (var roomData in AllBuilding)
        {
            for (int i = 0; i < roomData.Value.Count; i++)
            {
                SetAllBuildingData(roomData.Value[i]);
            }
        }
    }

    /// <summary>
    /// 将房间数据实例化
    /// </summary>
    /// <param name="roomData"></param>
    public void SetAllBuildingData(LocalBuildingData roomData)
    {
        MainCastle.instance.InstanceRoom(roomData);
        if (roomData.leftTime > 0) //如果该房间在升级
        {
            //发送消息给升级标签
        }
        for (int i = 0; i < roomData.roleData.Length; i++)
        {
            if (roomData.roleData[i] != null)
            {
                HallRole data = roomData.roleData[i].instance;
                data.roleRoomIndex = i;
                data.transform.parent = roomData.currentRoom.RolePoint;
                data.transform.localPosition = Vector3.zero;
                Vector2 endPoint = roomData.buildingData.RolePoint[i];
                data.RoleMove(endPoint);
            }
        }
    }
    #endregion

    #region 房间功能

    #region 生产类
    /// <summary>
    /// 全部生产类房间
    /// </summary>
    /// <param name="proData"></param>
    public void GetAllProduceRoom(List<proto.SLGV1.ProduceRoom> proData)
    {
        proBuilding = new Dictionary<int, LocalBuildingData>();
        for (int i = 0; i < proData.Count; i++)
        {
            GetProduceRoom(proData[i]);
        }
    }
    /// <summary>
    /// 给生产房间添加时间事件
    /// </summary>
    /// <param name="proData"></param>
    public void GetProduceRoom(proto.SLGV1.ProduceRoom proData)
    {
        string[] id = proData.roomId.Split('_');
        LocalBuildingData data = SearchRoomData((BuildRoomName)int.Parse(id[0]), int.Parse(id[1]));
        data.Stock = proData.toBeCollected;
        data.speedProd = proData.speedProd;
        if (proBuilding.ContainsValue(data))
        {
            Debug.Log("重复的生产房间 或者有房间生产参数需要更改,暂时return");
            return;
        }
        int index = CTimerManager.instance.AddListener(produceInterval, 0, ProduceCallBack);
        proBuilding.Add(index, data);
    }
    /// <summary>
    /// 生产事件计时回调
    /// </summary>
    /// <param name="index"></param>
    public void ProduceCallBack(int index)
    {
        LocalBuildingData roomData = proBuilding[index];
        roomData.Stock += (roomData.speedProd * produceInterval);
        //必要的情况下通知UI
    }
    #endregion

    #region 储存类
    /// <summary>
    /// 获取全部储存类房间
    /// </summary>
    /// <param name="stoData"></param>
    public void GetAllStoreRoom(List<proto.SLGV1.StoreRoom> stoData)
    {
        for (int i = 0; i < stoData.Count; i++)
        {
            GetStoreRoom(stoData[i]);
        }
    }

    /// <summary>
    /// 添加储存类房间数据
    /// </summary>
    /// <param name="stoData"></param>
    public void GetStoreRoom(proto.SLGV1.StoreRoom stoData)
    {
        int[] id = TypeOfRoomId(stoData.roomId);
        LocalBuildingData data = SearchRoomData((BuildRoomName)id[0], id[1]);
        data.Stock = stoData.depot;
    }
    #endregion

    #region 居民室

    public void GetAllResidentRoom(List<proto.SLGV1.ResidentRoom> resRoom)
    {
        for (int i = 0; i < resRoom.Count; i++)
        {
            GetResidentRoom(resRoom[i]);
        }
    }

    public void GetResidentRoom(proto.SLGV1.ResidentRoom resRoom)
    {
        //对应房间添加对应人数再加对应时间
    }

    #endregion

    #region 装备生产工厂
    public void GetAllEquipProRoom(List<proto.SLGV1.ProduceEquipInfo> equipRoom)
    {
        for (int i = 0; i < equipRoom.Count; i++)
        {
            GetEquipProRoom(equipRoom[i]);
        }
    }

    public void GetEquipProRoom(proto.SLGV1.ProduceEquipInfo equipRoom)
    {
        //给UI发送房间信息 让UI追踪房间 该房间施工状态为True;
    }
    #endregion

    #region 魔法工厂
    public void GetAllMagicSkillInfo(proto.SLGV1.AllMagicSkillInfo skillRoom)
    {
        //获取所有法术，获取所有法术等级;
    }
    #endregion

    #endregion

    #region UI刷新
    /// <summary>
    /// 刷新所有房间资源
    /// </summary>
    public void ResetUIProduce()
    {
        UpdateProduce(BuildRoomName.GoldSpace);
        UpdateProduce(BuildRoomName.FoodSpace);
        UpdateProduce(BuildRoomName.ManaSpace);
        UpdateProduce(BuildRoomName.WoodSpace);
        UpdateProduce(BuildRoomName.IronSpace);
    }
    /// <summary>
    /// 刷新房间资源
    /// </summary>
    /// <param name="room"></param>
    public void UpdateProduce(BuildRoomName room)
    {
        int produce = GetPlayerData.Instance.GetData().GetResSpace(room);
        HallEventManager.instance.SendEvent<UIMainCheckStock>(HallEventDefineEnum.CheckStock, new UIMainCheckStock(room, produce));
    }
    public int[] GetBuildDicInfo(BuildingData data)
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
        if (!AllBuilding.ContainsKey(data.RoomName))
        {
            index[0] = 0;
            return index;
        }
        for (int i = 0; i < AllBuilding[data.RoomName].Count; i++)
        {
            if (AllBuilding[data.RoomName][i] != null)
            {
                index[0]++;//获取该类建筑已建造数量
                if (AllBuilding[data.RoomName][i].buildingData.RoomType == RoomType.Production)
                {
                    switch (AllBuilding[data.RoomName][i].buildingData.RoomSize)
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
    #endregion

    #region 查找功能
    /// <summary>
    /// 通过类型和ID查找房间
    /// </summary>
    /// <param name="type">房间类型</param>
    /// <param name="id">房间ID</param>
    /// <returns></returns>
    public LocalBuildingData SearchRoomData(BuildRoomName type, int id)
    {
        List<LocalBuildingData> TypeRoom = AllBuilding[type];
        for (int i = 0; i < TypeRoom.Count; i++)
        {
            if (TypeRoom[i].id == id) return TypeRoom[i];
        }
        return null;
    }

    /// <summary>
    /// 通过BuildRoomName查找总容量
    /// </summary>
    /// <returns></returns>
    public int SearchRoomSpace(BuildRoomName name)
    {
        int space = 0;
        if (AllBuilding.ContainsKey(name))
        {
            space += (int)AllBuilding[name][0].buildingData.Param2;
        }
        space += GetPlayerData.Instance.GetData().defaultSpace;
        return space;
    }

    /// <summary>
    /// 通过BuildRoomName查找剩余空间
    /// </summary>
    /// <returns></returns>
    public int SearchRoomEmpty(BuildRoomName name)
    {
        int empty = 0;
        if (AllBuilding.ContainsKey(name))
        {
            LocalBuildingData data = AllBuilding[name][0];
            empty += (int)(data.buildingData.Param2 - data.Stock);
        }
        PlayerData playData = GetPlayerData.Instance.GetData();
        empty += (playData.defaultSpace - playData.GetResSpace(name));
        return empty;
    }

    /// <summary>
    /// 通过BuildRoomName查找总数量
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int SearchRoomStock(BuildRoomName name)
    {
        int stock = 0;
        if (AllBuilding.ContainsKey(name))
        {
            stock += (int)AllBuilding[name][0].Stock;
        }
        stock += GetPlayerData.Instance.GetData().GetResSpace(name);
        return stock;
    }
    #endregion

    #region 工具

    /// <summary>
    /// 转换名字
    /// </summary>
    /// <param name="stId"></param>
    /// <returns></returns>
    public int[] TypeOfRoomId(string stId)
    {
        string[] st = stId.Split('_');
        int[] id = new int[2] { int.Parse(st[0]), int.Parse(st[1]) };
        return id;
    }

    #endregion
}
