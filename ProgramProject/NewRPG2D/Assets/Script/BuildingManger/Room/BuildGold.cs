using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGold : RoomMgr, IProduction
{
    //暂无角色先用默认产能代替
    public float Yield { get { return buildingData.Param1; } }
    public float Stock { get { return stock; } set {stock = value; } }

    public override void GetNumber(int number)
    {
        base.GetNumber(number);
    }

    public override void ProductionType()
    {
        base.ProductionType();
    }

    public override void RoomAwake()
    {
        if (roomFunc == false)
        {
            return;
        }
        LocalServer.instance.GetNumber(this);
    }

    public override void ThisRoomFunc()
    {

    }
}
