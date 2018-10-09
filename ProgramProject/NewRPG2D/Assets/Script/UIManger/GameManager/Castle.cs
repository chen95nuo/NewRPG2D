using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    public const float high = 10.8f;//默认最小房间高度
    public const float width = 4.77f;//默认最小房间宽度
    private const int buildHigh = 4;//默认最小等级的高度
    private const int buildWidth = 17;//默认最小等级的长度
    private int buildH = 4;//可变高度
    private int buildW = 17;//可变长度
    public BuildPoint[,] buildPoint;//城堡墙面信息
    public Vector2 wallStartPoint;//城堡墙体起点位置
    public Vector2 limitPoint_1;//城堡墙体限制位置
    public int maxLength = 100;
    public int maxWidth = 100;

    public GameObject[] Wall;//墙面

    public Transform WallPoint;//墙面位置
    public Transform buildingPoint;//建筑位置
    public List<RoomMgr> allroom;//全部建筑信息
    public List<EmptyPoint> allEmptyPoint = new List<EmptyPoint>();//所有建筑空位信息

    public BuildingData currentBuilding;//当前需要建造的房间

    public CastleType castleType;

    protected void Init()
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

    /// <summary>
    /// 生成背景墙
    /// </summary>
    public void instanceWall()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        buildH = buildHigh + playerData.MainHallLevel;
        buildW = buildWidth + (playerData.MainHallLevel * 3);

        for (int i = 0; i < buildH; i++)
        {
            for (int j = 0; j < buildW; j++)
            {
                if (wallStartPoint.x + (width * j) < limitPoint_1.x && wallStartPoint.y + (high * i) > limitPoint_1.y)
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
                    Vector2 point = new Vector2(wallStartPoint.x + (width * j), wallStartPoint.y + (high * i));
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
                buildPoint[j, i].pointType = BuildingType.Wall;
                buildPoint[j, i].roomMgr = null;
                buildPoint[j, i].tip = null;
                Vector2 point = new Vector2(wallStartPoint.x + (width * j), wallStartPoint.y + (high * i));
                buildPoint[j, i].pointWall.localPosition = point;
            }
        }
    }

    /// <summary>
    /// 刷新建筑 在编辑模式保存后常规模式需要刷新建筑的位置
    /// </summary>
    /// <param name="allbuilding"></param>
    protected virtual void RefreshBuilding(List<LocalBuildingData> allbuilding)
    {
        ResetWall();
        for (int i = 0; i < allbuilding.Count; i++)
        {
            for (int j = 0; j < allroom.Count; j++)
            {
                if (allbuilding[i].id == allroom[j].Id)
                {
                    allroom[j].BuildingMove(allbuilding[i], this);//移动到位置
                    allroom[j].ChickLeftOrRight(buildPoint);//检查空位信息
                    break;
                }
            }
            //如果没找到对应的那么新建
            AddBuilding(allbuilding[i]);
        }

        //如果房间没找到对应的位置数据 那么删除该房间
        for (int i = 0; i < allroom.Count; i++)
        {
            Vector2 startPoint = allroom[i].buidStartPoint;
            if (buildPoint[(int)startPoint.x, (int)startPoint.y].roomMgr != allroom[i])
            {
                //这里是没有对应数据的房间 可能是被合成或者拆分了
                allroom[i].RemoveBuilding();
                allroom.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 实例化房间 服务器模式
    /// </summary>
    /// <param name="data"></param>
    /// <returns>返回房间ID</returns>
    public virtual int AddBuilding(ServerBuildData data)
    {
        //让本地新建房间 检测位置 让管理器去
        ChickPlayerInfo.instance.buildingIdIndex++;
        return ChickPlayerInfo.instance.buildingIdIndex;
    }
    /// <summary>
    /// 实例化房间 用于合并 直接创建并移动到
    /// </summary>
    /// <param name="data"></param>
    public virtual void AddBuilding(LocalBuildingData data)
    {
        InstanceRoom(data);
    }

    /// <summary>
    /// 实例化房间 用于合并 直接创建并移动到
    /// </summary>
    /// <param name="data"></param>
    public virtual void AddBuilding(LocalBuildingData data, bool isRun)
    {
        InstanceRoom(data, isRun);
    }

    /// <summary>
    /// 实例化房间 Data新建
    /// </summary>
    /// <param name="data"></param>
    /// <returns>返回房间ID</returns>
    public virtual void AddBuilding(Vector2 startPoint)
    {
        ChickPlayerInfo.instance.buildingIdIndex++;
        int size = ChickPlayerInfo.instance.ChickRoomSize(currentBuilding);
        LocalBuildingData localData = new LocalBuildingData(ChickPlayerInfo.instance.buildingIdIndex, startPoint, currentBuilding, size);
        ChickPlayerInfo.instance.AddBuilding(localData);
        InstanceRoom(localData);
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
                    b_data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.nearbyRoom[i].BuildingData.MergeID);
                }
                else
                {
                    b_data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.BuildingData.MergeID);
                }
                LocalBuildingData s_data = new LocalBuildingData();
                //判断谁在左边
                ChickPlayerInfo.instance.buildingIdIndex++;
                if (data.nearbyRoom[i].buidStartPoint.x < data.buidStartPoint.x)
                {
                    Debug.Log("对方靠左");
                    int size = ChickPlayerInfo.instance.ChickRoomSize(b_data);

                    //和对方合并
                    s_data = new LocalBuildingData(ChickPlayerInfo.instance.buildingIdIndex, data.nearbyRoom[i].buidStartPoint, b_data, size);
                }
                else
                {
                    Debug.Log("我方靠左");

                    int size = ChickPlayerInfo.instance.ChickRoomSize(b_data);
                    s_data = new LocalBuildingData(ChickPlayerInfo.instance.buildingIdIndex, data.buidStartPoint, b_data, size);
                }
                //这边要匹配是不是编辑模式
                switch (castleType)
                {
                    case CastleType.main:
                        ChickPlayerInfo.instance.MergeRoom(data.currentBuildData, data.nearbyRoom[i].currentBuildData, s_data);
                        //如果主场景房间升级或建造成功造成房间合并
                        if (MapControl.instance.type == CastleType.edit)
                        {
                            //如果可以合并但是模式是建造模式 那么通知建造模式删除该建筑并合并该建筑
                            EditCastle.instance.FindMergeRoom(data, data.nearbyRoom[i]);
                        }
                        break;
                    case CastleType.edit:
                        EditCastle.instance.ChickMergeRoom(data.currentBuildData, data.nearbyRoom[i].currentBuildData, s_data);
                        break;
                    default:
                        break;
                }
                data.nearbyRoom[i].RemoveBuilding();
                data.RemoveBuilding();
                InstanceRoom(s_data);
            }
        }
    }

    /// <summary>
    /// 提示框射线检测
    /// </summary>
    public virtual void ChickRaycast(RaycastHit hit)
    {
        //生成建筑 建筑去计算他附近的空位 那么需要知道他自身的起点坐标 上下左右坐标
        BuildTip tip = hit.collider.GetComponent<BuildTip>();
        Vector2 startPoint = new Vector2(tip.startX, tip.emptyPoint.startPoint.y);
        AddBuilding(startPoint);
        //删除当前已使用空位
        allEmptyPoint.Remove(tip.emptyPoint);
        //将所有标签移出屏幕
        MapControl.instance.ResetRoomTip();
    }
    /// <summary>
    /// 建筑生成提示框
    /// </summary>
    public void BuildRoomTip(BuildingData data)
    {
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
                Debug.LogError("有空地址");
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
        List<RoomMgr> removeRoom = MapControl.instance.removeRoom;
        for (int i = 0; i < removeRoom.Count; i++)
        {
            if (removeRoom[i].RoomName == data.buildingData.RoomName
                 && removeRoom[i].BuildingData.RoomSize == data.buildingData.RoomSize)
            {
                Debug.Log("有相同的  : " + data.buildingData);
                RoomMgr room = removeRoom[i];
                allroom.Add(room);
                room.UpdateBuilding(data, this);
                removeRoom.Remove(removeRoom[i]);
                return room;
            }
        }
        GameObject go = null;
        if (data.buildingData.RoomType != RoomType.Production)
        {
            go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName);
        }
        else
        {
            switch (data.buildingData.RoomSize)
            {
                case 3: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName); break;
                case 6: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName + "_1"); break;
                case 9: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName + "_2"); break;
                default:
                    break;
            }
        }
        go = Instantiate(go, buildingPoint) as GameObject;
        go.name = data.buildingData.RoomName.ToString();
        RoomMgr room_1 = go.GetComponent<RoomMgr>();
        room_1.UpdateBuilding(data, this);
        allroom.Add(room_1);
        return room_1;
    }

    /// <summary>
    /// 寻找房间
    /// </summary>
    /// <param name="id">房间ID</param>
    public void FindRoom(int id)
    {
        for (int i = 0; i < allroom.Count; i++)
        {
            if (allroom[i].Id == id)
            {
                allroom[i].ShowHarvest();
                return;
            }
        }
    }

    /// <summary>
    /// 生成房间
    /// </summary>
    public RoomMgr InstanceRoom(LocalBuildingData data, bool isRun)
    {
        List<RoomMgr> removeRoom = MapControl.instance.removeRoom;
        for (int i = 0; i < removeRoom.Count; i++)
        {
            if (removeRoom[i].RoomName == data.buildingData.RoomName
                 && removeRoom[i].BuildingData.RoomSize == data.buildingData.RoomSize)
            {
                Debug.Log("有相同的  : " + data.buildingData);
                RoomMgr room = removeRoom[i];
                allroom.Add(room);
                room.UpdateBuilding(data, this, isRun);
                removeRoom.Remove(removeRoom[i]);
                return room;
            }
        }
        GameObject go = null;
        if (data.buildingData.RoomType != RoomType.Production)
        {
            go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName);
        }
        else
        {
            switch (data.buildingData.RoomSize)
            {
                case 3: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName); break;
                case 6: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName + "_1"); break;
                case 9: go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + data.buildingData.RoomName + "_2"); break;
                default:
                    break;
            }
        }
        go = Instantiate(go, buildingPoint) as GameObject;
        go.name = data.buildingData.RoomName.ToString();
        RoomMgr room_1 = go.GetComponent<RoomMgr>();
        room_1.UpdateBuilding(data, this, isRun);
        allroom.Add(room_1);
        return room_1;
    }
}
