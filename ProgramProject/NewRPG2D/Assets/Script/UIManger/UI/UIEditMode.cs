using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;

public class UIEditMode : TTUIPage
{
    public static UIEditMode instance;

    //条件相同的存在一起 等级名称大小一致且不在升级中
    public List<EditModeHelper> rooms = new List<EditModeHelper>();

    public Button btn_back;//后退
    public Button btn_save;//保存
    public Button btn_repair;//修福城堡
    public Button btn_clearAll;//清除所有建筑
    public Button btn_clearType;//清除模式
    public Button btn_Remove;//移除
    public Button btn_split;//拆分
    public Text txt_ClearType;//清除模式文字提示
    public Transform Content;//建筑框创建地址
    public List<UIEditRoomGrid> roomGrid;//被删除的房间 放置到下方列表
    private RoomMgr selectRoom;//记录当前指定的房间
    private bool removeType = false;//删除模式

    private void Awake()
    {
        instance = this;

        btn_Remove.gameObject.SetActive(false);
        btn_split.gameObject.SetActive(false);
        HallEventManager.instance.AddListener<LocalBuildingData>(HallEventDefineEnum.AddBuild, RemoveBuildingList);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<LocalBuildingData>(HallEventDefineEnum.AddBuild, RemoveBuildingList);
    }

    void Start()
    {
        btn_back.onClick.AddListener(ChickBack);
        btn_save.onClick.AddListener(ChickSave);
        btn_repair.onClick.AddListener(ChickRepair);
        btn_clearAll.onClick.AddListener(ChickClearAll);
        btn_clearType.onClick.AddListener(ChickClearType);
        btn_Remove.onClick.AddListener(ChickRemove);
        btn_split.onClick.AddListener(ChickSplit);
    }

    private void OnEnable()
    {
        removeType = true;
        ChickClearType();
    }

