
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

[System.Serializable]
public class BuildingData : ItemBaseCsvData
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

    public override CsvEChartsType ItemCsvName
    {
        get { return CsvEChartsType.BuildingData; }
    }

    public override bool AnalySis(string[] data)
    {
        if (base.AnalySis(data))
        {
            Level = IntParse(data, 2);
            string name = StrParse(data, 3);
            RoomName = (BuildRoomName)System.Enum.Parse(typeof(BuildRoomName), name);
            RoomType = (RoomType)IntParse(data, 4);
            NeedGold = IntParse(data, 5);
            NeedMana = IntParse(data, 6);
            NeedWood = IntParse(data, 7);
            NeedIron = IntParse(data, 8);
            NeedTime = IntParse(data, 9);
            NeedLevel = IntParse(data, 10);
            RoomSize = IntParse(data, 11);
            RoomRole = ChickRoomRole(RoomSize);
            MergeID = IntParse(data, 12);
            SplitID = IntParse(data, 13);
            UnlockLevel = FloatArray(ListParse(data, 14));
            NextLevelID = IntParse(data, 15);
            Param1 = FloatParse(data, 16);
            Param2 = FloatParse(data, 17);
            RolePoint = Vector2Parse(data, 18);
            RoleAnim = StringArray(data, 19);
            return true;
        }
        return false;
    }

    private string[] StringArray(string[] data, int index)
    {
        string a = StrParse(data, index);
        return a.Split(',');
    }

    private Vector2[] Vector2Parse(string[] data, int index)
    {
        List<float> astr = ListParse(data, index);
        Vector2[] point = new Vector2[astr.Count];
        for (int i = 0; i < astr.Count; i++)
        {
            float f = astr[i];
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
