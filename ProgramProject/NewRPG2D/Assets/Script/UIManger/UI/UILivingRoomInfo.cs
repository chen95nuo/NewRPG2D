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

    public UILivingRoomHelper[] Grids = new UILivingRoomHelper[3];
    private List<UIRoleGrid> roleGrids = new List<UIRoleGrid>();

    protected override void Awake()
    {
        base.Awake();
        HallEventManager.instance.AddListener<int>(HallEventDefineEnum.ChickRoleLove, ChickLoveCallBack);
    }

    private void OnDestroy()
    {
        HallEventManager.instance.RemoveListener<int>(HallEventDefineEnum.ChickRoleLove, ChickLoveCallBack);
    }

    protected override void UpdateInfo(RoomMgr roomMgr)
    {
        txt_Tip_1.text = "居民数量";
        txt_Tip_2.text = "此房间可以增加城堡的最大居民数量并让居民们生育下一代，\n父母的潜能越高，孩子的潜能可能就越高";
        txt_roleNumber.text = roomMgr.currentBuildData.buildingData.Param2.ToString();

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
        //检查同性
        ChickTimeText(roomMgr);

        ChickRoleNumber(roleGrids);
        for (int i = 0; i < roleGrids.Count; i++)
        {
            if (roomMgr.currentBuildData.roleData[i] != null)
            {
                roleGrids[i].UpdateLivineRoom(roomMgr.currentBuildData.roleData[i], this, i);
            }
            else
            {
                roleGrids[i].UpdateLivineRoom(this, i);
            }
        }
    }

    /// <summary>
    /// 查找同性
    /// </summary>
    /// <param name="roomMgr"></param>
    private void ChickTimeText(RoomMgr roomMgr)
    {
        HallRoleData[] data = roomMgr.currentBuildData.roleData;
        for (int i = 0; i < data.Length; i += 2)
        {
            Debug.Log("i :" + i);
            if (i % 2 == 0 && data[i] != null
                && data[i + 1] != null && data[i].sexType == data[i + 1].sexType)
            {
                data[i].LoveType = RoleLoveType.Nothing;
                data[i + 1].LoveType = RoleLoveType.Nothing;
                int num = 0;
                if (i == 0 || i == 1)
                {
                    num = 0;
                }
                else if (i == 2 || i == 3)
                {
                    num = 1;
                }
                else if (i == 4 || i == 5)
                {
                    num = 2;
                }
                else
                {
                    Debug.LogError("角色位置错误");
                    return;
                }
                Grids[num].UpdateDontNothing();
            }
        }
    }

    /// <summary>
    /// 刷新角色位置背景数量
    /// </summary>
    /// <param name="index"></param>
    private void ChickGridBG(int index)
    {
        for (int i = 0; i < Grids.Length; i++)
        {
            Grids[i].UpdateInfo();
            if (i < index)
            {
                Grids[i].gameObject.SetActive(true);
            }
            else
            {
                Grids[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 恋爱倒计时
    /// </summary>
    /// <param name="index"></param>
    private void ChickLoveCallBack(int index)
    {
        RoleLoveHelper loveData = HallRoleMgr.instance.GetLoveData(index);
        HallRoleData[] roleData = roomData.currentBuildData.roleData;
        for (int i = 0; i < roleData.Length; i++)
        {
            if (roleData[i].id == loveData.roleID[0] && roleData[i + 1].id == loveData.roleID[1])
            {
                Debug.Log("找到房间对应角色");
                string st = SystemTime.instance.TimeNormalizedOfMin(loveData.time);
                float temp = (float)(loveData.allTime - loveData.time) / (float)loveData.allTime;
                int num = 0;
                if (i == 0 || i == 1)
                {
                    num = 0;
                }
                else if (i == 2 || i == 3)
                {
                    num = 1;
                }
                else if (i == 4 || i == 5)
                {
                    num = 2;
                }
                else
                {
                    Debug.LogError("角色位置错误");
                    return;
                }
                Grids[num].UpdateInfo(temp, st);
                return;
            }
        }
    }
}
