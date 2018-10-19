using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

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

    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
        btn_AllItem.onClick.AddListener(ChickAllItem);
        btn_Weapon.onClick.AddListener(ChickWeapon);
        btn_Armor.onClick.AddListener(ChickArmor);
        btn_Jewelry.onClick.AddListener(ChickJewelry);
        btn_Box.onClick.AddListener(ChickBox);
        btn_Prop.onClick.AddListener(ChickProp);
    }

    private void ChickAllItem()
    {
        Debug.Log("刷新所有物品");
        sc.UpdateInfo(10);
    }
    private void ChickWeapon()
    {
        Debug.Log("刷新全部武器");
        sc.UpdateInfo(11);
    }
    private void ChickArmor()
    {
        Debug.Log("刷新全部防具");
        sc.UpdateInfo(13);
    }
    private void ChickJewelry()
    {
        Debug.Log("刷新全部首饰");
        sc.UpdateInfo(15);
    }
    private void ChickBox()
    {
        Debug.Log("刷新全部宝箱");
        sc.UpdateInfo(12);
    }
    private void ChickProp()
    {
        Debug.Log("刷新全部道具");
        sc.UpdateInfo(19);
    }
}
