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
    public GameObject grid;
    private List<HallRoleData> screenRole = new List<HallRoleData>();
    private List<UIScreenRoleGrid> RoleGrid = new List<UIScreenRoleGrid>();

    public RectTransform rt;
    public ScrollRect sr;

    private int currentBtnIndex = 0;
    private UIRoomInfo info;
    private bool needAnim = false;
    private bool isLevel = true;
    private void Awake()
    {
        instance = this;
        for (int i = 0; i < btnGroup.Length; i++)
        {
            btnGroup[i].onClick.AddListener(ChickBtnClick);
        }
        btn_back.onClick.AddListener(ClosePage);
        btnGroup[currentBtnIndex].interactable = false;

        UpdateInfo();
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        info = mData as UIRoomInfo;
        needAnim = false;
        if (ChickPlayerInfo.instance.ChickProduction(info.roomData.currentBuildData))
            isLevel = false;
        else
            isLevel = true;
        ShowPage();
    }

    private void UpdateInfo()
    {
        int index = ChickPlayerInfo.instance.ChickAtrNumber();
        for (int i = 0; i < btnGroup.Length; i++)
        {
            if (i < index + 2)
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

        for (int i = 0; i < btnGroup.Length; i++)
        {
            if (btnGroup[i].gameObject == go)
            {
                btnGroup[currentBtnIndex].interactable = true;
                btnGroup[i].interactable = false;
                currentBtnIndex = i;
                ChickRoleInfo(i);
                break;
            }
        }
    }

    /// <summary>
    /// 开启面板同时显示的界面 默认显示排列
    /// </summary>
    public void ShowPage()
    {
        ChickRoleInfo(currentBtnIndex);
    }

    private void ChickRoleInfo(int index)
    {
        if (index == 0)
        {
            index = (int)RoleAttribute.Max;
        }
        else
        {
            index += (int)RoleAttribute.Fight - 1;
        }
        screenRole = HallRoleMgr.instance.ScreenRole((RoleAttribute)index, isLevel);
        ShowAllRole(screenRole, (RoleAttribute)index);
    }

    private void ShowAllRole(List<HallRoleData> AllRole, RoleAttribute needAtr)
    {
        ChickGrid(AllRole.Count);
        for (int i = 0; i < AllRole.Count; i++)
        {
            RoleGrid[i].gameObject.SetActive(true);
            RoleGrid[i].UpdateInfo(AllRole[i], needAtr, isLevel);
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
                GameObject go = Instantiate(grid, gridPoint) as GameObject;
                UIScreenRoleGrid data = go.GetComponent<UIScreenRoleGrid>();
                RoleGrid.Add(data);
            }
        }
    }
    public override void Hide(bool Anim = true)
    {
        if (needAnim)
        {
            rt.DOAnchorPos(Vector3.down * 500, 0.5f).OnComplete(() => base.Hide(false));
            return;
        }
        base.Hide(true);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
        rt.anchoredPosition = Vector3.down * 500;
        rt.DOAnchorPos(Vector3.zero, 0.5f);
    }

    public override void ClosePage()
    {
        needAnim = true;
        info.IsShow = false;
        base.ClosePage();
    }
}
