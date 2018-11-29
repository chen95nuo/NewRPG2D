/// <summary>
///攻击时，有x%概率使一名敌人的护甲降低y%，持续z秒
/// </summary>
/// 

namespace Assets.Script.Battle
{
    public class AttackThenReduceTargetArmorBuff : AttackTriggerBuff
    {
        private float duration;
        private float reduceArmorPercent;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            reduceArmorPercent = param2 * 0.01f;
            duration = param3;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info) && RandomValue < triggerChange)
            {
      
                Target.BuffMoment.AddBuff(BuffTypeEnum.ReduceArmor, duration, reduceArmorPercent);
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
