using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerRoundData
{

    private static GetPlayerRoundData instance;
    public static GetPlayerRoundData Instance
    {
        get
        {
            if (instance == null)
            {
                string json = ReadJsonNewMgr.instance.AllJsonDataDic[(int)JsonName.PlayerRoundData];
                instance = JsonUtility.FromJson<GetPlayerRoundData>(json);
                if (instance.items == null)
                {
                    instance.items = new PlayerRoundData();
                }

            }
            return instance;
        }
    }

    public PlayerRoundData items;//库中的道具


}
