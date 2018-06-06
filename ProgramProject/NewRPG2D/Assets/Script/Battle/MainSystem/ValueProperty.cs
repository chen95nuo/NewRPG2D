using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Battle
{
    public struct PropertyData
    {
        public float Hp;
        public float Mp;
        public float Attack;
        public float Defense;
        public float Speed;
        public float AttackSpeed;
        public float AttackCritial;
        public float MoveSpeed;
        public float HitEnemyHeal;
        public float ReduceCD;
        public float FireHurt;
        public float DizzyChance;
        public float ReflectHurt;
        public float HealTeam;
        public float BloodHurt;
    }

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

        public void InitRoleValue(PropertyData rolePropertyData)
        {
            RoleHp = rolePropertyData.Hp;
            RoleMp = rolePropertyData.Mp;
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
