namespace Assets.Script.Battle
{
    public class BuffIncreaseDamage : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }

        private float increasePercent;
        private float increaseDamage;

        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            increasePercent = (float)param[1];
            increaseDamage = currentRole.RolePropertyValue.Damage * increasePercent;
            currentRole.RolePropertyValue.SetDamage(increaseDamage);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetDamage(-increaseDamage);
        }

    }
}
