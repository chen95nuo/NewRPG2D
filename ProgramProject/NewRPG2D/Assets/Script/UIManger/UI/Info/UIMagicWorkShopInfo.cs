using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMagicWorkShopInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Num;

    public Image slider;

    private List<UIWorkShopTypeGrid> roleGrids = new List<UIWorkShopTypeGrid>();

    protected override void Awake()
    {
        base.Awake();
        txt_Tip_2.text = LanguageDataMgr.instance.GetString("WorkShopTip");
    }
    protected override void UpdateInfo(LocalBuildingData roomMgr)
    {
        int num = ChickRoleNumber(roleGrids);
        txt_Tip_2.gameObject.SetActive(num == 0 ? false : true);
        txt_DownTip.text = LanguageDataMgr.instance.GetInfo(roomMgr.buildingData.RoomName.ToString());

    }

    private void LeftUpTip(BuildingData data)
    {
        slider.fillAmount = data.Param2 / 6;
        txt_Num.text = data.Param2.ToString();
    }
}
