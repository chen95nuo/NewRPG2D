using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class GameEggData
{

    private static GameEggData instance;
    public static GameEggData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.EggData];
                instance = JsonUtility.FromJson<GameEggData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<EggData>();
                }

            }
            return instance;
        }
    }

    public List<EggData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EggData GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                return new EggData(items[i]);
            }
        }
        return null;
    }
}