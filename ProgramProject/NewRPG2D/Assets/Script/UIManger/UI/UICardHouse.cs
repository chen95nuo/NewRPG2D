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

    private GridType gridType;
    private int level;

    private bool isNothing = true;

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

        UIEventManager.instance.AddListener<GridType>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleGridType);
        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleLevel);
        UIEventManager.instance.AddListener<CardData[]>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleItem);
    }
    public override void Refresh()
    {
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);
        //gridType = GridType.Nothing;
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateRolesEvent);
    }

    public void Init()
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(ClosePage<UICardHouse>);
        this.gameObject.transform.Find("btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        updateBagItem = transform.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();
        grid = transform.Find("ItemList/Viewport/Content/Card_UIItem").GetComponent<UIBagGrid>();
        createMenu = new UIBag();
        roleMenu = transform.Find("MenuType/btn_type").gameObject;
        roleMenu.SetActive(false);
        createMenu.CreateMenu(10, roleMenu, roleMenu.transform.parent.transform);
    }


    public void ItemSortEvent()
    {
        Debug.Log("排序");
        createMenu.ItemSortEvent(transform);
    }

    private void UpdateRoleGridType(GridType type)
    {
        gridType = type;
    }
    private void UpdateRoleLevel(int level)
    {
        this.level = level;
    }

    /// <summary>
    /// 刷新角色
    /// </summary>
    /// <param name="data">撇除当前角色</param>
    public void UpdateRoleItem(CardData[] datas)
    {
        isNothing = false;
        grid.gridType = gridType;
        switch (gridType)
        {
            case GridType.Nothing:
                break;
            case GridType.Use:
                updateBagItem.UpdateRole(datas, gridType);
                break;
            case GridType.Store:
                break;
            case GridType.Explore:
                updateBagItem.UpdateRole(datas, level);
                break;
            case GridType.Team:
                updateBagItem.UpdateRole(datas, gridType);
                break;
            case GridType.NoClick:
                break;
            default:
                break;
        }
    }

    public void PageBack()
    {
        ClosePage<UICardHouse>();
    }

}
