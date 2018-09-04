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
    public bool isMerge = false;

    /// <summary>
    /// 移动提示框位置 并扩大提示框
    /// </summary>
    /// <param name="point">空位数据</param>
    /// <param name="size">建筑数据</param>
    /// <param name="startPoint">坐标数据</param>
    public void UpdateTip(EmptyPoint point, BuildingData data, BuildPoint[,] points)
    {
        int size = data.RoomSize;
        //如果房间相同并且之前房间可合并 那么提示可并入
        if (point.buildingData != null && point.buildingData.RoomType == data.RoomType
            && point.buildingData.RoomMerge && data.RoomMerge)
        {
            isMerge = true;
            //显示并入框

        }
        emptyPoint = point;
        roomSize = size;
        Vector2 tsSize = new Vector2(width * size, high);
        sr.size = tsSize;//图片大小
        bc.size = tsSize;//碰撞框大小
        transform.localPosition = new Vector3(width * (size * 0.5f), high * 0.5f);
        if (point.startPoint.x < point.endPoint)
        {
            parentPoint.position = points[(int)point.startPoint.x, (int)point.startPoint.y].pointWall.position;
        }
        else
        {
            parentPoint.position = points[(int)point.startPoint.x - size + 1, (int)point.startPoint.y].pointWall.position;
        }
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
