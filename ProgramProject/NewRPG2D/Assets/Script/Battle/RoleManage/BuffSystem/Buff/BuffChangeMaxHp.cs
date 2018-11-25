namespace Assets.Script.Battle
{
    public class BuffChangeMaxHp : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }
        private float maxHpValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            maxHpValue = (float)param[1] * currentRole.RolePropertyValue.MaxHp;
            currentRole.RolePropertyValue.SetMaxHp(maxHpValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetMaxHp(-maxHpValue);
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
