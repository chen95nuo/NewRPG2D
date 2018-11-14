using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIWorkShopGrid : MonoBehaviour
{

    public Image Icon;
    public Image Lock;
    public Text txt_Tip;

    public Button btn_Click;
    private EquipmentRealProperty currentEquip;

    private void Awake()
    {
        btn_Click.onClick.AddListener(ChickClick);
    }

    private void ChickClick()
    {
        UIPanelManager.instance.ShowPage<UIEquipView>();
    }

    public void UpdateInfo(BuildingData data, int index)
    {
        Icon.enabled = true;
        Lock.enabled = false;
        txt_Tip.text = "";
        Icon.sprite = GetSpriteAtlas.insatnce.GetWorkIcon(data.RoomName.ToString() + index);
        btn_Click.image.sprite = GetSpriteAtlas.insatnce.GetQuality(((QualityTypeEnum)index + 2).ToString());
        btn_Click.interactable = true;
    }

    public void UpdateInfo(EquipmentRealProperty equipData)
    {
        currentEquip = equipData;
        Icon.enabled = true;
        Lock.enabled = false;
        txt_Tip.text = "";
        Icon.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.SpriteName);
        btn_Click.image.sprite = GetSpriteAtlas.insatnce.GetIcon(equipData.QualityType.ToString());
        btn_Click.interactable = true;
    }

    public void UpdateLockInfo(int level)
    {
        Icon.enabled = false;
        Lock.enabled = true;
        txt_Tip.text = "";
        btn_Click.image.sprite = GetSpriteAtlas.insatnce.GetIcon("White");
        btn_Click.interactable = false;
    }
}
