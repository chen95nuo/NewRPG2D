using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightData
{
    private string currentMap;
    private CardData[] cardData;
    private LessonDropData currentLesson;

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

    public FightData() { }
    public FightData(string currentMap, CardData[] cardData, LessonDropData currentLesson)
    {
        this.currentMap = currentMap;
        this.cardData = cardData;
        this.currentLesson = currentLesson;
    }
}
