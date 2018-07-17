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

    public FurnacePopUpMaterial[] PopPoint
    {
        get
        {
            return popPoint;
        }

        set
        {
            popPoint = value;
        }
    }

    public FurnaceData() { }

    public FurnaceData(int id)
    {
        this.id = id;
    }
    public FurnaceData(ItemData[] material)
    {
        this.material = material;
    }

}

public class FurnacePopUpMaterial
{
    public ItemMaterialType materialType;//材料类型
    public int materialNumber;//材料数量
    public int materialPoint;//材料位置
}
