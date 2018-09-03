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

    public int buildLength = 17;//当前等级的长度
    public int buildWidth = 4;//当前等级的高度
    public buildingType[,] buildPoint;

    public List<RoomMgr> room;//所有建成的房间
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
        startPoint = new Vector2(startPoint.x + (width * 0.5f), startPoint.y + (high * 0.5f));

        louti.onClick.AddListener(ChickLouTi);
        woshi.onClick.AddListener(ChickWoShi);

        UpdateBGNumber();
        instanceWall();
    }

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
                    GameObject build = Resources.Load<GameObject>("UIPrefab/Building/" + buildingData.SprintName);
                    GameObject go = Instantiate(build, buildingPoint) as GameObject;
                    go.transform.position = hit.collider.transform.position;
                    BuildTip tip = hit.collider.GetComponent<BuildTip>();
                    Vector2 point = new Vector2(tip.emptyPoint.startPoint.x, tip.emptyPoint.startPoint.y);
                    int index = 0;//空位数量
                    //改变当前使用的空位信息 先将当前位置状态信息改为房屋，在继续遍历后面的，若不为墙则离开循环
                    for (int i = 0; i < buildingData.RoomSize + 9; i++)
                    {
                        if ((int)point.x + i > buildLength)
                        {
                            break;
                        }
                        if (i < buildingData.RoomSize)
                        {
                            buildPoint[(int)point.x + i, (int)point.y] = buildingType.Full;
                        }
                        else
                        {
                            if (buildPoint[(int)point.x + i, (int)point.y] == buildingType.Wall)
                            {
                                index++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (index > 0)
                    {

                    }
                    //清除当前使用的空位
                    //emptyPoint.Remove()
                    //重新检测空位


                    //将所有标签移出屏幕
                    for (int i = 0; i < buildTips.Count; i++)
                    {
                        buildTips[i].transform.position = new Vector2(-1000, -1000);
                    }
                }
            }
        }
    }

    private void UpdateBGNumber()
    {
        buildPoint = new buildingType[buildLength, buildWidth];
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
                    buildPoint[j, i] = 0;
                }
                else
                {
                    buildPoint[j, i] = buildingType.Wall;
                }
                if (buildPoint[j, i] == buildingType.Wall)
                {
                    GameObject go = Instantiate(Wall[index], buildingPoint) as GameObject;
                    Vector2 point = new Vector2(startPoint.x + (width * j), startPoint.y + (high * i));
                    go.transform.position = point;
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
            emptyPoint.Add(new EmptyPoint());
        }
        //有空位那么遍历所有已知空位寻找符合大小的空间
        for (int i = 0; i < emptyPoint.Count; i++)
        {
            if (emptyPoint[i].emptyNumber >= data.RoomSize)
            {
                //如果提示框数量不足 新建一个
                if (buildTips.Count <= i)
                {
                    GameObject go = Instantiate(buildTip, buildTipPoint) as GameObject;
                    BuildTip buidTip = go.GetComponent<BuildTip>();
                    buildTips.Add(buidTip);
                }
                //给符合条件的位置放置提示框
                buildTips[i].UpdateTip(emptyPoint[i], data.RoomSize, startPoint);/* = new Vector2(startPoint.x + (width * 2 * (emptyPoint[i].startPoint.x + (data.RoomSize * 0.5f - 0.5f))), startPoint.y + (high * 2 * emptyPoint[i].startPoint.y));*/
            }
        }
    }

    private void ChickLouTi()
    {
        BuildingData louTi = new BuildingData();
        louTi.RoomSize = 1;
        louTi.SprintName = "Build_Stairs";
        BuildRoomTip(louTi);
    }

    private void ChickWoShi()
    {
        BuildingData woShi = new BuildingData();
        woShi.RoomSize = 3;
        woShi.SprintName = "Build_Bedroom";
        BuildRoomTip(woShi);
    }
}

//空位信息
public class EmptyPoint
{
    public Vector2 startPoint;//起点位置
    public int emptyNumber;//空位数量

    public EmptyPoint()
    {
        startPoint = new Vector2(6, 1);
        emptyNumber = 9;
    }
}