    /// <summary>
    /// 显示菜单选项
    /// </summary>
    public void ShowMenu(RoomMgr data)
    {
        selectRoom = data;
        if (removeType == true && data != null)
        {
            ChickRemove();
            return;
        }
        if (data != null)
        {
            if (data.BuildingData.SplitID != 0 && data.ConstructionType == false)
                btn_split.gameObject.SetActive(true);
            else
                btn_split.gameObject.SetActive(false);

            btn_Remove.gameObject.SetActive(true);
        }
        else
        {
            btn_Remove.gameObject.SetActive(false);
            btn_split.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 拆分
    /// </summary>
    private void ChickSplit()
    {
        int index = selectRoom.BuildingData.RoomSize / 3;
        BuildingData data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(selectRoom.BuildingData.SplitID);
        List<EditMergeRoomData> mergeRoom = EditCastle.instance.allMergeRoom;
        EditCastle.instance.RemoveRoom(selectRoom);
        //查找合并表内是否有改建筑 如果有 释放
        for (int i = 0; i < mergeRoom.Count; i++)
        {
            if (mergeRoom[i].mergeRoom.id == selectRoom.currentBuildData.id)
            {
                ReleaseRoom(mergeRoom[i].room_1);
                ReleaseRoom(mergeRoom[i].room_2);
                ReleaseRoom(mergeRoom[i].room_3);
                ShowMenu(null);
                return;
            }
        }
        Debug.Log(index);
        for (int i = 0; i < index; i++)
        {
            int id = ChickPlayerInfo.instance.buildingIdIndex++;
            LocalBuildingData s_data = new LocalBuildingData(id, Vector2.zero, data);
            ListAddData(s_data);
        }
        ShowMenu(null);
    }

    /// <summary>
    /// 拆分时发现曾合并过则拆分
    /// </summary>
    /// <param name="data"></param>
    private void ReleaseRoom(LocalBuildingData data)
    {
        if (data == null)
        {
            return;
        }
        ListAddData(data);
    }

    private void ChickRemove()
    {
        ListAddData(selectRoom.currentBuildData);
        EditCastle.instance.RemoveRoom(selectRoom);
        ShowMenu(null);
    }

    public void ChickRemove(RoomMgr room)
    {
        ListAddData(room.currentBuildData);
        EditCastle.instance.RemoveRoom(room);
        ShowMenu(null);
    }


    private void ChickBack()
    {
        //返回城堡 不恢复操作
        MapControl.instance.ShowMainMap();
        UIPanelManager.instance.ClosePage<UIEditMode>();
        UIPanelManager.instance.ShowPage<UIMain>();
    }

    private void ChickSave()
    {
        //保存并返回主城堡 保存
        //若有断开 保存失败
        if (rooms.Count > 0)
        {
            //有未建造建筑 保存失败
            return;
        }
        HallEventManager.instance.SendEvent(HallEventDefineEnum.ChickBuild);
    }

    private void ChickRepair()
    {
        //关闭锁定状态
        CameraControl.instance.CloseRoomLock();
        //恢复至原城堡形象
        HallEventManager.instance.SendEvent(HallEventDefineEnum.EditMgr);
        rooms.Clear();//清除被删除的房间信息

    }
    private void ChickClearAll()
    {
        ShowMenu(null);

        //关闭锁定状态
        CameraControl.instance.CloseRoomLock();

        //清除所有建筑
        EditCastle.instance.ResetEditRoom();
    }
    private void ClearCallBack(LocalBuildingData data)
    {
        ListAddData(data);
    }
    private void ChickClearType()
    {
        ShowMenu(null);

        CameraControl.instance.CloseRoomLock();
        //清除模式 清除点击的建筑
        removeType = !removeType;
        if (removeType == true)
        {
            txt_ClearType.text = "关闭清除模式";
        }
        else
        {
            txt_ClearType.text = "清除模式";
        }
    }

    //链表中删除消息
    private void RemoveBuildingList(LocalBuildingData data)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].buildingData[0].id == data.id)
            {
                rooms[i].buildingData.RemoveAt(0);
                if (rooms[i].number <= 0)
                {
                    //把删除的那个元素调到最后一位格子也移动到最后
                    roomGrid[i].gameObject.SetActive(false);
                    roomGrid[i].transform.SetSiblingIndex(transform.parent.childCount - 1);
                    UIEditRoomGrid temp = roomGrid[i];
                    roomGrid.RemoveAt(i);
                    roomGrid.Add(temp);
                    rooms.RemoveAt(i);
                    return;
                }
                roomGrid[i].ChickNumber();
                return;
            }
        }
        Debug.LogError("没有找到需要删除的信息");
    }

    /// <summary>
    /// 下方列表添加被删除的建筑信息
    /// </summary>
    /// <param name="s_data"></param>
    private void ListAddData(LocalBuildingData s_data)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (s_data.ConstructionType == true)
            {
                break;
            }
            if (rooms[i].buildingData[0].buildingData.RoomName == s_data.buildingData.RoomName
                && rooms[i].buildingData[0].buildingData.Level == s_data.buildingData.Level
                && rooms[i].buildingData[0].buildingData.RoomSize == s_data.buildingData.RoomSize
                && s_data.ConstructionType == false && rooms[i].buildingData[0].ConstructionType == false)
            {
                rooms[i].buildingData.Add(s_data);
                roomGrid[i].ChickNumber();
                return;
            }
        }
        Debug.Log("没有找到同类 添加新的");
        rooms.Add(new EditModeHelper(s_data));
        ChickRoomGrid();
        roomGrid[rooms.Count - 1].gameObject.SetActive(true);
        roomGrid[rooms.Count - 1].UpdateInfo(rooms[rooms.Count - 1]);
    }

    /// <summary>
    /// 寻找升级中的房间并将信息更改
    /// </summary>
    /// <param name="data"></param>
    public void FindLevelUpData(LocalBuildingData data)
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].buildingData[0].id == data.id)
            {
                Debug.Log("等级" + rooms[i].buildingData[0].buildingData.Level);
                //把删除的那个元素调到最后一位格子也移动到最后
                //rooms[i].buildingData[0] = data;//直接把数据转变不合并或移动格子
            }
        }
    }

    /// <summary>
    /// 刷新下方列表信息
    /// </summary>
    private void RefreshMenu()
    {
        ChickRoomGrid();
        for (int i = 0; i < roomGrid.Count; i++)
        {
            if (i < rooms.Count)
            {
                roomGrid[i].UpdateInfo(rooms[i]);
            }
            else
            {
                roomGrid[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 检查UI格子是否足够 不足则添加
    /// </summary>
    private void ChickRoomGrid()
    {
        if (roomGrid.Count < rooms.Count)
        {
            int index = rooms.Count - roomGrid.Count;
            for (int i = 0; i < index; i++)
            {
                GameObject go = Resources.Load<GameObject>("UIPrefab/UIEditRoomGrid");
                go = Instantiate(go, Content) as GameObject;
                UIEditRoomGrid RG = go.GetComponent<UIEditRoomGrid>();
                roomGrid.Add(RG);
            }
        }
    }
}

[System.Serializable]
public class EditModeHelper
{
    public List<LocalBuildingData> buildingData;
    public int number
    {
        get { return buildingData.Count; }
    }

    public EditModeHelper(LocalBuildingData data)
    {
        if (buildingData == null)
        {
            buildingData = new List<LocalBuildingData>();
        }
        this.buildingData.Add(data);
    }
}
