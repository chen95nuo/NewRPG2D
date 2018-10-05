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
        PlayerData data = GetPlayerData.Instance.GetData();
        data.MainHall = currentBuildData;
    }

    public override void ChickComplete()
    {
        PlayerData data = GetPlayerData.Instance.GetData();
        data.MainHall = currentBuildData;
    }
}
