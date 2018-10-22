using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class ItemHelper
{
    public int instanceId;
    public ItemType itemType;
    public ItemHelper(int instanceId, ItemType itemType)
    {
        this.instanceId = instanceId;
        this.itemType = itemType;
    }
}
public class UIBag : TTUIPage
{
    #region GetButton
    public Button btn_back;
    public Button btn_AllItem;
    public Button btn_Weapon;
    public Button btn_Armor;
    public Button btn_Jewelry;
    public Button btn_Box;
    public Button btn_Prop;
    #endregion
    public ScrollControl sc;
    public List<ItemHelper> items = new List<ItemHelper>();

    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
        btn_AllItem.onClick.AddListener(ChickAllItem);
        btn_Weapon.onClick.AddListener(ChickWeapon);
        btn_Armor.onClick.AddListener(ChickArmor);
        btn_Jewelry.onClick.AddListener(ChickJewelry);
        btn_Box.onClick.AddListener(ChickBox);
        btn_Prop.onClick.AddListener(ChickProp);
        ChickAllItem();
    }

    private void ChickAllItem()
    {
        Debug.Log("刷新所有物品");
        items = ChickItemInfo.instance.GetAllItem();
        sc.UpdateInfo(items);
    }
    private void ChickWeapon()
    {
        Debug.Log("刷新全部武器");
        items = ChickItemInfo.instance.GetEquip(EquipTypeEnum.Sword);
        sc.UpdateInfo(items);
    }
    private void ChickArmor()
    {
        Debug.Log("刷新全部防具");
        items = ChickItemInfo.instance.GetEquip(EquipTypeEnum.Armor);
        sc.UpdateInfo(items);
    }
    private void ChickJewelry()
    {
        Debug.Log("刷新全部首饰");
        items = ChickItemInfo.instance.GetEquip(EquipTypeEnum.Jewelry);
        sc.UpdateInfo(items);
    }
    private void ChickBox()
    {
        Debug.Log("刷新全部宝箱");
        items = ChickItemInfo.instance.GetAllBox();
        sc.UpdateInfo(items);
    }
    private void ChickProp()
    {
        Debug.Log("刷新全部道具");
        items = ChickItemInfo.instance.GetAllProp();
        sc.UpdateInfo(items);
    }
}
