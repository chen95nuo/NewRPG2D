using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class GetPlayData
{
    private static GetPlayData instance;

    public static GetPlayData Instance
    {
        get
        {
            if (instance == null)
            {
                StreamReader sr = new StreamReader(Application.streamingAssetsPath + "/Json/PlayerData.txt", Encoding.UTF8);

                try
                {
                    string json = sr.ReadToEnd();

                    instance = JsonUtility.FromJson<GetPlayData>(json);

                    if (instance.player == null)
                    {
                        instance.player = new List<PlayerData>();
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
    public List<PlayerData> player; //玩家的信息



    /// <summary>
    /// 查找数据
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <returns></returns>
    public PlayerData GetPlayerData(int id)
    {
        for (int i = 0; i < player.Count; i++)
        {
            if (player[i].Id == id)
            {
                return player[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 添加物品的数据
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(PlayerData data)
    {
        player.Add(data);

        //数据改变，调用事件
        UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
    }

    /// <summary>
    /// 替换物品
    /// </summary>
    /// <param name="data">要插入的物品</param>
    /// <param name="index">插入的位置</param>
    public void ExchangeItem(PlayerData data, int index)
    {
        //判断你要插入的位置是否有效
        if (index < player.Count)
        {
            player[index] = data;

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
        PlayerData data = GetPlayerData(id);

        if (data != null)
        {
            player.Remove(data);

            //数据改变，调用事件
            UIEventManager.instance.SendEvent(UIEventDefineEnum.UpdateEggsEvent);
        }

    }

    /// <summary>
    /// 存储数据
    /// </summary>
    public void SaveData()
    {
        StreamWriter sw = new StreamWriter(Application.streamingAssetsPath + "/Json/PlayerData.txt", false, Encoding.UTF8);

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
