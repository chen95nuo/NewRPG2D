using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIUseRoleHousePage : TTUIPage
{
    private GameObject roleMenu;
    private UIBagPage createMenu;
    private UIBagGrid grid;
    private GridType gridType;
    private UIBagItem updateBagItem;
    private int level;

    public UIUseRoleHousePage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UIUseRoleHouse";
    }
    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
        this.gameObject.transform.Find("btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        updateBagItem = transform.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();
        grid = transform.Find("ItemList/Viewport/Content/UIItem").GetComponent<UIBagGrid>();
        Init();//初始化
        UIEventManager.instance.AddListener<GridType>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleGridType);
        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleLevel);
        UIEventManager.instance.AddListener<CardData[]>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleItem);

    }



    public override void Refresh()
    {
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public void ItemSortEvent()
    {
        createMenu.ItemSortEvent(transform);
    }

    public void Init()
    {
        createMenu = new UIBagPage();
        roleMenu = transform.Find("MenuType/btn_type").gameObject;
        roleMenu.SetActive(false);
        createMenu.CreateMenu(10, roleMenu, roleMenu.transform.parent.transform);
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
        grid.gridType = gridType;
        if (gridType == GridType.Explore)
        {
            updateBagItem.UpdateRole(datas, level);
        }
        else
        {
            updateBagItem.UpdateRole(datas, gridType);
        }

    }

    public void PageBack()
    {
        ClosePage<UIUseRoleHousePage>();
    }
}
