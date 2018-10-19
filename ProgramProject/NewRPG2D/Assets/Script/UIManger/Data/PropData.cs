using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class PropData : ItemBaseData
{
    public string spriteId;
    public int quality;
    public string purpose;//用途
    public int getAccess;//获取方式
    public int propId;
    public int num;

    public override XmlName ItemXmlName
    {
        get { return XmlName.PropData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        spriteId = ReadXmlDataMgr.StrParse(node, "SpriteId");
        quality = ReadXmlDataMgr.IntParse(node, "Quality");
        purpose = ReadXmlDataMgr.StrParse(node, "Purpose");
        getAccess = ReadXmlDataMgr.IntParse(node, "GetAccess");
        return base.GetXmlDataAttribute(node);
    }
}
