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
    public Image icon;
    public Image slider;

    public Button btn_back;
    public Button btn_back_1;

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
    }

    private void Start()
    {
        string st = "           ";
        txt_Tip_1.text = "升级增加";
        txt_Tip_2.text = "容量";
        txt_Tip_3.text = string.Format("升级该房间来增加{0}资源容量", st);
    }
    private void UpdateInfo(RoomMgr data)
    {
        txt_Name.text = LanguageDataMgr.instance.GetRoomName(data.BuildingData.RoomName.ToString());
        txt_Level.text = data.BuildingData.Level.ToString();
        txt_Stock.text = data.currentBuildData.Stock.ToString() + "/" + data.currentBuildData.buildingData.Param2;
        slider.fillAmount = data.currentBuildData.Stock / data.currentBuildData.buildingData.Param2;

        Sprite sp = null;
        switch (data.RoomName)
        {
            case BuildRoomName.GoldSpace:
                stockIcon.sprite = iconSp[0];
                sp = GetSpriteAtlas.insatnce.GetIcon("Gold");
                break;
            case BuildRoomName.FoodSpace:
                stockIcon.sprite = iconSp[1];
                sp = GetSpriteAtlas.insatnce.GetIcon("Food");
                break;
            case BuildRoomName.ManaSpace:
                stockIcon.sprite = iconSp[2];
                sp = GetSpriteAtlas.insatnce.GetIcon("Mana");
                break;
            case BuildRoomName.WoodSpace:
                stockIcon.sprite = iconSp[3];
                sp = GetSpriteAtlas.insatnce.GetIcon("Wood");
                break;
            case BuildRoomName.IronSpace:
                stockIcon.sprite = iconSp[4];
                sp = GetSpriteAtlas.insatnce.GetIcon("Iron");
                break;
            default:
                break;
        }
        icon.sprite = sp;
    }
    private void ChickBack()
    {
        UIPanelManager.instance.ClosePage<UIStockInfo>();
    }
}
