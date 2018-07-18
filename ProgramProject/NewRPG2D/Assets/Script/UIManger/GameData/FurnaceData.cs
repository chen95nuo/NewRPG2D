using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FurnaceData
{
    [SerializeField]
    private int id;//熔炉序号

    [SerializeField]
    private int needTime;//时间

    [SerializeField]
    private DateTime startTime;//开始时间

    [SerializeField]
    private DateTime endTime;//结束时间

    [SerializeField]
    private FurnaceType furnaceType = FurnaceType.Nothing;

    [SerializeField]
    private ItemData[] material;//材料

    [SerializeField]
    private FurnacePopUpMaterial[] popPoint;//气泡信息

    public int Id
    {
        get
        {
            return id;
        }
    }

    public int NeedTime
    {
        get
        {
            TimeSpan sp = endTime.Subtract(startTime);
            needTime = sp.Seconds;
            return needTime;
        }
    }

    public DateTime StartTime
    {
        get
        {
            return startTime;
        }
        set
        {
            if (value == startTime)
            {
                return;
            }
            else
            {
                startTime = value;
            }
        }
    }

    public DateTime EndTime
    {
        get
        {
            return endTime;
        }
        set
        {
            if (value == endTime)
            {
                return;
            }
            else
            {
                endTime = value;
            }
        }
    }

    public FurnaceType FurnaceType
    {
        get
        {
            return furnaceType;
        }

        set
        {
            if (value == furnaceType)
            {
                return;
            }
            else
            {
                furnaceType = value;
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateFurnaceEvent);
            }
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

    public FurnacePopUpMaterial[] PopPoint
    {
        get
        {
            return popPoint;
        }
    }

    public FurnaceData() { }

    public FurnaceData(int id)
    {
        this.id = id;
    }
    public FurnaceData(ItemData[] material, FurnacePopUpMaterial[] popPoint)
    {
        this.material = material;
        this.popPoint = popPoint;
    }
    public FurnaceData(FurnaceData data)
    {
        this.id = data.id;
        this.needTime = data.needTime;
        this.startTime = data.startTime;
        this.endTime = data.endTime;
        this.furnaceType = data.furnaceType;
        this.material = data.material;
        this.popPoint = data.popPoint;
    }
}

public class FurnacePopUpMaterial
{
    public ItemMaterialType materialType;//材料类型
    public int materialNumber;//材料数量
    public int materialPoint;//材料位置
}
