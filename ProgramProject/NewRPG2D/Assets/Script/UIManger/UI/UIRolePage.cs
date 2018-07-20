using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRolePage : TTUIPage
{
    private GameObject currentBtn;
    private UIRoleInformation pickUpRoleMessage;
    private UIRoleStrengthen pickUpRoleStrentthen;
    public UIRolePage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIRoleInformation";
    }
    public override void Awake(GameObject go)
    {
        currentBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(ClosePage<UIRolePage>);
        pickUpRoleStrentthen = transform.Find("UIRoleStrengthen").GetComponent<UIRoleStrengthen>();
        this.gameObject.transform.Find("btn_intensify").GetComponent<Button>().onClick.AddListener(ShowRoleStrengthenPage);
        pickUpRoleStrentthen.CloseThisPage();
    }

    public override void Refresh()
    {
        pickUpRoleMessage = transform.GetComponent<UIRoleInformation>();
        pickUpRoleMessage.Init();

        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name == "UIItem")
        {
            UIBagGrid information = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<UIBagGrid>();
            RectTransform rect = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<RectTransform>();

            switch (information.itemType)
            {
                case ItemType.Nothing:
                    TTUIPage.ClosePage("UIBagItemMessage");
                    break;
                case ItemType.Egg:
                    break;
                case ItemType.Prop:
                    break;
                case ItemType.Equip:
                    break;
                case ItemType.Role:
                    pickUpRoleMessage.updateMessage(information.roleData);
                    break;
                default:
                    break;
            }
        }
    }
    public void ShowRoleStrengthenPage()
    {
        if (pickUpRoleMessage.roleData.Fighting)
        {
            Debug.Log("正在探险");
            return;
        }
        pickUpRoleStrentthen.UpdateRole(pickUpRoleMessage);
    }
}
