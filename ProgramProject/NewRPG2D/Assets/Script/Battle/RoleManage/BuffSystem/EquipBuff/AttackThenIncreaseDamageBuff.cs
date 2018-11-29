/// <summary>
/// 攻击时，有x%概率在y秒内造成z%点分裂伤害
/// </summary>

namespace Assets.Script.Battle
{
    public class AttackThenIncreaseDamageBuff : AttackTriggerBuff
    {
 
        private float duration;
        private float extraDamagePercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param2;
            extraDamagePercent = param3;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                currentRole.BuffMoment.AddBuff(BuffTypeEnum.IncreaseDamage, duration, extraDamagePercent);
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
