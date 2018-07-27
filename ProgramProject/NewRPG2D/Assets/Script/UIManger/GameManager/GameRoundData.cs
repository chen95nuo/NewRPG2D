using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoundData
{
    private static GameRoundData instance;
    public static GameRoundData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.RoundData];
                instance = JsonUtility.FromJson<GameRoundData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<RoundData>();
                }

            }
            return instance;
        }
    }

    public List<RoundData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public RoundData GetItem(int id)
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
