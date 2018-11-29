namespace Assets.Script.Battle
{
    public class BuffIncreaseEnemyDamage : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Debuff; }
        }
        private float addtiveDamagePercent;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            addtiveDamagePercent = (float)param[1] * currentRole.RoleDamageMoment.AddtiveDamagePercent;
            currentRole.RoleDamageMoment.AddtiveDamagePercent += addtiveDamagePercent;
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RoleDamageMoment.AddtiveDamagePercent = 1;
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
