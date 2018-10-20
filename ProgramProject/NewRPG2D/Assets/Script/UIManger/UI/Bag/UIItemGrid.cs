using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemGrid : MonoBehaviour
{
    private Button btn_Click;
    public Image image_Item;
    public Text txt_Num;
    private ItemType itemType;
    private EquipmentRealProperty equipData;
    //private ; //宝箱Data
    private PropData propData;

    private void Awake()
    {
        btn_Click.onClick.AddListener(ChickClick);
    }

    public void UpdateInfo(EquipmentRealProperty EquipData)
    {
        itemType = ItemType.Equip;
        txt_Num.text = "";
    }

    public void UpdateInfo()
    {
        itemType = ItemType.Box;
        txt_Num.text = "";
    }

    public void UpdateInfo(PropData data)
    {
        itemType = ItemType.Prop;
        txt_Num.text = data.num.ToString();
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
                break;
            case ItemType.Prop:
                HallEventManager.instance.SendEvent<PropData>(HallEventDefineEnum.ShowPropInfo, propData);
                break;
            default:
                break;
        }
    }
}
