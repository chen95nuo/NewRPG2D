using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComposedTableData
{
    [SerializeField]
    private int id;//装备编号

    [SerializeField]
    private int[] needMaterial = new int[6];//所需材料

    [SerializeField]
    private int[] needProp;//所需道具

    [SerializeField]
    private int weight;//权重

    public int Id
    {
        get
        {
            return id;
        }
    }

    public int[] NeedMaterial
    {
        get
        {
            return needMaterial;
        }
    }

    public int[] NeedProp
    {
        get
        {
            return needProp;
        }
    }

    public int Weight
    {
        get
        {
            return weight;
        }
    }

    public ComposedTableData() { }
}