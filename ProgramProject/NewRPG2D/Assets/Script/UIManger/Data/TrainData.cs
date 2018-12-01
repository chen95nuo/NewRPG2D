using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

[System.Serializable]
public class TrainData : ItemBaseCsvData
{
    public TrainType trainType;
    public float[] level;
    public int needTime;

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.TrainData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            int typeIndex = IntParse(data, 1);
            trainType = (TrainType)typeIndex;
            level = FloatArray(StrParse(data, 2));
            needTime = IntParse(data, 3);
            return true;
        }
        return false;
    }
}
