using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildFoodSpace : RoomMgr, IStorage
{
    public float Stock
    {
        get
        {
            return stock;
        }

        set
        {
            float index = value;
            if (index != base.Stock)
            {
                stock = index;
            }
        }
    }
    private void Awake()
    {
        if (roomFunc == false)
        {
            return;
        }
        HallEventManager.instance.AddListener<ServerBuildData>(HallEventDefineEnum.ChickStock, GetNumber);
    }

    private void OnDestroy()
    {
        if (roomFunc == false)
        {
            return;
        }
        HallEventManager.instance.RemoveListener<ServerBuildData>(HallEventDefineEnum.ChickStock, GetNumber);
    }

    public override void ThisRoomFunc()
    {

    }

    public override void RoomAwake()
    {

    }

    public override void GetNumber(ServerBuildData storageRoom)
    {
        base.GetNumber(storageRoom);
    }
}
