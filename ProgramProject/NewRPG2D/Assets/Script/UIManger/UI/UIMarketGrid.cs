using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketGrid : MonoBehaviour
{

    public BuildingData building;
    public UIMarket market;
    public Text buildName;
    private Button thisGrid;
    private void Awake()
    {
        thisGrid = GetComponent<Button>();
        thisGrid.onClick.AddListener(AddBuilding);
    }

    public void UpdateBuilding(BuildingData data)
    {
        buildName.text = data.RoomName;
        building = data;
    }

    private void AddBuilding()
    {
        Debug.Log(building.RoomName);
        HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.AddBuild, building);
        market.ClosePage();
    }
}
