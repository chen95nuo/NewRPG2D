using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLessonFightData
{
    private static GameLessonFightData instance;
    public static GameLessonFightData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.LessonFightData];
                instance = JsonUtility.FromJson<GameLessonFightData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<LessonFightData>();
                }

            }
            return instance;
        }
    }

    public List<LessonFightData> items;//库中的道具

    /// <summary>
    /// 获取地图的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public LessonFightData GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                return items[i];
            }
        }
        return null;
    }

}
