using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using Assets.Script.Timer;

public class MagicDataMgr : ItemDataBaseMgr<MagicDataMgr>
{
    private MagicWorkShopHelper allMagicData = new MagicWorkShopHelper();
    private Dictionary<MagicName, int> MagicLevel = new Dictionary<MagicName, int>();//所有技能等级
    private Dictionary<int, int> workMagic = new Dictionary<int, int>();//技能逻辑ID,时间序列ID
    private MagicData currentLevelUpMagic;//当前正在升级的技能
    private int instanceMagicID = 0;
    public int InstanceMagicID
    {
        get
        {
            return instanceMagicID++;
        }
    }

    protected override XmlName CurrentXmlName
    {
        get { return XmlName.MagicData; }
    }

    public MagicWorkShopHelper AllMagicData
    {
        get
        {
            return allMagicData;
        }
    }

    public List<MagicData> GetMagic(int level)
    {
        List<MagicData> datas = new List<MagicData>();
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            MagicData temp = CurrentItemData[i] as MagicData;
            if (temp.level == level)
            {
                datas.Add(temp);
            }
        }
        return datas;
    }

    public MagicData GetMagic(MagicName name, int level)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            MagicData temp = CurrentItemData[i] as MagicData;
            if (temp.magicName == name && temp.level == level)
            {
                return temp;
            }
        }
        return null;
    }

    public void MagicLevelUp(int id, int time = 0)
    {

    }

    public void CryNewMagic(int id, int time = 0)
    {
        int index = InstanceMagicID;
        MagicData magicData = GetXmlDataByItemId<MagicData>(id);
        RealMagic data = new RealMagic(index, magicData, time);
        time = time == 0 ? data.magic.produceTime : time;
        allMagicData.workQueue.Add(index, data);
        int timeIndex = CTimerManager.instance.AddListener(time, 1, NewMagicCallBack);
        workMagic.Add(timeIndex, index);
    }
    public void NewMagicCallBack(int index)
    {
        int magicID = workMagic[index];
        allMagicData.workQueue[magicID].time--;
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.CryNewMagic, index);
        if (allMagicData.workQueue[magicID].time <= 0)
        {
            allMagicData.workQueue[magicID].time = 0;
            MagicWorkComplate(index);
        }
    }
    public void CancelNewMagic(int magicID)
    {
        int needProduce = allMagicData.workQueue[magicID].magic.produceNeed;
        allMagicData.workQueue.Remove(magicID);
        ChickPlayerInfo.instance.AddStock(BuildRoomName.ManaSpace, needProduce);
        foreach (var item in workMagic)
        {
            if (item.Value == magicID)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                workMagic.Remove(item.Key);
                break;
            }
        }
    }
    public void SpeedUpNewMagic(int magicID)
    {
        RealMagic magic = allMagicData.workQueue[magicID];
        allMagicData.workQueue.Remove(magicID);
        allMagicData.readyMagic.Add(magic);
    }
    public void MagicWorkComplate(int magicID)
    {
        foreach (var item in workMagic)
        {
            if (item.Value == magicID)
            {
                CTimerManager.instance.RemoveLister(item.Key);
                allMagicData.readyMagic.Add(allMagicData.workQueue[magicID]);
                allMagicData.workQueue.Remove(magicID);
                return;
            }
        }
    }


    public void UnloadMagic(int magicID)
    {
        //需要上传
        if (allMagicData.readyMagic.Count >= 18)
        {
            object st = "法术已满";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            return;
        }
        for (int i = 0; i < allMagicData.useMagic.Length; i++)
        {
            if (allMagicData.useMagic[i].magicID == magicID)
            {
                allMagicData.useMagic[i] = null;
                allMagicData.readyMagic.Add(allMagicData.useMagic[i]);
            }
        }
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
}

public class RealMagic
{
    public int magicID;
    public MagicData magic;
    public int time;
    public RealMagic(int magicID, MagicData magic, int time = 0)
    {
        this.magicID = magicID;
        this.magic = magic;
        this.time = time;
    }
}
public class MagicWorkShopHelper
{
    public RealMagic[] useMagic;
    public List<RealMagic> readyMagic;
    public Dictionary<int, RealMagic> workQueue;

    public MagicWorkShopHelper()
    {
        useMagic = new RealMagic[6];
        readyMagic = new List<RealMagic>();
        workQueue = new Dictionary<int, RealMagic>();
    }
    public MagicWorkShopHelper(RealMagic[] useMagic, List<RealMagic> readyMagic, Dictionary<int, RealMagic> workQueue)
    {
        this.useMagic = useMagic;
        this.readyMagic = readyMagic;
        this.workQueue = workQueue;
    }
}

public enum MagicGridType
{
    Empty,//空的
    Use,//使用中的
    Read,//制造出来的
    Work,//制造中的
    Lock,//锁住的
    CanLevelUp,//可以升级的
    NeedLevelUp,//需要升级解锁的
    NeedLevel,//需要房间等级的
}
