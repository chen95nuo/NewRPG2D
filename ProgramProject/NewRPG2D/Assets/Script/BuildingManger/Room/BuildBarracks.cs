using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBarracks : RoomMgr
{
    public override void AddRole(HallRole role)
    {
        if (role.RoleData.LoveType == RoleLoveType.ChildBirth)
        {
            return;
        }
        base.AddRole(role);
    }
}
