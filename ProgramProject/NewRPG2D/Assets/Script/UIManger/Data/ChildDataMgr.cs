using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class ChildDataMgr : ItemCsvDataBaseMgr<ChildDataMgr>
{
    protected override CsvEChartsType CurrentCsvName
    {
        get { return CsvEChartsType.ChildData; }
    }

    public ChildData GetData(int allStar)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            ChildData data = CurrentItemData[i] as ChildData;
            if (data.AllStar == allStar)
            {
                return data;
            }
        }
        Debug.LogError("没有找到对应等级");
        return null;
    }
}
