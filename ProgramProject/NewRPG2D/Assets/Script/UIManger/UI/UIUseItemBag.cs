using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUseItemBag : MonoBehaviour
{

    public Button btn_Back;
    public Button btn_Sort;
    public GameObject bagMenu;
    public UIBagItem updateBagItem;

    private GameObject bagItem;
    private bool itemSort = true;
    private EquipType equipType;



    private void Awake()
    {
        //初始化
        Init();

        UIEventManager.instance.AddListener<EquipType>(UIEventDefineEnum.UpdateEquipsEvent, UpdateItem);
        UIEventManager.instance.AddListener<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, UpdateItem);
        UIEventManager.instance.AddListener<PropMeltingType>(UIEventDefineEnum.UpdatePropsEvent, UpdateItem);
        UIEventManager.instance.AddListener<ItemType>(UIEventDefineEnum.UpdateUsePage, UpdateItem);
    }

    private void OnDestroy()
    {
        UIEventManager.instance.RemoveListener<EquipType>(UIEventDefineEnum.UpdateEquipsEvent, UpdateItem);
        UIEventManager.instance.RemoveListener<UIBagGrid>(UIEventDefineEnum.UpdateEquipsEvent, UpdateItem);
        UIEventManager.instance.RemoveListener<PropMeltingType>(UIEventDefineEnum.UpdatePropsEvent, UpdateItem);
        UIEventManager.instance.RemoveListener<ItemType>(UIEventDefineEnum.UpdateUsePage, UpdateItem);
    }


    public void Init()
    {
        btn_Back.GetComponent<Button>().onClick.AddListener(TinyTeam.UI.TTUIPage.ClosePage<UIUseItemBagPage>);
        btn_Sort.GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        bagMenu.SetActive(false);
    }

    /// <summary>
    /// 打开道具包时刷新物品
    /// </summary>
    /// <param name="type"></param>
    public void UpdateItem(EquipType type)
    {
        updateBagItem.itemType = ItemType.Equip;
        this.updateBagItem.UpdateEquip(type);
        equipType = type;
        itemSort = false;
        ItemSortEvent();
    }

    public void UpdateItem(PropMeltingType melting)
    {
        updateBagItem.itemType = ItemType.Prop;
        this.updateBagItem.UpdateProp(melting);
        itemSort = true;
        ItemSortEvent(ItemType.Prop);
        //itemSortEvent();
    }

    public void UpdateItem(ItemType type)
    {
        updateBagItem.itemType = type;
        switch (type)
        {
            case ItemType.Nothing:
                break;
            case ItemType.Egg:
                this.updateBagItem.UpdateEggs();
                break;
            case ItemType.Prop:
                break;
            case ItemType.Equip:
                break;
            case ItemType.Role:
                break;
            default:
                break;
        }
    }


    public void UpdateItem(UIBagGrid data)
    {
        TinyTeam.UI.TTUIPage.ClosePage<UIBagItemMessage>();
        TinyTeam.UI.TTUIPage.ClosePage<UIUseItemBagPage>();
    }
    /// <summary>
    /// 排序事件
    /// </summary>
    public void ItemSortEvent()
    {
        itemSort = !itemSort;

        ItemSortEvent(equipType);
    }

    public void ItemSortEvent(ItemType type)
    {
        switch (type)
        {
            case ItemType.Nothing:
                break;
            case ItemType.Egg:
                break;
            case ItemType.Prop:
                updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare((int)x.itemType, (int)y.itemType));
                if (itemSort)
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.quality, y.quality));
                else
                    updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.quality, y.quality));
                break;
            case ItemType.Equip:
                break;
            case ItemType.Role:
                break;
            default:
                break;
        }
    }

    public void ItemSortEvent(EquipType type)
    {

        updateBagItem.UpdateEquip(type);
        if (itemSort)
            updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.quality, y.quality));
        else
            updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.quality, y.quality));
        //排序后刷新格子内容
        for (int i = 0; i < updateBagItem.grids.Count; i++)
        {
            updateBagItem.grids[i].transform.SetSiblingIndex(i + 1);
        }
    }
}

public class BagGridConparer : IComparer<int>
{
    //倒序
    public int Compare(int x, int y)
    {
        if (x == 0) return 1;
        if (y == 0) return -1;


        if (x > y) return -1;
        if (x < y) return 1;

        return 0;
    }
    //顺序
    public int Compare1(int x, int y)
    {
        if (x == 0) return 1;
        if (y == 0) return -1;


        if (x > y) return 1;
        if (x < y) return -1;

        return 0;
    }
}



