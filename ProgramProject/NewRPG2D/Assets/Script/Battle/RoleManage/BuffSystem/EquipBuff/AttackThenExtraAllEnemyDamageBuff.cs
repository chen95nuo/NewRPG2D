/// <summary>
///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class AttackThenExtraAllEnemyDamageBuff : AttackTriggerBuff
    {
        private float buffDuration;
        private float damagePercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            buffDuration = param2;
            damagePercent = param3 * 0.01f;
        }

      
        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                HurtAllEnemyBuff(currentRole.RolePropertyValue.Damage * damagePercent, buffDuration);
                return true;
            }
            return false;
        }
    }
}
