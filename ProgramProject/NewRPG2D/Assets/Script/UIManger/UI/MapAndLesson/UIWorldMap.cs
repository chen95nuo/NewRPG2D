using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;
using Assets.Script.Battle.BattleData;

public class UIWorldMap : TTUIPage
{
    public UIMapLessonGrid Chapter_1;
    public Button btn_Back;

    private void Awake()
    {
        btn_Back.onClick.AddListener(ClosePage);
    }

    public override void Show(object mData)
    {
        base.Show(mData);
        Debug.Log("Show");
        UpdateInfo();
        Transform parent = this.transform.parent.transform;
        int index = parent.childCount - 1;
        transform.SetSiblingIndex(index);
    }

    public void UpdateInfo()
    {
        PlayerData playerData = GetPlayerData.Instance.GetData();
        MapLevelData lessonData = playerData.CurrentLessonData;
        WorldMapData data = WorldMapDataMgr.instance.GetXmlDataByItemId<WorldMapData>(10001);
        foreach (var item in WorldMapDataMgr.instance.AllLessonData)
        {
            data = item.Key;
            break;
        }

        if (data.ChapterID < lessonData.ChapterID)
        {
            Chapter_1.UpdateInfo(data, true);
        }
        else if (data.ChapterID == lessonData.ChapterID)
        {
            Chapter_1.UpdateInfo(data, false, lessonData);
        }
        else
        {
            Debug.Log("This Chapter is Locked");
        }
    }

    public override void Hide(bool needAnim = true)
    {
        base.Hide(needAnim = false);
    }

    public override void Active(bool needAnim = true)
    {
        base.Active(needAnim = false);
    }

    public override void ClosePage()
    {
        base.ClosePage();
        UIMain.instance.UIMainHight(-1);
    }
}
