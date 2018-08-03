using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIEggStore : TTUIPage
{
    public UIEggStore() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIEggStore";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(CloseEggStore);
    }

    private void CloseEggStore()
    {
        ClosePage<UIEggStore>();
    }
}
