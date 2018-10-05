using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildThroneRoom : RoomMgr
{
    private void Awake()
    {
        GetCompoment();
        ThisRoomFunc();
    }

    public override void RoomAwake()
    {
    }

    public override void ThisRoomFunc()
    {
        GetPlayerData.Instance.GetData();
    }
}
