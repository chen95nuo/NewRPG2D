using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIProdLevelUp : UILevelUp
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_Tip_5;

    public Text txt_Yield;
    public Text txt_YieldUp;
    public Text txt_Stock;
    public Text txt_Stock_2;
    public Text txt_StockUp;
    public Text txt_StockUp_2;

    public Image yieldSlider;
    public Image stockSlider;
    public Image stock_2Slider;

    public RectTransform Type_1;
    public RectTransform Type_2;
    public RectTransform Type_3;

    public Image[] Icons;
    public Image[] stockIcons;
    public Image[] trainIcons;

    protected override void Init(RoomMgr data)
    {
        base.Init(data);
        UIReset();

        switch (data.BuildingData.RoomName)
        {
            case BuildRoomName.Gold:
                UpdateInfo_1(data);
                UpdateIcon("Gold", "GoldStock", "");
                return;
            case BuildRoomName.GoldSpace:
                UpdateInfo_2(data);
                UpdateIcon("Gold", "GoldStock", "");
                return;
            case BuildRoomName.Food:
                UpdateInfo_1(data);
                UpdateIcon("Food", "FoodStock", "");
                return;
            case BuildRoomName.FoodSpace:
                UpdateInfo_2(data);
                UpdateIcon("Food", "FoodStock", "");
                return;
            case BuildRoomName.Wood:
                UpdateInfo_1(data);
                UpdateIcon("Wood", "WoodStock", "");
                return;
            case BuildRoomName.WoodSpace:
                UpdateInfo_2(data);
                UpdateIcon("Wood", "WoodStock", "");
                return;
            case BuildRoomName.Mana:
                UpdateInfo_1(data);
                UpdateIcon("Mana", "ManaStock", "");
                return;
            case BuildRoomName.ManaSpace:
                UpdateInfo_2(data);
                UpdateIcon("Mana", "ManaStock", "");
                return;
            case BuildRoomName.Iron:
                UpdateInfo_1(data);
                UpdateIcon("Iron", "IronStock", "");
                return;
            case BuildRoomName.IronSpace:
                UpdateInfo_2(data);
                UpdateIcon("Iron", "IronStock", "");
                return;
            case BuildRoomName.LivingRoom:
                UpdateInfo_2(data);
                txt_Tip_4.text = "此房间可以增加城堡的最大居民数量并让居民们生育下一代，\n父母的潜能越高，孩子的潜能可能就越高";
                txt_Tip_5.text = "居民数量";
                UpdateIcon("HumanStock", "HumanStock", "");
                return;
            case BuildRoomName.Barracks:
                UpdateInfo_2(data);
                txt_Tip_4.text = "升级可提升参与战斗的人数";
                txt_Tip_5.text = "可携带人数";
                UpdateIcon("HumanStock", "HumanStock", "");
                return;
            default:
                break;
        }
        switch (data.BuildingData.RoomType)
        {
            case RoomType.Nothing:
                break;
            case RoomType.Production:
                break;
            case RoomType.Training:
                UpdateInfo_2(data);
                string space_1 = "       ";
                string space_2 = "     ";
                txt_Tip_4.text = string.Format("将城堡内的居民移动至该房间，提升居民的{0}等级。\n{1}能够影响战斗表现。", space_1, space_2);
                break;
            case RoomType.Support:
                break;
            case RoomType.Max:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 界面1 仅用于生产类
    /// </summary>
    /// <param name="data"></param>
    private void UpdateInfo_1(RoomMgr data)
    {
        Type_1.anchoredPosition = Vector3.zero;

        txt_Tip_1.text = "升级增加";
        txt_Tip_2.text = "每小时产量";
        txt_Tip_3.text = "房间容量";
        txt_Tip_4.text = "升级该建筑来增加";

        PlayerData playerData = GetPlayerData.Instance.GetData();
        BuildingData b_Data_1;//当前房间信息
        BuildingData b_Data_2;//下一级房间信息
        b_Data_1 = data.BuildingData;
        txt_Yield.text = b_Data_1.Param1.ToString("#0");
        txt_Stock.text = b_Data_1.Param2.ToString("#0");
        if (b_Data_1.NextLevelID == 0)//如果满级了
        {
            txt_YieldUp.text = "+" + 0;
            txt_StockUp.text = "+" + 0;
            return;
        }
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(b_Data_1.NextLevelID);
        txt_YieldUp.text = "+" + (b_Data_2.Param1 - b_Data_1.Param1);
        txt_StockUp.text = "+" + (b_Data_2.Param2 - b_Data_1.Param2);

    }

    /// <summary>
    /// 界面2 通用性强 用于储存类 训练类 起居室 军营
    /// </summary>
    /// <param name="data"></param>
    private void UpdateInfo_2(RoomMgr data)
    {
        Type_2.anchoredPosition = Vector3.zero;

        txt_Tip_4.text = "容量";

        if (data.BuildingData.RoomType == RoomType.Training)
        {
            Type_3.anchoredPosition = Vector3.zero;
        }

        PlayerData playerData = GetPlayerData.Instance.GetData();
        BuildingData b_Data_1;//当前房间信息
        BuildingData b_Data_2;//下一级房间信息
        b_Data_1 = data.BuildingData;
        txt_Stock_2.text = b_Data_1.Param2.ToString("#0");
        if (b_Data_1.NextLevelID == 0)//如果满级了
        {
            txt_StockUp_2.text = "+" + 0;
            return;
        }
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(b_Data_1.NextLevelID);
        txt_StockUp_2.text = "+" + (b_Data_2.Param2 - b_Data_1.Param2);
    }

    private void ChickTip_4(BuildingData data)
    {
        switch (data.RoomType)
        {
            case RoomType.Production:
                txt_Tip_5.text = "升级该建筑来增加";
                return;
            case RoomType.Training:
                Type_3.anchoredPosition = Vector3.zero;
                string space_1 = "       ";
                string space_2 = "     ";
                txt_Tip_5.text = string.Format("将城堡内的居民移动至该房间，提升居民的{0}等级。\n{1}能够影响战斗表现。", space_1, space_2);
                return;
            default:
                break;
        }
        switch (data.RoomName)
        {
            case BuildRoomName.Nothing:
                break;
            case BuildRoomName.Gold:
                break;
            case BuildRoomName.GoldSpace:
                break;
            case BuildRoomName.Food:
                break;
            case BuildRoomName.FoodSpace:
                break;
            case BuildRoomName.Mana:
                break;
            case BuildRoomName.ManaSpace:
                break;
            case BuildRoomName.Wood:
                break;
            case BuildRoomName.WoodSpace:
                break;
            case BuildRoomName.Iron:
                break;
            case BuildRoomName.IronSpace:
                break;
            case BuildRoomName.FighterRoom:
                break;
            case BuildRoomName.Kitchen:
                break;
            case BuildRoomName.Mint:
                break;
            case BuildRoomName.Laboratory:
                break;
            case BuildRoomName.Crafting:
                break;
            case BuildRoomName.Foundry:
                break;
            case BuildRoomName.LivingRoom:
                break;
            case BuildRoomName.TrophyRoom:
                break;
            case BuildRoomName.Hospital:
                break;
            case BuildRoomName.ClanHall:
                break;
            case BuildRoomName.MagicWorkShop:
                break;
            case BuildRoomName.MagicLab:
                break;
            case BuildRoomName.WeaponsWorkShop:
                break;
            case BuildRoomName.ArmorWorkShop:
                break;
            case BuildRoomName.GemWorkSpho:
                break;
            case BuildRoomName.Stairs:
                break;
            case BuildRoomName.ThroneRoom:
                break;
            case BuildRoomName.Barracks:
                break;
            case BuildRoomName.MaxRoom:
                break;
            default:
                break;
        }
    }

    private void UIReset()
    {
        Type_1.anchoredPosition = Vector3.up * 2000;
        Type_2.anchoredPosition = Vector3.up * 2000;
        Type_3.anchoredPosition = Vector3.up * 2000;
    }

    private void UpdateIcon(string icon, string stock, string train)
    {
        if (icon != "")
        {
            Sprite IconSp = GetSpriteAtlas.insatnce.GetIcon(icon);
            for (int i = 0; i < Icons.Length; i++)
            {
                Icons[i].sprite = IconSp;
            }
        }
        if (stock != "")
        {
            Sprite StockSp = GetSpriteAtlas.insatnce.GetIcon(stock);
            for (int i = 0; i < stockIcons.Length; i++)
            {
                stockIcons[i].sprite = StockSp;
            }
        }
        if (train != "")
        {
            Sprite trainSp = GetSpriteAtlas.insatnce.GetIcon(train);
            for (int i = 0; i < trainIcons.Length; i++)
            {
                trainIcons[i].sprite = trainSp;
            }
        }
    }
}
