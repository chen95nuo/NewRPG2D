using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIThroneLevelUpGrid : MonoBehaviour
{
    public Text txt_Name;
    public Text txt_Type;

    public Image RoomIcon_1;
    public Image RoomIcon_2;
    public Image RoomIcon_3;
    public Image RoomIcon_4;

    public void UpdateInfo(BuildingData data, ThroneInfoType type)
    {
        txt_Name.text = data.RoomName.ToString();
        Sprite sp = GetSpriteAtlas.insatnce.GetIcon(data.RoomName.ToString());
        switch (data.RoomSize)
        {
            case 1:
                RoomIcon_1.gameObject.SetActive(true);
                RoomIcon_1.sprite = sp;
                break;
            case 3:
                RoomIcon_2.gameObject.SetActive(true);
                RoomIcon_2.sprite = sp;
                break;
            case 6:
                RoomIcon_3.gameObject.SetActive(true);
                RoomIcon_3.sprite = sp;
                break;
            case 9:
                RoomIcon_4.gameObject.SetActive(true);
                RoomIcon_4.sprite = sp;
                break;
            default:
                break;
        }

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
