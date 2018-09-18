using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

public class BuildingData : ItemBaseData
{

    //public int ID;
    public string Description;
    public int Level;
    public BuildRoomType RoomType;//房间类型
    public int NeedGold;
    public int NeedMana;
    public int NeedWood;
    public int NeedIron;
    public int NeedTime;//建造时间
    public int NeedLevel;//需要大厅等级
    public int RoomSize;//房间大小  
    public int MergeID;//合并ID
    public int SplitID;//拆分ID
    public int UnlockLevel;//解锁等级
    public int NexLevelID;//下一级ID
    public float Param1;//参数1
    public float Param2;//参数2
    public float Param3;//参数3
    public float Param4;//参数4

    public override XmlName ItemXmlName
    {
        get { return XmlName.BuildingData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        //ID = ReadXmlDataMgr.IntParse(node, "ID");
        Level = ReadXmlDataMgr.IntParse(node, "Level");
        Description = ReadXmlDataMgr.StrParse(node, "Description");
        RoomType = (BuildRoomType)ReadXmlDataMgr.IntParse(node, "RoomType");
        NeedGold = ReadXmlDataMgr.IntParse(node, "NeedGold");
        NeedMana = ReadXmlDataMgr.IntParse(node, "NeedMana");
        NeedWood = ReadXmlDataMgr.IntParse(node, "NeedWood");
        NeedIron = ReadXmlDataMgr.IntParse(node, "NeedIron");
        NeedTime = ReadXmlDataMgr.IntParse(node, "NeedTime");
        NeedLevel = ReadXmlDataMgr.IntParse(node, "NeedLevel");
        RoomSize = ReadXmlDataMgr.IntParse(node, "RoomSize");
        MergeID = ReadXmlDataMgr.IntParse(node, "MergeID");
        SplitID = ReadXmlDataMgr.IntParse(node, "SplitID");
        UnlockLevel = ReadXmlDataMgr.IntParse(node, "UnlockLevel");
        Param1 = ReadXmlDataMgr.FloatParse(node, "Param1");
        Param2 = ReadXmlDataMgr.FloatParse(node, "Param2");
        return base.GetXmlDataAttribute(node);
    }
}
