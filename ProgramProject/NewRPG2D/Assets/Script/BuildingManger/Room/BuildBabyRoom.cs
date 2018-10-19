using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBabyRoom : RoomMgr
{
    public void AddRole(HallRole role)
    {
        int index = 0;
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] == null)
            {
                index++;
                break;
            }
        }
        //AddRole(data.child);
    }
    //public override void AddRole(HallRole role)
    //{
    //    base.AddRole(role);
    //}
}
