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
            CriticalDamage(attackProperty, targeProperty, ref hurtValue);
            switch (hurtInfo.HurtType)
            {
                case HurtTypeEnum.Physic:
                    FinallyPhysicDamage(attackProperty, targeProperty, ref hurtValue);
                    break;
                case HurtTypeEnum.Real:
                    FinallyRealDamage(attackProperty, targeProperty, ref hurtValue);
                    break;
            }

            bool isLive = hurtInfo.TargeRole.RolePropertyValue.SetHp(hurtValue);
            if (isLive == false)
            {
                hurtInfo.TargeRole.SetRoleActionState(ActorStateEnum.Death);
            }
            else
            {
                hurtInfo.TargeRole.SetRoleActionState(ActorStateEnum.Hit);
            }
        }

        private void RolePropertyDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {

            if (attackProperty.RoleProperty == targeProperty.DefenseProperty)
            {
                hurtValue = hurtValue * BattleStaticAndEnum.RolePropertyAttackAddtion;
            }
        }

        private void CriticalDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {
            float criticalPercent = BattleStaticAndEnum.RoleBaseCriticalPercent + attackProperty.CriticalPercent;
            int promptOffeset = Mathf.Max(0, (int)(attackProperty.Prompt - targeProperty.Prompt));
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
            float promptOffeset = attackProperty.Prompt - targeProperty.Prompt;
            float hurtPercentRange = 0, hurtPercentRangeMin = 0f, hurtPercentRangeMax = 0f;
            if (promptOffeset > 0)
            {
                hurtPercentRangeMin = (BattleStaticAndEnum.RoleHurtRangePercentMin +
                                       BattleStaticAndEnum.RolePromptOffsetTime * promptOffeset /
                                       BattleStaticAndEnum.RolePromptOffsetMax);
                hurtPercentRangeMax = BattleStaticAndEnum.RoleHurtRangePercentMax;
            }
            else
            {
                hurtPercentRangeMin = BattleStaticAndEnum.RoleHurtRangePercentMin;
                hurtPercentRangeMax = BattleStaticAndEnum.RoleHurtRangePercentMax + BattleStaticAndEnum.RolePromptOffsetTime * promptOffeset /
                                       BattleStaticAndEnum.RolePromptOffsetMax;
            }
            hurtPercentRange = Random.Range(hurtPercentRangeMin, hurtPercentRangeMax);
            float defensePercentRange = UnityEngine.Random.Range(BattleStaticAndEnum.RoleDefenseRangePercentMin,
                            BattleStaticAndEnum.RoleDefenseRangePercentMax);

            hurtValue = hurtValue*BattleStaticAndEnum.RoleHurtPercent*hurtPercentRange -
                        targeProperty.Defense*BattleStaticAndEnum.RoleDefensePercent*defensePercentRange;

            hurtValue = Mathf.Max(1, hurtValue);

        }

        private void FinallyRealDamage(ValueProperty attackProperty, ValueProperty targeProperty, ref float hurtValue)
        {
           

        }
    }
}
