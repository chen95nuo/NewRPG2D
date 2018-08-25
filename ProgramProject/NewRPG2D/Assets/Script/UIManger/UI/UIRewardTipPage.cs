using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRewardTipPage : TTUIPage
{

    public UIRewardTipPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIRewardTip";
    }
    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("UIMain/btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
    }
    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    private void PageBack()
    {
        ClosePage<UIRewardTipPage>();
    }
}
