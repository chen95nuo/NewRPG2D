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

        AwakeInitialization();//初始化;

    }

    public void AwakeInitialization()
    {

    }

    public void PageBack()
    {
        ClosePage<UIFurnacePage>();
    }
}
