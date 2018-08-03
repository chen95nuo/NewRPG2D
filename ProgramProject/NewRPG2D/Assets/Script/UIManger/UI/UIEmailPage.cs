using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIEmailPage : TTUIPage
{

    public UIEmailPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIEmail";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
    }

    public void PageBack()
    {
        ClosePage<UIEmailPage>();
    }
}
