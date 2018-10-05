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
}
