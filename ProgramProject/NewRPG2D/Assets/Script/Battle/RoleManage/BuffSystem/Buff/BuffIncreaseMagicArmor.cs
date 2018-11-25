namespace Assets.Script.Battle
{
    public class BuffIncreaseMagicArmor : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }
        private float increaseMagicArmorValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            increaseMagicArmorValue = (float)param[1] * currentRole.RolePropertyValue.MagicArmor;
            currentRole.RolePropertyValue.SetMaxHp(increaseMagicArmorValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetMaxHp(-increaseMagicArmorValue);
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
