using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCastle : Castle
{
    public static EditCastle instance;
    public List<LocalBuildingData> editAllBuilding = new List<LocalBuildingData>();
    public List<LocalBuildingData> allRemoveRoom = new List<LocalBuildingData>();
    public List<EditMergeRoomData> allMergeRoom = new List<EditMergeRoomData>();
    private LocalBuildingData currentLocalData;
    private void Awake()
    {
        instance = this;
        Init();
    }

    private void OnEnable()
    {
        ShowMainMapRoom();
    }

    /// <summary>
    /// 显示所有房间
    /// </summary>
    public void ShowMainMapRoom()
    {
        List<LocalBuildingData> AllBuilding = ChickPlayerInfo.instance.GetAllBuilding();

        for (int i = 0; i < AllBuilding.Count; i++)
        {
            for (int j = 0; j < editAllBuilding.Count; j++)
            {
                if (AllBuilding[i].id == editAllBuilding[i].id)
                {
                    break;
                }
            }
            allRemoveRoom.Add(AllBuilding[i]);
        }
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
            editAllBuilding.Add(AllBuilding[i]);
            InstanceRoom(editAllBuilding[i]);
        }
    }

    /// <summary>
    /// 建造模式新建房间
    /// </summary>
    /// <param name="data"></param>
    public override void AddBuilding(LocalBuildingData data)
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
        currentLocalData.buildingPoint = startPoint;
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
    }

    public void RemoveAllRoom()
    {
        for (int i = 0; i < allroom.Count; i++)
        {
            allroom[i].RemoveBuilding();
            allroom[i].EditRemoveBuilding();
            editAllBuilding.Clear();
        }
    }

    public void ChickMergeRoom(LocalBuildingData data_1, LocalBuildingData data_2, LocalBuildingData data_3)
    {
        editAllBuilding.Remove(data_1);
        editAllBuilding.Remove(data_2);
        editAllBuilding.Add(data_3);

        //查找 有没有合并结果等于本次合并单元的
        for (int i = 0; i < allMergeRoom.Count; i++)
        {
            if (allMergeRoom[i].mergeRoom.id == data_1.id)
            {
                allMergeRoom[i].room_3 = data_2;
                allMergeRoom[i].mergeRoom = data_3;
                Debug.Log("该房间合并过的");
                return;
            }
            if (allMergeRoom[i].mergeRoom.id == data_2.id)
            {
                allMergeRoom[i].room_3 = data_1;
                allMergeRoom[i].mergeRoom = data_3;
                Debug.Log("该房间合并过的");
                return;
            }
        }

        //匹配建造信息
        EditMergeRoomData data = new EditMergeRoomData();
        data.room_1 = data_1;
        data.room_2 = data_2;
        data.mergeRoom = data_3;
        allMergeRoom.Add(data);
        Debug.Log("该房间没有合并过");
    }
    public void FindMergeRoom(RoomMgr data_1, RoomMgr data_2)
    {
        int index = editAllBuilding.IndexOf(data_1.currentBuildData);
        if (index > -1)
        {
            UIEditMode.instance.ChickRemove(data_1);
        }
        index = editAllBuilding.IndexOf(data_2.currentBuildData);
        if (index > -1)
        {
            UIEditMode.instance.ChickRemove(data_2);
        }
    }
}
