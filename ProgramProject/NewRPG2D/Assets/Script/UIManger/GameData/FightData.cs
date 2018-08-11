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
