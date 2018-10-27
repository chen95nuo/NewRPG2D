/// <summary>
///攻击时，有x%概率提高自身y%的暴击。持续z秒
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class IncreaseCritial : RoleEquipSpecialBuff
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
        private float extraCritialPercent;
        private float extraCritial;

        private bool canTrigger;
        private float addTime;


        public override void Init(RoleBase role, float param1, float param2, float param3)
        {
            base.Init(role, param1, param2, param3);
            triggerChange = param1;
            duration = param2;
            extraCritialPercent = param3;
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
                    currentRole.RolePropertyValue.SetCriticalPercent(-extraCritial);
                }
            }
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info) && Random.Range(0, 1f) < triggerChange)
            {
                canTrigger = true;
                addTime = 0;
                extraCritial = currentRole.RolePropertyValue.CriticalPercent * extraCritialPercent;
                currentRole.RolePropertyValue.SetCriticalPercent(extraCritial);
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
