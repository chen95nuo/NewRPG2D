namespace Assets.Script.Battle
{
    public class BuffReduceArmor : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Debuff; }
        }

        private float armorValue;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            armorValue = (float) param[1] * currentRole.RolePropertyValue.PhysicArmor;
            currentRole.RolePropertyValue.SetPhysicArmor(-armorValue);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetPhysicArmor(armorValue);
        }

        public override bool Update(float deltaTime)
        {
            if(base.Update(deltaTime) == false)
            {
                return false;
            }

            return true;
        }
        
    }
}
