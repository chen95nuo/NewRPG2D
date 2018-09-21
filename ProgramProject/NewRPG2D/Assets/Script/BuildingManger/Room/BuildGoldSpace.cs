using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGoldSpace : RoomMgr, IStorage
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
            if (index != stock)
            {
                stock = index;
            }
            Debug.Log(stock);
        }
    }
    private void Awake()
    {
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

    public override void RoomAwake()
    {
    }

    public override void ThisRoomFunc()
    {

    }

    public override void GetNumber(ServerBuildData storageRoom)
    {
        base.GetNumber(storageRoom);
    }
}
