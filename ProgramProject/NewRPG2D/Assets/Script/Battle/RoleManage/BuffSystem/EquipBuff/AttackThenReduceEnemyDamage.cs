/// <summary>
///每次攻击都提升x%点伤害，当前目标死亡时，损失所有伤害提升
/// </summary>
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.Script.Battle
{
    public class AttackThenReduceEnemyDamage : AttackTriggerBuff
    {
        private float extraDamagePercent;
        private float duration;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);

            extraDamagePercent = param2 * 0.01f;
            duration = param3;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                Target.BuffMoment.AddBuff(BuffTypeEnum.ReduceDamage, duration, extraDamagePercent);
                return true;
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
