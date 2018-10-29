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

    private Dictionary<WorldMapData, List<MapLevelData>> allLessonData;
    public Dictionary<WorldMapData, List<MapLevelData>> AllLessonData
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

    public Dictionary<WorldMapData, List<MapLevelData>> GetAllLessonData()
    {
        Dictionary<WorldMapData, List<MapLevelData>> dic = new Dictionary<WorldMapData, List<MapLevelData>>();
        MapLevelData[] allLesson = MapLevelDataMgr.instance.GetAllLisson();

        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            WorldMapData data = CurrentItemData[i] as WorldMapData;
            dic.Add(data, new List<MapLevelData>());
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
