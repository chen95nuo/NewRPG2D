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
    private Dictionary<MagicName, MagicData> MagicLevel = new Dictionary<MagicName, MagicData>();//所有技能等级
    private List<int> workMagic = new List<int>();//技能逻辑ID,时间序列ID
    private MagicData currentLevelUpMagic;//当前正在升级的技能
    private int timeIndex = 0;
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
        if (allMagicData.workQueue.Count <= 0)
        {
            timeIndex = CTimerManager.instance.AddListener(1, -1, NewMagicCallBack);
        }
        allMagicData.workQueue.Add(index, data);
        workMagic.Add(index);
    }
    public void NewMagicCallBack(int index)
    {
        int magicID = workMagic[0];
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
            if (item == magicID)
            {
                CTimerManager.instance.RemoveLister(item);
                workMagic.Remove(item);
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
            if (item == magicID)
            {
                CTimerManager.instance.RemoveLister(item);
                allMagicData.readyMagic.Add(allMagicData.workQueue[magicID]);
                allMagicData.workQueue.Remove(magicID);
                return;
            }
        }
    }

    public void LoadMagic(int magicID)
    {
        for (int i = 0; i < allMagicData.readyMagic.Count; i++)
        {
            if (allMagicData.readyMagic[i].magicID == magicID)
            {
                RealMagic data = allMagicData.readyMagic[i];
                for (int j = 0; j < allMagicData.useMagic.Length; j++)
                {
                    if (allMagicData.useMagic[j] == null)
                    {
                        allMagicData.useMagic[j] = allMagicData.readyMagic[i];
                        break;
                    }
                }
                allMagicData.readyMagic.RemoveAt(i);
            }
        }
    }
    public void UnloadMagic(int magicID)
    {
        //需要上传
        for (int i = 0; i < allMagicData.useMagic.Length; i++)
        {
            if (allMagicData.useMagic[i].magicID == magicID)
            {
                allMagicData.readyMagic.Add(allMagicData.useMagic[i]);
                allMagicData.useMagic[i] = null;
                break;
            }
        }
    }

    /// <summary>
    /// 获取技能等级
    /// </summary>
    public void SetMagicLevel(Dictionary<MagicName, MagicData> MagicData)
    {
        this.MagicLevel = MagicData;
    }
    /// <summary>
    /// 获取技能等级
    /// </summary>
    /// <returns></returns>
    public MagicData GetMagicLevel(MagicName name)
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

public class MagicWorkHelper
{

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
    Cry,//需要制造的
    CanLevelUp,//可以升级的
    NeedLevelUp,//需要升级解锁的
    NeedLevel,//需要房间等级的
}
