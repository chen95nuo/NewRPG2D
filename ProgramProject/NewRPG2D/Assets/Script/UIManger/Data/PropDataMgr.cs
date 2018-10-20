﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class PropDataMgr : ItemDataBaseMgr<PropDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.PropData; }
    }

    public List<PropData> GetPropDataByType(PropType propType, QualityTypeEnum qualityType)
    {
        List<PropData> dataList = new List<PropData>();
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            PropData data = CurrentItemData[i] as PropData;
            if (data == null || data.propType != propType || data.quality != qualityType)
            {
                continue;
            }
            dataList.Add(data);
        }

        return dataList;
    }
}
