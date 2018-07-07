using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;

public class UIBagItemMessage : TTUIPage
{
    public UIBagItemMessage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIBagPopUp";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.GetComponent<UIBagPopUp>();
    }
}
