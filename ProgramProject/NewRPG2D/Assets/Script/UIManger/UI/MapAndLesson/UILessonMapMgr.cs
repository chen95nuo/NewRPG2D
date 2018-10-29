using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILessonMapMgr : TTUIPage
{
    public GameObject[] lessons;
    public Transform mapPoint;

    public Button btn_back;

    private List<LessonMap> lessonMap = new List<LessonMap>();

    public WorldMapData currentMap;
    private void Awake()
    {
        btn_back.onClick.AddListener(ClosePage);
    }
    public override void Show(object mData)
    {
        base.Show(mData);
        transform.SetSiblingIndex(transform.parent.transform.childCount - 1);
        WorldMapData currentLesson = mData as WorldMapData;
        UpdateInfo(currentLesson);
    }

    private void UpdateInfo(WorldMapData mapData)
    {
        currentMap = mapData;
        HallEventManager.instance.SendEvent(HallEventDefineEnum.UiMainHight, 1);
        for (int i = 0; i < lessonMap.Count; i++)
        {
            if (lessonMap[i].currentMap == currentMap)
            {
                lessonMap[i].gameObject.SetActive(true);
                lessonMap[i].UpdateInfo(mapData);
                return;
            }
        }
        for (int i = 0; i < lessons.Length; i++)
        {
            if (lessons[i].name == mapData.SpriteName)
            {
                GameObject go = Instantiate(lessons[i], mapPoint) as GameObject;
                go.name = lessons[i].name;
                LessonMap data = go.GetComponent<LessonMap>();
                data.UpdateInfo(mapData);
                lessonMap.Add(data);
            }
        }
    }

    public override void ClosePage()
    {
        base.ClosePage();
        HallEventManager.instance.SendEvent(HallEventDefineEnum.UiMainHight, 0);
        for (int i = 0; i < lessonMap.Count; i++)
        {
            if (lessonMap[i].currentMap.SpriteName == currentMap.SpriteName)
            {
                lessonMap[i].gameObject.SetActive(false);
            }
        }
    }
}
