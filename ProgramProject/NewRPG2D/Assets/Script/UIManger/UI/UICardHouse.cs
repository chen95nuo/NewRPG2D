using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TinyTeam.UI;

public class UICardHouse : MonoBehaviour
{
    public GameObject roleMenu;
    public UIBagItem updateBagItem;
    public Button btn_back;
    public Button btn_sort;
    public UIBagGrid grid;
    private UIBag createMenu;

    private GridType gridType;
    private int level;
    private bool isHight;

    private bool isNothing = true;

    public Animation anim;
    public Image BG;

    private CardData[] cardDatas;

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
        createMenu = new UIBag();
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
    private void UpdateRoleLevel(int level)
    {

    }

    /// <summary>
    /// 刷新角色
    /// </summary>
    /// <param name="data">撇除当前角色</param>
    public void UpdateRoleItem(UpdateCardData datas)
    {
        Debug.Log("this ");
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
