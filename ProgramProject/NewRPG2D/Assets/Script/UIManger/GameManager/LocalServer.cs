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

    /// <summary>
    /// 初始刷新房间
    /// </summary>
    public void TestRoom()
    {
        List<ServerBuildData> data = new List<ServerBuildData>();
        ServerBuildData s_1 = new ServerBuildData(10035, new Vector2(6, 0), 0);
        data.Add(s_1);
        ServerBuildData s_2 = new ServerBuildData(10035, new Vector2(6, 1), 0);
        data.Add(s_2);
        ServerBuildData s_3 = new ServerBuildData(10035, new Vector2(6, 2), 0);
        data.Add(s_3);
        ServerBuildData s_4 = new ServerBuildData(10010, new Vector2(7, 1), 0);
        data.Add(s_4);
        ServerBuildData s_5 = new ServerBuildData(10013, new Vector2(7, 0), 0);
        data.Add(s_5);
        ServerBuildData s_6 = new ServerBuildData(10031, new Vector2(7, 2), 0);
        data.Add(s_6);
        ServerBuildData s_7 = new ServerBuildData(10016, new Vector2(3, 0), 3000);
        data.Add(s_7);
        ChickPlayerInfo.instance.ChickBuildDic(data);

        int[] level = new int[6];
        for (int i = 0; i < level.Length; i++)
        {
            level[i] = 3;
        }
        HallRoleData r_1 = new HallRoleData(3, level);
        HallRoleData r_2 = new HallRoleData(3, level);
        MagicLevel();
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
