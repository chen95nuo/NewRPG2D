/// <summary>
///攻击时，有x%概率时敌人流血，在y秒内造成z点伤害
/// </summary>

namespace Assets.Script.Battle
{
    public class HurtThenHealHp : HurtTriggerBuff
    {
        private float healHpPercent;


        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            healHpPercent = param2 * 0.01f;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                currentRole.RolePropertyValue.SetHp(-currentRole.RolePropertyValue.MaxHp * healHpPercent);
                return true;
            }
            return false;
        }
    }
}
