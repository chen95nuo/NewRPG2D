﻿using System.Collections;
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

    private int needDiaNumber = 0;

    private void Awake()
    {
        ChickAllTxt(false);

        txt_Tip_1.text = LanguageDataMgr.instance.GetUIString("goumaisuoxuwuzi");
        txt_Tip_2.text = LanguageDataMgr.instance.GetUIString("goumai");
        txt_Tip_3.text = LanguageDataMgr.instance.GetUIString("quxiao");

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
            GameHelper.instance.MaterialNameToBuildRoomName(item.Key);
            switch (item.Key)
            {
                case MaterialName.Gold:
                    txt_Gold.transform.parent.gameObject.SetActive(true);
                    txt_Gold.text = item.Value.ToString();
                    temp += BuildingManager.instance.SearchRoomStockToDiamonds(BuildRoomName.GoldSpace, item.Value);
                    break;
                case MaterialName.Mana:
                    txt_Mana.transform.parent.gameObject.SetActive(true);
                    txt_Mana.text = item.Value.ToString();
                    temp += BuildingManager.instance.SearchRoomStockToDiamonds(BuildRoomName.ManaSpace, item.Value);
                    break;
                case MaterialName.Wood:
                    txt_Wood.transform.parent.gameObject.SetActive(true);
                    txt_Wood.text = item.Value.ToString();
                    temp += BuildingManager.instance.SearchRoomStockToDiamonds(BuildRoomName.WoodSpace, item.Value);
                    break;
                case MaterialName.Iron:
                    txt_Iron.transform.parent.gameObject.SetActive(true);
                    txt_Iron.text = item.Value.ToString();
                    temp += BuildingManager.instance.SearchRoomStockToDiamonds(BuildRoomName.IronSpace, item.Value);
                    break;
                default:
                    break;
            }
        }

        if (needStock.ContainsKey(MaterialName.Diamonds))
        {
            Debug.LogError("服务器和本地材料数据不同步");
            if (temp != needStock[MaterialName.Diamonds])
            {
                Debug.LogError("服务器和本地钻石数据不同步");
            }
            temp = needStock[MaterialName.Diamonds];
        }

        PlayerData player = GetPlayerData.Instance.GetData();
        needDiaNumber = (int)(player.Diamonds - temp);
        if (needDiaNumber > 0)
        {
            txt_DiaNum.text = temp.ToString();
        }
        else
        {
            txt_DiaNum.text = "<color=#ee5151>" + temp.ToString() + "</color>";
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
        if (needDiaNumber < 0)
        {
            isStockFull = false;
            needDiaNumber = -needDiaNumber;
            UIPanelManager.instance.ShowPage<UIPopUP_5>(needDiaNumber);
        }
        else
        {
            //向服务器发送 消耗钻石购买建材
            Debug.Log("向服务器发送消耗钻石购买材料");
            //PlayerData player = GetPlayerData.Instance.GetData();
            //player.Diamonds -= needDiaNumber;
            //Dictionary<BuildRoomName, int> dic = new Dictionary<BuildRoomName, int>();
            //foreach (var item in needStock)
            //{
            //    BuildRoomName name = BuildRoomName.Nothing;
            //    if (item.Value > 0)
            //    {
            //        switch (item.Key)
            //        {
            //            case MaterialName.Gold:
            //                name = BuildRoomName.GoldSpace;
            //                dic.Add(name, item.Value);
            //                break;
            //            case MaterialName.Mana:
            //                name = BuildRoomName.ManaSpace;
            //                dic.Add(name, item.Value);
            //                break;
            //            case MaterialName.Wood:
            //                name = BuildRoomName.WoodSpace;
            //                dic.Add(name, item.Value);
            //                break;
            //            case MaterialName.Iron:
            //                name = BuildRoomName.IronSpace;
            //                dic.Add(name, item.Value);
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            //foreach (var item in dic)
            //{
            //    CheckPlayerInfo.instance.AddStock(item.Key, item.Value);
            //}
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
