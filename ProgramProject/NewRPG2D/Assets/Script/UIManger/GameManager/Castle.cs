using Assets.Script.Net;
using EasonAstar;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public float h;
    public float w;
    public float high
    {
        get
        {
            if (h == 0)
            {
                h = GameHelper.high;
            }
            return h;
        }
    }//默认最小房间高度
    public float width
    {
        get
        {
            if (w == 0)
            {
                w = GameHelper.width;
            }
            return w;
        }
    }//默认最小房间宽度
    private int buildH = 4;//可变高度
    private int buildW = 17;//可变长度
    public BuildPoint[,] buildPoint;//城堡墙面信息
    public Transform wallStartPoint;//城堡墙体起点位置
    public Transform limitPoint_1;//城堡墙体限制位置
    public int maxLength = 100;
    public int maxWidth = 100;

    public GameObject[] Wall;//墙面

    public Transform WallPoint;//墙面位置
    public Transform buildingPoint;//建筑位置
    public List<RoomMgr> allroom;//全部建筑信息
    public List<EmptyPoint> allEmptyPoint = new List<EmptyPoint>();//所有建筑空位信息

    public BuildingData currentBuilding;//当前需要建造的房间

    public CastleType castleType;

    public BuildCastleBg castleBg;

    private bool isCreateBuilding = false;//当前是否出现建造提示框

    public void Init()
    {
        UpdateBGNumber();

        instanceWall();
    }

    /// <summary>
    /// 更新背景大小
    /// </summary>
    private void UpdateBGNumber()
    {
        buildPoint = new BuildPoint[maxLength, maxWidth];
    }
    public List<Vector2> GetNowWallGrid(Vector2 rolePoint, Vector2 endPoint)
    {

        Vector2 rolePos = WorldToGrid(rolePoint);
        Vector2 endPos = WorldToGrid(endPoint);
        Debug.Log(endPos);
        List<Vector2> v = EasonAstarTest.instance.Find(buildPoint, rolePos, endPos);
        v[v.Count - 1] = endPoint;
        return v;
    }

    /// <summary>
    /// 世界坐标转换网格
    /// </summary>
    private Vector2 WorldToGrid(Vector2 point)
    {
        int widthY = (int)((point.y - wallStartPoint.position.y) / high);
        int widthX = (int)((point.x - wallStartPoint.position.x) / width);
        Vector2 gridPoint = new Vector2(widthX, widthY);
        return gridPoint;
    }

    /// <summary>
    /// 生成背景墙
    /// </summary>
    protected void instanceWall()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        Vector2 buildXY = playerData.Castle;
        buildH = (int)buildXY.y;
        buildW = (int)buildXY.x;
        castleBg.UpdateInfo(buildW, buildH);

        for (int i = 0; i < buildH; i++)
        {
            for (int j = 0; j < buildW; j++)
            {
                if (wallStartPoint.localPosition.x + (width * j) < limitPoint_1.localPosition.x && wallStartPoint.localPosition.y + (high * i) > limitPoint_1.localPosition.y)
                {
                    buildPoint[j, i] = new BuildPoint();
                    buildPoint[j, i].pointType = 0;
                }
                else
                {
                    buildPoint[j, i] = new BuildPoint();
                    buildPoint[j, i].pointType = BuildingType.Wall;
                }
                if (buildPoint[j, i].pointType == BuildingType.Wall)
                {
                    GameObject go = Instantiate(Wall[j % 3], WallPoint) as GameObject;
                    Vector2 point = new Vector2(wallStartPoint.localPosition.x + (width * j), wallStartPoint.localPosition.y + (high * i));
                    go.transform.localPosition = point;
                    buildPoint[j, i].pointWall = go.transform;
                }
            }
        }
    }

    /// <summary>
    /// 重置背景墙数据
    /// </summary>
    protected void ResetWall()
    {
        for (int i = 0; i < buildH; i++)
        {
            for (int j = 0; j < buildW; j++)
            {
                if (buildPoint[j, i].pointWall == null)
                {
                    continue;
                }
                buildPoint[j, i].pointType = BuildingType.Wall;
                buildPoint[j, i].roomMgr = null;
                buildPoint[j, i].tip = null;
                Vector2 point = new Vector2(wallStartPoint.localPosition.x + (width * j), wallStartPoint.localPosition.y + (high * i));
                buildPoint[j, i].pointWall.localPosition = point;
            }
        }
    }

    /// <summary>
    /// 扩建背景墙
    /// </summary>
    public void ExtensionWall(int x, int y)
    {
        buildH = y;
        buildW = x;
        castleBg.UpdateInfo(buildW, buildH);

        for (int i = 0; i < buildH; i++)
        {
            for (int j = 0; j < buildW; j++)
            {
                if (buildPoint[j, i] != null)
                {
                    continue;
                }
                if (wallStartPoint.localPosition.x + (width * j) < limitPoint_1.localPosition.x && wallStartPoint.localPosition.y + (high * i) > limitPoint_1.localPosition.y)
                {
                    buildPoint[j, i] = new BuildPoint();
                    buildPoint[j, i].pointType = 0;
                }
                else
                {
                    buildPoint[j, i] = new BuildPoint();
                    buildPoint[j, i].pointType = BuildingType.Wall;
                }
                if (buildPoint[j, i].pointType == BuildingType.Wall)
                {
                    GameObject go = Instantiate(Wall[j % 3], WallPoint) as GameObject;
                    Vector2 point = new Vector2(wallStartPoint.localPosition.x + (width * j), wallStartPoint.localPosition.y + (high * i));
                    go.transform.localPosition = point;
                    buildPoint[j, i].pointWall = go.transform;
                }
            }
        }
    }

    /// <summary>
    /// 检测当前房间是否可以合并
    /// </summary>
    /// <param name="data">房间信息</param>
    public virtual void ChickMergeRoom(RoomMgr data)
    {
        if (data.BuildingData.MergeID == 0 || data.currentBuildData.ConstructionType == true) //或者在施工中
        {
            return;
        }
        for (int i = 0; i < data.nearbyRoom.Length; i++)
        {
            //如果他们名字相同且等级一致 那么可以升级
            if (data.nearbyRoom[i] == null)
            {
                continue;
            }
            if (data.RoomName == data.nearbyRoom[i].RoomName
                && data.BuildingData.Level == data.nearbyRoom[i].BuildingData.Level
                && data.nearbyRoom[i].ConstructionType == false)
            {
                BuildingData b_data = new BuildingData();
                //如果这是个可以合并的组合
                if (data.BuildingData.RoomSize < data.nearbyRoom[i].BuildingData.RoomSize)
                {
                    b_data = BuildingDataMgr.instance.GetDataByItemId<BuildingData>(data.nearbyRoom[i].BuildingData.MergeID);
                }
                else
                {
                    b_data = BuildingDataMgr.instance.GetDataByItemId<BuildingData>(data.BuildingData.MergeID);
                }
                LocalBuildingData s_data = new LocalBuildingData();
                //判断谁在左边
                if (data.nearbyRoom[i].buidStartPoint.x < data.buidStartPoint.x)
                {
                    Debug.Log("对方靠左");
                    //和对方合并
                    s_data = new LocalBuildingData(data.nearbyRoom[i].buidStartPoint, b_data);
                }
                else
                {
                    Debug.Log("我方靠左");
                    s_data = new LocalBuildingData(data.buidStartPoint, b_data);
                }
                MergeRoom(data, data.nearbyRoom[i], s_data);
            }
        }
    }

    public virtual void MergeRoom(RoomMgr room_1, RoomMgr room_2, LocalBuildingData mergeData)
    {
        //发送合并请求

        //int index = 0;
        //for (int i = 0; i < room_1.currentBuildData.roleData.Length; i++)
        //{
        //    if (room_1.currentBuildData.roleData[i] != null)
        //    {
        //        mergeData.roleData[index] = room_1.currentBuildData.roleData[i];
        //        index++;
        //    }
        //}
        //for (int i = 0; i < room_2.currentBuildData.roleData.Length; i++)
        //{
        //    if (room_2.currentBuildData.roleData[i] != null)
        //    {
        //        mergeData.roleData[index] = room_2.currentBuildData.roleData[i];
        //        index++;
        //    }
        //}
        //room_1.RemoveBuilding();
        //room_2.RemoveBuilding();
        //CheckPlayerInfo.instance.AddBuilding(mergeData);
        //CheckPlayerInfo.instance.RemoveBuilding(room_1.currentBuildData);
        //CheckPlayerInfo.instance.RemoveBuilding(room_2.currentBuildData);
    }

    /// <summary>
    /// 提示框射线检测
    /// </summary>
    public virtual void ChickRaycast(Collider2D hit)
    {
        //发送消息给服务器

        //生成建筑 建筑去计算他附近的空位 那么需要知道他自身的起点坐标 上下左右坐标
        BuildTip tip = hit.GetComponent<BuildTip>();
        List<int> point = new List<int>() { tip.startX, (int)tip.emptyPoint.startPoint.y };
        //Vector2 startPoint = new Vector2(tip.startX, tip.emptyPoint.startPoint.y);
        //LocalBuildingData data = new LocalBuildingData(startPoint, currentBuilding);
        WebSocketManger.instance.Send(NetSendMsg.RQ_CreateNewRoom, (int)currentBuilding.ItemId, point);
        //CheckPlayerInfo.instance.AddBuilding(data);
        //删除当前已使用空位
        //allEmptyPoint.Remove(tip.emptyPoint);
        //将所有标签移出屏幕
        MapControl.instance.ResetRoomTip();
        //CheckPlayerInfo.instance.RoomUseStock(data.buildingData);
        UIMain.instance.CloseMarket();
    }

    /// <summary>
    /// 建筑生成提示框
    /// </summary>
    public void BuildRoomTip(BuildingData data)
    {
        isCreateBuilding = true;
        MapControl.instance.ResetRoomTip();
        currentBuilding = data;
        int index = 0;
        if (allEmptyPoint.Count <= 0)
        {
            EmptyPoint empty = new EmptyPoint(new Vector2(6, 1), 16, 9, null);
            allEmptyPoint.Add(empty);
        }
        for (int i = 0; i < allEmptyPoint.Count; i++)
        {
            if (allEmptyPoint[i].emptyNumber == 0)
            {
                Debug.LogError("有空地址 :" + allEmptyPoint[i].startPoint);
                allEmptyPoint.RemoveAt(i);
            }
            if (allEmptyPoint[i].emptyNumber >= data.RoomSize)
            {
                bool isReat = MapControl.instance.BuildingRoomTip(data, allEmptyPoint, this, index, i);
                if (isReat == true)
                {
                    index++;
                }
            }
        }
        MapControl.instance.ResetRoomTip(index);
    }

    /// <summary>
    /// 生成房间
    /// </summary>
    public RoomMgr InstanceRoom(LocalBuildingData data)
    {
        Debug.Log("建造房间");
        string st = "Build_" + data.buildingData.RoomName;
        Type t = Type.GetType(st);
        if (t == null)
        {
            t = Type.GetType("BuildStandardRoom");
        }
        List<RoomTag> removeRoom = MapControl.instance.removeRoom;
        RoomMgr room = null;
        for (int i = 0; i < removeRoom.Count; i++)
        {
            if (removeRoom[i].Size == data.buildingData.RoomSize)
            {
                Debug.Log("有相同的");
                room = removeRoom[i].gameObject.AddComponent(t) as RoomMgr;
                allroom.Add(room);
                room.gameObject.name = data.buildingData.RoomName.ToString();
                room.transform.parent = buildingPoint;
                room.UpdateInfo(data, this);
                removeRoom.RemoveAt(i);
                return room;
            }
        }
        //for (int i = 0; i < removeRoom.Count; i++)
        //{
        //    if (removeRoom[i].RoomName == data.buildingData.RoomName
        //         && removeRoom[i].BuildingData.RoomSize == data.buildingData.RoomSize)
        //    {
        //        Debug.Log("有相同的  : " + data.buildingData);
        //        RoomMgr room = removeRoom[i];
        //        allroom.Add(room);
        //        room.transform.parent = buildingPoint;

        //        room.UpdateInfo(data, this);

        //        removeRoom.Remove(removeRoom[i]);
        //        return room;
        //    }
        //}
        GameObject go = Resources.Load<GameObject>("UIPrefab/Building/Build_Template_" + (int)(data.buildingData.RoomSize / 3));
        go = Instantiate(go, buildingPoint) as GameObject;
        go.name = data.buildingData.RoomName.ToString();
        room = go.AddComponent(t) as RoomMgr;
        allroom.Add(room);
        room.UpdateInfo(data, this);
        //GameObject go = null;
        //if (data.buildingData.RoomType != RoomType.Production && data.buildingData.RoomName != BuildRoomName.LivingRoom)
        //{
        //    go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName);
        //}
        //else
        //{
        //    switch (data.buildingData.RoomSize)
        //    {
        //        case 3: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName); break;
        //        case 6: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName + "_1"); break;
        //        case 9: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName + "_2"); break;
        //        default:
        //            break;
        //    }
        //}
        //go = Instantiate(go, buildingPoint) as GameObject;
        //go.name = data.buildingData.RoomName.ToString();
        //RoomMgr room_1 = go.GetComponent<RoomMgr>();
        //allroom.Add(room_1);

        //room_1.UpdateInfo(data, this);

        return room;
    }

    public RoomMgr FindRoom(BuildRoomName name)
    {
        for (int i = 0; i < allroom.Count; i++)
        {
            if (allroom[i].RoomName == name)
            {
                return allroom[i];
            }
        }
        Debug.LogError("没有找到房间");
        return null;
    }

}
