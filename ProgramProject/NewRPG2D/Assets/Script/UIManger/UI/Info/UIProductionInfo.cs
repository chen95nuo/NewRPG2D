using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIProductionInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;

    public Text txt_Yield;
    public Text txt_Stock;

    public Image rightIcon;
    public Image icons;
    public Image slider;
    public Sprite[] iconSp;

    public List<UIRoleGrid> roleGrids;

    protected override void Awake()
    {
        base.Awake();
        HallEventManager.instance.AddListener(HallEventDefineEnum.CheckStock, RefreshStock);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.CheckStock, RefreshStock);
    }
    private void Start()
    {
        txt_Tip_1.text = "每小时产量";
        txt_Tip_2.text = "容量";
    }

    private void RefreshStock()
    {
        slider.fillAmount = roomData.Stock / roomData.buildingData.Param2;
        txt_Stock.text = roomData.Stock.ToString("#0") + "/" + roomData.buildingData.Param2.ToString("#0");
    }

    protected override void UpdateInfo(LocalBuildingData data)
    {
        txt_Yield.text = (data.buildingData.Param1 + data.AllRoleProduction()).ToString();
        txt_Stock.text = data.Stock.ToString("#0") + "/" + data.buildingData.Param2.ToString("#0");
        ChickRoleNumber(roleGrids);

        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(data.buildingData.RoomName.ToString());
        icons.sprite = sp;
        rightIcon.sprite = GetSpriteAtlas.insatnce.GetIcon(data.buildingData.RoomName + "Stock");
        RefreshStock();
    }

    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIProductionInfo>();
        CheckPlayerInfo.instance.RemoveRoomEvent();
    }

    protected override void UpdateName(LocalBuildingData data, bool NeedTip = true)
    {
        base.UpdateName(data, false);
        string space = string.Format("<quad name={0} size=36 width=1 />", data.buildingData.RoomName);
        txt_DownTip.text = string.Format("该建筑生产{0}资源", space);
    }
}
