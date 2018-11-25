namespace Assets.Script.Battle
{
    public class BuffIncreaseAvoid : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }
        private float increaseAvoidValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            increaseAvoidValue = (float)param[1] * currentRole.RolePropertyValue.AviodHurtPercent;
            currentRole.RolePropertyValue.SetMaxHp(increaseAvoidValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetMaxHp(-increaseAvoidValue);
        }

        public override bool Update(float deltaTime)
        {
            if (base.Update(deltaTime) == false)
            {
                return false;
            }
            return true;
        }

    }
}
