using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

public class UIItemGrid : MonoBehaviour
{
    private Button btn_Click;
    public Image image_Item;
    public Text txt_Num;
    private ItemType itemType;
    private EquipmentRealProperty equipData;
    private TreasureBox boxData;
    private PropData propData;

    private void Awake()
    {
        btn_Click = GetComponent<Button>();
        btn_Click.onClick.AddListener(ChickClick);
    }

    public void UpdateInfo(ItemHelper data)
    {
        propData = null;
        equipData = null;
        itemType = data.itemType;
        txt_Num.text = "";
        switch (data.itemType)
        {
            case ItemType.Equip:
                UpdateEquip(data);
                break;
            case ItemType.Box:
                UpdateBox(data);
                break;
            case ItemType.Prop:
                UpdateProp(data);
                break;
            default:
                break;
        }
        itemType = ItemType.Equip;
    }

    public void UpdateEquip(ItemHelper data)
    {
        equipData = EquipmentMgr.instance.GetEquipmentByEquipId(data.instanceId);
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(equipData.SpriteName);
        image_Item.sprite = sp;
    }

    public void UpdateBox(ItemHelper data)
    {
        boxData = ChickItemInfo.instance.GetBoxData(data.instanceId);
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(boxData.Icon);
        image_Item.sprite = sp;
        BoxDataHelper boxHData = ChickItemInfo.instance.GetBoxHelperData(data.instanceId);
        txt_Num.text = boxHData.num.ToString();
    }
    public void UpdateProp(ItemHelper data)
    {
        propData = ChickItemInfo.instance.GetPropData(data.instanceId);
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(propData.SpriteName);
        image_Item.sprite = sp;
        txt_Num.text = propData.num.ToString();
    }

    public void ChickClick()
    {
        Debug.Log("show装备简介");
        switch (itemType)
        {
            case ItemType.Equip:
                HallEventManager.instance.SendEvent<EquipmentRealProperty>(HallEventDefineEnum.ShowEquipInfo, equipData);
                break;
            case ItemType.Box:
                //UIPanelManager.instance.ShowPage<>();
                break;
            case ItemType.Prop:
                HallEventManager.instance.SendEvent<PropData>(HallEventDefineEnum.ShowPropInfo, propData);
                break;
            default:
                break;
        }
    }
}
