using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBabyRoom : RoomMgr
{
    public override void AddRole(HallRole role)
    {
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] == null)
            {
                break;
            }
        }
        base.AddRole(role);
    }
}
