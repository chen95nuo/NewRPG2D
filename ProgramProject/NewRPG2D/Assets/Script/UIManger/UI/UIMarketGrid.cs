﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using proto.SLGV1;

public class UIMarketGrid : MonoBehaviour
{

    public BuildingData buildingData;
    public Text buildName;

    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;

    public Text txt_Gold;
    public Text txt_Mana;
    public Text txt_Wood;
    public Text txt_Iron;

    public Image RoomIcon_1;
    public Image RoomIcon_2;
    public Image RoomIcon_3;
    public Image RoomIcon_4;

    public Text txt_Number;
    public Text txt_NeedTime;
    private Button thisGrid;

    public GameObject lockObj;
    public Image[] lockImages;
    public Material black;

    //public Material black;
    private Dictionary<MaterialName, int> needStock = new Dictionary<MaterialName, int>();
    private void Awake()
    {
        thisGrid = GetComponent<Button>();
        thisGrid.onClick.AddListener(AddBuilding);
    }

    public void UpdateBuilding(BuildingData data, int[] number, bool isTrue)
    {
        CheckProduce(false);

        txt_Tip_4.text = LanguageDataMgr.instance.GetDes(data.RoomName.ToString());
        buildName.text = LanguageDataMgr.instance.GetRoomName(data.RoomName.ToString());
        thisGrid.interactable = isTrue;

        Sprite sp = GetSpriteAtlas.insatnce.GetRoomSp(data.RoomName.ToString());
        switch (data.RoomSize)
        {
            case 1:
                RoomIcon_1.gameObject.SetActive(true);
                RoomIcon_1.sprite = sp;
                break;
            case 3:
                RoomIcon_2.gameObject.SetActive(true);
                RoomIcon_2.sprite = sp;
                break;
            case 6:
                RoomIcon_3.gameObject.SetActive(true);
                RoomIcon_3.sprite = sp;
                break;
            case 9:
                RoomIcon_4.gameObject.SetActive(true);
                RoomIcon_4.sprite = sp;
                break;
            default:
                break;
        }

        if (isTrue == false)
        {
            if (data.NeedLevel == 0 || (number[0] == number[1] && number[1] != 0)) txt_Tip_3.text = LanguageDataMgr.instance.GetUIString("jianzhuyidashangxian");
            else
            {
                string st = LanguageDataMgr.instance.GetUIString("xuyaoguowangdating");
                txt_Tip_3.text = string.Format(st, data.NeedLevel);
            }
            lockObj.SetActive(true);
            for (int i = 0; i < lockImages.Length; i++)
            {
                lockImages[i].material = black;
            }
            return;
        }

        txt_Tip_1.text = LanguageDataMgr.instance.GetUIString("jianzaoshijian");
        txt_Tip_2.text = LanguageDataMgr.instance.GetUIString("jianzhushuliang");

        buildingData = data;
        txt_Number.text = number[0] + "/" + number[1];
        txt_NeedTime.text = SystemTime.instance.TimeNormalizedOf(data.NeedTime);
        string redSt = "<color=#ee5151>{0}</color>";
        string whiteSt = "<color=#ffffff>{0}</color>";
        needStock.Clear();
        if (data.NeedGold > 0)
        {
            txt_Gold.gameObject.SetActive(true);
            int stock = BuildingManager.instance.SearchRoomStock(BuildRoomName.GoldSpace);
            if (stock < data.NeedGold)
            {
                txt_Gold.text = string.Format(redSt, data.NeedGold);
                int need = data.NeedGold - stock;

                needStock.Add(MaterialName.Gold, need);
            }
            else
            {
                txt_Gold.text = string.Format(whiteSt, data.NeedGold);
            }
        }
        if (data.NeedMana > 0)
        {
            txt_Mana.gameObject.SetActive(true);
            int stock = BuildingManager.instance.SearchRoomStock(BuildRoomName.ManaSpace);
            if (stock < data.NeedMana)
            {
                txt_Mana.text = string.Format(redSt, data.NeedMana);
                int need = data.NeedMana - stock;
                needStock.Add(MaterialName.Mana, need);
            }
            else
            {
                txt_Mana.text = string.Format(whiteSt, data.NeedMana);
            }
        }
        if (data.NeedWood > 0)
        {
            txt_Wood.gameObject.SetActive(true);
            txt_Wood.text = data.NeedWood.ToString();
            int stock = BuildingManager.instance.SearchRoomStock(BuildRoomName.WoodSpace);
            if (stock < data.NeedWood)
            {
                txt_Wood.text = string.Format(redSt, data.NeedWood);
                int need = data.NeedWood - stock;
                needStock.Add(MaterialName.Wood, need);
            }
            else
            {
                txt_Wood.text = string.Format(whiteSt, data.NeedWood);
            }
        }
        if (data.NeedIron > 0)
        {
            txt_Iron.gameObject.SetActive(true);
            txt_Iron.text = data.NeedIron.ToString();
            int stock = BuildingManager.instance.SearchRoomStock(BuildRoomName.IronSpace);
            if (stock < data.NeedGold)
            {
                txt_Iron.text = string.Format(redSt, data.NeedIron);
                int need = data.NeedIron - stock;
                needStock.Add(MaterialName.Iron, need);
            }
            else
            {
                txt_Iron.text = string.Format(whiteSt, data.NeedIron);
            }
        }

    }

    private void AddBuilding()
    {
        Debug.Log(buildingData.RoomName);
        //检测资源是否足够
        if (needStock.Count > 0)
        {
            BuildRoomName name = BuildRoomName.Nothing;
            foreach (var need in needStock)
            {
                switch (need.Key)
                {
                    case MaterialName.Gold:
                        name = BuildRoomName.GoldSpace;
                        break;
                    case MaterialName.Mana:
                        name = BuildRoomName.ManaSpace;
                        break;
                    case MaterialName.Wood:
                        name = BuildRoomName.WoodSpace;
                        break;
                    case MaterialName.Iron:
                        name = BuildRoomName.IronSpace;
                        break;
                    default:
                        break;
                }
                int empty = BuildingManager.instance.SearchRoomEmpty(name);
                if (empty < need.Value)
                {
                    ConsumeItem consume = new ConsumeItem();
                    consume.produceType = (int)need.Key;
                    UIPanelManager.instance.ShowPage<UIPopUp_4>(consume);
                    return;
                }
            }
            UIPanelManager.instance.ShowPage<UIPopUp_1>(needStock);
            return;
        }
        UIMain.instance.RSCheckCreateNewRoom(this.buildingData);
    }

    private void CheckProduce(bool isTrue)
    {
        txt_Gold.gameObject.SetActive(false);
        txt_Mana.gameObject.SetActive(false);
        txt_Wood.gameObject.SetActive(false);
        txt_Iron.gameObject.SetActive(false);

        RoomIcon_1.gameObject.SetActive(false);
        RoomIcon_2.gameObject.SetActive(false);
        RoomIcon_3.gameObject.SetActive(false);
        RoomIcon_4.gameObject.SetActive(false);

        txt_Tip_1.text = "";
        txt_Tip_2.text = "";
        txt_Tip_3.text = "";
        txt_NeedTime.text = "";
        txt_Number.text = "";

        lockObj.SetActive(false);

        for (int i = 0; i < lockImages.Length; i++)
        {
            lockImages[i].material = null;
        }
    }
}
