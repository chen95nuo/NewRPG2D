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
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using DG.Tweening;

public class HallRole : MonoBehaviour

{
    public int roleId;
    public RoleBabyData currentBaby;
    private HallRoleData currentData;
    public int roleRoomIndex;
    public Transform TipPoint;
    public SexTypeEnum sex;
    public bool isChildren = false;
    public float RunSpeed = 1f;
    public float MoveSpeed = .35f;
    public int currentBuildIndex;
    private Sequence mySequence;
    private bool isMove = false;

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
        isChildren = false;
        data.instance = this;
        ChickFace();
        ChickEquip();
    }

    public void UpdateInfo(RoleBabyData baby)
    {
        currentBaby = baby;
        currentData = baby.child;
        sex = baby.child.sexType;
        HallRoleMgr.instance.AddRole(baby.child, this);
        isChildren = true;
    }
    //刷新脸部皮肤
    private void ChickFace()
    {
        return;
        for (int i = 0; i < currentData.faceSpriteID.Length; i++)
        {
            if (currentData.faceSpriteID[i] != 0)
            {
                ChangeSkil(currentData.faceSpriteID[i]);
            }
        }
    }
    //刷新装备皮肤
    private void ChickEquip()
    {
        for (int i = 0; i < currentData.Equip.Length; i++)
        {
            if (currentData.Equip[i] != 0)
            {
                ChangeSkil(currentData.Equip[i]);
            }
        }
    }

    public void ChangeSkil(int id)
    {
        EquipmentRealProperty equipment = EquipmentMgr.instance.GetEquipmentByEquipId(id);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName);
        if (equipment.EquipType == EquipTypeEnum.Armor)
        {
            GetSpriteAtlas.insatnce.GetRoleIcon(currentData, true);
        }
    }
    public void ChangeOrigin(EquipTypeEnum type)
    {
        RoleSkinEquip.ChangeOriginalEquip(type);
        GetSpriteAtlas.insatnce.GetRoleIcon(currentData, true);
    }

    public void ChangeSkil(EquipmentRealProperty equipment)
    {
        Debug.Log("换皮肤 类型：" + equipment.EquipType + "  Name: " + equipment.EquipName);
        RoleSkinEquip.ChangeEquip(equipment.EquipType, equipment.EquipName, sex);
        if (equipment.EquipType == EquipTypeEnum.Armor)
        {
            GetSpriteAtlas.insatnce.GetRoleIcon(currentData, true);
        }
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

        isMove = true;

        Vector2 startPoint = new Vector2(transform.position.x, v[0].y);
        mySequence.Append(transform.DOMove(startPoint, MoveTime(Vector3.Distance(transform.position, startPoint))).SetEase(Ease.Linear))
        .Join(transform.DOScale(new Vector3(.35f, .35f, .3f), MoveTime(.2f)).SetEase(Ease.Linear));

        for (int i = 0; i < v.Count; i++)
        {
            Vector2 previous = i == 0 ? startPoint : v[i - 1];
            float faceType = v[i].x < previous.x ? -1 : 1;
            mySequence.Append(roleAnim.transform.DOScale(new Vector3(faceType, 1, 1), 0));
            if (v[i].y == previous.y)
            {
                mySequence.Append(transform.DOMove(v[i], MoveTime(Vector3.Distance(previous, v[i]))).SetEase(Ease.Linear))
                .Append(transform.DOMove(new Vector2(v[i].x, v[i].y + .2f), MoveTime(.2f)))
                .Join(transform.DOScale(new Vector3(.3f, .3f, .3f), MoveTime(.2f)).SetEase(Ease.Linear));
            }
            else
            {
                if (i + 1 < v.Count && v[i].y == v[i + 1].y)
                {
                    mySequence.Append(transform.DOScale(new Vector3(.1f, .1f, .1f), 1f))
                    .Append(transform.DOMove(new Vector2(v[i].x, v[i].y + .2f), 0).SetEase(Ease.Linear))
                    .Append(transform.DOScale(new Vector3(.35f, .35f, .35f), 1f))
                    .Append(transform.DOMove(v[i], MoveTime(.2f)).SetEase(Ease.Linear))
                    .Join(transform.DOScale(new Vector3(.35f, .35f, .3f), MoveTime(.2f)).SetEase(Ease.Linear));
                }
            }
        }
        mySequence.OnComplete(RoleMoveEnd);
    }

    public float MoveTime(float dis, bool isRun = true)
    {
        float t = 0;
        t = dis / (isRun ? RunSpeed : MoveSpeed);
        return t;
    }

    public void RoleMoveEnd()
    {
        isMove = false;
        Debug.Log("移动结束开始房间内动画");
        roleAnim.transform.localScale = Vector3.one;
        RoleAnim();

    }

    public void RoleAnim()
    {

        if (currentData.currentRoom == null)
        {
            Debug.Log("移动到下一房间");
            RoleWorldRoam();
        }
        else if (currentData.currentRoom.RoomWork)
        {
            mySequence.Kill();
            mySequence = DOTween.Sequence();
            Debug.Log("移动到工作位置");
            Vector2 temp = currentData.currentRoom.RolePoint.transform.position;
            Vector2 localPoint = temp + currentData.currentRoom.currentBuildData.buildingData.RolePoint[currentBuildIndex];
            Vector2 endPoint = new Vector2(localPoint.x, localPoint.y + .2f);
            if ((Vector2)transform.position != endPoint)
            {
                isMove = true;
                roleAnim.AnimationState.SetAnimation(1, "run", true);
                roleAnim.transform.localScale = new Vector3(transform.position.x < endPoint.x ? 1 : -1, 1, 1);
                mySequence.Append(transform.DOMove(endPoint, MoveTime(Vector3.Distance(transform.position, endPoint))).SetEase(Ease.Linear))
               .Join(transform.DOScale(new Vector3(.3f, .3f, .3f), MoveTime(.2f)).SetEase(Ease.Linear));
                mySequence.OnComplete(RoleMoveEnd);
            }
            else
            {
                RoleWorkAnim();
            }
        }
        else
        {
            Debug.Log("移动到下一闲置位置");
            RoleRoam();
        }
    }

    public void RoleRoam()
    {
        if (isMove)
        {
            return;
        }
        isMove = true;
        mySequence.Kill();
        mySequence = DOTween.Sequence();
        float x = currentData.currentRoom.RolePointMin;
        float roll = Random.Range(x, -x + 1);
        Vector2 endPoint = new Vector2(roll, transform.localPosition.y);
        float dis = Vector2.Distance(transform.localPosition, endPoint);
        roleAnim.AnimationState.SetAnimation(1, "walk1", true);
        roleAnim.transform.localScale = new Vector3(transform.localPosition.x < roll ? 1 : -1, 1, 1);
        mySequence.Append(transform.DOLocalMove(endPoint, MoveTime(dis, false)).SetEase(Ease.Linear).OnComplete(RoleRoamAnim));
    }

    private void RoleWorkAnim()
    {
        if (isMove)
        {
            return;
        }
        Debug.Log("运行工作动画");
        switch (currentData.currentRoom.RoomName)
        {
            case BuildRoomName.FighterRoom:
                WeapoinsAnim(currentData.currentRoom.RoomName.ToString());
                break;
            case BuildRoomName.Barracks:
                WeapoinsAnim(currentData.currentRoom.RoomName.ToString());
                roleAnim.AnimationState.AddAnimation(1, "stand", true, 5);
                Invoke("RoleWorkAnim", 10f);
                break;
            default:
                string workAnim = currentData.currentRoom.currentBuildData.buildingData.RoleAnim[currentBuildIndex];
                roleAnim.AnimationState.SetAnimation(1, workAnim, true);
                break;
        }
    }

    private void WeapoinsAnim(string frontName)
    {
        if (currentData.Equip[0] == 0)
        {
            roleAnim.AnimationState.SetAnimation(1, "train_hand", false);
        }
        else
        {
            EquipmentRealProperty equipData = EquipmentMgr.instance.GetEquipmentByEquipId(currentData.Equip[0]);
            string lastName = "";
            if (equipData.WeaponType == WeaponTypeEnum.Sword || equipData.WeaponType == WeaponTypeEnum.Axe)
            {
                lastName = "cut";
            }
            else
            {
                lastName = equipData.WeaponType.ToString();
            }

            roleAnim.AnimationState.SetAnimation(1, frontName + "_" + lastName, false);
        }
    }
    private void WeapoinsAnimLoop()
    {

    }

    public void RoleWorldRoam()
    {
        Debug.Log("运行漫游动画");
        roleAnim.AnimationState.SetAnimation(1, "work_common_3", false).Complete += HallRole_Complete;
    }
    private void RoleRoamAnim()
    {
        isMove = false;

        Debug.Log("运行闲置动画");
        //Random roll =
        roleAnim.AnimationState.SetAnimation(1, "work_common_3", false).Complete += HallRole_Complete;
    }

    private void HallRole_Complete(TrackEntry trackEntry)
    {
        Debug.Log("AnimComplete");
        RoleAnim();
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