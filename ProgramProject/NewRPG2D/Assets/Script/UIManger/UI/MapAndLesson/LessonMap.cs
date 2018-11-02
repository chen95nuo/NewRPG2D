using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.Battle.BattleData;
using Assets.Script.UIManger;

public class LessonMap : MonoBehaviour
{

    public Button[] btn_Lessons;
    public Sprite[] sp; //0、通过 1、解锁 2、锁定 3、入侵 4、其他

    public WorldMapData currentMap;

    private void Awake()
    {
        for (int i = 0; i < btn_Lessons.Length; i++)
        {
            btn_Lessons[i].onClick.AddListener(ChickLesson);
        }
    }

    public void UpdateInfo(WorldMapData mapData)
    {
        currentMap = mapData;
        PlayerData playerData = GetPlayerData.Instance.GetData();
        if (playerData.CurrentLessonData.ChapterID != mapData.ChapterID)
        {
            for (int i = 0; i < btn_Lessons.Length; i++)
            {
                btn_Lessons[i].image.sprite = sp[0];
            }
        }
        else
        {
            int index = playerData.CurrentLessonData.Lesson - 1;
            for (int i = 0; i < btn_Lessons.Length; i++)
            {
                //if (i < index)
                //{
                //    btn_Lessons[i].image.sprite = sp[0];
                //}
                //else if (i == index)
                //{
                btn_Lessons[i].image.sprite = sp[1];
                //}
                //else
                //{
                //    btn_Lessons[i].image.sprite = sp[2];
                //}
            }
        }
    }

    public void ChickLesson()
    {
        GameObject go = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        PlayerData playerData = GetPlayerData.Instance.GetData();
        int index = playerData.CurrentLessonData.Lesson - 1;
        for (int i = 0; i < btn_Lessons.Length; i++)
        {
            //if (btn_Lessons[i].gameObject == go)
            //{
            //    if (i < index)
            //    {
            //        object st = "已完成该关卡";
            //        UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            //    }
            //    else if (i == index)
            //    {
            for (int j = 0; j < WorldMapDataMgr.instance.AllLessonData[currentMap].Count; j++)
            {
                if (WorldMapDataMgr.instance.AllLessonData[currentMap][j].Lesson == i + 1)
                {
                    MapLevelData lessonData = WorldMapDataMgr.instance.AllLessonData[currentMap][j];
                    UIPanelManager.instance.ShowPage<UILessonInfo>(lessonData);
                    return;
                }
                Debug.LogError("错误没有找到对应关卡");
            }
            //}
            //else
            //{
            //    object st = string.Format("请先通过{0}关卡", playerData.CurrentLessonData.Name);
            //    UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            //}
            //}
        }
    }
}
