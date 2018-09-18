using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMgr : CastleMgr
{
    private CastleMgr mgr;

    private void Start()
    {
        HallEventManager.instance.AddListener(HallEventDefineEnum.EditMgr, UpdateEditCastle);
        HallEventManager.instance.AddListener(HallEventDefineEnum.ClearAllRoom, ClearAllRoom);
        HallEventManager.instance.AddListener(HallEventDefineEnum.ChickBuild, ChickBuilding);

        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.AddBuild, AddRoom);


    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.EditMgr, UpdateEditCastle);
        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ChickBuild, ChickBuilding);

        HallEventManager.instance.RemoveListener(HallEventDefineEnum.ClearAllRoom, ClearAllRoom);
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.AddBuild, AddRoom);
    }
    public void UpdateEditCastle(CastleMgr mgr)
    {
        this.mgr = mgr;
        if (rooms != null && mgr.serverRoom.Count > 0)
        {
            for (int i = 0; i < mgr.serverRoom.Count; i++)
            {
                InstanceRoom(mgr.serverRoom[i]);
            }
        }
    }
    public void UpdateEditCastle()
    {
        if (mgr == null)
        {
            Debug.LogError("需要重置却没有获取到内容");
            return;
        }
        ClearAllRoom();
        if (rooms != null && mgr.serverRoom.Count > 0)
        {
            for (int i = 0; i < mgr.serverRoom.Count; i++)
            {
                InstanceRoom(mgr.serverRoom[i]);
            }
        }

    }

    public void ClearAllRoom()
    {
        for (int i = rooms.Count - 1; i >= 0; i--)
        {
            Debug.Log(0);
            HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.ClearAllRoom, rooms[i]);
            rooms[i].RemoveBuilding(buildPoint);
        }
    }

    public void AddRoom(RoomMgr room)
    {
        BuildRoomTip(room.buildingData);
    }

    public void ChickBuilding()
    {
        HallEventManager.instance.SendEvent<List<RoomMgr>>(HallEventDefineEnum.ChickBuild, rooms);
    }
}
