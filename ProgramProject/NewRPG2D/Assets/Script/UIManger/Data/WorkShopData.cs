using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class WorkShopData : ItemBaseData
{
    public QualityTypeEnum Quality;
    public int RoomID;
    public int NeedPropId;
    public int NeedPropNum;
    public int NeedMana;
    public float[] Level;
    public int NeedTime;
    public float[] EquipType;

    public override XmlName ItemXmlName
    {
        get { return XmlName.WorkShopData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        Quality = (QualityTypeEnum)ReadXmlDataMgr.IntParse(node, "Quality");
        RoomID = ReadXmlDataMgr.IntParse(node, "RoomID");
        NeedPropId = ReadXmlDataMgr.IntParse(node, "NeedPropID");
        NeedPropNum = ReadXmlDataMgr.IntParse(node, "NeedProduceNum");
        NeedMana = ReadXmlDataMgr.IntParse(node, "NeedMana");
        Level = ReadXmlDataMgr.FloatArray(node, "Level");
        NeedTime = ReadXmlDataMgr.IntParse(node, "NeedTime");
        EquipType = ReadXmlDataMgr.FloatArray(node, "EquipType");
        return base.GetXmlDataAttribute(node);
    }
}
