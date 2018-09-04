using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleMgr : MonoBehaviour
{
    private float high = 10.8f;
    private float width = 4.77f;

    public Button louti;
    public Button woshi;

    public GameObject[] Wall;
    public GameObject buildTip;
    public Transform buildingPoint;
    public Transform buildTipPoint;

    public Vector2 startPoint;
    public Vector2 limitPoint_1;

    public int maxLength = 100;
    public int maxWidth = 100;
    private int buildLength = 17;//当前等级的长度
    private int buildWidth = 4;//当前等级的高度
    private BuildPoint[,] buildPoint;

    //public List<RoomMgr> room;//所有建成的房间
    private List<BuildTip> buildTips;//所有标签的位置
    private List<EmptyPoint> emptyPoint;//所有空位
    private BuildingData buildingData;

    void Awake()
    {
        Init();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ChickReycast();
    }

    private void Init()
    {
        louti.onClick.AddListener(ChickLouTi);
        woshi.onClick.AddListener(ChickWoShi);

        UpdateBGNumber();
        instanceWall();
    }

    /// <summary>
    /// 射线检测
    /// </summary>
    private void ChickReycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.collider.name);
                if (hit.collider.tag == "BuildTip")
                {
                    //生成建筑 建筑去计算他附近的空位 那么需要知道他自身的起点坐标 上下左右坐标
                    BuildTip tip = hit.collider.GetComponent<BuildTip>();
                    //生成建筑
                    if (tip.isMerge)
                    {
                        MergeRoom();
                    }
                    else
                    {
                        InstanceRoom(tip);
                    }


                    //删除当前已使用空位
                    emptyPoint.Remove(tip.emptyPoint);
                    //将所有标签移出屏幕
                    for (int i = 0; i < buildTips.Count; i++)
                    {
                        buildTips[i].transform.position = new Vector2(-1000, -1000);
                    }
                }
            }
        }
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
    private void instanceWall()
    {
        int index = 0;
        for (int i = 0; i < buildWidth; i++)
        {
            for (int j = 0; j < buildLength; j++)
            {
                if (index > 2)
                {
                    index = 0;
                }
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
                    GameObject go = Instantiate(Wall[index], buildingPoint) as GameObject;
                    Vector2 point = new Vector2(startPoint.x + (width * j), startPoint.y + (high * i));
                    go.transform.position = point;
                    buildPoint[j, i].pointWall = go.transform;
                }
                index++;
            }
        }
    }
    /// <summary>
    /// 建筑生成提示
    /// </summary>
    private void BuildRoomTip(BuildingData data)
    {
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
            Debug.Log("新建");
            emptyPoint.Add(new EmptyPoint());
        }
        //if (room == null || room.Count < 1)
        //{
        //    room = new List<RoomMgr>();
        //    emptyPoint.Add(new EmptyPoint());
        //}
        int index = 0;
        //有空位那么遍历所有已知空位寻找符合大小的空间
        for (int i = 0; i < emptyPoint.Count; i++)
        {
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
                buildTips[index].UpdateTip(emptyPoint[i], data, buildPoint);
                index++;
            }
        }
    }

    /// <summary>
    /// 合并房间
    /// </summary>
    private void MergeRoom() { Debug.Log("合并房间"); }

    /// <summary>
    /// 生成房间
    /// </summary>
    private void InstanceRoom(BuildTip tip)
    {
        GameObject go = Resources.Load<GameObject>("UIPrefab/Building/" + buildingData.SprintName);
        go = Instantiate(go, buildingPoint) as GameObject;
        go.transform.position = tip.parentPoint.position;
        go.GetComponent<RoomMgr>().UpdateBuilding(buildPoint, emptyPoint, tip.emptyPoint, buildingData);

        //EmptyPoint point = tip.emptyPoint;
        ////寻找附近空位 如果结束的X轴比开始的X轴小 那么这个物体是向右延伸的
        //if (point.startPoint.x < point.endPoint)//向右
        //{
        //    int index = 0;
        //    //向右遍历到最大数量
        //    for (int i = (int)point.startPoint.x; i < point.startPoint.x + buildingData.RoomSize + 9; i++)
        //    {
        //        if (i >= buildLength)
        //        {
        //            break;
        //        }
        //        if (i < point.startPoint.x + buildingData.RoomSize)
        //        {
        //            buildPoint[i, (int)point.startPoint.y].pointType = BuildingType.Full;
        //            buildPoint[i, (int)point.startPoint.y].pointWall.Translate(Vector3.back * 1000);
        //        }
        //        else if (buildPoint[i, (int)point.startPoint.y].pointType == BuildingType.Wall)
        //        {
        //            index++;
        //        }
        //    }
        //    EmptyPoint empty = new EmptyPoint(new Vector2(point.startPoint.x + buildingData.RoomSize, point.startPoint.y), (int)point.startPoint.x + buildingData.RoomSize + index, index);
        //    Debug.Log("空位起点 :" + (point.startPoint.x + buildingData.RoomSize + index));
        //    empty.buildingData = buildingData;
        //    emptyPoint.Add(empty);
        //}
        //else
        //{
        //    int index = 0;
        //    //向左遍历到最大数量
        //    for (int i = (int)point.startPoint.x; i > point.startPoint.x - buildingData.RoomSize - 9; i--)
        //    {
        //        if (i <= 0)
        //        {
        //            break;
        //        }
        //        if (i > point.startPoint.x - buildingData.RoomSize)
        //        {
        //            buildPoint[i, (int)point.startPoint.y].pointType = BuildingType.Full;
        //            buildPoint[i, (int)point.startPoint.y].pointWall.Translate(Vector3.back * 1000);
        //        }
        //        else if (buildPoint[i, (int)point.startPoint.y].pointType == BuildingType.Wall)
        //        {
        //            index++;
        //        }
        //    }
        //    EmptyPoint empty = new EmptyPoint(new Vector2(point.startPoint.x - buildingData.RoomSize, point.startPoint.y), (int)point.startPoint.x - buildingData.RoomSize - index, index);
        //    Debug.Log("空位起点 :" + (index));
        //    empty.buildingData = buildingData;
        //    emptyPoint.Add(empty);
        //}
    }

    private void ChangeLeftOrRight()
    {

    }
    private void ChangeUpOrDown()
    {

    }


    #region 测试区域
    private void ChickLouTi()
    {
        BuildingData louTi = new BuildingData();
        louTi.RoomSize = 1;
        louTi.SprintName = "Build_Stairs";
        louTi.RoomType = BuildRoomType.Stairs;
        BuildRoomTip(louTi);
    }

    private void ChickWoShi()
    {
        BuildingData woShi = new BuildingData();
        woShi.RoomSize = 3;
        woShi.SprintName = "Build_Bedroom";
        BuildRoomTip(woShi);
    }
    #endregion
}


public class BuildPoint
{
    public BuildingType pointType;
    public Transform pointWall;
}

/// <summary>
/// 空位信息
/// </summary>
public class EmptyPoint
{
    public Vector2 startPoint;//起点位置
    public int endPoint;//结束位置
    public int emptyNumber;//空位数量
    public BuildingData buildingData;

    public EmptyPoint()
    {
        startPoint = new Vector2(16, 1);
        endPoint = 6;
        emptyNumber = 9;
    }

    public EmptyPoint(Vector2 startPoint, int endPoint, int emptyNumber)
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        this.emptyNumber = emptyNumber;
    }
}