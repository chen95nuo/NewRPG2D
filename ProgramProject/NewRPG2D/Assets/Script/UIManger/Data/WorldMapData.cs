using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class WorldMapData : ItemBaseData
{
    public string Name;
    public int ChapterID;
    public string SpriteName;

    public override XmlName ItemXmlName
    {
        get { return XmlName.WorldMapData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        Name = ReadXmlDataMgr.StrParse(node, "Name");
        ChapterID = ReadXmlDataMgr.IntParse(node, "LessonID");
        SpriteName = ReadXmlDataMgr.StrParse(node, "SpriteName");
        return base.GetXmlDataAttribute(node);
    }
}
