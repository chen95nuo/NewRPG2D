﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPlayerData
{
    private static GetPlayerData instance;
    public static GetPlayerData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GetPlayerData();
            }
            return instance;
        }
    }

    private PlayerData data;
    public PlayerData GetData()
    {
        if (data == null)
        {
            data = new PlayerData();
        }
        return data;
    }
}
