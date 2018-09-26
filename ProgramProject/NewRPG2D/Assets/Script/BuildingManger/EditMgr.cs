using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditMgr : CastleMgr
{
    private CastleMgr mgr;
    public List<ServerBuildData> serverRoom;

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

    /// <summary>
    /// 切换到建造模式
    /// </summary>
    /// <param name="mgr"></param>
    public void UpdateEditCastle(CastleMgr mgr)
    {
        this.mgr = mgr;
        serverRoom = LocalServer.instance.ServerRoom;
        for (int i = 0; i < serverRoom.Count; i++)
        {
            InstanceRoom(serverRoom[i]);
        }
    }

    /// <summary>
    /// 恢复城堡
    /// </summary>
    public void UpdateEditCastle()
    {
        if (mgr == null)
        {
            Debug.LogError("需要重置却没有获取到内容");
            return;
        }
        ClearAllRoom();
        serverRoom = LocalServer.instance.ServerRoom;
        if (serverRoom.Count > 0)
        {
            for (int i = 0; i < serverRoom.Count; i++)
            {
                InstanceRoom(serverRoom[i]);
            }
        }

    }

    /// <summary>
    /// 清空所有房间
    /// </summary>
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

    /// <summary>
    /// 保存房间
    /// </summary>
    public void ChickBuilding()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].linkType == false)
            {
                //有建筑无法连通，保存失败
                Debug.Log("有建筑无法连通，保存失败");
                return;
            }
        }
        serverRoom = LocalServer.instance.ServerRoom;
        List<ServerBuildData> merge = new List<ServerBuildData>();
        List<RoomMgr> room = new List<RoomMgr>();

        //寻找更改的房间 将服务器上房间数据进行修改
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int j = 0; j < serverRoom.Count; j++)
            {
                if (rooms[i].buildingData == serverRoom[j].buildingData)
                {
                    serverRoom[j].buildingPoint = rooms[i].s_Data.buildingPoint;
                    break;
                }
                if (j == serverRoom.Count - 1)
                {

                }
            }
        }
        LocalServer.instance.GetNewRoom(serverRoom);
    }

    /// <summary>
    /// 检查建造状态的建筑合并
    /// </summary>
    /// <param name="data"></param>
    public override void ChickMergeRoom(RoomMgr data)
    {
        if (data.buildingData.MergeID == 0)
        {
            return;
        }
        for (int i = 0; i < data.nearbyRoom.Length; i++)
        {
            //如果他们名字相同且等级一致 那么可以升级
            if (data.nearbyRoom[i] == null)
            {
                break;
            }
            if (data.RoomName == data.nearbyRoom[i].RoomName
                && data.buildingData.Level == data.nearbyRoom[i].buildingData.Level)
            {
                BuildingData b_data = new BuildingData();
                //如果这是个可以合并的组合
                if (data.buildingData.RoomSize < data.nearbyRoom[i].buildingData.RoomSize)
                {
                    b_data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.nearbyRoom[i].buildingData.MergeID);
                }
                else
                {
                    b_data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.buildingData.MergeID);
                }
                ServerBuildData s_data = new ServerBuildData();
                //判断谁在左边
                if (data.nearbyRoom[i].buidStartPoint.x < data.buidStartPoint.x)
                {
                    Debug.Log("对方靠左");
                    //和对方合并
                    s_data = new ServerBuildData(data.nearbyRoom[i].buidStartPoint, b_data);
                }
                else
                {
                    Debug.Log("我方靠左");
                    s_data = new ServerBuildData(data.buidStartPoint, b_data);
                }
                data.nearbyRoom[i].RemoveBuilding();//移除该房间
                data.RemoveBuilding();//移除该房间
                InstanceRoom(s_data);
            }
        }
    }
}

/*
 * 建造模式 利用服务器本地服务器储存的房间序号进行操作
 * 
 * 
 */
