using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class PropData : ItemBaseData
{
    public string des;
    public string SpriteName;
    public QualityTypeEnum quality;
    public GetAccess getAccess;//获取方式
    public PropType propType;//道具类型
    public int num;

    public override XmlName ItemXmlName
    {
        get { return XmlName.PropData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        des = ReadXmlDataMgr.StrParse(node, "Description");
        SpriteName = ReadXmlDataMgr.StrParse(node, "SpriteId");
        quality = (QualityTypeEnum)ReadXmlDataMgr.IntParse(node, "Quality");
        getAccess = (GetAccess)ReadXmlDataMgr.IntParse(node, "GetAccess");
        propType = (PropType)ReadXmlDataMgr.IntParse(node, "PropType");
        return base.GetXmlDataAttribute(node);
    }
}
