using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TinyTeam.UI;
using UnityEngine.UI;
using DG.Tweening;

public class UIBagPage : TTUIPage
{
    private GameObject bagMenu_1 = null;
    private GameObject bagItem_1 = null;
    private GameObject bagMenu_2 = null;
    private GameObject bagItem_2 = null;
    private GameObject bagItem = null;
    private UIBagItem updateBagItem = null;

    public Button[] firstMenu;

    public int firstLoad = 2;

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

        updateBagItem = transform.Find("ItemList/Viewport/Content").GetComponent<UIBagItem>();

        //一级菜单创建
        CreateMenu(0, bagItem_1, bagMenu_1.transform);

        firstMenu = bagMenu_1.GetComponentsInChildren<Button>();

        bagMenu_1.transform.GetChild(firstLoad).GetComponent<Button>().onClick.Invoke();
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
        MenuData data;
        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name != "btn_Pack")
        {
            string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text;
            int index = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            data = BagMenuData.Instance.GetMenu(name, index);
        }
        else
        {
            data = BagMenuData.Instance.GetMenu(0, firstLoad - 1);
        }

        if (data.ParentNumber == 0)
        {
            updateBagItem.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

            for (int i = 0; i < firstMenu.Length; i++)
            {
                firstMenu[i].GetComponentInChildren<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                firstMenu[i].GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UITexture/BagMenuTexture/" + data.ParentNumber + "/0");
                firstMenu[i].GetComponent<Button>().interactable = true;
            }
            GameObject go;
            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name != "btn_Pack")
            {
                go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            }
            else
            {
                go = bagMenu_1.transform.GetChild(firstLoad).gameObject;
            }
            go.GetComponentInChildren<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.down * 18.0f;
            go.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UITexture/BagMenuTexture/" + data.ParentNumber + "/1");
            go.GetComponent<Button>().interactable = false;


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


        updateBagItem.grids.Sort(new BagGridConparer().Compare);

        updateBagItem.grids.Sort(delegate (UIBagGrid x, UIBagGrid y)
            {
                int a = y.eggStarsNumb.CompareTo(x.eggStarsNumb);
                return a;
            });

        updateBagItem.grids.Sort((UIBagGrid x, UIBagGrid y) => -x.eggStarsNumb.CompareTo(y.eggStarsNumb));

        for (int i = 0; i < updateBagItem.grids.Count; i++)
        {
            if (updateBagItem.grids[i].equipType != EquipType.Armor)
            {
                
            }
            
        }


        //排序后刷新格子内容
        for (int i = 0; i < updateBagItem.grids.Count; i++)
        {
            updateBagItem.grids[i].transform.SetSiblingIndex(i + 1);
        }
        
    }


    /// <summary>
    /// 加载该选项背包物品
    /// </summary>
    public void LoadItemEvene(MenuData data)
    {
        ItemType itemType = (ItemType)data.Id + 1;
        switch (itemType)
        {
            case ItemType.Egg:
                //背包加载蛋
                Debug.Log("加载背包中的" + data.Name);
                updateBagItem.itemType = itemType;
                updateBagItem.UpdateEggs();
                break;
            case ItemType.Prop:
                Debug.Log("加载背包中的" + data.Name);
                updateBagItem.itemType = itemType;
                updateBagItem.UpdateProp();
                //背包加载道具
                break;
            case ItemType.Equip:
                Debug.Log("加载背包中的" + data.Name);
                updateBagItem.itemType = itemType;
                updateBagItem.UpdateEquip();
                //背包加载装备
                break;
            default:
                break;
        }
    }

    public class BagGridConparer : IComparer<UIBagGrid>
    {
        public int Compare(UIBagGrid x, UIBagGrid y)
        {
            if (x.eggStarsNumb == 0) return 1;
            if (y.eggStarsNumb == 0) return -1;
            {
                Debug.Log(x.eggStarsNumb);
                if (x.eggStarsNumb > y.eggStarsNumb) return 1;
                if (x.eggStarsNumb < y.eggStarsNumb) return -1;
                if (x.eggStarsNumb == y.eggStarsNumb) return 0;
            }
            return 0;
        }
        public int Compare1(UIBagGrid x, UIBagGrid y)
        {
            if (x.eggHatchingTime == 0) return 1;
            if (y.eggHatchingTime == 0) return -1;
            {
                Debug.Log(x.eggHatchingTime);
                if (x.eggHatchingTime > y.eggHatchingTime) return 1;
                if (x.eggHatchingTime < y.eggHatchingTime) return -1;
                if (x.eggHatchingTime == y.eggHatchingTime) return 0;
            }
            return 0;
        }
        public int Compare2(UIBagGrid x, UIBagGrid y)
        {
            if (x.quality == 0) return 1;
            if (y.quality == 0) return -1;
            {
                Debug.Log(x.quality);
                if (x.quality > y.quality) return 1;
                if (x.quality < y.quality) return -1;
                if (x.quality == y.quality) return 0;
            }
            return 0;
        }
        public int Compare3(UIBagGrid x, UIBagGrid y)
        {
            if (x.isUse == 0) return 1;
            if (y.isUse == 0) return -1;
            {
                Debug.Log(x.isUse);
                if (x.isUse > y.isUse) return 1;
                if (x.isUse < y.isUse) return -1;
                if (x.isUse == y.isUse) return 0;
            }
            return 0;
        }
    }


}
