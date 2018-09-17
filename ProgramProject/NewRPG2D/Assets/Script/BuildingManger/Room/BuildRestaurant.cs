using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildRestaurant : RoomMgr
{
    public int index = 0;

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
        if (roomFunc == false)
        {
            return;
        }
        bool isTrue = LocalServer.instance.SetNumber(buildingData);

        if (isTrue)
        {
            //如果数量小于1 那么关闭提示框 关闭收获提示
            if (buildingData.Param4 <= 1)
            {
                Debug.Log(" 关闭提示框 ");
                roomProp.SetActive(false);
                isHarvest = false;
            }
            //显示动画并刷新数字
            else
            {
                Debug.Log("显示了1");
                roomProp.SetActive(true);
                isHarvest = true;
            }
            HallEventManager.instance.SendEvent(HallEventDefineEnum.FoodSpace);
        }
        else
        {
            //仓库已满
            Debug.Log("仓库已满");
        }
    }

    public override void GetRoomMaterial(int number)
    {
        if (roomFunc == false)
        {
            return;
        }
        //显示可获取
        isHarvest = true;
        Debug.Log("显示了2");
        roomProp.SetActive(true);
    }


}
