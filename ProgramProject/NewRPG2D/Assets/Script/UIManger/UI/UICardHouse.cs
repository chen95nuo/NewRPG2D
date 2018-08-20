using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UICardHouse : MonoBehaviour
{
    private GameObject roleMenu;
    private UIBag createMenu;
    private UIBagGrid grid;
    private UIBagItem updateBagItem;

    private GridType gridType;
    private int level;

    private bool isNothing = true;

    private Animation anim;
    public Image BG;

    private void Awake()
    {
        Init();//初始化

        UIEventManager.instance.AddListener<GridType>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleGridType);
        UIEventManager.instance.AddListener<int>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleLevel);
        UIEventManager.instance.AddListener<CardData[]>(UIEventDefineEnum.UpdateRolesEvent, UpdateRoleItem);
        UIEventManager.instance.AddListener<bool>(UIEventDefineEnum.UpdateRolesEvent, CloseCardsPage);
    }

    public void Init()
    {
        anim = transform.Find("UICardHouseMain").GetComponent<Animation>();
        this.gameObject.transform.Find("UICardHouseMain/btn_back").GetComponent<Button>().onClick.AddListener(PageBack);
        this.gameObject.transform.Find("UICardHouseMain/btn_sort").GetComponent<Button>().onClick.AddListener(ItemSortEvent);
        updateBagItem = transform.Find("UICardHouseMain/ItemList/Viewport/Content").GetComponent<UIBagItem>();
        grid = transform.Find("UICardHouseMain/ItemList/Viewport/Content/Card_UIItem").GetComponent<UIBagGrid>();
        createMenu = new UIBag();
        roleMenu = transform.Find("UICardHouseMain/MenuType/btn_type").gameObject;
        roleMenu.SetActive(false);
        createMenu.CreateMenu(10, roleMenu, roleMenu.transform.parent.transform);
    }

    private void OnEnable()
    {
        UIAnimTools.Instance.PlayAnim(anim, "UICardHouseMain_in", false);
        UIAnimTools.Instance.GetBG(BG, false, .2f);
    }

    public void ItemSortEvent()
    {
        Debug.Log("排序");
        createMenu.ItemSortEvent(transform);
    }

    private void UpdateRoleGridType(GridType type)
    {
        gridType = type;
    }
    private void UpdateRoleLevel(int level)
    {
        this.level = level;
    }

    /// <summary>
    /// 刷新角色
    /// </summary>
    /// <param name="data">撇除当前角色</param>
    public void UpdateRoleItem(CardData[] datas)
    {
        isNothing = false;
        grid.gridType = gridType;
        switch (gridType)
        {
            case GridType.Nothing:
                break;
            case GridType.Use:
                updateBagItem.UpdateRole(datas, gridType);
                break;
            case GridType.Store:
                break;
            case GridType.Explore:
                updateBagItem.UpdateRole(datas, level);
                break;
            case GridType.Team:
                updateBagItem.UpdateRole(datas, gridType);
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

}
