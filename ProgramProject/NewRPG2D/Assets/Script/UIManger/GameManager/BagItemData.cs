﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;


public class BagItemData
{

    private static BagItemData instance;
    public static BagItemData Instance
    {
        get
        {
            if (instance == null)
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/BagItemsData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<BagItemData>(json);

                    if (instance.items == null)
                    {
                        instance.items = new List<ItemData>();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                sr.Close();
            }

            return instance;
        }
    }

    public List<ItemData> items;//背包中的道具

    public event Action updateItemsEvent;//当数据发生变化时执行

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
                return items[i];
            }
        }
        return null;
    }


    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(ItemData data)
    {
        items.Add(data);

        //数据改变，调用事件
        if (null != updateItemsEvent)
        {
            updateItemsEvent();
        }
    }

    /// <summary>
    /// 替换物品
    /// </summary>
    /// <param name="data">要插入的物品</param>
    /// <param name="index">插入的位置</param>
    public void ExchangeItem(ItemData data, int index)
    {
        //判断你要插入的位置是否有效
        if (index < items.Count)
        {
            items[index] = data;

            //数据改变，调用事件
            if (null != updateItemsEvent)
            {
                updateItemsEvent();
            }
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        ItemData data = GetItem(id);

        if (data != null)
        {
            items.Remove(data);

            //数据改变，调用事件
            if (null != updateItemsEvent)
            {
                updateItemsEvent();
            }
        }

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/BagItemsData.txt", false, Encoding.UTF8);

        try
        {
            string json = JsonUtility.ToJson(instance, true);

            Debug.Log(json);

            sw.Write(json);

        }
        catch (Exception e)
        {

            Debug.LogError(e.ToString());
        }

        sw.Close();
    }

}
