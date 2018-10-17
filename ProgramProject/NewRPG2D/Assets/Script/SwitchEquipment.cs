using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
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
            equipment = EquipmentMgr.instance.CreateNewEquipment(1001);
            RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);

        }
    }
}
