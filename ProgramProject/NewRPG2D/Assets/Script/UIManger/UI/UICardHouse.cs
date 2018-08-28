using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;
using UnityEngine.EventSystems;

public class UICardHouse : MonoBehaviour
{
    public GameObject roleMenu;
    public UIBagItem updateBagItem;
    public Button btn_back;
    public Button btn_sort;
    public UIBagGrid grid;

    private GridType gridType;
    private int level;
    private bool isHight;

    private bool isNothing = true;

    public Animation anim;
    public Image BG;
    private CardData[] cardDatas;
    private MenuData data;
    private Button currentMenu;
    public int firstLoad = 2;
    public Button[] firstMenu;
    public GameObject bagMenu_1 = null;
    public GameObject bagMenu_2 = null;
    public GameObject bagItem_2 = null;
    private bool sortItems = true;
    public UIBagItem updateBagRole;


    private void Awake()
    {
        Init();//初始化

        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleLevel);
        UIEventManager.instance.AddListener<UpdateCardData>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleItem);
        UIEventManager.instance.AddListener<bool>(UIEventDefineEnum.UpdateRolesEvent, CloseCardsPage);
    }

    public void Init()
    {
        btn_back.GetComponent<Button>().onClick.AddListener(PageBack);
        btn_sort.GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        roleMenu.SetActive(false);
        CreateMenu(10, roleMenu, roleMenu.transform.parent.transform);
    }

    private void OnEnable()
    {
        UIAnimTools.Instance.PlayAnim(anim, "UICardHouseMain_in", false);
        UIAnimTools.Instance.GetBG(BG, false, .2f);
    }

    public void ItemSortEvent()
    {
        sortItems = !sortItems;
        ItemSortEvent(data, ItemType.Role);
    }
    private void UpdateRoleLevel(int level)
    {

    }

    /// <summary>
    /// 刷新角色
    /// </summary>
    /// <param name="data">撇除当前角色</param>
    public void UpdateRoleItem(UpdateCardData datas)
    {
        isNothing = false;
        gridType = datas.gridType;
        grid.gridType = gridType;
        cardDatas = datas.cardDatas;
        level = datas.level;
        isHight = datas.isHight;

        UpdateRoleItem();
    }

    public void UpdateRoleItem()
    {
        switch (gridType)
        {
            case GridType.Nothing:
                updateBagItem.UpdateRole();
                break;
            case GridType.Use:
                updateBagItem.UpdateRole(cardDatas, gridType);
                break;
            case GridType.Store:
                break;
            case GridType.Explore:
                updateBagItem.UpdateRole(cardDatas, level, isHight);
                break;
            case GridType.Team:
                updateBagItem.UpdateRole(cardDatas, gridType);
                break;
            case GridType.NoClick:
                break;
            default:
                break;
        }
    }

    public void PageBack()
    {
        UIAnimTools.Instance.PlayAnim(anim, "UICardHouseMain_in", true);
        UIAnimTools.Instance.GetBG(BG, true, .2f);
        Invoke("CloseThisPage", .8f);
    }
    public void CloseCardsPage(bool isTrue)
    {
        //UIAnimTools.Instance.PlayAnim(anim, "UICardHouseMain_in", isTrue);
        //UIAnimTools.Instance.GetBG(BG, !isTrue, .2f);
        if (isTrue == false)
        {
            //Invoke("CloseThisPage", .8f);
            CloseThisPage();
        }
    }

    public void CloseThisPage()
    {
        TTUIPage.ClosePage<UICardHousePage>();
    }


    /// <summary>
    /// 菜单创建
    /// </summary>
    /// <param name="parentName"></param>
    /// <param name="obj"></param>
    /// <param name="objParent"></param>
    public void CreateMenu(int ParentNumber, GameObject obj, Transform objParent)
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
        Debug.Log("触发点击");
        GameObject obj = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        if (obj == null)
        {
            return;
        }
        if (obj.name != "btn_Pack")
        {
            string name = obj.GetComponentInChildren<Text>().text;
            int index = int.Parse(obj.name);
            data = BagMenuData.Instance.GetMenu(name, index);
            Button btn_ = obj.GetComponent<Button>();
            btn_.interactable = false;

            if (currentMenu != null && currentMenu != btn_)
            {
                currentMenu.interactable = true;
            }
            currentMenu = btn_;
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
                firstMenu[i].GetComponentInChildren<Image>().sprite = GetSpriteAtlas.insatnce.GetIcon("Cry_Btn_2");
                firstMenu[i].GetComponent<Button>().interactable = true;
            }
            GameObject go;
            if (obj.name != "btn_Pack")
            {
                go = obj;

            }
            else
            {
                go = bagMenu_1.transform.GetChild(firstLoad).gameObject;
            }
            go.GetComponentInChildren<Image>().GetComponent<RectTransform>().anchoredPosition = Vector2.down * 18.0f;
            go.GetComponentInChildren<Image>().sprite = GetSpriteAtlas.insatnce.GetIcon("Cry_Btn_1");
            go.GetComponent<Button>().interactable = false;


            bagMenu_2.SetActive(true);
            bagItem_2.SetActive(false);
            for (int i = 0; i < bagMenu_2.transform.childCount; i++)
            {
                bagMenu_2.transform.GetChild(i).gameObject.SetActive(false);
            }
            CreateMenu(data.Id + 1, bagItem_2, bagMenu_2.transform);

            //通知读取当前点击的菜单信息
        }
        else
        {
            //底层选项触发排序
            if (data.ParentNumber >= 10)
            {
                Debug.LogError(obj.transform.parent.parent.transform.name);
                ItemSortEvent(data, ItemType.Role);
            }
        }
    }
    public void ItemSortEvent(MenuData data, ItemType type)
    {
        switch (data.Id)
        {
            case 0:
                if (sortItems)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.level, y.level));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.level, y.level));
                break;
            case 1:
                if (sortItems)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.stars, y.stars));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.stars, y.stars));
                break;
            case 2:
                if (sortItems)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.grow, y.grow));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.grow, y.grow));
                break;
            case 3:
                if (sortItems)
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare(x.goodFeeling, y.goodFeeling));
                else
                    updateBagRole.grids.Sort((UIBagGrid x, UIBagGrid y) => new BagGridConparer().Compare1(x.goodFeeling, y.goodFeeling));
                break;
            default:
                break;
        }
        //排序后刷新格子内容
        for (int i = 0; i < updateBagRole.grids.Count; i++)
        {
            updateBagRole.grids[i].transform.SetSiblingIndex(i + 1);
        }
    }
}

public class UpdateCardData
{
    public CardData[] cardDatas;
    public GridType gridType;
    public int level;
    public bool isHight;

    public UpdateCardData() { }
    public UpdateCardData(CardData[] data, GridType type)
    {
        this.cardDatas = data;
        this.gridType = type;
    }
    public UpdateCardData(CardData[] data, GridType type, int level, bool isHight)
    {
        this.cardDatas = data;
        this.gridType = type;
        this.level = level;
        this.isHight = isHight;
    }
}
