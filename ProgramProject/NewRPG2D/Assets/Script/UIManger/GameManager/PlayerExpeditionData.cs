using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpeditionData
{
    private static PlayerExpeditionData instance;
    public static PlayerExpeditionData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.ExpeditionData];
                instance = JsonUtility.FromJson<PlayerExpeditionData>(json);
                if (instance.items == null)
                {
                    instance.items = new ExpeditionData();
                }

            }
            return instance;
        }
    }

    public ExpeditionData items;//库中的道具

}