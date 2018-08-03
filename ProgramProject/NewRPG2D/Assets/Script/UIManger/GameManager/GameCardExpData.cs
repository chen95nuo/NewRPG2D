using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardExpData
{

    private static GameCardExpData instance;
    public static GameCardExpData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.CardExpData];
                instance = JsonUtility.FromJson<GameCardExpData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<CardExpData>();
                }

            }
            return instance;
        }
    }

    public List<CardExpData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public CardExpData GetItem(int level)
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
