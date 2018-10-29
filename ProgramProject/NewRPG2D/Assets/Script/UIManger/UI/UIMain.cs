﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIMain : TTUIPage
{
    public static UIMain instance;

    public GameObject rightDown;
    public GameObject leftDown;
    public Button btn_UIMarket;
    public Button btn_UIChat;
    public Button btn_UIRank;
    public Button btn_UIFightInfor;
    public Button btn_UIBag;
    public Button btn_UIMap;

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

    public Image goldSlider;
    public Image foodSlider;
    public Image manaSlider;
    public Image woodSlider;
    public Image ironSlider;

    public Image[] Icons;

    private PlayerData playerData;

    private ServerBuildData[] spaceStock = new ServerBuildData[5];

    public SpaceNumJump gold;
    public SpaceNumJump food;
    public SpaceNumJump mana;
    public SpaceNumJump wood;
    public SpaceNumJump iron;
    public SpaceNumJump diamonds;

    public Canvas[] hightIcons;

    private void Awake()
    {
        instance = this;


        btn_UIMarket.onClick.AddListener(ShowMarket);
        btn_UIBag.onClick.AddListener(ChickBag);
        btn_UIMap.onClick.AddListener(ChickMap);
        market.gameObject.SetActive(false);

        HallEventManager.instance.AddListener(HallEventDefineEnum.diamondsSpace, ChickDiamonds);
        HallEventManager.instance.AddListener<BuildRoomName>(HallEventDefineEnum.ChickStock, GetSpace);
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.UiMainHight, UIMainHight);

        Init();

        gold = new SpaceNumJump(text_gold);
        food = new SpaceNumJump(text_food);
        mana = new SpaceNumJump(text_mana);
        wood = new SpaceNumJump(text_wood);
        iron = new SpaceNumJump(text_iron);
        diamonds = new SpaceNumJump(text_diamonds);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.diamondsSpace, ChickDiamonds);
        HallEventManager.instance.RemoveListener<BuildRoomName>(HallEventDefineEnum.ChickStock, GetSpace);
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.UiMainHight, UIMainHight);

    }
    public void Init()
    {
        RefreshText();
        playerData = GetPlayerData.Instance.GetData();
        int level = playerData.MainHallLevel;
        if (level > 0)
        {
            btn_gold.gameObject.SetActive(true);
            text_gold.text = playerData.Gold.ToString();
            ChickAllStock(BuildRoomName.Gold, goldSlider);
            btn_food.gameObject.SetActive(true);
            text_food.text = playerData.Food.ToString();
            ChickAllStock(BuildRoomName.Food, foodSlider);
        }
        if (level >= 4)
        {
            btn_mana.gameObject.SetActive(true);
            text_mana.text = playerData.Mana.ToString();
            ChickAllStock(BuildRoomName.Mana, manaSlider);
        }
        if (level >= 6)
        {
            btn_wood.gameObject.SetActive(true);
            text_wood.text = playerData.Wood.ToString();
            ChickAllStock(BuildRoomName.Wood, woodSlider);
        }
        if (level >= 9)
        {
            btn_iron.gameObject.SetActive(true);
            text_iron.text = playerData.Iron.ToString();
            ChickAllStock(BuildRoomName.Iron, ironSlider);
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
        Debug.Log("检查容量");
        switch (name)
        {
            case BuildRoomName.Gold:
                gold.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Gold);
                StartCoroutine(JumpNumber(gold));
                break;
            case BuildRoomName.GoldSpace:
                gold.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Gold);
                StartCoroutine(JumpNumber(gold));
                break;
            case BuildRoomName.Food:
                food.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(food));
                break;
            case BuildRoomName.FoodSpace:
                food.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(food));
                break;
            case BuildRoomName.Mana:
                mana.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(mana));
                break;
            case BuildRoomName.ManaSpace:
                mana.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(mana));
                break;
            case BuildRoomName.Wood:
                wood.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(wood));
                break;
            case BuildRoomName.WoodSpace:
                wood.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(wood));
                break;
            case BuildRoomName.Iron:
                iron.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(iron));
                break;
            case BuildRoomName.IronSpace:
                iron.maxNum = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Food);
                StartCoroutine(JumpNumber(iron));
                break;
            default:
                break;
        }
    }

    private void ChickDiamonds()
    {
        text_diamonds.text = playerData.Diamonds.ToString();
    }

    public IEnumerator JumpNumber(SpaceNumJump data)
    {
        while (data.num < data.maxNum)
        {
            data.txt.text = data.num.ToString();
            float temp = (data.maxNum - data.num) * 0.1f;
            temp = temp <= 1f ? 1f : temp;
            data.num += (int)temp;
            yield return new WaitForSeconds(0.05f);
        }
        data.num = data.maxNum;
        data.txt.text = data.num.ToString();
    }

    public void ChickBag()
    {
        UIPanelManager.instance.ShowPage<UIBag>();
    }
    public void ChickMap()
    {
        UIPanelManager.instance.ShowPage<UIWorldMap>();
    }

    public void ChickAllStock(BuildRoomName name, Image slider)
    {
        float all = ChickPlayerInfo.instance.GetAllStock(name);
        float allStock = ChickPlayerInfo.instance.GetAllStockSpace(name);
        slider.fillAmount = (all / allStock);
    }

    public void RefreshText()
    {
        btn_mana.gameObject.SetActive(false);
        btn_wood.gameObject.SetActive(false);
        btn_iron.gameObject.SetActive(false);
    }

    public void UIMainHight(int index)
    {
        for (int i = 0; i < hightIcons.Length; i++)
        {
            hightIcons[i].sortingOrder = index;
        }
    }
}
public class SpaceNumJump
{
    public int num;
    public int maxNum;
    public Text txt;

    public SpaceNumJump(Text txt)
    {
        num = 0;
        this.maxNum = 0;
        this.txt = txt;
    }
}
