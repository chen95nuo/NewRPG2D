using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using DG.Tweening;

public class UIScreenRole : TTUIPage
{
    public static UIScreenRole instance;

    public Button[] btnGroup;
    public Button btn_back;
    public Transform gridPoint;
    private List<HallRoleData> screenRole = new List<HallRoleData>();
    private List<UIScreenRoleGrid> RoleGrid = new List<UIScreenRoleGrid>();

    public RectTransform rt;
    public ScrollRect sr;

    private int currentBtnIndex = 0;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < btnGroup.Length; i++)
        {
            btnGroup[i].onClick.AddListener(ChickBtnClick);
        }
        btn_back.onClick.AddListener(ClosePage);
        btnGroup[currentBtnIndex].interactable = false;

        ChickAll();
    }

    private void UpdateInfo()
    {
        int index = ChickPlayerInfo.instance.ChickAtrNumber();
        for (int i = 0; i < btnGroup.Length; i++)
        {
            if (i < index)
            {
                btnGroup[i].gameObject.SetActive(true);
            }
            else
            {
                btnGroup[i].gameObject.SetActive(false);
            }
        }
    }

    private void ChickBtnClick()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        int index = 0;
        for (int i = 0; i < btnGroup.Length; i++)
        {
            if (btnGroup[i].gameObject == go)
            {
                btnGroup[currentBtnIndex].interactable = true;
                btnGroup[i].interactable = false;
                currentBtnIndex = i;
                index = i;
                break;
            }
        }

        switch (index)
        {
            case 0: ChickAll(); break;
            case 1: ChickAtk(); break;
            case 2: ChickGold(); break;
            case 3: ChickFood(); break;
            case 4: ChickMana(); break;
            case 5: ChickWood(); break;
            case 6: ChickIron(); break;
            default:
                break;
        }
    }

    /// <summary>
    /// 开启面板同时显示的界面 默认显示排列
    /// </summary>
    public void ShowPage()
    {
        switch (currentBtnIndex)
        {
            case 0: ChickAll(); break;
            case 1: ChickAtk(); break;
            case 2: ChickGold(); break;
            case 3: ChickFood(); break;
            case 4: ChickMana(); break;
            case 5: ChickWood(); break;
            case 6: ChickIron(); break;
            default:
                break;
        }
    }

    private void ChickAll()
    {
        screenRole = HallRoleMgr.instance.ScreenRole();
        ShowAllRole(screenRole, RoleAttribute.Max);
    }
    private void ChickAtk()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Fight);
        ShowAllRole(screenRole, RoleAttribute.Fight);
    }
    private void ChickGold()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Gold);
        ShowAllRole(screenRole, RoleAttribute.Gold);
    }
    private void ChickFood()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Food);
        ShowAllRole(screenRole, RoleAttribute.Food);
    }
    private void ChickMana()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Mana);
        ShowAllRole(screenRole, RoleAttribute.Mana);
    }
    private void ChickWood()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Wood);
        ShowAllRole(screenRole, RoleAttribute.Wood);
    }
    private void ChickIron()
    {
        screenRole = HallRoleMgr.instance.ScreenRole(RoleAttribute.Iron);
        ShowAllRole(screenRole, RoleAttribute.Iron);
    }

    private void ShowAllRole(List<HallRoleData> AllRole, RoleAttribute needAtr)
    {
        ChickGrid(AllRole.Count);
        for (int i = 0; i < AllRole.Count; i++)
        {
            RoleGrid[i].gameObject.SetActive(true);
            RoleGrid[i].UpdateInfo(AllRole[i], needAtr);
        }
        for (int i = AllRole.Count; i < RoleGrid.Count; i++)
        {
            RoleGrid[i].gameObject.SetActive(false);
        }
    }

    private void ChickGrid(int count)
    {
        if (RoleGrid.Count < count)
        {
            for (int i = RoleGrid.Count; i < count; i++)
            {
                GameObject go = Resources.Load("UIPrefab/UIScreenRoleGrid") as GameObject;
                go = Instantiate(go, gridPoint) as GameObject;
                UIScreenRoleGrid data = go.GetComponent<UIScreenRoleGrid>();
                RoleGrid.Add(data);
            }
        }
    }
    public override void Hide(bool needAnim = true)
    {
        rt.DOAnchorPos(Vector3.down * 500, 0.5f).OnComplete(() => gameObject.SetActive(false));
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
        rt.anchoredPosition = Vector3.down * 500;
        rt.DOAnchorPos(Vector3.zero, 0.5f);
    }
}
