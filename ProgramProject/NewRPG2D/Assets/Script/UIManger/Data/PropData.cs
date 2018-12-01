using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class PropData : ItemBaseCsvData
{
    public string des;
    public string SpriteName;
    public QualityTypeEnum quality;
    public GetAccess getAccess;//获取方式
    public PropType propType;//道具类型
    public int num;

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.PropData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            des = StrParse(data, 1);
            SpriteName = StrParse(data, 2);
            quality = (QualityTypeEnum)IntParse(data, 3);
            getAccess = (GetAccess)IntParse(data, 4);
            propType = (PropType)IntParse(data, 5);
            return true;
        }
        return false;
    }
}
