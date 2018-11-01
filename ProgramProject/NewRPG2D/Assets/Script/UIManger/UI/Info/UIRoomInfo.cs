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

    public RectTransform pageDownTip;

    private int currentBtn = 0;

    protected RoomMgr roomData;

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
    protected virtual void UpdateName(RoomMgr data)
    {
        txt_Name.text = data.BuildingData.RoomName.ToString();
        txt_Level.text = data.BuildingData.Level.ToString();
    }

    /// <summary>
    /// 检查角色卡数量
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="roleGrids"></param>
    protected virtual void ChickRoleNumber<T>(List<T> roleGrids)
    {
        int index = roomData.BuildingData.RoomRole - roleGrids.Count;
        if (index > 0) //证明已有角色卡数量不足
        {
            for (int i = 0; i < index; i++)
            {
                GameObject go = Instantiate(roleGrid, roleTrans) as GameObject;
                T grid = go.GetComponent<T>();
                roleGrids.Add(grid);
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
        pageDownTip.DOAnchorPos(Vector3.up * 500, 0.5f);
    }
    protected virtual void DownPageAnimGoBack()
    {
        pageDownTip.DOAnchorPos(Vector3.zero, 0.5f);
    }

    protected abstract void UpdateInfo(RoomMgr roomMgr);
    protected virtual void ChickClose()
    {
        System.Type type = GetType();
        UIPanelManager.instance.ClosePage(type);
    }
}
