﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class SearchTarget
    {
        public RoleBase Target
        {
            get { return targetRole; }
        }
        private RoleBase mCurrentRole, targetRole;
        private Transform roleTransform;

        private SkillSlotTypeEnum cacheSkillSlot;
        private float attackDistance;
        private List<RoleBase> allRole;
        private bool bStartAttack = false;
        private float limitAngle;

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleTransform = mRole.RoleTransform;
        }

        public void InitData()
        {
            cacheSkillSlot = SkillSlotTypeEnum.NormalAttack;
            attackDistance = mCurrentRole.RolePropertyValue.AttackRange;
            // Debug.LogError("attack range == " + attackDistance);
            //  attackDistance = attackDistance * attackDistance;
            limitAngle = mCurrentRole.AttackType == AttackTypeEnum.ShortRange ? 40 : 90;
        }

        public void SetTarget(RoleBase targetRole)
        {
            this.targetRole = targetRole;
        }

        private float addTime = 0;
        public void Update()
        {
            if (mCurrentRole.FinishMoveToPoint == false)
            {
                targetRole = null;
                return;
            }
            if (targetRole != null && targetRole.IsDead == false)
            {
                if (mCurrentRole.CurrentSlot != cacheSkillSlot)
                {
                    cacheSkillSlot = mCurrentRole.CurrentSlot;
                    attackDistance = mCurrentRole.RolePropertyValue.AttackRange;
                    //  attackDistance = attackDistance * attackDistance;
                }
                Vector3 dis = targetRole.RoleTransform.position - roleTransform.position;
                if (dis.magnitude > attackDistance)
                {
                    mCurrentRole.RoleMoveMoment.SetTargetTranform(targetRole.RoleTransform);
                    mCurrentRole.RoleMoveMoment.SetTargetMinDistance(attackDistance);
                    bStartAttack = false;
                    addTime += Time.deltaTime;
                    if (addTime > 0.933f)
                    {
                        addTime = 0;
                        SearchEnemyByDistance();
                    }
                }
                else
                {
                    if (bStartAttack == false)
                    {
                        bStartAttack = true;
                        SearchEnemyByDistance();
                        mCurrentRole.SetRoleActionState(ActorStateEnum.NormalAttack);
                    }
                }
                //没到攻击距离时往目标走，
            }
            else
            {
                bStartAttack = false;
                SearchEnemyByDistance();
            }
        }

        private void SearchEnemyByDistance()
        {
            targetRole = null;
            if (allRole == null)
            {
                allRole = GameRoleMgr.instance.RolesList;
            }
            mCurrentRole.SetRoleActionState(ActorStateEnum.Idle);
            float minDistance = float.MaxValue;
            for (int i = 0; i < allRole.Count; i++)
            {
                if (mCurrentRole.TeamId == allRole[i].TeamId || allRole[i].IsDead)
                {
                    continue;
                }

                float distance = (roleTransform.position - allRole[i].RoleTransform.position).magnitude;
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetRole = allRole[i];
                }
            }
        }


        private Vector3 GetMovePosition(Transform currentRoleTransform, Transform targetRoleTransform, float _attackDistance)
        {
            Vector3 movePosition;
            movePosition = targetRoleTransform.position + (targetRoleTransform.position - currentRoleTransform.position).normalized * _attackDistance;
            return movePosition;
        }
    }
}
