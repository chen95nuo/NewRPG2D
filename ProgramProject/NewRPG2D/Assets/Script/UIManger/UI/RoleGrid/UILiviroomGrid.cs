using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILiviroomGrid : UIRoleGridMgr
{
    public Text txt_Type;

    public override void UpdateInfo(UIRoomInfo info)
    {
        base.UpdateInfo(info);
        txt_Type.text = "";
    }

    public override void UpdateInfo(HallRoleData role, UIRoomInfo info)
    {
        base.UpdateInfo(role, info);
        if (role.LoveType == RoleLoveType.WaitFor)
        {
            txt_Type.text = "等待伴侣中";
        }
        else txt_Type.text = "";
    }

    protected override void ShowLevelUp(bool isShow, HallRoleData role = null)
    {
    }
}
