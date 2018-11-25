namespace Assets.Script.Battle
{
    public class BuffChangeCritial : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }

        private float critialValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            critialValue = (float) param[1] * currentRole.RolePropertyValue.CriticalPercent;
            currentRole.RolePropertyValue.SetCriticalPercent(critialValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetCriticalPercent(-critialValue);
        }
    }
}
