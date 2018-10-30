/*
 * 角色功能
 * 新建时获取属性
 * 换装
 * 动画切换
*/

using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using Assets.Script.UIManger;
using UnityEngine;

public class HallRole : MonoBehaviour
{
    public int roleId;
    public RoleBabyData currentBaby;
    private HallRoleData currentData;
    public Transform TipPoint;
    public SexTypeEnum sex;
    public bool isChildren = false;

    #region 换装
    public ChangeRoleEquip RoleSkinEquip;
    #endregion
    public HallRoleData RoleData
    {
        get
        {
            if (currentData == null)
            {
                currentData = new HallRoleData();
            }
            return currentData;
        }
    }

    public void UpdateInfo(HallRoleData data)
    {
        currentData = data;
        sex = data.sexType;
        HallRoleMgr.instance.AddRole(data, this);
        isChildren = false;
        ChickEquip(data);
    }
    private void Update()
    {

    }
    public void UpdateInfo(RoleBabyData baby)
    {
        currentBaby = baby;
        sex = baby.child.sexType;
        HallRoleMgr.instance.AddRole(baby.child, this);
        isChildren = true;
    }

    private void ChickEquip(HallRoleData data)
    {
        for (int i = 0; i < data.Equip.Length; i++)
        {
            if (data.Equip[i] != 0)
            {
                ChangeSkil(data.Equip[i]);
            }
        }
    }

    public void ChangeSkil(int id)
    {
        EquipmentRealProperty equipment = EquipmentMgr.instance.CreateNewEquipment(id);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
    }
    public void ChangeSkil(EquipmentRealProperty equipment)
    {
        Debug.Log("换皮肤 类型：" + equipment.EquipType + "  Name: " + equipment.EquipName);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName, sex);
    }

    public void ChangeType(BuildRoomName name)
    {
        Debug.Log("漫游状态");
    }
    public void InstanceNewBody()
    {

    }


    /// <summary>
    /// 角色训练完成
    /// </summary>
    /// <param name="type"></param>
    public void TrainComplete(TrainType type)
    {
        UIPanelManager.instance.ShowPage<UIRoleTipGroup>();
        UIRoleTipGroup.instance.ShowIcon(this);
    }

    public void LoveComplete()
    {
        UIPanelManager.instance.ShowPage<UIRoleTipGroup>();
        UIRoleTipGroup.instance.ShowChildIcon(this);
    }

    public void BabyComplete()
    {
        UIPanelManager.instance.ShowPage<UIRoleTipGroup>();
        UIRoleTipGroup.instance.ShowBabyIcon(this);
    }
}