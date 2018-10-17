using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UITrainLevelUp : UILevelUp
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;

    public Text txt_Number;
    public Text txt_UpNumber;



    protected override void Init(RoomMgr data)
    {
        base.Init(data);
        BuildingData d1 = data.currentBuildData.buildingData;
        BuildingData d2 = BuildingDataMgr.instance.GetXmlDataByItemId<BuildingData>(d1.NextLevelID);

        txt_Number.text = d1.Param1.ToString();
        txt_UpNumber.text = (d2.Param1 - d1.Param1).ToString();
    }
}
