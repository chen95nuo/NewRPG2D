using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;


public class BagEquipData
{
    private static BagEquipData instance;
    public static BagEquipData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.BagEquipData];
                instance = JsonUtility.FromJson<BagEquipData>(json);
                if (instance.equips == null)
                {
                    instance.equips = new List<EquipData>();
                }
            }

            return instance;
        }
    }

    public List<EquipData> equips;//背包中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EquipData GetItem(int id)
    {
        for (int i = 0; i < equips.Count; i++)
        {
            if (equips[i].Id == id)
            {
                return equips[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(EquipData data)
    {
        equips.Add(data);

        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEquipsEvent);
    }

    /// <summary>
    /// 替换物品
    /// </summary>
    /// <param name="data">要插入的物品</param>
    /// <param name="index">插入的位置</param>
    public void ExchangeItem(EquipData data, int index)
    {
        //判断你要插入的位置是否有效
        if (index < equips.Count)
        {
            equips[index] = data;

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEquipsEvent);
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(EquipData data)
    {
        //EquipData data = GetItem(id);

        if (data != null)
        {
            equips.Remove(data);

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEquipsEvent);
        }

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/BagEquipData.txt", false, Encoding.UTF8);

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
