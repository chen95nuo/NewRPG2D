using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExploreData
{
    private static GameExploreData instance;
    public static GameExploreData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.ExploreData];
                instance = JsonUtility.FromJson<GameExploreData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<ExploreData>();
                }

            }
            return instance;
        }
    }

    public List<ExploreData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ExploreData GetItem(int id)
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