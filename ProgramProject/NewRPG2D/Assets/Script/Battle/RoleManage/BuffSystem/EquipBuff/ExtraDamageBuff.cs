
namespace Assets.Script.Battle
{
    public class ExtraDamageBuff : AlwayTriggerBuff
    {
        private float buffDuration;
        private float constDamage;
        private float magicAddtiveDamge;
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            buffDuration = param2;
            constDamage = param3;
            magicAddtiveDamge = param4;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {

                if (Target != null)
                {
                    Target.BuffMoment.AddBuff(BuffTypeEnum.ExtraDamage, buffDuration, constDamage + magicAddtiveDamge * MagicValue);
                }
                return true;
            }

            return false;
        }

    }
}
