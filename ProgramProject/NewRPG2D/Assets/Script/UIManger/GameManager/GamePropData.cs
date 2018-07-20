using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class GamePropData
{

    private static GamePropData instance;
    public static GamePropData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.ItemsData];
                instance = JsonUtility.FromJson<GamePropData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<ItemData>();
                }

            }
            return instance;
        }
    }

    public List<ItemData> items;//库中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ItemData GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                return new ItemData(items[i]);
            }
        }
        return null;
    }
    public ItemData GetItem(int id, int number)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                return new ItemData(items[i], number);
            }
        }
        return null;
    }
}