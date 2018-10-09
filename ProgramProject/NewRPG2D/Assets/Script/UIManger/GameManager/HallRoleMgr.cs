using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Timer;

public class HallRoleMgr : TSingleton<HallRoleMgr>
{
    Dictionary<HallRoleData, HallRole> dic = new Dictionary<HallRoleData, HallRole>();
    Dictionary<int, RoleTrainHelper> timeAction = new Dictionary<int, RoleTrainHelper>();

    public void AddRole(HallRoleData data, HallRole role)
    {
        dic.Add(data, role);
    }

    public void GetServerRole()
    {

    }

    public void BuildServerRole(HallRoleData data, RoomMgr room)
    {
        GameObject go = Resources.Load("UIPrefab/Role/Role") as GameObject;
        go = GameObject.Instantiate(go, MainCastle.instance.NewRolePoint);
        int count = MainCastle.instance.NewRolePoint.childCount;
        go.transform.localPosition = Vector3.right * (count + 2 * 2);
        HallRole role = go.GetComponent<HallRole>();
        role.UpdateInfo(data);
    }

    public void BuildNewRole()
    {
        int star = Random.Range(1, 4);
        int[] level = new int[6];
        for (int i = 0; i < level.Length; i++)
        {
            level[i] = Random.Range(1, 4);
        }
        HallRoleData data = new HallRoleData(star, level);
        GameObject go = Resources.Load("UIPrefab/Role/Role") as GameObject;
        go = GameObject.Instantiate(go, MainCastle.instance.NewRolePoint);
        int count = MainCastle.instance.NewRolePoint.childCount - 1;
        go.transform.localPosition = Vector3.left * (count * 2);
        HallRole role = go.GetComponent<HallRole>();
        role.UpdateInfo(data);
    }

    public void BuildNewBaby()
    {

    }

    /// <summary>
    /// 驱逐角色
    /// </summary>
    public void RemoveRole()
    {

    }

    /// <summary>
    /// 获取角色实例
    /// </summary>
    public HallRole GetRole(HallRoleData data)
    {
        return dic[data];
    }

    /// <summary>
    /// 角色切换房间
    /// </summary>
    /// <param name="room"></param>
    public void RoleChangeRoom(RoomMgr room, HallRoleData data)
    {
        data.currentRoom = null;
        room.AddRole(dic[data]);
    }
    /// <summary>
    /// 角色排序
    /// </summary>
    /// <param name="Atr"></param>
    /// <returns></returns>
    public List<HallRoleData> ScreenRole(RoleAttribute Atr)
    {
        List<HallRoleData> data = new List<HallRoleData>();
        foreach (var item in dic)
        {
            data.Add(item.Key);
        }
        data.Sort((x, y) => (y.Attribute(Atr).CompareTo(x.Attribute(Atr))));
        return data;
    }
    /// <summary>
    /// 角色排序
    /// </summary>
    /// <returns></returns>
    public List<HallRoleData> ScreenRole()
    {
        List<HallRoleData> data = new List<HallRoleData>();
        foreach (var item in dic)
        {
            data.Add(item.Key);
        }
        data.Sort((x, y) => (x.Star.CompareTo(y.Star)));
        return data;
    }

    /// <summary>
    /// 开始训练
    /// </summary>
    public void StartTrain(HallRoleData data, TrainType atr)
    {
        int needTime = TrainDataMgr.instance.GetTrainData(atr, data.RoleLevel[(int)atr + 1].Level);
        int index = CTimerManager.instance.AddListener(1f, needTime, ChickTrainTime);
        RoleTrainHelper trainHelper = new RoleTrainHelper(data, atr, needTime);
        timeAction.Add(index, trainHelper);
    }
    private void ChickTrainTime(int index)
    {
        timeAction[index].time--;
        if (timeAction[index].time <= 0)
        {
            CompleteTrain(index);
        }
    }

    /// <summary>
    /// 角色训练中止
    /// </summary>
    public void StopTrain(HallRoleData data)
    {
        foreach (var item in timeAction)
        {
            if (item.Value.role == data)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                timeAction.Remove(item.Key);
            }
        }
        Debug.LogError("没有找到要删除的角色");
    }

    /// <summary>
    /// 角色训练完成
    /// </summary>
    public void CompleteTrain(int index)
    {
        //删除这个角色 并且在角色头顶显示图标
        HallRole role = GetRole(timeAction[index].role);
        role.TrainComplete(timeAction[index].atr);
        timeAction.Remove(index);
    }
}

public class RoleTrainHelper
{
    public HallRoleData role;
    public TrainType atr;
    public float time;

    public RoleTrainHelper(HallRoleData data, TrainType atr, float time)
    {
        this.role = data;
        this.atr = atr;
        this.time = time;
    }
}
