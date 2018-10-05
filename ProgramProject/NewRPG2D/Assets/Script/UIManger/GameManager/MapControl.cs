using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class MapControl : MonoBehaviour
{
    public static MapControl instance;
    public GameObject MainMap;
    public GameObject EditMap;
    public CameraMgr mainCamera;
    public CameraMgr editCamera;
    public MainCastle mainCastle;
    public EditCastle editCastle;
    public CastleType type;//当前建筑模式
    public List<RoomMgr> removeRoom = new List<RoomMgr>();
    public List<BuildTip> allTips = new List<BuildTip>();
    public GameObject buildTip;//提示框
    public Transform TipPoint;//提示框位置

    private bool first = true;

    private void Awake()
    {
        instance = this;
        ShowMainMap();
    }
    public void ShowMainMap()
    {
        type = CastleType.main;
        MainMap.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        EditMap.SetActive(false);
        editCamera.gameObject.SetActive(false);
        mainCamera.transform.localPosition = editCamera.transform.localPosition;
        UIPanelManager.instance.ShowPage<UIMain>();
        UIPanelManager.instance.ClosePage<UIEditMode>();
        EditCastle.instance.RemoveAllRoom();
    }
    public void ShowEditMap()
    {
        type = CastleType.edit;
        EditMap.SetActive(true);
        editCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        editCamera.transform.localPosition = mainCamera.transform.localPosition;
        UIPanelManager.instance.ClosePage<UIMain>();
        UIPanelManager.instance.ShowPage<UIEditMode>();
        HallEventManager.instance.SendEvent<RoomMgr>(HallEventDefineEnum.CloseRoomLock, null);
        if (first == true)
        {
            EditCastle.instance.ResetEditRoom();
            return;
        }
        EditCastle.instance.ShowMainMapRoom();
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
}
