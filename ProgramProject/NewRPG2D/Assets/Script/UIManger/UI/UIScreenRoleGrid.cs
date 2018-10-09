using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScreenRoleGrid : MonoBehaviour
{
    public Image image_Photo;
    public Image image_TypeIcon;
    public Text txt_Point;
    public Text txt_Name;
    public Text txt_Level;
    public Text txt_NeedGold;
    public Text txt_Tip;
    public Text txt_Time;
    public GameObject TrainType;

    public void UpdateInfo(HallRoleData data, RoleAttribute needAtr)
    {
        TrainType.SetActive(false);
        txt_Name.text = data.Name;
        txt_Level.text = data.GetAtrProduce(needAtr).ToString();
        if (data.currentRoom != null)
        {
            txt_Point.text = data.currentRoom.RoomName.ToString();
            if (data.currentRoom.BuildingData.RoomType == RoomType.Training)
            {
                //如果是训练类的房间
                TrainType.SetActive(true);
            }
        }
        else
        {
            txt_Point.text = "漫游";
        }
    }
}
