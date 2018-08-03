using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardExpData
{
    [SerializeField]
    private int level;
    [SerializeField]
    private float needExp;

    public int Level
    {
        get
        {
            return level;
        }
    }

    public float NeedExp
    {
        get
        {
            return needExp;
        }
    }
}
