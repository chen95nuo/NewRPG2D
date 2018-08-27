using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIFurnacePopUpGrid : MonoBehaviour, IPointerDownHandler
{
    public GameObject PopUp;
    public Image pop;
    public Image FMaterialImage;
    public Text FMaterialNumber;
    public int fMaterialNumber;
    public ItemMaterialType MType;
    public Animation anim;

    private void Awake()
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TinyTeam.UI.TTUIPage.ShowPage<UILittleTipPage>();
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, transform);
        switch (MType)
        {
            case ItemMaterialType.Nothing:
                break;
            case ItemMaterialType.Iron:
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, "铁");
                break;
            case ItemMaterialType.Wood:
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, "木头");
                break;
            case ItemMaterialType.Stone:
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, "石头");
                break;
            case ItemMaterialType.Leatherwear:
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, "布料");
                break;
            case ItemMaterialType.Magic:
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, "魔法");
                break;
            case ItemMaterialType.Diamonds:
                UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, "宝石");
                break;
            default:
                break;
        }
    }
}

