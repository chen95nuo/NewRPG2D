using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerExpData {

    private static GetPlayerExpData instance;
    public static GetPlayerExpData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.playerExpData];
                instance = JsonUtility.FromJson<GetPlayerExpData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<PlayerExpData>();
                }

            }
            return instance;
        }
    }

    public List<PlayerExpData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public PlayerExpData GetItem(int level)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Level == level)
            {
                return items[i];
            }
        }
        return null;
    }
}