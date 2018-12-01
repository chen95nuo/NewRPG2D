using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class WorldMapData : ItemBaseCsvData
{
    public string Name;
    public int ChapterID;
    public string SpriteName;

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.WorldMapData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            Name = StrParse(data, 2);
            ChapterID = IntParse(data, 3);
            SpriteName = StrParse(data, 4);
            return true;
        }
        return false;
    }
}
