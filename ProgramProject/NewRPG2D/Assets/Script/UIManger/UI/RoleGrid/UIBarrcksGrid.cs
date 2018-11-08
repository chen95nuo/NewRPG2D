using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarrcksGrid : UIRoleGridMgr
{
    public Text txt_LevelTip;
    public GameObject lockOBJ;

    public override void UpdateInfo(UIRoomInfo info)
    {
        base.UpdateInfo(info);

    }

    public override void UpdateInfo(HallRoleData role, UIRoomInfo info)
    {
        base.UpdateInfo(role, info);
    }

    public void UpdateLockInfo(UIRoomInfo roomInfo, int ins)
    {
        LockType(true, ins + 1);
    }

    private void LockType(bool isTrue, int index = 0)
    {
        lockOBJ.SetActive(isTrue);
        txt_Level.gameObject.SetActive(!isTrue);
        txt_LevelTip.gameObject.SetActive(isTrue);
        txt_LevelTip.text = string.Format("需要{0}级军营", index);
    }
}
