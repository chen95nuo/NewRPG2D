using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class ChildData : ItemBaseCsvData
{
    public int AllStar;//总星级
    public int[] StarLevel;//星级
    public int StandLevel;//标准等级

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.ChildData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            AllStar = IntParse(data, 1);
            StarLevel = new int[5];
            StarLevel[0] = IntParse(data, 2);
            StarLevel[1] = IntParse(data, 3);
            StarLevel[2] = IntParse(data, 4);
            StarLevel[3] = IntParse(data, 5);
            StarLevel[4] = IntParse(data, 6);
            StandLevel = IntParse(data, 7);
            return true;
        }
        return false;
    }
}
