using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class LocalServer : TSingleton<LocalServer>
{
    private Dictionary<int, RoomMgr> buildNumber = new Dictionary<int, RoomMgr>();//房间序号 用于储存施工中的房间

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

    private List<ServerBuildData> saveRoomData;
    private List<HallRoleData> saveRoleData;

    /// <summary>
    /// 房间施工用 计时器
    /// </summary>
    /// <param name="data"></param>
    /// <param name="time"></param>
    public void Timer(RoomMgr data, int time)
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
        buildNumber[key].ConstructionComplete();
    }


    public void ChickTime(RoomMgr roomMgr)
    {
        foreach (var item in buildNumber)
        {
            if (item.Value == roomMgr)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                roomMgr.ConstructionComplete();
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
            for (int i = 0; i < 2; i++)
            {
                HallRoleMgr.instance.BuildNewRole(0);
            }
            HallRole data_1 = HallRoleMgr.instance.BuildNewRole(1);
            HallRole data_2 = HallRoleMgr.instance.BuildNewRole(2);
            List<ServerHallRoleData> s_Data = new List<ServerHallRoleData>();
            s_Data.Add(new ServerHallRoleData(9, data_1));
            s_Data.Add(new ServerHallRoleData(9, data_2));
            ChickPlayerInfo.instance.ChickRoleDic(s_Data);
        }
        MagicLevel();
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
        ServerBuildData s_8 = new ServerBuildData(8, 10042, new Vector2(10, 2), 0, 0, 0);
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
