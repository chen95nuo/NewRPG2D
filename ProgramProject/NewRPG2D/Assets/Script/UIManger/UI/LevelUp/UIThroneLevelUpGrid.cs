using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIThroneLevelUpGrid : MonoBehaviour
{
    public Image roomImage;
    public Text txt_Name;
    public Text txt_Type;

    public void UpdateInfo(BuildingData data, ThroneInfoType type)
    {
        txt_Name.text = data.RoomName.ToString();
        switch (type)
        {
            case ThroneInfoType.Upgraded:
                txt_Type.text = "可升级";
                break;
            case ThroneInfoType.Build:
                txt_Type.text = "新";
                break;
            default:
                break;
        }
    }
}
