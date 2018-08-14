using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using UnityEngine.SceneManagement;

public class UIMissionFailedPage : TTUIPage
{

    private Button btn;

    public UIMissionFailedPage() : base(UIType.Normal, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "UIPrefab/UIMissionFailed";
    }

    public override void Awake(GameObject go)
    {
        btn = this.transform.Find("BG").GetComponent<Button>();
        btn.onClick.AddListener(ChickBtn);
    }

    public override void Refresh()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    private void ChickBtn()
    {
        SceneManager.LoadScene(GoFightMgr.instance.mainScene);
    }
}
