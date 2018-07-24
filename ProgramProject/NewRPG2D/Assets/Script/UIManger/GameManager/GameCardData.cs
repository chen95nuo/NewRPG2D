using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class GameCardData
{


    private static GameCardData instance;
    public static GameCardData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.CardData];
                instance = JsonUtility.FromJson<GameCardData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<CardData>();
                }

            }
            return instance;
        }
    }

    public List<CardData> items;//库中的道具

    /// <summary>
    /// 获取道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CardData GetItem(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                return new CardData(items[i]);
            }
        }
        return null;
    }
}
