using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;

public class UITeamTypePage : TTUIPage
{
    public UITeamTypePage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UITeamType";
    }

    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }


}
