using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Battle
{
    public class ValueProperty
    {
        public float RoleHp { get; private set; }
        public float RoleMp { get; private set; }
        public float Attack;
        public float MoveSpeed { get; private set; }

        private RoleBase currenRole;

        public void SetCurrentRole(RoleBase mRole)
        {
            currenRole = mRole;
        }

        public void InitRoleValue(float Hp, float Mp)
        {
            RoleHp = Hp;
            RoleMp = Mp;
        }

        public void SetMoveSeed(float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }
        public bool SetHp(float HpChange)
        {
            RoleHp -= HpChange;
            return RoleHp > 0;
        }
        public bool SetMp(float MpChange)
        {
            RoleMp -= MpChange;
            return RoleMp > 0;
        }
    }
}
