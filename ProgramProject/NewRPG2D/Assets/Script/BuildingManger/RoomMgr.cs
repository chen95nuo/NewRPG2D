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

    public BuildRoomType roomType;//房间类型
    public Vector2 buidStartPoint;//起点坐标
    public int buildEndPoint;//终点坐标

    private BuildPoint[] wall;

    private EmptyPoint emptyPoint;
    private BuildingData buildingData;
    private bool isUsed = true;
    private EmptyPoint[] emptyPoints = new EmptyPoint[4]; //左,右,上,下的空位;
    public RoomMgr[] nearbyRoom = new RoomMgr[4]; // 附近的房间 左,右,上,下;
    public RoomMgr[] roomDependency = new RoomMgr[4];//链接依赖项，右,左,下,上;
    public GameObject[] littleTip = new GameObject[4];//右左下上
    public int maxRoomSize = 9;

    public bool mainLink = false;//主链接
    public bool linkType = false;//连接状态
    public GameObject disTip;//断开连接的标签


    private void Awake()
    {
        for (int i = 0; i < littleTip.Length; i++)
        {
            if (littleTip[i] != null)
            {
                littleTip[i].SetActive(false);
            }
        }
    }
    /// <summary>
    /// 新建建筑
    /// </summary>
    /// <param name="point"></param>
    public void UpdateBuilding(EmptyPoint point)
    {
        disTip.SetActive(false);//关闭断开图标

        emptyPoint = point;
        buildingData = CastleMgr.instance.buildingData;
        roomType = buildingData.RoomType;


        //将房间这一段墙壁移出 给这一段空间添加该房间引用
        int startX = (int)emptyPoint.startPoint.x;
        int startY = (int)emptyPoint.startPoint.y;
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
            wall[i - startX] = CastleMgr.instance.buildPoint[i, startY];
            wall[i - startX].pointType = BuildingType.Full;
            wall[i - startX].pointWall.Translate((Vector3.back * 1000));
            wall[i - startX].roomMgr = this;
        }

        ChickLeftOrRight(CastleMgr.instance.buildPoint);

        CastleMgr.instance.room.Add(this.gameObject);
        AddConnection();
    }

    /// <summary>
    /// 附近建筑被删除重新检测附近空位
    /// </summary>
    private void UpdateBuilding()
    {
        //删除当前建筑提供的位置信息
        for (int i = 0; i < emptyPoints.Length; i++)
        {
            if (emptyPoints[i] != null)
            {
                CastleMgr.instance.emptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
        nearbyRoom = new RoomMgr[4];
        //重新检测附近空位
        ChickLeftOrRight(CastleMgr.instance.buildPoint);
    }

    /// <summary>
    /// 左右
    /// </summary>
    /// <param name="buildPoint"></param>
    /// <param name="points"></param>
    protected void ChickLeftOrRight(BuildPoint[,] buildPoint)
    {
        int startX = (int)emptyPoint.startPoint.x;//当前房间开始位置
        int startY = (int)emptyPoint.startPoint.y;//当前房间楼层
        int endX = (int)emptyPoint.startPoint.x + buildingData.RoomSize;//当前房间结束位置
        int endPoint = 0;
        Vector2 startPoint = new Vector2();

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
            if (buildPoint[endX + i, startY] != null && right
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
            startPoint = new Vector2(startX - leftIndex, startY);
            endPoint = startX;
            EmptyPoint empty = new EmptyPoint(startPoint, endPoint, leftIndex, this);
            emptyPoints[0] = empty;
        }
        else
        {
            if (startX - 1 >= 0 && buildPoint[startX - 1, startY] != null &&
                buildPoint[startX - 1, startY].pointType == BuildingType.Full)
            {
                nearbyRoom[0] = buildPoint[startX - 1, startY].roomMgr;
                buildPoint[startX - 1, startY].roomMgr.nearbyRoom[1] = this;
                //if (buildPoint[startX - 1, startY].roomMgr.linkType == true)
                //{
                //    linkType = true;
                //    roomDependency[1] = buildPoint[startX - 1, startY].roomMgr;
                //}
            }
        }
        //右侧空位
        if (rightIndex > 0)
        {
            startPoint = new Vector2(endX, startY);
            endPoint = endX + rightIndex;
            EmptyPoint empty = new EmptyPoint(startPoint, endPoint, rightIndex, this);
            emptyPoints[1] = empty;
        }
        else
        {
            if (buildPoint[endX, startY] != null &&
                buildPoint[endX, startY].pointType == BuildingType.Full)
            {
                nearbyRoom[1] = buildPoint[endX, startY].roomMgr;
                buildPoint[endX, startY].roomMgr.nearbyRoom[0] = this;
                //if (buildPoint[endX, startY].roomMgr.linkType == true)
                //{
                //    linkType = true;
                //    roomDependency[0] = buildPoint[endX, startY].roomMgr;
                //}
            }
        }

        if (roomType == BuildRoomType.Stairs)
            ChickUpOrDown(CastleMgr.instance.buildPoint);
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
        int startX = (int)emptyPoint.startPoint.x;
        int startY = (int)emptyPoint.startPoint.y;
        Vector2 startPoint = new Vector2();
        if (buildPoint[startX, startY + 1] != null
            && buildPoint[startX, startY + 1].pointType == BuildingType.Wall)
        {
            startPoint = new Vector2(startX, startY + 1);
            EmptyPoint empty = new EmptyPoint(startPoint, startX + 1, 1, this);

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
            startPoint = new Vector2(startX, startY - 1);
            EmptyPoint empty = new EmptyPoint(startPoint, startX + 1, 1, this);

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
                CastleMgr.instance.emptyPoint.Add(emptyPoints[i]);
            }
        }

        if (linkType == true)//如果我自身链接正常则检查附近连接失败的房间 讲本房间添加进去并让其自检
        {
            for (int i = 0; i < nearbyRoom.Length; i++)
            {
                if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
                {
                    nearbyRoom[i].roomDependency[i] = this;
                    nearbyRoom[i].CheckConnection(this);
                }
            }
        }
    }
    /// <summary>
    /// 删除建筑
    /// </summary>
    public void RemoveBuilding()
    {
        //将建筑的使用信息改为停用 将墙面移动回原位
        CastleMgr.instance.room.Remove(this.gameObject);

        this.gameObject.transform.position = new Vector2(-1000, -1000);
        CastleMgr.instance.removeRoom.Add(this.gameObject);

        int startX = (int)emptyPoint.startPoint.x;
        int startY = (int)emptyPoint.startPoint.y;

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
                CastleMgr.instance.emptyPoint.Remove(emptyPoints[i]);
            }
        }
        emptyPoints = new EmptyPoint[4];
        linkType = false; //断开自身链接
        for (int i = 0; i < nearbyRoom.Length; i++)
        {
            if (nearbyRoom[i] != null)
            {
                nearbyRoom[i].UpdateBuilding();
                nearbyRoom[i].CheckConnection(this);//通知附近房间检查自身链接
            }
        }
    }

    /*
     * 
     */

    /// <summary>
    /// 检查链接
    /// </summary>
    /// <param name="room">发送消息的房间</param>
    /// <param name="number">位置</param>
    protected void CheckConnection(RoomMgr room)
    {
        //有附近所有房间的链接 如果是主房间则不需要与其他房间链接
        //问题 新建房屋如何告诉已建成的房屋添加新节点
        //如果本房间断开则通知附近房间自检
        //如果本房间没有断开则无视
        if (mainLink)
        {
            disTip.SetActive(false);
            return;
        }

        
        
        int lineIndex = 0;
        int offIndex = 0;

        int index = 0;
        for (int i = 0; i < nearbyRoom.Length; i++)
        {
            //如果这个房间是连接状态 检查他是否与本房间连接
            if (nearbyRoom[i] != null && nearbyRoom[i].linkType == true)
            {
                for (int j = 0; j < nearbyRoom[i].roomDependency.Length; j++)
                {
                    //判断连接点是否有效
                    if (nearbyRoom[i].roomDependency[j] != null && nearbyRoom[i].roomDependency[j] != this)
                    {
                        lineIndex++;
                        index = i;
                    }
                }
            }
            //如果当前这个房间断开连接了 记录数量
            else if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
            {
                offIndex++;

            }
            //如果附近是空的 但是链接里却有 那么判断该物体已经被移除
            else if (nearbyRoom[i] == null)
            {
                switch (i)
                {
                    case 0:
                        if (roomDependency[1] == null)
                        {
                            break;
                        }
                        littleTip[1].SetActive(false);
                        roomDependency[1] = null;
                        break;
                    case 1:
                        if (roomDependency[0] == null)
                        {
                            break;
                        }
                        littleTip[0].SetActive(false);
                        roomDependency[0] = null;
                        break;
                    case 2:
                        if (roomDependency[3] == null)
                        {
                            break;
                        }
                        littleTip[3].SetActive(false);
                        roomDependency[3] = null;
                        break;
                    case 3:
                        if (roomDependency[2] == null)
                        {
                            break;
                        }
                        littleTip[2].SetActive(false);
                        roomDependency[2] = null;
                        break;
                    default:
                        break;
                }
            }
        }
        Debug.Log(lineIndex);
        if (lineIndex > 0)
        {
            //如果只有一个有效链接 检查对方内是否有本链接 是的话清除节点并断开 不是或对方任有其他节点则连接
            if (lineIndex == 1)
            {
                Debug.Log("让对方运行自检");
                //如果对方连接有效 那么自身激活连接
                bool isTrue = nearbyRoom[index].ChickConnection(this, 1);
            }
        }
        //如果一个有效连接都没有 就是断开了
        else
        {
            linkType = false;

            roomDependency = new RoomMgr[4];
            for (int i = 0; i < littleTip.Length; i++)
            {
                if (littleTip[i] != null)
                {
                    littleTip[i].SetActive(false);
                }
            }

            for (int i = 0; i < nearbyRoom.Length; i++)
            {
                if (nearbyRoom[i] != null)
                {
                    nearbyRoom[i].ChickConnection(this, 1);
                }
            }
        }

        #region 暂停使用2
        ////只有链接
        //if (lineIndex > 0 && offIndex <= 0)
        //{
        //    Debug.Log("只有连接");
        //    if (linkType == true)
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        linkType = true;
        //    }
        //}
        ////只有断开 我断开了就通知附近的房间自检
        //else if (lineIndex <= 0 && offIndex > 0)
        //{
        //    Debug.Log("只有断开");
        //    linkType = false;
        //    for (int i = 0; i < nearbyRoom.Length; i++)
        //    {
        //        //排除发出这个消息的对象，防止死循环
        //        if (nearbyRoom[i] != null && nearbyRoom[i] != room)
        //        {
        //            nearbyRoom[i].CheckConnection(this);
        //        }
        //    }
        //}
        //else if (lineIndex > 0 && offIndex > 0)//有断开有连接
        //{
        //    Debug.Log("有断开有链接");
        //    if (lineIndex == 1)
        //    {
        //        Debug.Log("只有一个连接");
        //        for (int i = 0; i < roomDependency.Length; i++)
        //        {
        //            if (roomDependency[i] != null && roomDependency[i].linkType == true)
        //            {
        //                for (int j = 0; j < roomDependency[i].roomDependency.Length; j++)
        //                {
        //                    if (roomDependency[i].roomDependency[j] == this)
        //                    {
        //                        roomDependency[i].roomDependency[j] = null;
        //                        roomDependency[i].littleTip[j].SetActive(false);
        //                        roomDependency[i].CheckConnection(this);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else if (lineIndex >= 1) //超过一个链接
        //    {
        //        Debug.Log("大于一个连接");
        //    }
        //    //找到那些断开的链接
        //    for (int i = 0; i < nearbyRoom.Length; i++)
        //    {
        //        if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
        //        {
        //            nearbyRoom[i].roomDependency[i] = this;
        //            nearbyRoom[i].CheckConnection(this);
        //        }
        //    }

        //}
        //else if (lineIndex <= 0 && offIndex <= 0)
        //{
        //    Debug.LogError("附近无建筑");
        //    linkType = false;
        //} 
        #endregion

        #region 暂停使用
        ////检查所有连接点判断自身是否断开
        //int link = 0;
        //for (int i = 0; i < nearbyRoom.Length; i++)
        //{
        //    //如果有链接可以使用则记录数量
        //    if (nearbyRoom[i] != null && nearbyRoom[i].linkType == true)
        //    {
        //        //排除这个链接虽然是链接状态但是连接对象却是本房间
        //        for (int j = 0; j < nearbyRoom[i].roomDependency.Length; j++)
        //        {
        //            if (nearbyRoom[i].roomDependency[j] != null && nearbyRoom[i].roomDependency[j] != this)
        //            {
        //                link++;
        //            }
        //        }
        //    }
        //    //如果该链接不可使用则清除
        //    else
        //    {
        //        roomDependency[i] = null;
        //    }
        //}
        ////如果自己没断开
        //if (link > 0)
        //{
        //    linkType = true;
        //    //遍历附近房间若有房间断开将自身添加进去
        //    for (int i = 0; i < nearbyRoom.Length; i++)
        //    {
        //        //若该房间断开 将本房间添加进去并为其添加节点再让其自检
        //        if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false && nearbyRoom[i] != room)
        //        {
        //            nearbyRoom[i].roomDependency[i] = this;
        //            nearbyRoom[i].CheckConnection(this);
        //        }
        //    }
        //}
        ////如果自己断开了
        //else
        //{
        //    linkType = false;
        //    for (int i = 0; i < nearbyRoom.Length; i++)
        //    {
        //        if (nearbyRoom[i] != null && nearbyRoom[i] != room)
        //        {
        //            nearbyRoom[i].CheckConnection(this);
        //        }
        //    }
        //}

        #endregion
        if (linkType == false)
        {
            disTip.SetActive(true);
        }
        else
        {
            disTip.SetActive(false);
        }
    }
    //检查连接
    protected bool ChickConnection(RoomMgr data, int n)
    {
        if (mainLink)
        {
            disTip.SetActive(false);
            return true;
        }

        //判断剩余多少个链接
        int index = 0;
        RoomMgr room = null;
        for (int i = 0; i < roomDependency.Length; i++)
        {
            //如果等于发起消息的 那么清除他
            if (roomDependency[i] != null && roomDependency[i] == data)
            {
                roomDependency[i] = null;
                littleTip[i].SetActive(false);
                Debug.Log("清除DATA数据");
            }
            //如果是连接状态
            else if (roomDependency[i] != null && roomDependency[i].linkType == true)
            {
                if (index == 0)
                {
                    room = roomDependency[i];
                }
                index++;
            }
            //如果是断开状态 清除
            else if (roomDependency[i] != null && roomDependency[i].linkType == false)
            {
                roomDependency[i] = null;
                littleTip[i].SetActive(false);
            }
        }
        Debug.Log("自检数量" + index);
        if (index == 1)
        {
            room.ChickConnection(this, 1);
            return false;
        }
        else if (index == 0)
        {
            linkType = false;
            disTip.SetActive(true);
            for (int i = 0; i < nearbyRoom.Length; i++)
            {
                if (nearbyRoom[i] != null && nearbyRoom[i].linkType == true)
                {
                    nearbyRoom[i].CheckConnection(this);
                }
            }
            return false;
        }
        return true;
    }

    protected void AddConnection()
    {
        int lineIndex = 0;
        int offIndex = 0;
        //检查周边所有房间的连接状态
        for (int i = 0; i < nearbyRoom.Length; i++)
        {
            if (nearbyRoom[i] != null && nearbyRoom[i].linkType == true)
            {
                lineIndex++;
                //roomDependency[i] = nearbyRoom[i]; //如果对方是连接状态那么添加到本地可连接列表

                switch (i)
                {
                    case 0:
                        littleTip[1].SetActive(true);
                        roomDependency[1] = nearbyRoom[i];
                        break;
                    case 1:
                        littleTip[0].SetActive(true);
                        roomDependency[0] = nearbyRoom[i];
                        break;
                    case 2:
                        littleTip[3].SetActive(true);
                        roomDependency[3] = nearbyRoom[i];
                        break;
                    case 3:
                        littleTip[2].SetActive(true);
                        roomDependency[2] = nearbyRoom[i];
                        break;
                    default:
                        break;
                }

            }
            else if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
            {
                offIndex++;
            }
        }
        //如果 1、只有断开 2、只有链接 3、有链接有断开 4、全部没有....
        if (lineIndex <= 0 && offIndex > 0)
        {
            linkType = false;
        }
        else if (lineIndex > 0 && offIndex <= 0)
        {//只有链接 连接数量大于1
            linkType = true;
            if (lineIndex > 1)
            {
                for (int i = 0; i < nearbyRoom.Length; i++)
                {
                    if (nearbyRoom[i] != null)
                    {
                        nearbyRoom[i].roomDependency[i] = this;
                        nearbyRoom[i].littleTip[i].SetActive(true);
                    }
                }
            }
        }
        else if (lineIndex > 0 && offIndex > 0)
        {//有链接也有断开 连接大于1 断开等于1 断开大于1
            linkType = true;
            if (lineIndex > 1)
            {
                for (int i = 0; i < nearbyRoom.Length; i++)
                {
                    if (nearbyRoom != null)
                    {
                        nearbyRoom[i].roomDependency[i] = this;
                        nearbyRoom[i].littleTip[i].SetActive(true);
                    }
                }
            }
            for (int i = 0; i < nearbyRoom.Length; i++)
            {
                if (nearbyRoom[i] != null && nearbyRoom[i].linkType == false)
                {
                    nearbyRoom[i].roomDependency[i] = this;
                    nearbyRoom[i].littleTip[i].SetActive(true);
                    nearbyRoom[i].CheckConnection(this);
                }
            }
        }
        if (mainLink)
        {
            linkType = true;
            littleTip[1].SetActive(true);
        }
        if (linkType == false)
        {
            disTip.SetActive(true);
        }
        else
        {
            disTip.SetActive(false);
        }
    }

    protected abstract void ThisRoomFunc();
}
