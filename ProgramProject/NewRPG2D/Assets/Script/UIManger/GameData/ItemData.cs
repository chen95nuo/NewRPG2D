using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

[System.Serializable]
public class ItemData
{
    [SerializeField]
    private string name;//道具名称

    [SerializeField]
    private int id;//物品的id

    [SerializeField]
    private string spriteName;//道具的图片名称

    [SerializeField]
    private int number;//道具的数量

    [SerializeField]
    private int quality;//道具品质

    [SerializeField]
    private ItemType itemType;//物品类型

    [SerializeField]
    private int propType;

    [SerializeField]
    private int price;//物品价格

    [SerializeField]
    private int addProperty;//增加体力

    [SerializeField]
    private int addExp;//增加经验

    [SerializeField]
    private string describe;//描述

    public string Name
    {
        get
        {
            return name;
        }
    }

    public int Id
    {
        get
        {
            return id;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public ItemType ItemType
    {
        get
        {
            return itemType;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }
    }

    public int AddProperty
    {
        get
        {
            return addProperty;
        }
    }

    public int AddExp
    {
        get
        {
            return addExp;
        }
    }

    public string Describe
    {
        get
        {
            return describe;
        }
    }

    public int Number
    {
        get
        {
            return number;
        }
    }

    public int PropType
    {
        get
        {
            return propType;
        }
    }

    public ItemData() { }

    public ItemData(int id)
    {
        this.id = id;
    }

    public ItemData(int id, string sprite)
    {
        this.id = id;
        this.spriteName = sprite;
    }
    public ItemData(int id, string sprite, ItemType type)
    {
        this.id = id;
        this.spriteName = sprite;
        this.itemType = type;
    }
}
