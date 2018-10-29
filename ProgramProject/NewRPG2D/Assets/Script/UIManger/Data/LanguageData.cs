using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class LanguageData : ItemBaseData
{
    public string GetName;
    public string Chinese;

    public override XmlName ItemXmlName
    {
        get { return XmlName.LanguageData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        GetName = ReadXmlDataMgr.StrParse(node, "GetName");
        Chinese = ReadXmlDataMgr.StrParse(node, "Chinese");
        return base.GetXmlDataAttribute(node);
    }
}
