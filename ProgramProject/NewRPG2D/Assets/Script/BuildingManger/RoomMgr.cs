/*
 * 房屋操作因需要点击房屋判断房屋类型出现相应UI
 * 所以该脚本用于点击后获得房间类型与UI交互
 * 并且该脚本内存有房屋所占用墙的引用及当前房屋所占坐标系
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomMgr : MonoBehaviour
{
    private CastleMgr castleMgr;//主城堡信息
    public int roomID;//用于区分第几个相同房间
    public BuildRoomType roomType;//房间类型
    public Vector2 buidStartPoint;//起点坐标
    public int buildEndPoint;//终点坐标
    [SerializeField]
    public BuildingData buildingData;

    private BuildPoint[] wall;

    private Vector2 startPoint;
    private bool isUsed = true;
    private EmptyPoint[] emptyPoints = new EmptyPoint[4]; //左,右,上,下的空位;
    public RoomMgr[] nearbyRoom = new RoomMgr[4]; // 附近的房间 左,右,上,下;
    public int maxRoomSize = 9;

    public bool mainLink = false;//主链接
    public bool linkType = false;//连接状态
    public bool isHarvest = false;//可否收获
    public bool roomFunc = false;//房间功能是否开启

    private List<RoomMgr> disconnectRoom = new List<RoomMgr>();

    [System.NonSerialized]
    public GameObject disTip;//断开连接的标签
    [System.NonSerialized]
    public GameObject roomLock;//房间选定框
    [System.NonSerialized]
    public GameObject roomProp;//资源获取框

    /// <summary>
    /// 将数据添加到服务器
    /// </summary>
    protected void SetUpInformation()
    {
        ServerBuildData data = new ServerBuildData();
        data.buildingData = this.buildingData;
        data.buildingPoint = this.startPoint;
        LocalServer.instance.AddRoom(data);
        castleMgr.serverRoom.Add(data);
    }
    /// <summary>
    /// 新建建筑
    /// </summary>
    /// <param name="point"></param>
    public void UpdateBuilding(Vector2 point, BuildingData data, CastleMgr castleMgr)
    {
        this.castleMgr = castleMgr;
        GetCompoment();
        disTip.SetActive(false);//关闭断开图标
        buildingData = data;
        startPoint = point;
        roomType = buildingData.RoomType;
        if (castleMgr.castleType == CastleType.main)//如果不是建造模式生成的建筑
        {
            roomFunc = true;//房间功能开启
            //将其添加到服务器
            SetUpInformation();
        }
        RoomAwake();
        //将房间这一段墙壁移出 给这一段空间添加该房间引用
        int startX = (int)startPoint.x;
        int startY = (int)startPoint.y;
        wall = new BuildPoint[buildingData.RoomSize];
        buidStartPoint = new Vector2(startX, startY);
        if (buidStartPoint == new Vector2(6, 1))
        {
            mainLink = true;
            linkType = true;
        }
        else
        {
            mainLink = false;
        }
        buildEndPoint = startX + buildingData.RoomSize;

        for (int i = startX; i < startX + buildingData.RoomSize; i++)
        {
            wall[i - startX] = castleMgr.buildPoint[i, startY];
            wall[i - startX].pointType = BuildingType.Full;
            wall[i - startX].pointWall.Translate((Vector3.back * 1000));
            wall[i - startX].roomMgr = this;
        }

        ChickLeftOrRight(castleMgr.buildPoint);

        castleMgr.rooms.Add(this);
        AddConnection();
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
                castleMgr.emptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
        nearbyRoom = new RoomMgr[4];
        //重新检测附近空位
        ChickLeftOrRight(castleMgr.buildPoint);
    }

    /// <summary>
    /// 左右
    /// </summary>
    /// <param name="buildPoint"></param>
    /// <param name="points"></param>
    protected void ChickLeftOrRight(BuildPoint[,] buildPoint)
    {
        int startX = (int)startPoint.x;//当前房间开始位置
        int startY = (int)startPoint.y;//当前房间楼层
        int endX = (int)startPoint.x + buildingData.RoomSize;//当前房间结束位置
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

        if (roomType == BuildRoomType.Stairs)
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
        int startX = (int)startPoint.x;
        int startY = (int)startPoint.y;
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
            && buildPoint[startX, startY + 1].roomMgr.roomType == BuildRoomType.Stairs)
        //如果上面位置不是空的且有房间且房间类型是楼梯 那么上方添加该房间
        {
            nearbyRoom[2] = buildPoint[startX, startY + 1].roomMgr;
            buildPoint[startX, startY + 1].roomMgr.nearbyRoom[3] = this;
            //if (buildPoint[startX, startY + 1].roomMgr.linkType == true)
            //{
            //    linkType = true;
            //    roomDependency[3] = buildPoint[startX, startY + 1].roomMgr;
            //}
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
            && buildPoint[startX, startY - 1].roomMgr.roomType == BuildRoomType.Stairs)
        {
            nearbyRoom[3] = buildPoint[startX, startY - 1].roomMgr;
            buildPoint[startX, startY - 1].roomMgr.nearbyRoom[2] = this;
            //if (buildPoint[startX, startY - 1].roomMgr.linkType == true)
            //{
            //    linkType = true;
            //    roomDependency[2] = buildPoint[startX, startY - 1].roomMgr;
            //}
        }

        UpdateEmptyPoint();
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
                castleMgr.emptyPoint.Add(emptyPoints[i]);
            }
        }
    }
    /// <summary>
    /// 删除建筑
    /// </summary>
    public void RemoveBuilding(BuildPoint[,] buildPoint)
    {
        //将建筑的使用信息改为停用 将墙面移动回原位
        castleMgr.rooms.Remove(this);

        this.gameObject.transform.position = new Vector2(-1000, -1000);
        castleMgr.removeRoom.Add(this.gameObject);

        int startX = (int)startPoint.x;
        int startY = (int)startPoint.y;

        for (int i = 0; i < wall.Length; i++)
        {
            wall[i].pointType = BuildingType.Wall;
            wall[i].pointWall.Translate(Vector3.forward * 1000);
            wall[i].roomMgr = null;
        }

        //删除当前建筑提供的位置信息
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            if (emptyPoints[i] != null)
            {
                castleMgr.emptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
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
                if (buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr != null)
                {
                    buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
                if (buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr != null)
                {
                    buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
            }
        }
        else if (nearbyRoom[0] != null && nearbyRoom[1] == null)
        {
            for (int i = 0; i < 9; i++)
            {
                if (buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr != null)
                {
                    buildPoint[(int)buidStartPoint.x - i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                }
                if (buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr != null)
                {
                    buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
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
                if (left && ((int)buidStartPoint.x - (i + 1) <= 0 || buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y] == null
                    || buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].pointType == BuildingType.Nothing))
                {
                    left = false;
                }
                else if (left && buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].roomMgr != null)
                {
                    buildPoint[(int)buidStartPoint.x - (i + 1), (int)buidStartPoint.y].roomMgr.UpdateBuilding();
                    left = false;
                }
                if (right && (buildPoint[buildEndPoint + i, (int)buidStartPoint.y] == null
                    || buildPoint[buildEndPoint + i, (int)buidStartPoint.y].pointType == BuildingType.Nothing))
                {
                    right = false;
                }
                else if (right && buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr != null)
                {
                    buildPoint[buildEndPoint + i, (int)buidStartPoint.y].roomMgr.UpdateBuilding();
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
                Debug.Log("主要 ：对方堵塞 判断己方连接是否有效");
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
                                Debug.Log("保存断开的房间信息");
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
                        Debug.LogError("走到边缘楼梯 楼梯可通往上下" + roomType);
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
    protected void AddConnection()
    {
        int linkIndex = 0;
        int offIndex = 0;

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

    public abstract void ThisRoomFunc();
    public abstract void RoomAwake();
    public abstract void GetRoomMaterial(int number);

    protected void GetCompoment()
    {
        Debug.Log("运行了");
        disTip = this.transform.Find("RoomFrame/SelectSign").gameObject;
        roomLock = this.transform.Find("RoomTypes/RoomLock").gameObject;
        roomProp = this.transform.Find("RoomTypes/RoomProp").gameObject;
    }
}
