using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMapData
{
    private static GameMapData instance;
    public static GameMapData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.MapData];
                instance = JsonUtility.FromJson<GameMapData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<MapData>();
                }

            }
            return instance;
        }
    }

    public List<MapData> items;//库中的道具

    /// <summary>
    /// 获取地图的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public MapData GetItem(int id)
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
