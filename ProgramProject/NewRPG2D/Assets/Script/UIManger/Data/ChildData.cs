using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class ChildData : ItemBaseData
{
    public int AllStar;//总星级
    public int[] StarLevel;//星级
    public int StandLevel;//标准等级

    public override XmlName ItemXmlName
    {
        get { return XmlName.ChildData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        AllStar = ReadXmlDataMgr.IntParse(node, "AllStar");
        StarLevel = new int[5];
        StarLevel[0] = ReadXmlDataMgr.IntParse(node, "Star1");
        StarLevel[1] = ReadXmlDataMgr.IntParse(node, "Star2");
        StarLevel[2] = ReadXmlDataMgr.IntParse(node, "Star3");
        StarLevel[3] = ReadXmlDataMgr.IntParse(node, "Star4");
        StarLevel[4] = ReadXmlDataMgr.IntParse(node, "Star5");
        StandLevel = ReadXmlDataMgr.IntParse(node, "StandStar");
        return base.GetXmlDataAttribute(node);
    }
}
