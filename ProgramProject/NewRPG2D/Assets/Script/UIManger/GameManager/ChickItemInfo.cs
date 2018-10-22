using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Battle.BattleData;


public class ChickItemInfo : TSingleton<ChickItemInfo>
{
    private Dictionary<int, PropData> AllProp = new Dictionary<int, PropData>();
    private List<PropData> AllPropList = new List<PropData>();
    private Dictionary<int, BoxDataHelper> AllBox = new Dictionary<int, BoxDataHelper>();
    private List<BoxDataHelper> AllBoxList = new List<BoxDataHelper>();
    private static int propInstanceId = 1000;
    private static int boxInstanceId = 1000;

    public override void Init()
    {
        base.Init();
        AllProp.Clear();
    }

    public PropData CreateNewProp(int id)
    {
        foreach (var item in AllProp)
        {
            if (item.Value.ItemId == id && item.Value.num < 100)
            {
                item.Value.num++;
                return item.Value;
            }
        }
        propInstanceId++;
        PropData data = PropDataMgr.instance.GetXmlDataByItemId<PropData>(id);
        data.num = 1;
        data.instanceID = propInstanceId;
        AllProp.Add(propInstanceId, data);
        AllPropList.Add(data);
        return data;
    }

    public PropData GetPropData(int propId)
    {
        return AllProp[propId];
    }

    public BoxDataHelper GetBoxHelperData(int instanceId)
    {
        return AllBox[instanceId];
    }

    public TreasureBox GetBoxData(int instanceId)
    {
        int boxID = AllBox[instanceId].boxId;
        TreasureBox boxData = TreasureBoxDataMgr.instance.GetXmlDataByItemId<TreasureBox>(boxID);
        return boxData;
    }

    public BoxDataHelper CreateNewBox(int boxid)
    {
        foreach (var item in AllBox)
        {
            if (item.Value.boxId == boxid && item.Value.num < 100)
            {
                Debug.Log("已经有这个箱子了");
                item.Value.num++;
                return item.Value;
            }
        }
        boxInstanceId++;
        BoxDataHelper data = new BoxDataHelper(boxInstanceId, boxid, 1);
        AllBox.Add(boxInstanceId, data);
        AllBoxList.Add(data);
        return data;
    }

    public List<ItemHelper> GetAllItem()
    {
        List<ItemHelper> allItem = new List<ItemHelper>();
        List<EquipmentRealProperty> allData = EquipmentMgr.instance.GetAllEquipmentData();

        foreach (var item in AllBox)
        {
            allItem.Add(new ItemHelper(item.Key, ItemType.Box));
        }
        AllPropList.Sort(CompareByProp);
        for (int i = 0; i < AllPropList.Count; i++)
        {
            allItem.Add(new ItemHelper(AllPropList[i].instanceID, ItemType.Prop));
        }
        allData.Sort(CompareByEquip);
        for (int i = 0; i < allData.Count; i++)
        {
            allItem.Add(new ItemHelper(allData[i].EquipId, ItemType.Equip));
        }
        return allItem;
    }

    public List<ItemHelper> GetEquip(EquipTypeEnum type)
    {
        List<ItemHelper> Items = new List<ItemHelper>();
        List<EquipmentRealProperty> allData = EquipmentMgr.instance.GetAllEquipmentData();
        for (int i = 0; i < allData.Count; i++)
        {
            while (allData[i].EquipType != type)
            {
                allData.RemoveAt(i);
            }
        }
        allData.Sort(CompareByEquip);
        for (int i = 0; i < allData.Count; i++)
        {
            Items.Add(new ItemHelper(allData[i].EquipId, ItemType.Equip));
        }
        return Items;
    }

    public List<ItemHelper> GetAllBox()
    {
        List<ItemHelper> allbox = new List<ItemHelper>();
        foreach (var item in AllBox)
        {
            allbox.Add(new ItemHelper(item.Key, ItemType.Box));
        }
        return allbox;
    }

    public List<ItemHelper> GetAllProp()
    {
        List<ItemHelper> allProp = new List<ItemHelper>();
        AllPropList.Sort(CompareByProp);
        for (int i = 0; i < AllPropList.Count; i++)
        {
            allProp.Add(new ItemHelper(AllPropList[i].instanceID, ItemType.Prop));
        }
        return allProp;
    }


    public int CompareByEquip(EquipmentRealProperty x, EquipmentRealProperty y)
    {
        if (x.Level < y.Level) return 1;
        if (x.Level > y.Level) return -1;
        if (x.QualityType < y.QualityType) return 1;
        if (x.QualityType > y.QualityType) return -1;
        return 0;
    }
    public int CompareByProp(PropData x, PropData y)
    {
        if (x.quality < y.quality) return 1;
        if (x.quality > y.quality) return -1;
        return 0;
    }
}

public class BoxDataHelper
{
    public int instanceId;
    public int boxId;
    public int num;
    public BoxDataHelper(int instanceId, int boxId, int num)
    {
        this.instanceId = instanceId;
        this.boxId = boxId;
        this.num = num;
    }
}