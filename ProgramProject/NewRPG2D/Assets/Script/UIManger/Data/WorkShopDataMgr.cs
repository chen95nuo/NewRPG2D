using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using Assets.Script.Timer;
using System;

public class WorkShopDataMgr : ItemDataBaseMgr<WorkShopDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.WorkShopData; }
    }

    //RoomId , Quality
    private Dictionary<int, WorkShopData[]> dic = new Dictionary<int, WorkShopData[]>();

    //正在工作中的房间 计时器编号
    private Dictionary<int, WorkShopHelper> WorkDic = new Dictionary<int, WorkShopHelper>();

    private void GetAllDic()
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            WorkShopData data = CurrentItemData[i] as WorkShopData;
            if (dic.ContainsKey(data.RoomID))
            {
                dic[data.RoomID][(int)data.Quality - 2] = data;
            }
            else
            {
                dic.Add(data.RoomID, new WorkShopData[4]);
                dic[data.RoomID][(int)data.Quality - 2] = data;
            }
        }
    }
    public WorkShopData[] GetWorkData(int roomID)
    {
        if (dic.Count <= 0)
        {
            GetAllDic();
        }

        if (dic.ContainsKey(roomID))
        {
            return dic[roomID];
        }
        Debug.LogError("ID错误");
        return null;
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
        int index = CTimerManager.instance.AddListener(1, data.time, WorkCallBack);
        WorkDic.Add(index, data);
        WorkCallBack(index);
    }
    public void WorkSpeedUp(int roomId)
    {
        foreach (var item in WorkDic)
        {
            if (item.Value.roomId == roomId)
            {

                return;
            }
        }
    }
    public void WorkCallBack(int index)
    {
        if (WorkDic.ContainsKey(index))
        {
            WorkDic[index].time--;
            HallEventManager.instance.SendEvent<WorkShopHelper>(HallEventDefineEnum.ChickWorkTime, WorkDic[index]);
        }
        else
        {
            Debug.LogError("字典中没有找到这个key");
            CTimerManager.instance.RemoveLister(index);
        }
    }
    public void WorkComplate()
    {

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