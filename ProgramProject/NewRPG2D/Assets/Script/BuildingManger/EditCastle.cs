using Assets.Script.UIManger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditCastle : Castle
{
    public static EditCastle instance;
    public Dictionary<BuildRoomName, List<LocalBuildingData>> editAllBuilding = new Dictionary<BuildRoomName, List<LocalBuildingData>>();
    public List<LocalBuildingData> allRemoveRoom = new List<LocalBuildingData>();
    private LocalBuildingData currentLocalData;


    private void Awake()
    {
        instance = this;
        Init();
    }

    public void EditSave()
    {
        #region 检查连接状态
        foreach (var rooms in editAllBuilding)
        {
            for (int i = 0; i < rooms.Value.Count; i++)
            {
                if (rooms.Value[i].currentRoom.linkType == false)
                {
                    object st = "请检查建筑连接状态";
                    UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
                    return;
                }
            }
        }
        #endregion
        Debug.Log("上传最新数据给服务器");
    }

    /// <summary>
    /// 刷新所有房间 与主世界对应
    /// </summary>
    public void ResetEditRoom()
    {
        RemoveAllRoom(false);
        editAllBuilding.Clear();
        allRemoveRoom.Clear();
        Dictionary<BuildRoomName, List<LocalBuildingData>> AllBuilding = BuildingManager.instance.AllBuildingData;
        foreach (var datas in AllBuilding)
        {
            editAllBuilding.Add(datas.Key, new List<LocalBuildingData>());
            for (int i = 0; i < datas.Value.Count; i++)
            {
                LocalBuildingData data = new LocalBuildingData(datas.Value[i].id, datas.Value[i].buildingPoint, datas.Value[i].buildingData, datas.Value[i].ConstructionType);
                editAllBuilding[datas.Key].Add(data);
                InstanceRoom(data);
            }
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
    public override void ChickRaycast(Collider2D hit)
    {
        //生成建筑 建筑去计算他附近的空位 那么需要知道他自身的起点坐标 上下左右坐标
        BuildTip tip = hit.GetComponent<BuildTip>();
        Vector2 startPoint = new Vector2(tip.startX, tip.emptyPoint.startPoint.y);
        currentLocalData.buildingPoint = startPoint;
        Debug.Log("添加房间");

        if (!editAllBuilding.ContainsKey(currentLocalData.buildingData.RoomName))
        {
            editAllBuilding.Add(currentLocalData.buildingData.RoomName, new List<LocalBuildingData>());
        }

        editAllBuilding[currentLocalData.buildingData.RoomName].Add(currentLocalData);
        RoomMgr room = InstanceRoom(currentLocalData);
        //删除当前已使用空位
        allEmptyPoint.Remove(tip.emptyPoint);
        //将所有标签移出屏幕
        MapControl.instance.ResetRoomTip();
        //通知UI房间建设成功
        HallEventManager.instance.SendEvent<LocalBuildingData>(HallEventDefineEnum.AddBuild, currentLocalData);
        UIEditMode.instance.ShowCloseBG(false);
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
        allroom.Clear();
        editAllBuilding.Clear();
        allEmptyPoint.Clear();
    }

    public override void MergeRoom(RoomMgr room_1, RoomMgr room_2, LocalBuildingData mergeData)
    {
        List<LocalBuildingData> allData = editAllBuilding[room_1.currentBuildData.buildingData.RoomName];
        int newID = RoomIdControl.MergeID(room_1.currentBuildData, room_2.currentBuildData);
        mergeData.id = newID;
        allData.Add(mergeData);
        allData.Remove(room_1.currentBuildData);
        allData.Remove(room_2.currentBuildData);
        room_1.RemoveBuilding();
        room_2.RemoveBuilding();
        InstanceRoom(mergeData);
    }
}
