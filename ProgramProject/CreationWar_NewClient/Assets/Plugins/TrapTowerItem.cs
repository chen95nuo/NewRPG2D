/// <summary>
/// Copyright (c) 2014-2015 Zealm All rights reserved
/// Author: David Sheh
/// </summary>

using UnityEngine;
using System.Collections;

public class TrapTowerItem : MonoBehaviour 
{
    public GameObject bagItem;
    public UILabel itemName;
    public UISprite background;

    void Awake()
    {
        //this.gameObject.SetActive(false);
    }

    public void SetItemInfo(string itemID)
    {
        string[] itemInfo = itemID.Split(',');
        if (itemInfo.Length < 2)
        {
            //itemID = string.Format("{0},01", itemInfo[0]);
            itemID = itemInfo[0];
        }
        else if (itemInfo[1].Length < 2)
        {
            itemID = string.Format("{0},0{1}", itemInfo[0], itemInfo[1]);
        }
        if(!bagItem.gameObject.activeSelf)
        {
            bagItem.gameObject.SetActive(true);
        }
        bagItem.SendMessage("GetInvAsID", itemID , SendMessageOptions.DontRequireReceiver);
        object[] parms = new object[2];
        parms[0] = itemID;
        parms[1] = itemName;// 物品名称Label
        PanelStatic.StaticBtnGameManager.InvMake.SendMessage("ResolveInventory", parms, SendMessageOptions.DontRequireReceiver);
    }

    public void DisableItem()
    {
        itemName.color = Color.gray;
        background.spriteName = "UIB_Union_Player_O";
    }

    public void EnableItem()
    {
        itemName.color = Color.white;
        background.spriteName = "UIH_Usually_Button";
    }
}
