namespace Assets.Script.Battle
{
    public class BuffReduceDamage : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Debuff; }
        }
        private float reducePercent;
        private float reduceDamage;

        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            reducePercent = (float)param[1];
            reduceDamage = currentRole.RolePropertyValue.Damage * reduceDamage;
            currentRole.RolePropertyValue.SetDamage(-reduceDamage);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetDamage(reduceDamage);
        }
        
    }
}
