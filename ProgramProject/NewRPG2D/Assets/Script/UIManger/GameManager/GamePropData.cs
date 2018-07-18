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
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/library/ItemsData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<GamePropData>(json);

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
                return items[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/library/ItemsData.txt", false, Encoding.UTF8);

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