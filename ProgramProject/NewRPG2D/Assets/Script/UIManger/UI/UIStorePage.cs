using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIStorePage : TTUIPage
{
    private UIStore store;

    public UIStorePage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIStore";
    }

    public override void Awake(GameObject go)
    {

        store = this.gameObject.GetComponent<UIStore>();
    }
}
