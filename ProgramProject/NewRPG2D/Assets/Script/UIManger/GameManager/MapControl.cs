using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class MapControl : MonoBehaviour
{
    public static MapControl instance;
    public GameObject MainMap;
    public GameObject EditMap;
    public CameraControl mainCamera;
    public MainCastle mainCastle;
    public EditCastle editCastle;
    public CastleType type;//当前建筑模式
    public List<BuildTip> removeRoom = new List<BuildTip>();
    public List<BuildTip> allTips = new List<BuildTip>();
    public GameObject buildTip;//提示框
    public Transform TipPoint;//提示框位置
    public Transform RemoveRoomPoint;//删除的房间位置

    private Camera cam;
    private Vector3 SaveCameraPoint;
    private bool first = true;

    private void Awake()
    {
        cam = Camera.main;
        instance = this;
        ShowMainMap();
    }
    public void ShowMainMap()
    {
        type = CastleType.main;
        MainMap.SetActive(true);
        EditMap.SetActive(false);
        SaveCameraPoint = cam.transform.localPosition;
        cam.transform.parent = MainMap.transform;
        cam.transform.localPosition = SaveCameraPoint;
        if (EditCastle.instance == null)
        {
            return;
        }
        if (!first)
        {
            first = false;
            UIPanelManager.instance.ShowPage<UIMain>();
            UIPanelManager.instance.ClosePage<UIEditMode>();
            EditCastle.instance.RemoveAllRoom(false);
        }
    }
    public void ShowEditMap()
    {
        type = CastleType.edit;
        EditMap.SetActive(true);
        SaveCameraPoint = cam.transform.localPosition;
        cam.transform.parent = EditMap.transform;
        cam.transform.localPosition = SaveCameraPoint;
        UIPanelManager.instance.ClosePage<UIMain>();
        UIPanelManager.instance.ShowPage<UIEditMode>();
        CameraControl.instance.CloseRoomLock();
        CameraControl.instance.RefreshRoomLock(null);
        EditCastle.instance.ResetEditRoom();
        //if (first == true)
        //{
        //    EditCastle.instance.ResetEditRoom();
        //    return;
        //}
        //EditCastle.instance.ShowMainMapRoom();
    }

    public bool BuildingRoomTip(BuildingData data, List<EmptyPoint> point, Castle castle, int index, int i)
    {
        //如果提示框不足 新建
        if (allTips.Count <= index)
        {
            GameObject go = Instantiate(buildTip, TipPoint) as GameObject;
            BuildTip buidTip = go.GetComponentInChildren<BuildTip>();
            allTips.Add(buidTip);
        }
        bool isRead = allTips[index].UpdateTip(point[i], point, data, castle);
        return isRead;
    }

    /// <summary>
    /// 重置建筑生成提示框
    /// </summary>
    public void ResetRoomTip()
    {
        List<BuildTip> allTips = MapControl.instance.allTips;
        for (int i = 0; i < allTips.Count; i++)
        {
            allTips[i].RestartThisTip();
            allTips[i].transform.position = new Vector2(-1000, -1000);
        }
    }

    /// <summary>
    /// 清除多余的提示框
    /// </summary>
    /// <param name="index"></param>
    public void ResetRoomTip(int index)
    {
        for (int i = index; i < allTips.Count; i++)
        {
            allTips[i].transform.position = new Vector2(-1000, -1000);
        }
    }

    public void RemoveRoom(RoomMgr mgr)
    {
        BuildTip tip = mgr.GetComponent<BuildTip>();
        tip.transform.parent = RemoveRoomPoint;
        tip.transform.localPosition = Vector3.zero;
        removeRoom.Add(tip);
        Destroy(mgr);



    }
}
