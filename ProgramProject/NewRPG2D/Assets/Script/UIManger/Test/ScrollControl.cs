using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{

    public InfinityGridLayoutGroup infinity;

    //int amount = 6;
    public List<ItemHelper> items = new List<ItemHelper>();
    // Use this for initialization

    private void Start()
    {
        ChickAllItem();
    }

    public void UpdateInfo(List<ItemHelper> items)
    {
        int amount = items.Count;
        ////初始化数据列表;
        infinity.SetAmount(amount);
        infinity.updateChildrenCallback = UpdateChildrenCallback;
    }

    public void UpdateInfo(BagType type)
    {
        switch (type)
        {
            case BagType.AllItem:
                ChickAllItem();
                break;
            case BagType.Weapons:
                ChickWeapon();
                break;
            case BagType.Armor:
                ChickArmor();
                break;
            case BagType.Jewelry:
                ChickJewelry();
                break;
            case BagType.Box:
                ChickBox();
                break;
            case BagType.Prop:
                ChickProp();
                break;
            case BagType.Max:
                break;
            default:
                break;
        }
    }

    void UpdateChildrenCallback(int index, Transform trans)
    {
        UIBagGrid grid = trans.GetComponent<UIBagGrid>();
        if (grid == null)
        {
            return;
        }
        if (index < items.Count)
        {
            grid.UpdateInfo(items[index]);
        }
    }

    private void ChickAllItem()
    {
        Debug.Log("刷新所有物品");
        items = ChickItemInfo.instance.GetAllItem();
        UpdateInfo(items);
    }
    private void ChickWeapon()
    {
        Debug.Log("刷新全部武器");
        items = ChickItemInfo.instance.GetEquip(EquipTypeEnum.Sword);
        UpdateInfo(items);
    }
    private void ChickArmor()
    {
        Debug.Log("刷新全部防具");
        items = ChickItemInfo.instance.GetEquip(EquipTypeEnum.Armor);
        UpdateInfo(items);
    }
    private void ChickJewelry()
    {
        Debug.Log("刷新全部首饰");
        items = ChickItemInfo.instance.GetEquip(EquipTypeEnum.Jewelry);
        UpdateInfo(items);
    }
    private void ChickBox()
    {
        Debug.Log("刷新全部宝箱");
        items = ChickItemInfo.instance.GetAllBox();
        UpdateInfo(items);
    }
    private void ChickProp()
    {
        Debug.Log("刷新全部道具");
        items = ChickItemInfo.instance.GetAllProp();
        UpdateInfo(items);
    }
}