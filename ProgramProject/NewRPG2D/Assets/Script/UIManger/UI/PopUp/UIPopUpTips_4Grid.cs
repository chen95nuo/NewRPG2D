using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUpTips_4Grid : MonoBehaviour
{
    public Text txt_name;
    public Text[] txt_amounts;

    public void UpdateInfo(LocalBuildingData data)
    {
        if (data.buildingData.NeedGold > 0)
        {

        }
        if (data.buildingData.NeedMana > 0)
        {

        }
        if (data.buildingData.NeedWood > 0)
        {

        }
        if (data.buildingData.NeedIron > 0)
        {

        }

    }
}
