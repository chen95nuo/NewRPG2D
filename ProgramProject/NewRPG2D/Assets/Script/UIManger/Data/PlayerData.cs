using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Battle.BattleData;

public class PlayerData
{
    private string name;//玩家昵称
    private Vector2 castle;//城堡等级
    private int gold;//金币数量
    private int diamonds;//钻石数量
    private int food;//食物
    private int mana;//法力值
    private int wood;//木材
    private int iron;//铁矿
    private int goldSpace;//金币空间
    private int foodSpace;//食物空间
    private int manaSpace;//魔法空间
    private int woodSpace;//木材空间
    private int ironSpace;//铁矿空间
    private LocalBuildingData mainHall;
    private LocalBuildingData barracksLevel;
    private int currentLessonID;
    private MapLevelData currentLessonData;

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

    public Vector2 Castle
    {
        get
        {
            if (mainHall == null)
            {
                castle = new Vector2(17, 5);
            }
            else
            {
                castle = new Vector2(mainHall.buildingData.Param1, mainHall.buildingData.Param2);
            }
            return castle;
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
            if (MainHall != null)
            {
                return MainHall.buildingData.Level;
            }
            else return 1;
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

    public LocalBuildingData MainHall
    {
        get
        {
            return mainHall;
        }

        set
        {
            mainHall = value;
            if (UIMain.instance != null)
            {
                UIMain.instance.Init();
            }
            int x = (int)mainHall.buildingData.Param1;
            CameraControl.instance.currentAddWidth = (x - 17) * 2.7f;
            int y = (int)mainHall.buildingData.Param2;
            if (EditCastle.instance != null)
            {
                EditCastle.instance.ExtensionWall(x, y);
            }
            if (MainCastle.instance != null)
            {
                MainCastle.instance.ExtensionWall(x, y);
            }
        }
    }

    public MapLevelData CurrentLessonData
    {
        get
        {
            if (currentLessonData == null)
            {
                currentLessonData = MapLevelDataMgr.instance.GetXmlDataByItemId<MapLevelData>(currentLessonID);
            }
            return currentLessonData;
        }
        set
        {
            var temp = value;
            if (temp != currentLessonData)
            {
                currentLessonData = value;
                currentLessonID = value.ItemId;
            }
        }
    }
    public int CurrentLessonID
    {
        set
        {
            int temp = value;
            if (currentLessonID != value)
            {
                currentLessonID = value;
                currentLessonData = MapLevelDataMgr.instance.GetXmlDataByItemId<MapLevelData>(value);
            }
        }
    }

    public LocalBuildingData BarracksData
    {
        get
        {
            return barracksLevel;
        }
        set
        {
            barracksLevel = value;
        }
    }

    public PlayerData()
    {
        Name = "Baymax";
        Gold = 100;
        Diamonds = 10000;
        Food = 0;
        Mana = 0;
        Wood = 0;
        Iron = 0;
        GoldSpace = 5000;
        FoodSpace = 5000;
        ManaSpace = 5000;
        WoodSpace = 5000;
        IronSpace = 5000;
        currentLessonID = 10001;
    }
}
