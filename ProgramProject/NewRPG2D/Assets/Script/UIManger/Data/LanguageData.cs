using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class LanguageData : ItemBaseCsvData
{
    public string GetName;
    public string Chinese;

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.LanguageData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            GetName = StrParse(data, 1);
            Chinese = StrParse(data, 2);
            return true;
        }
        return false;
    }
}
