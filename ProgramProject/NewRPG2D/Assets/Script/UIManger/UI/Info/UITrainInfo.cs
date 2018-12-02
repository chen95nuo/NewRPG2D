using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UITrainInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_MaxLevel;
    public Slider slider_maxLevel;

    public Image[] tipIcon;

    List<UITrainRoleGrid> roleGrids = new List<UITrainRoleGrid>();

    protected override void UpdateInfo(LocalBuildingData roomMgr)
    {
        string space_1 = "       ";
        string space_2 = "     ";
        txt_Tip_1.text = "最高等级";
        txt_Tip_2.text = "广告";
        txt_Tip_3.text = string.Format("将城堡内的居民移动至该房间，提升居民的{0}等级。\n{1}能够影响战斗表现。", space_1, space_2);
        txt_Tip_4.text = "缩短<color=#8BFF7F>40</color>分钟";
        txt_MaxLevel.text = roomMgr.buildingData.Param2.ToString();
        slider_maxLevel.value = roomMgr.buildingData.Param2;
        ChickRoleNumber(roleGrids);
        Sprite sp = GetSpriteAtlas.insatnce.GetLevelIconToRoom(roomMgr.buildingData.RoomName);
        for (int i = 0; i < roleGrids.Count; i++)
        {
            if (roomData.roleData[i] != null)
            {
                roleGrids[i].UpdateInfo(roomData.roleData[i], sp, this);
            }
            else
            {
                roleGrids[i].UpdateInfo(this);
            }
        }

        for (int i = 0; i < tipIcon.Length; i++)
        {
            tipIcon[i].sprite = sp;
        }
    }
}
