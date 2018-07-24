using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpeditionData
{
    private static PlayerExpeditionData instance;
    public static PlayerExpeditionData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.ExpeditionData];
                instance = JsonUtility.FromJson<PlayerExpeditionData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<ExpeditionData>();
                }

            }
            return instance;
        }
    }

    public List<ExpeditionData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ExpeditionData GetItem(int id)
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