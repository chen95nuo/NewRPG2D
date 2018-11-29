/// <summary>
///攻击时，x%概率使被击中的敌人所受的伤害提高x%。效果持续y秒。
/// </summary>

namespace Assets.Script.Battle
{
    public class AttackThenEnemyExtraDamageBuff : AttackTriggerBuff
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
                Target.BuffMoment.AddBuff(BuffTypeEnum.IncreaseEnemyDamage, duration, extraDamagePercent);
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
