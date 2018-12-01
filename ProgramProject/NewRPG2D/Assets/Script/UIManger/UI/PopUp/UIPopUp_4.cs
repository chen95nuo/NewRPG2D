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

    private void Awake()
    {
        btn_Enter.onClick.AddListener(ShowBuildStock);
        btn_Cancel.onClick.AddListener(ClosePage);
        txt_Enter.text = LanguageDataMgr.instance.GetString("Enter");
        txt_Cancel.text = LanguageDataMgr.instance.GetString("Cancel");
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
        BuildRoomName name = BuildRoomName.Nothing;
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
        BuildingData buildData = BuildingDataMgr.instance.GetbuildingData(name);
        txt_Message.text = "仓库不足缺少" + buildData.RoomName;
    }

    /// <summary>
    /// 转向建造该仓库
    /// </summary>
    private void ShowBuildStock()
    {

    }
}
