using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class MagicDataMgr : ItemDataBaseMgr<MagicDataMgr>
{
    private MagicWorkShopHelper allMagicData;
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
