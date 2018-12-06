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

    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    public Image stockIcon;
    public Sprite[] iconSp;
    public Image slider;

    public Button btn_back;
    public Button btn_back_1;

    private LocalBuildingData roomData;
    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as LocalBuildingData;
        UpdateInfo(roomData);
    }
    private void Awake()
    {
        btn_back.onClick.AddListener(ChickBack);
        btn_back_1.onClick.AddListener(ChickBack);
    }

    private void Start()
    {
        txt_Tip_1.text = LanguageDataMgr.instance.GetUIString("shengjizengjia");
        txt_Tip_2.text = LanguageDataMgr.instance.GetUIString("rongliang");
    }
    private void UpdateInfo(LocalBuildingData data)
    {
        txt_Name.text = LanguageDataMgr.instance.GetRoomName(data.buildingData.RoomName.ToString());
        txt_Level.text = data.buildingData.Level.ToString();
        int stock = GetPlayerData.Instance.GetStock(data.buildingData.RoomName);
        stock = (stock - data.buildingData.Param2) >= 0 ? (int)data.buildingData.Param2 : (int)(data.buildingData.Param2 - (data.buildingData.Param2 - stock));
        txt_Stock.text = stock + "/" + data.buildingData.Param2;
        slider.fillAmount = data.Stock / data.buildingData.Param2;
        string st = LanguageDataMgr.instance.GetUIString("shengjigaifangjian");
        string iconName = "";
        Sprite sp = null;
        switch (data.buildingData.RoomName)
        {
            case BuildRoomName.GoldSpace:
                iconName = "Gold";
                break;
            case BuildRoomName.FoodSpace:
                iconName = "Food";
                break;
            case BuildRoomName.ManaSpace:
                iconName = "Mana";
                break;
            case BuildRoomName.WoodSpace:
                iconName = "Wood";
                break;
            case BuildRoomName.IronSpace:
                iconName = "Iron";
                break;
            default:
                break;
        }
        iconName = string.Format("<quad name={0} size=36 width=1 />", iconName);
        txt_Tip_3.text = string.Format(st, iconName);
    }
    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIStockInfo>();
    }
}
