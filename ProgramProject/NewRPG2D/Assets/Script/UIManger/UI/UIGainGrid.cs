using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIGainGrid : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Image itemQuality;
    public Image itemImage;
    public Text itemNumber;
    public Image itemStars;

    private ItemType type;

    private EggData eggData;
    private EquipData equipData;
    private ItemData itemData;

    public void UpdateItem(EggData data)
    {
        type = data.ItemType;
        eggData = data;

        itemQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        itemImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        itemStars.sprite = IconMgr.Instance.GetIcon("Starts_" + data.StarsLevel);

        itemStars.gameObject.SetActive(true);
        itemNumber.gameObject.SetActive(false);
    }

    public void UpdateItem(EquipData data)
    {
        type = data.ItemType;
        equipData = data;

        itemQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        itemImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);

        itemStars.gameObject.SetActive(false);
        itemNumber.gameObject.SetActive(false);
    }

    public void UpdateItem(ItemData data)
    {
        type = data.ItemType;
        itemData = data;

        itemQuality.sprite = IconMgr.Instance.GetIcon("quality_" + data.Quality);
        itemImage.sprite = IconMgr.Instance.GetIcon(data.SpriteName);
        itemNumber.text = "x" + data.Number;

        itemStars.gameObject.SetActive(false);
        itemNumber.gameObject.SetActive(true);

    }




    public void OnPointerDown(PointerEventData eventData)
    {
        TTUIPage.ShowPage<UILittleTipPage>();

        switch (type)
        {
            case ItemType.Nothing:
                break;
            case ItemType.Egg:
                UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, eggData.Name);
                break;
            case ItemType.Prop:
                UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, itemData.Name);
                break;
            case ItemType.Equip:
                UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, equipData.Name);
                break;
            case ItemType.Role:
                break;
            default:
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (Input.GetMouseButton(0))
        {
            TTUIPage.ShowPage<UILittleTipPage>();

            switch (type)
            {
                case ItemType.Nothing:
                    break;
                case ItemType.Egg:
                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, eggData.Name);
                    break;
                case ItemType.Prop:
                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, itemData.Name);
                    break;
                case ItemType.Equip:
                    UIEventManager.instance.SendEvent<string>(UIEventDefineEnum.UpdateLittleTipEvent, equipData.Name);
                    break;
                case ItemType.Role:
                    break;
                default:
                    break;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateLittleTipEvent, false);
        if (Input.GetMouseButton(0))
        {

            TTUIPage.ClosePage<UILittleTipPage>();
        }
    }

}
