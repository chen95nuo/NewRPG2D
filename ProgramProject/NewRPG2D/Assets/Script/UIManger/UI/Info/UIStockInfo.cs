using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIStockInfo : TTUIPage
{

    public Text txt_Name;
    public Text txt_Level;
    public Text txt_Stock;

    public Text txt_Tip;

    public Button btn_back;
    private RoomMgr roomData;
    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
        UpdateInfo(roomData);
    }
    private void Awake()
    {
        btn_back.onClick.AddListener(ChickBack);
    }

    private void Start()
    {
        txt_Tip.text = "该建筑储存资源";
    }
    private void UpdateInfo(RoomMgr data)
    {
        txt_Name.text = data.BuildingData.RoomName.ToString();
        txt_Level.text = data.BuildingData.Level.ToString();
        txt_Stock.text = data.currentBuildData.Stock.ToString() + "/" + data.currentBuildData.buildingData.Param2;
    }
    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIStockInfo>();
    }
}
