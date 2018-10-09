using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

[System.Serializable]
public class TrainData : ItemBaseData
{
    public TrainType trainType;
    public float[] level;
    public int needTime;

    public override XmlName ItemXmlName
    {
        get { return XmlName.TrainData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        int typeIndex = ReadXmlDataMgr.IntParse(node, "TrainType");
        trainType = (TrainType)typeIndex;
        level = ReadXmlDataMgr.FloatArray(node, "Level");
        needTime = ReadXmlDataMgr.IntParse(node, "NeedTime");
        return base.GetXmlDataAttribute(node);
    }
}
