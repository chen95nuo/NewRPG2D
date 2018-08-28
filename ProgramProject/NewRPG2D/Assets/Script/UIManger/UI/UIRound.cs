using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRound : MonoBehaviour
{

    public Text LessonName;
    public UILessonBtn[] grids;
    public UITypeTip typeTip;
    public int currentMap;
    public int currentLesson;
    public LessonFightData currentFight;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        typeTip.gameObject.SetActive(false);
        PlayerRoundData data = GetPlayerRoundData.Instance.items;
        currentMap = GetPlayData.Instance.player[0].MapNumber;
        for (int i = 0; i < grids.Length; i++)
        {
            grids[i].btn_lesson.onClick.AddListener(ChickLessonbtn);
            if (i < data.UnLockLesson.Count)
            {
                grids[i].gameObject.SetActive(true);
                grids[i].UpdateBtn(data.UnLockLesson[i].unLessonLevel);
            }
            else
            {
                grids[i].gameObject.SetActive(false);
            }
        }
    }

    public void ChickLessonbtn()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

        for (int i = 0; i < grids.Length; i++)
        {
            if (go == grids[i].btn_lesson.gameObject)
            {
                currentLesson = i;
                //if (currentLesson == 0)
                //{
                //    currentLesson = 1;
                //}
                currentFight = GameLessonFightData.Instance.items[currentLesson];
                typeTip.gameObject.SetActive(true);
                typeTip.UpdateGrids(currentFight);
                return;
            }
        }
    }
}
