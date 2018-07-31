using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;


public class UIBagItemMessage : TTUIPage
{
    public UIBagItemMessage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIBagPopUp";
    }

    public override void Awake(GameObject go)
    {
        currentBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
    }

    private GameObject currentBtn;
    private UIBagPopUp pickUpMessage;
    public override void Refresh()
    {
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);
        pickUpMessage = transform.GetComponent<UIBagPopUp>();
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "UIItem")
        {
            UIBagGrid information = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<UIBagGrid>();
            Debug.Log("CurrentBtn" + UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            RectTransform rect = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();

            switch (information.itemType)
            {
                case ItemType.Nothing:
                    TTUIPage.ClosePage("UIBagItemMessage");
                    break;
                case ItemType.Egg:
                    //显示气泡，根据是否在图鉴中决定名称
                    pickUpMessage.updateMessage(information.eggData);
                    pickUpMessage.PopUpMoveTo(rect, information.eggData.ItemType);
                    break;
                case ItemType.Prop:
                    //显示名字，简介，是否使用
                    pickUpMessage.updateMessage(information.propData);
                    pickUpMessage.PopUpMoveTo(rect, information.propData.ItemType);
                    break;
                case ItemType.Equip:
                    if (information.gridType == GridType.Use)
                    {
                        pickUpMessage.updateMessage(information);
                        pickUpMessage.PopUpMoveTo(rect, information.equipData.ItemType);
                    }
                    else
                    {
                        pickUpMessage.updateMessage(information);
                        pickUpMessage.PopUpMoveTo(rect, information.equipData.ItemType);
                    }
                    break;
                case ItemType.Role:
                    break;
                default:
                    break;
            }
        }
    }


}
