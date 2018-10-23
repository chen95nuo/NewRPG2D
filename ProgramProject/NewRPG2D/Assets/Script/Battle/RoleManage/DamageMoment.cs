using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Script.Battle
{

    public class DamageMoment
    {
        private RoleBase currenRole;


        public void SetCurrentRole(RoleBase mRole)
        {
            currenRole = mRole;
        }

        public void HurtDamage(ref HurtInfo hurtInfo)
        {
            ValueProperty attackProperty = hurtInfo.AttackRole.RolePropertyValue;
            ValueProperty targeProperty = hurtInfo.TargeRole.RolePropertyValue;
            float hurtValue = hurtInfo.HurtValue;
            RolePropertyDamage(attackProperty, targeProperty, ref hurtValue);
          //  CriticalDamage(attackProperty, targeProperty, ref hurtValue);
            switch (hurtInfo.HurtType)
            {
                case HurtTypeEnum.Physic:
                    FinallyPhysicDamage(attackProperty, targeProperty, ref hurtValue);
                    break;
                case HurtTypeEnum.Magic:
                    FinallyMagicDamage(attackProperty, targeProperty, ref hurtValue);
                    break;
            }

            bool isLive = hurtInfo.TargeRole.RolePropertyValue.SetHp(hurtValue, hurtInfo.AttackRole);
            if (isLive == false)
            {
                hurtInfo.TargeRole.IsCanInterrput = true;
                hurtInfo.TargeRole.SetRoleActionState(ActorStateEnum.Death);
            }
            else
            {
                hurtInfo.TargeRole.RoleWeapon.TriggerBuff(TirggerTypeEnum.Hurt, ref hurtInfo);
                hurtInfo.TargeRole.SetRoleActionState(ActorStateEnum.Hit);
            }
        }

        private void RolePropertyDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {
            bool bCri = (Random.Range(0, 1) < attackProperty.CriticalPercent);
            if ((Random.Range(0, 1) <
                 attackProperty.HitPercent / (attackProperty.HitPercent + targeProperty.AviodHurtPercent)))
            {
                hurtValue = 0;
            }
            else
            {
                hurtValue = hurtValue * (bCri ? 2 : 1);
            }
        }

        private void CriticalDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {
            float criticalPercent = BattleStaticAndEnum.RoleBaseCriticalPercent + attackProperty.CriticalPercent;
            int promptOffeset = 1;// Mathf.Max(0, (int)(attackProperty.Prompt - targeProperty.Prompt));
            int promptOffesetPercent = promptOffeset / BattleStaticAndEnum.RolePromptCalculate;
            criticalPercent = criticalPercent +
                              (promptOffesetPercent * 1.0f) /
                              (promptOffesetPercent + BattleStaticAndEnum.RolePromptCalculateCritical);
            if (Random.Range(0, 100) < criticalPercent)
            {
                hurtValue = hurtValue * BattleStaticAndEnum.RoleCriticalAttackAddtion;
            }
        }

        private void FinallyPhysicDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {
           
            hurtValue = Mathf.Max(0, hurtValue * (1 - targeProperty.PhysicArmor / (100 + targeProperty.PhysicArmor)));

        }

        private void FinallyMagicDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {

            hurtValue = Mathf.Max(0, hurtValue * (1 - targeProperty.MagicArmor / (100 + targeProperty.MagicArmor)));
        }
    }
}
