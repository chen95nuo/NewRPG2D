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
    private int maxTime;
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

    public int EndTime
    {
        set
        {
            if (endTime == 0)
            {
                maxTime = value;
                endTime = SystemTime.insatnce.GetTime().AddHours(value).ToFileTime();
            }
        }
    }

    public int MaxTime
    {
        get
        {
            return maxTime * 60 * 60;
        }
    }

    public float NowTime
    {
        get
        {
            TimeSpan sp = DateTime.FromFileTime(endTime).Subtract(SystemTime.insatnce.GetTime());

            float nowTime = (float)sp.TotalSeconds;
            return nowTime;
        }
    }

    public ExploreData CurrentMap
    {
        get
        {
            return currentMap;
        }
        set
        {
            currentMap = value;
        }
    }
    public ExpeditionTeam() { }
    public ExpeditionTeam(int id)
    {
        this.id = id;
    }
}
