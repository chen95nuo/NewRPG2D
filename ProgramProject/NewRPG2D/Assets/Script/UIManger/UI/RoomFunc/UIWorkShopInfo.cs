using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;

public class UIWorkShopInfo : TTUIPage
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_Tip_5;
    public Text txt_Tip_6;

    public Text txt_Name;
    public Text txt_Name_2;
    public Text txt_Quality;
    public Text txt_NeedMana;
    public Text txt_NeedPropNum;
    public Text txt_NeedTime;
    public Text txt_Diamonds;

    public Text txt_HaveMana;
    public Text[] txt_HaveProp;

    public Image propIcon;
    public Image TypeIcon;

    public Button btn_Next;//下一个
    public Button btn_Previous;//上一个
    public Button btn_Back;//后退
    public Button btn_Close;//退出
    public Button btn_Start;//启动

    private WorkShopInfoData currentData;
    private WorkShopData[] currentShopData;
    private int currentIndex = 0;

    private string whiteText = LanguageDataMgr.instance.whiteText;
    private string redText = LanguageDataMgr.instance.redSt;

    private void Awake()
    {
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickWorkTime, UpdateTime);
        txt_Tip_1.text = "所需资源";
        txt_Tip_2.text = "所需碎片";
        txt_Tip_3.text = "物品品质选择";
        txt_Tip_4.text = "制作";
        txt_Tip_5.text = "加速";
        txt_Tip_6.text = "制作时间:";

        btn_Next.onClick.AddListener(ChickNext);
        btn_Previous.onClick.AddListener(ChickPrevious);
        btn_Back.onClick.AddListener(ChickBack);
        btn_Close.onClick.AddListener(ChickClose);
        btn_Start.onClick.AddListener(ChickStart);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickWorkTime, UpdateTime);
    }

    private void ChickStart()
    {
        WorkShopDataMgr.instance.AddWork(currentData.data.ItemId, currentShopData[currentIndex]);
    }

    private void ChickClose()
    {
        UIPanelManager.instance.ClosePage<UIWorkShop>();
        ClosePage();
    }

    private void ChickBack()
    {
        ClosePage();
    }

    private void ChickPrevious()
    {
        UpdateInfo(currentIndex--);
    }

    private void ChickNext()
    {
        UpdateInfo(currentIndex--);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        WorkShopInfoData data = mData as WorkShopInfoData;
        UpdateInfo(data);
    }

    public void UpdateInfo(WorkShopInfoData data)
    {
        currentShopData = WorkShopDataMgr.instance.GetWorkData(currentData.data.ItemId, currentData.index);
        string nameSt = LanguageDataMgr.instance.GetString(currentData.data.RoomName.ToString() + currentData.index);
        txt_Name.text = nameSt + "(";
        if (data == currentData)
        {
            currentIndex = 0;
        }
        UpdateInfo(currentIndex);
    }

    public void UpdateInfo(int index)
    {
        txt_NeedMana.text = currentShopData[index].NeedMana.ToString();
        txt_NeedPropNum.text = currentShopData[index].NeedPropNum.ToString();
        txt_Name_2.text = currentShopData[index].Level[0] + currentShopData[0].Level[0] + ")";
        txt_Quality.text = LanguageDataMgr.instance.GetString("WorkShop_" + currentShopData[index].Quality.ToString());
        txt_NeedTime.text = SystemTime.instance.TimeNormalizedOf(currentShopData[index].NeedTime);
        int haveMana = ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Mana);
        if (haveMana > currentShopData[index].NeedMana)
        {
            txt_HaveMana.text = string.Format(whiteText, haveMana);
        }
        else
        {
            txt_HaveMana.text = string.Format(redText, haveMana);
        }
        int haveProp = UpdateHaveProp(currentShopData[index].NeedPropId);
        if (haveProp > currentShopData[index].NeedPropNum)
        {
            txt_NeedPropNum.text = string.Format(whiteText, haveProp);
        }
        else
        {
            txt_NeedPropNum.text = string.Format(redText, haveProp);
        }
    }

    public int UpdateHaveProp(int id = 0)
    {
        int index = 1006;//第一个道具的ID
        int haveProp = 0;
        for (int i = 0; i < txt_HaveProp.Length; i++)
        {
            int temp = ChickItemInfo.instance.GetItemNum(index + i);
            if (index + i == id)
            {
                haveProp = id;
            }
            txt_HaveProp[i].text = temp.ToString();
        }
        return haveProp;
    }

    public void UpdateTime(int roomId)
    {
        if (roomId == currentData.data.ItemId)
        {
            WorkShopHelper data = WorkShopDataMgr.instance.GetWorkTime(roomId);
            txt_NeedTime.text = SystemTime.instance.TimeNormalizedOf(data.time);
        }
    }
}
