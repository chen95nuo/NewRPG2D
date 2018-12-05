using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpTips_4Grid : MonoBehaviour
{
    public Text txt_name;
    public Text[] txt_amounts;
    public Button btn_Click;
    private LocalBuildingData currentData;

    public void UpdateInfo(LocalBuildingData data)
    {
        currentData = data;
        txt_name.text = LanguageDataMgr.instance.GetRoomName(data.buildingData.RoomName.ToString());

        foreach (var item in txt_amounts)
        {
            item.transform.parent.gameObject.SetActive(false);
        }

        for (int i = 0; i < data.buildingData.needMaterial.Length; i++)
        {
            if (data.buildingData.needMaterial[i] > 0)
            {
                txt_amounts[i].transform.parent.gameObject.SetActive(true);
                txt_amounts[i].text = data.buildingData.needMaterial[i].ToString();
            }
        }
    }

    public void OnClickRoom()
    {
        CameraControl.instance.LockRoom(currentData);
    }
}
