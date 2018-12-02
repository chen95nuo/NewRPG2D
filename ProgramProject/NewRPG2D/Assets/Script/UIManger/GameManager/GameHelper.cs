using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameHelper : TSingleton<GameHelper>
{
    public static float high = 2.7f;//默认最小房间高度
    public static float width = 1.2f;//默认最小房间宽度
    public bool ServerInfo = false;
    private int happiness = 0;

    public int Happiness
    {
        get
        {
            return happiness;
        }

        set
        {
            happiness = value;
            HallEventManager.instance.SendEvent<int>(HallEventDefineEnum.CheckHappiness, happiness);
        }
    }



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

    public BuildRoomName MaterialNameToBuildRoomName(MaterialName mat)
    {
        BuildRoomName name = BuildRoomName.Nothing;
        switch (mat)
        {
            case MaterialName.diamonds:
                break;
            case MaterialName.Gold:
                name = BuildRoomName.GoldSpace;
                break;
            case MaterialName.Mana:
                name = BuildRoomName.ManaSpace;
                break;
            case MaterialName.Wood:
                name = BuildRoomName.WoodSpace;
                break;
            case MaterialName.Iron:
                name = BuildRoomName.IronSpace;
                break;
            default:
                break;
        }
        return name;
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