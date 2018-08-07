using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipData
{
    [SerializeField]
    private string name;//物品名字

    [SerializeField]
    private int id;//物品的id

    [SerializeField]
    private string spriteName;//物品的图片名称

    [SerializeField]
    private ItemType itemType;//道具的类型

    [SerializeField]
    private EquipType equipType;//装备的类型

    [SerializeField]
    private int quality;//品质

    [SerializeField]
    private int price;//价格

    [SerializeField]
    private string affix_1;//词缀 1

    [SerializeField]
    private string affix_2;//词缀 2

    [SerializeField]
    private string affix_3;//词缀 3

    [SerializeField]
    private string affix_4;//词缀 4

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

    public ItemType ItemType
    {
        get
        {
            return itemType;
        }
    }

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public int Price
    {
        get
        {
            return price;
        }
    }

    public string Affix_1
    {
        get
        {
            return affix_1;
        }
    }

    public string Affix_2
    {
        get
        {
            return affix_2;
        }
    }

    public string Affix_3
    {
        get
        {
            return affix_3;
        }
    }

    public string Describe
    {
        get
        {
            return describe;
        }
    }

    public EquipType EquipType
    {
        get
        {
            return equipType;
        }

        set
        {
            equipType = value;
        }
    }

    public string Affix_4
    {
        get
        {
            return affix_4;
        }
    }


    public EquipData() { }

    public EquipData(int id)
    {
        this.id = id;
    }
    public EquipData(EquipData data)
    {
        this.affix_1 = data.affix_1;
        this.affix_2 = data.affix_2;
        this.affix_3 = data.affix_3;
        this.affix_4 = data.affix_4;
        this.describe = data.describe;
        this.equipType = data.equipType;
        this.id = data.id;
        this.itemType = data.itemType;
        this.name = data.name;
        this.price = data.price;
        this.quality = data.quality;
        this.spriteName = data.spriteName;
    }
    public EquipData(EquipData data, string affix_1, string affix_2, string affix_3, string affix_4)
    {
        this.affix_1 = affix_1;
        this.affix_2 = affix_2;
        this.affix_3 = affix_3;
        this.affix_4 = affix_4;
        this.describe = data.describe;
        this.equipType = data.equipType;
        this.id = data.id;
        this.itemType = data.itemType;
        this.name = data.name;
        this.price = data.price;
        this.quality = data.quality;
        this.spriteName = data.spriteName;
    }
    public EquipData(int id, string sprite)
    {
        this.id = id;
        this.spriteName = sprite;
    }

    public EquipData(int id, string sprite, ItemType type)
    {
        this.id = id;
        this.spriteName = sprite;
        this.itemType = type;
    }
}
