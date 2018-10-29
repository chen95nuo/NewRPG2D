using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class WorldMapDataMgr : ItemDataBaseMgr<WorldMapDataMgr>
{
    protected override XmlName CurrentXmlName
    {
        get { return XmlName.WorldMapData; }
    }

    private Dictionary<WorldMapData, List<CreateEnemyData>> allLessonData;
    public Dictionary<WorldMapData, List<CreateEnemyData>> AllLessonData
    {
        get
        {
            if (allLessonData == null)
            {
                allLessonData = GetAllLessonData();
            }
            return allLessonData;
        }
    }

    public Dictionary<int, WorldMapData> GetLessonData()
    {
        Dictionary<int, WorldMapData> dic = new Dictionary<int, WorldMapData>();
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            WorldMapData data = CurrentItemData[i] as WorldMapData;
            dic.Add(data.ChapterID, data);
        }
        return dic;
    }

    public Dictionary<WorldMapData, List<CreateEnemyData>> GetAllLessonData()
    {
        Dictionary<WorldMapData, List<CreateEnemyData>> dic = new Dictionary<WorldMapData, List<CreateEnemyData>>();
        CreateEnemyData[] allLesson = CreateEnemyMgr.instance.GetAllLisson();

        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            WorldMapData data = CurrentItemData[i] as WorldMapData;
            dic.Add(data, new List<CreateEnemyData>());
            for (int j = 0; j < allLesson.Length; j++)
            {
                if (data.ChapterID == allLesson[j].ChapterID)
                {
                    dic[data].Add(allLesson[j]);
                }
            }
        }
        return dic;
    }

}
