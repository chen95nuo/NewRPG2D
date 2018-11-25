/// <summary>
///攻击时，有x%概率提高自身y%的暴击。持续z秒
/// </summary>
using UnityEngine;

namespace Assets.Script.Battle
{
    public class AttackThenIncreaseCritial : AttackTriggerBuff
    {
        private float duration;
        private float extraCritialPercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            duration = param2;
            extraCritialPercent = param3 * 0.01f;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                currentRole.BuffMoment.AddBuff(BuffTypeEnum.ChangeCritial, duration, extraCritialPercent);
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
