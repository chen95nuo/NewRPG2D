using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleMgr : MonoBehaviour
{
    public const float high = 10.8f;//默认最小房间高度
    public const float width = 4.77f;//默认最小房间宽度
    private const int buildHigh = 4;//默认最小等级的高度
    private const int buildWidth = 17;//默认最小等级的长度
    private int buildH = 4;//可变高度
    private int buildW = 17;//可变长度

    public GameObject[] Wall;
    public GameObject buildTip;
    public Transform buildingPoint;
    public Transform buildTipPoint;

    public Vector2 startPoint;
    public Vector2 limitPoint_1;

    public int maxLength = 100;
    public int maxWidth = 100;
    public BuildPoint[,] buildPoint;

    public List<RoomMgr> rooms;//所有建成的房间
    public List<RoomMgr> removeRoom;//被删除的建筑
    public List<BuildTip> buildTips;//所有标签的位置
    public List<EmptyPoint> emptyPoint;//所有空位
    //public List<ServerBuildData> serverRoom;//服务器中的房间
    public BuildingData buildingData;

    public bool editMode = false;//是否可以建造
    public CastleType castleType;

    private int useEmptyIndex = 0;//使用过的空格数量
    private PlayerData playerData;


    private void Awake()
    {
        HallEventManager.instance.AddListener<RaycastHit>(HallEventDefineEnum.InEditMode, ChickReycast);
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.InEditMode, RemoveRoom);
        HallEventManager.instance.AddListener<BuildingData>(HallEventDefineEnum.AddBuild, BuildRoomTip);
        HallEventManager.instance.AddListener<List<ServerBuildData>>(HallEventDefineEnum.AddBuild, AcceptServerRoom);//监听服务器推送的建筑信息
        HallEventManager.instance.AddListener<RoomMgr>(HallEventDefineEnum.ChickRoomMerge, ChickMergeRoom);

        Init();
    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<RaycastHit>(HallEventDefineEnum.InEditMode, ChickReycast);
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.InEditMode, RemoveRoom);
        HallEventManager.instance.RemoveListener<BuildingData>(HallEventDefineEnum.AddBuild, BuildRoomTip);
        HallEventManager.instance.RemoveListener<List<ServerBuildData>>(HallEventDefineEnum.AddBuild, AcceptServerRoom);//监听服务器推送的建筑信息
        HallEventManager.instance.RemoveListener<RoomMgr>(HallEventDefineEnum.ChickRoomMerge, ChickMergeRoom);

    }
    // Use this for initialization
    void Start()
    {

    }

    protected void Init()
    {
        playerData = GetPlayerData.Instance.GetData();
        UpdateBGNumber();
        instanceWall();
    }

    protected void AcceptServerRoom(List<ServerBuildData> room)//游戏开始时将已建造的房间直接建造出来
    {
        if (castleType != CastleType.main)
        {
            return;
        }
        //判断是否为初始阶段 若不是先清空在重建
        if (this.rooms.Count > 0)
        {
            for (int i = this.rooms.Count - 1; i >= 0; i--)
            {
                this.rooms[i].RemoveBuilding(buildPoint);
            }
        }
        //建造房间
        if (room != null && room.Count > 0)
        {
            for (int i = 0; i < room.Count; i++)
            {
                InstanceRoom(room[i]);
            }
        }
    }

    /// <summary>
    /// 射线检测
    /// </summary>
    private void ChickReycast(RaycastHit hit)
    {
        if (editMode == false)
        {
            return;
        }
        //生成建筑 建筑去计算他附近的空位 那么需要知道他自身的起点坐标 上下左右坐标
        BuildTip tip = hit.collider.GetComponent<BuildTip>();
        Vector2 startPoint = new Vector2(tip.startX, tip.emptyPoint.startPoint.y);
        ServerBuildData data = new ServerBuildData(startPoint, buildingData, 0, 0);
        //生成建筑
        InstanceRoom(data);
        //删除当前已使用空位
        emptyPoint.Remove(tip.emptyPoint);
        //将所有标签移出屏幕
        ResetRoomTip();
    }

    /// <summary>
    /// 更新背景大小
    /// </summary>
    private void UpdateBGNumber()
    {
        buildPoint = new BuildPoint[maxLength, maxWidth];
    }
    /// <summary>
    /// 刷新背景墙
    /// </summary>
    protected void instanceWall()
    {
        buildH = buildHigh + playerData.MainHallLevel;
        buildW = buildWidth + (playerData.MainHallLevel * 3);

        for (int i = 0; i < buildH; i++)
        {
            for (int j = 0; j < buildW; j++)
            {
                if (startPoint.x + (width * j) < limitPoint_1.x && startPoint.y + (high * i) > limitPoint_1.y)
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
                    GameObject go = Instantiate(Wall[j % 3], buildingPoint) as GameObject;
                    Vector2 point = new Vector2(startPoint.x + (width * j), startPoint.y + (high * i));
                    go.transform.localPosition = point;
                    buildPoint[j, i].pointWall = go.transform;
                }
            }
        }
    }

    /// <summary>
    /// 建筑生成提示框
    /// </summary>
    public void BuildRoomTip(BuildingData data)
    {
        if (editMode == false)
        {
            return;
        }
        ResetRoomTip();
        buildingData = data;
        //如果没有提示框那么新建
        if (buildTips == null)
        {
            buildTips = new List<BuildTip>();
        }
        //如果没空位新建空位
        if (emptyPoint == null || emptyPoint.Count < 1)
        {
            emptyPoint = new List<EmptyPoint>();
            EmptyPoint empty = new EmptyPoint(new Vector2(6, 1), 16, 9, null);
            emptyPoint.Add(empty);
        }
        int index = 0;
        //有空位那么遍历所有已知空位寻找符合大小的空间
        for (int i = 0; i < emptyPoint.Count; i++)
        {
            if (emptyPoint[i].emptyNumber == 0)
            {
                Debug.LogError("发现空地址");
                emptyPoint.Remove(emptyPoint[i]);
            }
            if (emptyPoint[i].emptyNumber >= data.RoomSize)
            {
                //如果提示框数量不足 新建一个
                if (buildTips.Count <= index)
                {
                    GameObject go = Instantiate(buildTip, buildTipPoint) as GameObject;
                    BuildTip buidTip = go.GetComponentInChildren<BuildTip>();
                    buildTips.Add(buidTip);
                }
                //给符合条件的位置放置提示框
                bool isRead = buildTips[index].UpdateTip(emptyPoint[i], emptyPoint, data, this);
                if (isRead == true)
                {
                    index++;
                }
            }
        }
        for (int i = index; i < useEmptyIndex; i++)
        {
            buildTips[i].transform.position = new Vector2(-1000, -1000);
        }
        useEmptyIndex = index;
    }

    /// <summary>
    /// 重置建筑生成提示框
    /// </summary>
    private void ResetRoomTip()
    {
        for (int i = 0; i < buildTips.Count; i++)
        {
            buildTips[i].RestartThisTip(this);
            buildTips[i].transform.position = new Vector2(-1000, -1000);
        }
    }

    /// <summary>
    /// 检查当前房间是否可以合并
    /// </summary>
    public virtual void ChickMergeRoom(RoomMgr data)
    {
        if (data.buildingData.MergeID == 0)
        {
            return;
        }
        for (int i = 0; i < data.nearbyRoom.Length; i++)
        {
            //如果他们名字相同且等级一致 那么可以升级
            if (data.nearbyRoom[i] == null)
            {
                break;
            }
            if (data.RoomName == data.nearbyRoom[i].RoomName
                && data.buildingData.Level == data.nearbyRoom[i].buildingData.Level)
            {
                BuildingData b_data = new BuildingData();
                //如果这是个可以合并的组合
                if (data.buildingData.RoomSize < data.nearbyRoom[i].buildingData.RoomSize)
                {
                    b_data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.nearbyRoom[i].buildingData.MergeID);
                }
                else
                {
                    b_data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(data.buildingData.MergeID);
                }
                float Yield = data.Yield + data.nearbyRoom[i].Yield;
                float Stock = data.Stock + data.nearbyRoom[i].Stock;
                ServerBuildData s_data = new ServerBuildData();
                //判断谁在左边
                if (data.nearbyRoom[i].buidStartPoint.x < data.buidStartPoint.x)
                {
                    Debug.Log("对方靠左");
                    //和对方合并
                    s_data = new ServerBuildData(data.nearbyRoom[i].buidStartPoint, b_data, Yield, Stock);
                }
                else
                {
                    Debug.Log("我方靠左");
                    s_data = new ServerBuildData(data.buidStartPoint, b_data, Yield, Stock);
                }
                LocalServer.instance.RemoveRoom(data.nearbyRoom[i].s_Data);//删除服务器数据
                ChickPlayerInfo.instance.ChickBuildMerge(data.nearbyRoom[i].s_Data);//删除本地储存的数据
                LocalServer.instance.RemoveRoom(data.s_Data);//删除服务器中的数据
                ChickPlayerInfo.instance.ChickBuildMerge(data.s_Data);//删除本地储存的数据
                data.nearbyRoom[i].RemoveBuilding();//移除该房间
                data.RemoveBuilding();//移除该房间
                InstanceRoom(s_data);
            }
        }
    }

    ///// <summary>
    ///// 通过提示框生成房间
    ///// </summary>
    //public void InstanceRoom(BuildTip tip)
    //{
    //    //如果该房间在对象池内 那么直接调用
    //    for (int i = 0; i < removeRoom.Count; i++)
    //    {
    //        if (removeRoom[i].gameObject.name == buildingData.RoomName.ToString())
    //        {
    //            removeRoom[i].transform.position = tip.parentPoint.position;
    //            RoomMgr room = removeRoom[i].GetComponent<RoomMgr>();
    //            tip.InstanceRoom(room, buildingData, this);
    //            removeRoom.Remove(removeRoom[i]);
    //            return;
    //        }
    //    }
    //    GameObject go = Resources.Load<GameObject>("UIPrefab/Building/Build_" + buildingData.RoomName);
    //    go = Instantiate(go, buildingPoint) as GameObject;
    //    go.name = buildingData.RoomName.ToString();
    //    go.transform.position = tip.parentPoint.position;
    //    RoomMgr room_1 = go.GetComponent<RoomMgr>();
    //    tip.InstanceRoom(room_1, buildingData, this);
    //}

    /// <summary>
    /// 直接生成房间
    /// </summary>
    public void InstanceRoom(ServerBuildData data)
    {
        for (int i = 0; i < removeRoom.Count; i++)
        {
            if (removeRoom[i].RoomName == data.buildingData.RoomName
                && removeRoom[i].buildingData.RoomSize == data.buildingData.RoomSize)
            {
                Debug.Log("有相同的  : " + data.buildingData);
                removeRoom[i].transform.position = buildPoint[(int)data.buildingPoint.x, (int)data.buildingPoint.y].pointWall.transform.position;
                RoomMgr room = removeRoom[i];
                room.UpdateBuilding(data.buildingPoint, data.buildingData, this);
                removeRoom.Remove(removeRoom[i]);
                return;
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
        go.transform.position = this.buildPoint[(int)data.buildingPoint.x, (int)data.buildingPoint.y].pointWall.transform.position;
        RoomMgr room_1 = go.GetComponent<RoomMgr>();
        room_1.UpdateBuilding(data.buildingPoint, data.buildingData, this);
    }

    /// <summary>
    /// 删除房间 只有建造模式可用
    /// </summary>
    /// <param name="room"></param>
    public void RemoveRoom(RoomMgr room)
    {
        if (castleType == CastleType.main)
        {
            return;
        }
        room.RemoveBuilding(this.buildPoint);
    }

}

