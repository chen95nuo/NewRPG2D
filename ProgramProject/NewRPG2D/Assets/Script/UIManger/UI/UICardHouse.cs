using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UICardHouse : TTUIPage
{
    private GameObject roleMenu;
    public UIBag createMenu;
    private UIBagGrid grid;
    private UIBagItem updateBagItem;

    public UICardHouse() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UICardHouse";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(ClosePage<UICardHouse>);
        this.gameObject.transform.Find("btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        updateBagItem = transform.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();
        grid = transform.Find("ItemList/Viewport/Content/Card_UIItem").GetComponent<UIBagGrid>();
        Init();//初始化


    }


    public void Init()
    {
        createMenu = new UIBag();
        roleMenu = transform.Find("MenuType/btn_type").gameObject;
        roleMenu.SetActive(false);

        createMenu.CreateMenu(10, roleMenu, roleMenu.transform.parent.transform);
    }


    public void ItemSortEvent()
    {
        createMenu.ItemSortEvent(transform);
    }


}
