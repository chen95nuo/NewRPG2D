using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Battle
{
    public class SearchTarget
    {
        private RoleBase mCurrentRole, targetRole;
        private Transform roleTransform;

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
            if (targetRole != null && targetRole.IsDead)
            {
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
        }
    }
}
