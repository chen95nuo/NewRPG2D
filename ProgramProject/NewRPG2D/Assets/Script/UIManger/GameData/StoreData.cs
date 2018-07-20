using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StoreData
{
    [SerializeField]
    private int propId;

    [SerializeField]
    private int propNumber;

    public int PropId
    {
        get
        {
            return propId;
        }
    }

    public int PropNumber
    {
        get
        {
            return propNumber;
        }
    }

    public StoreData() { }
    public StoreData(StoreData storeData, int propNumber)
    {
        this.propId = storeData.propId;
        this.propNumber = propNumber;
    }
}
