using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRoleTipPage : TTUIPage
{

    private UIBagGrid CardGrid;

    public UIRoleTipPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIRoleTipPage";
    }

    public override void Awake(GameObject go)
    {
        UIEventManager.instance.AddListener<CardData>(UIEventDefineEnum.UpdateRoleTipEvent, UpdateRole);

        this.gameObject.transform.Find("Enter").GetComponent<Button>().onClick.AddListener(ClosePage<UIRoleTipPage>);
        CardGrid = this.gameObject.transform.Find("UIItem").GetComponent<UIBagGrid>();
    }

    public void UpdateRole(CardData data)
    {
        CardGrid.UpdateItem(data);
        BagRoleData.Instance.AddItem(data);
    }

}
