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
        txt_Name.text = data.buildingData.RoomName.ToString();
        txt_Level.text = data.buildingData.Level.ToString();
        IStorage Iprod = data.GetComponent<IStorage>();
        txt_Stock.text = Iprod.Stock.ToString() + "/" + data.buildingData.Param2;
    }
    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIStockInfo>();
    }
}
