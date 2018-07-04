using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;

public class UIBagPage : TTUIPage
{
    private GameObject bagMenu_1 = null;
    private GameObject bagMenu_2 = null;
    private GameObject bagItem = null;

    public UIBagPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIBag";
    }

    public override void Awake(GameObject go)
    {
        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
        {
            TTUIPage.ClosePage();
        });

        //bagMenu_2.SetActive(false);
        Debug.Log(BagMenuData.Instance.menu.Count);
        Debug.Log(BagMenuData.Instance.GetMenu(0,0));

    }

    public override void Refresh()
    {

    }

    private void CreateSkillItem()
    {

    }
}
