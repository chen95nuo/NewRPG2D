using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class LocalServer : TSingleton<LocalServer>
{
    private Dictionary<int, LocalBuildingData> buildNumber = new Dictionary<int, LocalBuildingData>();//房间序号 用于储存施工中的房间

    private Dictionary<BuildRoomName, LocalBuildingData> production;

    public Dictionary<BuildRoomName, LocalBuildingData> Production
    {
        get
        {
            if (production == null)
            {
                production = new Dictionary<BuildRoomName, LocalBuildingData>();
            }
            return production;
        }
    }

    public List<ServerBuildData> saveRoomData;
    public List<ServerHallRoleData> saveRoleData;
    public List<RoleBabyData> saveBabydata = new List<RoleBabyData>();

    /// <summary>
    /// 房间施工用 计时器
    /// </summary>
    /// <param name="data"></param>
    /// <param name="time"></param>
    public void Timer(LocalBuildingData data, int time)
    {
        int index = CTimerManager.instance.AddListener(time, 1, ChickTime);
        buildNumber.Add(index, data);
    }

    /// <summary>
    /// 服务器这边返回信息 直接完成
    /// </summary>
    /// <param name="key"></param>
    public void ChickTime(int key)
    {
        ChickLeveUp(buildNumber[key]);
    }

    public void ChickLeveUp(LocalBuildingData LocalData)
    {
        if (LocalData.currentRoom != null)
        {
            LocalData.currentRoom.ConstructionComplete();
        }
        else
        {
            BuildingData data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(LocalData.buildingData.NextLevelID);
            ChickPlayerInfo.instance.ChickBuildDicChange(LocalData, data);
        }
    }

    public void ChickTime(LocalBuildingData roomData)
    {
        foreach (var item in buildNumber)
        {
            if (item.Value == roomData)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                ChickLeveUp(roomData);
                return;
            }
        }
        Debug.Log("钻石加速没有找到计时器");
    }

    public void StartInit()
    {
        if (saveRoomData == null)
        {
            TestRoom();
        }
        ChickPlayerInfo.instance.ChickBuildDic(saveRoomData);
        if (saveRoleData == null)
        {
            HallRoleData data_1 = HallRoleMgr.instance.BuildNewRole(1);
            HallRoleData data_2 = HallRoleMgr.instance.BuildNewRole(2);
            saveRoleData = new List<ServerHallRoleData>();
            saveRoleData.Add(new ServerHallRoleData(9, data_1));
            saveRoleData.Add(new ServerHallRoleData(9, data_2));
        }
        HallRoleMgr.instance.ChickRoleDic(saveRoleData);
        HallRoleMgr.instance.ChickBabyDic(saveBabydata);
        MagicLevel();
    }

    public void RoleChangeRoom(HallRoleData role, int roomID)
    {
        for (int i = 0; i < saveRoleData.Count; i++)
        {
            if (saveRoleData[i].role == role)
            {
                saveRoleData[i].RoomId = roomID;
            }
        }
    }

    /// <summary>
    /// 初始刷新房间
    /// </summary>
    private void TestRoom()
    {
        saveRoomData = new List<ServerBuildData>();
        ServerBuildData s_1 = new ServerBuildData(1, 10035, new Vector2(6, 0), 0, 0, 0);
        saveRoomData.Add(s_1);
        ServerBuildData s_2 = new ServerBuildData(2, 10035, new Vector2(6, 1), 0, 0, 0);
        saveRoomData.Add(s_2);
        ServerBuildData s_3 = new ServerBuildData(3, 10035, new Vector2(6, 2), 0, 0, 0);
        saveRoomData.Add(s_3);
        ServerBuildData s_4 = new ServerBuildData(4, 10011, new Vector2(7, 1), 0, 0, 0);
        saveRoomData.Add(s_4);
        ServerBuildData s_8 = new ServerBuildData(8, 10036, new Vector2(10, 2), 0, 0, 0);
        saveRoomData.Add(s_8);
        ServerBuildData s_9 = new ServerBuildData(9, 10001, new Vector2(13, 2), 0, 0, 0);
        saveRoomData.Add(s_9);
        ServerBuildData s_5 = new ServerBuildData(5, 10013, new Vector2(7, 0), 0, 0, 0);
        saveRoomData.Add(s_5);
        ServerBuildData s_6 = new ServerBuildData(6, 10031, new Vector2(7, 2), 0, 0, 0);
        saveRoomData.Add(s_6);
        ServerBuildData s_7 = new ServerBuildData(7, 10016, new Vector2(3, 0), 3000, 0, 0);
        saveRoomData.Add(s_7);
    }

    /// <summary>
    /// 魔法技能等级
    /// </summary>
    public void MagicLevel()
    {
        Dictionary<MagicName, int> dic = new Dictionary<MagicName, int>();
        for (int i = 0; i < (int)MagicName.Max; i++)
        {
            dic.Add((MagicName)i, 1);
        }
        ChickPlayerInfo.instance.SetMagicLevel(dic);
    }

}
