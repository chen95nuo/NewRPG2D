namespace Assets.Script.Battle
{
    public class BuffFreezing : BuffBase
    {
        public override BuffEffectTypeEnum BuffType
        {
            get { return BuffEffectTypeEnum.Buff; }
        }

        private float healHp;

        public override void AddBuff(RoleBase currentRole, BuffTypeEnum buffType, params object[] param)
        {
            base.AddBuff(currentRole, buffType, param);
            healHp = (float)param[1];
            CurrentRole.RolePropertyValue.SetHp(-healHp);
            CurrentRole.SetRoleActionState(ActorStateEnum.Idle);
            CurrentRole.RoleSearchTarget.SetTarget(null);
            CurrentRole.IsCanInterrput = false;
        }

        public override void RmoveBuff()
        {
            base.RmoveBuff();
        }

        public override bool Update(float deltaTime)
        {
            if (base.Update(deltaTime) == false)
            {
                return false;
            }
            CurrentRole.RolePropertyValue.SetHp(-healHp);
            CurrentRole.IsCanInterrput = true;
            return true;
        }

    }
}
