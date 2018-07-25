using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHatcheryData
{

    private static PlayerHatcheryData instance;
    public static PlayerHatcheryData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.HatcheryData];
                instance = JsonUtility.FromJson<PlayerHatcheryData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<HatcheryData>();
                }
            }
            return instance;
        }
    }

    public List<HatcheryData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public HatcheryData GetItem(int id)
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
