using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHelper : TSingleton<GameHelper>
{
    public static float high = 2.7f;//默认最小房间高度
    public static float width = 1.2f;//默认最小房间宽度
    public bool ServerInfo = false;

    public Vector2 GetPoint(Canvas canvas, Vector3 point)
    {
        Vector2 pos;
        RectTransform rt = canvas.transform as RectTransform;
        Vector3 v3 = Camera.main.WorldToScreenPoint(point);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, v3, canvas.worldCamera, out pos);
        return pos;
    }

    public int GetNeedDiamondsOfTime(int time)
    {
        return (int)(time * 0.01f);
    }

    public float ChickRoleMovePoint(Vector2 rolePoint, int roomSize)
    {
        return (width * roomSize) / 2;
    }
}


public class ItemGridHelp
{
    public int itemId;
    public ItemType type;

    public ItemGridHelp(int itemId, ItemType type)
    {
        this.itemId = itemId;
        this.type = type;
    }
}