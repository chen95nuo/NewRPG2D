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

    public Image rightIcon;
    public Image[] icons;
    public Image slider;
    public Sprite[] iconSp;

    public Button btn_back;
    public Button btn_back_1;
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
        btn_back_1.onClick.AddListener(ChickBack);
        HallEventManager.instance.AddListener(HallEventDefineEnum.ChickStock, RefreshStock);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ChickStock, RefreshStock);
    }
    private void Start()
    {
        string space = "                ";
        txt_Tip_1.text = "每小时产量";
        txt_Tip_2.text = "容量";
        txt_Tip_3.text = string.Format("该建筑生产{0}资源", space);
    }

    private void RefreshStock()
    {
        slider.fillAmount = roomData.currentBuildData.Stock / roomData.BuildingData.Param2;
        txt_Stock.text = roomData.currentBuildData.Stock.ToString("#0") + "/" + roomData.BuildingData.Param2.ToString("#0");
    }

    private void UpdateInfo(RoomMgr data)
    {
        roomData = data;
        txt_Name.text = data.RoomName.ToString();
        txt_Level.text = data.BuildingData.Level.ToString();
        txt_Yield.text = (data.currentBuildData.buildingData.Param1 + data.currentBuildData.AllRoleProduction()).ToString();
        txt_Stock.text = data.currentBuildData.Stock.ToString("#0") + "/" + data.BuildingData.Param2.ToString("#0");
        ChickRoleNumber();
        ChickPlayerInfo.instance.GetRoomEvent(data.currentBuildData);

        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(data.RoomName.ToString());
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].sprite = sp;
        }

        switch (data.RoomName)
        {
            case BuildRoomName.Gold:
                rightIcon.sprite = iconSp[0];
                break;
            case BuildRoomName.Food:
                rightIcon.sprite = iconSp[1];
                break;
            case BuildRoomName.Mana:
                rightIcon.sprite = iconSp[2];
                break;
            case BuildRoomName.Wood:
                rightIcon.sprite = iconSp[3];
                break;
            case BuildRoomName.Iron:
                rightIcon.sprite = iconSp[4];
                break;
            default:
                break;
        }
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
        for (int i = 0; i < roomData.currentBuildData.roleData.Length; i++)
        {
            if (roomData.currentBuildData.roleData[i] != null)
            {
                roleGrids[i].UpdateInfo(roomData.currentBuildData.roleData[i], roomData.RoomName);
            }
            else
            {
                roleGrids[i].UpdateInfo();
            }
        }
    }

    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIProductionInfo>();
        ChickPlayerInfo.instance.RemoveRoomEvent();
    }
}
