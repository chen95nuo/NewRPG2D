/// <summary>
///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class ContinueDamage : RoleEquipSpecialBuff
    {
        public override TirggerTypeEnum TirggerType
        {
            get
            {
                return TirggerTypeEnum.Attack;
            }
        }

        private float triggerChange;
        private float duration;
        private float extraDamage;

        private bool canTrigger;
        private float addTime;
        private HurtInfo extraHurtInfo;
        // private int firstTargetId;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            triggerChange = param1;
            duration = param2;
            extraDamage = param3;
        }

        private float intervalTime = 0;

        public override void UpdateLogic(float deltaTime)
        {
            base.UpdateLogic(deltaTime);
            if (canTrigger)
            {
                addTime += deltaTime;
                if (addTime < duration)
                {
                    if (intervalTime < 1)
                    {
                        intervalTime += deltaTime;
                    }
                    else
                    {
                        intervalTime = 0;
                        currentRole.RoleDamageMoment.HurtDamage(ref extraHurtInfo);
                    }
                }
                else
                {
                    canTrigger = false;
                }
            }
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 1f) < triggerChange)
            {
                canTrigger = true;
                addTime = 0;
                extraHurtInfo = info;
                // firstTargetId = info.TargeRole.InstanceId;
                extraHurtInfo.HurtValue = extraDamage / (int)duration;
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
