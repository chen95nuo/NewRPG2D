using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightData
{
    private string currentMap;
    private CardData[] cardData;
    private LessonDropData currentLesson;
    private int playerLevel;

    public string CurrentMap
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

    public CardData[] CardData
    {
        get
        {
            return cardData;
        }

        set
        {
            cardData = value;
        }
    }

    public LessonDropData CurrentLesson
    {
        get
        {
            return currentLesson;
        }

        set
        {
            currentLesson = value;
        }
    }

    public int PlayerLevel
    {
        get
        {
            return playerLevel;
        }
    }

    public FightData() { }
    public FightData(string currentMap, CardData[] cardData, LessonDropData currentLesson, int playerLevel)
    {
        this.currentMap = currentMap;
        this.cardData = cardData;
        this.currentLesson = currentLesson;
        this.playerLevel = playerLevel;
    }
}
[System.Serializable]
public class LessonDropData
{
    [SerializeField]
    private int lessonId;
    [SerializeField]
    private int[] dropPropId;//掉落物品的ID
    [SerializeField]
    private int dropBoxId;//掉落包的ID


    public int LessonId
    {
        get
        {
            return lessonId;
        }
    }
    public int[] DropPropId
    {
        get
        {
            return dropPropId;
        }
    }

    public int DropBoxId
    {
        get
        {
            return dropBoxId;
        }
    }
    public LessonDropData() { }
    public LessonDropData(int lessonId, int dropBoxId)
    {
        this.lessonId = lessonId;
        this.dropBoxId = dropBoxId;
    }
}