using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;

public class UITalkPage : TTUIPage
{
    public UITalkPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UITalk";
    }

    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

}
