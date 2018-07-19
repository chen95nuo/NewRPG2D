using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;


public class GameComposedTableData
{
    private static GameComposedTableData instance;
    public static GameComposedTableData Instance
    {
        get
        {
            if (instance == null)
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/library/ComposedTableData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<GameComposedTableData>(json);

                    if (instance.items == null)
                    {
                        instance.items = new List<ComposedTableData>();
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

    public List<ComposedTableData> items;//库中的道具


    public List<ComposedTableData> GetTables(ComposedTableData data)
    {
        List<ComposedTableData> tempData = new List<ComposedTableData>();
        //检查我所有的合成公式
        for (int i = 0; i < items.Count; i++)
        {
            int index = 0;
            //检查每个合成公式里面所需的道具 道具
            for (int j = 0; j < items[i].NeedMaterial.Length; j++)
            {
                //如果当前的材料比表中的需求大或者相等，那么记录 材料
                if (data.NeedMaterial[j] >= items[i].NeedMaterial[j])
                {
                    index++;
                }

            }
            //如果当前的所有材料都比需求高，那么检查该道具是否拥有必需物品
            if (index >= items[i].NeedMaterial.Length)
            {
                //如果必须道具不等于空 那么匹配必须道具
                if (items[i].NeedProp.Length > 0)
                {
                    int index_2 = 0;
                    //循环当前公式所需的道具
                    for (int j = 0; j < items[i].NeedProp.Length; j++)
                    {
                        //循环当前拥有的道具 进行匹配
                        for (int k = 0; k < data.NeedProp.Length; k++)
                        {
                            if (items[i].NeedProp[j] == data.NeedProp[k])
                            {
                                index_2++;
                                break;
                            }
                        }
                    }
                    //如果材料列表拥有需求道具 则添加该物品
                    if (index_2 >= items[i].NeedProp.Length)
                    {
                        tempData.Add(items[i]);
                    }
                }
                else
                {
                    tempData.Add(items[i]);
                }
            }
        }


        return tempData;
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/library/ComposedTableData.txt", false, Encoding.UTF8);

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
