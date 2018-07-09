using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;

public class UICardStoreHouse : TTUIPage
{
    public UICardStoreHouse() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UICardStoreHouse";
    }

    public override void Awake(GameObject go)
    {
        
    }
}
