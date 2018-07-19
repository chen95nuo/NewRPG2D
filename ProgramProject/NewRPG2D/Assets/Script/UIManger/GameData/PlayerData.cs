using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    [SerializeField]
    private int id; //玩家ID

    [SerializeField]
    private string name;//玩家昵称

    [SerializeField]
    private string playerSprite;//玩家头像

    [SerializeField]
    private int level;//玩家等级

    [SerializeField]
    private float exp;//经验

    [SerializeField]
    private int goldCoin;//金币

    [SerializeField]
    private int diamonds;//钻石

    [SerializeField]
    private int fatigue;//体力

    [SerializeField]
    private List<FurnaceData> furnace;//熔炉

    [SerializeField]
    private int mapNumber;//玩家所在地图序号

    public int Id
    {
        get
        {
            return id;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }
    }

    public string PlayerSprite
    {
        get
        {
            return playerSprite;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }
    }

    public float Exp
    {
        get
        {
            return exp;
        }
        set
        {
            float temp = exp;//用临时变量去存储之前的值

            exp = value;

            if (temp != value)//之前的值跟现在的值进行比较
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePlayerExp);
            }
        }
    }

    public int GoldCoin
    {
        get
        {
            return goldCoin;
        }
        set
        {
            int temp = goldCoin;//用临时变量去存储之前的值

            goldCoin = value;

            if (temp != value)//之前的值跟现在的值进行比较
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePlayerGoldCoin);
            }
        }
    }

    public int Diamonds
    {
        get
        {
            return diamonds;
        }
        set
        {
            int temp = diamonds;//用临时变量去存储之前的值

            diamonds = value;

            if (temp != value)//之前的值跟现在的值进行比较
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePlayerDiamonds);
            }
        }
    }

    public int Fatigue
    {
        get
        {
            return fatigue;
        }

        set
        {
            int temp = fatigue;

            fatigue = value;

            if (fatigue != value)
            {
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePlayerFatigue);
            }
        }
    }

    public int MapNumber
    {
        get
        {
            return mapNumber;
        }
        set
        {
            mapNumber = value;
        }
    }

    public List<FurnaceData> Furnace
    {
        get
        {
            return furnace;
        }
    }

    public PlayerData() { }


    public PlayerData(string playerSprite)
    {
        this.playerSprite = playerSprite;
    }
    public PlayerData(int id, List<FurnaceData> furnace)
    {
        this.id = id;
        this.furnace = furnace;
    }

}