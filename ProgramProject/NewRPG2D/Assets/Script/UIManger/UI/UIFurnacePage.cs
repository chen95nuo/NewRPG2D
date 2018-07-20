using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIFurnacePage : TTUIPage
{

    private GameObject Tip;

    public UIFurnacePage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIFurnace";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
        Tip = this.gameObject.transform.Find("Tip").gameObject;
    }

    public override void Refresh()
    {
        Tip.SetActive(false);
    }

    public void PageBack()
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateFurnaceEvent);
        ClosePage<UIFurnacePage>();
    }
}
