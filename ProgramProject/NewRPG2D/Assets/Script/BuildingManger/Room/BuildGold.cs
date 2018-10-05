using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGold : RoomMgr
{
    //暂无角色先用默认产能代替
    public float Yield { get { return BuildingData.Param1; } }
    public float Stock { get { return stock; } set { stock = value; } }


    public override void RoomAwake()
    {
        if (roomFunc == false)
        {
            return;
        }
    }

    public override void ThisRoomFunc()
    {

    }
}
