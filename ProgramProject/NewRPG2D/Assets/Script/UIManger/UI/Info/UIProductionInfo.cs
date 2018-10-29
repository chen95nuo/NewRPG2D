using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIProductionInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    public Text txt_Yield;
    public Text txt_Stock;

    public Image rightIcon;
    public Image[] icons;
    public Image slider;
    public Sprite[] iconSp;

    public List<UIRoleGrid> roleGrids;

    protected override void Awake()
    {
        base.Awake();
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

    protected override void UpdateInfo(RoomMgr data)
    {
        txt_Yield.text = (data.currentBuildData.buildingData.Param1 + data.currentBuildData.AllRoleProduction()).ToString();
        txt_Stock.text = data.currentBuildData.Stock.ToString("#0") + "/" + data.BuildingData.Param2.ToString("#0");
        ChickRoleNumber(roleGrids);
        for (int i = 0; i < roleGrids.Count; i++)
        {
            if (data.currentBuildData.roleData[i] != null)
            {
                roleGrids[i].UpdateInfo(roomData.currentBuildData.roleData[i], roomData.RoomName, this, i);
            }
            else
            {
                roleGrids[i].UpdateInfo(this, i);
            }
        }

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

    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIProductionInfo>();
        ChickPlayerInfo.instance.RemoveRoomEvent();
    }


}
