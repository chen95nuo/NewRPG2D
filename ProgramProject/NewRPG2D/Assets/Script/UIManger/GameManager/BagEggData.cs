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
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
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
