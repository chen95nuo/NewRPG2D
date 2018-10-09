using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITrainInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_Tip_3;
    public Text txt_Tip_4;
    public Text txt_MaxLevel;


    List<UITrainRoleGrid> roleGrids = new List<UITrainRoleGrid>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected override void UpdateInfo(RoomMgr roomMgr)
    {
        txt_Tip_1.text = "最高等级";
        txt_Tip_2.text = "广告";
        txt_Tip_3.text = "将城堡内的居民移动至该房间，提升居民的     等级。/n     能够影响战斗表现。";
        txt_Tip_4.text = "缩短40分钟";
        txt_MaxLevel.text = roomMgr.BuildingData.Param1.ToString();

        ChickRoleNumber(roleGrids);
        for (int i = 0; i < roleGrids.Count; i++)
        {
            if (roomData.currentBuildData.roleData[i] != null)
            {
                roleGrids[i].UpdateInfo(roomData.currentBuildData.roleData[i]);
            }
            else
            {
                roleGrids[i].UpdateInfo();
            }
        }
    }



}
