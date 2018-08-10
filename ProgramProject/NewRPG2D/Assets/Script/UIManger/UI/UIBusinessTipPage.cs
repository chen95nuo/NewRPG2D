using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;

public class UIBusinessTipPage : TTUIPage
{
    public UIBusinessTipPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UIBusinessTip";
    }

    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
