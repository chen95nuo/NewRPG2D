using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UIBarracksInfo : UIRoomInfo
{
    public Text txt_AllFight;
    public Text txt_Tip_1;
    public Text txt_Tip_2;

    private List<UIRoleGrid> roleGrids = new List<UIRoleGrid>();

    protected override void UpdateInfo(RoomMgr roomMgr)
    {
        txt_AllFight.text = "19999";
        txt_Tip_1.text = "战斗力";
        txt_Tip_2.text = "升级军营来增加参与战斗的人数";

        ChickRoleNumber(roleGrids);
        for (int i = 0; i < roleGrids.Count; i++)
        {
            if (roomMgr.currentBuildData.roleData[i] != null)
            {
                roleGrids[i].UpdateInfo(roomMgr.currentBuildData.roleData[i], BuildRoomName.Barracks);
                btnAddLister(roleGrids[i].btn_ShowAllRole);
            }
            else
            {
                roleGrids[i].UpdateInfo();
            }
        }
        int number = (int)roomMgr.currentBuildData.buildingData.Param2;
        for (int i = number; i < roleGrids.Count; i++)
        {
            roleGrids[i].btn_ShowAllRole.interactable = false;
        }
    }
}
