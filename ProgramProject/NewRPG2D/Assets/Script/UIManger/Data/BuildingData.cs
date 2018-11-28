using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;
using System.Xml;

[System.Serializable]
public class BuildingData : ItemBaseData
{
    public int Level;
    public BuildRoomName RoomName;//房间类型
    public RoomType RoomType;
    public int NeedGold;
    public int NeedMana;
    public int NeedWood;
    public int NeedIron;
    public int NeedTime;//建造时间
    public int NeedLevel;//需要大厅等级
    public int RoomSize;//房间大小
    public int RoomRole;//房间人数
    public int MergeID;//合并ID
    public int SplitID;//拆分ID
    public float[] UnlockLevel;//解锁等级
    public int NextLevelID;//下一级ID
    public float Param1;//参数1
    public float Param2;//参数2
    public Vector2[] RolePoint;//每个位置的角色坐标
    public string[] RoleAnim;//每个位置的角色动画

    public override XmlName ItemXmlName
    {
        get { return XmlName.BuildingData; }
    }

    public override bool GetXmlDataAttribute(XmlNode node)
    {
        Level = ReadXmlDataMgr.IntParse(node, "Level");
        string name = ReadXmlDataMgr.StrParse(node, "RoomName");
        RoomName = (BuildRoomName)System.Enum.Parse(typeof(BuildRoomName), name);
        RoomType = (RoomType)ReadXmlDataMgr.IntParse(node, "RoomType");
        NeedGold = ReadXmlDataMgr.IntParse(node, "NeedGold");
        NeedMana = ReadXmlDataMgr.IntParse(node, "NeedMana");
        NeedWood = ReadXmlDataMgr.IntParse(node, "NeedWood");
        NeedIron = ReadXmlDataMgr.IntParse(node, "NeedIron");
        NeedTime = ReadXmlDataMgr.IntParse(node, "NeedTime");
        NeedLevel = ReadXmlDataMgr.IntParse(node, "NeedLevel");
        RoomSize = ReadXmlDataMgr.IntParse(node, "RoomSize");
        RoomRole = ChickRoomRole(RoomSize);
        MergeID = ReadXmlDataMgr.IntParse(node, "MergeID");
        SplitID = ReadXmlDataMgr.IntParse(node, "SplitID");
        UnlockLevel = ReadXmlDataMgr.FloatArray(node, "UnlockLevel");
        NextLevelID = ReadXmlDataMgr.IntParse(node, "NextLevelID");
        Param1 = ReadXmlDataMgr.FloatParse(node, "Param1");
        Param2 = ReadXmlDataMgr.FloatParse(node, "Param2");
        RolePoint = Vector2Parse(node, "RolePoint");
        RoleAnim = StringArray(node, "RoleAnim");
        return base.GetXmlDataAttribute(node);
    }

    private string[] StringArray(XmlNode node, string name)
    {
        string a = ReadXmlDataMgr.StrParse(node, name);
        return a.Split(',');
    }

    private Vector2[] Vector2Parse(XmlNode node, string name)
    {
        string a = ReadXmlDataMgr.StrParse(node, name);
        string[] astr = a.Split(',');
        Vector2[] point = new Vector2[astr.Length];
        for (int i = 0; i < astr.Length; i++)
        {
            float f = float.Parse(astr[i]);
            point[i] = new Vector2(f, 0);
        }
        return point;
    }

    private int ChickRoomRole(int size)
    {
        if (size == 3)
        {
            return 2;
        }
        else if (size == 6)
        {
            return 4;
        }
        else if (size == 9)
        {
            return 6;
        }
        else
        {
            return 0;
        }
    }
}
