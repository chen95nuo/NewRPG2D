/*
 * 房屋操作因需要点击房屋判断房屋类型出现相应UI
 * 所以该脚本用于点击后获得房间类型与UI交互
 * 并且该脚本内存有房屋所占用墙的引用及当前房屋所占坐标系
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMgr : MonoBehaviour
{

    private BuildRoomType roomType;//房间类型
    private Vector2 roomPoint;//当前房间位置
    private Vector2 startPoint;//起点坐标
    private int endPoint;//终点坐标

    private Transform[] wall;

    private EmptyPoint emptyPoint;
    private BuildingData buildingData;

    public int maxRoomSize = 9;
    public void UpdateBuilding(BuildPoint[,] buildPoint, List<EmptyPoint> points, EmptyPoint point, BuildingData data)
    {
        emptyPoint = point;
        buildingData = data;

        wall = new Transform[data.RoomSize];
        ChickLeftOrRight(buildPoint, points);

        //如果是梯子那么检测上下
        if (data.RoomType == BuildRoomType.Stairs)
            ChickUpOrDown(buildPoint, points);
    }

    private void ChickLeftOrRight(BuildPoint[,] buildPoint, List<EmptyPoint> points)
    {
        int startX = (int)emptyPoint.startPoint.x;
        int startY = (int)emptyPoint.startPoint.y;
        int endX = emptyPoint.endPoint;
        int index = 0;
        int endPoint = 0;
        Vector2 startPoint = new Vector2();
        if (startX < endX)//向右
        {
            for (int i = startX; i < startX + buildingData.RoomSize + 9; i++)
            {
                //如果当前遍历的对象已用或为空 那么退出本次循环
                if (buildPoint[i, startY] == null
                    || buildPoint[i, startY].pointType != BuildingType.Wall)
                {
                    break;
                }
                if (i < startX + buildingData.RoomSize)
                {
                    buildPoint[i, startY].pointType = BuildingType.Full;
                    wall[i - startX] = buildPoint[i, startY].pointWall;
                    wall[i - startX].Translate((Vector3.back * 1000));
                }
                else if (buildPoint[i, startY].pointType == BuildingType.Wall)
                {
                    index++;
                }
            }
            endPoint = startX + buildingData.RoomSize + index;
            startPoint = new Vector2(startX + buildingData.RoomSize, startY);
        }
        else
        {
            //向左遍历到最大数量
            for (int i = startX; i > startX - buildingData.RoomSize - 9; i--)
            {
                if (i < 0 || buildPoint[i, startY] == null
                    || buildPoint[i, startY].pointType != BuildingType.Wall)
                {
                    break;
                }
                if (i > startX - buildingData.RoomSize)
                {
                    buildPoint[i, startY].pointType = BuildingType.Full;
                    wall[startX - i] = buildPoint[i, startY].pointWall;
                    wall[startX - i].Translate((Vector3.back * 1000));
                }
                else if (buildPoint[i, startY].pointType == BuildingType.Wall)
                {
                    index++;
                }
            }
            endPoint = startX - buildingData.RoomSize - index;
            startPoint = new Vector2(startX - buildingData.RoomSize, startY);
        }
        EmptyPoint empty = new EmptyPoint(startPoint, endPoint, index);
        Debug.Log("空位起点 :" + (index));
        empty.buildingData = buildingData;
        points.Add(empty);
    }

    private void ChickUpOrDown(BuildPoint[,] buildPoint, List<EmptyPoint> points)
    {
        int startX = (int)emptyPoint.startPoint.x;
        int startY = (int)emptyPoint.startPoint.y;
        Vector2 startPoint = new Vector2();
        if (buildPoint[startX, startY + 1] != null
            && buildPoint[startX, startY + 1].pointType == BuildingType.Wall)
        {
            startPoint = new Vector2(startX, startY + 1);
            EmptyPoint empty = new EmptyPoint(startPoint, startX, 1);
            points.Add(empty);
        }
        if ((startY - 1 >= 0 && buildPoint[startX, startY - 1] != null
            && buildPoint[startX, startY - 1].pointType == BuildingType.Wall))
        {
            startPoint = new Vector2(startX, startY - 1);
            EmptyPoint empty = new EmptyPoint(startPoint, startX, 1);
            points.Add(empty);
        }
        //因为上下的距离皆为0 上方左右遍历只会有左右侧则不便利 在此单独遍历
        //仅上下楼梯可用
        if (startX == emptyPoint.endPoint)
        {
            int index = 0;
            int endX = emptyPoint.endPoint;
            for (int i = startX + 1; i < startX + buildingData.RoomSize + 9; i++)
            {
                //如果当前遍历的对象已用或为空 那么退出本次循环
                if (buildPoint[i, startY] == null
                    || buildPoint[i, startY].pointType != BuildingType.Wall)
                {
                    break;
                }
                else if (buildPoint[i, startY].pointType == BuildingType.Wall)
                {
                    index++;
                }
            }
            endPoint = startX + buildingData.RoomSize + index;
            startPoint = new Vector2(startX + buildingData.RoomSize, startY);
            EmptyPoint empty = new EmptyPoint(startPoint, endPoint, index);
            points.Add(empty);
        }
    }
}
