using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIPopUp_1 : TTUIPage
{
    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_DiaNum;
    public Button btn_enter;
    public Button btn_back;

    public bool isStockFull = false;//仓库满了
    private Dictionary<MaterialName, int> needStock;

    private bool diaEnough = true;

    private void Awake()
    {
        ChickAllTxt(false);

        txt_Tip_1.text = "购买所需物资";
        txt_Tip_2.text = "购买";
        txt_Tip_3.text = "取消";

        btn_enter.onClick.AddListener(ChickEnter);
        btn_back.onClick.AddListener(ClosePage);
    }

    private void OnDisable()
    {
        ChickAllTxt(false);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        Dictionary<MaterialName, int> needStock = mData as Dictionary<MaterialName, int>;
        UpdateInfo(needStock);
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
                txt_Gold.transform.parent.gameObject.SetActive(true);
                txt_Gold.text = item.Value.ToString("#0");
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.GoldSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
            if (item.Key == MaterialName.Mana)
            {
                txt_Mana.transform.parent.gameObject.SetActive(true);
                txt_Mana.text = item.Value.ToString("#0");
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.ManaSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
            if (item.Key == MaterialName.Wood)
            {
                txt_Wood.transform.parent.gameObject.SetActive(true);
                txt_Wood.text = item.Value.ToString("#0");
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.WoodSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
            if (item.Key == MaterialName.Iron)
            {
                txt_Iron.transform.parent.gameObject.SetActive(true);
                txt_Iron.text = item.Value.ToString("#0");
                if (ChickPlayerInfo.instance.ChickAllStock(BuildRoomName.IronSpace) < item.Value)
                {
                    isStockFull = true;
                }
            }
        }

        temp *= 0.01f;
        PlayerData player = GetPlayerData.Instance.GetData();
        if (player.Diamonds >= temp)
        {
            txt_DiaNum.text = temp.ToString("#0");
            diaEnough = true;
        }
        else
        {
            txt_DiaNum.text = "<color=#ee5151>" + temp.ToString("#0") + "</color>";
            diaEnough = false;
        }
    }

    private void ChickAllTxt(bool isTrue)
    {
        txt_Gold.transform.parent.gameObject.SetActive(isTrue);
        txt_Mana.transform.parent.gameObject.SetActive(isTrue);
        txt_Wood.transform.parent.gameObject.SetActive(isTrue);
        txt_Iron.transform.parent.gameObject.SetActive(isTrue);
    }

    private void ChickEnter()
    {
        PlayerData player = GetPlayerData.Instance.GetData();
        if (diaEnough)
        {
            isStockFull = false;
            object st = "钻石不足";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
        }
        //发现有材料仓库不足
        else if (isStockFull)
        {
            isStockFull = false;
            object st = "仓库空间余量不足请升级仓库";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
        }
        else
        {
            Debug.Log("添加材料");

            Dictionary<BuildRoomName, int> dic = new Dictionary<BuildRoomName, int>();
            foreach (var item in needStock)
            {
                BuildRoomName name = BuildRoomName.Nothing;
                if (item.Value > 0)
                {
                    switch (item.Key)
                    {
                        case MaterialName.Gold:
                            name = BuildRoomName.GoldSpace;
                            dic.Add(name, item.Value);
                            break;
                        case MaterialName.Food:
                            name = BuildRoomName.FoodSpace;
                            dic.Add(name, item.Value);
                            break;
                        case MaterialName.Mana:
                            name = BuildRoomName.ManaSpace;
                            dic.Add(name, item.Value);
                            break;
                        case MaterialName.Wood:
                            name = BuildRoomName.WoodSpace;
                            dic.Add(name, item.Value);
                            break;
                        case MaterialName.Iron:
                            name = BuildRoomName.IronSpace;
                            dic.Add(name, item.Value);
                            break;
                        default:
                            break;
                    }
                }
            }
            foreach (var item in dic)
            {
                ChickPlayerInfo.instance.AddStock(item.Key, item.Value);
            }
        }
        ClosePage();
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
