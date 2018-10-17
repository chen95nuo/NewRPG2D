using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCastle : Castle
{
    public static EditCastle instance;
    public List<LocalBuildingData> editAllBuilding = new List<LocalBuildingData>();
    public List<LocalBuildingData> allRemoveRoom = new List<LocalBuildingData>();
    public List<LocalBuildingData> ChangeBuilding = new List<LocalBuildingData>();
    private LocalBuildingData currentLocalData;
    private void Awake()
    {
        instance = this;
        Init();
    }

    public void SaveAllBuild()
    {
        ChickPlayerInfo.instance.ChickEditSave(editAllBuilding, ChangeBuilding);
    }

    /// <summary>
    /// 刷新所有房间 与主世界对应
    /// </summary>
    public void ResetEditRoom()
    {
        editAllBuilding.Clear();
        allRemoveRoom.Clear();
        List<LocalBuildingData> AllBuilding = ChickPlayerInfo.instance.GetAllBuilding();
        for (int i = 0; i < AllBuilding.Count; i++)
        {
            LocalBuildingData data = new LocalBuildingData(AllBuilding[i].id, AllBuilding[i].buildingPoint, AllBuilding[i].buildingData, AllBuilding[i].ConstructionType);
            editAllBuilding.Add(data);
            InstanceRoom(data);
        }
    }

    /// <summary>
    /// 建造模式新建房间
    /// </summary>
    /// <param name="data"></param>
    public void AddBuilding(LocalBuildingData data)
    {
        currentLocalData = data;
        BuildRoomTip(data.buildingData);
    }

    /// <summary>
    /// 建造模式射线检测
    /// </summary>
    /// <param name="hit"></param>
    public override void ChickRaycast(RaycastHit hit)
    {
        //生成建筑 建筑去计算他附近的空位 那么需要知道他自身的起点坐标 上下左右坐标
        BuildTip tip = hit.collider.GetComponent<BuildTip>();
        Vector2 startPoint = new Vector2(tip.startX, tip.emptyPoint.startPoint.y);
        if (currentLocalData.id == 0)
        {
            for (int i = 0; i < ChangeBuilding.Count; i++)
            {
                if (ChangeBuilding[i].buildingData.RoomName == currentLocalData.buildingData.RoomName
                    && ChangeBuilding[i].buildingData.RoomSize == currentLocalData.buildingData.RoomSize)
                {
                    currentLocalData = ChangeBuilding[i];
                    ChangeBuilding.RemoveAt(i);
                    break;
                }
            }
        }
        currentLocalData.buildingPoint = startPoint;
        Debug.Log("添加房间");
        editAllBuilding.Add(currentLocalData);
        InstanceRoom(currentLocalData);
        //删除当前已使用空位
        allEmptyPoint.Remove(tip.emptyPoint);
        //将所有标签移出屏幕
        MapControl.instance.ResetRoomTip();
        //通知UI房间建设成功
        HallEventManager.instance.SendEvent<LocalBuildingData>(HallEventDefineEnum.AddBuild, currentLocalData);
    }

    /// <summary>
    /// 建造模式删除房间
    /// </summary>
    /// <param name="room"></param>
    public void RemoveRoom(RoomMgr room)
    {
        room.RemoveBuilding();
        room.EditRemoveBuilding();
        //MapControl.instance.RemoveRoom(room);
    }

    /// <summary>
    /// 删除所有房间
    /// </summary>
    /// <param name="isShow">是否添加下方提示</param>
    public void RemoveAllRoom(bool isShow)
    {
        ResetWall();
        for (int i = 0; i < allroom.Count; i++)
        {
            if (isShow)
            {
                UIEditMode.instance.ChickRemove(allroom[i]);
            }
            MapControl.instance.RemoveRoom(allroom[i]);
            allroom[i].Clear();
        }
        editAllBuilding.Clear();
        allEmptyPoint.Clear();
    }

    public override void MergeRoom(RoomMgr room_1, RoomMgr room_2, LocalBuildingData mergeData)
    {
        #region 添加改动的
        if (room_1.currentBuildData.id > 0)
        {
            ChangeBuilding.Add(room_1.currentBuildData);
        }
        if (room_2.currentBuildData.id > 0)
        {
            ChangeBuilding.Add(room_2.currentBuildData);
        }
        #endregion
        editAllBuilding.Remove(room_1.currentBuildData);
        editAllBuilding.Remove(room_2.currentBuildData);
        room_1.RemoveBuilding();
        room_2.RemoveBuilding();
        Debug.Log("添加房间");
        editAllBuilding.Add(mergeData);
        InstanceRoom(mergeData);
    }
}
