using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildThroneRoom : RoomMgr
{
    private void Awake()
    {
        GetCompoment();
    }

    public override void RoomAwake()
    {
    }

    public override void ThisRoomFunc()
    {
    }

    public override void ChickComplete()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        data.MainHall = currentBuildData;
    }
}
