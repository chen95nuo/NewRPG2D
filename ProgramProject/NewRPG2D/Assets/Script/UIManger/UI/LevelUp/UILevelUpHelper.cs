using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;


public class UILevelUpHelper : MonoBehaviour
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;
    public Text txt_Diamonds;
    public Button btn_NowUp;
    public Button btn_LevelUp;

    private float needGold = 0;
    private float needMana = 0;
    private float needWood = 0;
    private float needIron = 0;

    private RoomMgr roomMgr = null;

    public void Init()
    {
        txt_Tip_1.text = "立即升级";
        txt_Tip_2.text = "升级";

        btn_NowUp.onClick.AddListener(ChickNowUp);
        btn_LevelUp.onClick.AddListener(ChickLevelUp);
        CloseTxt(false);
    }

    public void UpdateUIfo(RoomMgr roomMgr)
    {
        this.roomMgr = roomMgr;
        PlayerData playerData = GetPlayerData.Instance.GetData();

        BuildingData b_Data_1 = roomMgr.BuildingData;//当前房间信息
        BuildingData b_Data_2;//下一级房间信息
        if (b_Data_1.NextLevelID == 0)
        {
            btn_NowUp.interactable = false;
            btn_LevelUp.interactable = false;
            return;
        }
        b_Data_2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(b_Data_1.NextLevelID);

        if (b_Data_2.NeedGold > 0)
        {
            txt_Gold.gameObject.SetActive(true);
            txt_Gold.text = b_Data_2.NeedGold.ToString();
            needGold = b_Data_2.NeedGold - (ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Gold));
        }
        if (b_Data_2.NeedMana > 0)
        {
            txt_Mana.gameObject.SetActive(true);
            txt_Mana.text = b_Data_2.NeedMana.ToString();
            needMana = b_Data_2.NeedMana - (ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Mana));
        }
        if (b_Data_2.NeedWood > 0)
        {
            txt_Wood.gameObject.SetActive(true);
            txt_Wood.text = b_Data_2.NeedWood.ToString();
            needWood = b_Data_2.NeedWood - (ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Wood));
        }
        if (b_Data_2.NeedIron > 0)
        {
            txt_Iron.gameObject.SetActive(true);
            txt_Iron.text = b_Data_2.NeedIron.ToString();
            needIron = b_Data_2.NeedIron - (ChickPlayerInfo.instance.GetAllStock(BuildRoomName.Iron));
        }

        txt_Tip_3.text = SystemTime.instance.TimeNormalized(b_Data_2.NeedTime);
    }
    private void ChickNowUp()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        //如果钻石够直接扣钻石 如果钻石不够打开购买钻石界面
        if (data.Diamonds > 100)
        {
            data.Diamonds -= 100;
            //直接升级跳过时间
        }
        else
        {
            Debug.Log("钻石不足");
            //跳转到购买钻石界面
        }
    }

    /// <summary>
    /// 检查升级按钮
    /// </summary>
    private void ChickLevelUp()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        //如果材料足够进入升级 如果材料不够 提示是用钻石购买材料
        //需要各个材料的值，若不足需要知道还缺多少
        if (needGold <= 0 && needMana <= 0 && needWood <= 0 && needIron <= 0)
        {
            //材料足够倒计时升级
            int id = roomMgr.BuildingData.NextLevelID;
            BuildingData buildData = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(id);
            roomMgr.ConstructionStart(buildData);
            return;
        }
        else
        {
            Dictionary<MaterialName, int> needStock = new Dictionary<MaterialName, int>();
            if (needGold > 0)
            {
                needStock[MaterialName.Gold] = (int)needGold;
            }
            if (needMana > 0)
            {
                needStock[MaterialName.Mana] = (int)needMana;
            }
            if (needWood > 0)
            {
                needStock[MaterialName.Wood] = (int)needWood;
            }
            if (needIron > 0)
            {
                needStock[MaterialName.Iron] = (int)needIron;
            }
            UIPanelManager.instance.ShowPage<UIPopUp_1>();
            UIPopUp_1.instance.UpdateInfo(needStock);
        }
    }

    private void CloseTxt(bool isTrue)
    {
        txt_Gold.gameObject.SetActive(isTrue);
        txt_Mana.gameObject.SetActive(isTrue);
        txt_Wood.gameObject.SetActive(isTrue);
        txt_Iron.gameObject.SetActive(isTrue);
    }
}

