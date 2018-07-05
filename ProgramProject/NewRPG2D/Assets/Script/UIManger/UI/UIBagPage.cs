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

    public UIBagPage() : base(UIType.Normal, UIMode.NeedBack, UICollider.None)
    {
        uiPath = "UIPrefab/UIBag";
    }

    public override void Awake(GameObject go)
    {
        this.bagMenu_1 = transform.Find("Menu_1").gameObject;
        this.bagMenu_2 = transform.Find("Menu_2").gameObject;
        this.bagItem_1 = bagMenu_1.transform.Find("btn_menu_1").gameObject;
        this.bagItem_2 = bagMenu_2.transform.Find("btn_menu_2").gameObject;

        this.gameObject.transform.Find("btn_back").GetComponent<Button>().onClick.AddListener(TTUIPage.ClosePage);
        this.gameObject.transform.Find("btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);

        bagMenu_2.SetActive(false);
        bagItem_1.SetActive(false);

        //一级菜单创建
        CreateMenu(0, bagItem_1, bagMenu_1.transform);

    }

    public override void Refresh()
    {
        //默认如何排序?
        //bagMenu_2.SetActive(false);
    }

    /// <summary>
    /// 菜单创建
    /// </summary>
    /// <param name="parentName"></param>
    /// <param name="obj"></param>
    /// <param name="objParent"></param>
    private void CreateMenu(int ParentNumber, GameObject obj, Transform objParent)
    {
        for (int i = 0; i < BagMenuData.Instance.menu.Count; i++)
        {
            if (BagMenuData.Instance.GetMenu(ParentNumber, i) != null)
            {
                if (objParent.childCount - 1 > i)
                {
                    objParent.GetChild(i + 1).gameObject.SetActive(true);
                    objParent.GetChild(i + 1).name = BagMenuData.Instance.GetMenu(ParentNumber, i).ParentNumber.ToString();
                    objParent.GetChild(i + 1).GetComponentInChildren<Text>().text = BagMenuData.Instance.GetMenu(ParentNumber, i).Name;
                    objParent.GetChild(i + 1).GetComponent<Button>().onClick.AddListener(OnClickMenu);
                }
                else
                {
                    GameObject bagItem = GameObject.Instantiate(obj, objParent) as GameObject;
                    bagItem.SetActive(true);
                    bagItem.name = BagMenuData.Instance.GetMenu(ParentNumber, i).ParentNumber.ToString();
                    bagItem.GetComponentInChildren<Text>().text = BagMenuData.Instance.GetMenu(ParentNumber, i).Name;
                    bagItem.GetComponent<Button>().onClick.AddListener(OnClickMenu);
                }
            }
            else
            {
                return;
            }
        }
    }
    /// <summary>
    /// 菜单点击事件
    /// </summary>
    public void OnClickMenu()
    {

        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
        int index = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
        MenuData data = BagMenuData.Instance.GetMenu(name, index);

        if (data.ParentNumber == 0)
        {
            GameObject obj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            bagMenu_2.SetActive(true);
            bagItem_2.SetActive(false);
            for (int i = 0; i < bagMenu_2.transform.childCount; i++)
            {
                bagMenu_2.transform.GetChild(i).gameObject.SetActive(false);
            }
            CreateMenu(data.Id + 1, bagItem_2, bagMenu_2.transform);


            //通知读取当前点击的菜单信息
            LoadItemEvene(data);

        }
        else
        {
            //底层选项触发排序
            ItemSortEvent(data);
        }
    }

    /// <summary>
    /// 排序事件
    /// </summary>
    public void ItemSortEvent()
    {
        Debug.Log("按照首选项排序");
    }
    public void ItemSortEvent(MenuData data)
    {
        Debug.Log("将" + BagMenuData.Instance.GetMenu(0, data.ParentNumber - 1).Name + "按照" + data.Name + "排序");
    }


    /// <summary>
    /// 加载该选项背包物品
    /// </summary>
    public void LoadItemEvene(MenuData data)
    {
        Debug.Log("加载背包中的" + data.Name);
        ItemType itemType = (ItemType)data.Id;
        switch (itemType)
        {
            case ItemType.Egg:
                //背包加载蛋
                break;
            case ItemType.Prop:
                //背包加载道具
                break;
            case ItemType.Equip:
                //背包加载装备
                break;
            default:
                break;
        }
    }
}
