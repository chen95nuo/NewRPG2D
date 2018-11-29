/// <summary>
///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
/// </summary>

namespace Assets.Script.Battle
{
    public class HurtThenGod : HurtTriggerBuff
    {
        private float healHp;

        private float duration;
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            healHp = param2;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                currentRole.BuffMoment.AddBuff(BuffTypeEnum.Freezing, duration, healHp);
                return true;
            }
            return false;
        }
    }
}
