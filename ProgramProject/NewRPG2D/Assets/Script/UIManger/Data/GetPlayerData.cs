using System.Collections;
using System.Collections.Generic;
using proto.SLGV1;
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

    public void UseResource(LocalBuildingData data)
    {
        for (int i = 0; i < data.buildingData.needMaterial.Length; i++)
        {
            if (data.buildingData.needMaterial[i] > 0)
            {
                BuildRoomName name = BuildingManager.instance.MaterialNameToBuildRoomName((MaterialName)i);
                this.data.UseRes(name, data.buildingData.needMaterial[i]);
            }
        }
    }

    public void AddResource(ConsumeItem collectedResource)
    {
        BuildRoomName name = BuildingManager.instance.MaterialNameToBuildRoomName((MaterialName)collectedResource.produceType);
       data.AddRes(name, collectedResource.needNum);
    }
}
