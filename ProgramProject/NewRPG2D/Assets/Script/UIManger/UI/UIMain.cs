using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMain : TTUIPage
{
    public GameObject rightDown;
    public GameObject leftDown;
    public Button btn_UIMarket;
    public Button btn_UIChat;
    public Button btn_UIRank;
    public Button btn_UIFightInfor;

    public UIMarket market;


    public Button btn_gold;
    public Button btn_food;
    public Button btn_mana;
    public Button btn_wood;
    public Button btn_iron;
    public Button btn_diamonds;

    public Text text_gold;
    public Text text_food;
    public Text text_mana;
    public Text text_wood;
    public Text text_iron;
    public Text text_diamonds;

    private PlayerData playerData;

    private ServerBuildData[] spaceStock = new ServerBuildData[5];

    private void Awake()
    {
        btn_UIMarket.onClick.AddListener(ShowMarket);
        market.gameObject.SetActive(false);

        HallEventManager.instance.AddListener(HallEventDefineEnum.diamondsSpace, ChickDiamonds);
        HallEventManager.instance.AddListener<BuildRoomName>(HallEventDefineEnum.ChickStock, GetSpace);

        Init();
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.diamondsSpace, ChickDiamonds);
        HallEventManager.instance.RemoveListener<BuildRoomName>(HallEventDefineEnum.ChickStock, GetSpace);
    }
    private void Init()
    {
        playerData = GetPlayerData.Instance.GetData();
        if (playerData.MainHallLevel > 0)
        {
            btn_gold.gameObject.SetActive(true);
            text_gold.text = playerData.Gold.ToString();
            btn_food.gameObject.SetActive(true);
            text_food.text = playerData.Food.ToString();
        }
        if (playerData.MainHallLevel >= 4)
        {
            btn_mana.gameObject.SetActive(true);
            text_mana.text = playerData.Mana.ToString();
        }
        if (playerData.MainHallLevel >= 6)
        {
            btn_wood.gameObject.SetActive(true);
            text_wood.text = playerData.Wood.ToString();
        }
        if (playerData.MainHallLevel >= 9)
        {
            btn_iron.gameObject.SetActive(true);
            text_iron.text = playerData.Iron.ToString();
        }
        text_diamonds.text = playerData.Diamonds.ToString();
    }
    private void ShowMarket()
    {
        CloseSomeUI(false);
        market.gameObject.SetActive(true);
    }

    public void CloseSomeUI(bool isTrue)
    {
        rightDown.SetActive(isTrue);
        leftDown.SetActive(isTrue);
        btn_UIChat.gameObject.SetActive(isTrue);
        btn_UIRank.gameObject.SetActive(isTrue);
        btn_UIFightInfor.gameObject.SetActive(isTrue);
    }

    private void GetSpace(BuildRoomName name)
    {
        switch (name)
        {
            case BuildRoomName.Gold:
                text_gold.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Gold).ToString();
                break;
            case BuildRoomName.GoldSpace:
                text_gold.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Gold).ToString();
                break;
            case BuildRoomName.Food:
                text_food.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food).ToString();
                break;
            case BuildRoomName.FoodSpace:
                text_food.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food).ToString();
                break;
            case BuildRoomName.Mana:
                text_mana.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Mana).ToString();
                break;
            case BuildRoomName.ManaSpace:
                text_mana.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Mana).ToString();
                break;
            case BuildRoomName.Wood:
                text_wood.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Wood).ToString();
                break;
            case BuildRoomName.WoodSpace:
                text_wood.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Wood).ToString();
                break;
            case BuildRoomName.Iron:
                text_iron.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Iron).ToString();
                break;
            case BuildRoomName.IronSpace:
                text_iron.text = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Iron).ToString();
                break;
            default:
                break;
        }
    }

    private void ChickDiamonds()
    {
        text_diamonds.text = playerData.Diamonds.ToString();
    }
}
