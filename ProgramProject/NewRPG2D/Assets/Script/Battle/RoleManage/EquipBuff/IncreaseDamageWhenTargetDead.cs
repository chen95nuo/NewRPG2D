/// <summary>
///当前攻击目标被击杀时，x秒内造成的伤害提高y%
/// </summary>
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Assets.Script.Battle
{
    public class IncreaseDamageWhenTargetDead : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.TargetDeath;
            }
        }

        private float triggerChange;
        private float duration;
        private float extraDamagePercent;
        private float extraDamage;
        private bool canTrigger;
        private float addTime;


        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
            duration = param1;
            extraDamagePercent = param2;
        }

        private float intervalTime = 0;

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            if (canTrigger)
            {
                addTime += deltaTime;
                if (addTime >= duration)
                {
                    canTrigger = false;
                    currentRole.RolePropertyValue.SetDamage(-extraDamage);
                }
            }
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 1f) < triggerChange)
            {
                canTrigger = true;
                addTime = 0;
                extraDamage = currentRole.RolePropertyValue.Damage * extraDamagePercent;
                currentRole.RolePropertyValue.SetDamage(extraDamage);
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
