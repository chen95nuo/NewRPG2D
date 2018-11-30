/*
 * 提供所有建筑信息
 * 建筑数据中有建筑 则建造数据中的建筑
 * 若没有建筑 则等待服务器提供建筑信息
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : TSingleton<BuildingManager>
{
    private Dictionary<BuildRoomName, Dictionary<int, LocalBuildingData>> allBuilding = new Dictionary<BuildRoomName, Dictionary<int, LocalBuildingData>>();


}
