

namespace Assets.Script.Battle
{
    public class DeadThenHealAnother : TargetDeadTrigger
    {

        private float healHpPercent;
        private bool bTrigger = false;
        public override void Init(RoleBase role, float param1, float param2, float param3, float param4)
        {
            base.Init(role, param1, param2, param3, param4);
            healHpPercent = param1;
        }

        public override bool Trigger(TirggerTypeEnum triggerType, ref HurtInfo info)
        {
            if (base.Trigger(triggerType, ref info) && bTrigger == false)
            {
                RoleBase loseHpRole = FindLoseMaxHpRole();
                loseHpRole.RolePropertyValue.SetHp(-(loseHpRole.RolePropertyValue.MaxHp * healHpPercent));
                bTrigger = true;
                return true;
            }
            return false;
        }
    }
}
