using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Script.Battle
{
    public class ValueProperty
    {
        public RolePropertyEnum RoleProperty;  //自己的属性
        public RolePropertyEnum AttackProperty; //克制的属性
        public RolePropertyEnum DefenseProperty; // 被克制的属性
        public float RoleHp { get; private set; }
        public float RoleMp { get; private set; }
        public float Attack { get; private set; }
        public float Defense { get; private set; }
        public float Prompt { get; private set; }   //敏捷
        public float CriticalPercent { get; private set; }  //暴击
        public float MoveSpeed { get; private set; }

        private RoleBase currenRole;

        public void SetCurrentRole(RoleBase mRole)
        {
            currenRole = mRole;
        }

        public void InitBaseRoleValue(PropertyBaseData rolePropertyData)
        {
            RoleProperty = rolePropertyData.RoleProperty;
            RoleHp = rolePropertyData.Hp.BaseValue;
            Attack = rolePropertyData.Attack.BaseValue;
            Defense = rolePropertyData.Defense.BaseValue;
            Prompt = rolePropertyData.Prompt.BaseValue;
        }

        public void InitRoleOtherValue(PropertyData rolePropertyData)
        {
            AttackProperty = rolePropertyData.AttackProperty;
            DefenseProperty = rolePropertyData.DefenseProperty;
            CriticalPercent = rolePropertyData.AttackCritial;
        }

        public void SetMoveSeed(float moveSpeed)
        {
            MoveSpeed = moveSpeed;
        }
        public bool SetHp(float HpChange)
        {
            RoleHp -= HpChange;
            EventManager.instance.SendEvent(EventDefineEnum.HpChange, currenRole);
            DebugHelper.Log("name=  " + currenRole.RoleTransform.name + " hp " + RoleHp);
            return RoleHp > 0;
        }

        public bool SetMp(float MpChange)
        {
            RoleMp -= MpChange;
            return RoleMp > 0;
        }

        public void SetAttack(float attackChange)
        {
            Attack += attackChange;
        }

        public void SetDefense(float defenseChange)
        {
            Defense += defenseChange;
        }

        public void SetPrompt(float promptChange)
        {
            Prompt += promptChange;
        }
    }
}
