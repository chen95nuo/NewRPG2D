/// <summary>
///每次攻击都提升x%点伤害，当前目标死亡时，损失所有伤害提升
/// </summary>

namespace Assets.Script.Battle
{
    public class AttackThenIncreaseDamage : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }
        private float extraDamagePercent;
        private float extraDamage, allExtraDamage;
        private bool canTrigger;
        private RoleBase enemyRole;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            extraDamagePercent = param1 * 0.01f;
        }


        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                if (enemyRole != null && enemyRole == info.TargeRole)
                {
                    currentRole.RolePropertyValue.SetDamage(allExtraDamage);
                }

                {
                        
                    extraDamage = currentRole.RolePropertyValue.Damage*extraDamagePercent;
                    allExtraDamage += extraDamage;
                    currentRole.RolePropertyValue.SetDamage(extraDamage);
                    enemyRole = info.TargeRole;
                }
                return true;
            }
            else if(triggerType == TirggerTypeEnum.Death)
            {
                if (enemyRole.IsDead)
                {
                    enemyRole = null;
                    currentRole.RolePropertyValue.SetDamage(allExtraDamage);
                }
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
