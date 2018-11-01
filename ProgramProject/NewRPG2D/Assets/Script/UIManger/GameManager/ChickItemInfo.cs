using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Battle.BattleData;


public class ChickItemInfo : TSingleton<ChickItemInfo>
{
    private Dictionary<int, RealPropData> AllProp = new Dictionary<int, RealPropData>();
    private List<RealPropData> AllPropList = new List<RealPropData>();
    private Dictionary<int, BoxDataHelper> AllBox = new Dictionary<int, BoxDataHelper>();
    private List<BoxDataHelper> AllBoxList = new List<BoxDataHelper>();
    private static int propInstanceId = 1000;
    private static int boxInstanceId = 1000;

    public static int PropInstanceId
    {
        get
        {
            return propInstanceId++;
        }
    }

    public static int BoxInstanceId
    {
        get
        {
            return boxInstanceId++;
        }
    }

    public override void Init()
    {
        base.Init();
        AllProp.Clear();
    }

    public RealPropData CreateNewProp(int id)
    {
        foreach (var item in AllProp)
        {
            if (item.Value.propData != null)
            {
                if (item.Value.propData.ItemId == id && item.Value.number < 100)
                {
                    item.Value.number++;
                    return item.Value;
                }
            }
        }
        PropData pData = PropDataMgr.instance.GetXmlDataByItemId<PropData>(id);
        int index = PropInstanceId;
        RealPropData data = new RealPropData(pData, index, 1);
        data.instanceID = index;
        AllProp.Add(index, data);
        AllPropList.Add(data);
        return data;
    }
    public bool UseProp(int id)
    {
        if (AllProp[id].number <= 0)
        {
            Debug.LogError("道具数量出错");
            return false;
        }
        AllProp[id].number--;
        if (AllProp[id].number <= 0)
        {
            AllPropList.Remove(AllProp[id]);
            AllProp.Remove(id);
        }
        return true;
    }

    public RealPropData GetPropData(int propId)
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
        int index = BoxInstanceId;
        BoxDataHelper data = new BoxDataHelper(index, boxid, 1);
        AllBox.Add(index, data);
        AllBoxList.Add(data);
        return data;
    }

    public bool UseBox(int id)
    {
        if (AllBox.ContainsKey(id) == false || AllBox[id].num <= 0)
        {
            Debug.LogError("道具数量出错");
            return false;
        }
        AllBox[id].num--;
        if (AllBox[id].num <= 0)
        {
            AllBoxList.Remove(AllBox[id]);
            AllBox.Remove(id);
        }
        return true;
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
        List<EquipmentRealProperty> needData = new List<EquipmentRealProperty>();
        for (int i = 0; i < allData.Count; i++)
        {
            if (allData[i].EquipType == type)
            {
                needData.Add(allData[i]);
            }
        }
        needData.Sort(CompareByEquip);
        for (int i = 0; i < needData.Count; i++)
        {
            Items.Add(new ItemHelper(needData[i].EquipId, ItemType.Equip));
        }
        return Items;
    }

    public List<ItemHelper> GetEquip(EquipTypeEnum type_1, EquipTypeEnum type_2, EquipTypeEnum type_3)
    {
        List<ItemHelper> Items = new List<ItemHelper>();
        List<EquipmentRealProperty> allData = EquipmentMgr.instance.GetAllEquipmentData();
        List<EquipmentRealProperty> needData = new List<EquipmentRealProperty>();
        for (int i = 0; i < allData.Count; i++)
        {
            if (allData[i].EquipType == type_1 || allData[i].EquipType == type_2 || allData[i].EquipType == type_3)
            {
                needData.Add(allData[i]);
            }
        }
        needData.Sort(CompareByEquip);
        for (int i = 0; i < needData.Count; i++)
        {
            Items.Add(new ItemHelper(needData[i].EquipId, ItemType.Equip));
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
    public int CompareByProp(RealPropData x, RealPropData y)
    {
        if (x.propData.quality < y.propData.quality) return 1;
        if (x.propData.quality > y.propData.quality) return -1;
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

public class RealPropData
{
    public int instanceID;
    public PropData propData;
    public int number;

    public RealPropData(PropData propData, int id, int number = 0)
    {
        this.propData = propData;
        this.number = number;
    }
}