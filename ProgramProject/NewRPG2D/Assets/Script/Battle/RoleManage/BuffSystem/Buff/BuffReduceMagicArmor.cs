namespace Assets.Script.Battle
{
    public class BuffReduceMagicArmor : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Debuff; }
        }
        private float reduceMagicArmorValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            reduceMagicArmorValue = (float)param[1] * currentRole.RolePropertyValue.MagicArmor;
            currentRole.RolePropertyValue.SetMagicArmor(-reduceMagicArmorValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetMagicArmor(reduceMagicArmorValue);
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
