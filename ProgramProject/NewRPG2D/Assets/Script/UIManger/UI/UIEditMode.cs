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

    public List<RoomMgr> rooms;
    public RoomMgr selectRoom;

    private void Awake()
    {
        btn_Remove.gameObject.SetActive(false);
        btn_split.gameObject.SetActive(false);
        rooms = new List<RoomMgr>();
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.EditMode, ShowMenu);
        //HallEventManager.instance.AddListener<List<RoomMgr>>(HallEventDefineEnum.EditMode, ShowBuildingList);
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.EditMode, ShowMenu);
        //HallEventManager.instance.RemoveListener<List<RoomMgr>>(HallEventDefineEnum.EditMode, ShowBuildingList);
    }

    // Use this for initialization
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

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 显示菜单选项
    /// </summary>
    private void ShowMenu(RoomMgr data)
    {
        selectRoom = data;
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

    private void ChickSplit()
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

    }
    private void ChickBack()
    {
        //返回城堡 不恢复操作
    }
    private void ChickSave()
    {
        //保存并返回主城堡 保存
    }
    private void ChickRepair()
    {
        //恢复至远城堡形象
    }
    private void ChickClearAll()
    {
        //清除所有建筑
    }
    private void ChickClearType()
    {
        //清除模式 清除点击的建筑
    }
}
