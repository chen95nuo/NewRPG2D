using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GainData
{
    [SerializeField]
    public int itemId;
    [SerializeField]
    public ItemType itemtype;
    [SerializeField]
    public int itemNumber;
    [SerializeField]
    public int addGoin;

    public GainData() { }
    public GainData(int id, ItemType itemtype, int number,int addGoin)
    {
        this.itemId = id;
        this.itemtype = itemtype;
        this.itemNumber = number;
        this.addGoin = addGoin;
    }
}
