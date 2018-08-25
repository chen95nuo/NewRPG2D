using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRound : MonoBehaviour
{

    public Text name;
    public UILessonBtn[] grids;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        PlayerRoundData data = GetPlayerRoundData.Instance.items;
        for (int i = 0; i < grids.Length; i++)
        {
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
}
