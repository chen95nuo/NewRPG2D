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

    private void Awake()
    {
        btn_UIMarket.onClick.AddListener(ShowMarket);
        market.gameObject.SetActive(false);

        HallEventManager.instance.AddListener(HallEventDefineEnum.GlodSpace, ChickGlodSpace);
        HallEventManager.instance.AddListener(HallEventDefineEnum.FoodSpace, ChickFoodSpace);
        HallEventManager.instance.AddListener(HallEventDefineEnum.ManaSpace, ChickManaSpace);
        HallEventManager.instance.AddListener(HallEventDefineEnum.WoodSpace, ChickWoodSpace);
        HallEventManager.instance.AddListener(HallEventDefineEnum.IronSpace, ChickIronSpace);
        HallEventManager.instance.AddListener(HallEventDefineEnum.diamondsSpace, ChickDiamonds);

        Init();
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.GlodSpace, ChickGlodSpace);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.FoodSpace, ChickFoodSpace);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ManaSpace, ChickManaSpace);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.WoodSpace, ChickWoodSpace);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.IronSpace, ChickIronSpace);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.diamondsSpace, ChickDiamonds);
    }
    private void Init()
    {
        playerData = GetPlayerData.Instance.GetData();
        if (playerData.mainHallLevel > 0)
        {
            btn_gold.gameObject.SetActive(true);
            text_gold.text = playerData.gold.ToString();
            btn_food.gameObject.SetActive(true);
            text_food.text = playerData.food.ToString();
        }
        if (playerData.mainHallLevel >= 4)
        {
            btn_mana.gameObject.SetActive(true);
            text_mana.text = playerData.mana.ToString();
        }
        if (playerData.mainHallLevel >= 6)
        {
            btn_wood.gameObject.SetActive(true);
            text_wood.text = playerData.wood.ToString();
        }
        if (playerData.mainHallLevel >= 9)
        {
            btn_iron.gameObject.SetActive(true);
            text_iron.text = playerData.iron.ToString();
        }
        text_diamonds.text = playerData.diamonds.ToString();
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

    private void ChickGlodSpace()
    {
        text_gold.text = playerData.gold.ToString();
    }
    private void ChickFoodSpace()
    {
        text_food.text = playerData.food.ToString();
    }
    private void ChickManaSpace()
    {
        text_mana.text = playerData.mana.ToString();
    }
    private void ChickWoodSpace()
    {
        text_wood.text = playerData.wood.ToString();
    }
    private void ChickIronSpace()
    {
        text_iron.text = playerData.iron.ToString();
    }
    private void ChickDiamonds()
    {
        text_diamonds.text = playerData.diamonds.ToString();
    }
}
