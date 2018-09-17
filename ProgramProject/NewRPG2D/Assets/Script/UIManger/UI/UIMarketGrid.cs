using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketGrid : MonoBehaviour
{

    public BuildingData building;
    public UIMarket market;
    private Button thisGrid;
    private void Awake()
    {
        thisGrid = GetComponent<Button>();
        thisGrid.onClick.AddListener(AddBuilding);
    }

    private void AddBuilding()
    {
        BuildingData build = new BuildingData();
        build.Level = building.Level;
        build.RoomSize = building.RoomSize;
        build.RoomType = building.RoomType;
        build.Param1 = building.Param1;
        build.Param2 = building.Param2;
        HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.AddBuild, build);
        market.ClosePage();
    }
}
