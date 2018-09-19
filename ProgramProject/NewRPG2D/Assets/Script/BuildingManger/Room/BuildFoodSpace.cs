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
            if (index != stock)
            {
                stock = index;
                //HallEventManager.instance.AddListener<>(HallEventDefineEnum.ChickStock, GetNumber);
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

    public void GetNumber(ServerBuildData storageRoom)
    {
        if (roomFunc == false)
        {
            return;
        }
        if (storageRoom.buildingData.RoomName == RoomName)
        {
            stock = storageRoom.Stock;
            Debug.Log("仓库库存 :" + stock);
        }
    }
}
