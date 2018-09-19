/*
 * 建造提示框，需要获取该空位信息，空位起点信息，房间大小
 * 用于提示房屋建造
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTip : MonoBehaviour
{
    public int roomSize;//房间大小

    public SpriteRenderer sr;
    public BoxCollider bc;
    public Transform parentPoint;
    private float high = 10.8f;
    private float width = 4.77f;

    public EmptyPoint emptyPoint;
    public bool isMerge = false;//合并房间

    public int startX = 0;

    /// <summary>
    /// 移动提示框位置 并扩大提示框
    /// </summary>
    /// <param name="point">空位数据</param>
    /// <param name="size">建筑数据</param>
    /// <param name="startPoint">坐标数据</param>
    public bool UpdateTip(EmptyPoint point, List<EmptyPoint> emptyPoints, BuildingData data, CastleMgr castleMgr)
    {
        int size = data.RoomSize;
        //如果房间相同并且之前房间可合并 那么提示可并入
        //if (point.buildingData != null && point.buildingData.RoomType == data.RoomType
        //    && point.buildingData.RoomMerge && data.RoomMerge)
        //{
        //    isMerge = true;
        //    //显示并入框

        //}

        emptyPoint = point;
        roomSize = size;
        Vector2 tsSize = new Vector2(width * size, high);
        sr.size = tsSize;//图片大小
        bc.size = tsSize;//碰撞框大小
        transform.localPosition = new Vector3(width * (size * 0.5f), high * 0.5f);
        if (point.startPoint != null && point.roomData != null)
        {
            Debug.Log(point.startPoint + " " + point.roomData.buidStartPoint);
        }
        //如果创建这个位置的建筑的起点位置比这个位置的起点位置小 判定起点位置在右侧
        if (point.roomData == null || point.roomData.buidStartPoint.x < point.startPoint.x)
        {
            startX = (int)point.startPoint.x;
        }
        else
        {
            startX = point.endPoint - size;
        }
        int index = 0;
        //验证当前空位信息是否正确 若不正确 则更正 避免重复显示和错误显示
        for (int i = startX; i < startX + size; i++)
        {
            //如果是墙面则继续 如果已经是提示框了则判断两个提示框 如果已经是墙面了 那么表示格子并不够根据情况删除或修改当前空位信息
            switch (castleMgr.buildPoint[i, (int)point.startPoint.y].pointType)
            {
                case BuildingType.Nothing:
                    Debug.LogError("提示框探测到空墙");
                    break;
                case BuildingType.Wall:
                    //是墙面的话 正常 略过
                    if (castleMgr.buildPoint[i, (int)point.startPoint.y].tip == null)
                        index++;
                    break;
                case BuildingType.Full:
                    //是房间则表示格子不够更改空位信息
                    if (index > 0)//如果说还是有格子 只是变少了那么修改
                    {
                        Debug.LogError("空格内已有建筑只是变少了");

                        point.endPoint = i;
                        point.emptyNumber = index;
                        //清理之前更改的位置信息
                        for (int j = startX; j < index; j++)
                        {
                            castleMgr.buildPoint[i, (int)point.startPoint.y].tip = null;
                        }
                    }
                    else//如果没有格子了直接删除这个位置信息
                    {
                        Debug.LogError("空格内已有建筑没有格子了,空位信息" + i + "," + (int)point.startPoint.y);
                        //如果这是楼梯提供的且长度为1那么保留
                        if (point.roomData != null && point.roomData.RoomName == "Stairs" && point.emptyNumber == 1)
                        {
                            return false;
                        }
                        point.emptyNumber = 0;
                        //emptyPoints.Remove(point);
                    }
                    return false;
                default:
                    break;
            }
            if (castleMgr.buildPoint[i, (int)point.startPoint.y].tip != null)
            {
                //出现重复区域根据情况修改提示框状态
                //如果部分重叠判定左右，依左优先
                if (index > 0)
                {
                    Debug.LogError("空格内已有提示框依左,空位信息" + i + "," + (int)point.startPoint.y);
                    //如果本提示比已存在的更靠左 那么清除该提示
                    if (castleMgr.buildPoint[i, (int)point.startPoint.y].tip.startX > startX)
                    {
                        castleMgr.buildPoint[i, (int)point.startPoint.y].tip.RestartThisTip(castleMgr);
                    }
                    else//如果已存在的比较靠左 清除本提示
                    {
                        for (int j = startX; j < index; j++)
                        {
                            castleMgr.buildPoint[i, (int)point.startPoint.y].tip = null;
                        }
                        //清理之前更改的位置信息
                        return false;
                    }
                }
                else//如果完全重叠那么删除该信息 如何排除改次完全重叠但若房间足够小却不会重叠？
                {
                    Debug.LogError("空格内已有提示框完全重叠 不能删除,保留当前信息 排除出发点方向不同");

                    //emptyPoints.Remove(point);
                    return false;
                }
            }
            castleMgr.buildPoint[i, (int)point.startPoint.y].tip = this;
        }

        parentPoint.position = castleMgr.buildPoint[startX, (int)point.startPoint.y].pointWall.position;
        return true;
    }

    public void RestartThisTip(CastleMgr castleMgr)
    {
        for (int i = startX; i < startX + roomSize; i++)
        {
            castleMgr.buildPoint[i, (int)emptyPoint.startPoint.y].tip = null;
        }
        transform.position = new Vector2(-1000, -1000);
    }

    public void InstanceRoom(RoomMgr room, BuildingData data, CastleMgr castleMgr)
    {
        emptyPoint.startPoint = new Vector2(startX, emptyPoint.startPoint.y);
        room.UpdateBuilding(emptyPoint.startPoint, data, castleMgr);
    }

    ///// <summary>
    ///// 建造建筑
    ///// </summary>
    //public void UpdateBuild(Transform buildGroup)
    //{
    //    //判断是否合并
    //    if (isMerge && emptyPoint.buildingData.Level == buildData.Level)
    //    {
    //        //GameObject go = Resources.Load<GameObject>("");
    //        //isMerge = false;
    //        Debug.Log("合并房间");
    //    }
    //    else
    //    {
    //        GameObject go = Resources.Load<GameObject>("UIPrefab/Building/" + buildData.SprintName);
    //        go = Instantiate(go, buildGroup) as GameObject;
    //        go.transform.position = this.transform.position;

    //    }
    //}
}
