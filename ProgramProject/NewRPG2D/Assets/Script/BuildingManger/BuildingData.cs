using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingData
{
    public int ID;
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
}
