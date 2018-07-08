using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class SearchTarget
    {
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

        public void SetTarget(RoleBase targetRole)
        {
            this.targetRole = targetRole;
        }

        public void Update()
        {
            if (targetRole != null && targetRole.IsDead ==false)
            {
                if (mCurrentRole.CurrentSlot != cacheSkillSlot)
                {
                    cacheSkillSlot = mCurrentRole.CurrentSlot;
                    attackDistance = mCurrentRole.RoleSkill.GetSkillDataBySkilSlot(cacheSkillSlot).AttackRange;
                }

                if (Vector3.Distance(targetRole.RoleTransform.position, roleTransform.position) > attackDistance)
                {
                   mCurrentRole.RoleMoveMoment.SetTargetPosition(GetMovePosition(roleTransform, targetRole.RoleTransform,attackDistance));
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
            movePosition = targetRoleTransform.position + (currentRoleTransform.position - targetRoleTransform.position).normalized * attackDistance;
            return movePosition;
        }
    }
}
