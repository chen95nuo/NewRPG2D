using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class HallConfigData : ItemBaseCsvData
{

    public HallConfigEnum key;//key
    public float value;//value

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.HallConfigData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            string keystr = StrParse(data, 1);
            key = (HallConfigEnum)System.Enum.Parse(typeof(HallConfigEnum), keystr);
            value = FloatParse(data, 2);

            return true;
        }
        return false;
    }

}

public enum HallConfigEnum
{
    goldSpace,
    foodSpace,
    manaSpace,
    woodSpace,
    ironSpace,
    builder,//默认工人数量
    population,//默认人口
}
