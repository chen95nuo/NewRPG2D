

namespace Assets.Script.Battle
{

    public class BuffBase
    {
        public virtual BuffEffectTypeEnum BuffType
        {
            get
            {
                return BuffEffectTypeEnum.None;
            }
        }

        protected BuffStateEnum buffState;
        private BuffTypeEnum buffType;
        protected RoleBase CurrentRole;
        protected float buffDuration;

        private float currentTime;
        private float addTime;

        public virtual void AddBuff(RoleBase currentRole, BuffTypeEnum mBuffType, params object[] param)
        {
            buffDuration = (float)param[0];
            currentTime = 0;
            buffType = mBuffType;
            CurrentRole = currentRole;
            buffState = BuffStateEnum.Running;
        }

        public virtual void RmoveBuff()
        {
            buffDuration = -1;
            currentTime = 0;
            buffState = BuffStateEnum.Finish;
        }

        public virtual bool Update(float deltaTime)
        {
            if (buffState == BuffStateEnum.Finish)
            {
                return false;
            }
            currentTime += deltaTime;
            if (currentTime >= buffDuration)
            {
                CurrentRole.BuffMoment.RemoveBuff(buffType);
                buffState = BuffStateEnum.Finish;
            }

            addTime += deltaTime;
            if (addTime > 1)
            {
                addTime = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
