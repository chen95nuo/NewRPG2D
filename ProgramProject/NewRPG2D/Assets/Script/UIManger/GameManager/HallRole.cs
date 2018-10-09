/*
 * 角色功能
 * 新建时获取属性
 * 换装
 * 动画切换
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class HallRole : MonoBehaviour
{
    private HallRoleData currentData;
    public HallRoleData RoleData
    {
        get
        {
            if (currentData == null)
            {
                currentData = new HallRoleData();
            }
            return currentData;
        }
    }



    public void UpdateInfo(HallRoleData data)
    {
        currentData = data;
        HallRoleMgr.instance.AddRole(data, this);
    }

    public void ChangeSkil()
    {

    }

    public void ChangeType(BuildRoomName name)
    {
        Debug.Log("漫游状态");
    }

    public void TrainComplete(TrainType type)
    {
        UIPanelManager.instance.ShowPage<UIRoleTrainGroup>();
        UIRoleTrainGroup.instance.ShowIcon(this, type);

    }

}