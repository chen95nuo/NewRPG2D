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
    public RoleBabyData currentBaby;
    private HallRoleData currentData;
    public Transform TipPoint;
    public bool isChildren = false;

    #region 换装
    public ChangeRoleEquip RoleSkinEquip;
    private EquipmentRealProperty equipment;
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
        HallRoleMgr.instance.AddRole(data, this);
        isChildren = false;
    }
    private void Update()
    {

    }
    public void UpdateInfo(RoleBabyData baby)
    {
        currentBaby = baby;
        HallRoleMgr.instance.AddRole(baby.child, this);
        isChildren = true;
    }

    private void ChangeSkil(int id)
    {
        equipment = EquipmentMgr.instance.CreateNewEquipment(id);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
    }

    public void ChangeType(BuildRoomName name)
    {
        Debug.Log("漫游状态");
    }
    public void InstanceNewBody()
    {
        //HallRoleMgr.instance.
        //RoleSkinEquip.ChangeBody(BodyTypeEnum.Beard,);
        //RoleSkinEquip.ChangeBody(BodyTypeEnum.Beard,);
        //RoleSkinEquip.ChangeBody(BodyTypeEnum.Beard,);
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

}