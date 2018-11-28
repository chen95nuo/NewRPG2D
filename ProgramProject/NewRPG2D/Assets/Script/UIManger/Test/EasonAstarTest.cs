using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasonAstar
{
    public class EasonAstarTest : TSingleton<EasonAstarTest>
    {
        List<Node> Open = new List<Node>();
        List<Node> Close = new List<Node>();
        List<Node> All = new List<Node>();

        Node start;
        Node end;

        BuildPoint[,] map;

        private BuildPoint[,] Map
        {
            get
            {
                if (map == null)
                {
                    map = MainCastle.instance.buildPoint;
                }
                return map;
            }
        }

        void Reset()
        {
            Open.Clear();
            Close.Clear();
        }

        public List<Vector2> Find(BuildPoint[,] grids, Vector2 from, Vector2 to)
        {
            if (from == to)
            {
                List<Vector2> res = new List<Vector2>();
                Vector2 endPoint = map[(int)to.x, (int)to.y].roomMgr.RolePoint.position;
                res.Add(endPoint);
                return res;
            }

            Reset();
            map = grids;

            //初始化Start
            start = GetNode(from, to);
            end = GetNode(to, to);
            Open.Add(start);

            while (Open.Count != 0)
            {
                var N = GetMinFNode(); //当前最近的点 和值最小的点
                var dirs = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
                for (int i = 0; i < N.type; i++)
                {
                    bool isClose;
                    bool isNull;
                    int nodeType;
                    var X = GetNode(N.point + dirs[i], out isClose, out isNull, out nodeType);//检测下一个点的位置
                    if (isNull)
                    {
                        X = CryNewNode(N.point + dirs[i], nodeType, N);
                    }
                    //如果当前这个方向的建筑是空 跳过
                    if (X == null)
                    {
                        continue;
                    }
                    //如果当前这个方向的建筑是终点
                    else if (X.point == end.point)
                    {
                        X.last = N;
                        List<Vector2> res = new List<Vector2>();
                        var cur = X;
                        Vector2 endPoint = map[(int)cur.point.x, (int)cur.point.y].roomMgr.RolePoint.position;
                        res.Add(endPoint);

                        while (cur.last != null)
                        {
                            if (map[(int)cur.point.x, (int)cur.point.y].roomMgr.RoomName == BuildRoomName.Stairs)
                            {
                                Vector2 point = map[(int)cur.point.x, (int)cur.point.y].roomMgr.RolePoint.position;
                                res.Add(point);
                            }
                            //res.Add(cur.point);
                            cur = cur.last;
                        }
                        res.Reverse();
                        Debug.Log("完成了");
                        return res;
                    }
                    //在Open内
                    else if (!isClose)
                    {
                        UpdateFValue(N, X); //在Open内重新校准位置
                        Open.Add(X);
                        continue;
                    }
                    //在Close内
                    else if (isClose)
                    {
                        bool reOpen = UpdateFValue(N, X);
                        if (reOpen) //Close内的需要重新转到Open内
                        {
                            Open.Add(X);
                            Close.Remove(X);
                        }
                        continue;
                    }
                }
                Debug.DrawLine((Vector3)N.point, (Vector3)N.point + Vector3.up, Color.red, 1);
                Close.Add(N);
                Open.Remove(N);
            }
            return null;
        }

        /// <summary>
        /// 对X值重新调整
        /// </summary>
        /// <param name="N">当前正在Close的点</param>
        /// <param name="X">刷新值的点</param>
        /// <returns>返回是否需要重新OPEN</returns>
        bool UpdateFValue(Node N, Node X)
        {
            if (X.G > N.G + 1) X.G = N.G + 1; //如果该点在Open内 从n-x 需要n+1 但是远x>n+1 那么x改为当前路线n+1
            if (X.H <= 0) Node.distance(end.point, X);//重新校准H点位置
            var newF = X.G + X.H;
            if (newF < X.F)
            {
                X.F = newF;
                X.last = N;
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 获取最小和值点 循环全部待定的点
        /// </summary>
        /// <returns></returns>
        Node GetMinFNode()
        {
            if (Open.Count == 0) return null;
            Node result = Open[0];
            foreach (var node in Open)
            {
                if (node.F < result.F)
                {
                    result = node;
                }
            }
            return result;
        }

        Node GetNode(Vector2 point, out bool isClose, out bool isNull, out int type)
        {
            int mapType = ChickMap(point);
            type = mapType;
            isNull = false;
            //判断无效地址
            if (mapType == 0)
            {
                isClose = false;
                return null;
            }
            //判断是不是被考虑的点;
            for (int i = 0; i < Open.Count; i++)
            {
                if (Open[i].point == point)
                {
                    isClose = false;
                    return Open[i];
                }
            }
            //判断是不是不在考虑的点
            for (int i = 0; i < Close.Count; i++)
            {
                if (Close[i].point == point)
                {
                    isClose = true;
                    return Close[i];
                }
            }
            isNull = true;
            isClose = false;
            return null;
        }

        Node GetNode(Vector2 point, Vector2 endPoint)
        {
            int mapType = ChickMap(point);
            Node node = new Node(point, mapType, null);
            Node.distance(endPoint, node);
            return node;
        }

        Node CryNewNode(Vector2 point, int mapType, Node N)
        {
            Node node = new Node(point, mapType, N);
            Node.distance(end.point, node);
            return node;
        }

        int ChickMap(Vector2 point)
        {
            int pointX = (int)point.x;
            int pointY = (int)point.y;

            //说明该位置为墙 或 为空 判断死路
            if (pointX < 0 || pointY < 0 || map[pointX, pointY] == null ||
                map[pointX, pointY].pointType == BuildingType.Wall ||
                map[pointX, pointY].pointType == BuildingType.Nothing)
            {
                return 0;
            }
            //说明该房间不为楼梯 只能左右
            else if (map[pointX, pointY].roomMgr.RoomName != BuildRoomName.Stairs)
            {
                return 2;
            }
            //为楼梯可上下
            else
            {
                return 4;
            }
        }
    }
}
public class Node
{
    public int H; //推算方向
    public int G; //起点到此的移动量
    public int F; //和值
    public Vector2 point; //当前位置
    public int type; //可移动范围
    public Node last; //上一个点

    public Node(Vector2 point, int type, Node last)
    {
        this.point = point;
        this.type = type;
        this.G = last == null ? 0 : last.G + 1;
        this.last = last;
    }

    public static void distance(Vector2 end, Node x)
    {
        x.H = (int)(Mathf.Abs(end.x - x.point.x) + Mathf.Abs(end.y - x.point.y));
        x.F = x.G + x.H;
    }
}

