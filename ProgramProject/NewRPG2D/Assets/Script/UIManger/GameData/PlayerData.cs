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
    private int physical;//体力

    [SerializeField]
    private List<FurnaceData> furnace;//熔炉

    [SerializeField]
    private int mapNumber;//玩家所在地图序号

    private int addLevel;//添加等级

    private int addExp;//添加经验

    private int addGlodCoin;//添加金币

    private int addDiamonds;//添加钻石

    private int addPhysical;//添加体力

    private int savePlayerPoint;//记录玩家地图信息

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
    }

    public int GoldCoin
    {
        get
        {
            return goldCoin;
        }
    }

    public int Diamonds
    {
        get
        {
            return diamonds;
        }
    }

    public int Physical
    {
        get
        {
            return physical;
        }
    }

    public int MapNumber
    {
        get
        {
            return mapNumber;
        }
    }

    public int AddLevel
    {
        get
        {
            return addLevel;
        }

        set
        {
            if (level + value >= 0)
            {
                level += value;
            }
            else
            {
                Debug.Log("等级不足");
            }
        }
    }

    public int AddExp
    {
        set
        {
            if (exp + value >= 0)
            {
                exp += value;
            }
            else
            {
                Debug.Log("经验不足");
            }
        }
    }

    public int AddGlodCoin
    {
        get
        {
            return addGlodCoin;
        }

        set
        {
            if (goldCoin + value >= 0)
                goldCoin += value;
            else
                Debug.Log("金币不足");
        }
    }

    public int AddDiamonds
    {
        get
        {
            return addDiamonds;
        }

        set
        {
            if (diamonds + value >= 0)
                diamonds += value;
            else
                Debug.Log("钻石不足");
        }
    }

    public int AddPhysical
    {
        get
        {
            return addPhysical;
        }

        set
        {
            if (physical + value >= 0)
                physical += value;
            else
                Debug.Log("体力不足");
        }
    }

    public int SavePlayerPoint
    {
        get
        {
            return savePlayerPoint;
        }

        set
        {
            savePlayerPoint = value;
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