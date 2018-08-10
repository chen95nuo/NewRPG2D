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
            RectTransform rect = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();

            switch (information.itemType)
            {
                case ItemType.Nothing:
                    TTUIPage.ClosePage("UIBagItemMessage");
                    break;
                case ItemType.Prop:
                    //显示名字，简介，是否使用
                    pickUpMessage.updateMessage(information);
                    break;
                case ItemType.Equip:
                        pickUpMessage.updateMessage(information);
                    break;
                case ItemType.Role:
                    break;
                default:
                    break;
            }
        }
    }


}
