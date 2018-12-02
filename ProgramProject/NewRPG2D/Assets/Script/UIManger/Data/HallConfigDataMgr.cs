using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class HallConfigDataMgr : ItemCsvDataBaseMgr<HallConfigDataMgr>
{
    protected override CsvEChartsType CurrentCsvName
    {
        get { return CsvEChartsType.HallConfigData; }
    }

    Dictionary<HallConfigEnum, float> dic;
    public Dictionary<HallConfigEnum, float> Dic
    {
        get
        {
            if (dic == null)
            {
                dic = new Dictionary<HallConfigEnum, float>();
                GetDic();
            }
            return dic;
        }
    }

    private void GetDic()
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            HallConfigData data = CurrentItemData[i] as HallConfigData;
            dic.Add(data.key, data.value);
        }
    }

    public float GetValue(HallConfigEnum name)
    {
        return Dic[name];
    }


}
