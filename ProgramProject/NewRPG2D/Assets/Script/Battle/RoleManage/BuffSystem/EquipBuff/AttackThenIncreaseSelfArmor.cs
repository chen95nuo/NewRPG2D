
namespace Assets.Script.Battle
{
    public class AttackThenIncreaseSelfArmor : AlwayTriggerBuff
    {
        private float buffDuration;
        private float armorPrecent;


        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            armorPrecent = param2 * 0.01f;
            buffDuration = param3;
        }

        public override bool Trigger(TirggerTypeEnum tirggerType, ref HurtInfo info)
        {
            if (base.Trigger(tirggerType, ref info))
            {
                currentRole.BuffMoment.AddBuff(BuffTypeEnum.ChangeArmor, buffDuration, armorPrecent);
                return true;
            }

            return false;
        }
    }
}
