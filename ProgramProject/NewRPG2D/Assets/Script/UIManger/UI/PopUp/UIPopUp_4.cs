using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;
using proto.SLGV1;

public class UIPopUp_4 : TTUIPage
{
    public Text txt_Tip;
    public Text txt_Message;
    public Button btn_Enter;
    public Button btn_Cancel;
    public Text txt_Enter;
    public Text txt_Cancel;

    private BuildRoomName name;

    private void Awake()
    {
        btn_Enter.onClick.AddListener(ShowBuildStock);
        btn_Cancel.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        ConsumeItem data = mData as ConsumeItem;
        UpdateInfo(data);
    }

    /// <summary>
    /// 房间库存不足 点击确认转向房间
    /// </summary>
    /// <param name="data"></param>
    public void UpdateInfo(ConsumeItem data)
    {
        name = BuildRoomName.Nothing;
        switch ((MaterialName)data.produceType)
        {
            case MaterialName.Gold:
                name = BuildRoomName.GoldSpace;
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
        LocalBuildingData buildData = BuildingManager.instance.SearchRoomData(name, 1);
        if (BuildingDataMgr.instance.GetbuildingData(name) == null)
        {
            txt_Message.text = "仓库空间不足缺少" + buildData.buildingData.RoomName;
            txt_Enter.text = LanguageDataMgr.instance.GetUIString("xinjian");
        }
        else
        {
            txt_Message.text = "仓库等级不够升级" + buildData.buildingData.RoomName;
            txt_Enter.text = LanguageDataMgr.instance.GetUIString("shengji");
        }



        UIMain.instance.CloseMarket();
    }

    /// <summary>
    /// 转向建造该仓库
    /// </summary>
    private void ShowBuildStock()
    {
        //仓库容量不足判断是否有仓库
        //有仓库打开仓库升级
        LocalBuildingData buildData = BuildingManager.instance.SearchRoomData(name, 1);
        if (buildData == null)
        {
            UIMain.instance.ShowMarket();
        }
        else
        {
            UIPanelManager.instance.ShowPage<UIProdLevelUp>(buildData);
        }
        //没有仓库跳转到购买仓库
        ClosePage();
    }
}
