using System;
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

        public void SetCurrentRole(RoleBase mRole)
        {
            mCurrentRole = mRole;
            roleTransform = mRole.RoleTransform;
        }

        public void InitData()
        {
            cacheSkillSlot = SkillSlotTypeEnum.NormalAttack;
            attackDistance = mCurrentRole.RoleSkill.GetSkillDataBySkilSlot(cacheSkillSlot).AttackRange;
          //  attackDistance = attackDistance * attackDistance;
        }

        public void SetTarget(RoleBase targetRole)
        {
            this.targetRole = targetRole;
        }

        public void Update()
        {
            if(mCurrentRole.FinishMoveToPoint == false)
            {
                return;
            }
            if (targetRole != null && targetRole.IsDead == false)
            {
                if (mCurrentRole.CurrentSlot != cacheSkillSlot)
                {
                    cacheSkillSlot = mCurrentRole.CurrentSlot;
                    attackDistance = mCurrentRole.RoleSkill.GetSkillDataBySkilSlot(cacheSkillSlot).AttackRange;
                  //  attackDistance = attackDistance * attackDistance;
                }

                if ((targetRole.RoleTransform.position - roleTransform.position).magnitude > attackDistance)
                {
                    mCurrentRole.RoleMoveMoment.SetTargetTranform(targetRole.RoleTransform);
                    mCurrentRole.RoleMoveMoment.SetTargetMinDistance(attackDistance);
                   
                }
                else
                {
                    //Debug.LogError("have target then attack ");
                    mCurrentRole.SetRoleActionState(ActorStateEnum.NormalAttack);
                }
                //没到攻击距离时往目标走，
            }
            else
            {
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
                if (mCurrentRole.TeamId == allRole[i].TeamId)
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


        private Vector3 GetMovePosition(Transform currentRoleTransform, Transform targetRoleTransform, float attackDistance)
        {
            Vector3 movePosition;
            movePosition = targetRoleTransform.position + (targetRoleTransform.position - currentRoleTransform.position).normalized * attackDistance;
            return movePosition;
        }
    }
}
