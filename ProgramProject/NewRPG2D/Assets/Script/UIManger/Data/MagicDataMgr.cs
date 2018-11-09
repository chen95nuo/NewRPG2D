using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class MagicDataMgr : ItemDataBaseMgr<MagicDataMgr>
{
    private MagicWorkShopHelper allMagicData;
    private Dictionary<MagicName, int> LevelDic = new Dictionary<MagicName, int>();
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

    public void CryNewMagic(int magicId, int time = 0)
    {
        int index = InstanceMagicID;
        MagicData magicData = GetXmlDataByItemId<MagicData>(magicId);
        RealMagic data = new RealMagic(index, magicData, time);
    }

    public void MagicLevelUp(int magicId, int time = 0)
    {

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
    public RealMagic[] workQueue;

    public MagicWorkShopHelper()
    {

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
