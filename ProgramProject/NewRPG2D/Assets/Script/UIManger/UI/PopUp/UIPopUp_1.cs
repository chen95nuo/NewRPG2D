using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUp_1 : TTUIPage
{
    public static UIPopUp_1 instance;

    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Button btn_enter;
    public Button btn_back;

    public bool isStockFull = false;//仓库满了
    private Dictionary<MaterialName, int> needStock;

    private void Awake()
    {
        instance = this;
        ChickAllTxt(false);

        txt_Tip_1.text = "购买所需物资";
        txt_Tip_3.text = "否";

        btn_enter.onClick.AddListener(ChickEnter);
        btn_enter.onClick.AddListener(ChickBack);

    }

    private void OnDisable()
    {
        ChickAllTxt(false);
    }

    public void UpdateInfo(Dictionary<MaterialName, int> needStock)
    {
        this.needStock = needStock;
        float temp = 0;
        foreach (var item in needStock)
        {
            temp += item.Value;
            if (item.Key == MaterialName.Gold)
            {
                txt_Gold.gameObject.SetActive(true);
                txt_Gold.text = "金币 :" + item.Value;
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.GoldSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
            if (item.Key == MaterialName.Mana)
            {
                txt_Mana.gameObject.SetActive(true);
                txt_Mana.text = "魔法 :" + item.Value;
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.ManaSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
            if (item.Key == MaterialName.Wood)
            {
                txt_Wood.gameObject.SetActive(true);
                txt_Wood.text = "木材 :" + item.Value;
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.WoodSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
            if (item.Key == MaterialName.Iron)
            {
                txt_Iron.gameObject.SetActive(true);
                txt_Iron.text = "铁矿 :" + item.Value;
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.IronSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
        }

        temp *= 0.01f;

        txt_Tip_2.text = (int)temp + "\n购买";
    }

    private void ChickAllTxt(bool isTrue)
    {
        txt_Gold.gameObject.SetActive(isTrue);
        txt_Mana.gameObject.SetActive(isTrue);
        txt_Wood.gameObject.SetActive(isTrue);
        txt_Iron.gameObject.SetActive(isTrue);
    }

    private void ChickEnter()
    {
        //发现有材料仓库不足
        if (isStockFull)
        {
            isStockFull = false;
        }
        else
        {
            Debug.Log("添加材料");
            foreach (var item in needStock)
            {
                BuildRoomName name = BuildRoomName.Nothing;
                if (item.Value > 0)
                {
                    switch (item.Key)
                    {
                        case MaterialName.Gold:
                            name = BuildRoomName.GoldSpace;
                            break;
                        case MaterialName.Food:
                            name = BuildRoomName.FoodSpace;
                            break;
                        case MaterialName.Mana:
                            name = BuildRoomName.ManaSpace;
                            break;
                        case MaterialName.Wood:
                            name = BuildRoomName.WoodSpace;
                            break;
                        case MaterialName.Iron:
                            name = BuildRoomName.IronSpace;
                            break;
                        default:
                            break;
                    }
                    ChickPlayerInfo.instance.AddStock(name, item.Value);
                }
            }
        }
    }

    public void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIPopUp_1>();
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }
}
