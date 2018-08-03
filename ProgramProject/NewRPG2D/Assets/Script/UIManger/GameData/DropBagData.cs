using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropBagData
{
    [SerializeField]
    private int id;

    [SerializeField]
    private DropBagItems[] items;

    [SerializeField]
    private int maxDrop;

    [SerializeField]
    private int addGold;

    [SerializeField]
    private int addExp;

    [SerializeField]
    private int addPlayerExp;

    public int Id
    {
        get
        {
            return id;
        }
    }

    public DropBagItems[] Items
    {
        get
        {
            return items;
        }
    }

    public int MaxDrop
    {
        get
        {
            return maxDrop;
        }
    }

    public int AddGold
    {
        get
        {
            return addGold;
        }
    }

    public int AddExp
    {
        get
        {
            return addExp;
        }
    }

    public int AddPlayerExp
    {
        get
        {
            return addPlayerExp;
        }
    }
}

[System.Serializable]
public class DropBagItems
{
    [SerializeField]
    public int itemID;

    [SerializeField]
    public int dropOdds;//掉落概率

    [SerializeField]
    public ItemType itemType;

    [SerializeField]
    public int maxNumber;
}