using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class BagRoleData
{

    private static BagRoleData instance;
    public static BagRoleData Instance
    {
        get
        {
            if (instance == null)
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/BagRoleData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<BagRoleData>(json);

                    if (instance.roles == null)
                    {
                        instance.roles = new List<CardData>();
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

    public List<CardData> roles;//背包中的道具

    /// <summary>
    /// 查找道具的数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CardData GetItem(int id)
    {
        for (int i = 0; i < roles.Count; i++)
        {
            if (roles[i].Id == id)
            {
                return roles[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(CardData data)
    {
        roles.Add(data);

        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateRolesEvent);
    }

    /// <summary>
    /// 替换物品
    /// </summary>
    /// <param name="data">要插入的物品</param>
    /// <param name="index">插入的位置</param>
    public void ExchangeItem(CardData data, int index)
    {
        //判断你要插入的位置是否有效
        if (index < roles.Count)
        {
            roles[index] = data;

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateRolesEvent);
        }
    }


    /// <summary>
    /// 删除背包物品的数据
    /// </summary>
    /// <param name="id"></param>
    public void Remove(int id)
    {
        CardData data = GetItem(id);

        if (data != null)
        {
            roles.Remove(data);

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateRolesEvent);
        }
    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/BagRoleData.txt", false, Encoding.UTF8);

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
