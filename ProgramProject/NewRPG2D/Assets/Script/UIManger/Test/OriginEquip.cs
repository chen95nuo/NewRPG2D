using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginEquip : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Invoke("StartEquip", 1);
    }

    private void StartEquip()
    {
        if (EquipmentMgr.instance.GetAllEquipmentData().Count <= 0)
        {
            for (int i = 1; i < 5; i++)
            {
                int index = 10000 + i;
                EquipmentMgr.instance.CreateNewEquipment(index, 1);
            }
            ItemInfoManager.instance.CreateNewBox(1001);
        }
    }
}
