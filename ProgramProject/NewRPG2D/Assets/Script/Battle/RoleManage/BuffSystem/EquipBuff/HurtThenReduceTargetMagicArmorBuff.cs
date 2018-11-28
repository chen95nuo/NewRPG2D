﻿/// <summary>
///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class HurtThenReduceTargetMagicArmorBuff : HurtTriggerBuff
    {
        private float magicArmorPercent;

        private float duration;
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            magicArmorPercent = param2 * 0.01f;
            duration = param3;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                Target.BuffMoment.AddBuff(BuffTypeEnum.ReduceMagicArmor, magicArmorPercent, duration);
                return true;
            }
            return false;
        }
    }
}
