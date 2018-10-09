using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public abstract class UIRoomInfo : TTUIPage
{
    public Text txt_Name;
    public Text txt_Level;
    public GameObject roleGrid;
    public Transform roleTrans;

    protected RoomMgr roomData;
    public override void Show(object mData)
    {
        base.Show(mData);
        roomData = mData as RoomMgr;
        UpdateInfo(roomData);
        UpdateName(roomData);
    }

    protected virtual void UpdateName(RoomMgr data)
    {
        txt_Name.text = data.BuildingData.RoomName.ToString();
        txt_Level.text = data.BuildingData.Level.ToString();
    }

    protected abstract void UpdateInfo(RoomMgr roomMgr);

    protected void ChickRoleNumber<T>(List<T> roleGrids)
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

}
