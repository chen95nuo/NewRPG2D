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
        if (equipment.EquipType == EquipTypeEnum.Armor)
        {
            Debug.Log("换装换图");
            GetSpriteAtlas.insatnce.GetRoleIcon(currentData, true);
        }
    }
    public void ChangeOrigin(EquipTypeEnum type)
    {
        RoleSkinEquip.ChangeOriginalEquip(type);
        Debug.Log("换装换图");
        GetSpriteAtlas.insatnce.GetRoleIcon(currentData, true);
    }

    public void ChangeSkil(EquipmentRealProperty equipment)
    {
        Debug.Log("换皮肤 类型：" + equipment.EquipType + "  Name: " + equipment.EquipName);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName, sex);
        if (equipment.EquipType == EquipTypeEnum.Armor)
        {
            Debug.Log("换装换图");
            GetSpriteAtlas.insatnce.GetRoleIcon(currentData, true);
        }
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
        mySequence.Kill();

        mySequence = DOTween.Sequence();

        Vector2 startPoint = new Vector2(transform.position.x, v[0].y);
        mySequence.Append(transform.DOMove(startPoint, MoveTime(transform.position, startPoint)).SetEase(Ease.Linear));

        for (int i = 0; i < v.Count; i++)
        {
            Vector2 previous = i == 0 ? startPoint : v[i - 1];
            float faceType = v[i].x < previous.x ? -.35f : .35f;
            mySequence.Append(transform.DOScale(new Vector3(faceType, .35f, .35f), 0));
            if (v[i].y == previous.y)
            {
                mySequence.Append(transform.DOMove(v[i], MoveTime(previous, v[i])).SetEase(Ease.Linear));
                mySequence.Append(transform.DOMove(new Vector2(v[i].x, v[i].y + .2f), MoveTime(v[i], new Vector2(v[i].x, v[i].y + .2f))));
            }
            else
            {
                if (i + 1 < v.Count && v[i].y == v[i + 1].y)
                {
                    mySequence.Append(transform.DOScale(new Vector3(.1f, .1f, .1f), 1f));
                    mySequence.Append(transform.DOMove(new Vector2(v[i].x, v[i].y + .2f), 0).SetEase(Ease.Linear));
                    mySequence.Append(transform.DOScale(new Vector3(.35f, .35f, .35f), 1f));
                    mySequence.Append(transform.DOMove(v[i], MoveTime(new Vector2(v[i].x, v[i].y + .2f), v[i])).SetEase(Ease.Linear));
                }
            }
        }
        mySequence.OnComplete(RoleMoveEnd);
    }

    public float MoveTime(Vector3 from, Vector3 to)
    {
        float t = 0;
        t = Vector3.Distance(from, to) / MoveSpeed;
        return t;
    }

    public void RoleMoveEnd()
    {

        roleAnim.AnimationState.SetAnimation(1, "stand", true);
        Debug.Log("移动结束开始房间内动画");

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