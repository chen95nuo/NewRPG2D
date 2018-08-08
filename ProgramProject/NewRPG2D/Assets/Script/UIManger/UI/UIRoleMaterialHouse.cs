using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRoleMaterialHouse : MonoBehaviour
{

    private GameObject roleMenu;
    public UIBag createMenu;
    private UIBagGrid grid;
    private UIBagItem updateBagItem;

    private void Awake()
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
        this.gameObject.transform.Find("btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        updateBagItem = transform.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();
        grid = transform.Find("ItemList/Viewport/Content/UIItem").GetComponent<UIBagGrid>();
        Init();//初始化
    }

    public void Init()
    {
        createMenu = new UIBag();
        roleMenu = transform.Find("MenuType/btn_type").gameObject;
        roleMenu.SetActive(false);
        grid.gridType = GridType.Use;
        createMenu.CreateMenu(10, roleMenu, roleMenu.transform.parent.transform);
    }

    public void ItemSortEvent()
    {
        createMenu.ItemSortEvent(transform);
    }

    public void UpdateBagItem(CardData[] datas)
    {
        updateBagItem.UpdateRole(datas, GridType.Use);
    }

    public void PageBack()
    {
        gameObject.SetActive(false);
    }
}
