using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

public class UIMapLessonGrid : MonoBehaviour
{

    public Text txt_Name;
    public Text txt_Complete;

    public LessonGridHelper[] tipFill;

    private WorldMapData currentMap;
    private Button btn_MapGrid;


    private void Awake()
    {
        btn_MapGrid = GetComponent<Button>();
        btn_MapGrid.onClick.AddListener(ChickLesson);
    }

    public void UpdateInfo(WorldMapData mapData, bool isComplete, CreateEnemyData lessonData = null)
    {
        currentMap = mapData;
        txt_Name.text = mapData.Name;
        List<CreateEnemyData> data = WorldMapDataMgr.instance.AllLessonData[mapData];
        int max = data.Count;
        int now = 0;
        if (isComplete)
        {
            now = max;
        }
        else
        {
            now = lessonData.Lesson;
        }
        txt_Complete.text = string.Format("{0}/{1}", now, max);

    }

    public void ChickLesson()
    {
        UIPanelManager.instance.ShowPage<UILessonMapMgr>(currentMap);
    }
}

[System.Serializable]
public class LessonGridHelper
{
    public GameObject Type;
    public Text txt_Tip;
    public Text txt_Num;
}
