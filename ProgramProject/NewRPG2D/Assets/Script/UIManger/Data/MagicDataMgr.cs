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
    private MagicLevelUpHelper magicLevelUpData;
    private int newMagicTimeIndex = -1;
    private int instanceMagicID = 0;
    public int allMagicSpace = 18;
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

    public int Space
    {
        get
        {
            int space = allMagicData.readyMagic.Count + allMagicData.workQueue.Count;
            return space;
        }
    }

    public MagicLevelUpHelper GetLevelUpData
    {
        get
        {
            return magicLevelUpData;
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

    /// <summary>
    /// 升级信息
    /// </summary>
    /// <param name="id"></param>
    /// <param name="time"></param>
    public void MagicLevelUp(int id, int time = 0)
    {
        MagicData data = null;
        if (time == 0)
        {
            for (int i = 0; i < CurrentItemData.Length; i++)
            {
                data = CurrentItemData[i] as MagicData;
                if (data.ItemId == id)
                {
                    time = data.levelUpTime;
                    break;
                }
            }
        }
        magicLevelUpData = new MagicLevelUpHelper(id, time);
        int index = CTimerManager.instance.AddListener(1f, time, LevelUpCallBack);
        LevelUpCallBack(index);
    }

    public void LevelUpCallBack(int index)
    {
        magicLevelUpData.time--;
        HallEventManager.instance.SendEvent<MagicLevelUpHelper>(HallEventDefineEnum.MagicLevelUp, magicLevelUpData);
    }

    public void CryNewMagic(int id, int time = 0)
    {
        int index = InstanceMagicID;
        MagicData magicData = GetXmlDataByItemId<MagicData>(id);
        RealMagic data = new RealMagic(index, magicData, time);
        data.time = time == 0 ? data.magic.produceTime : time;
        allMagicData.workQueue.Add(data);
        NewMagicTimeControl();
    }
    public void NewMagicTimeControl()
    {
        if (allMagicData.workQueue.Count > 0 && newMagicTimeIndex != -1)
        {
            //继续
        }
        else if (allMagicData.workQueue.Count > 0 && newMagicTimeIndex == -1)
        {
            //新建时间
            newMagicTimeIndex = CTimerManager.instance.AddListener(1, -1, NewMagicCallBack);
        }
        else
        {
            //删除时间
            CTimerManager.instance.RemoveLister(newMagicTimeIndex);
            newMagicTimeIndex = -1;
        }
    }
    public void NewMagicCallBack(int index)
    {
        if (index != newMagicTimeIndex)
        {
            Debug.LogError("出现重复计时器");
            CTimerManager.instance.RemoveLister(index);
            return;
        }
        int magicID = allMagicData.workQueue[0].magicID;
        allMagicData.workQueue[0].time--;
        HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.CryNewMagic, magicID);
        if (allMagicData.workQueue[0].time <= 0)
        {
            allMagicData.workQueue[0].time = 0;
            MagicWorkComplate(0);
        }
    }
    public void CancelNewMagic(int magicID)
    {
        RealMagic data = null;
        foreach (var item in allMagicData.workQueue)
        {
            if (item.magicID == magicID)
            {
                data = item;
            }
        }
        int needProduce = data.magic.produceNeed;
        allMagicData.workQueue.Remove(data);
        ChickPlayerInfo.instance.AddStock(BuildRoomName.ManaSpace, needProduce);
        NewMagicTimeControl();
    }
    public void SpeedUpNewMagic(int magicID)
    {
        RealMagic magic = null;
        foreach (var item in allMagicData.workQueue)
        {
            if (item.magicID == magicID)
            {
                magic = item;
            }
        }

        allMagicData.workQueue.Remove(magic);
        allMagicData.readyMagic.Add(magic);
        NewMagicTimeControl();
    }
    public void MagicWorkComplate(int magicID)
    {
        allMagicData.readyMagic.Add(allMagicData.workQueue[0]);
        allMagicData.workQueue.RemoveAt(0);
        NewMagicTimeControl();
        if (UIMagicWorkShop.instance != null)
        {
            UIMagicWorkShop.instance.UpdateReadMagic();
            UIMagicWorkShop.instance.UpdateWorkMagic();
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
            if (allMagicData.useMagic[i] != null && allMagicData.useMagic[i].magicID == magicID)
            {
                allMagicData.readyMagic.Add(allMagicData.useMagic[i]);
                allMagicData.useMagic[i] = null;
                break;
            }
        }
    }

    public void ChangeMagic(RealMagic magic_1, RealMagic magic_2)
    {
        int index_1 = 0;
        int index_2 = 0;
        RealMagic data_1 = null;
        RealMagic data_2 = null;
        for (int i = 0; i < allMagicData.useMagic.Length; i++)
        {
            if (allMagicData.useMagic[i] == null)
            {
                continue;
            }
            if (allMagicData.useMagic[i].magicID == magic_1.magicID || allMagicData.useMagic[i].magicID == magic_2.magicID)
            {
                data_1 = allMagicData.useMagic[i];
                index_1 = i;
            }
        }
        for (int i = 0; i < allMagicData.readyMagic.Count; i++)
        {
            data_2 = allMagicData.readyMagic[i];
            index_2 = i;
        }
        allMagicData.useMagic[index_1] = data_2;
        allMagicData.readyMagic[index_2] = data_1;
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

public class MagicLevelUpHelper
{
    public int time;
    public int id;
    public MagicLevelUpHelper(int id, int time)
    {
        this.id = id;
        this.time = time;
    }
}

public class MagicWorkShopHelper
{
    public RealMagic[] useMagic;
    public List<RealMagic> readyMagic;
    public List<RealMagic> workQueue;

    public MagicWorkShopHelper()
    {
        useMagic = new RealMagic[6];
        readyMagic = new List<RealMagic>();
        workQueue = new List<RealMagic>();
    }
    public MagicWorkShopHelper(RealMagic[] useMagic, List<RealMagic> readyMagic, List<RealMagic> workQueue)
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
