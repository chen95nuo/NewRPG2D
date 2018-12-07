using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Net;


public class UILevelUpHelper : MonoBehaviour
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text[] txt_material;
    public Text txt_Diamonds;
    public Button btn_NowUp;
    public Button btn_LevelUp;

    private float[] needMaterial;

    private LocalBuildingData currentRoomData;

    private Dictionary<MaterialName, int> needStock = new Dictionary<MaterialName, int>();

    private int allNeed = 0;
    private void Awake()
    {
        txt_Tip_1.text = LanguageDataMgr.instance.GetUIString("lijishengji");
        txt_Tip_2.text = LanguageDataMgr.instance.GetUIString("shengji");

        btn_NowUp.onClick.AddListener(ChickNowUp);
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
    }
    public void Init()
    {
        CloseTxt(false);
    }

    public void UpdateUIfo(LocalBuildingData roomData)
    {
        this.currentRoomData = roomData;
        PlayerData playerData = GetPlayerData.Instance.GetData();

        BuildingData data = roomData.buildingData;//当前房间信息
        if (data.NextLevelID == 0)
        {
            btn_NowUp.interactable = false;
            btn_LevelUp.interactable = false;
            txt_Tip_3.text = LanguageDataMgr.instance.GetUIString("manji");
            txt_Diamonds.text = "";
            return;
        }

        for (int i = 0; i < data.needMaterial.Length; i++)
        {
            if (data.needMaterial[i] != 0)
            {
                txt_material[i].transform.parent.gameObject.SetActive(true);
            }
        }
        needStock.Clear();
        needStock = BuildingManager.instance.RoomNeedMaterialHelper(data, txt_material);

        txt_Tip_3.text = SystemTime.instance.TimeNormalizedOf(data.NeedTime);
        int timeToDia = BuildingManager.instance.TimeToDiamonds(data);
        int diamonds = BuildingManager.instance.RoomNeedDiamondsHelper(data) + timeToDia;
        txt_Diamonds.text = diamonds.ToString();

        if (roomData.buildingData.NeedLevel > data.NeedLevel)
        {
            btn_NowUp.interactable = false;
            btn_LevelUp.interactable = false;
        }
        else if (!BuildingManager.instance.RoomNeedSpaceHelper(data))
        {
            btn_LevelUp.interactable = false;
        }
    }
    private void ChickNowUp()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        //如果钻石够直接扣钻石 如果钻石不够打开购买钻石界面
        if (data.Diamonds >= allNeed)
        {
            data.Diamonds -= allNeed;
            //直接升级跳过时间
            string RoomID = currentRoomData.buildingData.RoomName + "_" + currentRoomData.id;
            WebSocketManger.instance.Send(NetSendMsg.RQ_RoomUpdateLevel, RoomID, 3);
        }
        else
        {
            //跳转到购买钻石界面
            object st = LanguageDataMgr.instance.GetUIString("zuanshibuzu");
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
        }
    }

    /// <summary>
    /// 检查升级按钮
    /// </summary>
    private void ChickLevelUp()
    {
        //如果材料足够进入升级 如果材料不够 提示是用钻石购买材料
        //需要各个材料的值，若不足需要知道还缺多少
        if (needStock[MaterialName.Diamonds] > 0)
        {
            //材料不足需要消耗钻石
            UIPanelManager.instance.ShowPage<UIPopUp_1>(needStock);
        }
        //材料足够倒计时升级
        Debug.Log("材料足够 开始升级");
        string RoomID = currentRoomData.buildingData.RoomName + "_" + currentRoomData.id;
        WebSocketManger.instance.Send(NetSendMsg.RQ_RoomUpdateLevel, RoomID, 1);
    }

    private void CloseTxt(bool isTrue)
    {
        for (int i = 0; i < txt_material.Length; i++)
        {
            txt_material[i].transform.parent.gameObject.SetActive(isTrue);
        }
    }
}

