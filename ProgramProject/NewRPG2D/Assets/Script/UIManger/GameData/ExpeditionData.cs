using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExpeditionData
{
    [SerializeField]
    private List<ExpeditionTeam> expeditionTeam;
    [SerializeField]
    private List<int> unLockMap;

    public List<ExpeditionTeam> ExpeditionTeam
    {
        get
        {
            return expeditionTeam;
        }

        set
        {
            expeditionTeam = value;
        }
    }
    public List<int> UnLockMap
    {
        get
        {
            return unLockMap;
        }
    }
    public int addMap //添加地图信息
    {
        set
        {
            for (int i = 0; i < unLockMap.Count; i++)
            {
                if (unLockMap[i] == value)
                {
                    Debug.LogError("这个地图已经有了");
                    return;
                }
            }
            unLockMap.Add(value);
        }
    }
}

[System.Serializable]
public class ExpeditionTeam
{
    [SerializeField]
    private int id;
    [SerializeField]
    private CardData[] cardsData;
    [SerializeField]
    private ExploreType exploreType;
    [SerializeField]
    private long endTime;
    [SerializeField]
    private float needTime;
    [SerializeField]
    private int nowTime;
    [SerializeField]
    private ExploreData currentMap;

    public int Id
    {
        get
        {
            return id;
        }
    }
    public CardData[] CardsData
    {
        get
        {
            return cardsData;
        }
        set
        {
            cardsData = value;
        }
    }

    public ExploreType ExploreType
    {
        get
        {
            return exploreType;
        }
        set
        {
            exploreType = value;
        }
    }

    public long EndTime
    {
        get
        {
            return endTime;
        }
        set
        {
            endTime = value;
        }
    }

    public float NeedTime
    {
        get
        {

            return needTime;
        }
    }

    public int NowTime
    {
        get
        {
            TimeSpan sp = DateTime.FromFileTime(endTime).Subtract(SystemTime.insatnce.GetTime());
            nowTime = sp.Seconds;
            return nowTime;
        }
    }

    public ExploreData CurrentMap
    {
        get
        {
            return currentMap;
        }
    }
}
