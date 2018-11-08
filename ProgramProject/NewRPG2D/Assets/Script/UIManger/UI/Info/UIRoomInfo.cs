using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using DG.Tweening;
using UnityEngine.EventSystems;

public abstract class UIRoomInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_Level;
    public GameObject roleGrid;
    public Transform roleTrans;
    public Button btn_Close;
    public Button btn_Close_1;

    public Text txt_DownTip;
    public RectTransform pageDownTip;

    protected Button[] btn_ScreenRole;
    [System.NonSerialized]
    public RoomMgr roomData;
    protected int currentBtn = 0;
    private bool isShow = false;

    public bool IsShow
    {
        get
        {
            return isShow;
        }

        set
        {
            bool temp = value;
            if (isShow != temp)
            {
                isShow = value;
                if (isShow)
                {
                    pageDownTip.DOAnchorPos(Vector2.up * 500, 0.5f);
                    UIPanelManager.instance.ShowPage<UIScreenRole>(this);
                }
                else
                {
                    btn_ScreenRole[currentBtn].interactable = true;
                    pageDownTip.DOAnchorPos(Vector2.zero, 0.5f);
                }
            }
        }
    }

    protected virtual void Awake()
    {
        btn_Close.onClick.AddListener(ChickClose);
        if (btn_Close_1 != null)
        {
            btn_Close_1.onClick.AddListener(ChickClose);
        }
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
        UpdateInfo(roomData);
        UpdateName(roomData);
    }

    /// <summary>
    /// 刷新房间名称和等级
    /// </summary>
    /// <param name="data"></param>
    protected virtual void UpdateName(RoomMgr data, bool NeedTip = true)
    {
        string name = data.BuildingData.RoomName.ToString();
        txt_Name.text = LanguageDataMgr.instance.GetRoomName(name);
        txt_Level.text = data.BuildingData.Level.ToString();
        if (NeedTip)
        {
            txt_DownTip.text = LanguageDataMgr.instance.GetInfoDownTip(name);
        }
    }

    /// <summary>
    /// 检查角色卡数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="roleGrids"></param>
    protected virtual int ChickRoleNumber<T>(List<T> roleGrids)
    {
        int index = roomData.BuildingData.RoomRole;
        btn_ScreenRole = new Button[index];
        int num = 0;//角色数量
        for (int i = 0; i < index; i++)
        {
            if (roleGrids.Count == i)
            {
                GameObject go = Instantiate(roleGrid, roleTrans) as GameObject;
                T grid = go.GetComponent<T>();
                roleGrids.Add(grid);
            }
            UIRoleGridMgr mgr = roleGrids[i] as UIRoleGridMgr;
            if (roomData.currentBuildData.roleData[i] != null)
            {
                mgr.UpdateInfo(roomData.currentBuildData.roleData[i], this);
                num++;
            }
            else
            {
                mgr.UpdateInfo(this);
            }
            btn_ScreenRole[i] = mgr.btn_ScreenRole;
            btn_ScreenRole[i].onClick.AddListener(ChickShowScreenRole);
        }
        return num;
    }

    protected virtual void ChickShowScreenRole()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        for (int i = 0; i < btn_ScreenRole.Length; i++)
        {
            if (btn_ScreenRole[i].gameObject == go)
            {
                btn_ScreenRole[currentBtn].interactable = true;
                btn_ScreenRole[i].interactable = false;
                currentBtn = i;
                if (!IsShow)
                {
                    IsShow = true;
                }
                return;
            }
        }
    }

    public virtual void RoomAddRole(HallRole role, int oldRoleId)
    {
        roomData.AddRole(role, oldRoleId);
        UpdateInfo(roomData);
    }



    protected virtual void DownPageAnimStart()
    {
        pageDownTip.DOAnchorPos(Vector2.up * 500, 0.5f);
    }
    protected virtual void DownPageAnimGoBack()
    {
        pageDownTip.DOAnchorPos(Vector2.zero, 0.5f);
    }

    protected abstract void UpdateInfo(RoomMgr roomMgr);
    protected virtual void ChickClose()
    {
        System.Type type = GetType();
        UIPanelManager.instance.ClosePage(type);
        if (isShow == true)
        {
            isShow = false;
            UIPanelManager.instance.ClosePage<UIScreenRole>();
            btn_ScreenRole[currentBtn].interactable = true;
            pageDownTip.anchoredPosition = Vector2.zero;
        }
    }
}
