using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIRoundPage : TTUIPage
{

    public UIRoundPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIRound";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("MainLesson/btn_back").GetComponent<Button>().onClick.AddListener(ClosePage<UIRoundPage>);

    }
}
