
namespace Assets.Script.Battle
{
    public class ExtraDamage : AlwayTriggerBuff
    {
        private float constDamage;
        private float magicAddtiveDamge;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            constDamage = param2;
            magicAddtiveDamge = param3;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                HurtEnemy(constDamage + magicAddtiveDamge * MagicValue);
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
