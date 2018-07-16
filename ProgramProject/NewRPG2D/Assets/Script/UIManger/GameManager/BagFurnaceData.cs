using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class BagFurnaceData
{
    private static BagFurnaceData instance;

    public static BagFurnaceData Instance
    {
        get
        {
            if (instance == null)
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/BagFurnaceData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<BagFurnaceData>(json);

                    if (instance.furnaces == null)
                    {
                        instance.furnaces = new List<FurnaceData>();
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
    public List<FurnaceData> furnaces; //所有熔炉



    /// <summary>
    /// 查找蛋数据
    /// </summary>
    /// <param name="id">熔炉ID</param>
    /// <returns></returns>
    public FurnaceData Getfurnaces(int id)
    {
        for (int i = 0; i < furnaces.Count; i++)
        {
            if (furnaces[i].Id == id)
            {
                return furnaces[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(FurnaceData data)
    {
        furnaces.Add(data);

        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
    }

    /// <summary>
    /// 替换物品
    /// </summary>
    /// <param name="data">要插入的物品</param>
    /// <param name="index">插入的位置</param>
    public void ExchangeItem(FurnaceData data, int index)
    {
        //判断你要插入的位置是否有效
        if (index < furnaces.Count)
        {
            furnaces[index] = data;

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        FurnaceData data = Getfurnaces(id);

        if (data != null)
        {
            furnaces.Remove(data);

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
        }

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/BagFurnaceData.txt", false, Encoding.UTF8);

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
