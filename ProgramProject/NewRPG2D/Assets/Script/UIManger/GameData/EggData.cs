using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EggData
{
    [SerializeField]
    private string name;//物品名字

    [SerializeField]
    private int id;//物品的id

    [SerializeField]
    private string spriteName;//物品的图片名称

    [SerializeField]
    private ItemType itemType;//物品的类型

    [SerializeField]
    private string attribute;//物品的属性

    [SerializeField]
    private int quality;//物品的品质

    [SerializeField]
    private int starsLevel;//物品的星级

    [SerializeField]
    private int itemNumber;//物品的数量

    [SerializeField]
    private bool isKnown;//是否存于图鉴中

    [SerializeField]
    private float hatchingTime;//孵化时间

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

    public string Attribute
    {
        get
        {
            return attribute;
        }
    }

    public bool IsKnown
    {
        get
        {
            return isKnown;
        }
    }

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public float HatchingTime
    {
        get
        {
            return hatchingTime;
        }
    }

    public int StarsLevel
    {
        get
        {
            return starsLevel;
        }
    }

    public int ItemNumber
    {
        get
        {
            return itemNumber;
        }
    }

    public EggData() { }

    public EggData(int id)
    {
        this.id = id;
    }
    public EggData(int id, string sprite)
    {
        this.id = id;
        this.spriteName = sprite;
    }
    public EggData(int id, string sprite, ItemType type)
    {
        this.id = id;
        this.spriteName = sprite;
        this.itemType = type;
    }
}
