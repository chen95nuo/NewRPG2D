using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class MagicData : ItemBaseData
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

    public override XmlName ItemXmlName
    {
        get { return XmlName.MagicData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        string name = ReadXmlDataMgr.StrParse(node, "Name");
        magicName = (MagicName)System.Enum.Parse(typeof(MagicName), name);
        magicType = (MagicType)ReadXmlDataMgr.IntParse(node, "Type");
        level = ReadXmlDataMgr.IntParse(node, "Level");
        produceTime = ReadXmlDataMgr.IntParse(node, "ProduceTime");
        produceNeed = ReadXmlDataMgr.IntParse(node, "ProduceNeed");
        levelUpTime = ReadXmlDataMgr.IntParse(node, "LevelUpTime");
        levelUpNeed = ReadXmlDataMgr.IntParse(node, "LevelUpNeed");
        param = ReadXmlDataMgr.FloatParse(node, "Attribute");
        needLevel = ReadXmlDataMgr.IntParse(node, "NeedLevel");
        needWorkLevel = ReadXmlDataMgr.IntParse(node, "NeedWorkLevel");
        return base.GetXmlDataAttribute(node);
    }
}
