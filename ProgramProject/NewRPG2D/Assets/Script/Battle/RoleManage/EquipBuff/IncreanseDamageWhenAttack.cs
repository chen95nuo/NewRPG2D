/// <summary>
///每次攻击都提升x%点伤害，当前目标死亡时，损失所有伤害提升
/// </summary>
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.Script.Battle
{
    public class IncreanseDamageWhenAttack : RoleEquipSpecialBuff
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

        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
            extraDamagePercent = param1;
        }

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                extraDamage = currentRole.RolePropertyValue.Damage*extraDamagePercent;
                allExtraDamage += extraDamage;
                currentRole.RolePropertyValue.SetDamage(extraDamage);
                enemyRole = info.TargeRole;
                return true;
            }
            else if(tirggerType == TirggerTypeEnum.Death)
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
