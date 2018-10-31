using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class BuildBarracks : RoomMgr
{
    public override bool AddRole(HallRole role)
    {
        if (role.RoleData.LoveType == RoleLoveType.ChildBirth)
        {
            return false;
        }
        int index = 0;
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] != null)
            {
                index++;
            }
        }
        if (index >= currentBuildData.buildingData.Param2)
        {
            object st = "军营人数已满，请升级";
            UIPanelManager.instance.ShowPage<UIPopUp_2>(st);
            return false;
        }
        base.AddRole(role);
        return true;
    }
}
