using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnaceData
{
    [SerializeField]
    private int id;//熔炉序号

    [SerializeField]
    private int time;//时间

    [SerializeField]
    private ItemData[] material = new ItemData[4];//材料

    public int Id
    {
        get
        {
            return id;
        }
    }

    public int Time
    {
        get
        {
            return time;
        }
    }

    public ItemData[] Material
    {
        get
        {
            return material;
        }

        set
        {
            material = value;
        }
    }

    public FurnaceData() { }

    public FurnaceData(int id)
    {
        this.id = id;
    }

}
