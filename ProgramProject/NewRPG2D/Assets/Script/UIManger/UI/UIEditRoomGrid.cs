using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIEditRoomGrid : MonoBehaviour
{
    public Button btn_enter;
    public Text txt_Name;
    public Text txt_Num;
    public Text txt_Des;
    public EditModeHelper s_data;

    public Image RoomIcon;

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
        string st = LanguageDataMgr.instance.GetRoomName(data.buildingData[0].buildingData.RoomName.ToString());
        txt_Name.text = st;

        string des = LanguageDataMgr.instance.GetRoomDes(data.buildingData[0].buildingData.RoomName.ToString());
        txt_Des.text = des;

        Sprite sp = GetSpriteAtlas.insatnce.GetRoomSp(data.buildingData[0].buildingData.RoomName.ToString());
        RoomIcon.sprite = sp;
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
        txt_Num.text = s_data.number.ToString();
    }
}
