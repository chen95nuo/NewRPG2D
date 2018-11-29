namespace Assets.Script.Battle
{
    public class BuffIncreaseCDTime : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Debuff; }
        }
        private float CDTimePercent;
        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            CDTimePercent = (float)param[1] * currentRole.RolePropertyValue.CDTimePercent;
            currentRole.RolePropertyValue.SetCDTimePercent(CDTimePercent);
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.RolePropertyValue.SetCDTimePercent(-CDTimePercent);
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
