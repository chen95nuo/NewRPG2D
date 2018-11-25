

namespace Assets.Script.Battle
{
    public class HealFriendHpBase : AlwayTriggerBuff
    {

        private float healHp;
        private float healHpMagic;

        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            healHp = param2;
            healHpMagic = param3;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info))
            {
                RoleBase loseHpRole = FindLoseMaxHpRole();
                loseHpRole.RolePropertyValue.SetHp(-(healHp + MagicValue * healHpMagic));
                return true;
            }
            return false;
        }
    }
}
