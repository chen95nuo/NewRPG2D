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
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
        store = this.gameObject.GetComponent<UIStore>();
    }

    public override void Refresh()
    {
        store.buyTip.SetActive(false);
    }

    public void PageBack()
    {
        ClosePage<UIStorePage>();
    }
}
