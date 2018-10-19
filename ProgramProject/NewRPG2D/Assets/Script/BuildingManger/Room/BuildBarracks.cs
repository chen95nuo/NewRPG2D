using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBarracks : RoomMgr
{
    public override bool AddRole(HallRole role)
    {
        if (role.RoleData.LoveType == RoleLoveType.ChildBirth)
        {
            return false;
        }
        base.AddRole(role);
        return true;
    }
}
