using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoundData
{
    private static PlayerRoundData instance;
    public static PlayerRoundData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.PlayerRoundData];
                instance = JsonUtility.FromJson<PlayerRoundData>(json);
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
