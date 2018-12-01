using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class MagicData : ItemBaseCsvData
{
    public MagicName magicName;
    public MagicType magicType;
    public int level;
    public int produceTime;
    public int produceNeed;
    public int levelUpTime;
    public int levelUpNeed;
    public float param;
    public int needLevel;
    public int needWorkLevel;

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.MagicData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            string name = StrParse(data, 2);
            magicName = (MagicName)System.Enum.Parse(typeof(MagicName), name);
            magicType = (MagicType)IntParse(data, 3);
            level =IntParse(data, 4);
            produceTime = IntParse(data, 5);
            produceNeed = IntParse(data, 6);
            levelUpTime = IntParse(data, 7);
            levelUpNeed = IntParse(data, 8);
            param = FloatParse(data, 9);
            needLevel = IntParse(data,10);
            needWorkLevel = IntParse(data, 11);
            return true;
        }
        return false;
    }
}
