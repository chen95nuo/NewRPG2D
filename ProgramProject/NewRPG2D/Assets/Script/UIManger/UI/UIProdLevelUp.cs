using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIProdLevelUp : TTUIPage
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_Tip_5;
    public Text txt_Tip_6;
    public Text txt_Tip_7;

    public Text txt_Name;
    public Text txt_Level;
    public Text txt_Yield;
    public Text txt_YieldUp;
    public Text txt_Stock;
    public Text txt_StockUp;
    public Text txt_Dimonds;
    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;


    public Button btn_back;
    public Button btn_BgBack;
    public Button btn_NowUp;
    public Button btn_LevelUp;

    private float needGold = 0;
    private float needMana = 0;
    private float needWood = 0;
    private float needIron = 0;

    private RoomMgr roomData;

    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
        btn_BgBack.onClick.AddListener(ClosePage);
        btn_NowUp.onClick.AddListener(ChickNowUp);
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
        CloseTxt(false);
        switch (roomData.buildingData.RoomName)
        {
            case BuildRoomName.Gold:
                UpdateInfo(roomData);
                break;
            case BuildRoomName.GoldSpace:
                break;
            case BuildRoomName.Food:
                UpdateInfo(roomData);
                break;
            case BuildRoomName.FoodSpace:
                break;
            case BuildRoomName.Wood:
                UpdateInfo(roomData);
                break;
            case BuildRoomName.WoodSpace:
                break;
            case BuildRoomName.Mana:
                UpdateInfo(roomData);
                break;
            case BuildRoomName.ManaSpace:
                break;
            case BuildRoomName.Iron:
                UpdateInfo(roomData);
                break;
            case BuildRoomName.IronSpace:
                break;
            default:
                break;
        }
    }

    private void UpdateInfo(RoomMgr data)
    {
        txt_Tip_1.text = "升级增加";
        txt_Tip_2.text = "每小时产量";
        txt_Tip_3.text = "房间容量";
        txt_Tip_4.text = "升级增加产量和容量";
        txt_Tip_5.text = "立即升级";
        txt_Tip_6.text = "升级";

        PlayerData playerData = GetPlayerData.Instance.GetData();
        BuildingData b_Data_1;//当前房间信息
        BuildingData b_Data_2;//下一级房间信息
        b_Data_1 = data.buildingData;
        txt_Name.text = b_Data_1.RoomName.ToString();
        txt_Level.text = b_Data_1.Level.ToString();
        txt_Yield.text = b_Data_1.Param1.ToString("#0");
        txt_Stock.text = b_Data_1.Param2.ToString("#0");
        if (b_Data_1.NexLevelID == 0)//如果满级了
        {
            CloseTxt(false);
            txt_YieldUp.text = "+" + 0;
            txt_StockUp.text = "+" + 0;
            return;
        }
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(b_Data_1.NexLevelID);
        txt_YieldUp.text = "+" + (b_Data_2.Param1 - b_Data_1.Param1);
        txt_StockUp.text = "+" + (b_Data_2.Param2 - b_Data_1.Param2);
        if (b_Data_2.NeedGold > 0)
        {
            txt_Gold.gameObject.SetActive(true);
            txt_Gold.text = b_Data_2.NeedGold.ToString();
            needGold = playerData.Gold - b_Data_2.NeedGold;

        }
        if (b_Data_2.NeedMana > 0)
        {
            txt_Mana.gameObject.SetActive(true);
            txt_Mana.text = b_Data_2.NeedMana.ToString();
            needMana = playerData.Mana - b_Data_2.NeedMana;
        }
        if (b_Data_2.NeedWood > 0)
        {
            txt_Wood.gameObject.SetActive(true);
            txt_Wood.text = b_Data_2.NeedWood.ToString();
            needWood = playerData.Wood - b_Data_2.NeedWood;
        }
        if (b_Data_2.NeedIron > 0)
        {
            txt_Iron.gameObject.SetActive(true);
            txt_Iron.text = b_Data_2.NeedIron.ToString();
            needIron = playerData.Iron - b_Data_2.NeedIron;
        }

        txt_Tip_7.text = b_Data_2.NeedTime + "min";
    }

    private void CloseTxt(bool isTrue)
    {
        txt_Gold.gameObject.SetActive(isTrue);
        txt_Mana.gameObject.SetActive(isTrue);
        txt_Wood.gameObject.SetActive(isTrue);
        txt_Iron.gameObject.SetActive(isTrue);
    }

    private void ChickNowUp()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        //如果钻石够直接扣钻石 如果钻石不够打开购买钻石界面
        if (data.Diamonds > 100)
        {
            data.Diamonds -= 100;
            //直接升级跳过时间
        }
        else
        {
            Debug.Log("钻石不足");
            //跳转到购买钻石界面
        }
        ClosePage();
    }

    /// <summary>
    /// 检查升级按钮
    /// </summary>
    private void ChickLevelUp()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        BuildingData b_Data_2;//下一级房间信息
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(roomData.buildingData.NexLevelID);
        //如果材料足够进入升级 如果材料不够 提示是用钻石购买材料
        //需要各个材料的值，若不足需要知道还缺多少
        if (needGold >= 0 && needMana >= 0 && needWood >= 0 && needIron >= 0)
        {
            //材料足够倒计时升级
            roomData.RoomLevelUp();
            ClosePage();
            return;
        }
        else
        {
            int[] needStock = new int[4];
            if (needGold < 0)
            {
                needStock[0] = (int)-needGold;
            }
            if (needMana < 0)
            {
                needStock[0] = (int)-needMana;
            }
            if (needWood < 0)
            {
                needStock[0] = (int)-needWood;
            }
            if (needIron < 0)
            {
                needStock[0] = (int)-needIron;
            }
        }
        ClosePage();
    }

    private void ClosePage()
    {
        UIPanelManager.instance.ClosePage<UIProdLevelUp>();
    }
}
