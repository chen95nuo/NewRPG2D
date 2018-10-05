using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMarketGrid : MonoBehaviour
{

    public BuildingData buildingData;
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
        buildingData = data;
        txt_Number.text = number[0] + "/" + number[1];
        thisGrid.interactable = isTrue;
    }

    private void AddBuilding()
    {
        Debug.Log(buildingData.RoomName);
        MainCastle.instance.BuildRoomTip(buildingData);
        market.ClosePage();
    }
}
