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
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.PlayerData];
                instance = JsonUtility.FromJson<GetPlayData>(json);

                if (instance.player == null)
                {
                    instance.player = new List<PlayerData>();
                }

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
}
