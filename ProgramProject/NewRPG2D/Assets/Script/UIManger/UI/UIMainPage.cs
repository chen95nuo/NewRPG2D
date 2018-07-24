using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;


public class UIMainPage : TTUIPage
{
    public UIMainPage() : base(UIType.Normal, UIMode.NoNeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIMain";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("DownBackGround/btn_Pack").GetComponent<Button>().onClick.AddListener(ShowPage<UIBagPage>);
        this.gameObject.transform.Find("DownBackGround/btn_CardHouse").GetComponent<Button>().onClick.AddListener(ShowPage<UICardHouse>);
        this.gameObject.transform.Find("BackGround/btn_Furnace").GetComponent<Button>().onClick.AddListener(ShowPage<UIFurnacePage>);
        this.gameObject.transform.Find("BackGround/btn_Store").GetComponent<Button>().onClick.AddListener(ShowPage<UIStorePage>);
        this.gameObject.transform.Find("BackGround/btn_Explore").GetComponent<Button>().onClick.AddListener(ShowPage<UIExplorePage>);
        this.gameObject.transform.Find("BackGround/btn_Hatchery").GetComponent<Button>().onClick.AddListener(ShowPage<UIHatcheryPoolPage>);

    }
}
