using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UIMapTypePage : TTUIPage
{
    public UIMapTypePage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIMapType";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("MainLesson/btn_back").GetComponent<Button>().onClick.AddListener(ClosePage<UIMapTypePage>);
    }

}
