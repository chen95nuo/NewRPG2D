using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class GameEquipData
{

    private static GameEquipData instance;
    public static GameEquipData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.EquipData];
                instance = JsonUtility.FromJson<GameEquipData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<EquipData>();
                }

            }
            return instance;
        }
    }

    public List<EquipData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipData GetItem(int id)
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