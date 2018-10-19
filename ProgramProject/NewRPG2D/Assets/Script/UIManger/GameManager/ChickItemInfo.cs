using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChickItemInfo : TSingleton<ChickItemInfo>
{
    private Dictionary<int, PropData> AllProp = new Dictionary<int, PropData>();
    private static int propInstanceId = 1000;

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
        data.propId = propInstanceId;
        AllProp.Add(propInstanceId, data);
        return data;
    }

    public PropData GetPropData(int propId)
    {
        return AllProp[propId];
    }
}
