﻿/*
 * 房屋操作因需要点击房屋判断房屋类型出现相应UI
 * 所以该脚本用于点击后获得房间类型与UI交互
 * 并且该脚本内存有房屋所占用墙的引用及当前房屋所占坐标系
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public abstract class RoomMgr : MonoBehaviour
{
    private Castle castleMgr;
    public Vector2 buidStartPoint;//起点坐标
    public int buildEndPoint;//终点坐标

    private BuildPoint[] wall;

    private Vector2 startPoint;
    private EmptyPoint[] emptyPoints = new EmptyPoint[4]; //左,右,上,下的空位;
    public RoomMgr[] nearbyRoom = new RoomMgr[4]; // 附近的房间 左,右,上,下;
    public int maxRoomSize = 9;

    public bool mainLink = false;//主链接
    public bool linkType = false;//连接状态
    private bool isHarvest = false;//可否收获
    private bool constructionType = false;//是否在施工中
    private bool stockFull = false;//该类资源是否满值

    private List<RoomMgr> disconnectRoom = new List<RoomMgr>();//断开连接的房间用于楼梯上下

    [System.NonSerialized]
    public GameObject disTip;//断开连接的标签
    private GameObject roomLock;//房间选定框
    private SpriteRenderer roomLockRend;
    [System.NonSerialized]
    private GameObject roomProp;//资源获取框
    [System.NonSerialized]
    public SpriteRenderer roomPropIcon;//资源Icon
    [System.NonSerialized]
    public SpriteRenderer roomPropIconBG;//资源Icon背景

    private int needTime = 0;
    private int listNumber = 0;
    private UILevelUpTip levelUpTip;
    public LocalBuildingData currentBuildData;
    private BuildingData changeData;

    public Vector2 StartPoint
    {
        get
        {
            return currentBuildData.buildingPoint;
        }
    }

    public BuildingData BuildingData
    {
        get
        {
            return currentBuildData.buildingData;
        }
    }

    public bool ConstructionType
    {
        get
        {
            return constructionType;
        }
        set
        {
            bool index = value;
            if (index != constructionType && MapControl.instance.type == CastleType.main)
            {
                constructionType = value;
                currentBuildData.ConstructionType = value;
                if (ChickPlayerInfo.instance.ChickProduction(currentBuildData))
                {
                    if (value == false)
                    {
                        Debug.Log("施工结束 添加监听事件");
                        //施工结束就添加事件
                        ChickPlayerInfo.instance.ThisProduction(currentBuildData);
                    }
                    else
                    {
                        Debug.Log("施工中 关闭监听事件");
                        //施工中就关闭事件
                        ChickPlayerInfo.instance.ClostProduction(currentBuildData);
                    }
                }
            }
        }
    }
    public bool StockFull
    {
        get { return stockFull; }
        set
        {
            bool temp = value;
            if (temp != StockFull)
            {
                stockFull = value;
                if (stockFull == false)
                {
                    Debug.Log("未满");
                    RoomPropIconBG.sprite = GetSpriteAtlas.insatnce.GetIcon("RoomNoFull");
                }
                else
                {
                    Debug.Log("已满");
                    RoomPropIconBG.sprite = GetSpriteAtlas.insatnce.GetIcon("RoomFull");
                }
            }
        }
    }

    public void Clear()
    {
        castleMgr = null;
        buidStartPoint = Vector3.zero;
        wall = null;
        emptyPoints = new EmptyPoint[4];
        nearbyRoom = new RoomMgr[4];
        changeData = null;
    }
    public int Id
    {
        get
        {
            return currentBuildData.id;
        }
    }

    private int timeIndex;

    public RoleAttribute NeedAttribute
    {
        get
        {
            switch (RoomName)
            {
                case BuildRoomName.Nothing:
                    break;
                case BuildRoomName.Gold:
                    return RoleAttribute.Gold;
                case BuildRoomName.Food:
                    return RoleAttribute.Food;
                case BuildRoomName.Mana:
                    return RoleAttribute.Mana;
                case BuildRoomName.Wood:
                    return RoleAttribute.Wood;
                case BuildRoomName.Iron:
                    return RoleAttribute.Iron;
                case BuildRoomName.FighterRoom:
                    return RoleAttribute.Fight;
                case BuildRoomName.Kitchen:
                    return RoleAttribute.Food;
                case BuildRoomName.Mint:
                    return RoleAttribute.Gold;
                case BuildRoomName.Laboratory:
                    return RoleAttribute.Mana;
                case BuildRoomName.Crafting:
                    return RoleAttribute.Wood;
                case BuildRoomName.Foundry:
                    return RoleAttribute.Fight;
                case BuildRoomName.Hospital:
                    return RoleAttribute.HP;
                case BuildRoomName.MagicWorkShop:
                    return RoleAttribute.ManaSpeed;
                case BuildRoomName.MagicLab:
                    return RoleAttribute.ManaSpeed;
                case BuildRoomName.WeaponsWorkShop:
                case BuildRoomName.ArmorWorkShop:
                    return RoleAttribute.ManaSpeed;
                case BuildRoomName.GemWorkSpho:
                    return RoleAttribute.ManaSpeed;
                case BuildRoomName.Barracks:
                    return RoleAttribute.Fight;
                case BuildRoomName.LivingRoom:
                    return RoleAttribute.Max;
                case BuildRoomName.MaxRoom:
                default:
                    break;
            }
            return RoleAttribute.Nothing;
        }
    }
    public BuildRoomName RoomName
    {
        get
        {
            return currentBuildData.buildingData.RoomName;
        }
    }


    public bool IsHarvest
    {
        get
        {
            return isHarvest;
        }

        set
        {
            bool temp = value;
            if (temp != isHarvest)
            {
                isHarvest = value;
                RoomProp.SetActive(value);
                if (roomPropIcon == null)
                {
                    roomPropIcon = this.transform.Find("RoomTypes/RoomProp/PropBG/Gold").GetComponent<SpriteRenderer>();
                    Sprite sp = GetSpriteAtlas.insatnce.GetIcon(RoomName.ToString());
                    roomPropIcon.sprite = sp;
                }
            }
        }
    }

    public SpriteRenderer RoomPropIconBG
    {
        get
        {
            if (roomPropIconBG == null)
            {
                Debug.Log(RoomName);
                roomPropIconBG = this.transform.Find("RoomTypes/RoomProp/PropBG").GetComponent<SpriteRenderer>();
            }
            return roomPropIconBG;
        }
    }

    public SpriteRenderer RoomLockRend
    {
        get
        {
            if (roomLockRend == null)
            {
                roomLockRend = roomLock.GetComponent<SpriteRenderer>();
            }
            return roomLockRend;
        }
    }

    public void ShowRoomLockUI(bool isTrue, bool isRole = false)
    {
        if (isTrue == false)
        {
            roomLock.SetActive(isTrue);
            return;
        }
        roomLock.SetActive(isTrue);
        if (isRole == true)
        {
            RoomLockRend.color = new Color(255 / 255f, 238 / 255f, 89 / 255f);
        }
        else
        {
            RoomLockRend.color = new Color(90 / 255f, 255 / 255f, 167 / 255f);
        }
    }

    public GameObject RoomProp
    {
        get
        {
            if (roomProp == null)
            {
                GetCompoment();
            }
            return roomProp;
        }

        set
        {
            roomProp = value;
        }
    }

    /// <summary>
    /// 左右
    /// </summary>
    /// <param name="buildPoint"></param>
    /// <param name="points"></param>
    public void ChickLeftOrRight(BuildPoint[,] buildPoint)
    {
        int startX = (int)StartPoint.x;//当前房间开始位置
        int startY = (int)StartPoint.y;//当前房间楼层
        int endX = (int)StartPoint.x + BuildingData.RoomSize;//当前房间结束位置
        int endPoint = 0;
        Vector2 sPoint = new Vector2();

        bool left = true;
        bool right = true;
        int leftIndex = 0;
        int rightIndex = 0;

        //计算房间两侧空间
        for (int i = 0; i < maxRoomSize; i++)
        {
            //向左
            if (startX - (i + 1) >= 0 && buildPoint[startX - (i + 1), startY] != null && left
                && buildPoint[startX - (i + 1), startY].pointType == BuildingType.Wall)
            {
                leftIndex++;
            }
            else
                left = false;

            //向右
            if (right && buildPoint[endX + i, startY] != null
                && buildPoint[endX + i, startY].pointType == BuildingType.Wall)
            {
                rightIndex++;
            }
            else
                right = false;

            if (left == false && right == false)
            {
                break;
            }
        }
        //左侧空位
        if (leftIndex > 0)
        {
            sPoint = new Vector2(startX - leftIndex, startY);
            endPoint = startX;
            EmptyPoint empty = new EmptyPoint(sPoint, endPoint, leftIndex, this);
            emptyPoints[0] = empty;
        }
        else
        {
            if (startX - 1 >= 0 && buildPoint[startX - 1, startY] != null &&
                buildPoint[startX - 1, startY].pointType == BuildingType.Full)
            {
                nearbyRoom[0] = buildPoint[startX - 1, startY].roomMgr;
                buildPoint[startX - 1, startY].roomMgr.nearbyRoom[1] = this;
            }
        }
        //右侧空位
        if (rightIndex > 0)
        {
            sPoint = new Vector2(endX, startY);
            endPoint = endX + rightIndex;
            EmptyPoint empty = new EmptyPoint(sPoint, endPoint, rightIndex, this);
            emptyPoints[1] = empty;
        }
        else
        {
            if (buildPoint[endX, startY] != null &&
                buildPoint[endX, startY].pointType == BuildingType.Full)
            {
                nearbyRoom[1] = buildPoint[endX, startY].roomMgr;
                buildPoint[endX, startY].roomMgr.nearbyRoom[0] = this;
            }
        }

        if (RoomName == BuildRoomName.Stairs)
            ChickUpOrDown(castleMgr.buildPoint);
        else
        {
            UpdateEmptyPoint();
        }
    }
    /// <summary>
    /// 上下
    /// </summary>
    /// <param name="buildPoint"></param>
    /// <param name="points"></param>
    protected void ChickUpOrDown(BuildPoint[,] buildPoint)
    {
        int startX = (int)StartPoint.x;
        int startY = (int)StartPoint.y;
        Vector2 sPoint = new Vector2();
        if (buildPoint[startX, startY + 1] != null
            && buildPoint[startX, startY + 1].pointType == BuildingType.Wall)
        {
            sPoint = new Vector2(startX, startY + 1);
            EmptyPoint empty = new EmptyPoint(sPoint, startX + 1, 1, this);

            emptyPoints[2] = empty;
        }
        else if (buildPoint[startX, startY + 1] != null
            && buildPoint[startX, startY + 1].pointType == BuildingType.Full
            && buildPoint[startX, startY + 1].roomMgr.RoomName == BuildRoomName.Stairs)
        //如果上面位置不是空的且有房间且房间类型是楼梯 那么上方添加该房间
        {
            nearbyRoom[2] = buildPoint[startX, startY + 1].roomMgr;
            buildPoint[startX, startY + 1].roomMgr.nearbyRoom[3] = this;
        }
        if (startY - 1 >= 0 && buildPoint[startX, startY - 1] != null
            && buildPoint[startX, startY - 1].pointType == BuildingType.Wall)
        {
            sPoint = new Vector2(startX, startY - 1);
            EmptyPoint empty = new EmptyPoint(sPoint, startX + 1, 1, this);

            emptyPoints[3] = empty;
        }
        else if (startY - 1 >= 0 && buildPoint[startX, startY - 1] != null
            && buildPoint[startX, startY - 1].pointType == BuildingType.Full
            && buildPoint[startX, startY - 1].roomMgr.RoomName == BuildRoomName.Stairs)
        {
            nearbyRoom[3] = buildPoint[startX, startY - 1].roomMgr;
            buildPoint[startX, startY - 1].roomMgr.nearbyRoom[2] = this;
        }

        UpdateEmptyPoint();
    }

    public virtual void Awake()
    {
        GetCompoment();
        HallEventManager.instance.AddListener<RoomStockFullHelper>(HallEventDefineEnum.ChickStockFull, ChickStockFull);


    }
    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<RoomStockFullHelper>(HallEventDefineEnum.ChickStockFull, ChickStockFull);
    }
    private void ChickStockFull(RoomStockFullHelper data)
    {
        if (data.name != RoomName)
        {
            return;
        }
        StockFull = data.isFull;
    }
    /// <summary>
    /// 创建或重新激活建筑
    /// </summary>
    /// <param name="data"></param>
    public void UpdateBuilding(LocalBuildingData data, Castle castle)
    {
        currentBuildData = data;
        data.currentRoom = this;
        if (MapControl.instance.type == CastleType.main)
        {
            if (currentBuildData.buildingData.RoomName == BuildRoomName.ThroneRoom)
            {
                PlayerData playerdata = GetPlayerData.Instance.GetData();
                playerdata.MainHall = currentBuildData;
            }
            else if (currentBuildData.buildingData.RoomName == BuildRoomName.Barracks)
            {
                PlayerData playerdata = GetPlayerData.Instance.GetData();
                playerdata.BarracksData = currentBuildData;
            }
        }
        castleMgr = castle;
        BuildingMove(data, castle);
        ChickLeftOrRight(castle.buildPoint);

        //如果是主场景 那么施工
        if (castle.castleType == CastleType.main)
        {
            if (data.buildingData.NeedTime != 0)
            {
                //新建的建筑需要建造时间 那么开始施工
                ConstructionStart(data.buildingData.ItemId, 0);
            }
            else
            {
                //不需要建造时间的判断是否需要添加事件
                ConstructionType = false;
                if (ChickPlayerInfo.instance.ChickProduction(currentBuildData))
                {
                    Debug.Log("添加监听");
                    //施工结束就添加事件
                    ChickPlayerInfo.instance.ThisProduction(currentBuildData);
                }
            }
        }
        else
        {
            constructionType = data.ConstructionType;
            castleMgr.ChickMergeRoom(this);
            AddConnection();
        }
    }

    /// <summary>
    /// 直接创建建筑
    /// </summary>
    /// <param name="data"></param>
    public void UpdateBuilding(LocalBuildingData data, Castle castle, ServerBuildData s_data)
    {
        currentBuildData = data;
        data.currentRoom = this;

        if (currentBuildData.buildingData.RoomName == BuildRoomName.ThroneRoom)
        {
            PlayerData playerdata = GetPlayerData.Instance.GetData();
            playerdata.MainHall = currentBuildData;
        }
        else if (currentBuildData.buildingData.RoomName == BuildRoomName.Barracks)
        {
            PlayerData playerdata = GetPlayerData.Instance.GetData();
            playerdata.BarracksData = currentBuildData;
        }
        castleMgr = castle;
        BuildingMove(data, castle);
        ChickLeftOrRight(castle.buildPoint);

        //不需要建造时间的判断是否需要添加事件
        ConstructionType = false;
        if (ChickPlayerInfo.instance.ChickProduction(currentBuildData))
        {
            Debug.Log("添加监听");
            //施工结束就添加事件
            ChickPlayerInfo.instance.ThisProduction(currentBuildData);
        }
    }


    /// <summary>
    /// 添加空位信息
    /// </summary>
    protected void UpdateEmptyPoint()
    {
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            if (emptyPoints[i] != null && emptyPoints[i].roomData != null)
            {
                castleMgr.allEmptyPoint.Add(emptyPoints[i]);
            }
        }
    }

    /// <summary>
    /// 删除建筑
    /// </summary>
    public void RemoveBuilding()
    {
        //将建筑的使用信息改为停用 将墙面移动回原位
        MapControl.instance.removeRoom.Add(this);
        MapControl.instance.RemoveRoom(this);
        bool isRemove = castleMgr.allroom.Remove(this);
        int startX = (int)StartPoint.x;
        int startY = (int)StartPoint.y;

        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].pointType = BuildingType.Wall;
            wall[i].pointWall.localPosition = new Vector3(wall[i].pointWall.localPosition.x, wall[i].pointWall.localPosition.y, 0);
            wall[i].roomMgr = null;
        }

        //删除当前建筑提供的位置信息
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            if (emptyPoints[i] != null)
            {
                castleMgr.allEmptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
    }

    /// <summary>
    /// 建造模式删除建筑
    /// </summary>
    public void EditRemoveBuilding()
    {
        linkType = false; //断开自身链接
        int index = 0;
        for (int i = 0; i < nearbyRoom.Length; i++)
        {
            if (nearbyRoom[i] != null)
            {
                nearbyRoom[i].UpdateBuilding();
                nearbyRoom[i].ChickConnection(this, linkType);//通知附近房间检查自身链接
                nearbyRoom[i] = null;
                index++;
            }
        }
        if (nearbyRoom[0] == null && nearbyRoom[1] != null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (castleMgr.buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr != null)
                {
                    castleMgr.buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
                if (castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr != null)
                {
                    castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
            }
        }
        else if (nearbyRoom[0] != null && nearbyRoom[1] == null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (castleMgr.buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr != null)
                {
                    castleMgr.buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
                if (castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr != null)
                {
                    castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
            }
        }

        if (nearbyRoom[0] == null && nearbyRoom[1] == null)
        {
            bool right = true;
            bool left = true;
            for (int i = 0; i < 9; i++)
            {
                //Debug.Log(buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].pointType);
                if (left && ((int)buidStartPoint.x - (i + 1) <= 0 || castleMgr.buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y] == null
                    || castleMgr.buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].pointType == BuildingType.Nothing))
                {
                    left = false;
                }
                else if (left && castleMgr.buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].roomMgr != null)
                {
                    castleMgr.buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                    left = false;
                }
                if (right && (castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y] == null
                    || castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].pointType == BuildingType.Nothing))
                {
                    right = false;
                }
                else if (right && castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr != null)
                {
                    castleMgr.buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                    right = false;
                }

                if (left == false && right == false)
                {
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 附近建筑被删除重新检测附近空位
    /// </summary>
    public void UpdateBuilding()
    {
        //删除当前建筑提供的位置信息
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            if (emptyPoints[i] != null)
            {
                castleMgr.allEmptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
        nearbyRoom = new RoomMgr[4];
        //重新检测附近空位
        ChickLeftOrRight(castleMgr.buildPoint);
    }

    /// <summary>
    /// 将房间移动到某位置 重置附近房间和空位信息 移动到指定位置移出背景墙
    /// </summary>
    /// <param name="data"></param>
    public void BuildingMove(LocalBuildingData data, Castle castle)
    {
        castleMgr = castle;
        currentBuildData = data;
        data.currentRoom = this;
        nearbyRoom = new RoomMgr[4];
        emptyPoints = new EmptyPoint[4];
        if (castle.castleType == CastleType.main)
        {
            linkType = true;
            ChickDisTip();
        }
        //将房间移动到指定位置
        transform.position = castle.buildPoint[(int)data.buildingPoint.x, (int)data.buildingPoint.y].pointWall.transform.position;
        //将房间这一段墙壁移出 给这一段空间添加该房间引用
        int startX = (int)StartPoint.x;
        int startY = (int)StartPoint.y;
        //重置墙面信息
        wall = new BuildPoint[BuildingData.RoomSize];
        buidStartPoint = new Vector2(startX, startY);
        for (int i = startX; i < startX + BuildingData.RoomSize; i++)
        {
            wall[i - startX] = castle.buildPoint[i, startY];
            wall[i - startX].pointType = BuildingType.Full;
            wall[i - startX].pointWall.Translate((Vector3.back * 1000));
            wall[i - startX].roomMgr = this;
        }
    }



    /// <summary>
    /// 检查链接信息 让附近房间递归搜寻信息
    /// </summary>
    public void BuildingChickLink()
    {
        linkType = false; //断开自身链接
        int index = 0;
        for (int i = 0; i < nearbyRoom.Length; i++)
        {
            if (nearbyRoom[i] != null)
            {
                nearbyRoom[i].ChickNearBuilding();
                nearbyRoom[i].ChickConnection(this, linkType);//通知附近房间检查自身链接
                nearbyRoom[i] = null;
                index++;
            }
        }
    }

    /// <summary>
    /// 递归检查附近房间链接信息
    /// </summary>
    public void ChickNearBuilding()
    {
        //删除当前建筑提供的位置信息
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            if (emptyPoints[i] != null)
            {
                castleMgr.allEmptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
        nearbyRoom = new RoomMgr[4];
        //重新检测附近空位
        ChickLeftOrRight(castleMgr.buildPoint);
    }


    /// <summary>
    /// 检查连接、断开、路径点
    /// </summary>
    /// <param name="data"></param>
    /// <param name="n"></param>
    protected bool ChickConnection(RoomMgr data, bool islink)
    {
        if (mainLink)
        {
            linkType = true;
            ChickDisTip();
            return true;
        }
        if (linkType == true)
        {
            if (islink)
            {
                Debug.Log("我是通的对方也是通的");
                return true;
            }
            else//我方通畅对方堵塞 判断己方链接是否有效
            {
                RoomMgr room = null;
                int length = nearbyRoom.Length - 1;
                for (int i = length; i > -1; i--)//反向遍历 从大往小
                {
                    if (nearbyRoom[i] != null && nearbyRoom[i].linkType == true && nearbyRoom[i] != data) //如果对方是连接状态 搜索到楼梯 判断是否可通向大门
                    {
                        if (Mathf.Abs(nearbyRoom[i].buidStartPoint.y - 1) < Mathf.Abs(buidStartPoint.y - 1))//如果对方楼梯离大门比较近且对方是通的那么本条路线通畅
                        {
                            if (nearbyRoom[i].linkType == true)
                            {
                                for (int j = 0; j < nearbyRoom.Length; j++)
                                {
                                    if (nearbyRoom[i].linkType == false)
                                    {
                                        nearbyRoom[i].ChickConnection(this, true);
                                    }
                                }
                                //本就是通常所以不需要改变状态.
                                return true;
                            }
                            else
                            {
                                Debug.Log("下方楼梯是false");
                                disconnectRoom.Add(nearbyRoom[i]);
                            }
                        }
                        else if (Mathf.Abs(nearbyRoom[i].buidStartPoint.y - 1) > Mathf.Abs(buidStartPoint.y - 1))//先不着急便利远的 如果有楼上的存下先便利本层
                        {
                            room = nearbyRoom[i];//存下远的
                        }
                        else if ((nearbyRoom[i].buidStartPoint.y == buidStartPoint.y))
                        {
                            linkType = false;
                            bool thisLink = nearbyRoom[i].ChickConnection(this, false);
                            if (thisLink == false) //保存断开的房间信息
                            {
                                disconnectRoom.Add(nearbyRoom[i]);
                            }
                            //当前这个路线回来是通畅那么就返回了 所以不会出现通畅之后在堵塞的问题
                            linkType = thisLink; //如果返回的信息是通畅那么就不用管了 如果是堵塞那就改成堵塞
                            ChickDisTip();
                            if (thisLink == true)//如果某条路线返回通畅 查询附近堵塞的路线 将其改为通畅 然后返回通畅
                            {
                                for (int j = 0; j < disconnectRoom.Count; j++)
                                {
                                    if (disconnectRoom[j].linkType == false)
                                    {
                                        Debug.Log("运行了");
                                        disconnectRoom[j].ChickConnection(this, linkType);
                                    }
                                }
                                disconnectRoom.Clear();
                                return true;
                            }
                        }
                    }
                }
                //走到这里说明 要么所有路线都堵塞 要么还剩余一个远的楼层没有检查
                if (room != null)//检查远的楼层
                {
                    if (linkType == true)//通常这种情况出现在楼梯 且楼梯只向上
                    {
                        Debug.LogError("走到边缘楼梯 楼梯可通往上下" + RoomName);
                        bool link = room.ChickConnection(this, false);
                        linkType = link;
                        ChickDisTip();
                        return link;
                    }
                    bool thisLink = room.ChickConnection(this, linkType);
                    Debug.Log(linkType);
                    if (thisLink == true)//如果远的楼层返回true
                    {
                        return true;
                    }
                }
                else
                {
                    linkType = false;
                    ChickDisTip();
                    return false;
                }
            }
        }
        else
        {
            if (islink)//我方堵塞 对方通畅
            {
                Debug.Log("我方堵塞 对方通畅");
                linkType = true;
                ChickDisTip();
                for (int i = 0; i < nearbyRoom.Length; i++)
                {
                    if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
                    {
                        nearbyRoom[i].ChickConnection(this, linkType);
                    }
                }
                return true;
            }
            else //双方堵塞 因为对方发消息的时候己方这条线已经拥堵 所以没必要检测
            {
                Debug.Log("双方堵塞");
                return false;
            }
        }

        return false;
    }

    /*
     * 遍历附近的房间 检查连接状态
     */
    /// <summary>
    /// 建造房间 检查附近连接
    /// </summary>
    public void AddConnection()
    {
        int linkIndex = 0;
        int offIndex = 0;

        if (buidStartPoint == new Vector2(6, 1))
        {
            mainLink = true;
            linkType = true;
        }
        else
        {
            mainLink = false;
        }

        if (mainLink)
        {
            linkType = true;
            ChickDisTip();
            for (int i = 0; i < nearbyRoom.Length; i++)
            {
                if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
                {
                    nearbyRoom[i].ChickConnection(this, true);
                }
            }
            return;
        }
        for (int i = 0; i < nearbyRoom.Length; i++)
        {
            if (nearbyRoom[i] != null && nearbyRoom[i].linkType == true)
            {
                linkIndex++;
            }
            else if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
            {
                offIndex++;
            }
        }
        if (linkIndex > 0)//建造的时候因为附近的建筑都与自己无关所以只要有连接就可以使用
        {
            linkType = true;
            for (int i = 0; i < nearbyRoom.Length; i++)
            {
                if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
                {
                    //如果我有多个连接 但是也有一些断开的那就顺便让他们连接并检查;
                    nearbyRoom[i].ChickConnection(this, linkType);//将自己的连接状态发给对方
                }
            }
        }
        else if (linkIndex <= 0)
        {
            linkType = false;
        }

        ChickDisTip();
    }

    /// <summary>
    /// 检查断开连接提示框状态
    /// </summary>
    protected void ChickDisTip()
    {
        if (linkType == true)
        {
            disTip.SetActive(false);
        }
        else
        {
            disTip.SetActive(true);
        }
    }

    /// <summary>
    /// 开始施工
    /// </summary>
    /// <param name="data">当前房间需要变成哪个</param>
    public void ConstructionStart(int nextId, int time)
    {
        if (ConstructionType == true)
        {
            Debug.LogError("已在升级");
            return;
        }
        ConstructionType = true;
        BuildingData data = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(nextId);
        changeData = data;//记录需要升级的DATA信息
        if (time == 0)
        {
            needTime = data.NeedTime * 60;
        }
        else
        {
            needTime = time;
        }
        levelUpTip = UIPanelManager.instance.ShowPage<UILevelUpTip>(this);
        timeIndex = ChickPlayerInfo.instance.Timer(currentBuildData, needTime, timeIndex);
        listNumber = levelUpTip.AddLister();
        levelUpTip.UpdateTime(needTime, listNumber);
        CameraControl.instance.CloseRoomLock();
    }

    public bool TimerCallBack(LevelUPHelper levelUpHelper)
    {
        return levelUpTip.UpdateTime(levelUpHelper.needTime, levelUpHelper.tipID);
    }

    public void ConstructionCancel()
    {

    }

    /// <summary>
    /// 施工完成
    /// </summary>
    public void ConstructionComplete()
    {
        ChickPlayerInfo.instance.RemoveThisTime(timeIndex);
        ChickPlayerInfo.instance.ChickBuildDicChange(currentBuildData, changeData);
        levelUpTip.RemoveLister(listNumber);
        levelUpTip = null;
        ConstructionType = false;
        ChickConstructionCompleteRole();
        CameraControl.instance.RefreshRoomLock(this);
        //检查合并
        if (MapControl.instance.type == CastleType.main)
        {
            castleMgr.ChickMergeRoom(this);
        }
        else
        {

        }
        ChickComplete();
    }

    /// <summary>
    /// 若房间移动 则将角色重新移动到房间位置
    /// </summary>
    public virtual void RoleMove()
    {
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] != null)
            {
                HallRole role = HallRoleMgr.instance.GetRole(currentBuildData.roleData[i]);
                Vector3 point = new Vector3(transform.position.x + (1.2f * (i + 1)), transform.position.y + 0.3f, role.transform.position.z);
                role.transform.position = point;
            }
        }
    }
    /// <summary>
    /// 添加角色
    /// </summary>
    public virtual bool AddRole(HallRole role)
    {
        if (currentBuildData.roleData == null)
        {
            Debug.LogError("房间角色空间出错");
        }
        if (currentBuildData.buildingData.RoomType == RoomType.Training)
        {
            ChickAddTrainRole(role.RoleData);
        }
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] == null)
            {
                currentBuildData.roleData[i] = role.RoleData;
                Vector3 point = new Vector3(transform.position.x + (1.2f * (i + 1)), transform.position.y + 0.3f, role.transform.position.z);
                LocalServer.instance.RoleChangeRoom(role.RoleData, currentBuildData.id);
                role.transform.position = point;
                role.ChangeType(RoomName);
                if (role.RoleData.currentRoom != null)
                {
                    role.RoleData.currentRoom.RemoveRole(role);
                }
                role.RoleData.currentRoom = this;
                return true;
            }
        }

        //这边筛选出属性较低的更换位置
        int index = currentBuildData.ScreenAllYeild(NeedAttribute, false);
        HallRole oldRole = HallRoleMgr.instance.GetRole(currentBuildData.roleData[index]);
        if (role.RoleData.currentRoom != null)
        {
            Debug.Log("切换房间 调换角色");
            HallRoleMgr.instance.RoleChangeRoom(role, oldRole);
        }
        else
        {
            Debug.Log("当前角色原房间为空 进入漫游状态");
            RoomMgr room = MainCastle.instance.FindRoom(BuildRoomName.ThroneRoom);
            HallRoleData data = currentBuildData.roleData[index];
            HallRole roleTemp = HallRoleMgr.instance.GetRole(data);
            room.AddRole(roleTemp);
            AddRole(role);
        }
        return true;
    }
    public virtual bool AddRole(HallRole role, int index)
    {
        if (currentBuildData.roleData[index] == null)
        {
            currentBuildData.roleData[index] = role.RoleData;
            Vector3 point = new Vector3(transform.position.x + (1.2f * (index + 1)), transform.position.y + 0.3f, role.transform.position.z);
            LocalServer.instance.RoleChangeRoom(role.RoleData, currentBuildData.id);
            role.transform.position = point;
            role.ChangeType(RoomName);
            if (role.RoleData.currentRoom != null)
            {
                role.RoleData.currentRoom.RemoveRole(role);
            }
            role.RoleData.currentRoom = this;
            return true;
        }
        HallRole oldRole = HallRoleMgr.instance.GetRole(currentBuildData.roleData[index]);
        if (role.RoleData.currentRoom != null)
        {
            Debug.Log("切换房间 调换角色");
            HallRoleMgr.instance.RoleChangeRoom(role, oldRole);
        }
        else
        {
            Debug.Log("当前角色原房间为空 进入漫游状态");
            role.RoleData.currentRoom.RemoveRole(role);
            HallRoleData data = currentBuildData.roleData[index];
            HallRole roleTemp = HallRoleMgr.instance.GetRole(data);
            roleTemp.ChangeType(BuildRoomName.Nothing);
            currentBuildData.roleData[index] = role.RoleData;
        }
        return true;

    }
    /// <summary>
    /// 删除房间内的角色
    /// </summary>
    /// <param name="role"></param>
    public void RemoveRole(HallRole role)
    {
        if (currentBuildData.buildingData.RoomType == RoomType.Training)
        {
            ChickRemoveTrainRole(role.RoleData);
        }
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] == role.RoleData)
            {
                currentBuildData.roleData[i] = null;
                return;
            }
        }
        Debug.LogError("没有找到要删除的角色");
    }

    public virtual void ThisRoomFunc() { }
    public virtual void RoomAwake() { GetCompoment(); }
    public virtual void ChickComplete() { }

    /// <summary>
    /// 房间开始施工时 角色功能
    /// </summary>
    public virtual void ChickConstructionStartRole()
    {
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] != null)
            {
                HallRoleData role = currentBuildData.roleData[i];
                if (role.TrainType == RoleTrainType.LevelUp)
                {
                    HallRoleMgr.instance.PauseTrain(role.trainIndex);
                }
            }
        }
    }
    /// <summary>
    /// 房间施工完成 角色功能
    /// </summary>
    public virtual void ChickConstructionCompleteRole()
    {
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] != null)
            {
                HallRoleData role = currentBuildData.roleData[i];
                if (role.TrainType == RoleTrainType.Nothing)
                {
                    ChickAddTrainRole(role);
                }
                else if (role.TrainType == RoleTrainType.LevelUp)
                {
                    HallRoleMgr.instance.ContinueTrain(role.trainIndex);
                }
            }
        }
    }
    /// <summary>
    /// 添加角色时角色功能
    /// </summary>
    /// <param name="role"></param>
    public virtual void ChickAddTrainRole(HallRoleData role)
    {
        if (ConstructionType) //如果在施工 跳过
        {
            return;
        }
        switch (RoomName)
        {
            case BuildRoomName.FighterRoom:
                if (role.RoleLevel[0].Level <= currentBuildData.buildingData.Param2)
                {
                    HallRoleMgr.instance.StartTrain(role, TrainType.Fight);
                    return;
                }
                break;
            case BuildRoomName.Mint:
                if (role.RoleLevel[1].Level <= currentBuildData.buildingData.Param2)
                {
                    HallRoleMgr.instance.StartTrain(role, TrainType.Gold);
                    return;
                }
                break;
            case BuildRoomName.Kitchen:
                if (role.RoleLevel[2].Level <= currentBuildData.buildingData.Param2)
                {
                    HallRoleMgr.instance.StartTrain(role, TrainType.Food);
                    return;
                }
                break;
            case BuildRoomName.Laboratory:
                if (role.RoleLevel[3].Level <= currentBuildData.buildingData.Param2)
                {
                    HallRoleMgr.instance.StartTrain(role, TrainType.Mana);
                    return;
                }
                break;
            case BuildRoomName.Crafting:
                if (role.RoleLevel[4].Level <= currentBuildData.buildingData.Param2)
                {
                    HallRoleMgr.instance.StartTrain(role, TrainType.Wood);
                    return;
                }
                break;
            case BuildRoomName.Foundry:
                if (role.RoleLevel[5].Level <= currentBuildData.buildingData.Param2)
                {
                    HallRoleMgr.instance.StartTrain(role, TrainType.Iron);
                    return;
                }
                break;
            default:
                break;
        }
        //如果不能升级说明等级到达上限了
        role.TrainType = RoleTrainType.MaxLevel;
    }
    /// <summary>
    /// 删除角色时角色功能
    /// </summary>
    /// <param name="role"></param>
    public virtual void ChickRemoveTrainRole(HallRoleData role)
    {
        if (role.TrainType == RoleTrainType.LevelUp)
        {
            HallRoleMgr.instance.StopTrain(role);
        }
        else if (role.TrainType == RoleTrainType.Complete)
        {
            UIRoleTipGroup.instance.CloseIcon(role.id);
        }

        role.TrainType = RoleTrainType.Nothing;
    }

    public void UpdateProduceIcon(Sprite IconBG)
    {

    }

    protected void GetCompoment()
    {
        disTip = this.transform.Find("RoomFrame/SelectSign").gameObject;
        roomLock = this.transform.Find("RoomTypes/RoomLock").gameObject;
        RoomProp = this.transform.Find("RoomTypes/RoomProp").gameObject;
    }

    #region ProductionType
    public virtual void ShowHarvest()
    {
        IsHarvest = true;
    }

    public virtual void ChickRoomStock()
    {
        Debug.Log(currentBuildData.Stock);
        if (currentBuildData.Stock < 1)
        {
            Debug.Log("资源被取走了");
            IsHarvest = false;
        }
        else
        {
            Debug.Log("部分资源被取走了");
        }
    }
    #endregion
}
