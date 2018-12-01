using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.Utility;
using Assets.Script.Battle.BattleData;

public class BuildingDataMgr : ItemCsvDataBaseMgr<BuildingDataMgr>
{
    protected override CsvEChartsType CurrentCsvName
    {
        get { return CsvEChartsType.BuildingData; }
    }

    /// <summary>
    /// 获取每个建筑的数量
    /// </summary>
    /// <returns></returns>
    public Dictionary<RoomType, List<BuildingData>> GetBuildingType()
    {
        Dictionary<RoomType, List<BuildingData>> dic = new Dictionary<RoomType, List<BuildingData>>();

        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            BuildingData data = CurrentItemData[i] as BuildingData;
            if (data.UnlockLevel != null)
            {
                if (dic.ContainsKey(data.RoomType) == false)
                {
                    dic.Add(data.RoomType, new List<BuildingData>());
                }
                dic[data.RoomType].Add(data);
            }
        }
        return dic;
    }

    /// <summary>
    /// 获取每个建筑的建造数量信息
    /// </summary>
    /// <returns></returns>
    public Dictionary<BuildRoomName, BuildingData[]> GetBuilding()
    {
        Dictionary<BuildRoomName, BuildingData[]> dic = new Dictionary<BuildRoomName, BuildingData[]>();

        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            BuildingData data = CurrentItemData[i] as BuildingData;
            if (data.UnlockLevel != null)
            {
                if (dic.ContainsKey(data.RoomName) == false)
                {
                    dic.Add(data.RoomName, new BuildingData[data.UnlockLevel.Length]);
                }
            }
        }
        return dic;
    }

    public BuildingData GetbuildingData(BuildRoomName name)
    {
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            BuildingData data = CurrentItemData[i] as BuildingData;
            if (data.RoomName == name && data.Level == 1)
            {
                return data;
            }
        }
        return null;
    }

    public List<BuildingData> AllRoomData()
    {
        List<BuildingData> data = new List<BuildingData>();
        for (int i = 0; i < CurrentItemData.Length; i++)
        {
            BuildingData Temp = (CurrentItemData[i] as BuildingData);
            data.Add(Temp);
        }
        return data;
    }
}
