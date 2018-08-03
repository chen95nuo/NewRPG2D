using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class BagEggData
{

    private static BagEggData instance;

    public static BagEggData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.BagEggData];
                instance = JsonUtility.FromJson<BagEggData>(json);
                if (instance.eggs == null)
                {
                    instance.eggs = new List<EggData>();
                }
            }

            return instance;
        }
    }
    public List<EggData> eggs; //背包中所有的蛋

    private void LoadEggs()
    {
        for (int i = 0; i < eggs.Count; i++)
        {
            eggs[i] = GameEggData.Instance.GetItem(eggs[i].Id);
        }
    }

    /// <summary>
    /// 查找蛋数据
    /// </summary>
    /// <param name="id">蛋的ID</param>
    /// <returns></returns>
    public EggData GetEgg(int id)
    {
        for (int i = 0; i < eggs.Count; i++)
        {
            if (eggs[i].Id == id)
            {
                return eggs[i];
            }
        }
        return null;
    }
    public List<EggData> GetEggs(int id)
    {
        List<EggData> getItems = new List<EggData>();
        for (int i = 0; i < eggs.Count; i++)
        {
            if (eggs[i].Id == id)
            {
                getItems.Add(eggs[i]);
            }
        }
        return getItems;
    }

    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(EggData data)
    {
        eggs.Add(data);

        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
    }
    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(int itemID, int number)
    {
        EggData data = GameEggData.Instance.GetItem(itemID);
        data.ItemNumber = number;
        eggs.Add(data);

        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
    }
    public void AddItems(EggData item, int number)
    {
        List<EggData> data = GetEggs(item.Id);
        int index = 0;
        int temp = 0;
        //检查有多少个相同的道具
        for (int i = 0; i < data.Count; i++)
        {
            do
            {
                if (data[i].ItemNumber + 1 <= 99)
                {
                    data[i].ItemNumber++;
                    index++;
                    Debug.Log(index);
                    Debug.Log(data[i].ItemNumber);
                }
            } while (index < number && data[i].ItemNumber < 99);
        }
        //如果这个背包中所有的该道具的总数都超过99或者背包中已经没有这个道具了 新建一个这个道具
        if (number - index > 0)
        {
            Debug.Log(number - index);
            EggData newData = new EggData(item, number - index);
            AddItem(newData);
        }
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdatePropsEvent);
    }

    /// <summary>
    /// 替换物品
    /// </summary>
    /// <param name="data">要插入的物品</param>
    /// <param name="index">插入的位置</param>
    public void ExchangeItem(EggData data, int index)
    {
        //判断你要插入的位置是否有效
        if (index < eggs.Count)
        {
            eggs[index] = data;

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(EggData data)
    {
        eggs.Remove(data);
        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/BagEggData.txt", false, Encoding.UTF8);

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
