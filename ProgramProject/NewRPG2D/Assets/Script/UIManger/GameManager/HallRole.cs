/*
 * 角色功能
 * 新建时获取属性
 * 换装
 * 动画切换
*/

using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Battle;
using Assets.Script.UIManger;
using UnityEngine;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using DG.Tweening;

public class HallRole : MonoBehaviour
{
    public int roleId;
    public RoleBabyData currentBaby;
    private HallRoleData currentData;
    public Transform TipPoint;
    public SexTypeEnum sex;
    public bool isChildren = false;
    public float MoveSpeed = 1;
    private Sequence mySequence;

    #region 换装
    public ChangeRoleEquip RoleSkinEquip;
    #endregion
    [SerializeField]
    private SkeletonAnimation roleAnim;
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

    private void Awake()
    {
        mySequence = DOTween.Sequence();
    }

    public void UpdateInfo(HallRoleData data)
    {
        currentData = data;
        sex = data.sexType;
        HallRoleMgr.instance.AddRole(data, this);
        isChildren = false;
        ChickEquip(data);
    }

    public void UpdateInfo(RoleBabyData baby)
    {
        currentBaby = baby;
        currentData = baby.child;
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
        EquipmentRealProperty equipment = EquipmentMgr.instance.GetEquipmentByEquipId(id);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
    }
    public void ChangeOrigin(EquipTypeEnum type)
    {
        RoleSkinEquip.ChangeOriginalEquip(type);
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

    public void RoleMove(Vector2 endPoint)
    {
        roleAnim.AnimationState.SetAnimation(1, "run", true);
        List<Vector2> v = MainCastle.instance.GetNowWallGrid(transform.position, endPoint);
        RoleMove(v);
    }

    public void RoleMove(List<Vector2> v)
    {
        mySequence.Kill(true);
        //Vector3[] path = new Vector3[v.Count];
        //for (int i = 0; i < v.Count; i++)
        //{
        //    path[i] = new Vector3(v[i].x, v[i].y - 1.05f, transform.position.z);
        //}
        //transform.DOPath(path, 10, PathType.Linear, PathMode.Sidescroller2D, 0).OnComplete(RoleMoveEnd);
        bool isInside = false;
        bool isUp = false;
        Vector3 startPoint = new Vector3(transform.position.x, v[0].y);

        mySequence.Append(transform.DOMove(startPoint, MoveTime(transform.position, startPoint)));

        for (int i = 1; i < v.Count - 1; i++)
        {
            if (isInside)
            {

                if (isUp)
                {

                }
                else
                {

                }
            }
            isInside = !isInside;
        }
        mySequence.Append(transform.DOMove(v[v.Count - 1], MoveTime(transform.position, v[v.Count - 1])));
    }

    public float MoveTime(Vector3 from, Vector3 to)
    {
        float t = 0;
        t = Vector3.Distance(from, to) * Time.deltaTime;
        return t;
    }

    public void RoleMoveEnd()
    {
        roleAnim.AnimationState.SetAnimation(1, "stand", true);
    }

    public void RoleAnim()
    {

    }

    /// <summary>
    /// 角色训练完成
    /// </summary>
    /// <param name="type"></param>
    public void TrainComplete(TrainType type)
    {
        UIPanelManager.instance.ShowPage<UIRoleTipGroup>();
        UIRoleTipGroup.instance.ShowIcon(this, type);
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

    public void Clear()
    {
        roleId = 0;
        currentBaby = null;
        currentData = null;
    }
}