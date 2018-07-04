using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;

public class UIBagPage : TTUIPage
{
    private GameObject bagMenu_1 = null;
    private GameObject bagItem_1 = null;
    private GameObject bagMenu_2 = null;
    private GameObject bagItem_2 = null;
    private GameObject bagItem = null;

    public UIBagPage() : base(UIType.Normal, UIMode.HideOther, UICollider.None)
    {
        uiPath = "UIPrefab/UIBag";
    }

    public override void Awake(GameObject go)
    {
        this.bagMenu_1 = transform.Find("Menu_1").gameObject;
        this.bagMenu_2 = transform.Find("Menu_2").gameObject;
        this.bagItem_1 = bagMenu_1.transform.Find("btn_menu_1").gameObject;
        this.bagItem_2 = bagMenu_2.transform.Find("btn_menu_2").gameObject;

        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(() =>
        {
            TTUIPage.ClosePage();
        });

        bagMenu_2.SetActive(false);
        bagItem_1.SetActive(false);
        //一级菜单 创建 生成
        for (int i = 0; i < BagMenuData.Instance.menu.Count; i++)
        {
            if (BagMenuData.Instance.GetMenu(0, i) != null)
            {
                GameObject bagItem = GameObject.Instantiate(bagItem_1, bagMenu_1.transform) as GameObject;
                bagItem.SetActive(true);
                bagItem.GetComponentInChildren<Text>().text = BagMenuData.Instance.GetMenu(0, i).Name;
                bagItem.GetComponent<Button>().onClick.AddListener(OnClickMenuItem);
            }
            else
            {
                return;
            }
        }
    }

    public override void Refresh()
    {

    }

    public void OnClickMenuItem()
    {

    }

    private void CreateMenuItem()
    {

    }
}
