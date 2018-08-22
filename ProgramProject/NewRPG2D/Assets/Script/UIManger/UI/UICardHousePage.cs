using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UICardHousePage : TTUIPage
{

    public UICardHousePage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UICardHouse";
    }

    public override void Refresh()
    {
        this.transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}