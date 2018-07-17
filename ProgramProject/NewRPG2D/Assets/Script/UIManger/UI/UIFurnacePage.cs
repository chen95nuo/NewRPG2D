using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIFurnacePage : TTUIPage
{


    public UIFurnacePage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIFurnace";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
    }

    public override void Refresh()
    {
        this.gameObject.transform.Find("Tip").gameObject.SetActive(false);
    }

    public void PageBack()
    {
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateFurnaceEvent);
        ClosePage<UIFurnacePage>();
    }
}
