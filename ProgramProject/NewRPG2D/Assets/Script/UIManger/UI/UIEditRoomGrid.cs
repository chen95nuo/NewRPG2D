using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIEditRoomGrid : MonoBehaviour
{
    public Button btn_enter;
    public Text txt_name;
    public EditModeHelper s_data;

    private void Awake()
    {
        btn_enter.onClick.AddListener(ChickEndter);
        HallEventManager.instance.AddListener<LocalBuildingData>(HallEventDefineEnum.AddBuild, ChickNumber);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<LocalBuildingData>(HallEventDefineEnum.AddBuild, ChickNumber);
    }

    public void UpdateInfo(EditModeHelper data)
    {
        s_data = data;
        txt_name.text = data.buildingData[0].buildingData.RoomName.ToString() + " 数量 : " + data.number;
    }

    private void ChickEndter()
    {
        EditCastle.instance.AddBuilding(s_data.buildingData[0]);
    }

    private void ChickNumber(LocalBuildingData data)
    {
        ChickNumber();
    }
    public void ChickNumber()
    {
        if (s_data.buildingData.Count <= 0)
        {
            return;
        }
        txt_name.text = s_data.buildingData[0].buildingData.RoomName.ToString() + " 数量 : " + s_data.number;
    }
}
