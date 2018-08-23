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
    private PropMeltingType melting;//可否熔炼

    [SerializeField]
    private int quality;//道具品质

    [SerializeField]
    private ItemType itemType;//物品类型

    [SerializeField]
    private PropType propType;//道具类型

    [SerializeField]
    private CurrencyType storePropType;//商品类型

    [SerializeField]
    private int sellPrice;//物品价格

    [SerializeField]
    private int buyPrice;//购买价格

    [SerializeField]
    private int usePrice;//使用价格

    [SerializeField]
    private int addProperty;//增加体力

    [SerializeField]
    private int addExp;//增加经验

    [SerializeField]
    private RawMaterial[] rawMaterial;//原料类型 数量

    [SerializeField]
    private string getPlace;//获取地点

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

    public int SellPrice
    {
        get
        {
            return sellPrice;
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
        set
        {
            number = value;
        }
    }

    public PropType PropType
    {
        get
        {
            return propType;
        }
    }

    public string GetPlace
    {
        get
        {
            return getPlace;
        }
    }

    public PropMeltingType Melting
    {
        get
        {
            return melting;
        }

        set
        {
            melting = value;
        }
    }

    public int UsePrice
    {
        get
        {
            return usePrice;
        }

        set
        {
            usePrice = value;
        }
    }

    public RawMaterial[] RawMaterial
    {
        get
        {
            return rawMaterial;
        }

        set
        {
            rawMaterial = value;
        }
    }

    public int BuyPrice
    {
        get
        {
            return buyPrice;
        }
    }

    public CurrencyType StorePropType
    {
        get
        {
            return storePropType;
        }
    }

    public ItemData() { }
    public ItemData(ItemType type)
    {
        this.itemType = type;

    }

    public ItemData(ItemData data)
    {
        this.usePrice = data.usePrice;
        this.spriteName = data.spriteName;
        this.rawMaterial = data.rawMaterial;
        this.quality = data.quality;
        this.sellPrice = data.sellPrice;
        this.buyPrice = data.buyPrice;
        this.number = data.number;
        this.name = data.name;
        this.melting = data.melting;
        this.itemType = data.itemType;
        this.id = data.id;
        this.getPlace = data.getPlace;
        this.describe = data.describe;
        this.addProperty = data.addProperty;
        this.addExp = data.addExp;
        this.propType = data.propType;
        this.storePropType = data.storePropType;
    }
    public ItemData(ItemData data, int number)
    {
        this.usePrice = data.usePrice;
        this.spriteName = data.spriteName;
        this.rawMaterial = data.rawMaterial;
        this.quality = data.quality;
        this.sellPrice = data.sellPrice;
        this.buyPrice = data.buyPrice;
        this.number = number;
        this.name = data.name;
        this.melting = data.melting;
        this.itemType = data.itemType;
        this.id = data.id;
        this.getPlace = data.getPlace;
        this.describe = data.describe;
        this.addProperty = data.addProperty;
        this.addExp = data.addExp;
        this.propType = data.propType;
        this.storePropType = data.storePropType;
    }
}

[System.Serializable]
public class RawMaterial
{
    [SerializeField]
    public ItemMaterialType materialType;
    [SerializeField]
    public int number;
}