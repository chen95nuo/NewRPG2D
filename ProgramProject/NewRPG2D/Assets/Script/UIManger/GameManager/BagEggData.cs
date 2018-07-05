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
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/BagEggData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<BagEggData>(json);

                    if (instance.eggs == null)
                    {
                        instance.eggs = new List<EggData>();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
                sr.Close();

                //  instance = new BagEggData();
            }
            return instance;
        }
    }
    public List<EggData> eggs; //背包中所有的蛋

    public event Action updateEggsEvent;//当数据发生变化时执行

    /// <summary>
    /// 查找蛋数据
    /// </summary>
    /// <param name="id">蛋的ID</param>
    /// <returns></returns>
    public EggData GetEggs(int id)
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

    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(EggData data)
    {
        eggs.Add(data);

        //数据改变，调用事件
        if (null != updateEggsEvent)
        {
            updateEggsEvent();
        }
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
            if (null != updateEggsEvent)
            {
                updateEggsEvent();
            }
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        EggData data = GetEggs(id);

        if (data != null)
        {
            eggs.Remove(data);

            //数据改变，调用事件
            if (null != updateEggsEvent)
            {
                updateEggsEvent();
            }
        }

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/BagEggData.txt",false, Encoding.UTF8);

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
