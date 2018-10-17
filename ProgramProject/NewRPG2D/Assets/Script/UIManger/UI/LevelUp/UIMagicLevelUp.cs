using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script.UIManger;

public class UIMagicLevelUp : TTUIPage
{

    public UIMagicGrid[] grids;

    public override void Show(object mData)
    {
        base.Show(mData);
        RoomMgr room = mData as RoomMgr;
        UpdateInfo(room);
    }

    public void UpdateInfo(RoomMgr room)
    {
        for (int i = 0; i < grids.Length; i++)
        {
            if (room.currentBuildData.buildingData.Param1 > i)
            {
                int level = ChickPlayerInfo.instance.GetMagicLevel((MagicName)i);
                MagicData data = MagicDataMgr.instance.GetMagic((MagicName)i, level + 1);
                grids[i].UpdateInfo(data, room.BuildingData.Level);
            }
            else
            {
                MagicData data = MagicDataMgr.instance.GetMagic((MagicName)i, 1);
                grids[i].UpdateInfo(data, 0);
            }
        }
    }
}
