using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRewardPage : TTUIPage
{

    public UIRewardPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIReward";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
    }

    public void PageBack()
    {
        ClosePage<UIRewardPage>();
    }
}

