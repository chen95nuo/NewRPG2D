using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    private string name;//玩家昵称
    private int level;//城堡等级
    private int gold;//金币数量
    private int diamonds;//钻石数量
    private int food;//食物
    private int mana;//法力值
    private int wood;//木材
    private int iron;//铁矿
    private int mainHallLevel;//大殿等级
    private int goldSpace;//金币空间
    private int foodSpace;//食物空间
    private int manaSpace;//魔法空间
    private int woodSpace;//木材空间
    private int ironSpace;//铁矿空间


    public string Name
    {
        get
        {
            return name;
        }

        set
        {
            name = value;
        }
    }

    public int Level
    {
        get
        {
            return level;
        }

        set
        {
            int temp = value;
            if (temp != level)
            {
                level = temp;
                //HallEventManager.instance.SendEvent(HallEventDefineEnum.)
            }
        }
    }

    public int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            int temp = value;
            if (temp != gold)
            {
                gold = temp;
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Gold);
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
            int temp = value;
            if (temp != diamonds)
            {
                diamonds = temp;
                HallEventManager.instance.SendEvent(HallEventDefineEnum.diamondsSpace);
            }
        }
    }

    public int Food
    {
        get
        {
            return food;
        }

        set
        {
            int temp = value;
            if (temp != food)
            {
                food = temp;
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Food);
            }
        }
    }

    public int Mana
    {
        get
        {
            return mana;
        }

        set
        {
            int temp = value;
            if (temp != mana)
            {
                mana = temp;
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Mana);
            }
        }
    }

    public int Wood
    {
        get
        {
            return wood;
        }

        set
        {
            int temp = value;
            if (temp != wood)
            {
                wood = temp;
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Wood);
            }
        }
    }

    public int Iron
    {
        get
        {
            return iron;
        }

        set
        {
            int temp = value;
            if (temp != iron)
            {
                iron = temp;
                //HallEventManager.instance.SendEvent<BuildRoomName>(HallEventDefineEnum.ChickStock, BuildRoomName.Iron);
            }
        }
    }

    public int MainHallLevel
    {
        get
        {
            return mainHallLevel;
        }

        set
        {
            mainHallLevel = value;
        }
    }

    public int GoldSpace
    {
        get
        {
            return goldSpace;
        }

        set
        {
            goldSpace = value;
        }
    }

    public int FoodSpace
    {
        get
        {
            return foodSpace;
        }

        set
        {
            foodSpace = value;
        }
    }

    public int ManaSpace
    {
        get
        {
            return manaSpace;
        }

        set
        {
            manaSpace = value;
        }
    }

    public int WoodSpace
    {
        get
        {
            return woodSpace;
        }

        set
        {
            woodSpace = value;
        }
    }

    public int IronSpace
    {
        get
        {
            return ironSpace;
        }

        set
        {
            ironSpace = value;
        }
    }

    public PlayerData()
    {
        Name = "Baymax";
        Level = 1;
        Gold = 16000;
        Diamonds = 100;
        Food = 0;
        Mana = 0;
        Wood = 0;
        Iron = 0;
        MainHallLevel = 1;
        GoldSpace = 5000;
        FoodSpace = 5000;
        ManaSpace = 5000;
        WoodSpace = 5000;
        IronSpace = 5000;
    }
}
