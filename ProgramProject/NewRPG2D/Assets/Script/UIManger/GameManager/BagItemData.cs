using UnityEngine;
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
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.BagItemsData];
                instance = JsonUtility.FromJson<BagItemData>(json);
                if (instance.items == null)
                {
                    instance.items = new List<ItemData>();
                }
            }

            return instance;
        }
    }

    public List<ItemData> items;//背包中的道具

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

    public List<ItemData> GetItems(int id)
    {
        List<ItemData> getItems = new List<ItemData>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                getItems.Add(items[i]);
            }
        }
        return getItems;
    }
    /// <summary>
    /// 减少道具数量
    /// </summary>
    /// <param name="id">道具ID</param>
    /// <param name="number">减少多少?</param>
    public void ReduceItems(int id, int number)
    {
        List<ItemData> getItems = new List<ItemData>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].Id == id)
            {
                getItems.Add(items[i]);
            }
        }
        ItemData data = getItems[0];
        if (getItems.Count > 0)
        {
            do
            {
                for (int i = 0; i < getItems.Count - 1; i++)
                {
                    if (getItems[i].Number > getItems[i + 1].Number)
                    {
                        data = getItems[i + 1];
                    }
                }
                if (data.Number - number >= 0)
                {
                    data.Number -= number;
                    number = 0;
                }
                else
                {
                    number -= data.Number;
                    data.Number = 0;
                    Remove(data);
                }

            } while (number != 0);
        }
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent);
    }


    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(ItemData data)
    {
        items.Add(data);
        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent);
    }

    public void AddItems(ItemData item, int number)
    {
        List<ItemData> data = GetItems(item.Id);
        int index = 0;
        int temp = 0;
        //检查有多少个相同的道具
        for (int i = 0; i < data.Count; i++)
        {
            do
            {
                if (data[i].Number + 1 <= 99)
                {
                    data[i].Number++;
                    index++;
                    Debug.Log(index);
                    Debug.Log(data[i].Number);
                }
            } while (index < number && data[i].Number < 99);
        }
        //如果这个背包中所有的该道具的总数都超过99或者背包中已经没有这个道具了 新建一个这个道具
        if (number - index > 0)
        {
            Debug.Log(number - index);
            ItemData newData = new ItemData(item, number - index);
            AddItem(newData);
        }
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent);
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
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent);
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(ItemData data)
    {
        if (data != null)
        {
            items.Remove(data);

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent);
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
