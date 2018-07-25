using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HatcheryData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private long endTime;
    [SerializeField]
    private int needTime;
    [SerializeField]
    private HatcheryType hatcheryType;
    [SerializeField]
    private EggData eggdata;

    public int Id
    {
        get
        {
            return id;
        }
    }

    public HatcheryType HatcheryType
    {
        get
        {
            return hatcheryType;
        }

        set
        {
            hatcheryType = value;
        }
    }

    public EggData Eggdata
    {
        get
        {
            return eggdata;
        }

        set
        {
            eggdata = value;
        }
    }

    public int NeedTime
    {
        get
        {
            TimeSpan sp = DateTime.FromFileTime(endTime).Subtract(SystemTime.insatnce.GetTime());
            needTime = sp.Seconds;
            if (needTime < 0)
                needTime = 0;
            return needTime;
        }
    }

    public long EndTime
    {
        get
        {
            return endTime;
        }

        set
        {
            endTime = value;
        }
    }

    public HatcheryData() { }

    public HatcheryData(int id, HatcheryType hatcheryType)
    {
        this.id = id;
        this.hatcheryType = hatcheryType;
    }
}
