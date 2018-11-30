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
    private Dictionary<BuildRoomName, List<LocalBuildingData>> allBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
    private Dictionary<int, LocalBuildingData> proBuilding = new Dictionary<int, LocalBuildingData>();

    private int produceInterval = 30;

    #region 房间建设
    /// <summary>
    /// 第一次启动通过服务器或的房间
    /// </summary>
    /// <param name="s_AllRoom"></param>
    public void GetAllBuildingData(List<proto.SLGV1.RoomInfo> s_AllRoom)
    {
        allBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
        for (int i = 0; i < s_AllRoom.Count; i++)
        {
            AddNewRoom(s_AllRoom[i]);
        }
    }

    public void AddNewRoom(proto.SLGV1.RoomInfo roomInfo)
    {
        int[] id = TypeOfRoomId(roomInfo.roomId);
        BuildRoomName name = (BuildRoomName)id[0];
        if (!allBuilding.ContainsKey(name))
        {
            allBuilding.Add(name, new List<LocalBuildingData>());
        }
        LocalBuildingData buildingData = new LocalBuildingData(roomInfo, id[1]);
        allBuilding[name].Add(buildingData);
        SetAllBuildingData(buildingData);
    }

    /// <summary>
    /// 切场景回来后重新放置房间
    /// </summary>
    public void ResetBuildingData()
    {
        foreach (var roomData in allBuilding)
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
        //while (MainCastle.instance == null) { }
        MainCastle.instance.InstanceRoom(roomData);
        if (roomData.leftTime > 0) //如果该房间在升级
        {

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
    public void GetAllEquipProRoom(List<proto.SLGV1.ProudctEquipInfo> equipRoom)
    {
        for (int i = 0; i < equipRoom.Count; i++)
        {
            GetEquipProRoom(equipRoom[i]);
        }
    }

    public void GetEquipProRoom(proto.SLGV1.ProudctEquipInfo equipRoom)
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

    #region 查找功能

    /// <summary>
    /// 通过类型和ID查找房间
    /// </summary>
    /// <param name="type">房间类型</param>
    /// <param name="id">房间ID</param>
    /// <returns></returns>
    public LocalBuildingData SearchRoomData(BuildRoomName type, int id)
    {
        List<LocalBuildingData> TypeRoom = allBuilding[type];
        for (int i = 0; i < TypeRoom.Count; i++)
        {
            if (TypeRoom[i].id == id) return TypeRoom[i];
        }
        return null;
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
