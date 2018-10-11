using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Script.UIManger;

public class UILivingRoomInfo : UIRoomInfo
{
    public Text txt_Tip_1;
    public Text txt_Tip_2;
    public Text txt_roleNumber;
    public Image[] gridBG;

    List<UIRoleGrid> roleGrids = new List<UIRoleGrid>();

    protected override void UpdateInfo(RoomMgr roomMgr)
    {
        txt_Tip_1.text = "居民数量";
        txt_Tip_2.text = "此房间可以增加城堡的最大居民数量并让居民们生育下一代，\n父母的潜能越高，孩子的潜能可能就越高";
        txt_roleNumber.text = roomMgr.currentBuildData.buildingData.Param2.ToString();

        ChickRoleNumber(roleGrids);
        for (int i = 0; i < roleGrids.Count; i++)
        {
            if (roomMgr.currentBuildData.roleData[i] != null)
            {
                roleGrids[i].UpdateInfo(roomMgr.currentBuildData.roleData[i], BuildRoomName.Nothing);
            }
            else
            {
                roleGrids[i].UpdateInfo();
            }
        }

        //刷角色格子背景数量
        switch (roomMgr.BuildingData.RoomSize)
        {
            case 3:
                ChickGridBG(1);
                break;
            case 6:
                ChickGridBG(2);
                break;
            case 9:
                ChickGridBG(3);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 刷新角色位置背景数量
    /// </summary>
    /// <param name="index"></param>
    private void ChickGridBG(int index)
    {
        for (int i = 0; i < gridBG.Length; i++)
        {
            if (i < index)
            {
                gridBG[i].gameObject.SetActive(true);
            }
            else
            {
                gridBG[i].gameObject.SetActive(false);
            }
        }
    }
}
