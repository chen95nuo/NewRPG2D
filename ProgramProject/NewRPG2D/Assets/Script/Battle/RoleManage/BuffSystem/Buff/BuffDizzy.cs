namespace Assets.Script.Battle
{
    public class BuffDizzy : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Debuff; }
        }

        public override void AddBuff(RoleBase currentRole, BuffTypeEnum mBuffType, params object[] param)
        {
            base.AddBuff(currentRole, mBuffType, param);
            CurrentRole.IsCanControl = false;
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
            CurrentRole.IsCanControl = true;
        }

    }
}
