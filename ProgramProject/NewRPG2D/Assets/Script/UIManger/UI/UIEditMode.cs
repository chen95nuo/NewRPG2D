using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using System;

public class UIEditMode : TTUIPage
{
    public Button btn_back;//后退
    public Button btn_save;//保存
    public Button btn_repair;//修福城堡
    public Button btn_clearAll;//清除所有建筑
    public Button btn_clearType;//清除模式
    public Button btn_Remove;//移除
    public Button btn_split;//拆分
    public Text txt_ClearType;//清除模式文字提示
    public Transform Content;//建筑框创建地址
    public List<RoomMgr> rooms;//被删除的房间 放置到下方列表
    public List<UIEditRoomGrid> roomGrid;//被删除的房间 放置到下方列表
    public List<ServerBuildData> serverRoom;//将新位置上传服务器
    private RoomMgr selectRoom;//记录当前指定的房间
    private bool removeType = false;//删除模式

    private void Awake()
    {
        btn_Remove.gameObject.SetActive(false);
        btn_split.gameObject.SetActive(false);
        rooms = new List<RoomMgr>();
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.EditMode, ShowMenu);
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.ClearAllRoom, ClearCallBack);
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.AddBuild, RemoveBuildingList);
        HallEventManager.instance.AddListener<List<RoomMgr>>(HallEventDefineEnum.ChickBuild, RoomSave);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.EditMode, ShowMenu);
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.ClearAllRoom, ClearCallBack);
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.AddBuild, RemoveBuildingList);
        HallEventManager.instance.RemoveListener<List<RoomMgr>>(HallEventDefineEnum.ChickBuild, RoomSave);


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
    private void ShowMenu(RoomMgr data)
    {
        selectRoom = data;
        if (removeType == true && data != null)
        {
            ChickRemove();
            return;
        }
        if (data != null)
        {
            if (data.buildingData.SplitID == 0)
            {
                btn_Remove.gameObject.SetActive(true);
                btn_split.gameObject.SetActive(false);
            }
            else
            {
                btn_Remove.gameObject.SetActive(true);
                btn_split.gameObject.SetActive(true);
            }
        }
        else
        {
            btn_Remove.gameObject.SetActive(false);
            btn_split.gameObject.SetActive(false);
        }
    }

    private void ChickSplit()//拆分
    {

    }

    private void ChickRemove()
    {
        rooms.Add(selectRoom);
        HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.InEditMode, selectRoom);
        ShowMenu(null);
        ShowBuildingList();
    }
    /// <summary>
    /// 显示建筑列表
    /// </summary>
    private void ShowBuildingList()
    {
        Debug.Log("Rooms :" + rooms.Count);
        Debug.Log("RoomGrid :" + roomGrid.Count);
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

        for (int i = 0; i < roomGrid.Count; i++)
        {
            Debug.Log(roomGrid[i].gameObject.activeInHierarchy);
            if (i < rooms.Count)
            {
                roomGrid[i].gameObject.SetActive(true);
                roomGrid[i].txt_name.text = rooms[i].roomType.ToString();
                roomGrid[i].roomMgr = rooms[i];
            }
            else
            {
                roomGrid[i].gameObject.SetActive(false);
            }
        }
    }
    private void ChickBack()
    {
        //返回城堡 不恢复操作
        HallEventManager.instance.SendEvent(HallEventDefineEnum.EditMode);
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
    private void RoomSave(List<RoomMgr> rooms)
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
        serverRoom.Clear();
        //将新位置保存成服务器格式上传服务器
        for (int i = 0; i < rooms.Count; i++)
        {
            Debug.Log("上传服务器");
            serverRoom.Add(new ServerBuildData(rooms[i].buidStartPoint, rooms[i].buildingData));
        }
        LocalServer.instance.GetNewRoom(serverRoom);
    }

    private void ChickRepair()
    {
        HallEventManager.instance.SendEvent(HallEventDefineEnum.CloseRoomLock);
        //恢复至原城堡形象
        HallEventManager.instance.SendEvent(HallEventDefineEnum.EditMgr);
        rooms.Clear();//清除被删除的房间信息
        ShowBuildingList();
    }
    private void ChickClearAll()
    {
        ShowMenu(null);

        HallEventManager.instance.SendEvent(HallEventDefineEnum.CloseRoomLock);

        //清除所有建筑
        HallEventManager.instance.SendEvent(HallEventDefineEnum.ClearAllRoom);
    }
    private void ClearCallBack(RoomMgr room)
    {
        rooms.Add(room);
        ShowBuildingList();
    }
    private void ChickClearType()
    {
        ShowMenu(null);

        HallEventManager.instance.SendEvent(HallEventDefineEnum.CloseRoomLock);
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
    private void RemoveBuildingList(RoomMgr room)
    {
        rooms.Remove(room);
    }
}
