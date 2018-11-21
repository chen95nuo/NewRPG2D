using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildBabyRoom : RoomMgr
{
    public override bool AddRole(HallRole role)
    {
        if (currentBuildData.roleData == null)
        {
            Debug.LogError("房间角色空间出错");
        }
        for (int i = 0; i < currentBuildData.roleData.Length; i++)
        {
            if (currentBuildData.roleData[i] == null)
            {
                currentBuildData.roleData[i] = role.RoleData;
                Vector3 point = new Vector3(transform.position.x + (1.2f * (i + 1)), transform.position.y + 0.3f, role.transform.position.z);
                role.transform.position = point;
                if (role.RoleData.currentRoom != null)
                {
                    role.RoleData.currentRoom.RemoveRole(role);
                }
                role.RoleData.currentRoom = this;
                return true;
            }
        }
        return false;
    }
}
