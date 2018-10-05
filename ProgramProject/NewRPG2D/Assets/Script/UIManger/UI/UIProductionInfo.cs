using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIProductionInfo : TTUIPage
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    public Text txt_Name;
    public Text txt_Level;
    public Text txt_Yield;
    public Text txt_Stock;

    public Button btn_back;
    public Transform roleTrans;
    public GameObject roleGrid;
    public List<UIRoleGrid> roleGrids;
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
        HallEventManager.instance.AddListener(HallEventDefineEnum.ChickStock, RefreshStock);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ChickStock, RefreshStock);
    }
    private void Start()
    {
        txt_Tip_1.text = "每小时产量";
        txt_Tip_2.text = "容量";
        txt_Tip_3.text = "该建筑生产资源";
    }

    private void RefreshStock()
    {
        txt_Stock.text = roomData.currentBuildData.Stock.ToString("#0") + "/" + roomData.BuildingData.Param2.ToString("#0");
    }

    private void UpdateInfo(RoomMgr data)
    {
        roomData = data;
        txt_Name.text = data.RoomName.ToString();
        txt_Level.text = data.BuildingData.Level.ToString();
        txt_Yield.text = data.currentBuildData.buildingData.Param1.ToString();
        txt_Stock.text = data.currentBuildData.Stock.ToString("#0") + "/" + data.BuildingData.Param2.ToString("#0");
        ChickRoleNumber();
        ChickPlayerInfo.instance.GetRoomEvent(data.currentBuildData);
    }
    private void ChickRoleNumber()
    {
        int index = roomData.BuildingData.RoomRole - roleGrids.Count;
        if (index > 0) //证明已有角色卡数量不足
        {
            for (int i = 0; i < index; i++)
            {
                GameObject go = Instantiate(roleGrid, roleTrans) as GameObject;
                UIRoleGrid grid = go.GetComponent<UIRoleGrid>();
                roleGrids.Add(grid);
            }
        }
    }

    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIProductionInfo>();
        ChickPlayerInfo.instance.RemoveRoomEvent();
    }
}
