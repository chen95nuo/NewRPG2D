using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketGrid : MonoBehaviour
{

    public BuildingData building;
    public UIMarket market;
    public Text buildName;
    public Text txt_Number;
    private Button thisGrid;
    private void Awake()
    {
        thisGrid = GetComponent<Button>();
        thisGrid.onClick.AddListener(AddBuilding);
    }

    public void UpdateBuilding(BuildingData data, int[] number, bool isTrue)
    {
        buildName.text = data.RoomName.ToString();
        building = data;
        txt_Number.text = number[0] + "/" + number[1];
        thisGrid.interactable = isTrue;
    }

    private void AddBuilding()
    {
        Debug.Log(building.RoomName);
        HallEventManager.instance.SendEvent<BuildingData>(HallEventDefineEnum.AddBuild, building);
        market.ClosePage();
    }
}
