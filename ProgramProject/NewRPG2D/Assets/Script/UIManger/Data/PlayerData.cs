using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Battle.BattleData;
using proto.SLGV1;

public class PlayerData
{
    private int roleId;//角色ID
    private int userId;//用户ID
    private string name;//玩家昵称
    private string spriteName;//头像
    private int diamonds;//钻石数量
    private Dictionary<BuildRoomName, int> produceSpace;//资源容量
    private int power;//战力
    private int grailNum;//圣杯数量
    private int popNum;//人口(居民)数量
    private int builderNum;//建筑工人数量
    private List<int> castleSkin;//城堡皮肤[背景,天气,桥梁,旗帜,砖瓦,墙壁,窗户]
    private int loginDays;//角色登陆天数
    private List<string> thirdAcctInfo;//绑定信息
    private Vector2 castle;//城堡等级
    private LocalBuildingData mainHall;
    private LocalBuildingData barracksLevel;
    private int currentLessonID;
    private MapLevelData currentLessonData;
    private int happiness = 0;//幸福度
    private bool vip;

    public bool Vip
    {
        get
        {
            return vip;
        }
    }

    public int Happiness
    {
        get
        {
            return happiness;
        }

        set
        {
            happiness = value;
            HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.CheckHappiness, happiness);
        }
    }


    //待删除
    public int Gold;
    public int Food;
    public int Mana;
    public int Wood;
    public int Iron;
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
            int x = (int)mainHall.buildingData.Param1;
            CameraControl.instance.currentAddWidth = (x - 17) * 2.7f;
            int y = (int)mainHall.buildingData.Param2;
            //if (EditCastle.instance != null)
            //{
            //    EditCastle.instance.ExtensionWall(x, y);
            //}
            //if (MainCastle.instance != null)
            //{
            //    MainCastle.instance.ExtensionWall(x, y);
            //}
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
        get
        {
            return currentLessonID;
        }
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

    public int defaultSpace
    {
        get { return 5000; }
    }

    public int GrailNum
    {
        get
        {
            return grailNum;
        }

        set
        {
            grailNum = value;
        }
    }

    public int BuilderNum
    {
        get
        {
            return builderNum;
        }

        set
        {
            builderNum = value;
        }
    }

    public int GetResSpace(BuildRoomName name)
    {
        return produceSpace[name];
    }

    public PlayerData(proto.SLGV1.CharacterInfo s_PlayerData, int happiness)
    {
        this.roleId = s_PlayerData.roleId;
        this.userId = s_PlayerData.userId;
        this.name = s_PlayerData.roleName;
        this.spriteName = s_PlayerData.headimgurl;
        this.diamonds = s_PlayerData.diamond;
        //this.diamonds = 0;
        produceSpace = new Dictionary<BuildRoomName, int>();
        produceSpace.Add(BuildRoomName.GoldSpace, s_PlayerData.gold);
        //produceSpace.Add(BuildRoomName.GoldSpace,0);
        produceSpace.Add(BuildRoomName.FoodSpace, s_PlayerData.appNum);
        produceSpace.Add(BuildRoomName.ManaSpace, s_PlayerData.magicNum);
        produceSpace.Add(BuildRoomName.WoodSpace, s_PlayerData.woodNum);
        produceSpace.Add(BuildRoomName.IronSpace, s_PlayerData.feNum);
        this.power = s_PlayerData.power;
        this.GrailNum = s_PlayerData.grailNum;
        this.popNum = s_PlayerData.popNum;
        this.BuilderNum = s_PlayerData.builderNum;
        this.castleSkin = s_PlayerData.castleSkin;
        this.loginDays = s_PlayerData.loginDays;
        this.thirdAcctInfo = s_PlayerData.thirdAcctInfo;
        this.Happiness = happiness;
        this.vip = false;
    }
}
