using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIUseItemBagPage : TTUIPage
{
    private GameObject bagMenu;
    private GameObject bagItem;
    private UIBagItem updateBagItem;
    private bool itemSort = true;
    private EquipType equipType;

    public UIUseItemBagPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIUseItemBag";
    }

    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
    
}