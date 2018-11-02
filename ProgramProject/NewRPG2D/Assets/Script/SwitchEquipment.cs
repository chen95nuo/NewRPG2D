using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using Assets.Script.Battle.Equipment;
using UnityEngine;

public class SwitchEquipment : MonoBehaviour
{
    public ChangeRoleEquip RoleSkinEquip;

    private EquipmentRealProperty equipment;
    private void Start()
    {
      
    }

    void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.A))
	    {
            equipment = EquipmentMgr.instance.CreateNewEquipment(10001);
            RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            equipment = EquipmentMgr.instance.CreateNewEquipment(10002);
            RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ItemDataInTreasure box = TreasureBoxMgr.instance.OpenTreasureBox(1004,5,5);

            for (int i = 0; i < box.EquipmentList.Count; i++)
            {
                Debug.Log(box.EquipmentList[i].Name);
            }

//             for (int i = 0; i < box.PropDataList.Count; i++)
//             {
//                 Debug.Log(box.PropDataList[i].Name);
//             }
        }
    }
}
