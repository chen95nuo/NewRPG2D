using System.Collections;
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

    public int Happiness
    {
        set
        {
            if (data != null)
            {
                data.Happiness = happiness;
            }
            happiness = value;
        }
    }

    private PlayerData data;
    private int happiness;
    public PlayerData GetData()
    {
        return data;
    }
    public void SetData(proto.SLGV1.CharacterInfo S_PlayerData)
    {
        data = new PlayerData(S_PlayerData, happiness);
    }
    public int GetStock(BuildRoomName name)
    {
        return data.GetResSpace(name);
    }

    public void SetThroneRoom(LocalBuildingData buildingData)
    {
        data.MainHall = buildingData;
    }
    public void SetBarracks(LocalBuildingData buildingData)
    {
        data.BarracksData = buildingData;
    }
}
