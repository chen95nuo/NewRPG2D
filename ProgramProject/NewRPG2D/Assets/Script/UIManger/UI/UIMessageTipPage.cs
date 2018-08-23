using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIMessageTipPage : TTUIPage
{


    public UIMessageTipPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UIMessageTip";
    }
    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
