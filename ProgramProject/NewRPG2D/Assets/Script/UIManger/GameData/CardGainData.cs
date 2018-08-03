using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGainData
{
    private int quality;
    private string spriteName;
    private int level;
    private float currentExp;
    private float maxExp;
    private float addExp;
    private float playerAddExp;
    private int playerLevel;
    private float playerExp;

    public int Quality
    {
        get
        {
            return quality;
        }
    }

    public string SpriteName
    {
        get
        {
            return spriteName;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public float CurrentExp
    {
        get
        {
            return currentExp;
        }
    }

    public float MaxExp
    {
        get
        {
            return maxExp;
        }
    }

    public float AddExp
    {
        get
        {
            return addExp;
        }
    }

    public float PlayerAddExp
    {
        get
        {
            return playerAddExp;
        }
    }

    public int PlayerLevel
    {
        get
        {
            return playerLevel;
        }
    }

    public float PlayerExp
    {
        get
        {
            return playerExp;
        }
    }

    public CardGainData() { }

    public CardGainData(int quality, string spriteName, int level, float currentExp,
        float maxExp, float addExp, int playerLevel, float playerExp, float playerAddExp)
    {
        this.quality = quality;
        this.spriteName = spriteName;
        this.level = level;
        this.currentExp = currentExp;
        this.maxExp = maxExp;
        this.addExp = addExp;
        this.playerLevel = playerLevel;
        this.playerExp = playerExp;
        this.playerAddExp = playerAddExp;
    }
}
