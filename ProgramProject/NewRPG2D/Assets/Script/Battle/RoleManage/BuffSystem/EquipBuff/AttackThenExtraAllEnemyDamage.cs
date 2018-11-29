/// <summary>
/// 以X+（法术强度*Y)点生命值复活一名盟友。Z秒冷却
/// </summary>

namespace Assets.Script.Battle
{
    public class AttackThenExtraAllEnemyDamage : AttackTriggerBuff
    {
        private float  damagePercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            damagePercent = param2 * 0.01f;
        }


        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                HurtAllEnemy(currentRole.RolePropertyValue.Damage * damagePercent);
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
