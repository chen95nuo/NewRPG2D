using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HatcheryData
{
    [SerializeField]
    private int id;
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
    public HatcheryData() { }

    public HatcheryData(int id)
    {
        this.id = id;
    }
}
