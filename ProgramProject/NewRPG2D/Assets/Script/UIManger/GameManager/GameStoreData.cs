using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStoreData
{

    private static GameStoreData instance;
    public static GameStoreData Instance
    {

        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.StoreData];
                instance = JsonUtility.FromJson<GameStoreData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<StoreData>();
                }
            }
            return instance;
        }
    }

    public List<StoreData> items;

    /// <summary>
    /// 查找ID
    /// </summary>
    /// <param name="id">道具的id</param>
    /// <returns></returns>
    public StoreData GetStoreItems(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].PropId == id)
            {
                return items[i];
            }
        }
        return null;
    }

}
