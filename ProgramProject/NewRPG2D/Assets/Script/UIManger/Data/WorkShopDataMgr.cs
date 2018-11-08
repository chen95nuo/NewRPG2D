using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using Assets.Script.Timer;

public class WorkShopDataMgr : ItemDataBaseMgr<WorkShopDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.WorkShopData; }
    }

    //RoomId , TypeID
    private Dictionary<int, Dictionary<int, WorkShopData[]>> dic = new Dictionary<int, Dictionary<int, WorkShopData[]>>();
    public Dictionary<int, Dictionary<int, WorkShopData[]>> Dic
    {
        get
        {
            if (dic == null)
            {
                GetAllDic();
            }
            return dic;
        }
    }

    //正在工作中的房间 roomId
    private Dictionary<int, WorkShopHelper> WorkDic = new Dictionary<int, WorkShopHelper>();

    private void GetAllDic()
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            WorkShopData data = CurrentItemData[i] as WorkShopData;
            if (dic.ContainsKey(data.RoomID))
            {
                if (dic[data.RoomID].ContainsKey(data.EquipType))
                {
                    dic[data.RoomID][data.EquipType][(int)data.Quality - 1] = data;
                }
                else
                {
                    dic[data.RoomID].Add(data.EquipType, new WorkShopData[4]);
                    dic[data.RoomID][data.EquipType][(int)data.Quality] = data;
                }
            }
            else
            {
                dic.Add(data.RoomID, new Dictionary<int, WorkShopData[]>());
                dic[data.RoomID].Add(data.EquipType, new WorkShopData[4]);
                dic[data.RoomID][data.EquipType][(int)data.Quality - 1] = data;
            }
        }
    }
    public WorkShopData[] GetWorkData(int roomID, int type)
    {
        return Dic[roomID][type];
    }
    public WorkShopHelper GetWorkTime(int roomID)
    {
        foreach (var item in WorkDic)
        {
            if (item.Value.roomId == roomID)
            {
                return item.Value;
            }
        }
        return null;
    }
    public void AddWork(int roomId, WorkShopData shopData, int needTime = 0)
    {
        needTime = needTime == 0 ? shopData.NeedTime : needTime;
        WorkShopHelper data = new WorkShopHelper(roomId, shopData, needTime);
        WorkDic.Add(roomId, data);
        CTimerManager.instance.AddListener(1, data.time, WorkCallBack);
    }
    public void WorkCallBack(int index)
    {
        if (WorkDic.ContainsKey(index))
        {
            WorkDic[index].time--;
            HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.ChickWorkTime, WorkDic[index].roomId);
        }
        else
        {
            Debug.LogError("字典中没有找到这个key");
            CTimerManager.instance.RemoveLister(index);
        }
    }
}

public class WorkShopHelper
{
    public int roomId;
    public int time;
    public int maxTime
    {
        get
        {
            return workData.NeedTime;
        }
    }
    public WorkShopData workData;
    public int SpeedTime;
    public WorkShopHelper(int roomId, WorkShopData workData, int time = 0, int speedTime = 0)
    {
        this.roomId = roomId;
        this.workData = workData;
        this.time = time;
        this.SpeedTime = speedTime;
    }
}