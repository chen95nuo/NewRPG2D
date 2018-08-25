﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerRoundData
{
    [SerializeField]
    private List<int> unlockMapID;
    [SerializeField]
    private List<PlayerLessonData> unLockLesson;

    public List<int> UnlockMapID
    {
        get
        {
            Debug.Log(unlockMapID.Count + "CT");
            if (unlockMapID == null || unlockMapID.Count < 1)
            {

                unlockMapID = new List<int>();
                unlockMapID.Add(100001);
                Debug.Log(unlockMapID.Count + "CT");
            }
            return unlockMapID;
        }
    }

    public int AddUnlockMap
    {
        set
        {
            for (int i = 0; i < unlockMapID.Count; i++)
            {
                if (unlockMapID[i] == value)
                {
                    Debug.LogError("已经解锁该地图了 重复解锁" + value);
                    return;
                }
            }
            unlockMapID.Add(value);
        }
    }

    public List<PlayerLessonData> UnLockLesson
    {
        get
        {
            if (unLockLesson == null)
            {
                unLockLesson = new List<PlayerLessonData>();
            }
            if (unLockLesson.Count < 1)
            {
                unLockLesson.Add(new PlayerLessonData(1, 1));
            }
            return unLockLesson;
        }

        set
        {
            unLockLesson = value;
        }
    }
}

[System.Serializable]
public class PlayerLessonData
{
    [SerializeField]
    public int unLockLessonID;
    [SerializeField]
    public int unLessonLevel;

    public PlayerLessonData() { }
    public PlayerLessonData(int id, int level)
    {
        this.unLockLessonID = id;
        this.unLessonLevel = level;
    }
}
